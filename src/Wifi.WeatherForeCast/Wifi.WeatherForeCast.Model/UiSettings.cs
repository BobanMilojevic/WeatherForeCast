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
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}
