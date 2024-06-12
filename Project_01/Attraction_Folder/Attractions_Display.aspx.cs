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
using System.Globalization;
using System.Text.RegularExpressions;

namespace Project_01
{
    public partial class Attractions_Display : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["User"] != null) // אם משתמש רשום
                {

                    if (((User)Session["User"]).User_Type == "בעל עסק")//בעל עסק
                    {
                        //מאסטר פייג
                        Site master = (Site)this.Master;
                        master.MasterPageSignUpOut.Text = "התנתק";
                        master.MasterPageOrders.Visible = false;
                        master.MasterPageSignUpOut.CommandName = "/Attraction_Folder/Attraction_Owner"; //התנתקות
                        master.MasterPageNewOrder.CommandName = "/Attraction_Folder/Attractions_Display";
                        master.MasterPageAbout.CommandName = "/About";
                        master.MasterPageLogo.CommandName = "/Attraction_Folder/Attraction_Owner";
                    }
                    else if (((User)Session["User"]).User_Type == "אדמין")//אדמין
                    {
                        //מאסטר פייג
                        Site master = (Site)this.Master;
                        master.MasterPageSignUpOut.Text = "התנתק";
                        master.MasterPageOrders.Visible = false;
                        master.MasterPageSignUpOut.CommandName = "/Homepage"; //התנתקות
                        master.MasterPageNewOrder.CommandName = "/Attraction_Folder/AttractionDisplay_Admin";
                        master.MasterPageAbout.CommandName = "/About";
                        master.MasterPageLogo.CommandName = "/admin";
                    }
                    else//משתמש רשום
                    {
                        //מאסטר פייג
                        Site master = (Site)this.Master;
                        master.MasterPageSignUpOut.Text = "התנתק";
                        master.MasterPageOrders.Visible = true;
                        master.MasterPageOrders.CommandName = "/User_Folder/UsersOrders";
                        master.MasterPageSignUpOut.CommandName = "/Homepage"; //התנתקות
                        master.MasterPageNewOrder.CommandName = "/Attraction_Folder/Attractions_Display";
                        master.MasterPageAbout.CommandName = "/About";
                        master.MasterPageLogo.CommandName = "/Homepage";
                    }
                }
                else
                {
                    //מאסטר פייג
                    Site master = (Site)this.Master;
                    master.MasterPageSignUpOut.Text = "התחבר";
                    master.MasterPageOrders.Visible = false;
                    master.MasterPageSignUpOut.CommandName = "/User_Folder/User_Login_Register";
                    master.MasterPageNewOrder.CommandName = "/Attraction_Folder/Attractions_Display";
                    master.MasterPageAbout.CommandName = "/About";
                    master.MasterPageLogo.CommandName = "/Homepage";
                }

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

                FilterMenu.DataSource = t;// בר החופשה
                FilterMenu.DataBind();

                DataSet ds;
                if (Session["Attraction"] == null)
                {
                    ds = Connect.Connect_DataSet("SELECT * FROM Attraction INNER JOIN AttractionType ON Attraction.Attraction_Type" +
                        " = AttractionType.AttractionType_ID WHERE Attraction_Valid = " + true, "Attraction"); //טבלת סוגי אטרקציות inner join - טבלת אטרקציות ללא אטרקציה 67
                    ds.Tables.Add(Connect.Connect_DataTable("SELECT * FROM Attraction INNER JOIN NatureAttraction ON Attraction.Attraction_ID = NatureAttraction.Attraction_ID WHERE Attraction_Valid = " + true, "NatureAttraction"));//טבלת אטרקציות טבע inner join - טבלת אטרקציות 

                    foreach (DataRow row in ds.Tables["Attraction"].Rows)//
                    {
                        if (row.RowState != DataRowState.Deleted) // אם השורה לא נמחקה = שובצה במסלול
                        {
                            // Calculate the difference in minutes
                            row["Attraction_ClosingHour"] = (DateTime.ParseExact(((row["Attraction_ClosingHour"]).ToString()).Substring(11), "HH:mm:ss", CultureInfo.InvariantCulture)); // ההפרש בדקות מזמן סגירת האטרקציה לשעה הנוכחית
                            row["Attraction_OpeningHour"] = (DateTime.ParseExact(((row["Attraction_OpeningHour"]).ToString()).Substring(11), "HH:mm:ss", CultureInfo.InvariantCulture));// עדכון שעת הפתיחה של ארטקציה - עדכון תאריך - שעה זהה- עושה בעיות אם לא התאריך הנוכחי
                        }
                    }

                    Session["Attraction"] = ds;

                }
                else
                {
                    ds = (DataSet)Session["Attraction"];
                }

                AttractaionDisplay.DataSource = ds.Tables["Attraction"];
                AttractaionDisplay.DataBind();

                ArrayList AttractionTypeArr = Connect.FillArrayListForDropDownList("SELECT AttractionType_ID, AttractionType_Type FROM AttractionType WHERE IsValid = "+ true, "AttractionType_Type", "AttractionType_ID");
                for (int i = 0; i < AttractionTypeArr.Count; i++)
                {
                    AttractionType.Items.Add((ListItem)AttractionTypeArr[i]);
                }

                //שינוי צבע הכפתור - "הכל" לנבחר
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
                    foreach (DataRow row in ((DataSet)Session["Attraction"]).Tables["Attraction"].Rows)
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
                    foreach (DataRow row in ((DataSet)Session["Attraction"]).Tables["NatureAttraction"].Rows)
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

        private string ValidateAge(string minAge, string maxAge)
        {
            string pattern = @"^\d{1,10}$";

            if (!string.IsNullOrWhiteSpace(minAge) && !Regex.IsMatch(minAge, pattern))
            {
                // Handle invalid minimum age
                return "שגיאה";
            }
            if (!string.IsNullOrWhiteSpace(maxAge) && !Regex.IsMatch(maxAge, pattern))
            {
                // Handle invalid maximum age
                return "שגיאה";
            }

            if (!string.IsNullOrWhiteSpace(minAge) && !string.IsNullOrWhiteSpace(maxAge))
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

        private string ValidateTime(string time, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(time) || !DateTime.TryParse(time, out DateTime parsedTime))
            {
                // Handle invalid time format
                return "שגיאה";
            }

            TimeSpan timeOfDay = parsedTime.TimeOfDay;

            if (timeOfDay < new TimeSpan(0, 1, 0) || timeOfDay > new TimeSpan(23, 59, 0))
            {
                // Handle time out of range
                return "שגיאה";
            }

            return "הצלחה";
        }

        private string ValidateOpeningAndClosingHours(string openingHour, string closingHour)
        {
            string openingResult = ValidateTime(openingHour, "שעת פתיחה");
            string closingResult = ValidateTime(closingHour, "שעת סגירה");

            if (openingResult != "הצלחה")
            {
                return "שגיאה";
            }

            if (closingResult != "הצלחה")
            {
                return "שגיאה";
            }

            if (openingResult == "הצלחה" && closingResult == "הצלחה")
            {
                TimeSpan openingTime = DateTime.Parse(openingHour).TimeOfDay;
                TimeSpan closingTime = DateTime.Parse(closingHour).TimeOfDay;

                if (closingTime <= openingTime)
                {
                    return "שגיאה";
                }
            }
            return "הצלחה";
        }

        private string ValidatePrice(string price)
        {
            string pattern = @"^\d{1,10}$";
            if (string.IsNullOrWhiteSpace(price) || !Regex.IsMatch(price, pattern))
            {
                // Handle invalid price
                return "שגיאה";
            }
            return "הצלחה";
        }

        private string ValidateDuration(string duration)
        {
            string pattern = @"^\d{1,10}$";
            if (string.IsNullOrWhiteSpace(duration) || !Regex.IsMatch(duration, pattern))
            {
                // Handle invalid duration
                return "שגיאה";
            }
            return "הצלחה";
        }



        //סינון
        protected void FilterMenu_ItemCommand(object source, DataListCommandEventArgs e) // העברת נתוני ההזמנה לעמוד הזמנה
        {
            NoResult_Lable.Visible = false; // כותרת לא נמצאו תוצאות - נעלמת
            result_Order.Visible = false;

            //Move to another page to show the product
            if (e.CommandName == "All")
            {
                Min_Age.Text = "";
                Max_Age.Text = "";
                ((TextBox)e.Item.FindControl("Price")).Text = "";
                ((TextBox)e.Item.FindControl("Duration")).Text = "";
                AttractionType.SelectedIndex = 0;
                AttractaionDisplay.DataSource = ((DataSet)Session["Attraction"]).Tables["Attraction"];
                AttractaionDisplay.DataBind();
                AttractionType.SelectedIndex = -1;
                AttractionType.Items[0].Selected = false;
                AttractionFilter.Style["display"] = "none";
                Hours.Style["display"] = "none";
                Age.Style["display"] = "none";

            }
            if (e.CommandName == "Age")
            {
                Age.Style["display"] = "block";
            }
            if (e.CommandName == "Hours")
            {
                Hours.Style["display"] = "block";
            }
            if (e.CommandName == "Type")
            {
                AttractionFilter.Style["display"] = "block";

            }

            if (e.CommandName == "Filter")
            {
                
                //וולידיישן
                bool NoTextError = true;//שגיאות וולידישן
                if (NoTextError && ValidateAge(Min_Age.Text, Max_Age.Text) == "שגיאה")
                {
                    result_Order.Text = "הכנס טווח גילאים תקין";
                    NoTextError = false;
                    result_Order.Visible = true;
                }
                if (NoTextError && ValidateAge(Min_Age.Text, Max_Age.Text) == "שגיאה")
                {
                    result_Order.Text = "הכנס טווח גילאים תקין";
                    NoTextError = false;
                    result_Order.Visible = true;
                }
                if (OpeningHour.Text != "" && ClosingHour.Text != "")
                {
                    if (NoTextError && ValidateOpeningAndClosingHours(OpeningHour.Text, ClosingHour.Text) == "שגיאה")
                    {
                        result_Order.Text = "הכנס שעות פעילות תקינות";
                        NoTextError = false;
                        result_Order.Visible = true;
                    }
                }
                if (((TextBox)e.Item.FindControl("Price")).Text != "")
                {
                    if (NoTextError && ValidatePrice(((TextBox)e.Item.FindControl("Price")).Text) == "שגיאה")
                    {
                        result_Order.Text = "הכנס מחיר תקין";
                        NoTextError = false;
                        result_Order.Visible = true;
                    }
                }
                if (((TextBox)e.Item.FindControl("Duration")).Text != "")
                {
                    if (NoTextError && ValidateDuration(((TextBox)e.Item.FindControl("Duration")).Text) == "שגיאה")
                    {
                        result_Order.Text = "הכנס משך אטרקציה תקין";
                        NoTextError = false;
                        result_Order.Visible = true;
                    }
                }

                //אם וולידיישן תקין מבצע את הפעולה
                if (NoTextError)
                {

                    string s = "";
                    for (int i = 0; i < AttractionType.Items.Count; i++)// בדיקת סימון בצקבוקסליסט - סינון לפי אטרקציה
                    {
                        if (AttractionType.Items[i].Selected)
                        {
                            if (s.Length == 0)
                                s += "Attraction_Type =";

                            s += AttractionType.Items[i].Value + " OR Attraction_Type= ";
                        }
                    }
                    if (s.Contains("OR"))// אם סינן לפי אטרקציה מסדר את המחרוזת
                        s = s.Substring(0, s.Length - 21);

                    // בדיקת סינון גילים 
                    if (Min_Age.Text != "")// גיל מינמלי
                    {
                        if (s.Length != 0)
                            s += " AND ";
                        s += "Attraction_MinAge <= " + Min_Age.Text;
                    }
                    if (Max_Age.Text != "") // גיל מקסימלי
                    {
                        if (s.Length != 0)
                            s += " AND ";
                        s += "Attraction_MaxAge >= " + Max_Age.Text;
                    }

                    //חשוב!!! - לא יכול לעבור ימים - כלומר אחרי כולל 00:00 שעת התחלה ו עד 23:59 שעת סיום******
                    // בדיקת סינון שעות 
                    if (OpeningHour.Text != "")// שעת פתיחה
                    {
                        if (s.Length != 0)
                            s += " AND ";
                        s += "Attraction_OpeningHour <= #" + (DateTime.Parse(OpeningHour.Text)).ToString("yyyy-MM-dd HH:mm:ss") + "#";
                    }
                    if (ClosingHour.Text != "")//שעת סגירה
                    {
                        if (s.Length != 0)
                            s += " AND ";
                        s += "Attraction_ClosingHour >= #" + (DateTime.Parse(ClosingHour.Text)).ToString("yyyy-MM-dd HH:mm:ss") + "#";
                    }

                    if (((TextBox)e.Item.FindControl("Price")).Text != "")// סינון לפי מחיר
                    {
                        if (s.Length != 0)
                            s += " AND ";
                        s += "Attraction_Price = " + ((TextBox)e.Item.FindControl("Price")).Text;
                    }

                    if (((TextBox)e.Item.FindControl("Duration")).Text != "")// סינון לפי משך אטרקציה
                    {
                        if (s.Length != 0)
                            s += " AND ";
                        s += "Attraction_Duration = " + ((TextBox)e.Item.FindControl("Duration")).Text;
                    }

                    try
                    {
                        AttractaionDisplay.DataSource = ((DataSet)Session["Attraction"]).Tables["Attraction"].Select(s).CopyToDataTable();

                    }
                    catch (Exception)
                    {

                        AttractaionDisplay.DataSource = null;
                        NoResult_Lable.Visible = true; // כותרת לא נמצאו תוצאות - מופיעה
                    }

                    AttractaionDisplay.DataBind();
                }
            }
        }


        //דיב גילאים
        protected void AgeDone_Click(object sender, EventArgs e)
        {
            Age.Style["display"] = "none";
        }

        //דיב שעות
        protected void HoursDone_Click(object sender, EventArgs e)
        {
            Hours.Style["display"] = "none";
        }

        //דיב סוגי אטרקציות
        protected void AttractionFilterDone_Click(object sender, EventArgs e)
        {
            AttractionFilter.Style["display"] = "none";
        }

        //חיפוש אטרקציה
        protected void SearchBar_TextChanged(object sender, EventArgs e)// חיפוש משתמשים
        {
            //מחיקת נתוני סינון מהתצוגה
            Min_Age.Text = "";
            Max_Age.Text = "";
            ((TextBox)(((DataList)FilterMenu).Items[0].FindControl("Price"))).Text = "";
            ((TextBox)(((DataList)FilterMenu).Items[0].FindControl("Duration"))).Text = "";
            AttractionType.SelectedIndex = 0;
            AttractaionDisplay.DataSource = ((DataSet)Session["Attraction"]).Tables["Attraction"];
            AttractaionDisplay.DataBind();
            AttractionType.SelectedIndex = -1;
            AttractionType.Items[0].Selected = false;
            AttractionFilter.Style["display"] = "none";
            Age.Style["display"] = "none";

            try // הצגת משתמש
            {
                AttractaionDisplay.DataSource = ((DataSet)Session["Attraction"]).Tables["Attraction"].Select("Attraction_Name = '" + txtFilter.Text + "'").CopyToDataTable();
                NoResult_Lable.Visible = false;// כותרת לא נמצאו תוצאות - נעלמת
            }
            catch (InvalidOperationException e1)// מקרה בו אין משתמשים חסומים ואז הסינון מחזיר שגיאה
            {
                AttractaionDisplay.DataSource = null;
                NoResult_Lable.Visible = true;// כותרת לא נמצאו תוצאות - מופיעה
            }
            AttractaionDisplay.DataBind();
        }

        //מעבר לדף אטרקציה
        protected void AttractaionDisplay_ItemCommand(object source, DataListCommandEventArgs e)
        {
            //Move to another page to show the product
            if (e.CommandName == "DoShow")
            {
                Session["AttractionID_ForAttractionPage"] = ((Label)e.Item.FindControl("Attraction_ID")).Text;
                Session["from"] = "Attractions_Display.aspx";
                Response.Redirect("Attraction_Page.aspx");
            }
        }
    }
}