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
            DataSet ds = null;
            string User_Name = "ofer"; //((User)Session["User"]).User_Name;

            if (!Page.IsPostBack)
            {
                DataSet k = Connect.Connect_DataSet("SELECT TOP 1 * FROM Attraction ", "Try"); // הבר חופשה
                OrderMenu.DataSource = k;
                OrderMenu.DataBind();

                //חמש האטרציות אחרונות שהועלו מאז הכניסה האחרונה 
                if (Session["LastCreatedAttractions"] == null)
                {
                    string User_LastEntrance = "03/11/2022"; //((User)Session["User"]).User_LastEntrance;
                    ds = Connect.Connect_DataSet("SELECT TOP 5 * FROM Attraction INNER JOIN AttractionType ON Attraction.Attraction_Type" +
                        " = AttractionType.AttractionType_ID WHERE Attraction_AddDate BETWEEN "
                        + User_LastEntrance + " AND DATE() ORDER BY Attraction_AddDate DESC", "LastCreatedAttractions");
                    Session["LastCreatedAttractions"] = ds;
                }
                else
                    ds = (DataSet)Session["LastCreatedAttractions"];
                NewAttractionsSinceLastEntrance.DataSource = ds;

                ArrayList arr_DSO = new ArrayList();
                arr_DSO.Add(new DS_Object("SELECT * FROM Attraction INNER JOIN OwnedAttraction ON Attraction.Attraction_ID = OwnedAttraction.Attraction_ID  ", "OwnedAttraction"));
                arr_DSO.Add(new DS_Object("SELECT * FROM Attraction INNER JOIN NatureAttraction ON Attraction.Attraction_ID = NatureAttraction.Attraction_ID  ", "NatureAttraction"));
                DataSet innerds = Connect.Connect_MultipleDataSet(arr_DSO);
                Session["FullAttractions"] = innerds;

                NewAttractionsSinceLastEntrance.DataBind();


                // חמש ההזמנות האחרונות של משתמש מחובר - ממויין מחדש לישן
                if (Session["UserOrders"] == null)
                {
                    ds = Connect.Connect_DataSet("SELECT TOP 5 * FROM Orders WHERE Order_UserName = '"
                        + User_Name + "' ORDER BY Order_AddDate DESC", "Orders");
                    Session["UserOrders"] = ds;
                }
                else
                    ds = (DataSet)Session["UserOrders"];
                DataList2.DataSource = ds;
                DataList2.DataBind();


                // חמש אטרקציות מומלצות לפי הזמנות קודמות של משתמש
                if (Session["AttractionTypeByPreviousOrders"] == null)
                {
                    string queryString = @"SELECT DISTINCT [Attraction].[Attraction_Type] 
                       FROM ((([Attraction] 
                       INNER JOIN [Day_Attraction] ON [Attraction].[Attraction_ID] = [Day_Attraction].[Attraction_ID]) 
                       INNER JOIN [Day] ON [Day_Attraction].[Day_ID] = [Day].[Day_ID]) 
                       INNER JOIN [Orders] ON [Day].[Order_ID] = [Orders].[Order_ID]) 
                       WHERE [Orders].[Order_UserName] = '" + User_Name + "';";
                    ds = Connect.Connect_DataSet(queryString, "AttractionTypeByPreviousOrders");

                    ArrayList arr = new ArrayList();
                    foreach (DataRow row in ds.Tables["AttractionTypeByPreviousOrders"].Rows)// ריצה על טבלת סוגי אטרקציות
                    {
                        arr.Add(Convert.ToInt32(row["Attraction_Type"]));
                    }
                    string s = "Select * From Attraction INNER JOIN AttractionType ON Attraction.Attraction_Type = AttractionType.AttractionType_ID  ";
                    bool HaveOrder = false;
                    for (int i = 0; i < arr.Count; i++)
                    {
                        if (!HaveOrder)
                            s += "WHERE Attraction_Type = " + Convert.ToInt32(arr[i]);
                        else
                            s += " OR Attraction_Type = " + Convert.ToInt32(arr[i]);
                    }
                    s += " ORDER BY Attraction_AddDate DESC"; // אם לא הזמין אף פעם יוצגו לו האטרקציות האחרונות שהועלו
                    ds = Connect.Connect_DataSet(s, "Attraction ");
                    Session["AttractionTypeByPreviousOrders"] = ds;
                }
                else
                    ds = (DataSet)Session["AttractionTypeByPreviousOrders"];
                DataList3.DataSource = ds;
                DataList3.DataBind();
            }
        }

        //חמש האטרציות אחרונות שהועלו מאז הכניסה האחרונה 
        // להוסיף דאטאסא שמחזיר את כל האטרקציות ללא שום סינון או מיון
        ////על פי עמודה DataSet מיון - סידור - ORDER BY על DataSet
        //DataView dataView = new DataView(ds.Tables["Attraction"]);
        //dataView.Sort = "Attraction_AddDate" + " DESC";// Specify the column you want to order by. If you want to sort in descending order, use: dataView.Sort = columnName + " DESC";
        //DataTable sortedDataTable = dataView.ToTable();// העברת תוצאת המיון לטבלה חדשה.

        //string User_LastEntrance = "03/11/2022"; //((User)Session["User"]).User_LastEntrance;//2023-11-02
        ////לפי תנאי DataSet  סינון ה
        //DataTable top5; //שיוצג יכיל 5 איברים בלבד DataSet  ה
        //try 
        //{
        //    // הסינון לפי תנאי
        //    sortedDataTable = sortedDataTable.Select("Attraction_AddDate >= #" + User_LastEntrance +
        //        "# AND Attraction_AddDate <= #" + DateTime.Now.ToString("MM/dd/yyyy") + "#").CopyToDataTable(); // בין תאריך הכניסה האחרון של המשתמש לתאריך היום
        //    top5 = sortedDataTable.Clone(); //בטבלה החדשה Attraction השמת העמודות מטבלת  
        //}
        //catch(InvalidOperationException e1)// מקרה בו אין אטרקציות חדשות בטווח הזמן ואז הסינון מחזיר שגיאה
        //{
        //    top5 = null; //שיוצג יהיה ריק - פתרון לסינון שמחזיר שגיאה DataSet  ה
        //}
        //if (top5 != null && sortedDataTable.Rows.Count <= 5) // יש אטרקציות לאחר הסינון=  Catch הממוין והמסונן מכיל פחות מ5 איברים - ולא ריק - לא נכנס ל DataSet אם ה 
        //{
        //    top5 = sortedDataTable;
        //}
        //else if (sortedDataTable.Rows.Count > 5) // אם מכיל יותר מ5 איברים - לוקח רק 5
        //{
        //    for (int i = 1; i <= 5; i++)
        //    {
        //        // Create a new row for 'top5' DataTable and copy the data
        //        DataRow row = top5.NewRow();
        //        row.ItemArray = sortedDataTable.Rows[i].ItemArray; // Copy data from 'row' to 'newRow'
        //        top5.Rows.Add(row);
        //    }
        //}

        //DataList1.DataSource = top5;
        //DataList1.DataBind();

        // למחוק
        //protected void DataList1_ItemDataBound(object sender, DataListItemEventArgs e)
        //{
        //    Label date = (Label)e.Item.FindControl("Label2");
        //    date.Text = date.Text.Remove(date.Text.IndexOf(' '));
        //}

        protected void DataList2_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            Label days = (Label)e.Item.FindControl("Label2");
            days.Text += " ימים";
        }
        protected void OrderMenu_ItemCommand(object source, DataListCommandEventArgs e) // העברת נתוני ההזמנה לעמוד הזמנה
        {
            //Move to another page to show the product
            if (e.CommandName == "create")
            {
                ArrayList orderarr = new ArrayList();
                orderarr.Add(((TextBox)e.Item.FindControl("FromDate")).Text); //מתאריך
                orderarr.Add(((TextBox)e.Item.FindControl("ToDate")).Text); // לתאריך
                orderarr.Add(MinAgeTextBox.Text); // גיל מינימלי
                orderarr.Add(MaxAgeTextBox.Text);// גיל מקסימלי
                Session["orderarr"] = orderarr;
            }
            if (e.CommandName == "DoShow")
            {
                Age.Style["display"] = "block";
            }
        }

        protected void Button10_Click(object sender, EventArgs e)
        {
            Age.Style["display"] = "none";
            MinAgeTextBox.Text = MinAgeTextBox.Text;
            MaxAgeTextBox.Text = MaxAgeTextBox.Text;
        }


        protected void DataList1_ItemCommand1(object source, DataListCommandEventArgs e)
        {
            //Move to another page to show the product
            if (e.CommandName == "DoShow")
            {
                Session["AttractionID_ForAttractionPage"] = Convert.ToInt32(((Label)e.Item.FindControl("Attraction_ID")).Text);
            }
            Session["from"] = "SmartPage.aspx";
            Response.Redirect("Attraction_Folder/Attraction_Page.aspx");
        }

        protected void DataList2_ItemCommand(object source, DataListCommandEventArgs e)
        {
            //מעבר לעמוד הזמנה
        }

        protected void DataList3_ItemCommand(object source, DataListCommandEventArgs e)
        {
            //Move to another page to show the product
            if (e.CommandName == "DoShow")
            {
                Session["AttractionID_ForAttractionPage"] = Convert.ToInt32(((Label)e.Item.FindControl("Attraction_ID")).Text);
            }
            Session["from"] = "Project_01/Homepage.aspx";
            Response.Redirect("Attraction_Folder/Attraction_Page.aspx");
        }

        protected void DataList1_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int IdNum = Convert.ToInt32(((Label)e.Item.FindControl("Attraction_ID")).Text);
                int TypeNum = Convert.ToInt32(((Label)e.Item.FindControl("Attraction_VacationType_ID")).Text);
                if (TypeNum != 3)
                {
                    foreach (DataRow row in ((DataSet)Session["FullAttractions"]).Tables["OwnedAttraction"].Rows)
                    {
                        if ((int)row["Attraction.Attraction_ID"] == IdNum)
                        {
                            if (row["OwnedAttraction_Price"].ToString() == "0")
                            {
                                ((Label)e.Item.FindControl("PriceOrKilometers")).Text = "חינם";
                            }
                            else
                            {
                                ((Label)e.Item.FindControl("PriceOrKilometers")).Text = row["OwnedAttraction_Price"].ToString() + " שח";
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

        protected void DataList3_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int IdNum = Convert.ToInt32(((Label)e.Item.FindControl("Attraction_ID")).Text);
                int TypeNum = Convert.ToInt32(((Label)e.Item.FindControl("Attraction_VacationType_ID")).Text);
                if (TypeNum != 3)
                {
                    foreach (DataRow row in ((DataSet)Session["FullAttractions"]).Tables["OwnedAttraction"].Rows)
                    {
                        if ((int)row["Attraction.Attraction_ID"] == IdNum)
                        {
                            if (row["OwnedAttraction_Price"].ToString() == "0")
                            {
                                ((Label)e.Item.FindControl("PriceOrKilometers")).Text = "חינם";
                            }
                            else
                            {
                                ((Label)e.Item.FindControl("PriceOrKilometers")).Text = row["OwnedAttraction_Price"].ToString() + " שח";
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



        protected void Button2_Click(object sender, EventArgs e)
        {
            Session["from"] = "SmartPage.aspx";
            Response.Redirect("Attraction_Display.aspx");
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            Session["from"] = "SmartPage.aspx";
            Response.Redirect("AllUserOrders.aspx");
        }

        protected void Button4_Click(object sender, EventArgs e)
        {
            Session["from"] = "SmartPage.aspx";
            Response.Redirect("Attraction_Display.aspx");
        }
    }
}