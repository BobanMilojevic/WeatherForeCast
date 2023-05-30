using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Wifi.WeatherForeCast.Model;
using Wifi.WeatherForeCast.WeatherDataApi.HelperJsonObject;

namespace Wifi.WeatherForeCast.WeatherDataApi
{
    public class WeatherData : IWeatherData
    {

        private Root OriginalWeatherData { get; set; }
        public double Latitude { get; set; } = 47.3670017;
        public double Longitude { get; set; } = 9.6881199;

        /// <summary>
        /// Konstruktor ruft sofort die Daten für den übergebenen Standort ab.
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        public WeatherData(double latitude, double longitude)
        {
            this.Latitude = latitude;
            this.Longitude = longitude;

            this.OriginalWeatherData = new();
            this.OriginalWeatherData = Task.Run(async () => await FetchData(latitude, longitude)).Result;
        }




        /// <summary>
        /// refresh Data without changing the location
        /// </summary>
        /// <returns></returns>
        public async Task RefreshData()
        {
            this.OriginalWeatherData = await FetchData(Latitude, Longitude);
        }


        /// <summary>
        /// change the location and refesh the data
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        public async Task RefreshDataWithNewLocation(double latitude, double longitude)
        {
            this.OriginalWeatherData = await FetchData(latitude, longitude);
        }


        /// <summary>
        /// Get weather informations for the nex n day at a specific hour of the day (0-23h)
        /// </summary>
        /// <param name="hourOfDay"></param>
        /// <param name="n_Days"></param>
        /// <returns></returns>
        public IQueryable<WeatherItem> GetAllDataAtHourOfDayForTheNext_n_Days(int hourOfDay, int n_Days)
        {
            List<Timeseries> data = new List<Timeseries>();
            data = OriginalWeatherData.properties.timeseries.Where(x => x.time.Hour == hourOfDay && x.time.Date > DateTime.Now.Date).Take(n_Days).ToList();

            List<WeatherItem> weatherItems = new List<WeatherItem>();
            foreach (var item in data)
            {
                WeatherItem weatherItem = new WeatherItem();
                weatherItem = ConvertFormYrDetailsToWeaterItem(item);
                weatherItems.Add(weatherItem);
            }
            return weatherItems.AsQueryable();
        }


        /// <summary>
        /// Get Weather informations for the rest of the current day
        /// </summary>
        /// <returns></returns>
        public IQueryable<WeatherItem> GetAllDataForRemainingDay()
        {
            List<Timeseries> data = new List<Timeseries>();
            data = OriginalWeatherData.properties.timeseries.Where(x => x.time.Date == DateTime.Now.Date).ToList();

            List<WeatherItem> weatherItems = new List<WeatherItem>();
            foreach (var item in data)
            {
                WeatherItem weatherItem = new WeatherItem();
                weatherItem = ConvertFormYrDetailsToWeaterItem(item);
                weatherItems.Add(weatherItem);
            }
            return weatherItems.AsQueryable();
        }



        #region private Methods
        private async Task<Root> FetchData(double latitude = 59.93, double longitude = 10.72)
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

        private string GetRequestString(string latitude, string longitude, string altitude = "0")
        {
            string baseUrl = "https://api.met.no/weatherapi/locationforecast/2.0/compact?lat=";
            return $"{baseUrl}{latitude}&lon={longitude}"; // &altitude={altitude}";   
        }

        //From ChatGpt 24.05.2023
        private static double CalculatePerceivedTemperature(double temperature, double windSpeed, double humidity)
        {
            double perceivedTemperature;

            if (temperature < 10)
            {
                perceivedTemperature = 13.12 + 0.6215 * temperature - 11.37 * Math.Pow(windSpeed, 0.16)
                                              + 0.3965 * temperature * Math.Pow(windSpeed, 0.16);
            }
            else
            {

                perceivedTemperature = -8.784695 + 1.61139411 * temperature
                    + 2.338549 * humidity - 0.14611605 * temperature * humidity
                    - 0.012308094 * temperature * temperature
                    - 0.016424828 * humidity * humidity
                    + 0.002211732 * temperature * temperature * humidity
                    + 0.00072546 * temperature * humidity * humidity
                    - 0.000003582 * temperature * temperature * humidity * humidity;
            }
            return perceivedTemperature;
        }

        private static WeatherItem ConvertFormYrDetailsToWeaterItem(Timeseries item)
        {
            WeatherItem weatherItem = new();

            weatherItem.DateTime = item.time;
            weatherItem.Temperature = item.data.instant.details.air_temperature;
            weatherItem.WindSpeed = item.data.instant.details.wind_speed;
            weatherItem.PerceivedTemperature = CalculatePerceivedTemperature(weatherItem.Temperature, weatherItem.WindSpeed, weatherItem.Humidity);
            weatherItem.Humidity = item.data.instant.details.relative_humidity;
            weatherItem.Pressure = item.data.instant.details.air_pressure_at_sea_level;
            weatherItem.SymbolCode = item.data.next_6_hours.summary.symbol_code;

            return weatherItem;

        }

        private async Task<WeatherItem> GetInstantWeatherItem(double lattitude, double longitude)
        {
            Root YrWeatherData = new();
            WeatherItem weatherItem = new();
            YrWeatherData = await FetchData(lattitude, longitude);

            weatherItem = ConvertFormYrDetailsToWeaterItem(YrWeatherData.properties.timeseries[0]);

            return weatherItem;


        }
        #endregion

    }
}