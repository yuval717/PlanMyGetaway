using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Web.UI.WebControls;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Project_01
{
    public class AttractionService
    {
        // פעולת הרשמת אטרקציה - מחזירה מחרוזת: נרשם בהצלחה
        public static async Task<string> RegisterAttAsync(string Attraction_Name, string Attraction_Type, string Attraction_MinAge, string Attraction_MaxAge,
            string Attraction_Price, string Attraction_Duration, string Attraction_Address, string Attraction_Gmail,
            string Attraction_PhonNumber, string Attraction_RecommendedMonth, string Attraction_FreeEntry, string Attraction_Text,
            string Attraction_Photo)
        {
            ArrayList arr = await BingMapsGeocoder.GetCoordinatesByAddressAsync(Attraction_Address);
            string s = "INSERT INTO Attraction (Attraction_Name, Attraction_Type, Attraction_MinAge, Attraction_MaxAge, Attraction_Price, Attraction_Duration," +
                " Attraction_Address, Attraction_Gmail, Attraction_PhonNumber, Attraction_RecommendedMonth, Attraction_FreeEntry," +
                " Attraction_Text, Attraction_Photo,Attraction_Latitude,Attraction_Longitude ) VALUES ('" + Attraction_Name + "','" + Attraction_Type + "'," + Attraction_MinAge + "," + Attraction_MaxAge + "," +
                Attraction_Price + "," + Attraction_Duration + ",'" + Attraction_Address + "','" + Attraction_Gmail + "','" +
                Attraction_PhonNumber + "','" + Attraction_RecommendedMonth + "'," + Attraction_FreeEntry + ",'" + Attraction_Text + "','" + Attraction_Photo + "'," + Convert.ToDouble(arr[0]) + "," + Convert.ToDouble(arr[1]) + ") ";
            Connect.Connect_ExecuteNonQuery(s);

            List<Attraction> attractions = GetAttractionsFromDataBase();
            attractions = FindEfficientPath(attractions);
            UpdatePathToDataBase(attractions);

            return "Register is complete";
        }

        // פעולת עדכון נתוני אטרקציה - מחזירה מחרוזת: עודכן בהצלחה
        public static async Task<string> UpdateAtt(int Attraction_ID, string Attraction_Name, string Attraction_Type, string Attraction_MinAge, string Attraction_MaxAge,
            string Attraction_Price, string Attraction_Duration, string Attraction_Address, string Attraction_Gmail,
            string Attraction_PhonNumber, string Attraction_RecommendedMonth, string Attraction_FreeEntry, string Attraction_Text,
            string Attraction_Photo)
        {
            ArrayList arr = await BingMapsGeocoder.GetCoordinatesByAddressAsync(Attraction_Address);
            string s = "UPDATE Attraction SET Attraction_Name = '" + Attraction_Name + "',Attraction_Type = '" + Attraction_Type + "',Attraction_MinAge = " + Attraction_MinAge +
                ",Attraction_MaxAge = " + Attraction_MaxAge + ",Attraction_Price = " + Attraction_Price + ",Attraction_Duration = " + Attraction_Duration +
                ",Attraction_Address = '" + Attraction_Address + "',Attraction_Gmail = '" + Attraction_Gmail + "',Attraction_PhonNumber = '" +
                Attraction_PhonNumber + "',Attraction_RecommendedMonth = '" + Attraction_RecommendedMonth + "',Attraction_FreeEntry = " + Attraction_FreeEntry +
                ",Attraction_Text = '" + Attraction_Text + "',Attraction_Photo = '" + Attraction_Photo + "',Attraction_Latitude = " + Convert.ToDouble(arr[0]) + ",Attraction_Longitude = " + Convert.ToDouble(arr[1]) + " WHERE Attraction_ID =" + Attraction_ID;
            Connect.Connect_ExecuteNonQuery(s);



            return "Update is complete ";
        }

        // יוצר עצם מסוג אטרקציה המכיל נתונים ממסד הנתונים - מקבל שאילת - מחזיר את העצם
        public static Attraction FillAttraction(string s)
        {
            OleDbConnection Conn = new OleDbConnection();
            Conn.ConnectionString = Connect.GetConnectionString();
            OleDbCommand command = new OleDbCommand(s, Conn);
            Attraction a = new Attraction();
            Conn.Open();
            OleDbDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                a = new Attraction();
                a.Attraction_ID = reader["Attraction_ID"].ToString();
                a.Attraction_Name = reader["Attraction_Name"].ToString();
                a.Attraction_TypeID = reader["AttractionType_ID"].ToString();
                a.Attraction_TypeName = reader["AttractionType_Type"].ToString();
                a.Attraction_MinAge = reader["Attraction_MinAge"].ToString();
                a.Attraction_MaxAge = reader["Attraction_MaxAge"].ToString();
                //a.Attraction_Price = reader["Attraction_Price"].ToString();
                //a.Attraction_Duration = reader["Attraction_Duration"].ToString();
                a.Attraction_Address = reader["Attraction_Address"].ToString();
                //a.Attraction_Gmail = reader["Attraction_Gmail"].ToString();
                //a.Attraction_PhonNumber = reader["Attraction_PhonNumber"].ToString();
                //a.Attraction_recommendedMonth = reader["Attraction_recommendedMonth"].ToString();
                //a.Attraction_FreeEntry = reader["Attraction_FreeEntry"].ToString();
                a.Attraction_Text = reader["Attraction_Text"].ToString();
                a.Attraction_Photo = reader["Attraction_Photo"].ToString();
            }
            Conn.Close();
            return a;
        }  

        // Method to calculate distance between two attractions (Euclidean distance)
        // חישוב מרחק בין 2 נקודות
        public static double Distance(Attraction a, Attraction b)
        {
            double latDiff = a.Attraction_Latitude - b.Attraction_Latitude;
            double lonDiff = a.Attraction_Longitude - b.Attraction_Longitude;
            return Math.Sqrt(latDiff * latDiff + lonDiff * lonDiff);
        }

        // Method to find the attraction with the highest longitude
        // מציאת האטרקציה הגבוהה ביותר - לפי אורך
        public static Attraction FindHighestAttraction(List<Attraction> attractions)
        {
            Attraction highest = attractions[0];
            foreach (var attraction in attractions)
            {
                if (attraction.Attraction_Longitude > highest.Attraction_Longitude)
                {
                    highest = attraction;
                }
            }
            return highest;
        }

        // Method to find the attraction with the lowest longitude
        // מציאת האטרקציה הנמוכה ביותר - לפי אורך
        public static Attraction FindLowestAttraction(List<Attraction> attractions)
        {
            Attraction lowest = attractions[0];
            foreach (var attraction in attractions)
            {
                if (attraction.Attraction_Longitude < lowest.Attraction_Longitude)
                {
                    lowest = attraction;
                }
            }
            return lowest;
        }

        // Method to find the most efficient path from highest to lowest attraction
        // מיון סדר עצמי אטרקציות לפי המסלול האופטימלי מהנקודה הגבוהה לנמוכה לפי קורדינטאות - האיבר הראשון ברישמה הוא הנקודה הראשונה במסלול - מקבל רשימת אטרקציות ממסד הנתונים - מחזיר רשימה ממויינת
        public static List<Attraction> FindEfficientPath(List<Attraction> attractions)
        {
            List<Attraction> sortedAttractions = new List<Attraction>();
            Attraction highest = FindHighestAttraction(attractions);
            Attraction lowest = FindLowestAttraction(attractions);
            sortedAttractions.Add(highest);
            attractions.Remove(highest);
            // Remove the lowest attraction from the attractions list
            attractions.Remove(lowest);

            while (attractions.Count > 0)
            {
                double minDistance = double.MaxValue;
                Attraction closest = null;
                foreach (var attraction in attractions)
                {
                    double distance = Distance(sortedAttractions[sortedAttractions.Count - 1], attraction);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        closest = attraction;
                    }
                }
                sortedAttractions.Add(closest);
                attractions.Remove(closest);
            }

            sortedAttractions.Add(lowest); // Add the lowest attraction at the end
            return sortedAttractions;
        }

        // הכנסת כל האטרציות ממסד הנתונים לרשימה מסוג אטרציות
        public static List<Attraction> GetAttractionsFromDataBase()
        {
            List<Attraction> attractions = new List<Attraction>();
            OleDbConnection Conn = new OleDbConnection();
            Conn.ConnectionString = Connect.GetConnectionString();
            OleDbCommand command = new OleDbCommand("SELECT * FROM Attraction", Conn);
            Conn.Open();
            OleDbDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Attraction a = new Attraction(reader["Attraction_ID"].ToString() , Convert.ToDouble(reader["Attraction_Latitude"]),
                    Convert.ToDouble(reader["Attraction_Longitude"]));
                attractions.Add(a);
            }
            Conn.Close();
            return attractions;
        }

        // מקבל רשימה ממויינת מסוג אטרקציות -Attraction_PathOrder עדכון סדר המסלול (מהגבוה -1  לנמוך - 2 ) במסד הנתונים  - שדה 
        public static void UpdatePathToDataBase(List<Attraction> attractions)
        {
            for (int i = 0; i < attractions.Count ; i++)
            {
                Connect.Connect_ExecuteNonQuery("UPDATE Attraction SET Attraction_PathOrder = " + (i + 1) + " WHERE Attraction_ID = " + attractions[i].Attraction_ID);
            }
        }


    }

}
