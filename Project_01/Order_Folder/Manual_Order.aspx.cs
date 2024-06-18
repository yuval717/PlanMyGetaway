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
    public partial class Manual_Order : System.Web.UI.Page
    {
        public static DataSet ds = null; //יצירת  דאטאסט ריק לצרכים שונים
        public static DataSet OrderDs = null;
        public static int Order_ID; // מקושר או ליצירת הזמנה או לתוצדת הזמנות
        public static string DayDate; // ימחק - ויוחלף ב לייבל נקודה טקסט
        public static ArrayList Days = new ArrayList();
        public static string s; // שאילתת ימי החופשה
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack) // בפעם הראשונה שנכנס לעמוד
            {
                MasterPage_UserName.Text = ((User)Session["User"]).User_Name;
                s = null;
                Site master = (Site)this.Master; // מאסטר פייג
                if (((User)Session["User"]).User_Type != "אדמין")//משתמש רשום
                {
                    //מאסטר פייג
                    master.MasterPageSignUpOut.Text = "התנתק";
                    master.MasterPageOrders.Visible = true;
                    master.MasterPageOrders.CommandName = "/User_Folder/UsersOrders";
                    master.MasterPageSignUpOut.CommandName = "/Homepage"; //התנתקות
                    master.MasterPageNewOrder.CommandName = "/Attraction_Folder/Attractions_Display";
                    master.MasterPageAbout.CommandName = "/About";
                    master.MasterPageLogo.CommandName = "/Homepage";
                }
                else//אדמין
                {
                    //מאסטר פייג
                    
                    master.MasterPageSignUpOut.Text = "התנתק";
                    master.MasterPageOrders.Visible = false;
                    master.MasterPageSignUpOut.CommandName = "/Homepage"; //התנתקות
                    master.MasterPageNewOrder.CommandName = "/Attraction_Folder/AttractionDisplay_Admin";
                    master.MasterPageAbout.CommandName = "/About";
                    master.MasterPageLogo.CommandName = "/admin";

                    //תצוגה
                    AttractionAdd.Style["Display"] = "none";
                    AttractionRemove.Style["Display"] = "none";
                }

                Order_ID = Convert.ToInt32(Session["OrderCode_ForPathDIs"]);//השמת קוד הזמנה

                //דאטאסט הזמנה
                OrderDs = Connect.Connect_DataSet("SELECT * FROM Orders WHERE Order_ID = " + Order_ID, "Order");//לפי קוד הזמנה
                Session["OrderDs"] = OrderDs;
                //דאטאסט ימים
                //דאטאסט ימים לפי קוד הזמנה
                Days = new ArrayList();
                ds = Connect.Connect_DataSet("SELECT * FROM Days WHERE Order_ID = " + Order_ID, "Days");
                if (ds.Tables["Days"].Rows.Count > 0) // אם יש נתונים להצגה
                {
                    // מערך קוד הימים לפי דאטא סט ימים

                    s = "Day_ID = "; // מחרוזת קודי ימים
                    foreach (DataRow row in ds.Tables["Days"].Rows)
                    {
                        int Day = (int)row["Day_ID"];
                        Days.Add(Day); // הוספה למערך ימים
                        s += Day + " OR Day_ID = ";// הוספה למחרוזת כל סוגי קודי הימים
                    }

                    //השמה בדאטאסט את כל פרטי ההזמנה - ימים במלואם
                    s = s.Substring(0, s.Length - 13);// החסרת " OR Day_ID = "
                    ds.Tables.Add((Connect.Connect_DataTable("SELECT * FROM Day_Attraction WHERE " + s + " ORDER BY StartHour ASC", "Day_Attraction"))); // טבלת אטרקציות לפי קודי הימים
                    ds.Tables.Add((Connect.Connect_DataTable("SELECT * FROM Attraction WHERE Attraction_Valid = " + true, "Attraction"))); // טבלת אטרקציות 

                    // השמת נתונים בתצוגה
                    OrderName_Lable.Text = (OrderDs.Tables["Order"].Rows[0])["Order_Name"].ToString(); // שם הזמנה
                    DayDate = DateTime.ParseExact((OrderDs.Tables["Order"].Rows[0])["Order_StartDate"].ToString(), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");// תאריך תחילת החופשה
                    DayDate_Lable.Text = DayDate; // עדכון תאריך יום לתצוגה
                } 

                // הוספת אטרקציה למסלול ידני

                if (Session["AttractionID_ForAddingToPath"] != null)// מתקיים כאשר חוזר מעמוד בחירת אטרקציה להוספה - מסלול ידני
                {
                    //מיקום התאריך במערך בימים
                    int DayPlaceInDayArr = (int)((TimeSpan)(DateTime.ParseExact(Session["ManualAttractionSearch_DateTOadd"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture) - DateTime.ParseExact((OrderDs.Tables["Order"].Rows[0])["Order_StartDate"].ToString(), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture))).TotalDays;

                    //הכנסה למסד לאחר חזרה מדף בחירת אטארקציה להוספה
                    string str = "INSERT INTO Day_Attraction (Day_ID, StartHour, EndHour, Attraction_ID )" +
                            " VALUES (" + Days[DayPlaceInDayArr] + ", '" + DateTime.Parse(((ArrayList)Session["ManualAttractionSearch"])[0].ToString()).ToString("HH:mm:ss") + "', '" +
                            DateTime.Parse(((ArrayList)Session["ManualAttractionSearch"])[1].ToString()).ToString("HH:mm:ss") + "', " + (Session["AttractionID_ForAddingToPath"]).ToString() + " )";
                    Connect.Connect_ExecuteNonQuery(str);

                    // עדכון דאטאסט מסלול אטרקציות
                    //החלפת טבלאות לטבלה מסוננת
                    ds.Tables.Remove("Day_Attraction");
                    DataTable NewDay_Attraction = Connect.Connect_DataTable("SELECT * FROM Day_Attraction WHERE " + s + " ORDER BY StartHour ASC", "Day_Attraction");
                    ds.Tables.Add(NewDay_Attraction);

                    DayDate = Session["ManualAttractionSearch_DateTOadd"].ToString();
                    DayDate_Lable.Text = DayDate; // עדכון תאריך יום לתצוגה

                    //איפוס נתוני הסשנים - להוספה עתידית
                    Session["AttractionID_ForAddingToPath"] = null;
                    Session["ManualAttractionSearch_DateTOadd"] = null;
                }
                Session["Days_Manual"] = ds;
            }

            //השמת נתונים בתצוגת רשימה
            DataView view = new DataView(ds.Tables["Day_Attraction"]); //nullעמיד ב 
            view.Sort = "Day_ID ASC";  // Sort by "Day_ID" column in ascending order
            AllAttractions.DataSource = view.ToTable();
            AllAttractions.DataBind();

            DisplayFunc();// עדכון תצוגת המסלול
                DayDate_Lable.Visible = true;
            
        }

        // עדכון תצוגת המסלול
        public void DisplayFunc()
        {
            DataSet tr = (DataSet)(Session["OrderDs"]);
            //אם היום הראשון במסלול משנה את צבע הכפתור יום קודם
            if (DayDate == DateTime.ParseExact((tr.Tables["Order"].Rows[0])["Order_StartDate"].ToString(), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd")) // אם תאריך היום שווה ליום הראשון בהזמנה
                PrevDayInPath.Style["background-color"] = "#FFFFE0 ";
            else
                PrevDayInPath.Style["background-color"] = "Yellow ";


            //אם היום האחרון במסלול משנה את צבע הכפתור יום הבא
            if (DayDate == DateTime.ParseExact((((DataSet)(Session["OrderDs"])).Tables["Order"].Rows[0])["Order_StartDate"].ToString(), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture).AddDays(Convert.ToInt32((OrderDs.Tables["Order"].Rows[0])["Order_DaysNumber"].ToString()) - 1).ToString("yyyy-MM-dd")) // אם תאריך היום שווה ליום הראשון בהזמנה
                NextDayInPath.Style["background-color"] = "#FFFFE0 ";
            else
                NextDayInPath.Style["background-color"] = "Yellow ";


            //חישוב מיקום תאריך היום במערך
            int DayInArrayPath = (int)((TimeSpan)(DateTime.ParseExact(DayDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) - DateTime.ParseExact((OrderDs.Tables["Order"].Rows[0])["Order_StartDate"].ToString(), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture))).TotalDays;

            //דאטאסט אטרקציות
            try
            {
                Attraction.DataSource = ds.Tables["Day_Attraction"].Select("Day_ID = " + (int)Days[DayInArrayPath]).CopyToDataTable(); // סינון על טבלת אטרקציות
            }
            catch (InvalidOperationException e2)// אם אין אטרקציות התואמות את הסינונים = נגמר המסלול
            {
                Attraction.DataSource = null;
            }
            Attraction.DataBind();
        }

        //יום הבא במסלול
        protected void NextDayInPath_Click(object sender, EventArgs e)
        {
            //הסתרת כותרות שגיאה
            NoResult_Lable.Visible = false;
            NoAttraction_Lable.Visible = false;
            RemoveAttractionConfirm.Visible = false;
            //מחיקת תוכן טקטסבוקסים
            FromHour_Remove.Text = "";
            ToHour_Remove.Text = "";
            FromHour.Text = "";
            ToHour.Text = "";


            DateTime t1 = DateTime.ParseExact(DayDate, "yyyy-MM-dd", CultureInfo.InvariantCulture); // תאריך היום הנוכחי
            DateTime t2 = DateTime.ParseExact((OrderDs.Tables["Order"].Rows[0])["Order_StartDate"].ToString(), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture).AddDays(Convert.ToInt32((OrderDs.Tables["Order"].Rows[0])["Order_DaysNumber"].ToString()) - 1); // תאריך הום האחרון בהזמנה
            if (t1 < t2)
            {
                DayDate = DateTime.ParseExact(DayDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).AddDays(1).ToString("yyyy-MM-dd"); // הוספת יום לתאריך
                DayDate_Lable.Text = DayDate; // עדכון תאריך יום בתצוגה
                DisplayFunc(); // עדכון תצוגה
            }


        }

        //יום קודם במסלול
        protected void PrevDayInPath_Click(object sender, EventArgs e)
        {
            //הסתרת כותרות שגיאה
            NoResult_Lable.Visible = false;
            NoAttraction_Lable.Visible = false;
            RemoveAttractionConfirm.Visible = false;
            //מחיקת תוכן טקטסבוקסים
            FromHour_Remove.Text = "";
            ToHour_Remove.Text = "";
            FromHour.Text = "";
            ToHour.Text = "";

            DateTime t1 = DateTime.ParseExact(DayDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);// תאריך היום הנוכחי
            DateTime t2 = DateTime.ParseExact((OrderDs.Tables["Order"].Rows[0])["Order_StartDate"].ToString(), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture); // תאריך תחילת ההזמנה
            if (t1 > t2)
            {
                DayDate = DateTime.ParseExact(DayDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).AddDays(-1).ToString("yyyy-MM-dd");
                DayDate_Lable.Text = DayDate; // עדכון תאריך יום בתצוגה
                DisplayFunc(); // עדכון תצוגה
            }
        }


        //דאטאליסט אטרקציות - דאטאבאונד
        protected void Attraction_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            // Check if the Repeater item is an item or alternating item (not a header/footer)
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                foreach (DataRow row in ((DataSet)Session["Days_Manual"]).Tables["Attraction"].Rows)
                {
                    if ((int)row["Attraction_ID"] == Convert.ToInt32(((Label)e.Item.FindControl("Attraction_ID")).Text))
                    {
                        ((Image)e.Item.FindControl("Attraction_Photo")).ImageUrl = row["Attraction_Photo"].ToString();
                        ((Label)e.Item.FindControl("Attraction_Name")).Text = row["Attraction_Name"].ToString();
                    }
                }
            }
        }

        private string ValidateTime(string time, string fieldName)
        {
            if (!DateTime.TryParse(time, out DateTime parsedTime))
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

        //מעבר לעמוד בחירת אטרקציה להוספה
        protected void SearchForAttraction_Click(object sender, EventArgs e)
        {
            //הסתרת כותרות
            NoAttraction_Lable.Visible = false;
            NoResult_Lable.Visible = false; // העלמת כותרת - בשעה זו משובצת כבר אטרקציה
            RemoveAttractionConfirm.Visible = false;

            //וולידיישן
            bool NoTextError = true;//שגיאות וולידישן
                if (NoTextError && ValidateOpeningAndClosingHours(FromHour.Text, ToHour.Text) == "שגיאה")
                {
                    NoResult_Lable.Text = "הכנס שעות שיבוץ תקינות";
                    NoTextError = false;
                    NoResult_Lable.Visible = true;
                }
            //אם וולידיישן תקין מבצע את הפעולה
            if (NoTextError)
            {

                //בדיקת פניות השעות במסלול ביום מסויים****

                //מיקום התאריך במערך בימים
                int DayPlaceInDayArr = (int)((TimeSpan)(DateTime.ParseExact(DayDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) - DateTime.ParseExact((OrderDs.Tables["Order"].Rows[0])["Order_StartDate"].ToString(), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture))).TotalDays;
                string s = "SELECT * FROM Day_Attraction WHERE Day_ID = " + Days[DayPlaceInDayArr].ToString() + " AND (#" + FromHour.Text + "# BETWEEN StartHour AND EndHour OR #" + ToHour.Text + "# BETWEEN StartHour AND EndHour)";
                if (Connect.Connect_ExecuteScalar(s) == null) // אם לא קיימת אטרקציה בשעות בהן קיימות אטרקציות אחרות
                {
                    ArrayList array = new ArrayList();
                    array.Add(FromHour.Text);// מהשעה
                    array.Add(ToHour.Text);// לשעה
                    array.Add(OrderDs.Tables["Order"].Rows[0]["Order_MinAge"].ToString()); // גיל מינמלי
                    array.Add(OrderDs.Tables["Order"].Rows[0]["Order_MaxAge"].ToString()); // גיל מקסימלי
                    Session["ManualAttractionSearch"] = array;
                    Session["ManualAttractionSearch_DateTOadd"] = DayDate;
                    Response.Redirect("AttractionSearch_ManualOrder.aspx");
                }
                else // אם קיימות אטרקציות אחרות בזמן הרצון לשיבוץ
                {
                    NoResult_Lable.Text = "בשעות אלו משובצת אטרקציה";
                    NoResult_Lable.Visible = true; // הצגת כותרת - בשעה זו משובצת כבר אטרקציה
                                                   //מחיקת תוכן טקטסבוקסים
                    FromHour_Remove.Text = "";
                    ToHour_Remove.Text = "";
                    FromHour.Text = "";
                    ToHour.Text = "";
                }
            }

        }


        //מחיקת אטרקציה מהמסלול
        protected void RemoveAttraction_Click(object sender, EventArgs e)
        {
            //הסתרת הודעות שגיאה
            NoResult_Lable.Visible = false;
            NoAttraction_Lable.Visible = false;
            RemoveAttractionConfirm.Visible = false;

            //וולידיישן
            bool NoTextError = true;//שגיאות וולידישן
                if (NoTextError && ValidateOpeningAndClosingHours(FromHour_Remove.Text, ToHour_Remove.Text) == "שגיאה")
                {
                    NoAttraction_Lable.Text = "הכנס שעות מחיקה תקינות";
                    NoTextError = false;
                    NoAttraction_Lable.Visible = true;
                }
            //אם וולידיישן תקין מבצע את הפעולה
            if (NoTextError)
            {

                //מיקום התאריך במערך בימים
                int DayPlaceInDayArr = (int)((TimeSpan)(DateTime.ParseExact(DayDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) - DateTime.ParseExact((OrderDs.Tables["Order"].Rows[0])["Order_StartDate"].ToString(), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture))).TotalDays;
                //מחרוזת בדיקת קיימות
                string s = "SELECT* FROM Day_Attraction WHERE Day_ID = " + Days[DayPlaceInDayArr].ToString() + " AND (StartHour = #" + FromHour_Remove.Text + "# AND EndHour = #" + ToHour_Remove.Text + "#)";
                if (Connect.Connect_ExecuteScalar(s) != null)//  אם קיימת אטרציה בין השעות האלה - למחיקה
                {
                    RemoveAttractionConfirm.Visible = true;
                }
                else
                {
                    NoAttraction_Lable.Text = "בשעות אלו בדיוק לא משובצת אטרקציה";
                    NoAttraction_Lable.Visible = true;
                    //מחיקת תוכן טקטסבוקסים
                    FromHour_Remove.Text = "";
                    ToHour_Remove.Text = "";
                    FromHour.Text = "";
                    ToHour.Text = "";
                }
            }
                
        }

        //אישור מחיקת אטרקציה
        protected void RemoveAttractionConfirm_Click(object sender, EventArgs e)
        {
            //מיקום התאריך במערך בימים
            int DayPlaceInDayArr = (int)((TimeSpan)(DateTime.ParseExact(DayDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) - DateTime.ParseExact((OrderDs.Tables["Order"].Rows[0])["Order_StartDate"].ToString(), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture))).TotalDays;
            Connect.Connect_ExecuteNonQuery("DELETE FROM Day_Attraction WHERE StartHour = #" + FromHour_Remove.Text + "# AND Day_ID = " + Days[DayPlaceInDayArr]); // מחיקת האטרקציה מהיום במסד

            //תצוגה
            //החלפת טבלאות לטבלה לאחר המחיקה
            ds.Tables.Remove("Day_Attraction");
            DataTable NewDay_Attraction = Connect.Connect_DataTable("SELECT * FROM Day_Attraction WHERE " + Days[DayPlaceInDayArr] + " ORDER BY StartHour ASC", "Day_Attraction");
            ds.Tables.Add(NewDay_Attraction);
            Response.Redirect("Manual_Order.aspx");// מעבר לאותו עמוד- לרענון התצוגה
        }


        //תצוגת כל האטרקציות - רשימה
        protected void AllAttractions_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            // Check if the Repeater item is an item or alternating item (not a header/footer)
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                foreach (DataRow row in ((DataSet)Session["Days_Manual"]).Tables["Attraction"].Rows)
                {
                    if ((int)row["Attraction_ID"] == Convert.ToInt32(((Label)e.Item.FindControl("Attraction_ID")).Text))
                    {
                        ((Label)e.Item.FindControl("Attraction_Name")).Text = row["Attraction_Name"].ToString();
                    }
                }

                bool DidNotChange = true;
                foreach (DataRow row in ds.Tables["Days"].Rows)
                {
                    if (DidNotChange && (int)row["Day_ID"] == Convert.ToInt32((((Label)e.Item.FindControl("Date")).Text)))
                    {
                        ((Label)e.Item.FindControl("Date")).Text = (row["Day_Date"].ToString()).Substring(0,10);
                        DidNotChange = false;
                    }
                }
            }
        }

        protected void DisplayWay_Click(object sender, EventArgs e)
        {
            //הסתרת הודעות שגיאה
            NoResult_Lable.Visible = false;
            NoAttraction_Lable.Visible = false;
            RemoveAttractionConfirm.Visible = false;

            //מחיקת תוכן טקטסבוקסים
            FromHour_Remove.Text = "";
            ToHour_Remove.Text = "";
            FromHour.Text = "";
            ToHour.Text = "";

            if (DisplayWay.Text != "כל הימים")//תצוגת כל הימים
            {
                DisplayFunc();// עדכון תצוגת המסלול
                DayDate_Lable.Visible = true;
                AllDaysDispaly.Style["Display"] = "None";
                SingleDayDisplay.Style["Display"] = "Block";
                DisplayWay.Text = "כל הימים";
            }
            else
            {
                DayDate_Lable.Visible = false;
                AllDaysDispaly.Style["Display"] = "Block";
                SingleDayDisplay.Style["Display"] = "None";
                DisplayWay.Text = "יום יחיד";
            }
        }

        protected void User_Edit_Click(object sender, EventArgs e)
        {
            Connect.Connect_ExecuteNonQuery("UPDATE Orders SET NotValid = " + true + " WHERE Order_ID = " + Order_ID);
            Response.Redirect("/User_Folder/UsersOrders.aspx"); // העברה לדף התחברות
        }
    }
}