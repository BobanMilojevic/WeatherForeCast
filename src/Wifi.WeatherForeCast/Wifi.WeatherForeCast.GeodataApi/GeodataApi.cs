using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Wifi.WeatherForeCast.Geodata.Interfaces;
using Wifi.WeatherForeCast.Model;

namespace Wifi.WeatherForeCast.Geodata.Apis
{
    public class GeodataApi : IGeodataApi
    {
        public Coordinates MainApiForBoban(string city)
        {
            if (string.IsNullOrWhiteSpace(city))
            {
                return GetEmptyCityCoordinates();
            }

            return GetCoordinates(city);
        }

        public Coordinates GetEmptyCityCoordinates()
        {
            var coord = new Coordinates();
            coord.IsValid = false;
            coord.Latitude = 0;
            coord.Longitude = 0;
            return coord;
        }

        public Coordinates GetCoordinates(string city)
        {
            string query = String.Format("https://nominatim.openstreetmap.org/search.php?q={0}&format=jsonv2", city);
            string result = GetRequest(query); //returns a stringified array of js objects

            var list = JsonConvert.DeserializeObject<List<Place>>(result);

            if (list == null || list.Count == 0)
            {
                return GetEmptyCityCoordinates();
            }

            var place = list.FirstOrDefault();

            if (place == null)
            {
                return GetEmptyCityCoordinates(); 
            }

            var coordinates = GetCoordinatesFromPlace(place);

            return coordinates;

        }

        public string GetRequest(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.UserAgent = @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.106 Safari/537.36";
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        public Coordinates GetCoordinatesFromPlace(Place place)
        {
            var coord = new Coordinates();

            //wenn hier ein error auftritt einfach die umwandlung herausnehmen ( . zu ,)
            var longitude = place.lon.Replace(".", ",");
            var latitude = place.lat.Replace(".", ",");

            coord.Longitude = Convert.ToDouble(longitude);
            coord.Latitude = Convert.ToDouble(latitude);
            coord.IsValid = true;

            return coord;
        }

        
    }
}
