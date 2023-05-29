using Wifi.WeatherForeCast.Model;
using Wifi.WeatherForeCast.WeatherDataApi;

namespace Wifi.WeatherForeCast.Business;

public class WeatherItemService
{
    private WeatherData _weatherData;

    public WeatherItemService()
    {
        _weatherData = new WeatherData();
    }

    public async Task<List<WeatherItem>> GetWeatherDataOfRemainingDay()
    {
        return await _weatherData.GetAllDataForRemainingDay(47.23306, 9.6);
    }
}