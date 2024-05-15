using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_01
{
    public class Attraction
    {
        public int Attraction_ID { get; set; }
        public string Attraction_Name { get; set; }
        public string Attraction_TypeID { get; set; }
        public string Attraction_TypeName { get; set; }
        public string Attraction_MinAge { get; set; }
        public string Attraction_MaxAge { get; set; }
        public string Attraction_Price { get; set; }
        public int Attraction_Duration { get; set; }
        public string Attraction_Address { get; set; }
        public string Attraction_Gmail { get; set; }
        public string Attraction_PhonNumber { get; set; }
        public string Attraction_recommendedMonth { get; set; }
        public string Attraction_FreeEntry { get; set; }
        public string Attraction_Text { get; set; }
        public string Attraction_Photo { get; set; }
        public double Attraction_Latitude { get; set; }
        public double Attraction_Longitude { get; set; }
        public int Attraction_PathOrder { get; set; }


        public Attraction() { }

        public Attraction(int Attraction_ID, double Attraction_Latitude, double Attraction_Longitude, int Attraction_Duration, int Attraction_PathOrder)
        {
            this.Attraction_ID = Attraction_ID;
            this.Attraction_Latitude = Attraction_Latitude;
            this.Attraction_Longitude = Attraction_Longitude;
            this.Attraction_Duration = Attraction_Duration;
            this.Attraction_PathOrder = Attraction_PathOrder;
        }

        public Attraction(int Attraction_ID, double Attraction_Latitude, double Attraction_Longitude)
        {
            this.Attraction_ID = Attraction_ID;
            this.Attraction_Latitude = Attraction_Latitude;
            this.Attraction_Longitude = Attraction_Longitude;
        }

        public Attraction (string Attraction_Name, string Attraction_TypeID, string Attraction_TypeName,
            string Attraction_MinAge,string Attraction_MaxAge, string Attraction_Price, int Attraction_Duration,
            string Attraction_Address, string Attraction_Gmail, string Attraction_PhonNumber, string Attraction_recommendedMonth, 
            string Attraction_FreeEntry, string Attraction_Text, string Attraction_Photo)
        {
            this.Attraction_Name = Attraction_Name;
            this.Attraction_TypeID = Attraction_TypeID;
            this.Attraction_TypeName = Attraction_TypeName;
            this.Attraction_MinAge = Attraction_MinAge;
            this.Attraction_MaxAge = Attraction_MaxAge;
            this.Attraction_Price = Attraction_Price;
            this.Attraction_Duration = Attraction_Duration;
            this.Attraction_Address = Attraction_Address;
            this.Attraction_Gmail = Attraction_Gmail;
            this.Attraction_PhonNumber = Attraction_PhonNumber;
            this.Attraction_recommendedMonth = Attraction_recommendedMonth;
            this.Attraction_FreeEntry = Attraction_FreeEntry;
            this.Attraction_Text = Attraction_Text;
            this.Attraction_Photo = Attraction_Photo;
        }

        public Attraction(string Attraction_Name, string Attraction_TypeID, string Attraction_TypeName,
            string Attraction_MinAge, string Attraction_MaxAge, string Attraction_Price, int Attraction_Duration,
            string Attraction_Address, string Attraction_Gmail, string Attraction_PhonNumber, string Attraction_recommendedMonth,
            string Attraction_FreeEntry, string Attraction_Text, string Attraction_Photo, double Attraction_Latitude, double Attraction_Longitude, int Attraction_PathOrder)
        {
            this.Attraction_Name = Attraction_Name;
            this.Attraction_TypeID = Attraction_TypeID;
            this.Attraction_TypeName = Attraction_TypeName;
            this.Attraction_MinAge = Attraction_MinAge;
            this.Attraction_MaxAge = Attraction_MaxAge;
            this.Attraction_Price = Attraction_Price;
            this.Attraction_Duration = Attraction_Duration;
            this.Attraction_Address = Attraction_Address;
            this.Attraction_Gmail = Attraction_Gmail;
            this.Attraction_PhonNumber = Attraction_PhonNumber;
            this.Attraction_recommendedMonth = Attraction_recommendedMonth;
            this.Attraction_FreeEntry = Attraction_FreeEntry;
            this.Attraction_Text = Attraction_Text;
            this.Attraction_Photo = Attraction_Photo;
            this.Attraction_Latitude = Attraction_Latitude;
            this.Attraction_Longitude = Attraction_Longitude;
            this.Attraction_PathOrder = Attraction_PathOrder;
        }
    }
}