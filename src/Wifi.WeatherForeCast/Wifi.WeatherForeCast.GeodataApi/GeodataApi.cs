using System.Net;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Wifi.WeatherForeCast.Geodata.HelperJsonObject;
using Wifi.WeatherForeCast.Model;

namespace Wifi.WeatherForeCast.Geodata
{
    public class GeodataApi : IGeodataApi
    {
      
        public async Task<IQueryable<Coordinate>> GetCoordinates(string city)
        {
            if (string.IsNullOrWhiteSpace(city))
            {
                return null;
            }

            string query = String.Format("https://nominatim.openstreetmap.org/search?city={0}&format=jsonv2", city);
            string result = await GetRequest(query); //returns a stringified array of js objects

            var list = JsonConvert.DeserializeObject<List<GeoDataApiJsonModel>>(result);

            if (list == null)
            {
                return null;
            }
            
            List<Coordinate> coordinates = new List<Coordinate>();
            
            foreach (var item in list)
            {
                coordinates.Add(new Coordinate()
                {
                    City = item.display_name,
                    Latitude = item.lat,
                    Longitude = item.lon
                });
            }

            return coordinates.AsQueryable();

        }

        private async Task<string> GetRequest(string url)
        {
            HttpClient client = new HttpClient();
            //client.BaseAddress = new Uri(requestString);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.UserAgent.Add(
                new ProductInfoHeaderValue("PostmanRuntime", "7.32.2"));
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync(url);
            return await response.Content.ReadAsStringAsync();

            // Root wetterdaten = JsonConvert.DeserializeObject<Root>(jsonString);
            //
            // return wetterdaten;
            //
            // HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            // request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            // request.UserAgent = @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.106 Safari/537.36";
            // using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            // using (Stream stream = response.GetResponseStream())
            // using (StreamReader reader = new StreamReader(stream))
            // {
            //     return reader.ReadToEnd();
            // }
        }
    }
}
