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
    public partial class User_Login : System.Web.UI.Page
    {
        public static User LogedUser;
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        private string ValidateUserName(string userName)
        {
            string pattern = @"^[a-zA-Z0-9_\u0590-\u05FF]{4,50}$";
            if (string.IsNullOrWhiteSpace(userName) || !Regex.IsMatch(userName, pattern))
            {
                // Handle invalid username
                return "שגיאה";
            }
            return "הצלחה";
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

        private string ValidateCreditCardNumber(string creditCardNumber)
        {
            string pattern = @"^\d{16}$";
            if (string.IsNullOrWhiteSpace(creditCardNumber) || !Regex.IsMatch(creditCardNumber, pattern))
            {
                // Handle invalid credit card number
                return "שגיאה";
            }
            return "הצלחה";
        }

        private string ValidateExpirationDate(string expirationDate)
        {
            DateTime date;
            if (!DateTime.TryParse(expirationDate, out date) || date <= DateTime.Now)
            {
                // Handle invalid expiration date
                return "שגיאה";
            }
            return "הצלחה";
        }

        private string ValidateCVV(string cvv)
        {
            string pattern = @"^\d{3,4}$";
            if (string.IsNullOrWhiteSpace(cvv) || !Regex.IsMatch(cvv, pattern))
            {
                // Handle invalid CVV
                return "שגיאה";
            }
            return "הצלחה";
        }

        private string ValidateID(string id)
        {
            string pattern = @"^\d{9}$"; // Assuming 9 digit ID for simplicity
            if (string.IsNullOrWhiteSpace(id) || !Regex.IsMatch(id, pattern))
            {
                // Handle invalid ID
                return "שגיאה";
            }
            return "הצלחה";
        }


        //כפתור התחברות
        protected void Button1_Click(object sender, EventArgs e)// כניסת משתמש - מציג: אם משתמש קיים ונכנס/לא קיים
        {
            //וולידיישן
            bool NoTextError = true;//שגיאות וולידישן
            if (NoTextError && ValidateUserName(User_Name.Text) == "שגיאה")
            {
                Result_LogIn.Text = "הכנס שם משתמש תקין";
                NoTextError = false;
            }
            if (NoTextError && ValidatePassword(User_Password.Text) == "שגיאה")
            {
                Result_LogIn.Text = "הכנס סיסמה תקינה";
                NoTextError = false;
            }
            
            //אם וולידיישן תקין מבצע את הפעולה
            if(NoTextError)
            {
                User IsLoginUser = UserService.Login("SELECT * FROM Users WHERE User_IsBlocked = " + false + " AND Users.User_Name = '"
                + User_Name.Text + "'" + "AND Users.User_Password= '" + User_Password.Text + "'");// Login הפעלת פעולת 
                if (IsLoginUser != null)
                {
                    UserService.UpdateLastEntrance(User_Name.Text);
                    Result_LogIn.Text = "Login Successfully";
                    Session["User"] = IsLoginUser; // הכנסה לסשן פרטי משתמש
                    LogedUser = (User)Session["User"];

                    //העברה לדף בית מתאים
                    if (LogedUser.User_Type == "בעל עסק")
                    {
                        //Move to another page 
                        Response.Redirect("/Attraction_Folder/Attraction_Owner.aspx"); // העברה לדף עריכת מסלול
                    }
                    else if (LogedUser.User_Type == "משתמש")
                    {
                        Response.Redirect("/Homepage.aspx"); // העברה לדף עריכת מסלול
                    }
                    else
                    {
                        Response.Redirect("/admin.aspx"); // העברה לדף עריכת מסלול
                    }

                }
                else
                {
                    if (Connect.Connect_ExecuteScalar("SELECT * FROM Users WHERE Users.User_Name = '"
                + User_Name.Text + "'" + "AND Users.User_Password= '" + User_Password.Text + "'") !=null)
                    {
                        Result_LogIn.Text = "המשתמש חסום";
                    }
                    else
                        Result_LogIn.Text = "פרטי המשתמש שגויים";
                }
                    
            }

        }

        //כפתור הרשמה
        protected void DoRegister_Click(object sender, EventArgs e)
        {
            Login_div.Style["display"] = "none";
            Register_div.Style["display"] = "block";
            ForgotPassword_Div.Style["display"] = "none";
        }


        //כפתור הרשמה - פרטים אישיים
        protected void Button2_Click(object sender, EventArgs e)// הרשמה
        {

            //וולידיישן
            bool NoTextError = true;//שגיאות וולידישן
            if (NoTextError && ValidateUserName(UserName.Text) == "שגיאה")
            {
                Result_Register.Text = "הכנס שם משתמש תקין";
                NoTextError = false;
            }
            if (NoTextError && ValidatePassword(Password.Text) == "שגיאה")
            {
                Result_Register.Text = "הכנס סיסמה תקינה";
                NoTextError = false;
            }
            if (NoTextError && ValidateName(User_FirstName.Text, "שם פרטי") == "שגיאה")
            {
                Result_Register.Text = "הכנס שם פרטי תקין";
                NoTextError = false;
            }
            if (NoTextError && ValidateName(User_LastName.Text, "שם משפחה") == "שגיאה")
            {
                Result_Register.Text = "הכנס שם משפחה תקין";
                NoTextError = false;
            }
            if (NoTextError && ValidateEmail(User_Gmail.Text) == "שגיאה")
            {
                Result_Register.Text = "הכנס אימייל תקין";
                NoTextError = false;
            }
            if (NoTextError && ValidatePhoneNumber(User_PhoneNumber.Text) == "שגיאה")
            {
                Result_Register.Text = "הכנס מספר טלפון תקין";
                NoTextError = false;
            }

            //אם וולידיישן תקין מבצע את הפעולה
            if (NoTextError)
            {

                if (UserService.UserCheckBeforeRegister(UserName.Text) == null)
                {
                    Register_div.Style["display"] = "none";
                    Payment_div.Style["display"] = "block";
                }
                else
                    Result_Register.Text = "שם המשתמש בשימוש";
            }
        }


        //כפתור הרשמה - תשלום
        protected void Button3_Click(object sender, EventArgs e)// הרשמה
        {

            //וולידיישן
            bool NoTextError = true;//שגיאות וולידישן
            if (NoTextError && ValidateCreditCardNumber(Number.Text) == "שגיאה")
            {
                Result_Payment.Text = "הכנס מספר אשראי תקין";
                NoTextError = false;
            }
            if (NoTextError && ValidateExpirationDate(DateOfExpiration.Text) == "שגיאה")
            {
                Result_Payment.Text = "הכנס תאריך תקין";
                NoTextError = false;
            }
            if (NoTextError && ValidateCVV(CVV.Text) == "שגיאה")
            {
                Result_Payment.Text = "הכנס שלוש ספרות תקינות";
                NoTextError = false;
            }
            if (NoTextError && ValidateID(Owner_ID.Text) == "שגיאה")
            {
                Result_Payment.Text = "הכנס תעודת זהות תקינה";
                NoTextError = false;
            }

            //אם וולידיישן תקין מבצע את הפעולה
            if (NoTextError)
            {

                PayCheck p = new PayCheck();
                string payResult = p.Pay(Number.Text, Owner_ID.Text, Provider.SelectedValue, CVV.Text, DateOfExpiration.Text, "100", 1);
                if (payResult == "שולם")
                {
                    UserService.Register(UserName.Text, Password.Text, User_FirstName.Text, User_LastName.Text, User_Gmail.Text, User_PhoneNumber.Text,
                    User_Gender.SelectedValue, UserType.SelectedValue); // הפעלת פעולת הרשמה

                    Session["User"] = new User(User_Name.Text, User_Password.Text, User_FirstName.Text, User_LastName.Text, User_Gmail.Text, User_PhoneNumber.Text,
                        User_Gender.SelectedValue, UserType.SelectedValue);// שמירת פרטי משתמש נוכחי("מחובר") באתר
                    LogedUser = (User)Session["User"];

                    //העברה לדף בית מתאים
                    if (LogedUser.User_Type == "בעל עסק")
                    {
                        //Move to another page 
                        Response.Redirect("/Attraction_Folder/Attraction_Owner.aspx"); // העברה לדף עריכת מסלול
                    }
                    else if (LogedUser.User_Type == "משתמש")
                    {
                        Response.Redirect("/Homepage.aspx"); // העברה לדף עריכת מסלול
                    }
                    else
                    {
                        Response.Redirect("/admin.aspx"); // העברה לדף עריכת מסלול
                    }

                }
                else
                {
                    Result_Payment.Text = payResult;
                }
            }


        }

        //כפתור שחזור סיסמה
        protected void ForgotPassWord_Button_Click(object sender, EventArgs e)
        {
            //וולידיישן
            bool NoTextError = true;//שגיאות וולידישן
            if (NoTextError && ValidateUserName(Forgot_UserName.Text) == "שגיאה")
            {
                Forgot_Result.Text = "הכנס שם משתמש תקין";
                NoTextError = false;
            }
            if (NoTextError && ValidateEmail(Forgot_Gmail.Text) == "שגיאה")
            {
                Forgot_Result.Text = "הכנס אימייל תקין";
                NoTextError = false;
            }

            //אם וולידיישן תקין מבצע את הפעולה
            if (NoTextError)
            {
                if (UserService.UserCheckForgotPassword(Forgot_UserName.Text, Forgot_Gmail.Text) != null)
                {
                    //שליחת אימייל

                    //מעבר לכניסה
                    Login_div.Style["display"] = "block";
                    ForgotPassword_Div.Style["display"] = "none";
                }
                else
                    Forgot_Result.Text = "פרטי המשתמש שגויים";
            }
        }

        //כפתור שכחתי סיסמה
        protected void ForgotPassword_Click(object sender, EventArgs e)
        {
            Login_div.Style["display"] = "none";
            ForgotPassword_Div.Style["display"] = "block";
        }
    }
}
