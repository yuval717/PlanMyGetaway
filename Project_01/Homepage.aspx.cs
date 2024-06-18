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
using System.Text.RegularExpressions;

namespace Project_01
{
    public partial class Homepage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DataSet ds = null; //יצירת  דאטאסט ריק לצרכים שונים
            string User_Name;


            if (!Page.IsPostBack)
            {
                

                //בר הזמנה
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

                if (Session["User"] != null) // אם משתמש רשום
                {
                    MasterPage_UserName.Text = ((User)Session["User"]).User_Name;
                    //מאסטר פייג
                    Site master = (Site)this.Master;
                    master.MasterPageSignUpOut.Text = "התנתק";
                    master.MasterPageOrders.Visible = true;
                    master.MasterPageOrders.CommandName = "User_Folder/UsersOrders";
                    master.MasterPageSignUpOut.CommandName = "Homepage"; //התנתקות
                    master.MasterPageNewOrder.CommandName = "/Attraction_Folder/Attractions_Display";
                    master.MasterPageAbout.CommandName = "/About";

                    User_Name = ((User)Session["User"]).User_Name; //שם משתמש

                    //***ארבע האטרציות אחרונות שהועלו מאז הכניסה האחרונה 
                    if (Session["LastCreatedAttractions"] == null)
                    {
                        string User_LastEntrance = ((User)Session["User"]).User_LastEntrance; //((User)Session["User"]).User_LastEntrance;
                        string k = "SELECT TOP 4 * FROM Attraction INNER JOIN AttractionType ON Attraction.Attraction_Type = AttractionType.AttractionType_ID " +
                   "WHERE Attraction.Attraction_ID <> 67 AND Attraction.Attraction_AddDate BETWEEN DATE() AND #" + User_LastEntrance +
                   "# ORDER BY Attraction.Attraction_AddDate DESC";
                        ds = Connect.Connect_DataSet(k, "LastCreatedAttractions");

                        //אם אין אטרקציות שהעולו מאז כניסתו הארחונה של המשתמש
                        if (ds.Tables["LastCreatedAttractions"].Rows.Count == 0) // אם אין אטרקציות שהוספו מאז כניסתו הארחונה של המשתמש - יציג סתם אטרקציות
                        {
                            ds = Connect.Connect_DataSet("SELECT TOP 4 * FROM Attraction INNER JOIN AttractionType ON Attraction.Attraction_Type" +
                            " = AttractionType.AttractionType_ID ORDER BY Attraction_AddDate DESC", "LastCreatedAttractions");

                            Label3.Text = "אטרקציות";
                        }
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
                    Session["UserOrders"] = null;//עדכון בכל פעם שנכנס לתצוגה
                    if (Session["UserOrders"] == null)
                    {
                        ds = Connect.Connect_DataSet("SELECT TOP 4 * FROM Orders WHERE NotValid = " +false+" AND Order_UserName = '"
                            + User_Name + "'  ORDER BY Order_AddDate DESC", "Orders");
                        Session["UserOrders"] = ds;
                    }
                    else
                        ds = (DataSet)Session["UserOrders"];
                    try
                    {
                        DataList2.DataSource = ds.Tables["Orders"].AsEnumerable().Take(4).CopyToDataTable();//ההזמנות האחרונות של משתמש מחובר
                    }
                    catch (Exception)
                    {
                        DataList2.DataSource = null;
                        Label4.Visible = false;
                    }

                    DataList2.DataBind();

                    Session["AttractionTypeByPreviousOrders"] = null; //עדכון בכל פעם שנכנס לתצוגה
                    //*** ארבע אטרקציות מומלצות לפי הזמנות קודמות של משתמש - ממויון מחדש לישן
                    if (Session["AttractionTypeByPreviousOrders"] == null)
                    {
                        //מציאת סוגי האטרקציות לפי ההזמנות האחרונות
                        string queryString = @"SELECT DISTINCT [Attraction].[Attraction_Type] FROM ((([Attraction] 
                            INNER JOIN [Day_Attraction] ON [Attraction].[Attraction_ID] = [Day_Attraction].[Attraction_ID]) 
                            INNER JOIN [Days] ON [Day_Attraction].[Day_ID] = [Days].[Day_ID]) 
                            INNER JOIN [Orders] ON [Days].[Order_ID] = [Orders].[Order_ID]) 
                            WHERE [Orders].[NotValid] = " + false+" AND [Orders].[Order_UserName] = '" + User_Name + "' AND [Attraction].[Attraction_ID] <> 67;";
                        ds = Connect.Connect_DataSet(queryString, "AttractionTypeByPreviousOrders");

                        ArrayList arr = new ArrayList();
                        foreach (DataRow row in ds.Tables["AttractionTypeByPreviousOrders"].Rows)// ריצה על טבלת סוגי אטרקציות
                        {
                            arr.Add(Convert.ToInt32(row["Attraction_Type"]));
                        }
                        if (arr.Count != 0)
                        {
                            //בסיס שאילתה אטרקציות מומלצות לפי הזמנות קודמות של משתמש - לפני הוספת האטרקציות לפי המערך 
                            string s = "Select * From Attraction INNER JOIN AttractionType ON Attraction.Attraction_Type = AttractionType.AttractionType_ID ";
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
                            ds = Connect.Connect_DataSet(s, "Attractions");
                            Session["AttractionTypeByPreviousOrders"] = ds;
                        }
                        else
                        {
                            ds = Connect.Connect_DataSet("Select * From Attraction INNER JOIN AttractionType ON Attraction.Attraction_Type = AttractionType.AttractionType_ID WHere Attraction_ID = " + -1, "Attractions"); // דאטאסט פייק - לא יחזיר כלום - נמטרתו למנוע קריסה בדאדטא סורס
                            Session["AttractionTypeByPreviousOrders"] = ds;
                        }

                    }
                    else
                        ds = (DataSet)Session["AttractionTypeByPreviousOrders"];

