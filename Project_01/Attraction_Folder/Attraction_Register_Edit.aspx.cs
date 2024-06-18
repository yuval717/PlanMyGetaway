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
using System.Text.RegularExpressions;

namespace Project_01
{
    public partial class Attraction_Register_Edit : System.Web.UI.Page
    {
        public static string UserName ; // לשנות לסשן משתמש
        public static DataTable PhotosTable = new DataTable(); // טבלת תמונות נוספות
        public static DataTable MainPhotoTable = new DataTable(); // טבלת תמונה ראשית - לטובת דאטאליסט הצגה 
        public static DataTable RemovePhotoTable = new DataTable(); // טבלת תמונות נוספות למחיקה מהמסד
        public static DataTable AddPhotoTable = new DataTable(); // טבלת תמונות נוספות להוספה
        public static DataTable RemovePhotoFileFromServer = new DataTable(); // טבלת תמונות נוספות למחיקה מהמסד
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                MasterPage_UserName.Text = ((User)Session["User"]).User_Name;
                string l = ((User)Session["User"]).User_Type;
                if (((User)Session["User"]).User_Type == "בעל עסק")//בעל עסק
                {
                    //מאסטר פייג
                    Site master = (Site)this.Master;
                    master.MasterPageSignUpOut.Text = "התנתק";
                    master.MasterPageOrders.Visible = false;
                    master.MasterPageSignUpOut.CommandName = "/Homepage"; //התנתקות
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

                DataSet ds;// יצירת דאטאסט
                UserName = ((User)Session["User"]).User_Name;// אכלוס שם המשתמש

                // שמירת דאטאסט סוגי אטרקציות וחופשות וסשן
                if (Session["AttractionType_Type"] == null)
                {
                    ds = Connect.Connect_DataSet("SELECT AttractionType_Type,AttractionType_ID,VacationType_ID FROM AttractionType WHERE IsValid = "+true, "AttractionType_Type");
                    Session["AttractionType_Type"] = ds;
                }
                else
                    ds = (DataSet)Session["AttractionType_Type"];

                /*הוספת ערכי סוג אטרקציה לדרופדאון ליסט*/
                foreach (DataRow row in ds.Tables["AttractionType_Type"].Rows) // הוספת ערכים לדרופדאוןליסט
                {
                    Attraction_Type.Items.Add(new ListItem(row["AttractionType_Type"].ToString(), row["AttractionType_ID"].ToString()));
                }

                 PhotosTable = new DataTable(); // טבלת תמונות נוספות
                 MainPhotoTable = new DataTable(); // טבלת תמונה ראשית - לטובת דאטאליסט הצגה 
                 RemovePhotoTable = new DataTable(); // טבלת תמונות נוספות למחיקה מהמסד
                 AddPhotoTable = new DataTable(); // טבלת תמונות נוספות להוספה
                 RemovePhotoFileFromServer = new DataTable(); // טבלת תמונות נוספות למחיקה מהמסד
                    //הוספת עמודה לטבלת "תמונות נוספות" המכילה את מיקום הקובץ 
                    PhotosTable.Columns.Add("FileLocation", typeof(string));
                    PhotosTable.Columns.Add("מזהה", typeof(int)); // מזהה תמונה - נועד למחיקת תמונות שכבר הוכנסו למסד
                                                                  //הוספת עמודה לטבלת "תמונה ראשית" המכילה את מיקום הקובץ 
                    MainPhotoTable.Columns.Add("FileLocation", typeof(string));

                    ////הוספת עמודה לטבלת "מחיקת תמונות נוספות" - עדכון פרטים - המכילה את מיקום הקובץ 
                    RemovePhotoTable.Columns.Add("FileLocation", typeof(string));
                    RemovePhotoTable.Columns.Add("מזהה", typeof(int)); // מזהה תמונה - נועד למחיקת תמונות שכבר הוכנסו למסד

                    //הוספת עמודה לטבל "הוספת תמונות נוספות" - עדכון פרטים
                    AddPhotoTable.Columns.Add("FileLocation", typeof(string));

                    //******* בהוספת אטרקציה: תמונות נוספות משמש להצגה והוספה מסד הנתונים
                    //*******בעדכון פרטי אטרקציה: תמונות נוספות משמשת רק להצגת התמנות בעוג הוספה ומחיקה לעדכון במסד 


                    ////הוספת עמודה לטבלת "מחיקת תמונות השרת" - בעת לחיצה על כפתור יצירה/עדכון - המכילה את מיקום הקובץ 
                    RemovePhotoFileFromServer.Columns.Add("FileLocation", typeof(string));
                
                

                //בדיקה האם : הוספת אטרקציה או עריכית פרטי אטרקציה
                //אם עריכת פרטי אטרקציה ישנה את נתוני העמוד
                if (Session["Attraction_ID_EditAtt"] !=null)//אם הסשן מכיל פרטי אטרקציה לעריכה
                {

                    //שינוי תצוגת חלק מהפקדים
                    AttractionRegister_Lable.Text = "עריכת פרטי אטרקציה";
                    AttractionRegister_Lable.Style["right"] = "36.75%";
                    Register.Style["display"] = "none";
                    Edit.Style["display"] = "block";

                    //הכנסת פרטי אטרקציה לפקדים
                    // שמירת פרטי אטרקציה בעצם מסוג אטרקציה הנשמר בסשן 
                    Attraction a = AttractionService.FillAttraction("SELECT * FROM Attraction INNER JOIN AttractionType ON " +
                        "Attraction.Attraction_Type = AttractionType.AttractionType_ID WHERE Attraction_ID = " + (int)Session["Attraction_ID_EditAtt"]); ;//  TextBox השמת ערכים קיימים של אטרקציה ב
                    Attraction_Name.Text = a.Attraction_Name;
                    Attraction_Type.SelectedValue = a.Attraction_TypeID;//לבדוק אם עובד
                    Attraction_MinAge.Text = a.Attraction_MinAge;
                    Attraction_MaxAge.Text = a.Attraction_MaxAge;
                    Attraction_Price.Text = a.Attraction_Price;
                    Attraction_Duration.Text = a.Attraction_Duration.ToString();
                    Attraction_Address.Text = a.Attraction_Address;//
                    Attraction_Gmail.Text = a.Attraction_Gmail;
                    Attraction_PhonNumber.Text = a.Attraction_PhonNumber;
                    Attraction_Text.Text = a.Attraction_Text;
                    Session["filelocation"] = a.Attraction_Photo;
                    Attraction_OpeningHour.Text = (DateTime.Parse(a.Attraction_OpeningHours)).ToString("HH:mm") ;
                    Attraction_ClosingHour.Text = (DateTime.Parse(a.Attraction_ClosingHours)).ToString("HH:mm");
                    Attraction_KilometersNumber.Text = a.Attraction_KilometersNumber.ToString();
                    if (a.Attraction_Difficulty == null)
                    {
                        Attraction_Difficulty.SelectedIndex = -1;
                    }
                    else
                    {
                        Attraction_Difficulty.SelectedValue = a.Attraction_Difficulty;
                    }
                    

                    //הצגת נתוני הרחבה - טבע
                    if ( a.Attraction_Difficulty != null)
                    {
                        Attraction_Difficulty.Visible = true;
                        Attraction_Difficulty_Lable.Visible = true;
                        Attraction_KilometersNumber.Visible = true;
                        Attraction_KilometersNumber_Label.Visible = true;
                    }

                    DataRow newRow = MainPhotoTable.NewRow();// יצירת שורה חדשה
                    newRow["FileLocation"] = Session["filelocation"].ToString();// השדה שישלח לשמירה בבסיס הנתונים
                    MainPhotoTable.Rows.Add(newRow);//הוספה לדאטאסט

                    // הצגת התמונה הראשית
                    MainPhoto.DataSource = MainPhotoTable;
                    MainPhoto.DataBind();

                    //הצגת התמונות הנוספות
                    DataTable MorePhotosTable = Connect.Connect_DataTable("SELECT * FROM Photos Where Attraction_ID = " + (int)Session["Attraction_ID_EditAtt"]/* מזהה האטרקציה*/, "Photos");// הבאת טבלת תמונות נוספות של האטרקציה
                    foreach (DataRow row in MorePhotosTable.Rows) // ריצה על טבלת תמונות נוספות
                    {
                        DataRow newRow1 = PhotosTable.NewRow();// יצירת שורה חדשה
                        newRow1["FileLocation"] = row["FileLocation"].ToString();// השדה שישלח לשמירה בבסיס הנתונים
                        newRow1["מזהה"] = row["מזהה"].ToString();// השדה שישלח לשמירה בבסיס הנתונים
                        PhotosTable.Rows.Add(newRow1);//הוספה לדאטאסט
                    }
                    MorePhotos.DataSource = PhotosTable;
                    MorePhotos.DataBind();

                }

            }
        }


