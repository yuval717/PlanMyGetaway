using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Project_01
{
    public class TravelTimeCalculator
    {
        public static string apiKey = "AphGf7K-TJ30vZssMVV1eQVF0lB-mOFjYPX8SazonHDwXtFi3l3IBmvsdmSRYouw"; // מפתח API;
        public static HttpClient _httpClient = new HttpClient();

        public static async Task<ArrayList> CalculateTravelTime(Attraction attraction1, Attraction attraction2)
        {
            string apiUrl = $"https://dev.virtualearth.net/REST/v1/Routes?wp.0={attraction1.Attraction_Latitude},{attraction1.Attraction_Longitude}&wp.1={attraction2.Attraction_Latitude},{attraction2.Attraction_Longitude}&key={apiKey}";

            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

            //if (!response.IsSuccessStatusCode)
            //{
            //    throw new Exception($"Failed to retrieve data from Bing Maps API. Status code: {response.StatusCode}");
            //}

            string responseBody = await response.Content.ReadAsStringAsync();

            // Parse JSON response
            JObject jsonResponse = JObject.Parse(responseBody);

            // Create ArrayList to store travel times
            ArrayList travelTimes = new ArrayList();

            // Extract travel times for different transportation modes
            travelTimes.Add((int)jsonResponse["resourceSets"][0]["resources"][0]["travelDurationTraffic"]); // Car
            travelTimes.Add("Car");
            travelTimes.Add((int)jsonResponse["resourceSets"][0]["resources"][0]["travelDuration"]); // Walking
            travelTimes.Add("Walking");

            // Extract transit travel time if available
            JToken transitToken = jsonResponse["resourceSets"][0]["resources"][0]["routeLegs"].FirstOrDefault(leg => leg["travelMode"].ToString() == "Transit");
            if (transitToken != null)
            {
                travelTimes.Add((int)transitToken["travelDuration"]); // Transit
            }
            else
            {
                // If transit mode is not available, set the transit travel time to a large value or indicate it's not available
                travelTimes.Add(-1); // Or any other appropriate indication
            }
            travelTimes.Add("Transit");

            for (int i = 0; i < travelTimes.Count; i++)
            {
                //if ((int)travelTimes[i] != -1)
                //{
                //    int remainder = number % 10;
                //    if (remainder < 5)
                //    {
                //        return number - remainder;
                //    }
                //    else
                //    {
                //        return number + (10 - remainder);
                //    }
                //}
            }

            return travelTimes;
        }
    }
}

