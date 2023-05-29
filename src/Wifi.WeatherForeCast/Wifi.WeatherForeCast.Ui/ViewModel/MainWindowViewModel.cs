using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Wifi.WeatherForeCast.Business;
using Wifi.WeatherForeCast.Model;

namespace Wifi.WeatherForeCast.Ui.ViewModel;

public class MainWindowViewModel : ObservableValidator
{
    private string _searchValue;
    private string _dayPeriod;
    private WeatherItem _selectedWeatherItem;
    private List<WeatherItem> _weatherRemainingDayItemsList;

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

    public string DayPeriod
    {
        get => _dayPeriod;
        set;
    }

    public WeatherItem SelectedWeatherItem
    {
        get
        {
            return _selectedWeatherItem; 
        }
        set
        {
            SetProperty(ref _selectedWeatherItem, value);
        }
    }

    public List<WeatherItem> WeatherRemainingDayItemsList
    {
        get => _weatherRemainingDayItemsList; 
        set
        {
            SetProperty(ref _weatherRemainingDayItemsList, value);
        }
    }
    
    private async Task LoadDataAsync()
    {
        WeatherItemService service = new WeatherItemService();
        this.WeatherRemainingDayItemsList = new List<WeatherItem>();
        var items = await service.GetWeatherDataOfRemainingDay();

        foreach (var item in items)
        {
            if (item.DateTime.Hour == 6)
            {
                this.WeatherRemainingDayItemsList.Add(item);
            }
            if (item.DateTime.Hour == 12)
            {
                this.WeatherRemainingDayItemsList.Add(item);
            }
            if (item.DateTime.Hour == 18)
            {
                this.WeatherRemainingDayItemsList.Add(item);
            }
            if (item.DateTime.Hour == 23)
            {
                this.WeatherRemainingDayItemsList.Add(item);
            }
        }

        this.SelectedWeatherItem = this.WeatherRemainingDayItemsList.First();

    }
}