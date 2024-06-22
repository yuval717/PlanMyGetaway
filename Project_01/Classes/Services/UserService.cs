using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.OleDb;

namespace Project_01
{
    public class UserService
    {
        //פעולת הרשמה - משתמש
        public static void Register(string User_Name, string User_Password, string User_FirstName, string User_LastName, string User_Gmail, string User_PhoneNumber,
            string User_Gender, string UserType)// פעולת הרשמה - מחזירה מחרוזת: האם נרשם או כבר קיים משתמש בעל אותו שם
        {
            //בדיקת סוג המשתמש
            bool User_Type = false;
            if (UserType == "בעל עסק")
            {
                User_Type = true;
            }
            //שאילתת הרשמה
            string s = "INSERT INTO Users (User_Name, User_Password, User_FirstName, User_LastName, User_Gmail, User_PhoneNumber, User_Gender, User_IsAttractionOwner, User_IsAdmin, User_LastEntrance, User_IsBlocked ) " +
                    "VALUES ('" + User_Name + "','" + User_Password + "','" + User_FirstName + "','" + User_LastName + "','" + User_Gmail + "','" + User_PhoneNumber + "','" +
                    User_Gender + "'," + User_Type + ", " + false + ", #"+ DateTime.Now + "#, "+ false+")";
                Connect.Connect_ExecuteNonQuery(s);
        }

        //עדכון פרטי משתמש
        public static void Update(string User_Password, string User_FirstName, string User_LastName, string User_Gmail, string User_PhoneNumber,
            string User_Gender, string OldUser_Name)
        {
            //שאילתת עדכון
            string s = "UPDATE users SET User_Password = '"+ User_Password + "', User_FirstName = '"+ User_FirstName + "', User_LastName = '"+ User_LastName + "', User_Gmail = '"+ User_Gmail +
                "', User_PhoneNumber = '"+ User_PhoneNumber + "', User_Gender = '"+ User_Gender + "' WHERE User_Name = '"+ OldUser_Name+"'";
            Connect.Connect_ExecuteNonQuery(s);
        }

        public static void UpdateLastEntrance(string User_Name)
        {
            Connect.Connect_ExecuteNonQuery("UPDATE Users SET User_LastEntrance = #" + DateTime.Now + "# WHERE User_Name = '"+ User_Name + "'" );
        }

        //פעולת בדיקת קיימות שם משתמש
        public static string UserCheckBeforeRegister(string User_Name)
        {
            return (string)Connect.Connect_ExecuteScalar("select user_name from users where user_name= '" + User_Name + "'");
        }

        //פעולת בדיקת קיימות משתמש - שכחתי סיסמה
        public static string UserCheckForgotPassword(string User_Name, string Gmail)
        {
            return (string)Connect.Connect_ExecuteScalar("select User_Password from users WHERE User_IsBlocked = " + false +" AND user_name = '" + User_Name + "' AND User_Gmail = '"+ Gmail+"'");
        }

        //פעולת התחברות למשתמש
        public static User Login(string s)// פעולת כניסת משתמש - מחזירה עצם מסוג משתמש   
        {
            
            OleDbConnection Conn = new OleDbConnection();
            Conn.ConnectionString = Connect.GetConnectionString();
            OleDbCommand command = new OleDbCommand(s, Conn);
            User u = null;
            Conn.Open();
            OleDbDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                string a = reader["User_IsAttractionOwner"].ToString();
                string b = reader["User_IsAttractionOwner"].ToString();
                string c = reader["User_IsAdmin"].ToString();
                string usertype = "";
                if (reader["User_IsAdmin"].ToString() == "True")
                    usertype = "אדמין";
                else if (reader["User_IsAttractionOwner"].ToString() == "False")
                    usertype = "משתמש";
                else 
                    usertype = "בעל עסק";

                u = new User(reader["User_Name"].ToString(), reader["User_Password"].ToString(), reader["User_FirstName"].ToString(), reader["User_LastName"].ToString(),
                    reader["User_Gmail"].ToString() , reader["User_PhoneNumber"].ToString(), reader["User_Gender"].ToString(), usertype);
            }
            Conn.Close();
            return u;


        }
    }
}