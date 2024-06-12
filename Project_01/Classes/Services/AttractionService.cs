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
        // פעולת הרשמת אטרקציה
        public static async Task RegisterAttAsync(string Attraction_Name, string Attraction_Type, string Attraction_MinAge, string Attraction_MaxAge,
            string Attraction_Price, string Attraction_Duration, string Attraction_Address, string Attraction_Gmail,
            string Attraction_PhonNumber, string Attraction_Text, string Attraction_Photo, string Attraction_OpeningHour, string Attraction_ClosingHour,
            string User_Name, DataTable Photos, string Attraction_KilometersNumber, String Attraction_Difficulty)
        {
            ArrayList arr = await BingMapsGeocoder.GetCoordinatesByAddressAsync(Attraction_Address);// הפיכת הכתובת לקואורדינטות
            string s = "INSERT INTO Attraction(Attraction_Name, Attraction_Type, Attraction_MinAge, Attraction_MaxAge, Attraction_Price, Attraction_Duration, " +
             "Attraction_Address, Attraction_Gmail, Attraction_PhoneNumber, Attraction_Text, Attraction_Photo, Attraction_Latitude, Attraction_Longitude, " +
             "Attraction_OpeningHour, Attraction_ClosingHour, Attraction_AddDate, Attraction_Valid, User_Name) VALUES ('" +
             Attraction_Name + "','" + Attraction_Type + "'," + Attraction_MinAge + "," + Attraction_MaxAge + "," + Attraction_Price + "," +
             Attraction_Duration + ",'" + Attraction_Address + "','" + Attraction_Gmail + "','" + Attraction_PhonNumber + "','" + Attraction_Text + "','" +
             Attraction_Photo + "'," + Convert.ToDouble(arr[0]) + "," + Convert.ToDouble(arr[1]) + ",#" + (DateTime.Parse(Attraction_OpeningHour)).ToString("HH:mm") + "#,#" +
             (DateTime.Parse(Attraction_ClosingHour)).ToString("HH:mm") + "#,'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," + true + ",'" +
             User_Name + "');";
            Connect.Connect_ExecuteNonQuery(s); // הכנסת האטרקציה למסד

            // לאחר שהאטרקציה הוספה - סידור  מחדש של המסלול האופטימלי של האטרקציות *כולן* - לפי קרבה לארטקציות אחרות
            List<Attraction> attractions = GetAttractionsFromDataBase(); // שליפת כל האטרקציות ממסד הנתונים
            attractions = FindEfficientPath(attractions); // סידור האטרקציות
            UpdatePathToDataBase(attractions); // עדכון במסד

            //הוספת האטרקציה לטבלת  תמונות נוספות אם יש 
            int AttractionCode = (int)Connect.Connect_ExecuteScalar("Select Max(Attraction_ID) From Attraction"); // שליפת הערך האחרון של המפתח רץ - קוד האטרקציה שיצרנו
            foreach (DataRow row in Photos.Rows)//
            {
                s = "INSERT INTO Photos (Attraction_ID, FileLocation) VALUES (" + AttractionCode + ", '" + row["FileLocation"] + "')";
                Connect.Connect_ExecuteNonQuery(s); // הכנסת תמונה למסד
            }

            // אם סוג החופשה של סוג האטרקציה מסוג טיול בטבע - מוסיף לטבלת "אטרקציה בטבע" ערכים
            if(Attraction_KilometersNumber != "" && Attraction_Difficulty != "") // אם הוכנסו ערכים = מסוג חופשה טיול בטבע
            {
                s = "INSERT INTO NatureAttraction (Attraction_ID, NatureAttraction_Difficulty, NatureAttraction_KilometersNumber ) " +
                    "VALUES (" + AttractionCode + ", '"  + Attraction_Difficulty + "', "+ Attraction_KilometersNumber + ")";
                Connect.Connect_ExecuteNonQuery(s); // הכנסת ערכים למסד
            }
        }


        // פעולת עדכון נתוני אטרקציה
        public static async Task UpdateAtt(int Attraction_ID, string Attraction_Name, string Attraction_Type, string Attraction_MinAge, string Attraction_MaxAge,
    string Attraction_Price, string Attraction_Duration, string Attraction_Address, string Attraction_Gmail,
    string Attraction_PhonNumber, string Attraction_Text, string Attraction_Photo, string Attraction_OpeningHour, string Attraction_ClosingHour,
    string Attraction_KilometersNumber, string Attraction_Difficulty, DataTable RemovePhotoTable, DataTable AddPhotoTable)
        {
            string OldAddress = Connect.Connect_ExecuteScalar("SELECT Attraction_Address FROM Attraction WHERE Attraction_ID = " + Attraction_ID).ToString(); // שמירת הכתובת הקודמת - לפני העדכון לבדיקת צורך בשינוי מיקום במסלול
            string OldAttraction_OpeningHour = Connect.Connect_ExecuteScalar("SELECT Attraction_OpeningHour FROM Attraction WHERE Attraction_ID = " + Attraction_ID).ToString();
            string OldAttraction_ClosingHour = Connect.Connect_ExecuteScalar("SELECT Attraction_ClosingHour FROM Attraction WHERE Attraction_ID = " + Attraction_ID).ToString();

            // עדכון פרטי אטרקציה במסד
            ArrayList arr = await BingMapsGeocoder.GetCoordinatesByAddressAsync(Attraction_Address);
            string s = "UPDATE Attraction SET Attraction_Name = '" + Attraction_Name + "', Attraction_Type = '" + Attraction_Type + "', Attraction_MinAge = " + Attraction_MinAge +
                ", Attraction_MaxAge = " + Attraction_MaxAge + ", Attraction_Price = " + Attraction_Price + ", Attraction_Duration = " + Attraction_Duration +
                ", Attraction_Address = '" + Attraction_Address + "', Attraction_Gmail = '" + Attraction_Gmail + "', Attraction_PhoneNumber = '" +
                Attraction_PhonNumber + "', Attraction_Text = '" + Attraction_Text + "', Attraction_Photo = '" + Attraction_Photo + "', Attraction_OpeningHour = #" + (DateTime.Parse(Attraction_OpeningHour)).ToString("HH:mm") +
                "#, Attraction_ClosingHour = #" + (DateTime.Parse(Attraction_ClosingHour)).ToString("HH:mm") + "#," +
                " Attraction_Latitude = " + Convert.ToDouble(arr[0]) + ", Attraction_Longitude = " + Convert.ToDouble(arr[1]) + " WHERE Attraction_ID = " + Attraction_ID;
            Connect.Connect_ExecuteNonQuery(s);

            // אם התאריך שונה
            if (OldAttraction_OpeningHour != Attraction_OpeningHour || OldAttraction_ClosingHour != Attraction_ClosingHour)
            {
                // Day_Attraction הסרת האטרקצית מטבלת 
                DataTable AllAttractionsInDay_AttractionTable = Connect.Connect_DataTable("Select * from Day_Attraction WHERE Attraction_ID = " + Attraction_ID, "AttractionsToRemove"); // כל העמודות המכילות את האטרקציה בטבלת יום - אטרקציה
                foreach (DataRow row in AllAttractionsInDay_AttractionTable.Rows)
                {
                    Connect.Connect_ExecuteNonQuery("DELETE FROM Day_Attraction WHERE Attraction_ID = " + row["Attraction_ID"]);
                }

                // Day_Transportation הסרת התחבורה מטבלת 
                DataTable AllAttractionsInDay_TransportationTable = Connect.Connect_DataTable("Select * from Day_Transportation WHERE ToAttraction = " + Attraction_ID, "TransportationToRemove"); // כל העמודות המכילות את האטרקציה בטבלת יום - אטרקציה
                foreach (DataRow row in AllAttractionsInDay_TransportationTable.Rows)
                {
                    Connect.Connect_ExecuteNonQuery("DELETE FROM Day_Transportation WHERE ToAttraction = " + (int)row["ToAttraction"]);
                }
            }

            // אם הכתובת השתנתה - שינוי סדר האטרקציות
            if (OldAddress != Attraction_Address)
            {
                // לאחר שהאטרקציה הוספה - סידור מחדש של המסלול האופטימלי של האטרקציות *כולן* - לפי קרבה לארטקציות אחרות
                List<Attraction> attractions = GetAttractionsFromDataBase(); // שליפת כל האטרקציות ממסד הנתונים
                attractions = FindEfficientPath(attractions); // סידור האטרקציות
                UpdatePathToDataBase(attractions); // עדכון במסד
            }

            // אם סוג האטרקציה הקודם היה חלק מסוג החופשה " טבע" ועכשיו הוא לא - מוחק את הנתונים מטבלת הרחבת נתוני אטרקציית טבע - מחיקת ערכים
            if ((Attraction_KilometersNumber == "" || Attraction_Difficulty == "") && (Connect.Connect_ExecuteScalar("SELECT * FROM NatureAttraction WHERE Attraction_ID = " + Attraction_ID) != null))
                Connect.Connect_ExecuteNonQuery("DELETE FROM NatureAttraction WHERE Attraction_ID = " + Attraction_ID); // מחיקת הרחבת נתוני אטרקציית טבע

            // אם סוג החופשה של סוג האטרקציה מסוג טיול בטבע קיים במסד- מוסיף לטבלת "אטרקציה בטבע" ערכים - עדכון ערכים
            if ((Attraction_KilometersNumber != "" && Attraction_Difficulty != "") && (Connect.Connect_ExecuteScalar("SELECT * FROM NatureAttraction WHERE Attraction_ID = " + Attraction_ID) != null)) // אם הוכנסו ערכים = מסוג חופשה טיול בטבע
            {
                s = "UPDATE NatureAttraction SET  NatureAttraction_Difficulty = '" + Attraction_Difficulty + "',  NatureAttraction_KilometersNumber =  " + Attraction_KilometersNumber + " where Attraction_ID = " + Attraction_ID;
                Connect.Connect_ExecuteNonQuery(s); // הכנסת ערכים למסד
            }

            // אם סוג החופשה של סוג האטרקציה מסוג טיול בטבע ולא היה קודם - לא קיים במסד- מוסיף לטבלת "אטרקציה בטבע" ערכים - הוספת ערכים
            if ((Attraction_KilometersNumber != "" && Attraction_Difficulty != "") && (Connect.Connect_ExecuteScalar("SELECT * FROM NatureAttraction WHERE Attraction_ID = " + Attraction_ID) == null)) // אם הוכנסו ערכים = מסוג חופשה טיול בטבע
            {
                s = "INSERT INTO NatureAttraction (Attraction_ID, NatureAttraction_Difficulty, NatureAttraction_KilometersNumber ) " +
                    "VALUES (" + Attraction_ID + ", '" + Attraction_Difficulty + "', " + Attraction_KilometersNumber + ")";
                Connect.Connect_ExecuteNonQuery(s); // הכנסת ערכים למסד
            }

            // הוספת תמונות נוספות למסד
            // הוספת האטרקציה לטבלת תמונות נוספות אם יש 
            foreach (DataRow row in AddPhotoTable.Rows)
            {
                s = "INSERT INTO Photos (Attraction_ID, FileLocation) VALUES (" + Attraction_ID + ", '" + row["FileLocation"] + "')";
                Connect.Connect_ExecuteNonQuery(s); // הכנסת תמונה למסד
            }

            // מחיקת תמונות נוספות מהמסד - אם יש
            foreach (DataRow row in RemovePhotoTable.Rows)
            {
                Connect.Connect_ExecuteNonQuery("DELETE FROM Photos WHERE מזהה = " + row["מזהה"]); // מחיקת הרחבת נתוני אטרקציית טבע
            }
        }

        //מחיקת אטרקציה
        public static void RemoveAttraction(int Attraction_ID)
        {
            //הפיכה ללא ואליד
            Connect.Connect_ExecuteNonQuery("UPDATE Attraction SET Attraction_Valid = " + false + " WHERE Attraction_ID = "+ Attraction_ID);

            //הסרת האטרקציה מהמסלולים
            // Day_Attraction הסרת האטרקצית מטבלת 
            DataTable AllAttractionsInDay_AttractionTable = Connect.Connect_DataTable("Select * from Day_Attraction WHERE Attraction_ID = " + Attraction_ID, "AttractionsToRemove"); // כל העמודות המכילות את האטרקציה בטבלת יום - אטרקציה
            foreach (DataRow row in AllAttractionsInDay_AttractionTable.Rows)
            {
                Connect.Connect_ExecuteNonQuery("DELETE FROM Day_Attraction WHERE Attraction_ID = " + row["Attraction_ID"]);
            }

            // Day_Transportation הסרת התחבורה מטבלת 
            DataTable AllAttractionsInDay_TransportationTable = Connect.Connect_DataTable("Select * from Day_Transportation WHERE ToAttraction = " + Attraction_ID, "TransportationToRemove"); // כל העמודות המכילות את האטרקציה בטבלת יום - אטרקציה
            foreach (DataRow row in AllAttractionsInDay_TransportationTable.Rows)
            {
                Connect.Connect_ExecuteNonQuery("DELETE FROM Day_Transportation WHERE ToAttraction = " + (int)row["ToAttraction"]);
            }

            // מחיקת תמונות נוספות מהמסד - אם יש
            DataTable AllAttractionsPhotosInPhotosTable = Connect.Connect_DataTable("Select * from Photos WHERE Attraction_ID = " + Attraction_ID, "PhotosToRemove");
            foreach (DataRow row in AllAttractionsPhotosInPhotosTable.Rows)
            {
                Connect.Connect_ExecuteNonQuery("DELETE FROM Photos WHERE Attraction_ID = " + row["Attraction_ID"]); // מחיקת הרחבת נתוני אטרקציית טבע
            }
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
                a.Attraction_ID = (int)reader["Attraction_ID"];
                a.Attraction_Name = reader["Attraction_Name"].ToString();
                a.Attraction_TypeID = reader["AttractionType_ID"].ToString();
                a.Attraction_TypeName = reader["AttractionType_Type"].ToString();
                a.Attraction_MinAge = reader["Attraction_MinAge"].ToString();
                a.Attraction_MaxAge = reader["Attraction_MaxAge"].ToString();
                a.Attraction_Price = reader["Attraction_Price"].ToString();
                a.Attraction_Duration = (int)reader["Attraction_Duration"];
                a.Attraction_Address = reader["Attraction_Address"].ToString();
                a.Attraction_Gmail = reader["Attraction_Gmail"].ToString();
                a.Attraction_PhonNumber = reader["Attraction_PhoneNumber"].ToString();
                a.Attraction_Text = reader["Attraction_Text"].ToString();
                a.Attraction_OpeningHours = reader["Attraction_OpeningHour"].ToString();
                a.Attraction_ClosingHours = reader["Attraction_ClosingHour"].ToString();
                a.Attraction_Photo = reader["Attraction_Photo"].ToString();
            }
            Conn.Close();

            if ((int)(Connect.Connect_ExecuteScalar("SELECT VacationType_ID FROM AttractionType where AttractionType_ID = " + a.Attraction_TypeID)) == 3)
            {
                Conn.Open();
                command = new OleDbCommand("SELECT * FROM NatureAttraction where Attraction_ID = " + a.Attraction_ID, Conn);
                reader = command.ExecuteReader();
                if (reader.Read())
                {
                    a.Attraction_KilometersNumber = (int)reader["NatureAttraction_KilometersNumber"];
                    a.Attraction_Difficulty = reader["NatureAttraction_Difficulty"].ToString();
                }
                Conn.Close();
            }
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
            OleDbCommand command = new OleDbCommand("SELECT * FROM Attraction where Attraction_ID <> 67", Conn);
            Conn.Open();
            OleDbDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Attraction a = new Attraction((int)reader["Attraction_ID"] , Convert.ToDouble(reader["Attraction_Latitude"]),Convert.ToDouble(reader["Attraction_Longitude"]), (int)reader["Attraction_Duration"], (int)reader["Attraction_PathOrder"]);
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
