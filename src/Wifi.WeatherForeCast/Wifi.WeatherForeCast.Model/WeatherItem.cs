namespace Wifi.WeatherForeCast.Model
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
        public string SymbolCode { get; set; }
    }
}
