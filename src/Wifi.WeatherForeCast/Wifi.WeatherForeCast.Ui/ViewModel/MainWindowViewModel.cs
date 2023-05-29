using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Wifi.WeatherForeCast.Business;
using Wifi.WeatherForeCast.Model;

namespace Wifi.WeatherForeCast.Ui.ViewModel;

public class MainWindowViewModel : ObservableValidator
{
    private string _searchValue = "Feldkirch Austria";
    private string _result;
    private WeatherItem _selectedWeatherItem;
    
    public MainWindowViewModel()
    {
        LoadDataAsync();
    }
    
    public string SearchValue
    {
        get => _searchValue;
        set
        {
            SetProperty(ref _searchValue, value);
        }
    }

    public WeatherItem SelectedWeatherItem
    {
        get => _selectedWeatherItem; 
        set
        {
            SetProperty(ref _selectedWeatherItem, value);
        }
    }
    
    private async Task LoadDataAsync()
    {
        WeatherItemService service = new WeatherItemService();
        var items = await service.GetWeatherDataOfRemainingDay();

        _selectedWeatherItem = items.FirstOrDefault();
    }
}