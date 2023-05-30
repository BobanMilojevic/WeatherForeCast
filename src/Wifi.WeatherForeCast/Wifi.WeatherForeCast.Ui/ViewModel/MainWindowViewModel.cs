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
    private int _count;
    private string _searchValue;
    private string _dayPeriod;
    private string _iconSource;
    private WeatherItem _selectedWeatherItem;
    private ObservableCollection<WeatherItem> _weatherRemainingDayItemsList;
    private ObservableCollection<WeatherItem> _weatherNDaysItemsList;

    public MainWindowViewModel()
    {
        LoadDataAsync();

        NextWeatherItemCommand = new AsyncRelayCommand(NextWeatherItem);
        PreviousWeatherItemCommand = new AsyncRelayCommand(PreviousWeatherItem);
    }

    private async Task PreviousWeatherItem()
    {
        if(_count > 0)
        {
            _count--;
            this.SelectedWeatherItem = this.WeatherRemainingDayItemsList[_count];
            GetDayPeriod(this.SelectedWeatherItem);
            GetIconSource(this.SelectedWeatherItem);
        }
    }

    private async Task NextWeatherItem()
    {
        if(_count+1 < WeatherRemainingDayItemsList.Count)
        {
            _count++;
            this.SelectedWeatherItem = this.WeatherRemainingDayItemsList[_count];
            GetDayPeriod(this.SelectedWeatherItem);
            GetIconSource(this.SelectedWeatherItem);
        }
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
        set
        {
            SetProperty(ref _dayPeriod, value);
        }
    }

    public string IconSource
    {
        get => _iconSource;
        set
        {
            SetProperty(ref _iconSource, value);
        }
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

    public ObservableCollection<WeatherItem> WeatherRemainingDayItemsList
    {
        get => _weatherRemainingDayItemsList; 
        set
        {
            SetProperty(ref _weatherRemainingDayItemsList, value);
        }
    }
    
    public ObservableCollection<WeatherItem> WeatherNDayItemsList
    {
        get => _weatherNDaysItemsList; 
        set
        {
            SetProperty(ref _weatherNDaysItemsList, value);
        }
    }
    
    public IAsyncRelayCommand NextWeatherItemCommand { get; }
    public IAsyncRelayCommand PreviousWeatherItemCommand { get; }
    
    private async Task LoadDataAsync()
    {
        WeatherItemService service = new WeatherItemService();
        this.WeatherRemainingDayItemsList = new ObservableCollection<WeatherItem>();
        this.WeatherNDayItemsList = new ObservableCollection<WeatherItem>();

        var items = await service.GetWeatherDataOfNDays();

        foreach (var item in items)
        {
            this.WeatherNDayItemsList.Add(item);
        }

        foreach (var item in this.WeatherNDayItemsList)
        {
            string symbolCode = "pack://siteoforigin:,,,/Resources/weathericon/" + item.SymbolCode + ".png";
            item.SymbolCode = symbolCode;
        }
        
        items = await service.GetWeatherDataOfRemainingDay();
       
        foreach (var item in items)
        {
            if (item.DateTime.Hour == DateTime.Now.Hour)
            {
                this.WeatherRemainingDayItemsList.Add(item);
            }
            else 
            {
                if (item.DateTime.Hour == 6 || item.DateTime.Hour == 12 || 
                    item.DateTime.Hour == 18 || item.DateTime.Hour == 23)
                {
                    this.WeatherRemainingDayItemsList.Add(item);
                }
            }
        }

        this.SelectedWeatherItem = this.WeatherRemainingDayItemsList.First();
        GetIconSource(this.SelectedWeatherItem);
        GetDayPeriod(this.SelectedWeatherItem);
    }

    private void GetIconSource(WeatherItem item)
    {
        this.IconSource = "pack://siteoforigin:,,,/Resources/weathericon/" + item.SymbolCode + ".png";
    }
    
    private void GetDayPeriod(WeatherItem item)
    {
        if (item.DateTime.Hour == DateTime.Now.Hour)
        {
            this.DayPeriod = "Jetzt";
        }
        if (item.DateTime.Hour == 6)
        {
            this.DayPeriod = "Vormittag";
        }
        if (item.DateTime.Hour == 12)
        {
            this.DayPeriod = "Nachmittag";
        }
        if (item.DateTime.Hour == 18)
        {
            this.DayPeriod = "Abend";
        }
        if (item.DateTime.Hour == 23)
        {
            this.DayPeriod = "Nacht";
        }
    }
}