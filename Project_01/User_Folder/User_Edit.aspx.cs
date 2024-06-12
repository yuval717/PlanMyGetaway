using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using web_service;
using System.Text.RegularExpressions;

namespace Project_01
{
    public partial class User_Edit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
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

                User user = (User)Session["User"];
                Password.Text = user.User_Password;
                User_FirstName.Text = user.User_FirstName;
                User_LastName.Text = user.User_LastName;
                User_Gender.SelectedValue = user.User_Gender;
                User_Gmail.Text = user.User_Gmail;
                User_PhoneNumber.Text = user.User_PhoneNumber;
            }
        }

        private string ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Contains(" ") || password.Length < 4 || password.Length > 16)
            {
                // Handle invalid password
                return "שגיאה";
            }
            return "הצלחה";
        }

        private string ValidateEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@gmail\.com$";
            if (string.IsNullOrWhiteSpace(email) || !Regex.IsMatch(email, pattern))
            {
                // Handle invalid email
                return "שגיאה";
            }
            return "הצלחה";
        }

        private string ValidateName(string name, string fieldName)
        {
            string pattern = @"^[a-zA-Z\u0590-\u05FF]{4,50}$";
            if (string.IsNullOrWhiteSpace(name) || !Regex.IsMatch(name, pattern))
            {
                // Handle invalid name
                return "שגיאה";
            }
            return "הצלחה";
        }

        private string ValidatePhoneNumber(string phoneNumber)
        {
            string pattern = @"^(\+972|0)?[2-9]\d{7,8}$|^(\+1|1)?\d{10}$";
            if (string.IsNullOrWhiteSpace(phoneNumber) || !Regex.IsMatch(phoneNumber, pattern))
            {
                // Handle invalid phone number
                return "שגיאה";
            }
            return "הצלחה";
        }

        //עדכון פרטים
        protected void Edit_Click(object sender, EventArgs e)
        {
            //וולידיישן
            bool NoTextError = true;//שגיאות וולידישן
            if (NoTextError && ValidatePassword(Password.Text) == "שגיאה")
            {
                Update_Result.Text = "הכנס סיסמה תקינה";
                NoTextError = false;
            }
            if (NoTextError && ValidateName(User_FirstName.Text, "שם פרטי") == "שגיאה")
            {
                Update_Result.Text = "הכנס שם פרטי תקין";
                NoTextError = false;
            }
            if (NoTextError && ValidateName(User_LastName.Text, "שם משפחה") == "שגיאה")
            {
                Update_Result.Text = "הכנס שם משפחה תקין";
                NoTextError = false;
            }
            if (NoTextError && ValidateEmail(User_Gmail.Text) == "שגיאה")
            {
                Update_Result.Text = "הכנס אימייל תקין";
                NoTextError = false;
            }
            if (NoTextError && ValidatePhoneNumber(User_PhoneNumber.Text) == "שגיאה")
            {
                Update_Result.Text = "הכנס מספר טלפון תקין";
                NoTextError = false;
            }

            //אם וולידיישן תקין מבצע את הפעולה
            if (NoTextError)
            {

                //שאילתת עדכון
                UserService.Update(Password.Text, User_FirstName.Text, User_LastName.Text, User_Gmail.Text, User_PhoneNumber.Text,
                User_Gender.SelectedValue, ((User)Session["User"]).User_Name);

                Response.Redirect("User_Login_Register.aspx"); // העברה לדף הרשמה 
            }
        }
    }
}