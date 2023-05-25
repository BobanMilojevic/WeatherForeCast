using Newtonsoft.Json;
using System.ComponentModel;
using System.Net.Http.Headers;
using System.Drawing;

namespace Wifi.WeatherForeCast.YrApi
{
    public class HelperClass
    {
        public static async Task<Root> FetchData(double latitude = 59.93, double longitude = 10.72)
        {
            // https://api.met.no/weatherapi/locationforecast/2.0/compact?lat=59.93&lon=10.72&altitude=90
            // https://api.met.no/weatherapi/locationforecast/2.0/compact?lat=59.93&lon=10.72&altitude=90


            string requestString = GetRequestString(latitude.ToString().Replace(",", "."), longitude.ToString().Replace(",", "."));

            HttpClient client = new HttpClient();
            //client.BaseAddress = new Uri(requestString);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.UserAgent.Add(
                new ProductInfoHeaderValue("PostmanRuntime", "7.32.2"));
            client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync(requestString);
            string jsonString = await response.Content.ReadAsStringAsync();

            Root wetterdaten = JsonConvert.DeserializeObject<Root>(jsonString);

            return wetterdaten;

        }


        public static string GetRequestString(string latitude, string longitude, string altitude = "0")
        {
            string baseUrl = "https://api.met.no/weatherapi/locationforecast/2.0/compact?lat=";
            return $"{baseUrl}{latitude}&lon={longitude}"; // &altitude={altitude}";   
        }

        public static List<Timeseries> GetAllDataAtHourOfDay(Root wetterdaten, int hourOfDay)
        {
            List<Timeseries> data = new List<Timeseries>();
            data = wetterdaten.properties.timeseries.Where(x => x.time.Hour == hourOfDay).ToList();
            return data;
        }

        public static List<Timeseries> GetAllDataAtHourOfDayForTheNext_n_Days(Root wetterdaten, int hourOfDay, int n_Days)
        {
            List<Timeseries> data = new List<Timeseries>();
            data = wetterdaten.properties.timeseries.Where(x => x.time.Hour == hourOfDay && x.time.Date > DateTime.Now.Date).Take(n_Days).ToList();
            return data;
        }

        public static List<Timeseries> GetAllDataForRemainingDay(Root wetterdaten)
        {
            return wetterdaten.properties.timeseries.Where(x => x.time.Date == DateTime.Now.Date).ToList();
        }

        //From ChatGpt 24.05.2023
        public static double CalculatePerceivedTemperature(double temperature, double windSpeed)
        {
            double perceivedTemperature = 13.12 + 0.6215 * temperature - 11.37 * Math.Pow(windSpeed, 0.16)
                                          + 0.3965 * temperature * Math.Pow(windSpeed, 0.16);
            return perceivedTemperature;
        }

        public static WeatherItem ConvertFormYrDetailsToWeaterItem(Timeseries item)
        {
            WeatherItem weatherItem = new();

            weatherItem.DateTime = item.time;
            weatherItem.Temperature = item.data.instant.details.air_temperature;
            weatherItem.WindSpeed = item.data.instant.details.wind_speed;
            weatherItem.PerceivedTemperature = CalculatePerceivedTemperature(weatherItem.Temperature, weatherItem.WindSpeed);
            weatherItem.Humidity = item.data.instant.details.relative_humidity;
            weatherItem.Pressure = item.data.instant.details.air_pressure_at_sea_level;

            return weatherItem;

        }

        public static async Task<WeatherItem> GetInstantWeatherItem(double lattitude, double longitude)
        {
            Root YrWeatherData = new();
            WeatherItem weatherItem = new();
            YrWeatherData = await FetchData(lattitude, longitude);

            weatherItem = ConvertFormYrDetailsToWeaterItem(YrWeatherData.properties.timeseries[0]);

            return weatherItem;


        }


    }
}