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
        public static string Register(string User_Name, string User_Password, string User_FirstName, string User_LastName, string User_Gmail, string User_PhoneNumber,
            string User_Gender, string User_Country, string User_Photo)// פעולת הרשמה - מחזירה מחרוזת: האם נרשם או כבר קיים משתמש בעל אותו שם
        {
            string s = "select user_name from users where user_name= '" + User_Name + "'";
            if ((string)Connect.Connect_ExecuteScalar(s) == null)
            {
                s = "INSERT INTO Users (User_Name, User_Password, User_FirstName, User_LastName, User_Gmail, User_PhoneNumber, User_Gender,User_Country, User_Photo) " +
                    "VALUES ('" + User_Name + "','" + User_Password + "','" + User_FirstName + "','" + User_LastName + "','" + User_Gmail + "','" + User_PhoneNumber + "','" +
                    User_Gender + "','" + User_Country + "','" + User_Photo + "')";
                Connect.Connect_ExecuteNonQuery(s);
                return "Register is complete you're in";
            }
            else
            {
                return " User Name is already exist, enter a new one ";
            }
        }

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
                u = new User(reader["User_Name"].ToString(), reader["User_Password"].ToString(), reader["User_FirstName"].ToString(), reader["User_LastName"].ToString(),
                    reader["User_Gmail"].ToString() , reader["User_PhoneNumber"].ToString(), reader["User_Gender"].ToString(), reader["Country_ID"].ToString(),
                    reader["Country_Name"].ToString(), reader["User_Photo"].ToString() );
            }
            Conn.Close();
            return u;


        }
    }
}