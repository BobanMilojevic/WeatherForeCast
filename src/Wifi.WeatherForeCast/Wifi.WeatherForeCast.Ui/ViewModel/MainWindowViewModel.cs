using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
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
    private bool _isFahrenheit;
    private WeatherItem _selectedWeatherItem;
    private Coordinate _selectedCoordinateItem;
    private ObservableCollection<string> _cityItemsList;
    private IQueryable<Coordinate> _coordinatesItemsList;
    private ObservableCollection<WeatherItem> _weatherRemainingDayItemsList;
    private ObservableCollection<WeatherItem> _weatherNDaysItemsList;
    private CancellationTokenSource _geoDataCancellationTokenSource;
    private ResourceDictionary _dict;

    public MainWindowViewModel()
    {
        string language = "de-DE";
        language = "en-US";
        this.SetLanguageDictionary(language);
        _geoDataCancellationTokenSource = new CancellationTokenSource();
        CityItemsList = new ObservableCollection<string>();
        this.SearchValueDropDown = false;

        LoadDataAsync();

        NextWeatherItemCommand = new AsyncRelayCommand(NextWeatherItem);
        PreviousWeatherItemCommand = new AsyncRelayCommand(PreviousWeatherItem);
        SaveDataCommand = new AsyncRelayCommand(SaveData);
        
    }

    private void SetLanguageDictionary(string manualLanguage = null)
    {
        _dict = new ResourceDictionary();
        ResourceDictionary dict = new ResourceDictionary();
        string language = null;
        if (manualLanguage != null)
        {
            language = manualLanguage;
        }
        else
        {
            language = Thread.CurrentThread.CurrentCulture.ToString();
        }
        switch (language)
        {
            case "en-US":
                dict.Source = new Uri("pack://siteoforigin:,,,/Resources/StringResources.xaml", UriKind.Absolute);
                break;
            case "de-DE":
                dict.Source = new Uri("pack://siteoforigin:,,,/Resources/StringResources.de.xaml", UriKind.Absolute);
                break;
            default:
                dict.Source = new Uri("pack://siteoforigin:,,,/Resources/StringResources.xaml", UriKind.Absolute);
                break;
        }
        _dict.MergedDictionaries.Add(dict);

    }


    private async Task SaveData()
    {
        if (this.SelectedCoordinateItem == null)
        {
            return;
        }

        SaveLoadService loadService = new SaveLoadService();

        UiSettings uiSettings = new UiSettings()
        {
            ForecastDays = this.SelectedNumberOfDays,
            IsDegree = this.IsDegree,
            Language = "",
            Location = this.SelectedCoordinateItem.City,
            Latitude = this.SelectedCoordinateItem.Latitude,
            Longitude = this.SelectedCoordinateItem.Longitude
        };

        loadService.SaveUiSetting(uiSettings);
    }

    private async Task LoadData()
    {
        SaveLoadService loadService = new SaveLoadService();
        UiSettings loadUiSettings = loadService.LoadUiSettings();

        if (loadUiSettings != null)
        {
            this.SelectedCoordinateItem = new Coordinate()
            {
                City = loadUiSettings.Location,
                Latitude = loadUiSettings.Latitude,
                Longitude = loadUiSettings.Longitude
            };

            UpdateWeatherNDayItemsList();
            UpdateWeatherRemainingDayItemsList();

            this.IsDegree = loadUiSettings.IsDegree;
            this.IsFahrenheit = !this.IsDegree;
            this.SelectedNumberOfDays = loadUiSettings.ForecastDays;
        }
    }

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
        this.IsDegree = true;
        this.SelectedNumberOfDays = 0;

        LoadData();
    }

    private async Task PreviousWeatherItem()
    {
        if (_count > 0)
        {
            _count--;
            this.SelectedWeatherItem = this.WeatherRemainingDayItemsList[_count];
            GetDayPeriod(this.SelectedWeatherItem);
            GetIconSource(this.SelectedWeatherItem);
        }
    }

    private async Task NextWeatherItem()
    {
        if (_count + 1 < WeatherRemainingDayItemsList.Count)
        {
            _count++;
            this.SelectedWeatherItem = this.WeatherRemainingDayItemsList[_count];
            GetDayPeriod(this.SelectedWeatherItem);
            GetIconSource(this.SelectedWeatherItem);
        }
    }

    // Properties
    public bool IsFahrenheit
    {
        get => _isFahrenheit;
        set
        {
            SetProperty(ref _isFahrenheit, value);
        }
    }

    public Coordinate SelectedCoordinateItem
    {
        get => _selectedCoordinateItem;
        set
        {
            SetProperty(ref _selectedCoordinateItem, value);
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
            _geoDataCancellationTokenSource.Cancel();
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
        if (this.SelectedCoordinateItem == null)
            return;

        WeatherItemService service = new WeatherItemService();

        this.WeatherRemainingDayItemsList.Clear();

        var items = await service.GetWeatherDataOfRemainingDay(this.SelectedCoordinateItem.Latitude, this.SelectedCoordinateItem.Longitude);

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
        if (this.SelectedCoordinateItem == null)
            return;

        WeatherItemService service = new WeatherItemService();

        this.WeatherNDayItemsList.Clear();

        var items = await service.GetWeatherDataOfNDays(this.SelectedCoordinateItem.Latitude, this.SelectedCoordinateItem.Longitude);

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
        if (item.DateTime.Hour == DateTime.Now.Hour && this.DayPeriod != _dict["Jetzt"].ToString())
        //if (item.DateTime.Hour == DateTime.Now.Hour && this.DayPeriod != "Jetzt")
        {
            this.DayPeriod = _dict["Jetzt"].ToString();
            //this.DayPeriod = "Jetzt";
        }
        else
        {
            if (item.DateTime.Hour == 6)
            {
                this.DayPeriod = _dict["Vormittag"].ToString();
            }
            if (item.DateTime.Hour == 12)
            {
                this.DayPeriod = _dict["Nachmittag"].ToString();
            }
            if (item.DateTime.Hour == 18)
            {
                this.DayPeriod = _dict["Abend"].ToString();
            }
            if (item.DateTime.Hour == 23)
            {
                this.DayPeriod = _dict["Nacht"].ToString();
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
                this.SelectedCoordinateItem = item;
            }
        }
    }

    private async Task UpdateCityItemsList()
    {
        _geoDataCancellationTokenSource = new CancellationTokenSource();

        if (SearchItem.Length >= 1)
        {
            this.SearchValueDropDown = true;

            this.CityItemsList.Clear();

            GeoDataService service = new GeoDataService();

            _coordinatesItemsList = await service.GetCoordinates(SearchItem, _geoDataCancellationTokenSource.Token);
            if (_coordinatesItemsList != null)
            {
                foreach (var item in _coordinatesItemsList)
                {
                    CityItemsList.Add(item.City);
                }
            }
        }
    }
}