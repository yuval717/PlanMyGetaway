using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Project_01
{
    public partial class User_Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)// כניסת משתמש - מציג: אם משתמש קיים ונכנס/לא קיים
        {
            User IsLoginUser = UserService.Login("SELECT * FROM Users INNER JOIN Country ON Users.User_Country = Country.Country_ID WHERE Users.User_Name = '"
                + User_Name.Text + "'" + "AND Users.User_Password= '" + User_Password.Text + "'");// Login הפעלת פעולת 
            if (IsLoginUser != null)
            {
                Result.Text = "Login Successfully";
                Session["User"] = IsLoginUser;
            }
            else
                Result.Text = " User Name do not exists ";
        }
    }
}