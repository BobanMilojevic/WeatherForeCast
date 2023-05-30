using Newtonsoft.Json;
using System.Security.Cryptography.X509Certificates;
using Wifi.WeatherForeCast.Model;

namespace Wifi.WeatherForeCast.Repositories
{
    public class JsonRepository
    {
        public void SaveSettings(UiSettings settings, string fileName = "c:\\temp\\WeatherForecastSettings.json")
        {

            if (settings == null || string.IsNullOrEmpty(fileName)) { return; }

            string json = JsonConvert.SerializeObject(settings);

            File.WriteAllText(fileName, json);
        }


        public UiSettings LoadSettings(string fileName = "c:\\temp\\WeatherForecastSettings.json")
        {
            string json = File.ReadAllText(fileName);

            if (string.IsNullOrEmpty(json)) { return null; }

            UiSettings settings = new UiSettings();
            settings = JsonConvert.DeserializeObject<UiSettings>(json);
            return settings;
        }



    }
}