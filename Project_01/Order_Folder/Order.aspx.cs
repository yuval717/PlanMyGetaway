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

namespace Project_01
{
    public partial class Order : System.Web.UI.Page
    {
        public static int totalDays; // לצורך שימוש במספר פעולות
        public static ArrayList orderarr; // לצורך שימוש במספר פעולות
        public static string DaysDate = "";// לצורך שימוש במספר פעולות
        protected void Page_Load(object sender, EventArgs e)
        {
            DataSet ds = null; //יצירת  דאטאסט ריק לצרכים שונים
            if (!IsPostBack) // בפעם הראשונה שנכנס לעמוד
            {
                // הבאת\יצירת דאטאסט עם  מספר טבלאות
                if (Session["PreferneceDS"] == null)
                {
                    //DataSet הוספת טבלאות ל
                    ArrayList arr = new ArrayList();
                    arr.Add(new DS_Object("SELECT * FROM VacationType", "VacationType")); // סןג חופשה
                    arr.Add(new DS_Object("SELECT * FROM AttractionType", "AttractionType"));//סוג אטרקציה
                    arr.Add(new DS_Object("SELECT * FROM Attraction WHERE Attraction_ID <> "+67+ "", "Attraction")); // אטרקציות
                    arr.Add(new DS_Object("SELECT * FROM Day_Attraction", "Day_Attraction")); // יום אטרקציה
                    ds = Connect.Connect_MultipleDataSet(arr);// יצירת דטאסט המכיל כמה טבלאות
                    Session["PreferneceDS"] = ds; // שמירה בסשן - במידה ולא שמור

                }
                else
                    ds = (DataSet)Session["PreferneceDS"]; // הבאה מהסשן


                // אכלוס צקבוקס ליסט סוגי אטרקציות (ללא מסעדה) בדאטליסט
                ArrayList arrtype = Connect.FillArrayListForDropDownList("SELECT AttractionType_Type ,AttractionType_ID FROM AttractionType WHERE AttractionType_ID <> 5 ORDER BY VacationType_ID ASC; ", "AttractionType_Type", "AttractionType_ID");
                for (int i = 0; i < arrtype.Count; i++) //שהחזירה עצמי דרופדאוןליסט FillArrayListForDropDownList שקיבלנו מהפעלת הפעולה  ArrayList הכנסת ערכי מדינות (מזהה ושם מדינה) לדרופדאוןליסט מתוך  
                {
                    AttractionTypePreference.Items.Add((ListItem)arrtype[i]);
                }

                //יבוא סשן
                orderarr = (ArrayList)Session["orderarr"];// יבוא הסשן ערכי ההזמנה הראשוניים

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



        // להוסיף כפתור אישור לבחירת ההעדפות - לוודא שנבחר לפחות העדפה אחת********** או לוודא בכפתור יצירה ולפסול

        protected async void Create_Click(object sender, EventArgs e)
        {
            //*** עבור כל הימים
            // יצירת הזמנה במסד נתונים
            Connect.Connect_ExecuteNonQuery("INSERT INTO Orders (Order_DaysNumber, Order_StartDate, Order_MinAge, Order_MaxAge, Order_Name, Order_UserName, Order_AddDate) " +
               "VALUES (" + totalDays + ", #" + orderarr[0] + "#, " + orderarr[2] + ", " + orderarr[3] +
               ", '" + OrderName.Text + "', 'ofer', #" + DateTime.Now.ToString("yyyy-MM-dd") + "#);"); //גילאים ותאריך לצורך סינון בדף חופשות
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
            AttractionTable.Columns.Add("TimeDiffMinutes", typeof(int));
            //עבור כל יום
            for (int i = 0; i < totalDays; i++)
            {
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
                    "', " + OrderCode + ", #" + OrderDate + "#, '"+ StartAddress + "')");

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
                // Add a new column to store the difference in minutes
                foreach (DataRow row in AttractionTable.Rows)
                {
                    if (row.RowState != DataRowState.Deleted) // אם השורה לא נמחקה = שובצה במסלול
                    {
                        // Calculate the difference in minutes
                        row["TimeDiffMinutes"] =  ((Convert.ToDateTime(row["Attraction_ClosingHour"])) - CurrentHour).TotalMinutes;
                    }
                }

                //שאילתת הסינון
                string FilterStr = s + " AND Attraction_Duration <= " + CurrenttotalMinutes + " AND Attraction_OpeningHour <= #" + (DateTime.Parse(StartDayTime)).AddMinutes(DaytotalMinutes - CurrenttotalMinutes).ToString("yyyy-MM-dd HH:mm:ss") + "#"//שעת פתיחה קטנה מהזמן כרגע
                + " AND (TimeDiffMinutes) >= Attraction_Duration ";// בדיקה אם עומד בשעת סגירה
                ErrorHandler:
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
                                row["TimeDiffMinutes"] = ((Convert.ToDateTime(row["Attraction_ClosingHour"])) - CurrentHour).TotalMinutes;
                            }
                        }
                        FilterStr = s + " AND Attraction_Duration <= " + CurrenttotalMinutes + " AND Attraction_OpeningHour <= #" + (DateTime.Parse(StartDayTime)).AddMinutes(DaytotalMinutes - CurrenttotalMinutes).ToString("yyyy-MM-dd HH:mm:ss") + "#"//שעת פתיחה קטנה מהזמן כרגע
                            + " AND (TimeDiffMinutes) >= Attraction_Duration ";// בדיקה אם עומד בשעת סגירה
                        goto ErrorHandler; // סינון על טבלת אטרקציות
                    }
                    break;// יציאה מלולאת היום
                }

