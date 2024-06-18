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

namespace Project_01
{
    public partial class AutomaticPathDisplay : System.Web.UI.Page
    {
        public static DataSet ds = null; //יצירת  דאטאסט ריק לצרכים שונים
        public static DataSet OrderDs = null;
        public static int Order_ID ; //(Session["OrderID"]); מקושר או ליצירת הזמנה או לתוצדת הזמנות
        public static string DayDate ; // ימחק - ויוחלף ב לייבל נקודה טקסט
        public static ArrayList Days = new ArrayList();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) // בפעם הראשונה שנכנס לעמוד
            {
                MasterPage_UserName.Text = ((User)Session["User"]).User_Name;
                if (((User)Session["User"]).User_Type != "אדמין")//משתמש רשום
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
                    else//אדמין
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
                

                Order_ID = Convert.ToInt32(Session["OrderCode_ForPathDIs"]);
                Session["OrderCode_ForPathDIs"] = null;

                //דאטאסט הזמנה
                OrderDs = Connect.Connect_DataSet("SELECT * FROM Orders WHERE Order_ID = " + Order_ID, "Order");//לפי קוד הזמנה

                Session["Days"] = null;//עדכון תצוגה
                Days = new ArrayList();
                //דאטאסט ימים
                if (Session["Days"] == null)
                {
                    //דאטאסט ימים לפי קוד הזמנה
                    ds = Connect.Connect_DataSet("SELECT * FROM Days WHERE Order_ID = " + Order_ID, "Days");

                    // מערך קוד הימים לפי דאטא סט ימים

                    string s = "Day_ID = "; // מחרוזת קודי ימים
                    foreach (DataRow row in ds.Tables["Days"].Rows)
                    {
                        int Day = (int)row["Day_ID"];
                        Days.Add(Day); // הוספה למערך ימים
                        s += Day + " OR Day_ID = ";// הוספה למחרוזת כל סוגי קודי הימים
                    }

                    //השמה בדאטאסט את כל פרטי ההזמנה - ימים במלואם
                    s = s.Substring(0, s.Length - 13);// החסרת "OR Attraction_Type= "
                    ds.Tables.Add((Connect.Connect_DataTable("SELECT * FROM Day_Transportation WHERE " + s, "Day_Transportation"))); // טבלת תחבורה לפי קודי הימים
                    ds.Tables.Add((Connect.Connect_DataTable("SELECT * FROM Day_Attraction WHERE " + s, "Day_Attraction"))); // טבלת אטרקציות לפי קודי הימים
                    ds.Tables.Add((Connect.Connect_DataTable("SELECT * FROM Attraction ", "Attraction"))); // טבלת אטרקציות 
                    Session["Days"] = ds;
                }
                else
                    ds = (DataSet)Session["Days"];

                // השמת נתונים בתצוגה
                OrderName_Lable.Text = (OrderDs.Tables["Order"].Rows[0])["Order_Name"].ToString(); // שם הזמנה
                DayDate = DateTime.ParseExact((OrderDs.Tables["Order"].Rows[0])["Order_StartDate"].ToString(), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");// תאריך תחילת החופשה
                DayDate_Lable.Text = DayDate; // עדכון תאריך יום לתצוגה
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

            //אם היום הראשון במסלול משנה את צבע הכפתור יום קודם
            if (DayDate == DateTime.ParseExact((OrderDs.Tables["Order"].Rows[0])["Order_StartDate"].ToString(), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd")) // אם תאריך היום שווה ליום הראשון בהזמנה
                PrevDayInPath.Style["background-color"] = "#FFFFE0 ";
            else
                PrevDayInPath.Style["background-color"] = "Yellow ";


            //אם היום האחרון במסלול משנה את צבע הכפתור יום הבא
            if (DayDate == DateTime.ParseExact((OrderDs.Tables["Order"].Rows[0])["Order_StartDate"].ToString(), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture).AddDays(Convert.ToInt32((OrderDs.Tables["Order"].Rows[0])["Order_DaysNumber"].ToString()) - 1).ToString("yyyy-MM-dd")) // אם תאריך היום שווה ליום הראשון בהזמנה
                NextDayInPath.Style["background-color"] = "#FFFFE0 ";
            else
                NextDayInPath.Style["background-color"] = "Yellow ";


            //חישוב מיקום תאריך היום במערך
            int DayInArrayPath = (int)((TimeSpan)(DateTime.ParseExact(DayDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) - DateTime.ParseExact((OrderDs.Tables["Order"].Rows[0])["Order_StartDate"].ToString(), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture))).TotalDays;

            // מיקום התחלתי
            try
            {
                StartPlace.DataSource = ds.Tables["Days"].Select("Day_Date = #" + DayDate + "#").CopyToDataTable(); // סינון על טבלת מיקום התחלתי
            }
            catch (Exception)
            {
                StartPlace.DataSource = null;
            }
            StartPlace.DataBind();

            //דאטאסט תחבורה
            try
            {
                Transportation.DataSource = ds.Tables["Day_Transportation"].Select("Day_ID = " + (int)Days[DayInArrayPath]).CopyToDataTable(); // סינון על טבלת תחבורה
            }
            catch (Exception)
            {
                Transportation.DataSource = null;
            }
            
            Transportation.DataBind();

            //דאטאסט אטרקציות
            try
            {
                Attraction.DataSource = ds.Tables["Day_Attraction"].Select("Day_ID = " + (int)Days[DayInArrayPath]).CopyToDataTable(); // סינון על טבלת אטרקציות
            }
            catch (Exception)
            {
                Attraction.DataSource = null;
            }
            
            Attraction.DataBind();
        }

        //דאטאבאונד - אטרקציות
        protected void Attraction_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            // Check if the Repeater item is an item or alternating item (not a header/footer)
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                foreach (DataRow row in ((DataSet)Session["Days"]).Tables["Attraction"].Rows)
                {
                    if ((int)row["Attraction_ID"] == Convert.ToInt32(((Label)e.Item.FindControl("Attraction_ID")).Text))
                    {
                        ((Image)e.Item.FindControl("Attraction_Photo")).ImageUrl = row["Attraction_Photo"].ToString();
                        ((Label)e.Item.FindControl("Attraction_Name")).Text = row["Attraction_Name"].ToString();
                        Label StartHour = (Label)e.Item.FindControl("AttractionStartHour");
                        StartHour.Text = (DateTime.ParseExact(StartHour.Text, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture)).ToString("HH:mm");
                        Label EndHour = (Label)e.Item.FindControl("AttractionEndHour");
                        EndHour.Text = (DateTime.ParseExact(EndHour.Text, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture)).ToString("HH:mm");
                    }
                }
            }
        }

        //דאטאבאונד - תחבורה
        protected void Transportation_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            // Check if the Repeater item is an item or alternating item (not a header/footer)
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                foreach (DataRow row in ((DataSet)Session["Days"]).Tables["Attraction"].Rows)
                {
                    if ((int)row["Attraction_ID"] == Convert.ToInt32(((Label)e.Item.FindControl("FromAttraction")).Text))
                    {
                        ((Label)e.Item.FindControl("FromAttraction_Lable")).Text = row["Attraction_Name"].ToString();
                        Label StartHour = (Label)e.Item.FindControl("TransportationStartHour");
                        StartHour.Text = (DateTime.ParseExact(StartHour.Text, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).ToString("HH:mm");
                        Label EndHour = (Label)e.Item.FindControl("TransportationEndHour");
                        EndHour.Text = (DateTime.ParseExact(EndHour.Text, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).ToString("HH:mm");
                        Label TravelType = (Label)e.Item.FindControl("TravelType");
                        if (TravelType.Text == "Walking")
                        {
                            ((ImageButton)e.Item.FindControl("TravelType_Photo")).ImageUrl = "~/pictures/White-Man-Walk.png";
                        }
                        else
                        {
                            ((ImageButton)e.Item.FindControl("TravelType_Photo")).ImageUrl = "~/pictures/white-car-icon.jpg"; ;
                        }
                    }

                    if ((int)row["Attraction_ID"] == Convert.ToInt32(((Label)e.Item.FindControl("ToAttraction")).Text))
                    {
                        ((Label)e.Item.FindControl("ToAttraction_Lable")).Text = row["Attraction_Name"].ToString();
                    }
                }
            }
        }

        //דאטאבאונד - מיקום התחלתי
        protected void StartPlace_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            // Check if the Repeater item is an item or alternating item (not a header/footer)
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label StartHour = (Label)e.Item.FindControl("StartPlaceStartHour");
                StartHour.Text = StartHour.Text.Substring(0, 5);
            }
        }

        //יום הבא במסלול
        protected void NextDayInPath_Click(object sender, EventArgs e)
        {
            DateTime t1 = DateTime.ParseExact(DayDate, "yyyy-MM-dd", CultureInfo.InvariantCulture); // תאריך היום הנוכחי
            DateTime t2 = DateTime.ParseExact((OrderDs.Tables["Order"].Rows[0])["Order_StartDate"].ToString(), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture).AddDays(Convert.ToInt32((OrderDs.Tables["Order"].Rows[0])["Order_DaysNumber"].ToString())-1 ); // תאריך הום האחרון בהזמנה
            if ( t1 < t2)
            {
                DayDate = DateTime.ParseExact(DayDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).AddDays(1).ToString("yyyy-MM-dd"); // הוספת יום לתאריך
                DayDate_Lable.Text = DayDate; // עדכון תאריך יום בתצוגה
                DisplayFunc(); // עדכון תצוגה
            }
                

        }

        //יום קודם במסלול
        protected void PrevDayInPath_Click(object sender, EventArgs e)
        {
            DateTime t1 = DateTime.ParseExact(DayDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);// תאריך היום הנוכחי
            DateTime t2 = DateTime.ParseExact((OrderDs.Tables["Order"].Rows[0])["Order_StartDate"].ToString(), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture); // תאריך תחילת ההזמנה
            if (t1 > t2) 
            {
                DayDate = DateTime.ParseExact(DayDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).AddDays(-1).ToString("yyyy-MM-dd");
                DayDate_Lable.Text = DayDate; // עדכון תאריך יום בתצוגה
                DisplayFunc(); // עדכון תצוגה
            }
        }

        //תצוגת כל האטרקציות - רשימה
        protected void AllAttractions_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            // Check if the Repeater item is an item or alternating item (not a header/footer)
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                foreach (DataRow row in ((DataSet)Session["Days"]).Tables["Attraction"].Rows)
                {
                    if ((int)row["Attraction_ID"] == Convert.ToInt32(((Label)e.Item.FindControl("Attraction_ID")).Text))
                    {
                        ((Label)e.Item.FindControl("Attraction_Name")).Text = row["Attraction_Name"].ToString();
                        ((Label)e.Item.FindControl("AttractionStartHour")).Text = (((Label)e.Item.FindControl("AttractionStartHour")).Text).Substring(10);
                        ((Label)e.Item.FindControl("AttractionEndHour")).Text = (((Label)e.Item.FindControl("AttractionEndHour")).Text).Substring(10);
                    }
                }

                bool DidNotChange = true;
                foreach (DataRow row in ds.Tables["Days"].Rows)
                {
                    if (DidNotChange && (int)row["Day_ID"] == Convert.ToInt32((((Label)e.Item.FindControl("Date")).Text)))
                    {
                        ((Label)e.Item.FindControl("Date")).Text = (row["Day_Date"].ToString()).Substring(0, 10);
                        DidNotChange = false;
                    }
                }
            }
        }

        protected void DisplayWay_Click(object sender, EventArgs e)
        {
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