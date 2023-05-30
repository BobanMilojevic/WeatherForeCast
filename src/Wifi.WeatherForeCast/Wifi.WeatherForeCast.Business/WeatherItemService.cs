using Wifi.WeatherForeCast.Model;
using Wifi.WeatherForeCast.WeatherDataApi;

namespace Wifi.WeatherForeCast.Business;

public class WeatherItemService
{
    private IWeatherData _IWeatherData;

    public WeatherItemService()
    {
        _IWeatherData = new WeatherData(47.23306, 9.6);
    }

    public async Task<IQueryable<WeatherItem>> GetWeatherDataOfRemainingDay()
    {
        return await _IWeatherData.GetAllDataForRemainingDay();
    }

    public async Task<IQueryable<WeatherItem>> GetWeatherDataOfNDays()
    {
        return _IWeatherData.GetAllDataAtHourOfDayForTheNext_n_Days(12, 8);
    }
}