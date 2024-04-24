using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Data.OleDb;
using System.Web.UI.WebControls;

namespace Project_01
{
    public class BingMapsGeocoder
    {
        private const string BingMapsApiKey = "AphGf7K-TJ30vZssMVV1eQVF0lB-mOFjYPX8SazonHDwXtFi3l3IBmvsdmSRYouw"; // מפתח API
        private const string BingMapsGeocodingApiUrl = "https://dev.virtualearth.net/REST/v1/Locations"; //API קישור ל

        //  עם קורדינטות - האיבר הראשון רוחב, השני אורך ArrayList מקבל כתובת - מחזיר
        public static async Task<ArrayList> GetCoordinatesByAddressAsync(string address)
        {
            // Geocode the address to get latitude and longitude
            var (latitude, longitude) = await GeocodeAddressAsync(address);
            ArrayList arr = new ArrayList();
            arr.Add(latitude);
            arr.Add(longitude);
            return arr;
        }

        //ArrayList מקבל כתובת מחזיר קורדינטות - ללא תיווך ל
        private static async Task<(double Latitude, double Longitude)> GeocodeAddressAsync(string address)
        {
            // Creating a HttpClient instance within a using block ensures proper disposal after usage
            using (var httpClient = new HttpClient())
            {
                // Constructing query parameters including the address and Bing Maps API key
                var queryParams = $"?q={Uri.EscapeDataString(address)}&key={BingMapsApiKey}";

                // Sending an asynchronous GET request to the Bing Maps Geocoding API endpoint
                var response = await httpClient.GetAsync($"{BingMapsGeocodingApiUrl}{queryParams}");

                // Ensuring that the response status code indicates success
                response.EnsureSuccessStatusCode();

                // Reading the response content as a string asynchronously
                var json = await response.Content.ReadAsStringAsync();

                // Deserializing the JSON response into a dynamic object
                dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject(json);

                // Accessing the resourceSets property of the dynamic object
                var resourceSets = result.resourceSets;

                // Checking if the resourceSets property is not null and contains at least one element
                if (resourceSets != null && resourceSets.Count > 0)
                {
                    // Accessing the resources property of the first resource set
                    var resources = resourceSets[0].resources;

                    // Checking if the resources property is not null and contains at least one element
                    if (resources != null && resources.Count > 0)
                    {
                        // Accessing the coordinates property of the first resource
                        var location = resources[0].point.coordinates;

                        // Returning the coordinates as a tuple of doubles
                        return ((double)location[0], (double)location[1]);
                    }
                }
                // Returning default coordinates (0, 0) if the address is not found or there's an issue with the response
                return (0, 0);
            }
        }


        // הסבר על הפעולות ומדוע משתמשים בפעולה אסינכרונית
        //The methods `GetCoordinatesByAddressAsync` and `GeocodeAddressAsync` are marked as `async` because they perform asynchronous
        //operations.Let's break down why:

        //1. **HttpClient.GetAsync**: This method sends an asynchronous GET request to the specified URI.It returns a Task<HttpResponseMessage>,
        //which represents the asynchronous operation of sending the request and receiving the response.By using the `await` keyword before
        //`httpClient.GetAsync`, the method asynchronously waits for the completion of this operation without blocking the thread.

        //2. **response.Content.ReadAsStringAsync**: This method asynchronously reads the response content as a string. Similarly, it returns a
        //Task<string>, representing the asynchronous operation of reading the response content.Again, using the `await` keyword allows the
        //method to asynchronously wait for this operation to complete.

        //3. **Deserialization**: After receiving the JSON response, the code deserializes it into a dynamic object using
        //Newtonsoft.Json.JsonConvert.DeserializeObject. Deserialization can involve significant processing, especially for large
        //JSON payloads.Marking this operation as asynchronous ensures that it doesn't block the calling thread while waiting for the
        //deserialization to finish.

        //In summary, marking these methods as `async` allows them to perform I/O-bound operations such as HTTP requests
        //and asynchronous file I/O without blocking the calling thread, thus improving the responsiveness and scalability of the application.

    }
}
