using Wifi.WeatherForeCast.Model;
using Wifi.WeatherForeCast.Repositories;

namespace Wifi.WeatherForeCast.Business;

public class SaveLoadService
{
    private IJsonRepository _IJsonRepository;

    public SaveLoadService()
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