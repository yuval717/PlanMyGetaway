using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Collections;

namespace Project_01
{
    public partial class Homepage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DataSet ds = null; //יצירת  דאטאסט ריק לצרכים שונים
            string User_Name = "ofer"; //((User)Session["User"]).User_Name;

            if (!Page.IsPostBack)
            {
                //*** יאוכלס בשסן בעמוד הראשון - פייק דאטא סט - לוצרכי אכלוס עיצובי
                DataSet dataSet = new DataSet();// Create a new DataSet
                DataTable table = new DataTable("FakeDataset");// Create a new DataTable
                table.Columns.Add("RowNumber", typeof(int));// Define the columns in the DataTable
                dataSet.Tables.Add(table);// Add the DataTable to the DataSet // דאטא טייבל רק - יאוכלס לפי צורך
                Session["FakeDataset"] = dataSet;// הכנסה לסשן - לא בעמוד הזה - בעמוד הזה שימוש בסשן
                //***

                // Populate the DataTable with 1 rows - בר החופשה 
                DataTable t = ((DataSet)Session["FakeDataset"]).Tables["FakeDataset"]; // לקיחת דאטא סט מזוייף
                DataRow rows = t.NewRow();// Create a new DataRow
                rows["RowNumber"] = 1;// Set the value of the 'RowNumber' column to the current iteration
                t.Rows.Add(rows);// Add the DataRow to the DataTable

                OrderMenu.DataSource = t;// בר החופשה
                OrderMenu.DataBind();

                //***ארבע האטרציות אחרונות שהועלו מאז הכניסה האחרונה 
                if (Session["LastCreatedAttractions"] == null)
                {
                    string User_LastEntrance = "03/11/2022"; //((User)Session["User"]).User_LastEntrance;
                    ds = Connect.Connect_DataSet("SELECT TOP 4 * FROM Attraction INNER JOIN AttractionType ON Attraction.Attraction_Type" +
                        " = AttractionType.AttractionType_ID WHERE Attraction_AddDate BETWEEN "
                        + User_LastEntrance + " AND DATE() ORDER BY Attraction_AddDate DESC", "LastCreatedAttractions");
                    Session["LastCreatedAttractions"] = ds;
                }
                else
                    ds = (DataSet)Session["LastCreatedAttractions"];
                NewAttractionsSinceLastEntrance.DataSource = ds;// ארקציות אחרונות שהעולו מאז הכניסה האחרונה

                //יצירת דאטא סט אטרקציות והרחבה אטרקציות טבע 
                ArrayList arr_DSO = new ArrayList();
                arr_DSO.Add(new DS_Object("SELECT * FROM Attraction", "Attraction"));
                arr_DSO.Add(new DS_Object("SELECT * FROM Attraction INNER JOIN NatureAttraction ON Attraction.Attraction_ID = NatureAttraction.Attraction_ID  ", "NatureAttraction"));
                Session["FullAttractions"] = Connect.Connect_MultipleDataSet(arr_DSO);

                NewAttractionsSinceLastEntrance.DataBind(); // עושה בינד אחרי יצירת דאטאסט אטרקציות בו משתמש

                //*** ארבע ההזמנות האחרונות של משתמש מחובר - ממויין מחדש לישן
                if (Session["UserOrders"] == null)
                {
                    ds = Connect.Connect_DataSet("SELECT TOP 5  *  FROM Orders WHERE Order_UserName = '"
                        + User_Name + "'  ORDER BY Order_AddDate DESC", "Orders");
                    Session["UserOrders"] = ds;
                }
                else
                    ds = (DataSet)Session["UserOrders"];
                DataList2.DataSource = ds;//ההזמנות האחרונות של משתמש מחובר
                DataList2.DataBind();


                //*** ארבע אטרקציות מומלצות לפי הזמנות קודמות של משתמש - ממויון מחדש לישן
                if (Session["AttractionTypeByPreviousOrders"] == null)
                {
                    //מציאת סוגי האטרקציות לפי ההזמנות האחרונות
                    string queryString = @"SELECT DISTINCT [Attraction].[Attraction_Type] 
                       FROM ((([Attraction] 
                       INNER JOIN [Day_Attraction] ON [Attraction].[Attraction_ID] = [Day_Attraction].[Attraction_ID]) 
                       INNER JOIN [Days] ON [Day_Attraction].[Day_ID] = [Days].[Day_ID]) 
                       INNER JOIN [Orders] ON [Days].[Order_ID] = [Orders].[Order_ID]) 
                       WHERE [Orders].[Order_UserName] = '" + User_Name + "';";
                    ds = Connect.Connect_DataSet(queryString, "AttractionTypeByPreviousOrders");

                    ArrayList arr = new ArrayList();
                    foreach (DataRow row in ds.Tables["AttractionTypeByPreviousOrders"].Rows)// ריצה על טבלת סוגי אטרקציות
                    {
                        arr.Add(Convert.ToInt32(row["Attraction_Type"]));
                    }
                    //בסיס שאילתה אטרקציות מומלצות לפי הזמנות קודמות של משתמש - לפני הוספת האטרקציות לפי המערך 
                    string s = "Select * From Attraction INNER JOIN AttractionType ON Attraction.Attraction_Type = AttractionType.AttractionType_ID  ";
                    bool HaveOrder = false; // משתנה בדיקה לאטרקציה הראשונה
                    for (int i = 0; i < arr.Count; i++)
                    {
                        if (!HaveOrder) //where בודק אם הוסיף 
                        {
                            s += "WHERE Attraction_Type = " + Convert.ToInt32(arr[i]);
                            HaveOrder = true;
                        }
                        else
                            s += " OR Attraction_Type = " + Convert.ToInt32(arr[i]);
                    }
                    s += " ORDER BY Attraction_AddDate DESC"; // אם לא הזמין אף פעם יוצגו לו האטרקציות האחרונות שהועלו
                    ds = Connect.Connect_DataSet(s, "Attraction ");
                    Session["AttractionTypeByPreviousOrders"] = ds;
                }
                else
                    ds = (DataSet)Session["AttractionTypeByPreviousOrders"];
                DataList3.DataSource = ds; // אטרקציות מומלצות לפי הזמנות קודמות
                DataList3.DataBind();
            }
        }


        //דאטא באונד לחופשות
        protected void DataList2_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            Label days = (Label)e.Item.FindControl("Label2");
            days.Text += " ימים";
        }

        //כפתור יצירת הזמנה/גילאים
        protected void OrderMenu_ItemCommand(object source, DataListCommandEventArgs e) // העברת נתוני ההזמנה לעמוד הזמנה
        {
            //Move to another page to show the product
            if (e.CommandName == "create")
            {
                //שומר את הנתונים בסשן
                ArrayList orderarr = new ArrayList();
                orderarr.Add(((TextBox)e.Item.FindControl("FromDate")).Text); //מתאריך
                orderarr.Add(((TextBox)e.Item.FindControl("ToDate")).Text); // לתאריך
                orderarr.Add(MinAgeTextBox.Text); // גיל מינימלי
                orderarr.Add(MaxAgeTextBox.Text);// גיל מקסימלי
                Session["orderarr"] = orderarr;
                //Move to another page 
                Session["from"] = "Homepage.aspx"; // שמירת העמוד הנוכחי
                Response.Redirect("Order_Folder/Order.aspx"); // העברה לדף אטרקציה

            }
            if (e.CommandName == "DoShow")
            {
                // מציג חלון גילאים
                Age.Style["display"] = "block"; 
            }
        }

        // כפתור "אישור" חלון גילאים
        protected void Button10_Click(object sender, EventArgs e)
        {
            Age.Style["display"] = "none";
            MinAgeTextBox.Text = MinAgeTextBox.Text;
            MaxAgeTextBox.Text = MaxAgeTextBox.Text;
        }

        //העברה לדף ארטציה
        protected void DataList1_ItemCommand1(object source, DataListCommandEventArgs e)
        {
            //Move to another page to show the product
            if (e.CommandName == "DoShow")
            {
                //שמירת קוד האטרציה לדף אליה רוצים לעבור
                Session["AttractionID_ForAttractionPage"] = Convert.ToInt32(((Label)e.Item.FindControl("Attraction_ID")).Text);
            }
            Session["from"] = "SmartPage.aspx"; // שמירת העמוד הנוכחי
            Response.Redirect("Attraction_Folder/Attraction_Page.aspx"); // העברה לדף אטרקציה
        }

        //העברה לדף חופשה
        protected void DataList2_ItemCommand(object source, DataListCommandEventArgs e)
        {
            //מעבר לעמוד הזמנה
        }


        //דאטא באונד אטרקציה
        protected void DataList1_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int IdNum = Convert.ToInt32(((Label)e.Item.FindControl("Attraction_ID")).Text);
                int TypeNum = Convert.ToInt32(((Label)e.Item.FindControl("Attraction_VacationType_ID")).Text);
                if (TypeNum != 3)
                {
                    foreach (DataRow row in ((DataSet)Session["FullAttractions"]).Tables["Attraction"].Rows)
                    {
                        if ((int)row["Attraction_ID"] == IdNum)
                        {
                            if (row["Attraction_Price"].ToString() == "0")
                            {
                                ((Label)e.Item.FindControl("PriceOrKilometers")).Text = "חינם";
                            }
                            else
                            {
                                ((Label)e.Item.FindControl("PriceOrKilometers")).Text = row["Attraction_Price"].ToString() + " שח";
                            }
                            break;
                        }
                    }
                }
                else
                {
                    foreach (DataRow row in ((DataSet)Session["FullAttractions"]).Tables["NatureAttraction"].Rows)
                    {
                        if ((int)row["Attraction.Attraction_ID"] == IdNum)
                        {
                            ((Label)e.Item.FindControl("PriceOrKilometers")).Text = row["NatureAttraction_KilometersNumber"].ToString() + " קילומטר";
                            break;
                        }
                    }
                }
            }
        }

    }
}