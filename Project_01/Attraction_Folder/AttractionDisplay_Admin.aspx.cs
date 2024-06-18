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
    public partial class AttractionDispaly_Admin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                MasterPage_UserName.Text = ((User)Session["User"]).User_Name;
                //מאסטר פייג
                Site master = (Site)this.Master;
                master.MasterPageSignUpOut.Text = "התנתק";
                master.MasterPageOrders.Visible = false;
                master.MasterPageSignUpOut.CommandName = "/Homepage"; //התנתקות
                master.MasterPageNewOrder.CommandName = "/Attraction_Folder/AttractionDisplay_Admin";
                master.MasterPageAbout.CommandName = "/About";
                master.MasterPageLogo.CommandName = "/admin";

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

                ArrayList AttractionTypeArr = Connect.FillArrayListForDropDownList("SELECT AttractionType_ID, AttractionType_Type FROM AttractionType WHERE IsValid = " + true, "AttractionType_Type", "AttractionType_ID");
                for (int i = 0; i < AttractionTypeArr.Count; i++)
                {
                    AttractionType.Items.Add((ListItem)AttractionTypeArr[i]);
                }

                DataSet ds;
                Session["Attraction_AdminAttDis"] = null;
                if (Session["Attraction_AdminAttDis"] == null)
                {
                    ds = Connect.Connect_DataSet("SELECT * FROM Attraction INNER JOIN AttractionType ON Attraction.Attraction_Type" +
                        " = AttractionType.AttractionType_ID WHERE Attraction_Valid = "+true, "Attraction"); //טבלת סוגי אטרקציות inner join - טבלת אטרקציות ללא אטרקציה 67
                    ds.Tables.Add(Connect.Connect_DataTable("SELECT * FROM Attraction INNER JOIN NatureAttraction ON Attraction.Attraction_ID = NatureAttraction.Attraction_ID WHERE Attraction_Valid = " + true, "NatureAttraction"));//טבלת אטרקציות טבע inner join - טבלת אטרקציות ללא אטרקציה 67

                    foreach (DataRow row in ds.Tables["Attraction"].Rows)//
                    {
                        if (row.RowState != DataRowState.Deleted) // אם השורה לא נמחקה = שובצה במסלול
                        {
                            // Calculate the difference in minutes
                            row["Attraction_ClosingHour"] = (DateTime.ParseExact(((row["Attraction_ClosingHour"]).ToString()).Substring(11), "HH:mm:ss", CultureInfo.InvariantCulture)); // ההפרש בדקות מזמן סגירת האטרקציה לשעה הנוכחית
                            row["Attraction_OpeningHour"] = (DateTime.ParseExact(((row["Attraction_OpeningHour"]).ToString()).Substring(11), "HH:mm:ss", CultureInfo.InvariantCulture));// עדכון שעת הפתיחה של ארטקציה - עדכון תאריך - שעה זהה- עושה בעיות אם לא התאריך הנוכחי
                        }
                    }

                    Session["Attraction_AdminAttDis"] = ds;

                }
                else
                {
                    ds = (DataSet)Session["Attraction_AdminAttDis"];
                }

                if (Session["filterdAttractionsForAttractoinPage"] != null) // אם חזר מתצוגת דף אטרקציה עם דאטאסט מסונן
                {
                    ArrayList filteredAttractions = (ArrayList)Session["filterdAttractionsForAttractoinPage"];
                    (((TextBox)(((DataList)FilterMenu).Items[0].FindControl("Duration")))).Text = filteredAttractions[1].ToString();
                    (((TextBox)(((DataList)FilterMenu).Items[0].FindControl("Price")))).Text = filteredAttractions[2].ToString();
                    Min_Age.Text = filteredAttractions[3].ToString();
                    Max_Age.Text = filteredAttractions[4].ToString();
                    if (filteredAttractions[6].ToString() != "")
                    {
                        OpeningHour.Text = (DateTime.Parse(filteredAttractions[6].ToString())).ToString("HH:mm");
                    }
                    if (filteredAttractions[7].ToString() != "")
                    {
                        ClosingHour.Text = (DateTime.Parse(filteredAttractions[7].ToString())).ToString("HH:mm");
                    }
                    CheckBoxList sourceCheckBoxList = (CheckBoxList)filteredAttractions[5];
                    // Iterate over the items in the source CheckBoxList
                    for (int i = 0; i < sourceCheckBoxList.Items.Count; i++)
                    {
                        // Check if the current item is selected
                        if (sourceCheckBoxList.Items[i].Selected)
                        {
                            // Select the corresponding item in the AttractionType CheckBoxList
                            AttractionType.Items[i].Selected = true;
                        }
                    }

                    DataTable attractionTable = (DataTable)filteredAttractions[0]; // הוספת הטבלה המסוננת לדאטאסט
                    attractionTable.TableName = "Attraction";

                    AttractaionDisplay.DataSource = attractionTable;

                    Session["filterdAttractionsForAttractoinPage"] = null;// איפוס הסשן - יתמלא שוב לאותו דבר אם המשתמש לוחץ על עוד אטרקציה באותו סינון
                }
                else //אם  לא חזר מתצוגת דף אטרקציה עם דאטאסט מסונן
                {
                    AttractaionDisplay.DataSource = ds.Tables["Attraction"];
                }

                AttractaionDisplay.DataBind();

                //שינוי צבע הכפתור - "הכל" לנבחר
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
                Attractions_Display.filterdTableForAttractoinPage = null;
                Min_Age.Text = "";
                Max_Age.Text = "";
                ((TextBox)e.Item.FindControl("Price")).Text = "";
                ((TextBox)e.Item.FindControl("Duration")).Text = "";
                AttractionType.SelectedIndex = 0;
                AttractaionDisplay.DataSource = ((DataSet)Session["Attraction_AdminAttDis"]).Tables["Attraction"];
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
                        AttractaionDisplay.DataSource = ((DataSet)Session["Attraction_AdminAttDis"]).Tables["Attraction"].Select(s).CopyToDataTable();
                        Attractions_Display.filterdTableForAttractoinPage = (DataTable)AttractaionDisplay.DataSource; // שמירת מצב הסינון הנוכחי של האטרקציות
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

        //סגירת דיב גילאים
        protected void AgeDone_Click(object sender, EventArgs e)
        {
            Age.Style["display"] = "none";
        }

        //סגירת דיב שעות
        protected void HoursDone_Click(object sender, EventArgs e)
        {
            Hours.Style["display"] = "none";
        }

        //סגירת דיב סוגי אטרקציות
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
            AttractaionDisplay.DataSource = ((DataSet)Session["Attraction_AdminAttDis"]).Tables["Attraction"];
            AttractaionDisplay.DataBind();
            AttractionType.SelectedIndex = -1;
            AttractionType.Items[0].Selected = false;
            AttractionFilter.Style["display"] = "none";
            Age.Style["display"] = "none";
            Attractions_Display.filterdTableForAttractoinPage = null;
            try 
            {
                AttractaionDisplay.DataSource = ((DataSet)Session["Attraction_AdminAttDis"]).Tables["Attraction"].Select("Attraction_Name = '" + txtFilter.Text + "'").CopyToDataTable();
                NoResult_Lable.Visible = false;// כותרת לא נמצאו תוצאות - נעלמת
            }
            catch (InvalidOperationException e1)// מקרה בו אין משתמשים חסומים ואז הסינון מחזיר שגיאה
            {
                AttractaionDisplay.DataSource = null;
                NoResult_Lable.Visible = true;// כותרת לא נמצאו תוצאות - מופיעה
            }
            AttractaionDisplay.DataBind();
        }

        //פעולות דאטאליסט אטרקציות
        protected void AttractaionDisplay_ItemCommand(object source, DataListCommandEventArgs e)
        {
            //מעבר לעמוד אטרקציה
            if (e.CommandName == "DoShow")
            {
                if (Attractions_Display.filterdTableForAttractoinPage != null) // אם התכונה לא ריקה - אם סונן - התכונה מתאפסת אם לוץ על כפתור אטרקציות בתפריט
                {
                    ArrayList arr = new ArrayList();
                    arr.Add(Attractions_Display.filterdTableForAttractoinPage);//דאטאסט סינון
                    arr.Add((((TextBox)(((DataList)FilterMenu).Items[0].FindControl("Duration")))).Text); // משך
                    arr.Add(((TextBox)(((DataList)FilterMenu).Items[0].FindControl("Price"))).Text); //מחיר
                    arr.Add(Min_Age.Text);//גיל מינימלי
                    arr.Add(Max_Age.Text);//גיל מקסימלי
                    arr.Add(AttractionType);//סוגי אטרקציות
                    CheckBoxList c = AttractionType;
                    arr.Add(OpeningHour.Text);//שעת פתיחה
                    arr.Add(ClosingHour.Text);//שעת סגירה
                    Session["filterdAttractionsForAttractoinPage"] = arr;
                }
                Session["AttractionID_ForAttractionPage"] = ((Label)e.Item.FindControl("Attraction_ID")).Text;
                Session["from"] = "AttractionDisplay_Admin.aspx";
                Response.Redirect("Attraction_Page.aspx");
            }

            if (e.CommandName == "Block")
            {
                AttractionService.RemoveAttraction(Convert.ToInt32(((Label)e.Item.FindControl("Attraction_ID")).Text));
                //עדכון התצוגה
                Session["Attraction_AdminAttDis"] = null; //כדי שיעדכן את התצוגה בפעם הבאה שיחזור לדף בעל עסק null  השמת 
                // Move to another page 
                Response.Redirect("AttractionDisplay_Admin.aspx"); // העברה לדף בעל עסק
            }
        }
    }
}