using Wifi.WeatherForeCast.Model;
using Wifi.WeatherForeCast.WeatherDataApi;

namespace Wifi.WeatherForeCast.Business;

public class WeatherItemService
{
    private WeatherData _weatherData;

    public WeatherItemService()
    {
        _weatherData = new WeatherData(47.23306, 9.6);
    }

    public async Task<IQueryable<WeatherItem>> GetWeatherDataOfRemainingDay()
    {
        return await _weatherData.GetAllDataForRemainingDay();
    }

    public async Task<IQueryable<WeatherItem>> GetWeatherDataOfNDays()
    {
        return _weatherData.GetAllDataAtHourOfDayForTheNext_n_Days(12, 8);
    }
}