        private string ValidateAttractionName(string name)
        {
            string pattern = @"^[a-zA-Z0-9\u0590-\u05FF\s]{1,60}$";
            if (string.IsNullOrWhiteSpace(name) || !Regex.IsMatch(name, pattern))
            {
                // Handle invalid attraction name
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

        private string ValidateAddress(string address)
        {
            string pattern = @"^[a-zA-Z0-9\u0590-\u05FF\s,]{1,100}$";
            if (string.IsNullOrWhiteSpace(address) || !Regex.IsMatch(address, pattern))
            {
                // Handle invalid address
                return "שגיאה";
            }
            return "הצלחה";
        }

        private string ValidateAttractionInfo(string info)
        {
            string pattern = @"^[a-zA-Z0-9\u0590-\u05FF\s,.\'""\-]{1,5000}$";
            if (string.IsNullOrWhiteSpace(info) || !Regex.IsMatch(info, pattern))
            {
                // Handle invalid attraction info
                return "שגיאה";
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

        private string ValidatePhoneNumber(string phoneNumber)
        {
            string pattern = @"^(\+972|0)?[2-9]\d{7,8}$|^(\+1|1)?[ -]?(\d{3})[ -]?(\d{3})[ -]?(\d{4})$";
            if (string.IsNullOrWhiteSpace(phoneNumber) || !Regex.IsMatch(phoneNumber, pattern))
            {
                // Handle invalid phone number
                return "שגיאה";
            }
            return "הצלחה";
        }

        private string ValidateEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            if (string.IsNullOrWhiteSpace(email) || !Regex.IsMatch(email, pattern))
            {
                // Handle invalid email
                return "שגיאה";
            }
            return "הצלחה";
        }





        // RegisterAtt הרשמת אטרקציה  - משתמש בפעולה 
        protected async void Register_Click(object sender, EventArgs e)
        {
            //וולידיישן
            bool NoTextError = true;//שגיאות וולידישן
            if (NoTextError && ValidateAttractionName(Attraction_Name.Text) == "שגיאה")
            {
                Result_Att.Text = "הכנס שם אטרקציה תקינה";
                NoTextError = false;
            }
            if (NoTextError && ValidateAge(Attraction_MinAge.Text, Attraction_MaxAge.Text) == "שגיאה")
            {
                Result_Att.Text = "הכנס גיל מינמלי תקין";
                NoTextError = false;
            }
            if (NoTextError && ValidateAge(Attraction_MinAge.Text, Attraction_MaxAge.Text) == "שגיאה")
            {
                Result_Att.Text = "הכנס גיל מקסימלי תקין";
                NoTextError = false;
            }
            if (NoTextError && ValidateAddress(Attraction_Address.Text) == "שגיאה")
            {
                Result_Att.Text = "הכנס כתובת תקינה";
                NoTextError = false;
            }
            if (NoTextError && ValidateAttractionInfo(Attraction_Text.Text) == "שגיאה")
            {
                Result_Att.Text = "הכנס מידע על אטרקציה תקין";
                NoTextError = false;
            }
            if (NoTextError && ValidateOpeningAndClosingHours(Attraction_OpeningHour.Text, Attraction_ClosingHour.Text) == "שגיאה")
            {
                Result_Att.Text = "הכנס שעות פעילות תקינות";
                NoTextError = false;
            }
            if (NoTextError && ValidatePrice(Attraction_Price.Text) == "שגיאה")
            {
                Result_Att.Text = "הכנס מחיר תקין";
                NoTextError = false;
            }
            if (NoTextError && ValidateDuration(Attraction_Duration.Text) == "שגיאה")
            {
                Result_Att.Text = "הכנס משך אטרקציה תקין";
                NoTextError = false;
            }
            if (NoTextError && ValidatePhoneNumber(Attraction_PhonNumber.Text) == "שגיאה")
            {
                Result_Att.Text = "הכנס מספר טלפון תקין";
                NoTextError = false;
            }
            if (NoTextError && ValidateEmail(Attraction_Gmail.Text) == "שגיאה")
            {
                Result_Att.Text = "הכנס אימייל תקין";
                NoTextError = false;
            }

            //אם וולידיישן תקין מבצע את הפעולה
            if (NoTextError)
            {

                //מחיקת תמונות מהשרת
                foreach (DataRow row in RemovePhotoFileFromServer.Rows)// הסרת התמונה מדאטאסט התמונות + מהתקייה במחשב
                {
                    if (row.RowState != DataRowState.Deleted)
                        File.Delete(Server.MapPath(row["filelocation"].ToString()));// מחיקה מהתקייה במחשב
                }

                for (int i = RemovePhotoFileFromServer.Rows.Count - 1; i >= 0; i--)//ניקוי הדאטאטייבל
                {
                    DataRow row = RemovePhotoFileFromServer.Rows[i];
                    if (row.RowState != DataRowState.Deleted)
                        row.Delete();
                }


                // הרשמת אטרקציה
                await AttractionService.RegisterAttAsync(Attraction_Name.Text, Attraction_Type.SelectedValue, Attraction_MinAge.Text,
                    Attraction_MaxAge.Text, Attraction_Price.Text, Attraction_Duration.Text, Attraction_Address.Text,
                    Attraction_Gmail.Text, Attraction_PhonNumber.Text, Attraction_Text.Text, Session["filelocation"].ToString(),
                     Attraction_OpeningHour.Text, Attraction_ClosingHour.Text, UserName /*לשנות לסשן משתמש*/, PhotosTable, Attraction_KilometersNumber.Text,
                     Attraction_Difficulty.SelectedValue);

                Session["Attraction_AttOwner"] = null; //כדי שיעדכן את התצוגה בפעם הבאה שיחזור לדף בעל עסק null  השמת 
                                                       // Move to another page 
                Response.Redirect("Attraction_Owner.aspx"); // העברה לדף בעל עסק
            }
        }


        // הוספת פרטים נוספים על האטרקציה במידה והיא מסוג חופשה טיול בטבע
        protected void Attraction_Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            //אם נבחר סוג חופשה "טבע" בסוג אטרקציה שנבחרה מציג פרטים נוספים 
            foreach (DataRow row in ((DataSet)Session["AttractionType_Type"]).Tables["AttractionType_Type"].Rows)
                if (Attraction_Type.SelectedValue == row["AttractionType_ID"].ToString())
                {
                    if (row["VacationType_ID"].ToString() == "3")
                    {
                        Attraction_Difficulty.Visible = true;
                        Attraction_Difficulty_Lable.Visible = true;
                        Attraction_KilometersNumber.Visible = true;
                        Attraction_KilometersNumber_Label.Visible = true;
                    }
                    else
                    {
                        Attraction_Difficulty.Visible = false;
                        Attraction_Difficulty_Lable.Visible = false;
                        Attraction_KilometersNumber.Visible = false;
                        Attraction_KilometersNumber_Label.Visible = false;

                        Attraction_Difficulty.SelectedIndex = -1;
                        Attraction_KilometersNumber.Text = null;
                    }
                }
        }

        //UpdateAtt  עדכון פרטי אטרקציה  - משתמש בפעולה 
        protected async void Edit_Click(object sender, EventArgs e)
        {
            //וולידיישן
            bool NoTextError = true;//שגיאות וולידישן
            if (NoTextError && ValidateAttractionName(Attraction_Name.Text) == "שגיאה")
            {
                Result_Att.Text = "הכנס שם אטרקציה תקינה";
                NoTextError = false;
            }
            if (NoTextError && ValidateAge(Attraction_MinAge.Text, Attraction_MaxAge.Text) == "שגיאה")
            {
                Result_Att.Text = "הכנס גיל מינמלי תקין";
                NoTextError = false;
            }
            if (NoTextError && ValidateAge(Attraction_MinAge.Text, Attraction_MaxAge.Text) == "שגיאה")
            {
                Result_Att.Text = "הכנס גיל מקסימלי תקין";
                NoTextError = false;
            }
            if (NoTextError && ValidateAddress(Attraction_Address.Text) == "שגיאה")
            {
                Result_Att.Text = "הכנס כתובת תקינה";
                NoTextError = false;
            }
            if (NoTextError && ValidateAttractionInfo(Attraction_Text.Text) == "שגיאה")
            {
                Result_Att.Text = "הכנס מידע על אטרקציה תקין";
                NoTextError = false;
            }
            if (NoTextError && ValidateOpeningAndClosingHours(Attraction_OpeningHour.Text, Attraction_ClosingHour.Text) == "שגיאה")
            {
                Result_Att.Text = "הכנס שעות פעילות תקינות";
                NoTextError = false;
            }
            if (NoTextError && ValidatePrice(Attraction_Price.Text) == "שגיאה")
            {
                Result_Att.Text = "הכנס מחיר תקין";
                NoTextError = false;
            }
            if (NoTextError && ValidateDuration(Attraction_Duration.Text) == "שגיאה")
            {
                Result_Att.Text = "הכנס משך אטרקציה תקין";
                NoTextError = false;
            }
            if (NoTextError && ValidatePhoneNumber(Attraction_PhonNumber.Text) == "שגיאה")
            {
                Result_Att.Text = "הכנס מספר טלפון תקין";
                NoTextError = false;
            }
            if (NoTextError && ValidateEmail(Attraction_Gmail.Text) == "שגיאה")
            {
                Result_Att.Text = "הכנס אימייל תקין";
                NoTextError = false;
            }

            //אם וולידיישן תקין מבצע את הפעולה
            if (NoTextError)
            {
                // מחיקת תמונות מהשרת
                foreach (DataRow row in RemovePhotoFileFromServer.Rows) // הסרת התמונה מדאטאסט התמונות + מהתקייה במחשב
                {
                    if (row.RowState != DataRowState.Deleted) ;
                    File.Delete(Server.MapPath(row["filelocation"].ToString())); // מחיקה מהתקייה במחשב
                }

                for (int i = RemovePhotoFileFromServer.Rows.Count - 1; i >= 0; i--) // ניקוי הדאטאטייבל
                {
                    DataRow row = RemovePhotoFileFromServer.Rows[i];
                    if (row.RowState != DataRowState.Deleted)
                        row.Delete();
                }

                // עדכון אטרקציה
                await AttractionService.UpdateAtt((int)Session["Attraction_ID_EditAtt"], Attraction_Name.Text, Attraction_Type.SelectedValue, Attraction_MinAge.Text,
                    Attraction_MaxAge.Text, Attraction_Price.Text, Attraction_Duration.Text, Attraction_Address.Text,
                    Attraction_Gmail.Text, Attraction_PhonNumber.Text, Attraction_Text.Text, Session["filelocation"].ToString(),
                    Attraction_OpeningHour.Text, Attraction_ClosingHour.Text, Attraction_KilometersNumber.Text,
                    Attraction_Difficulty.SelectedValue, RemovePhotoTable, AddPhotoTable);

                Session["Attraction_AttOwner"] = null; //כדי שיעדכן את התצוגה בפעם הבאה שיחזור לדף בעל עסק null  השמת 
                                                       // Move to another page 
                Response.Redirect("Attraction_Owner.aspx"); // העברה לדף בעל עסק
            }
        }


        //הוספת תמונה ראשית + הצגתה
        protected void Addphoto_Button_Click(object sender, EventArgs e)
        {
            if (Attraction_MainPhoto.HasFile) // אם הוספה תמונה חדשה - לא נלחץ כפתור ההעלאה ללא קובץ
            {
                // Get the uploaded file
                HttpPostedFile postedFile = Attraction_MainPhoto.PostedFile;

                //***הגבלת סוג הקובץ
                // מציאת שם סוג הקובץ
                string fileName = System.IO.Path.GetFileName(postedFile.FileName);// Get the file name
                string fileExtension = System.IO.Path.GetExtension(fileName);// Get the file extension
                if (fileExtension != ".jpg") // מציג הודעה-  JPG אם סוג הקובץ לא 
                {
                    Result_Att.Text = "סוג הקובץ שצורף לא תואם הסוג המותר";
                    goto FileWasNotUploaded;
                }

                //***הגבלת גודל הקובץ
                //מציאת גודל הקובץ
                int fileSizeInBytes = postedFile.ContentLength;// Get the file size in bytes
                double fileSizeInKB = Math.Round((double)fileSizeInBytes / 1024, 2);// Convert the file size to kilobytes 
                if (fileSizeInKB > 1000.0)//  אם גודל הקובץ גדול מ1000 קילו ביט- מציג הודעה
                {
                    Result_Att.Text = "גודל הקובץ שצורף חורג מהגודל המקסימלי המותר";
                    goto FileWasNotUploaded;
                }


                //טיפול בתמונה ראשית קיימת
                if (Session["filelocation"] != null) // אם מעדכן תמונה/משנה אותה
                {
                    //הוספה לדאטאטייבל מחיקה מהשרת
                    DataRow newRow2 = RemovePhotoFileFromServer.NewRow();// יצירת שורה חדשה
                    newRow2["FileLocation"] = Session["filelocation"].ToString();// השדה שישלח לשמירה בבסיס הנתונים
                    RemovePhotoFileFromServer.Rows.Add(newRow2);//הוספה לדאטאטייבל


                    MainPhotoTable.Rows[0].Delete();// מחיקה מהדאטאסט - תצוגה
                    Session["filelocation"] = null;
                }


                //הוספת תמונה ראשית
                string FileName = DateTime.Now.Ticks + Path.GetFileName(Attraction_MainPhoto.PostedFile.FileName); // שם הקובץ
                string filelocation = "~\\pictures\\" + FileName;// השדה שישלח לשמירה בבסיס הנתונים
                Session["filelocation"] = filelocation; // שמירה בסשן
                Attraction_MainPhoto.PostedFile.SaveAs(Server.MapPath("~\\pictures\\" + FileName)); // שמירה בתיקיה

                DataRow newRow = MainPhotoTable.NewRow();// יצירת שורה חדשה
                newRow["FileLocation"] = "~\\pictures\\" + FileName;// השדה שישלח לשמירה בבסיס הנתונים
                MainPhotoTable.Rows.Add(newRow);//הוספה לדאטאסט

                // הצגת התמונה
                MainPhoto.DataSource = MainPhotoTable;
                MainPhoto.DataBind();

            }
            else // אם לא נוספה תמונה חדשה מציג הודעה
            {
                //Label1.Text = "לא צורף קובץ";
            }
        FileWasNotUploaded:;
        }


        //הוספת תמונה לאטרקציה - תמונות נוספות
        //לסדר מקרה בו נלחץ פעמיים על אישור בלי העלאת תמונה חדשה
        protected void AddPhoto_Confirm_Click1(object sender, EventArgs e)
        {
            if (AddPhoto.HasFile) // אם הוספה תמונה חדשה - לא נלחץ כפתור ההעלאה ללא קובץ
            {
                // Get the uploaded file
                HttpPostedFile postedFile = AddPhoto.PostedFile;

                //***הגבלת סוג הקובץ
                // מציאת שם סוג הקובץ
                string fileName = System.IO.Path.GetFileName(postedFile.FileName);// Get the file name
                string fileExtension = System.IO.Path.GetExtension(fileName);// Get the file extension
                if (fileExtension != ".jpg") // מציג הודעה-  JPG אם סוג הקובץ לא 
                {
                    Result_Att.Text = "סוג הקובץ שצורף לא תואם הסוג המותר";
                    goto FileWasNotUploaded;
                }

                //***הגבלת גודל הקובץ
                //מציאת גודל הקובץ
                int fileSizeInBytes = postedFile.ContentLength;// Get the file size in bytes
                double fileSizeInKB = Math.Round((double)fileSizeInBytes / 1024, 2);// Convert the file size to kilobytes 
                if (fileSizeInKB > 1000.0)//  אם גודל הקובץ גדול מ1000 קילו ביט- מציג הודעה
                {
                    Result_Att.Text = "גודל הקובץ שצורף חורג מהגודל המקסימלי המותר";
                    goto FileWasNotUploaded;
                }


                //טיפול בתמונה להוספה
                DataRow newRow = PhotosTable.NewRow();// יצירת שורה חדשה
                string FileName = DateTime.Now.Ticks + Path.GetFileName(AddPhoto.PostedFile.FileName); // שם הקובץ
                newRow["FileLocation"] = "~\\pictures\\" + FileName;// השדה שישלח לשמירה בבסיס הנתונים
                AddPhoto.PostedFile.SaveAs(Server.MapPath("~\\pictures\\" + FileName)); // שמירה בתיקיה
                                                                                        // Add the DataRow to the DataTable
                PhotosTable.Rows.Add(newRow);//הוספה לדאטאסט

                //הצגת התמונה שהוספה
                MorePhotos.DataSource = PhotosTable;
                MorePhotos.DataBind();

                // אם במצב עריכת פרטי אטרקציה - הוספת התמונה לטבלת הוספת תמונות בעדכון פרטי אטרקציה
                if(true)//לשנות לתנאי עריכת פרטי אטרקציה
                {
                    newRow = AddPhotoTable.NewRow();// יצירת שורה חדשה
                    FileName = DateTime.Now.Ticks + Path.GetFileName(AddPhoto.PostedFile.FileName); // שם הקובץ
                    newRow["FileLocation"] = "~\\pictures\\" + FileName;// השדה שישלח לשמירה בבסיס הנתונים
                    AddPhoto.PostedFile.SaveAs(Server.MapPath("~\\pictures\\" + FileName)); // שמירה בתיקיה
                                                                                            // Add the DataRow to the DataTable
                    AddPhotoTable.Rows.Add(newRow);//הוספה לדאטאטייבל
                    
                }
            }
            else // אם לא נוספה תמונה חדשה מציג הודעה
            {
                //Label1.Text = "לא צורף קובץ";
            }
            FileWasNotUploaded:;

        }

        //מחיקת תמונה - מתמונות נוספות
        protected void MorePhotos_ItemCommand1(object source, DataListCommandEventArgs e) // העברת נתוני ההזמנה לעמוד הזמנה
        {
            if (e.CommandName == "remove")
            {
                foreach (DataRow row in PhotosTable.Rows)// הסרת התמונה מדאטאסט התמונות + מהתקייה במחשב
                {
                    if (row.RowState != DataRowState.Deleted) //בדיקה אם השורה קיימת בדאטאסט או שנמחקה כבר - ואז יחזיר שגיאה
                    {
                        if (row["FileLocation"].ToString() == ((Image)e.Item.FindControl("Photo")).ImageUrl)
                        {
                            string PhotoID = row["מזהה"].ToString();
                            // מחיקת תמונה הקיימת במסד - עדכון פרטי אטרקציה
                            if (PhotoID != "")// אם התמונה הנמחקת קיימת כבר במסד
                            {
                                DataRow newRow = RemovePhotoTable.NewRow();// יצירת שורה חדשה
                                string FileName = DateTime.Now.Ticks + Path.GetFileName(AddPhoto.PostedFile.FileName); // שם הקובץ
                                newRow["FileLocation"] = "~\\pictures\\" + FileName;// השדה שישלח לשמירה בבסיס הנתונים
                                newRow["מזהה"] = Convert.ToInt32(PhotoID);// השדה שישלח לשמירה בבסיס הנתונים
                                AddPhoto.PostedFile.SaveAs(Server.MapPath("~\\pictures\\" + FileName)); // שמירה בתיקיה
                                RemovePhotoTable.Rows.Add(newRow);//הוספה לדאטאטייבל
                            }


                            //הוספה לדאטאטייבל מחיקה מהשרת
                            DataRow newRow2 = RemovePhotoFileFromServer.NewRow();// יצירת שורה חדשה
                            string FileName2 = DateTime.Now.Ticks + Path.GetFileName(AddPhoto.PostedFile.FileName); // שם הקובץ
                            newRow2["FileLocation"] = "~\\pictures\\" + FileName2;// השדה שישלח לשמירה בבסיס הנתונים
                            AddPhoto.PostedFile.SaveAs(Server.MapPath("~\\pictures\\" + FileName2)); // שמירה בתיקיה
                            RemovePhotoFileFromServer.Rows.Add(newRow2);//הוספה לדאטאטייבל

                            //מחיקת התמונה מהתצוגה
                            row.Delete(); // מחיקה מהדאטאסט
                            break;
                        }

                    }
                }
                MorePhotos.DataSource = PhotosTable; // עדכון הדאטאליסט
                MorePhotos.DataBind();

                
            }
        }


    }
}