                //הוספת הנתונים הראשונים למסלול
                ArrayList Path = new ArrayList();// יצירת עצם מסלול - אליו יכנסו זמני ההדעה והאטרקציות
                // הוספת המיקום ההתחלתי למסד נוסף ביצירת היום
                ArrayList StratLocationcoordinates = await BingMapsGeocoder.GetCoordinatesByAddressAsync("'"+/*StartPlace.Text*/StartAddress+"'"); // הפיכת כתובת ההתחלה לקורדינאטות
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
                int pathcount = 1; // מיקום האיברים הבאים במסלול - משומש בהוספה למערך
                while (CurrenttotalMinutes != 0)// במידה ואכשיהו מתאפס אחלה - כנראה שלא יקרה = יהיה ברייק ברגע שלא יעמוד בתנאים
                {
                    pathcount++; //נכנס ללואה = איבר חדש - אם לאחר סינון איןם תוצאות יוצא - אז לא משנה שהוסף
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
                            row["TimeDiffMinutes"] = ((Convert.ToDateTime(row["Attraction_ClosingHour"])) - CurrentHour).TotalMinutes;
                        }
                    }

                    //שאילתת הסינון - מחושבת שוב ושוב כי ערכי הזמים משתנים בהתאם להוספה למסלול
                    FilterStr = s + " AND Attraction_Duration <= " + CurrenttotalMinutes + " AND Attraction_OpeningHour <= #" + (DateTime.Parse(StartDayTime)).AddMinutes(DaytotalMinutes - CurrenttotalMinutes).ToString("yyyy-MM-dd HH:mm:ss") + "#"//שעת פתיחה קטנה מהזמן כרגע
                        + " AND (TimeDiffMinutes) >= Attraction_Duration ";
                    try // מניעת שגיאה במקרה והסינון לא מחזיר כלום
                    {
                        filteredAttractionTable = AttractionTable.Select(FilterStr).CopyToDataTable(); // סינון על טבלת אטרקציות
                    }
                    catch (InvalidOperationException e2)// אם אין אטרקציות התואמות את הסינונים = נגמר המסלול
                    {
                        break;// יציאה מלולאת היום
                    }

                    //סידור האטרקציות לפי האטרקציה הקרובה ביותר לפי מסלול האטרקציות לאטרקציה הקודמת במסלול - אם יש תיקו בוחר אחד
                    filteredAttractionTable.Columns.Add("AbsValue", typeof(int)); // הוספת עמודה לאכסון תוצאות חיסור מיקום האטרקציה ממיקום האטרקציה הקודמת
                    foreach (DataRow row in filteredAttractionTable.Rows) // השמת תוצאות החיסור בעמודה שהוספה בדאטאסט
                    {
                        if (row.RowState != DataRowState.Deleted)
                        {
                            row["AbsValue"] = Math.Abs((int)row["Attraction_PathOrder"] - (int)(((Attraction)Path[(Path.Count) - 1]).Attraction_PathOrder));
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

                    Path.Add(await OrderService.Transport((Attraction)Path[(Path.Count) - 1], Nextattractioninpath)); // זמן הגעה מנקודת האטרקציה הקודמת לאטרקציה החדשה - OrderService.Transport(FromAttraction, ToAttraction));
                    Path.Add(Nextattractioninpath);// האטרקציה הבאה במסלול
                    CurrenttotalMinutes -= (int)((Transportation)Path[pathcount]).TravelTime; // חיסור זמן האטרקציה וזמן ההגעה אליה מנק היציאה  - עוגל לדקות
                    pathcount++;
                    CurrenttotalMinutes -= ((Attraction)Path[pathcount]).Attraction_Duration; // חיסור משך האטרקציה
                }

                //העברה למסד נתונים
                int MinutesFromBeginningDay = 0;// מספר הדקות מתחילת היום - לכל איבר - לאחר כל איבר שהוספה למסד מתעדכן
                for (int k = 0; k < Path.Count; k++)
                {
                    if (Path[k] is Transportation)//זמן הגעה
                    {
                        int Transportationtraveltime = (int)(((Transportation)Path[k]).TravelTime); //משך הנסיעה
                        Connect.Connect_ExecuteNonQuery("INSERT INTO Day_Transportation (Day_ID, StartHour, EndHour, FromAttraction, ToAttraction, TravelType) " +
                            "VALUES (" + DayCode + ", '" + DateTime.Parse(StartDayTime).AddMinutes(MinutesFromBeginningDay).ToString("yyyy-MM-dd HH:mm:ss") +
                            "', '" +DateTime.Parse(StartDayTime).AddMinutes(MinutesFromBeginningDay + Transportationtraveltime).ToString("yyyy-MM-dd HH:mm:ss") + "', '" +
                            ((Transportation)Path[k]).FromAttraction.Attraction_ID + "', '" + ((Transportation)Path[k]).ToAttraction.Attraction_ID + "', '" + ((Transportation)Path[k]).TravelWay + "')");
                        MinutesFromBeginningDay += Transportationtraveltime;
                    }
                    else//אטרקציה
                    {
                        int AttractionDuration = (int)(((Attraction)Path[k]).Attraction_Duration); // משך האטרקציה
                        Connect.Connect_ExecuteNonQuery("INSERT INTO Day_Attraction (Day_ID, StartHour, EndHour, Attraction_ID )" +
                            " VALUES (" + DayCode + ", '" + (DateTime.Parse(StartDayTime)).AddMinutes(MinutesFromBeginningDay) + "', '" +
                            (DateTime.Parse(StartDayTime)).AddMinutes(MinutesFromBeginningDay + AttractionDuration) + "', " + (int)(((Attraction)Path[k]).Attraction_ID) +" )");
                        MinutesFromBeginningDay += AttractionDuration;
                    }
                }
            }






            //To find the closest number to a specified number in an Access query, you can use the Abs() function in a SQL query to compute
            //the absolute difference between each number in your data and the target number, and then find the smallest of these differences.
            //האטרקציה הקרובה ביותר לאטרקציה אחרת במסלול
            // מחשב את הערך המוחלט של כל סדר מסלול אטרקציה ממיקום סדר האטרקציה הקודמת 
            //SELECT TOP 1 *
            //FROM Attraction
            //WHERE NOT Attraction_PathOrder = 1
            //ORDER BY ABS(Attraction_PathOrder - 1) ASC




            DataTable OriginalTable = ds.Tables["Attraction"];
            int j = 2;
            string AttractionType;
            int PathCount;

            //  להוסיף שעומד בשעות פתיחה סגירה
            // לעגל שעות נסיעה
            //    bool IsAddToPath = false, AreThereMoreAttractions = true;
            //    while (AllDayTime < DayLongTime - 1 && !AreThereMoreAttractions) // טווח זמן של שעה לפני סיום היום
            //    {
            //        for (int i = 0; i < selectedItems.Count && !IsAddToPath; i++)
            //        {
            //            string x = selectedItems[i].ToString();
            //            if (((Attraction)Path[Path.Count - 1]).Attraction_TypeID != x)
            //            {
            //                AttractionType = x;
            //                // סיום הIF 
            //                PathCount = Path.Count;
            //                Path = AddAttractionToPath(OriginalTable, Path, j, AttractionType);
            //                if (PathCount != Path.Count)
            //                {
            //                    AllDayTime += ((Transportation)Path[j + 1]).TravelTime;
            //                    AllDayTime += Convert.ToDouble(((Attraction)Path[j + 2]).Attraction_Duration);
            //                    j += 2;
            //                    IsAddToPath = true;
            //                }

            //            }
            //        }
            //        if (!IsAddToPath)
            //        {
            //            PathCount = Path.Count;
            //            Path = AddAttractionToPath(OriginalTable, Path, j, ((Attraction)Path[Path.Count - 1]).Attraction_TypeID);
            //            if (PathCount != Path.Count)
            //            {
            //                AllDayTime += ((Transportation)Path[j + 1]).TravelTime;
            //                AllDayTime += Convert.ToDouble(((Attraction)Path[j + 2]).Attraction_Duration);
            //                j += 2;
            //                IsAddToPath = true;
            //            }
            //            else
            //                AreThereMoreAttractions = false;

            //        }
            //        IsAddToPath = false;
            //        AttractionType = null;
            //    }
            //}

            //public static ArrayList AddAttractionToPath(DataTable OriginalTable, ArrayList Path, int j, string AttractionType)
            //{
            //    DataTable t = OriginalTable;
            //    string s = "WHERE Attraction_ID NOT IN(SELECT Attraction_ID FROM Day_Attraction) AND NOT Attraction_PathOrder = "
            //        + ((Attraction)Path[j]).Attraction_ID + "ORDER BY ABS(Attraction_PathOrder - " + ((Attraction)Path[j]).Attraction_ID +
            //        ") AND Attraction_Type = " + AttractionType;
            //    t = t.Select(s).CopyToDataTable();
            //    DataRow row = t.Rows[0];
            //    if (row != null) // אם לא נגמרו 
            //    {
            //        Attraction NextAttractionInPath = new Attraction((String)row["Attraction_Name"], (String)row["Attraction_TypeID"], (String)row["Attraction_TypeName"],
            //            (String)row["Attraction_MinAge"], (String)row["Attraction_MaxAge"], (String)row["Attraction_Price"], (String)row["Attraction_Duration"],
            //            (String)row["Attraction_Address"], (String)row["Attraction_Gmail"], (String)row["Attraction_PhonNumber"], (String)row["Attraction_recommendedMonth"],
            //            (String)row["Attraction_FreeEntry"], (String)row["Attraction_Text"], (String)row["Attraction_Photo"], (double)row["Attraction_Latitude"],
            //            (double)row["Attraction_Longitude"], (int)row["Attraction_PathOrder"]);

            //        Path.Add(OrderService.Transport((Attraction)Path[j], NextAttractionInPath)); // זמן ההדעה מנקודת ההתחלה לאטרקציה הראשונה
            //        Path.Add(NextAttractionInPath);
            //    }
            //    return Path;
            //}



            //protected async void Create_Click(object sender, EventArgs e)
            //{
            //    DataSet ds = (DataSet)Session["PreferneceDS"]; // לקיחת הסשן
            //    // מחרוזת לפי יודעים בפעם הראשונה מה נבחר + הוספה לארייליסט
            //    string s = "Attraction_Type = ";
            //    ArrayList selectedItems = new ArrayList();
            //    for (int i = 0; i < DataList1.Items.Count; i++)
            //    {
            //        CheckBoxList checkBoxList = (CheckBoxList)DataList1.Items[i].FindControl("CheckBoxList1"); // עבור כל צקבוקסליסט בכל אייטם
            //        if (checkBoxList != null)
            //        {
            //            foreach (ListItem item in checkBoxList.Items)
            //            {
            //                // If the item is selected, add it to the ArrayList
            //                if (item.Selected)
            //                {
            //                    selectedItems.Add(item.Value);// הוספה למערך - יכול להיות שהמערך ללא שימוש
            //                    s += item.Value + " OR Attraction_Type= ";// הוספה למחרוזת כל סוגי סוגי האטרקציות המועדפות
            //                }
            //            }
            //        }
            //    }

            //    //ואז עושים לו אורך selectedItems בדיקה אם נבחר רק סוג אטרקציה אחד- ניתן לוותר אם משאירים את הארייליסט 
            //    string searchSubstring = " OR Attraction_Type= ";
            //    int count = 0;
            //    int index = 0;

            //    while ((index = s.IndexOf(searchSubstring, index)) != -1)
            //    {
            //        index += searchSubstring.Length;
            //        count++;
            //    }
            //    bool IsOnlyOneSelected = count == 1;


            //    ArrayList Path = new ArrayList();
            //    Attraction FromAttraction = new Attraction(48.86043549, 2.34166503), ToAttraction = new Attraction(48.8587974, 2.2946038);
            //    s = s.Substring(0, s.Length - 21);
            //    // s+= " and duration between Attraction.Duration and Attraction.Duration + 1/6 (10 דקות)"
            //    //ArrayList StratLocationcoordinates = await BingMapsGeocoder.GetCoordinatesByAddressAsync(StartPlace.Text); // הפיכת כתובת ההתחלה לקורדינאטות
            //    //Attraction StratLocation = new Attraction(Convert.ToDouble(StratLocationcoordinates[0]), Convert.ToDouble(StratLocationcoordinates[1])); // יצירת עצם אטרקציה שמכיל את המיקום ההתחלתי של המסלול - המקום ממנו יוצאים למסלול
            //    //להוסיף גם שתואם את הזמן של היום וגם את זמן האטרקציה
            //    //Attraction closestAttraction = OrderService.FindClosestAttraction(ds.Tables[0].Select(s).CopyToDataTable(), StratLocation); // מציאת האטרציה הקרובה ביותר למיקום ההתחלתי - האטרקציה הראשונה במסלול - לפי מחרוזת שמקבל
            //    //Path.Add(StratLocation); // המיקום ההתחלתי של היום - ממנו יוצאים למסלול
            //    Path.Add(await OrderService.Transport(FromAttraction, ToAttraction)); // זמן ההדעה מנקודת ההתחלה לאטרקציה הראשונה
            //    //Path.Add(closestAttraction);// האטרקציה הראשונה
            //    double AllDayTime = ((Transportation)Path[1]).TravelTime;
            //    AllDayTime += Convert.ToDouble(((Attraction)Path[2]).Attraction_Duration);

            //    double DayLongTime = Convert.ToDouble(EndDayTime.Text) - Convert.ToDouble(StartDayTime.Text);

            //    ////הצגת כל האטרקציות שלא נמצאות כבר במסלול
            //    //SELECT*
            //    //FROM Attraction
            //    //WHERE Attraction_ID NOT IN(SELECT Attraction_ID FROM Day_Attraction)
            //    //ORDER BY Attraction_PathOrder ASC;

            //    ////הצגת האטרקציה הקרובה ביותר לאטרקציה הראשונה במסלול - שינוי המספר 1 לפי הסדר בתור
            //    //SELECT TOP 1 *
            //    //FROM Attraction
            //    //WHERE NOT Attraction_PathOrder = 1
            //    //ORDER BY ABS(Attraction_PathOrder - 1)


            //    //שילוב של שניהם:
            //    //SELECT TOP 1 *
            //    //FROM Attraction
            //    //WHERE Attraction_ID NOT IN(SELECT Attraction_ID FROM Day_Attraction)
            //    //AND NOT Attraction_PathOrder = 1
            //    //ORDER BY ABS(Attraction_PathOrder - 1);

            //    DataTable OriginalTable = ds.Tables["Attraction"];
            //    int j = 2;
            //    string AttractionType;
            //    int PathCount;

            //    //  להוסיף שעומד בשעות פתיחה סגירה
            //    // לעגל שעות נסיעה
            //    bool IsAddToPath = false, AreThereMoreAttractions = true;
            //    while (AllDayTime < DayLongTime - 1 && !AreThereMoreAttractions) // טווח זמן של שעה לפני סיום היום
            //    {
            //        for (int i = 0; i < selectedItems.Count && !IsAddToPath; i++)
            //        {
            //            string x = selectedItems[i].ToString();
            //            if (((Attraction)Path[Path.Count - 1]).Attraction_TypeID != x)
            //            {
            //                AttractionType = x;
            //                // סיום הIF 
            //                PathCount = Path.Count;
            //                Path = AddAttractionToPath(OriginalTable, Path, j, AttractionType);
            //                if (PathCount != Path.Count)
            //                {
            //                    AllDayTime += ((Transportation)Path[j + 1]).TravelTime;
            //                    AllDayTime += Convert.ToDouble(((Attraction)Path[j + 2]).Attraction_Duration);
            //                    j += 2;
            //                    IsAddToPath = true;
            //                }

            //            }
            //        }
            //        if (!IsAddToPath)
            //        {
            //            PathCount = Path.Count;
            //            Path = AddAttractionToPath(OriginalTable, Path, j, ((Attraction)Path[Path.Count - 1]).Attraction_TypeID);
            //            if (PathCount != Path.Count)
            //            {
            //                AllDayTime += ((Transportation)Path[j + 1]).TravelTime;
            //                AllDayTime += Convert.ToDouble(((Attraction)Path[j + 2]).Attraction_Duration);
            //                j += 2;
            //                IsAddToPath = true;
            //            }
            //            else
            //                AreThereMoreAttractions = false;

            //        }
            //        IsAddToPath = false;
            //        AttractionType = null;
            //    }
        }
    }
}

