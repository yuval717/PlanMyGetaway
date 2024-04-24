using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace Project_01
{
    public partial class User_Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ArrayList arr = Connect.FillArrayListForDropDownList("SELECT Country_Name ,Country_ID FROM Country; ", "Country_Name", "Country_ID");
            for (int i = 0; i < arr.Count; i++) //שהחזירה עצמי דרופדאוןליסט FillArrayListForDropDownList שקיבלנו מהפעלת הפעולה  ArrayList הכנסת ערכי מדינות (מזהה ושם מדינה) לדרופדאוןליסט מתוך  
            {
                User_Country.Items.Add((ListItem)arr[i]);
            }
        }

        protected void Button1_Click(object sender, EventArgs e)// הרשמה
        {
            FileUploadFunc();// הפעלת פעולת העלאת תמונה

            Result.Text = UserService.Register(User_Name.Text, User_Password.Text, User_FirstName.Text, User_LastName.Text, User_Gmail.Text, User_PhoneNumber.Text,
                User_Gender.SelectedValue, User_Country.SelectedValue, Session["filelocation"].ToString()); // הפעלת פעולת הרשמה

            Session["User"] = new User(User_Name.Text, User_Password.Text, User_FirstName.Text, User_LastName.Text, User_Gmail.Text, User_PhoneNumber.Text,
                User_Gender.SelectedValue, User_Country.SelectedValue, User_Country.SelectedItem.Text, Session["filelocation"].ToString());// שמירת פרטי משתמש נוכחי("מחובר") באתר
        }

        public void FileUploadFunc()// יוצר נתיב תמונה במידה וקיימת ,שומר אותו בסשן כמחרוזת, שומר את התמונה בתקיית תמונות השמורה בפרוייקט
        {
            if (FileUpload1.PostedFile.FileName == "")
            {
                Label16.Text = "No file name";
            }
            else
            {
                string filename = Path.GetFileName(FileUpload1.PostedFile.FileName); //DateTime.Now.Ticks
                FileUpload1.PostedFile.SaveAs(Server.MapPath("~\\pictures\\" + filename)); // שמירה בתיקיה
                string filelocation = "~\\pictures\\" + filename; // השדה שישלח לשמירה בבסיס הנתונים
                Session["filelocation"] = filelocation;
            }
        }
    }
}