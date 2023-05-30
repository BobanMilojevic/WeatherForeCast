using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wifi.WeatherForeCast.Model
{
    public class UiSettings
    {
        public bool IsDegree { get; set; }
        public string Location { get; set; }
        public int ForecastDays { get; set; }
        public string Language { get; set; }

        public UiSettings() { }

        public UiSettings(bool isDegree, string location, int forecastDays, string language)
        {
            IsDegree = isDegree;
            Location = location;
            ForecastDays = forecastDays;
            Language = language;
        }
    }
}
