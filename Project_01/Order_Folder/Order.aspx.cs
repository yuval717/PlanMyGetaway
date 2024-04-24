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
        protected void Page_Load(object sender, EventArgs e)
        {
            //DataSet השמת 
            DataSet ds = null;
            if (!IsPostBack)
            {


                if (Session["PreferneceDS"] == null)
                {
                    //DataSet הוספת טבלאות ל
                    ArrayList arr = new ArrayList();
                    arr.Add(new DS_Object("SELECT * FROM VacationType", "VacationType"));
                    arr.Add(new DS_Object("SELECT * FROM AttractionType", "AttractionType"));
                    arr.Add(new DS_Object("SELECT * FROM Attraction", "Attraction"));
                    arr.Add(new DS_Object("SELECT * FROM Day_Attraction", "Day_Attraction"));
                    ds = Connect.Connect_MultipleDataSet(arr);// יצירת דטאסט המכיל כמה טבלאות
                    Session["PreferneceDS"] = ds;

                }
                else
                    ds = (DataSet)Session["PreferneceDS"];

                DataList1.DataSource = ds;
                DataList1.DataBind();

            }

        }

        // הוספת ערכים לצקבוקסלסט לפי סוג חופשה
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

        protected async void Create_Click(object sender, EventArgs e)
        {
            DataSet ds = (DataSet)Session["PreferneceDS"]; // לקיחת הסשן
            // מחרוזת לפי יודעים בפעם הראשונה מה נבחר + הוספה לארייליסט
            string s = "Attraction_Type = ";
            ArrayList selectedItems = new ArrayList();
            for (int i = 0; i < DataList1.Items.Count; i++)
            {
                CheckBoxList checkBoxList = (CheckBoxList)DataList1.Items[i].FindControl("CheckBoxList1"); // עבור כל צקבוקסליסט בכל אייטם
                if (checkBoxList != null)
                {
                    foreach (ListItem item in checkBoxList.Items)
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

            //ואז עושים לו אורך selectedItems בדיקה אם נבחר רק סוג אטרקציה אחד- ניתן לוותר אם משאירים את הארייליסט 
            string searchSubstring = " OR Attraction_Type= ";
            int count = 0;
            int index = 0;

            while ((index = s.IndexOf(searchSubstring, index)) != -1)
            {
                index += searchSubstring.Length;
                count++;
            }
            bool IsOnlyOneSelected = count == 1;


            ArrayList Path = new ArrayList();
            Attraction FromAttraction = new Attraction(48.86043549, 2.34166503), ToAttraction = new Attraction(48.8587974, 2.2946038);
            s = s.Substring(0, s.Length - 21);
            // s+= " and duration between Attraction.Duration and Attraction.Duration + 1/6 (10 דקות)"
            //ArrayList StratLocationcoordinates = await BingMapsGeocoder.GetCoordinatesByAddressAsync(StartPlace.Text); // הפיכת כתובת ההתחלה לקורדינאטות
            //Attraction StratLocation = new Attraction(Convert.ToDouble(StratLocationcoordinates[0]), Convert.ToDouble(StratLocationcoordinates[1])); // יצירת עצם אטרקציה שמכיל את המיקום ההתחלתי של המסלול - המקום ממנו יוצאים למסלול
            //להוסיף גם שתואם את הזמן של היום וגם את זמן האטרקציה
            //Attraction closestAttraction = OrderService.FindClosestAttraction(ds.Tables[0].Select(s).CopyToDataTable(), StratLocation); // מציאת האטרציה הקרובה ביותר למיקום ההתחלתי - האטרקציה הראשונה במסלול - לפי מחרוזת שמקבל
            //Path.Add(StratLocation); // המיקום ההתחלתי של היום - ממנו יוצאים למסלול
            Path.Add(await OrderService.Transport(FromAttraction, ToAttraction)); // זמן ההדעה מנקודת ההתחלה לאטרקציה הראשונה
            //Path.Add(closestAttraction);// האטרקציה הראשונה
            double AllDayTime = ((Transportation)Path[1]).TravelTime;
            AllDayTime += Convert.ToDouble(((Attraction)Path[2]).Attraction_Duration);

            double DayLongTime = Convert.ToDouble(EndDayTime.Text) - Convert.ToDouble(StartDayTime.Text);

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
            bool IsAddToPath = false, AreThereMoreAttractions = true;
            while (AllDayTime < DayLongTime - 1 && !AreThereMoreAttractions) // טווח זמן של שעה לפני סיום היום
            {
                for (int i = 0; i < selectedItems.Count && !IsAddToPath; i++)
                {
                    string x = selectedItems[i].ToString();
                    if (((Attraction)Path[Path.Count - 1]).Attraction_TypeID != x)
                    {
                        AttractionType = x;
                        // סיום הIF 
                        PathCount = Path.Count;
                        Path = AddAttractionToPath(OriginalTable, Path, j, AttractionType);
                        if (PathCount != Path.Count)
                        {
                            AllDayTime += ((Transportation)Path[j + 1]).TravelTime;
                            AllDayTime += Convert.ToDouble(((Attraction)Path[j + 2]).Attraction_Duration);
                            j += 2;
                            IsAddToPath = true;
                        }

                    }
                }
                if (!IsAddToPath)
                {
                    PathCount = Path.Count;
                    Path = AddAttractionToPath(OriginalTable, Path, j, ((Attraction)Path[Path.Count - 1]).Attraction_TypeID);
                    if (PathCount != Path.Count)
                    {
                        AllDayTime += ((Transportation)Path[j + 1]).TravelTime;
                        AllDayTime += Convert.ToDouble(((Attraction)Path[j + 2]).Attraction_Duration);
                        j += 2;
                        IsAddToPath = true;
                    }
                    else
                        AreThereMoreAttractions = false;

                }
                IsAddToPath = false;
                AttractionType = null;
            }
        }

        public static ArrayList AddAttractionToPath(DataTable OriginalTable, ArrayList Path, int j, string AttractionType)
        {
            DataTable t = OriginalTable;
            string s = "WHERE Attraction_ID NOT IN(SELECT Attraction_ID FROM Day_Attraction) AND NOT Attraction_PathOrder = "
                + ((Attraction)Path[j]).Attraction_ID + "ORDER BY ABS(Attraction_PathOrder - " + ((Attraction)Path[j]).Attraction_ID +
                ") AND Attraction_Type = " + AttractionType ;
            t = t.Select(s).CopyToDataTable();
            DataRow row = t.Rows[0];
            if (row != null) // אם לא נגמרו 
            {
                Attraction NextAttractionInPath = new Attraction((String)row["Attraction_Name"], (String)row["Attraction_TypeID"], (String)row["Attraction_TypeName"],
                    (String)row["Attraction_MinAge"], (String)row["Attraction_MaxAge"], (String)row["Attraction_Price"], (String)row["Attraction_Duration"],
                    (String)row["Attraction_Address"], (String)row["Attraction_Gmail"], (String)row["Attraction_PhonNumber"], (String)row["Attraction_recommendedMonth"],
                    (String)row["Attraction_FreeEntry"], (String)row["Attraction_Text"], (String)row["Attraction_Photo"], (double)row["Attraction_Latitude"],
                    (double)row["Attraction_Longitude"], (int)row["Attraction_PathOrder"]);

                Path.Add(OrderService.Transport((Attraction)Path[j], NextAttractionInPath)); // זמן ההדעה מנקודת ההתחלה לאטרקציה הראשונה
                Path.Add(NextAttractionInPath);
            }
            return Path;
        }


            protected void Button2_Click(object sender, EventArgs e)
        {
            Calendar1.Visible = false;
            Button2.Visible = false;
            DataList1.Visible = true;
            Create.Visible = true;
            Button3.Visible = true;
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            Calendar1.Visible = true;
            Button2.Visible = true;
            DataList1.Visible = false;
            Create.Visible = false;
            Button3.Visible = false;
        }
    }
}
