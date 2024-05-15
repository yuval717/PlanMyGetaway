using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.OleDb;
using System.Web.UI.WebControls;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Project_01
{
    public class OrderService
    {

        public static Attraction FindClosestAttraction(DataTable attractionTable, Attraction targetAttraction)
        {
            Attraction closestAttraction = null;
            double minDistance = double.MaxValue;

            foreach (DataRow row in attractionTable.Rows)
            {
                int attractionID = Convert.ToInt32(row["Attraction_ID"]);
                double attractionLat = Convert.ToDouble(row["Attraction_Latitude"]);
                double attractionLng = Convert.ToDouble(row["Attraction_Longitude"]);
                int AttractionDuration = Convert.ToInt32(row["Attraction_Duration"]);
                int AttractionPathOrder = Convert.ToInt32(row["Attraction_PathOrder"]);

                // Calculate distance between the target attraction and the attraction in the DataSet
                double distance = CalculateDistance(targetAttraction.Attraction_Latitude, targetAttraction.Attraction_Longitude, attractionLat, attractionLng);

                // Update closest attraction if this one is closer
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestAttraction = new Attraction(attractionID, attractionLat, attractionLng, AttractionDuration, AttractionPathOrder);
                }
            }
            return closestAttraction;
        }

        public static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371; // Radius of the Earth in kilometers
            double dLat = DegreeToRadian(lat2 - lat1);
            double dLon = DegreeToRadian(lon2 - lon1);
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(DegreeToRadian(lat1)) * Math.Cos(DegreeToRadian(lat2)) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double distance = R * c;
            return distance;
        }

        public static double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }


        public async static Task<Transportation> Transport(Attraction FromAttraction, Attraction ToAttraction)//Transportation - "יצירת עצם "דרך תחבורה 
        {
            ArrayList arr = await TravelTimeCalculator.CalculateTravelTime(FromAttraction, ToAttraction); //במקום האפס - רכב. במקום הראשון - הליכה. במקום השני - תחבצ ArrayList זמן הגעה בין 2 נקודות - מחזיר 
            double MinTravleTime = int.MaxValue;
            string MinTravleTimeType = "";
            for (int i = 0; i < arr.Count; i += 2)// המרת הזמן בין 2 נקודות לדקות משניות - 
            {
                if ((int)arr[i] != -1)
                {
                    if ((int)arr[i] < MinTravleTime)
                    {
                        MinTravleTime = (int)arr[i];
                        MinTravleTimeType = (string)arr[i + 1];
                    }
                }
            }
            return (new Transportation(FromAttraction, ToAttraction, (int)(MinTravleTime/60), MinTravleTimeType));
        }

    }
}