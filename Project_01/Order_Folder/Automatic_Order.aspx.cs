using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Globalization;

namespace Project_01
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        public static int i = 0;
        public static int totalDays; // לצורך שימוש במספר פעולות
        public static ArrayList orderarr; // לצורך שימוש במספר פעולות
        public static string DaysDate = "";// לצורך שימוש במספר פעולות
        protected void Page_Load(object sender, EventArgs e)
        {
            DataSet ds = null; //יצירת  דאטאסט ריק לצרכים שונים
            if (!IsPostBack) // בפעם הראשונה שנכנס לעמוד
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


                // הבאת\יצירת דאטאסט עם  מספר טבלאות
                if (Session["PreferneceDS"] == null)
                {
                    //DataSet הוספת טבלאות ל
                    ArrayList arr = new ArrayList();
                    arr.Add(new DS_Object("SELECT * FROM VacationType WHERE IsValid = " + true, "VacationType")); // סוג חופשה
                    arr.Add(new DS_Object("SELECT * FROM AttractionType WHERE IsValid = " + true, "AttractionType"));//סוג אטרקציה
                    arr.Add(new DS_Object("SELECT * FROM Attraction WHERE Attraction_Valid = " + true, "Attraction")); // אטרקציות
                    arr.Add(new DS_Object("SELECT * FROM Day_Attraction", "Day_Attraction")); // יום אטרקציה
                    ds = Connect.Connect_MultipleDataSet(arr);// יצירת דטאסט המכיל כמה טבלאות
                    Session["PreferneceDS"] = ds; // שמירה בסשן - במידה ולא שמור

                }
                else
                    ds = (DataSet)Session["PreferneceDS"]; // הבאה מהסשן


                // אכלוס צקבוקס ליסט סוגי אטרקציות (ללא מסעדה) בדאטליסט
                ArrayList arrtype = Connect.FillArrayListForDropDownList("SELECT AttractionType_Type ,AttractionType_ID FROM AttractionType WHERE IsValid = " + true + " ORDER BY VacationType_ID ASC;  ", "AttractionType_Type", "AttractionType_ID");
                for (int i = 0; i < arrtype.Count; i++) //שהחזירה עצמי דרופדאוןליסט FillArrayListForDropDownList שקיבלנו מהפעלת הפעולה  ArrayList הכנסת ערכי מדינות (מזהה ושם מדינה) לדרופדאוןליסט מתוך  
                {
                    AttractionTypePreference.Items.Add((ListItem)arrtype[i]);
                }

                //יבוא סשן
                orderarr = (ArrayList)Session["orderarr"];// יבוא הסשן ערכי ההזמנה הראשוניים

                ////*למחוק
                //orderarr = new ArrayList();
                //orderarr.Add("2024-06-1");
                //orderarr.Add("2024-06-3");
                //orderarr.Add(7);
                //orderarr.Add(77);
                ////*

                //*** יאוכלס בשסן בעמוד הראשון - פייק דאטא סט - לוצרכי אכלוס עיצובי
                DataSet dataSet = new DataSet();// Create a new DataSet
                DataTable table = new DataTable("FakeDataset");// Create a new DataTable
                table.Columns.Add("RowNumber", typeof(int));// Define the columns in the DataTable
                dataSet.Tables.Add(table);// Add the DataTable to the DataSet // דאטא טייבל רק - יאוכלס לפי צורך
                Session["FakeDataset"] = dataSet;// הכנסה לסשן - לא בעמוד הזה - בעמוד הזה שימוש בסשן
                //***

                //בדיקת מספר ימי הטיול- לפי ערכי הזמנה ראשוניים
                // המרת ערכי תאריך מסרינג  לצורך בדיקת מספק הימים
                DateTime startDate = Convert.ToDateTime(orderarr[0]);//מתאריך
                DateTime endDate = Convert.ToDateTime(orderarr[1]);//לתאריך
                totalDays = (endDate - startDate).Days + 1; // מספר ימי המסלול
                //totalDays = 4; // מספר ימי המסלול
                DataTable t = ((DataSet)Session["FakeDataset"]).Tables["FakeDataset"];
                // Populate the DataTable with totalDays rows
                for (int i = 1; i <= totalDays; i++)
                {
                    // Create a new DataRow
                    DataRow row = t.NewRow();

                    // Set the value of the 'RowNumber' column to the current iteration
                    row["RowNumber"] = i;

                    // Add the DataRow to the DataTable
                    t.Rows.Add(row);
                }
                //הוספת מספר דאטא ליסט לפי מספר הימים
                DayPreferences.DataSource = t;
                DayPreferences.DataBind();
            }

        }


        private string ValidateStartPlace(DataListItem item)
        {
            TextBox startPlaceTextBox = (TextBox)item.FindControl("StartPlace");
            string startPlace = startPlaceTextBox.Text;
            string pattern = @"^[a-zA-Z0-9\u0590-\u05FF\s,\.]{1,50}$";

            if (string.IsNullOrWhiteSpace(startPlace) || !Regex.IsMatch(startPlace, pattern))
            {
                // Handle invalid start place
                return "שגיאה";
            }
            return "הצלחה";
        }

        private string ValidateDayTimes(DataListItem item)
        {
            TextBox startDayTimeTextBox = (TextBox)item.FindControl("StartDayTime");
            TextBox endDayTimeTextBox = (TextBox)item.FindControl("EndDayTime");

            string startDayTime = startDayTimeTextBox.Text;
            string endDayTime = endDayTimeTextBox.Text;

            string startTimeValidationResult = ValidateTime(startDayTime, "שעת תחילת היום");
            string endTimeValidationResult = ValidateTime(endDayTime, "שעת סיום היום");

            if (startTimeValidationResult != "הצלחה")
            {
                return "שגיאה";
            }

            if (endTimeValidationResult != "הצלחה")
            {
                return "שגיאה";
            }

            if (startTimeValidationResult == "הצלחה" && endTimeValidationResult == "הצלחה")
            {
                TimeSpan startTime = TimeSpan.Parse(startDayTime);
                TimeSpan endTime = TimeSpan.Parse(endDayTime);

                if (endTime <= startTime)
                {
                    return "שגיאה";
                }
            }
            return "הצלחה";
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


        //דאטאבאונד לדאטאליסט פרטי הימים - מעדכן תאריך
        protected void DayPreferences_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            // Check if the Repeater item is an item or alternating item (not a header/footer)
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //DataSet על כל איבר(אייטם-שורה מהטייבל) שנוצר ב -Bind רץ כל פעם שמתבצע DataBind ה

                if (DaysDate == "") // בדיקה האם זה היום הראשון במסלול
                {

                    //השמת היום הראשון בדאטאליסט ושמירת תאריך נוכחי
                    string dateString = (string)orderarr[0]; //תאריך יום ראשון מסשן ערכי ההזמנה הראשוניים
                    ((Label)e.Item.FindControl("DayDate_Lable")).Text = dateString; // השמה בדאטאליסט
                    DaysDate = dateString; // השמת התאריך נוכחי - נשמר היום  אליו יוסיפו יום אחד
                }
                else
                {
                    //הוספת יום לתאריכי המסלול - ושמירת תאריך נוכחי
                    DateTime date = DateTime.Parse(DaysDate); // המרת התאריך מהיום הקודם 
                    DateTime newDate = date.AddDays(1);// הוספת יום אחד לתאריך

                    // If you need it back as a string
                    string newDateString = newDate.ToString("yyyy-MM-dd"); // המרה חזרה לסטרינג
                    ((Label)e.Item.FindControl("DayDate_Lable")).Text = newDateString; // השמת התאריך בדאטאליסט
                    DaysDate = newDateString; // השמת התאריך הנוכחי
                }
            }
        }

        //בחירת סוג חופשה - אוטומטי
        protected void Automatic_Order_Button_Click(object sender, EventArgs e)
        {
            OrderChoice_Div.Style["display"] = "none";
            Automatic_Div.Style["display"] = "block";
        }
        //בחירת סוג חופשה - ידני
        protected void manualOrder_Button_Click(object sender, EventArgs e)
        {
            OrderChoice_Div.Style["display"] = "none";
            Manual_Div.Style["display"] = "block";
        }

        //מסלול ידני
        protected void ManualOrder_Create_Click(object sender, EventArgs e)
        {
            bool NoTextError = true; // Validation errors
            if (NoTextError && ManualOrder_OrderName.Text == "")
            {
                Result_Day.Text = "הכנס שם חופשה תקין";
                NoTextError = false;
            }
            //אם וולידיישן תקין מבצע את הפעולה
            if (NoTextError)
            {
                //יצירת הזמנה
                Connect.Connect_ExecuteNonQuery("INSERT INTO Orders (Order_DaysNumber, Order_StartDate, Order_MinAge, Order_MaxAge, Order_Name, Order_UserName, Order_AddDate, Order_Type ) " +
               "VALUES (" + totalDays + ", #" + orderarr[0] + "#, " + orderarr[2] + ", " + orderarr[3] +
               ", '" + ManualOrder_OrderName.Text + "', '" + ((User)Session["User"]).User_Name + "', #" + DateTime.Now.ToString("yyyy-MM-dd") + "#,'ידני');"); //גילאים ותאריך לצורך סינון בדף חופשות
                Session["OrderCode_ForPathDIs"] = (int)Connect.Connect_ExecuteScalar("Select Max(Order_ID) From Orders"); // שליפת הערך האחרון של המפתח רץ - קוד ההזמנה שיצרנו

                //יצירת כל ימי ההזמנה
                for (int i = 0; i < totalDays; i++)
                {
                    DateTime date = DateTime.Parse((string)orderarr[0]); // המרת תאריך היום הראשון 
                    DateTime newDate = date.AddDays(i);// הוספת מספר משתנה של ימים לתאריך
                    string OrderDate = newDate.ToString("yyyy-MM-dd"); // המרה חזרה לסטרינג

                    // יצירת יום במסד נתונים
                    Connect.Connect_ExecuteNonQuery("INSERT INTO Days (Day_StartHour, Day_EndHour, Order_ID, Day_Date, Day_StartLocationAddress) " +
                        "VALUES( '" + DateTime.Parse("00:01").ToString("HH:mm:ss") + "', '" + DateTime.Parse("00:02").ToString("HH:mm:ss") +
                        "', " + (int)Session["OrderCode_ForPathDIs"] + ", #" + OrderDate + "#, '" + "111th St, Queens, NY 11368, USA" + "')");
                }
                //Move to another page 
                Response.Redirect("Manual_Order.aspx"); // העברה לדף מסלול ידני
            }
            
        }


        //מסלול אוטומטי
        protected async void Create_Click(object sender, EventArgs e)
        {
            bool NoTextError = true; // Validation errors
            int selectedCount = 0; // וידוא שנחבר לפחות סוג אטרקציה אחד
            foreach (ListItem item in AttractionTypePreference.Items)//ריצה על הצקבוקסליסט
            {
                if (item.Selected)
                {
                    selectedCount++;
                }
            }
            if (NoTextError && selectedCount == 0)
            {
                Result_Day.Text = "בחר לפחות סוג אטרקציה מועדפת אחד";
                NoTextError = false;
            }
            if (NoTextError && OrderName.Text == "")
            {
                Result_Day.Text = "הכנס שם חופשה תקין";
                NoTextError = false;
            }
                foreach (DataListItem item in DayPreferences.Items)
            {
                if (NoTextError && ValidateStartPlace(item) == "שגיאה")
                {
                    Result_Day.Text = "הכנס כתובת תקינה";
                    NoTextError = false;
                }
                if (NoTextError && ValidateDayTimes(item) == "שגיאה")
                {
                    Result_Day.Text = "הכנס טווח שעות תקין";
                    NoTextError = false;
                }
            }
            //אם וולידיישן תקין מבצע את הפעולה
            if (NoTextError)
            {


                //ברגע שמוחק מוחק מהסשן גם - עובד בלחצית כפתור אחת
                //*** עבור כל הימים
                // יצירת הזמנה במסד נתונים
                Connect.Connect_ExecuteNonQuery("INSERT INTO Orders (Order_DaysNumber, Order_StartDate, Order_MinAge, Order_MaxAge, Order_Name, Order_UserName, Order_AddDate, Order_Type ) " +
               "VALUES (" + totalDays + ", #" + orderarr[0] + "#, " + orderarr[2] + ", " + orderarr[3] +
               ", '" + OrderName.Text + "', '" + ((User)Session["User"]).User_Name + "', #" + DateTime.Now.ToString("yyyy-MM-dd") + "#,'אוטמטי');"); //גילאים ותאריך לצורך סינון בדף חופשות
                int OrderCode = (int)Connect.Connect_ExecuteScalar("Select Max(Order_ID) From Orders"); // שליפת הערך האחרון של המפתח רץ - קוד ההזמנה שיצרנו

                DataSet ds = (DataSet)Session["PreferneceDS"]; // לקיחת הסשן

                //  מחרוזת בחירות העדפות משתמש + הוספה לארייליסט 
                string s = "(Attraction_Type = ";
                ArrayList selectedItems = new ArrayList();
                if (AttractionTypePreference.Items.Count != 0)// לא יקרה כי נוודא בכפתור אישור בחירת העדפות - אבל בכל זאת
                {
                    // כשיהיה שדה ואליד לשנות לספירת אלה שואליד וירוץ גם על אלה שואליד
                    foreach (ListItem item in AttractionTypePreference.Items)
                    {
                        // If the item is selected, add it to the ArrayList
                        if (item.Selected)
                        {
                            selectedItems.Add(item.Value);// הוספה למערך - יכול להיות שהמערך ללא שימוש
                            s += item.Value + " OR Attraction_Type= ";// הוספה למחרוזת כל סוגי סוגי האטרקציות המועדפות
                        }
                    }
                }
                s = s.Substring(0, s.Length - 21);// החסרת "OR Attraction_Type= "
                s += ")";// סוגר שני לקדימות בשאילתה

                DataTable AttractionTable = ds.Tables["Attraction"];// טבלת אטרקציות עליה מתבצעים הסינונים ומוחסרות אטרקציות אשר נכנסות למסלול - אטרקציות שהחוסרו לא יופיעו שוב בכל הימים
                if (!AttractionTable.Columns.Contains("TimeDiffMinutes"))// אם הטבלה לא מכילה את העמודה
                    AttractionTable.Columns.Add("TimeDiffMinutes", typeof(int));
                //עבור כל יום

                for (i = 0; i < totalDays; i++)
                {
                NewDay: // במקרה והיום לא עובר בלולאה - כלומר אין אטרקציות התואמות את הסינון, ויש עוד ימים למסלולת נעצר כאשר אין עוד ימים למסלול
                        //תאריך היום במסלול 
                    DateTime date = DateTime.Parse((string)orderarr[0]); // המרת תאריך היום הראשון 
                    DateTime newDate = date.AddDays(i);// הוספת מספר משתנה של ימים לתאריך
                    string OrderDate = newDate.ToString("yyyy-MM-dd"); // המרה חזרה לסטרינג

                    string StartDayTime = ((TextBox)DayPreferences.Items[i].FindControl("StartDayTime")).Text;//שעת התחלת היום
                    string EndDayTime = ((TextBox)DayPreferences.Items[i].FindControl("EndDayTime")).Text;// שעת סיום היום
                    string StartAddress = ((TextBox)DayPreferences.Items[i].FindControl("StartPlace")).Text;//כתובת תחילת היום

                    // יצירת יום במסד נתונים
                    Connect.Connect_ExecuteNonQuery("INSERT INTO Days (Day_StartHour, Day_EndHour, Order_ID, Day_Date, Day_StartLocationAddress) " +
                        "VALUES( '" + DateTime.Parse(StartDayTime).ToString("HH:mm:ss") + "', '" + DateTime.Parse(EndDayTime).ToString("HH:mm:ss") +
                        "', " + OrderCode + ", #" + OrderDate + "#, '" + StartAddress + "')");

                    int DayCode = (int)Connect.Connect_ExecuteScalar("Select Max(Day_ID) From Days"); // שליפת הערך האחרון של המפתח רץ - קוד היום שיצרנו

                    // חישוב הזמן בדקות של הזמן שהוכנס משעה עד שעה
                    // Parse times into TimeSpan objects
                    TimeSpan t1 = TimeSpan.Parse(EndDayTime);// "12:30"
                    TimeSpan t2 = TimeSpan.Parse(StartDayTime);// "10:15"
                    TimeSpan difference = t1 - t2;// Subtract times
                                                  // Get the total difference in minutes
                    int CurrenttotalMinutes = (int)difference.TotalMinutes; // מספר דקות היום - משתנה לאחר חיסור כל זמן אטרקציה וזמן הגעה 
                    int DaytotalMinutes = CurrenttotalMinutes;// סך מספר דקות היום - לא משתנה 

                    //הוספת המיקום ההתחלתי האטרקציה הראשונה וזמן ההגעה אליה לדאטא בייס - במידה ואין אטרקציות התואמות את הסינונים יוצא מהלולאה
                    DataTable filteredAttractionTable; // סינון דאטא ליסט אטרקציות
                    DateTime CurrentHour = (DateTime.Parse(StartDayTime)).AddMinutes(DaytotalMinutes - CurrenttotalMinutes);//זמן textmode השעה הנוכחית ב

                    //הסינון - הוספת הפרש הזמנים בין שעת הסגירה והשעה הנוכחית - לבדוק אם חיובי  וניתן לשבץ - אם משך האטרקציה תואם את הזמן שנשאר בין הזמן הנוכחי ושעת הסגירה
                    // Added a new column to store the difference in minutes
                    foreach (DataRow row in AttractionTable.Rows)
                    {
                        if (row.RowState != DataRowState.Deleted) // אם השורה לא נמחקה = שובצה במסלול
                        {
                            // Calculate the difference in minutes
                            row["TimeDiffMinutes"] = ((DateTime.ParseExact(((row["Attraction_ClosingHour"]).ToString()).Substring(11), "HH:mm:ss", CultureInfo.InvariantCulture)) - CurrentHour).TotalMinutes; // ההפרש בדקות מזמן סגירת האטרקציה לשעה הנוכחית
                            row["Attraction_OpeningHour"] = (DateTime.ParseExact(((row["Attraction_OpeningHour"]).ToString()).Substring(11), "HH:mm:ss", CultureInfo.InvariantCulture));// עדכון שעת הפתיחה של ארטקציה - עדכון תאריך - שעה זהה
                        }
                    }

                ErrorHandler:
                    //שאילתת הסינון
                    string FilterStr = s + " AND Attraction_Duration <= " + CurrenttotalMinutes + " AND Attraction_OpeningHour <= #" + (DateTime.Parse(StartDayTime)).AddMinutes(DaytotalMinutes - CurrenttotalMinutes).ToString("yyyy-MM-dd HH:mm:ss") + "#"//שעת פתיחה קטנה מהזמן כרגע
                    + " AND (TimeDiffMinutes) >= Attraction_Duration AND Attraction_MinAge <= " + orderarr[2] + " AND Attraction_MaxAge >= " + orderarr[3];// בדיקה אם עומד בשעת סגירה
                    try // מניעת שגיאה במקרה והסינון לא מחזיר כלום
                    {
                        filteredAttractionTable = AttractionTable.Select(FilterStr).CopyToDataTable(); // סינון על טבלת אטרקציות
                    }
                    catch (InvalidOperationException e2)// אם אין אטרקציות התואמות את הסינונים = נגמר המסלול
                    {
                        while ((DateTime.Parse(StartDayTime)).AddMinutes(60) < DateTime.Parse(EndDayTime))
                        {
                            StartDayTime = ((DateTime.Parse(StartDayTime)).AddMinutes(60)).ToString();
                            DaytotalMinutes -= 60;
                            CurrenttotalMinutes -= 60;
                            CurrentHour = (DateTime.Parse(StartDayTime)).AddMinutes(DaytotalMinutes - CurrenttotalMinutes);//זמן textmode השעה הנוכחית ב
                            foreach (DataRow row in AttractionTable.Rows)
                            {
                                if (row.RowState != DataRowState.Deleted) // אם השורה לא נמחקה = שובצה במסלול
                                {
                                    // Calculate the difference in minutes
                                    row["TimeDiffMinutes"] = ((DateTime.ParseExact(((row["Attraction_ClosingHour"]).ToString()).Substring(11), "HH:mm:ss", CultureInfo.InvariantCulture)) - CurrentHour).TotalMinutes;
                                }
                            }
                            goto ErrorHandler; // סינון על טבלת אטרקציות
                        }
                        if (i + 1 < totalDays) // אם כבר מתחילת היום אין שום אטרקציות שמתאימות - מדלג על הכנסת הנתונים למסד - כי אין כאלה
                        {
                            i++;
                            goto NewDay;// יציאה מלולאת היום

                        }

                        break;
                    }

                    //הוספת הנתונים הראשונים למסלול
                    ArrayList Path = new ArrayList();// יצירת עצם מסלול - אליו יכנסו זמני ההדעה והאטרקציות
                                                     // הוספת המיקום ההתחלתי למסד נוסף ביצירת היום
                    ArrayList StratLocationcoordinates = await BingMapsGeocoder.GetCoordinatesByAddressAsync("'" +/*StartPlace.Text*/StartAddress + "'"); // הפיכת כתובת ההתחלה לקורדינאטות
                    Attraction StratLocation = new Attraction(67, Convert.ToDouble(StratLocationcoordinates[0]), Convert.ToDouble(StratLocationcoordinates[1])); //  יצירת עצם אטרקציה שמכיל את המיקום ההתחלתי של המסלול - המקום ממנו יוצאים למסלול לטובת מציאת האטרקציה הראשונה
                    Attraction closestAttraction = OrderService.FindClosestAttraction(filteredAttractionTable, StratLocation); // מציאת האטרציה הקרובה ביותר למיקום ההתחלתי - האטרקציה הראשונה במסלול - לפי מחרוזת שמקבל
                    foreach (DataRow row in AttractionTable.Rows)// הסרת האטרקציה מדאטאסט אטרקציות
                    {
                        if (row.RowState != DataRowState.Deleted) //בדיקה אם השורה קיימת בדאטאסט או שנמחקה כבר - ואז יחזיר שגיאה
                        {
                            if ((int)row["Attraction_ID"] == (closestAttraction.Attraction_ID))
                                row.Delete();
                        }
                    }

                    // המיקום ההתחלתי של היום הוסף ישירות למסד - חלק המסלול הראשון במסלול - הזמן בין נק התחלה לאטרקציה ראשונה
                    Path.Add(await OrderService.Transport(StratLocation, closestAttraction)); // זמן הגעה מנקודת ההתחלה לאטרקציה הראשונה - OrderService.Transport(FromAttraction, ToAttraction));
                    Path.Add(closestAttraction);// האטרקציה הראשונה
                    CurrenttotalMinutes -= (int)((Transportation)Path[0]).TravelTime; // חיסור זמן האטרקציה וזמן ההגעה אליה מנק היציאה  - עוגל לדקות
                    CurrenttotalMinutes -= ((Attraction)Path[1]).Attraction_Duration; // חיסור משך האטרקציה
                                                                                      //***

                    while (CurrenttotalMinutes != 0)// במידה ואכשיהו מתאפס אחלה - כנראה שלא יקרה = יהיה ברייק ברגע שלא יעמוד בתנאים
                    {
                        //***סינוני אטרקציות מסלול
                        //בדיקה אם משך האטרציה לא גדול מהזמן שנשאר למסלול
                        //בדיקה אם שעת הפתיחה קטנה שווה לשעה כרגע = אם עומד בשעת פתיחה
                        // בדיקה אם שעת סיום פחות השעה כרגע((זמן תחילת היום + (סך דקות היום פחות הזמן שנשאר)) = הזמן שנוצל ) גדול שווה למשך האטרקציה = אם עומד בשעת סיום
                        // בדיקה אם האטרציה לא שובצה כבר בהזמנה

                        CurrentHour = (DateTime.Parse(StartDayTime)).AddMinutes(DaytotalMinutes - CurrenttotalMinutes);//זמן textmode השעה הנוכחית ב

                        //הסינון - הוספת הפרש הזמנים בין שעת הסגירה והשעה הנוכחית - לבדוק אם חיובי  וניתן לשבץ - אם משך האטרקציה תואם את הזמן שנשאר בין הזמן הנוכחי ושעת הסגירה
                        foreach (DataRow row in AttractionTable.Rows)
                        {
                            if (row.RowState != DataRowState.Deleted) // אם השורה לא נמחקה - שובצה במסלול
                            {
                                // Calculate the difference in minutes
                                row["TimeDiffMinutes"] = ((DateTime.ParseExact(((row["Attraction_ClosingHour"]).ToString()).Substring(11), "HH:mm:ss", CultureInfo.InvariantCulture)) - CurrentHour).TotalMinutes;
                            }
                        }
                    ErrorHandler2:
                        //שאילתת הסינון - מחושבת שוב ושוב כי ערכי הזמים משתנים בהתאם להוספה למסלול
                        FilterStr = s + " AND Attraction_Duration <= " + CurrenttotalMinutes + " AND Attraction_OpeningHour <= #" + (DateTime.Parse(StartDayTime)).AddMinutes(DaytotalMinutes - CurrenttotalMinutes).ToString("yyyy-MM-dd HH:mm:ss") + "#"//שעת פתיחה קטנה מהזמן כרגע
                            + " AND (TimeDiffMinutes) >= Attraction_Duration AND Attraction_MinAge <= " + orderarr[2] + " AND Attraction_MaxAge >= " + orderarr[3];
                        try // מניעת שגיאה במקרה והסינון לא מחזיר כלום
                        {
                            filteredAttractionTable = AttractionTable.Select(FilterStr).CopyToDataTable(); // סינון על טבלת אטרקציות
                        }
                        catch (InvalidOperationException e2)// אם אין אטרקציות התואמות את הסינונים = נגמר המסלול
                        {
                            while (CurrentHour.AddMinutes(60) < DateTime.Parse(EndDayTime))
                            {
                                CurrenttotalMinutes -= 60; // חיסור הזמן ממשך היום - מדלגים על השעות האלה
                                CurrentHour = CurrentHour.AddMinutes(60);//זמן - לאחר הוספת הזמן textmode השעה הנוכחית ב 
                                foreach (DataRow row in AttractionTable.Rows)
                                {
                                    if (row.RowState != DataRowState.Deleted) // אם השורה לא נמחקה = שובצה במסלול
                                    {
                                        // Calculate the difference in minutes
                                        row["TimeDiffMinutes"] = ((DateTime.ParseExact(((row["Attraction_ClosingHour"]).ToString()).Substring(11), "HH:mm:ss", CultureInfo.InvariantCulture)) - CurrentHour).TotalMinutes;
                                    }
                                }

                                //המססמל את הזמן שיש לדלג עד האטרקציה הבאה במסלול - נועד למקרים בהם יש עוד זמן לסוף היום ואין אטרקציות מתאימות ישר אחרי האטרקציה האחרונה (int) מוסיף למסלול איבר מרווח 
                                // עצם המרווח ישנה את הזמן אך לא יוכנס למסד עצמו
                                // אם האיבר האחרון במערך הוא עצם מרווח - אם כן יעדכן אותו - אם לא יצור אותו ויוסיף למערך
                                if (Path[Path.Count - 1] is Attraction)// אם לא הוסף כבר עצם מרווח - מוסיף
                                {
                                    Path.Add(60);
                                }
                                else // אם נוצר עצם מרווח - מעדכן אותו
                                    Path[Path.Count - 1] = (int)Path[Path.Count - 1] + 60;
                                goto ErrorHandler2; // סינון על טבלת אטרקציות
                            }
                            Path.Remove(Path.Count - 1); // אם למרות המרווח אין אטרקציות תואמות מסיר את עצם המרווח
                            goto DayToDataBase;// אין *עוד* אטרקצות - מכניס למסד אם מה שיש
                        }

                        //סידור האטרקציות לפי האטרקציה הקרובה ביותר לפי מסלול האטרקציות לאטרקציה הקודמת במסלול - אם יש תיקו בוחר אחד
                        filteredAttractionTable.Columns.Add("AbsValue", typeof(int)); // הוספת עמודה לאכסון תוצאות חיסור מיקום האטרקציה ממיקום האטרקציה הקודמת
                        foreach (DataRow row in filteredAttractionTable.Rows) // השמת תוצאות החיסור בעמודה שהוספה בדאטאסט
                        {
                            if (row.RowState != DataRowState.Deleted)
                            {
                                if (Path[Path.Count - 1] is Attraction) // אם העצם האחרון שהוסף הוא אטרקציה ולא מרווח
                                    row["AbsValue"] = Math.Abs((int)row["Attraction_PathOrder"] - (int)(((Attraction)Path[(Path.Count) - 1]).Attraction_PathOrder));
                                else// אם העצם האחרון שהוסף הוא מרווח
                                    row["AbsValue"] = Math.Abs((int)row["Attraction_PathOrder"] - (int)(((Attraction)Path[(Path.Count) - 2]).Attraction_PathOrder));
                            }
                        }
                        DataView dataviewfilter = new DataView(filteredAttractionTable);//יצירת העצם המסנן
                                                                                        // Set the sort by the AbsValue column
                        dataviewfilter.Sort = "AbsValue ASC";  // סידור מהקטן לגדול - כלומר מהכי קרוב לרחוק 
                        DataRow Nextattraction_row = ((DataRowView)dataviewfilter[0]).Row; // Accessing the first row of the DataView +Converting DataRowView to DataRow האטרקציה הנבחרת - ראושנה בסידור

                        Attraction Nextattractioninpath = new Attraction(Convert.ToInt32(Nextattraction_row["Attraction_ID"]), Convert.ToDouble(Nextattraction_row["Attraction_Latitude"]),
                            Convert.ToDouble(Nextattraction_row["Attraction_Longitude"]), Convert.ToInt32(Nextattraction_row["Attraction_Duration"]), Convert.ToInt32(Nextattraction_row["Attraction_PathOrder"])); // יצירת עצם האטרקציה הבאה במסלול

                        foreach (DataRow row in AttractionTable.Rows) // מחיקת האטרקציה שהוספה למסלול מדאטאסט אטרקציות
                        {
                            if (row.RowState != DataRowState.Deleted)
                            {
                                if ((int)row["Attraction_ID"] == (Nextattractioninpath.Attraction_ID))
                                    row.Delete();
                            }
                        }
                        if (Path[Path.Count - 1] is Attraction) // אם העצם האחרון שהוסף הוא אטרקציה ולא מרווח
                            Path.Add(await OrderService.Transport((Attraction)Path[(Path.Count) - 1], Nextattractioninpath)); // זמן הגעה מנקודת האטרקציה הקודמת לאטרקציה החדשה - OrderService.Transport(FromAttraction, ToAttraction));
                        else// אם העצם האחרון שהוסף הוא מרווח
                            Path.Add(await OrderService.Transport((Attraction)Path[(Path.Count) - 2], Nextattractioninpath));
                        Path.Add(Nextattractioninpath);// האטרקציה הבאה במסלול
                        CurrenttotalMinutes -= (int)((Transportation)Path[(Path.Count) - 2/*איבר לפני האיבר האחרון*/]).TravelTime; // חיסור זמן האטרקציה וזמן ההגעה אליה מנק היציאה  - עוגל לדקות
                        CurrenttotalMinutes -= ((Attraction)Path[(Path.Count) - 1]).Attraction_Duration; // חיסור משך האטרקציה
                    }

                DayToDataBase:
                    //העברה למסד נתונים
                    int MinutesFromBeginningDay = 0;// מספר הדקות מתחילת היום - לכל איבר - לאחר כל איבר שהוספה למסד מתעדכן
                    for (int k = 0; k < Path.Count; k++)
                    {
                        if (Path[k] is Transportation)//זמן הגעה
                        {
                            int Transportationtraveltime = (int)(((Transportation)Path[k]).TravelTime); //משך הנסיעה
                            Connect.Connect_ExecuteNonQuery("INSERT INTO Day_Transportation (Day_ID, StartHour, EndHour, FromAttraction, ToAttraction, TravelType) " +
                                "VALUES (" + DayCode + ", '" + DateTime.Parse(StartDayTime).AddMinutes(MinutesFromBeginningDay).ToString("yyyy-MM-dd HH:mm:ss") +
                                "', '" + DateTime.Parse(StartDayTime).AddMinutes(MinutesFromBeginningDay + Transportationtraveltime).ToString("yyyy-MM-dd HH:mm:ss") + "', '" +
                                ((Transportation)Path[k]).FromAttraction.Attraction_ID + "', '" + ((Transportation)Path[k]).ToAttraction.Attraction_ID + "', '" + ((Transportation)Path[k]).TravelWay + "')");
                            MinutesFromBeginningDay += Transportationtraveltime;
                        }
                        else if (Path[k] is Attraction)//אטרקציה
                        {
                            int AttractionDuration = (int)(((Attraction)Path[k]).Attraction_Duration); // משך האטרקציה
                            Connect.Connect_ExecuteNonQuery("INSERT INTO Day_Attraction (Day_ID, StartHour, EndHour, Attraction_ID )" +
                                " VALUES (" + DayCode + ", '" + (DateTime.Parse(StartDayTime)).AddMinutes(MinutesFromBeginningDay) + "', '" +
                                (DateTime.Parse(StartDayTime)).AddMinutes(MinutesFromBeginningDay + AttractionDuration) + "', " + (int)(((Attraction)Path[k]).Attraction_ID) + " )");
                            MinutesFromBeginningDay += AttractionDuration;
                        }
                        else//מרווח
                        {
                            MinutesFromBeginningDay += (int)Path[k]; // במידה והיה מרווח באטרקציות מויסף את זמן המרווח ליום - גורם לכך שהכנסת השעות לטבלאות תהיה נכונה עם המרווח
                        }
                    }
                }
                Session["OrderCode_ForPathDIs"] = OrderCode;
                Response.Redirect("AutomaticPathDisplay.aspx"); // העברה לדף בעל עסק
            }
        }

    }

}