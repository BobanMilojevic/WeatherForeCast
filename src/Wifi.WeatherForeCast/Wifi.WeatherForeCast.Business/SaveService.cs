using Wifi.WeatherForeCast.Model;
using Wifi.WeatherForeCast.Repositories;

namespace Wifi.WeatherForeCast.Business;

public class SaveService
{
    private IJsonRepository _IJsonRepository;

    public SaveService()
    {
        _IJsonRepository = new JsonRepository();
    }

    public UiSettings LoadUiSettings()
    {
        return _IJsonRepository.LoadSettings();
    }

    public void SaveUiSetting(UiSettings settings)
    {
        _IJsonRepository.SaveSettings(settings);
    }
}