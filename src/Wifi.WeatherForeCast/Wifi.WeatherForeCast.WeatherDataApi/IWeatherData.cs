using Wifi.WeatherForeCast.Model;

namespace Wifi.WeatherForeCast.WeatherDataApi
{
    public interface IWeatherData
    {
        double Latitude { get; set; }
        double Longitude { get; set; }

        IQueryable<WeatherItem> GetAllDataAtHourOfDayForTheNext_n_Days(int hourOfDay, int n_Days);
        Task<IQueryable<WeatherItem>> GetAllDataForRemainingDay();
        Task RefreshData();
        Task RefreshDataWithNewLocation(double latitude, double longitude);
    }
}