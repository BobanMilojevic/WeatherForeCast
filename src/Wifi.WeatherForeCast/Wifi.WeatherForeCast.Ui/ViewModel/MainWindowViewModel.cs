using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Wifi.WeatherForeCast.Geodata.Apis;
using Wifi.WeatherForeCast.Geodata.Models;
using Wifi.WeatherForeCast.YrApi;

namespace Wifi.WeatherForeCast.Ui.ViewModel;

public class MainWindowViewModel : ObservableValidator
{
    private string _searchValue = "Feldkirch Austria";
    private string _result;

    public MainWindowViewModel()
    {
        SearchForWeatherDataCommand = new AsyncRelayCommand(SearchForWeatherData);
    }

    private async Task SearchForWeatherData()
    {
        GeodataApi geodata = new GeodataApi();

        Coordinates coordinates = geodata.MainApiForBoban(SearchValue);

        WeatherItem weatherItem = await HelperClass.GetInstantWeatherItem(coordinates.Latitude, coordinates.Longitude);

        Result = weatherItem.Temperature.ToString();
    }


    public string SearchValue
    {
        get => _searchValue;
        set
        {
            SetProperty(ref _searchValue, value);
        }
    }

    public string Result 
    {
        get => _result;
        set
        {
            SetProperty(ref _result, value);
        } }
    
    public IAsyncRelayCommand SearchForWeatherDataCommand { get; }
}