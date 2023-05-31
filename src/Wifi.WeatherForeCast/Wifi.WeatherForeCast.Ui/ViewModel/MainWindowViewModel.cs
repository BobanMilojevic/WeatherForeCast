﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
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
    private int[] _numberOfDays;
    private int _selectedNumberOfDays;
    private bool _isDegree;
    private string _temperatureStringFormat;
    private string _searchItem;
    private bool _searchValueDropDown;
    private string _selectedSearchSearchItem;
    private string _city;
    private WeatherItem _selectedWeatherItem;
    private Coordinate _selectedCoordinateItem;
    private ObservableCollection<string> _cityItemsList;
    private IQueryable<Coordinate> _coordinatesItemsList;
    private ObservableCollection<WeatherItem> _weatherRemainingDayItemsList;
    private ObservableCollection<WeatherItem> _weatherNDaysItemsList;

    public MainWindowViewModel()
    {
        this.IsDegree = true;

        CityItemsList = new ObservableCollection<string>();
        this.SearchValueDropDown = false;

        LoadDataAsync();

        NextWeatherItemCommand = new AsyncRelayCommand(NextWeatherItem);
        PreviousWeatherItemCommand = new AsyncRelayCommand(PreviousWeatherItem);
        SaveDataCommand = new AsyncRelayCommand(SaveData);
    }

    private async Task SaveData()
    {
        SaveService service = new SaveService();

        UiSettings uiSettings = new UiSettings()
        {
            ForecastDays = this.SelectedNumberOfDays,
            IsDegree = this.IsDegree,
            Language = "",
            Location = City
        };
        
        service.SaveUiSetting(uiSettings);
    }

    // Properties
    private async Task LoadDataAsync()
    {
        this.WeatherRemainingDayItemsList = new ObservableCollection<WeatherItem>();
        this.WeatherNDayItemsList = new ObservableCollection<WeatherItem>();

        SelectedWeatherItem = new WeatherItem()
        {
            DateTime = DateTime.Now,
            Humidity = 0,
            PerceivedTemperature = 0,
            Pressure = 0,
            SymbolCode = "",
            Temperature = 0,
            WindSpeed = 0
        };
        
        _numberOfDays = new int[] { 1, 2, 3, 4, 5, 6, 7 };
        this.SelectedNumberOfDays = 0;
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

    public int[] NumberOfDays
    {
        get => _numberOfDays;
        set
        {
            SetProperty(ref _numberOfDays, value);
        }
    }
    
    public int SelectedNumberOfDays
    {
        get => _selectedNumberOfDays;
        set
        {
            SetProperty(ref _selectedNumberOfDays, value);
            UpdateWeatherNDayItemsList();
        }
    }

    public bool IsDegree
    {
        get => _isDegree;
        set
        {
            SetProperty(ref _isDegree, value);
            UpdateTempFormatstring();
        }
    }

    public string TemperatureFormatString
    {
        get => _temperatureStringFormat;
        set
        {
            SetProperty(ref _temperatureStringFormat, value);
        }
    }
    
    public string SelectedSearchItem 
    {
        get => _selectedSearchSearchItem;
        set
        {
            SetProperty(ref _selectedSearchSearchItem, value);
            SetSelectedCoordinateItem();
            UpdateWeatherNDayItemsList();
            UpdateWeatherRemainingDayItemsList();
        }
    }
    
    public bool SearchValueDropDown 
    {
        get => _searchValueDropDown;
        set
        {
            SetProperty(ref _searchValueDropDown, value);
        }
    }
    
    public string SearchItem 
    {
        get => _searchItem;
        set
        {
            SetProperty(ref _searchItem, value);
            UpdateCityItemsList();
        }
    }

    public ObservableCollection<string> CityItemsList 
    {
        get => _cityItemsList;
        set
        {
            SetProperty(ref _cityItemsList, value);
        }
    }
    
    public string City 
    {
        get => _city;
        set
        {
            SetProperty(ref _city, value);
        }
    }
    
    //Commands
    public IAsyncRelayCommand NextWeatherItemCommand { get; }
    public IAsyncRelayCommand PreviousWeatherItemCommand { get; }
    public IAsyncRelayCommand SaveDataCommand { get; }
    
    // private Methodes
    private async Task UpdateWeatherRemainingDayItemsList()
    {
        if (_selectedCoordinateItem == null)
            return;
        
        WeatherItemService service = new WeatherItemService();
        
        this.WeatherRemainingDayItemsList.Clear();
        
        var items = await service.GetWeatherDataOfRemainingDay(_selectedCoordinateItem.Latitude, _selectedCoordinateItem.Longitude);
        
        foreach (var item in items)
        {
            if (item.DateTime.Hour == DateTime.Now.Hour)
            {
                this.WeatherRemainingDayItemsList.Add(item);
            }
        }

        foreach (var item in items)
        {
            if (item.DateTime.Hour == 6 || item.DateTime.Hour == 12 || 
                item.DateTime.Hour == 18 || item.DateTime.Hour == 23)
            {
                this.WeatherRemainingDayItemsList.Add(item);
            }
        }
        
        this.SelectedWeatherItem = this.WeatherRemainingDayItemsList.First();
        GetIconSource(this.SelectedWeatherItem);
        GetDayPeriod(this.SelectedWeatherItem);
    }
    
    private async Task UpdateWeatherNDayItemsList()
    {
        if (_selectedCoordinateItem == null)
            return;
        
        WeatherItemService service = new WeatherItemService();
        
        this.WeatherNDayItemsList.Clear();
        
        var items = await service.GetWeatherDataOfNDays(_selectedCoordinateItem.Latitude, _selectedCoordinateItem.Longitude);

        int index = 0;
        foreach (var item in items)
        {
            if (index <= this.SelectedNumberOfDays)
            {
                string symbolCode = "pack://siteoforigin:,,,/Resources/weathericon/" + item.SymbolCode + ".png";
                item.SymbolCode = symbolCode;
                this.WeatherNDayItemsList.Add(item);
                index++;
            }
        }
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
        else
        {
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
    
    private void UpdateTempFormatstring()
    {
        if (this.IsDegree)
        {
            this.TemperatureFormatString = "{0:0.0}°C";
        }
        else
        {
            this.TemperatureFormatString = "{0:0.0}°F";
        }
    }
    
    private void SetSelectedCoordinateItem()
    {
        foreach (var item in _coordinatesItemsList)
        {
            if (item.City == SelectedSearchItem)
            {
                _selectedCoordinateItem = item;
                this.City = _selectedCoordinateItem.City;
            }
        }
    }
    
    private async Task UpdateCityItemsList()
    {
        if (SearchItem.Length >= 1)
        {
            this.SearchValueDropDown = true;
            
            this.CityItemsList.Clear();

            GeoDataService service = new GeoDataService();

            _coordinatesItemsList = await service.GetCoordinates(SearchItem, new CancellationToken(false));

            foreach (var item in _coordinatesItemsList)
            {
                CityItemsList.Add(item.City);
            }
        }
    }
}