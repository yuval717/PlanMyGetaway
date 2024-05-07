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

namespace Project_01
{
    public partial class Order : System.Web.UI.Page
    {

        public int totalDays; // לצורך שימוש במספר פעולות
        public ArrayList orderarr; // לצורך שימוש במספר פעולות
        public string DaysDate = "";// לצורך שימוש במספר פעולות
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
                    arr.Add(new DS_Object("SELECT * FROM Attraction", "Attraction")); // אטרקציות
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
                //DateTime startDate = Convert.ToDateTime(orderarr[0]);//מתאריך
                //DateTime endDate = Convert.ToDateTime(orderarr[1]);//לתאריך
                //totalDays = (endDate - startDate).Days + 1; // מספר ימי המסלול
                totalDays = 4; // מספר ימי המסלול
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
                    //***
                    ArrayList orderarr = new ArrayList();//רק לבדיקה
                    orderarr.Add("2024-05-10");
                    //***

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


        //הוספת ערכים לצקבוקסלסט לפי סוג חופשה
        protected void DataList1_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //DataSet על כל איבר(אייטם-שורה מהטייבל) שנוצר ב -Bind רץ כל פעם שמתבצע DataBind ה 

                // כשיהיה שדה ואליד לשנות לספירת אלה שואליד וירוץ גם על אלה שואליד
                //סוג חופשה שנלחץ ID הוספת אטרציות לפי
                DataSet ds = (DataSet)Session["PreferneceDS"]; // טבלת סוגי חופשה
                int code = (int)ds.Tables[0].Rows[e.Item.ItemIndex]["VacationType_ID"]; //מתבצע עבור כל שורה - כל סוג חופשה- DataList- סוג חופשה - לפי ריצה על השורות  ID שמירת ה
                foreach (DataRow row in ds.Tables["AttractionType"].Rows)// ריצה על טבלת סוגי אטרקציות
                {
                    if ((int)row["VacationType_ID"] == code) //של סוג החופשה ID של האטרציה זהה ל ID בדיקה האם ה
                    {
                        //משלו המכיךל ערכים לפי סוג החופשה שלו CheckBoxList , Item לכל -  CheckBoxList הוספת איבר ל
                        ((CheckBoxList)e.Item.FindControl("CheckBoxList1")).Items.Add(new ListItem(row["AttractionType_Type"].ToString(), row["AttractionType_ID"].ToString()));
                        //((CheckBoxList)DataList1.Items[e.Item.ItemIndex].FindControl("CheckBoxList1")).Items.Add(new ListItem(row1["AttractionType_Type"].ToString(), row1["AttractionType_ID"].ToString()));
                    }
                }
            }
        }


        // להוסיף כפתור אישור לבחירת ההעדפות - לוודא שנבחר לפחות העדפה אחת********** או לוודא בכפתור יצירה ולפסול

        protected async void Create_Click(object sender, EventArgs e)
        {
            // יצירת הזמנה
            Connect.Connect_ExecuteNonQuery("INSERT INTO Orders (Order_DaysNumber, Order_StratDate, Order_MinAge, Order_MaxAge, Order_Name," +
                " Order_UserName, Order_AddDate )VALUES(" + "מספר הימים" + ", " + "תאריך התחלה" + ", " + " גיל מינמלי" + ", " + "גיל מקסימלי" +
                ", " + ", " + "שם הזמנה" + ", " + "מפתח ראשי מתשמש" + ", " + DateTime.Now + "); ");
            int OrderCode = (int)Connect.Connect_ExecuteScalar("Select Max(Order_ID) From Orders"); // שליפת הערך האחרון של המפתח רץ - קוד ההזמנה שיצרנו

            DataSet ds = (DataSet)Session["PreferneceDS"]; // לקיחת הסשן

            //  מחרוזת בחירות העדפות משתמש + הוספה לארייליסט 
            string s = "SELECT Attraction_ID, Attraction_Type, Attraction_MinAge, Attraction_MaxAge, Attraction_Latitude, Attraction_Longitude," +
                " Attraction_PathOrder, Attraction_Duration FROM Attraction WHERE Attraction_Type = ";
            ArrayList selectedItems = new ArrayList();
            for (int i = 0; i < DataList1.Items.Count; i++)
            {
                if (AttractionTypePreference != null)// לא יקרה כי נוודא בכפתור אישור בחירת העדפות - אבל בכל זאת
                {
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
            }
            s = s.Substring(0, s.Length - 21);// החסרת "OR Attraction_Type= "


            string OrderDate = "";//לפי תאריך המסלול - לוקחים מבדאטא ליסט
            // מסלול יומי - לולאה

            string StartDayTime = ((TextBox)DayPreferences.Items[1].FindControl("StartDayTime")).Text;
            string EndDayTime=((TextBox) DayPreferences.Items[1].FindControl("EndDayTime")).Text;

            Connect.Connect_ExecuteNonQuery("INSERT INTO Day (Day_StartHour, Day_EndHour, Order_ID, Day_Date ...) VALUES( " + StartDayTime + ", " + EndDayTime + ", " + OrderCode + ", " + OrderDate + "); ");// יצירת יום
            int DayCode = (int)Connect.Connect_ExecuteScalar("Select Max(Day_ID) From Day"); // שליפת הערך האחרון של המפתח רץ - קוד היום שיצרנו

            ;

            // חישוב הזמן בדקות של הזמן שהוכנס משעה עד שעה
            string time1 = EndDayTime; // "12:30"
            string time2 = StartDayTime; // "10:15"

            // Parse times into TimeSpan objects
            TimeSpan t1 = TimeSpan.Parse(time1);
            TimeSpan t2 = TimeSpan.Parse(time2);

            // Subtract times
            TimeSpan difference = t1 - t2;

            // Get the total difference in minutes
            int totalMinutes = (int)difference.TotalMinutes;

            string FilteringStr = s + "AND Attraction_Duration <= " + totalMinutes + "AND Attraction.Attraction_ID NOT IN( SELECT Attraction_ID FROM Day_Attraction WHERE Day_Attraction.Day_ID " + // כל מה שכבר לא מופיע בהזמנה קיימת
                    "IN(SELECT Day_ID FROM Day WHERE Order_ID = " + OrderCode + ")) "; //  לפני ביצוע סינון totalMinutes צריך להתשנות לאחר כל הוספה למסלול - לחסר את

            DataTable filteredAttractionTable; // סינון דאטא ליסט אטרקציות
            try // מניעת שגיאה במקרה והסינון לא מחזיר כלום
            {
                filteredAttractionTable = ds.Tables["Attraction"].Select(s).CopyToDataTable();
            }
            catch (InvalidOperationException e2)
            {
                filteredAttractionTable = null;
            }

            if (filteredAttractionTable == null) // אם אין אטרקציות התואמות את הסינונים = נגמר המסלול
            {
                //break;// יציאה מלולאת 
            }

            ArrayList Path = new ArrayList();
            Connect.Connect_ExecuteNonQuery("INSERT INTO Day_Attraction (Day_ID , IsTransportation , StartLocation_Address, ...) VALUES(" + DayCode + " , FALSE, " + /*StartPlace*/"" + "); ");// הוספת המיקום ההתחלתי למסד
            ArrayList StratLocationcoordinates = await BingMapsGeocoder.GetCoordinatesByAddressAsync(/*StartPlace.Text*/""); // הפיכת כתובת ההתחלה לקורדינאטות
            Attraction StratLocation = new Attraction(Convert.ToDouble(StratLocationcoordinates[0]), Convert.ToDouble(StratLocationcoordinates[1])); //  יצירת עצם אטרקציה שמכיל את המיקום ההתחלתי של המסלול - המקום ממנו יוצאים למסלול לטובת מציאת האטרקציה הראשונה
            Attraction closestAttraction = OrderService.FindClosestAttraction(filteredAttractionTable, StratLocation); // מציאת האטרציה הקרובה ביותר למיקום ההתחלתי - האטרקציה הראשונה במסלול - לפי מחרוזת שמקבל

            // המיקום ההתחלתי של היום מוסף ישירות למסד - חלק המסלול הראשון במסלול - הזמן בין נק התחלה לאטרקציה ראשונה
            Path.Add(await OrderService.Transport(StratLocation, closestAttraction)); // זמן הגעה מנקודת ההתחלה לאטרקציה הראשונה - OrderService.Transport(FromAttraction, ToAttraction));
            Path.Add(closestAttraction);// האטרקציה הראשונה
            totalMinutes -= (int)((Transportation)Path[1]).TravelTime; // חיסור זמן האטרקציה וזמן ההגעה אליה מנק היציאה  - עוגל לדקות
            totalMinutes -= ((Attraction)Path[2]).Attraction_Duration; // חיסור משך האטרקציה


            ////הצגת כל האטרקציות שלא נמצאות כבר במסלול
            //SELECT*
            //FROM Attraction
            //WHERE Attraction_ID NOT IN(SELECT Attraction_ID FROM Day_Attraction)
            //ORDER BY Attraction_PathOrder ASC;

            ////הצגת האטרקציה הקרובה ביותר לאטרקציה הראשונה במסלול - שינוי המספר 1 לפי הסדר בתור
            //SELECT TOP 1 *
            //FROM Attraction
            //WHERE NOT Attraction_PathOrder = 1
            //ORDER BY ABS(Attraction_PathOrder - 1)


            //שילוב של שניהם:
            //SELECT TOP 1 *
            //FROM Attraction
            //WHERE Attraction_ID NOT IN(SELECT Attraction_ID FROM Day_Attraction)
            //AND NOT Attraction_PathOrder = 1
            //ORDER BY ABS(Attraction_PathOrder - 1);

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

