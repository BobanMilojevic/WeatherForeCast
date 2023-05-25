using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wifi.WeatherForeCast.YrApi
{
    public class WeatherItem
    {
        public DateTime DateTime { get; set; }
        public double Temperature { get ; set; }
       // public double MinTemperature { get; set; }
        //public double MaxTemperature { get; set; }
        public double PerceivedTemperature { get; set; }
        public double Humidity { get; set; }
        public double Pressure { get; set; }
        public double WindSpeed { get; set; }
    }
}
