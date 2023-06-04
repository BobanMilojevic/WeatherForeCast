using System.Net;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Wifi.WeatherForeCast.Geodata.HelperJsonObject;
using Wifi.WeatherForeCast.Model;

namespace Wifi.WeatherForeCast.Geodata
{
    public class GeodataApi : IGeodataApi
    {
      
        public async Task<IQueryable<Coordinate>> GetCoordinates(string city, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(city))
            {
                return null;
            }

            string query = String.Format("https://nominatim.openstreetmap.org/search?city={0}&format=jsonv2", city.Replace(" ", "%20"));
            string result = await GetRequest(query, cancellationToken); //returns a stringified array of js objects

            if (cancellationToken.IsCancellationRequested)
            {
                return null;
            }
            
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

        public async Task<string> GetCity(double longitude, double latitude, CancellationToken cancellationToken)
        {
            string query = String.Format("https://nominatim.openstreetmap.org/reverse?lat={0}&lon={1}&format=jsonv2", latitude.ToString().Replace(",", "."), longitude.ToString().Replace(",", "."));
            string result = await GetRequest(query, cancellationToken); //returns a stringified array of js objects

            var coordinateJsonModel = JsonConvert.DeserializeObject<GeoDataApiJsonModel>(result);

            if (coordinateJsonModel == null)
            {
                return null;
            }

            return coordinateJsonModel.display_name;
        }

        private async Task<string> GetRequest(string url, CancellationToken cancellationToken)
        {
            HttpClient client = new HttpClient();
            //client.BaseAddress = new Uri(requestString);
            client.DefaultRequestHeaders.AcceptLanguage.Clear();
            client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("de-DE"));
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.UserAgent.Add(
                new ProductInfoHeaderValue("PostmanRuntime", "7.32.2"));
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync(url, cancellationToken);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
