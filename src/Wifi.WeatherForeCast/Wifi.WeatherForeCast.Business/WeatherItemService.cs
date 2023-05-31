using Wifi.WeatherForeCast.Model;
using Wifi.WeatherForeCast.WeatherDataApi;

namespace Wifi.WeatherForeCast.Business;

public class WeatherItemService
{
    private IWeatherData _IWeatherData;

    public WeatherItemService()
    {
        
    }

    public async Task<IQueryable<WeatherItem>> GetWeatherDataOfRemainingDay(double latitude, double longitude)
    {
        _IWeatherData = new WeatherData(latitude, longitude);
        return await _IWeatherData.GetAllDataForRemainingDay();
    }

    public async Task<IQueryable<WeatherItem>> GetWeatherDataOfNDays(double latitude, double longitude)
    {
        _IWeatherData = new WeatherData(latitude, longitude);
        return _IWeatherData.GetAllDataAtHourOfDayForTheNext_n_Days(12, 8);
    }
}