                    try
                    {
                        DataList3.DataSource = ds.Tables["Attractions"].AsEnumerable().Take(4).CopyToDataTable();// אטרקציות מומלצות לפי הזמנות קודמות
                    }
                    catch (Exception)
                    {

                        DataList3.DataSource = null;
                        Label5.Visible = false;
                    }

                    DataList3.DataBind();
                }

                else // אם משתמש אורח
                {
                    //מאסטר פייג
                    Site master = (Site)this.Master;
                    master.MasterPageSignUpOut.Text = "התחבר";
                    master.MasterPageOrders.Visible = false;
                    master.MasterPageSignUpOut.CommandName = "User_Folder/User_Login_Register";
                    master.MasterPageNewOrder.CommandName = "/Attraction_Folder/Attractions_Display";
                    master.MasterPageAbout.CommandName = "/About";

                    //***ארבע האטרציות  
                    if (Session["LastCreatedAttractions"] == null)
                    {
                       
                        ds = Connect.Connect_DataSet("SELECT TOP 4 * FROM Attraction INNER JOIN AttractionType ON Attraction.Attraction_Type" +
                            " = AttractionType.AttractionType_ID ORDER BY Attraction_AddDate DESC", "LastCreatedAttractions");

                        Label3.Text = "אטרקציות";
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

                    //העלמת פרטים מהתצוגה
                    Label3.Text = "אטרקציות";
                    Label4.Visible = false;
                    Label5.Visible = false;
                    User_Edit.Visible = false;
                }
            }
                
        }

        private string ValidateDates(DataListItem item)
        {
            TextBox fromDateTextBox = (TextBox)item.FindControl("FromDate");
            TextBox toDateTextBox = (TextBox)item.FindControl("ToDate");

            string fromDate = fromDateTextBox.Text;
            string toDate = toDateTextBox.Text;

            if (!DateTime.TryParse(fromDate, out DateTime parsedFromDate))
            {
                return "שגיאה";
            }

            if (!DateTime.TryParse(toDate, out DateTime parsedToDate))
            {
                return "שגיאה";
            }

            if (parsedFromDate.Date < DateTime.Now.Date)
            {
                return "שגיאה";
            }

            if (parsedToDate.Date < DateTime.Now.Date)
            {
                return "שגיאה";
            }

            if (parsedToDate.Date < parsedFromDate.Date)
            {
                return "שגיאה";
            }

            return "הצלחה";
        }


        private string ValidateAge(string minAge, string maxAge)
        {
            string pattern = @"^\d{1,10}$";
            if (string.IsNullOrWhiteSpace(minAge) || !Regex.IsMatch(minAge, pattern))
            {
                // Handle invalid minimum age
                return "שגיאה";
            }
            else if (string.IsNullOrWhiteSpace(maxAge) || !Regex.IsMatch(maxAge, pattern))
            {
                // Handle invalid maximum age
                return "שגיאה";
            }
            else
            {
                int min = int.Parse(minAge);
                int max = int.Parse(maxAge);
                if (min >= max)
                {
                    // Handle the case where minimum age is not less than maximum age
                    return "שגיאה";
                }
            }
            return "הצלחה";
        }


        //דאטא באונד לחופשות
        protected void DataList2_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            Label days = (Label)e.Item.FindControl("Label2");
            days.Text += " ימים";

            if (DateTime.Parse((((Label)e.Item.FindControl("Datesituation")).Text).Substring(0, 10)) < DateTime.Now)
            {
                ((Label)e.Item.FindControl("Datesituation")).Text = "התקיימה";
            }
            else
                ((Label)e.Item.FindControl("Datesituation")).Text = "עתידה להתקיים";
        }

        //כפתור יצירת הזמנה/גילאים
        protected void OrderMenu_ItemCommand(object source, DataListCommandEventArgs e) // העברת נתוני ההזמנה לעמוד הזמנה
        {
            //Move to another page to show the product
            if (e.CommandName == "create")
            {
                bool NoTextError = true; // Validation errors
                // Validate OrderMenu DataList items
                foreach (DataListItem item in OrderMenu.Items)
                {
                    if (NoTextError && ValidateDates(item) == "שגיאה")
                    {
                        result_Order.Text = "הכנס תאריכים תקינים";
                        NoTextError = false;
                    }
                }
                if (NoTextError && ValidateAge(MinAgeTextBox.Text, MaxAgeTextBox.Text) == "שגיאה")
                {
                    result_Order.Text = "הכנס טווח גילאים תקין";
                    NoTextError = false;
                }
                if (NoTextError && ValidateAge(MinAgeTextBox.Text, MaxAgeTextBox.Text) == "שגיאה")
                {
                    result_Order.Text = "הכנס טווח גילאים תקין";
                    NoTextError = false;
                }
                //אם וולידיישן תקין מבצע את הפעולה
                if (NoTextError)
                {

                    if (Session["User"] != null) // אם משתמש רשום
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
                        Response.Redirect("Order_Folder/Automatic_Order.aspx"); // העברה לדף יצירת הזמנה
                    }
                    else
                    {
                        Session["from"] = "Homepage.aspx"; // שמירת העמוד הנוכחי
                        Response.Redirect("User_Folder/User_Login_Register.aspx"); // העברה לדף התחברות
                    }
                }                      

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
                Session["AttractionID_ForAttractionPage"] = ((Label)e.Item.FindControl("Attraction_ID")).Text;
                Session["from"] = "/HomePage.aspx";
                Response.Redirect("Attraction_Folder/Attraction_Page.aspx");
            }
        }

        //העברה לדף חופשה
        protected void DataList2_ItemCommand(object source, DataListCommandEventArgs e)
        {
            //Move to another page to show the product
            if (e.CommandName == "DoShow")
            {
                Session["OrderCode_ForPathDIs"] = ((Label)e.Item.FindControl("Order_ID")).Text;
                Session["from"] = "/HomePage.aspx";
                if (((Label)e.Item.FindControl("OrderType")).Text == "ידני") // מציאת סוג האטרקציה - לפיה יעבור לעמוד תצוגה
                    Response.Redirect("Order_Folder/Manual_Order.aspx");
                else
                    Response.Redirect("Order_Folder/AutomaticPathDisplay.aspx");
            }
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

        protected void User_Edit_Click(object sender, EventArgs e)
        {
            Response.Redirect("/User_Folder/User_Edit.aspx"); // העברה לדף בעל עסק
        }

    }
}