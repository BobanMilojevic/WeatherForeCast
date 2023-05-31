using Wifi.WeatherForeCast.Model;

namespace Wifi.WeatherForeCast.Repositories;

public interface IJsonRepository
{
    void SaveSettings(UiSettings settings);
    UiSettings LoadSettings();
}