using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_01
{
    public class User
    {
        public string User_Name { get; set; }
        public string User_Password { get; set; }
        public string User_FirstName { get; set; }
        public string User_LastName { get; set; }
        public string User_Gmail { get; set; }
        public string User_PhoneNumber { get; set; }
        public string User_Gender { get; set; }
        public string User_CountryID { get; set; }
        public string User_CountryName { get; set; }
        public string User_Photo { get; set; }
        public string User_LastEntrance { get; set; }

        public User (string User_Name, string User_Password, string User_FirstName, string User_LastName, string User_Gmail, string User_PhoneNumber, string User_Gender,
            string User_CountryID, string User_CountryName, string User_Photo)// פעולה בונה
        {
            this.User_Name = User_Name;
            this.User_Password = User_Password;
            this.User_FirstName = User_FirstName;
            this.User_LastName = User_LastName;
            this.User_Gmail = User_Gmail;
            this.User_PhoneNumber = User_PhoneNumber;
            this.User_Gender = User_Gender;
            this.User_CountryID = User_CountryID;
            this.User_CountryName = User_CountryName;
            this.User_Photo = User_Photo;
        }

        public User (string User_Name)
        {
            this.User_Name = User_Name;
        }
    }
}