using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wifi.WeatherForeCast.Model;

namespace Wifi.WeatherForeCast.WeatherDataApi.Test
{
    [TestFixture]
    public class ApiTest
    {
        public WeatherData weatherData = new WeatherData(47.37197, 9.8704901);
        public List<WeatherItem> weatherItemslist = new List<WeatherItem>();
        public List<WeatherItem> todaysWeatherItemsList = new();

        [SetUp]
        protected void SetUp()
        {

        }


        [Test]
        public async Task Test()
        {
            weatherData.RefreshData();

            weatherData.RefreshDataWithNewLocation(47.37198, 9.8704902);

            var todaysWeatherItemsListxyz = await weatherData.GetAllDataForRemainingDay();

            todaysWeatherItemsList = todaysWeatherItemsListxyz.ToList();

            weatherItemslist = weatherData.GetAllDataAtHourOfDayForTheNext_n_Days(12, 5).ToList();



            Assert.AreEqual(todaysWeatherItemsList[0].DateTime.Date, DateTime.Now.Date);

            Assert.AreEqual(weatherItemslist.Count, 5);

            Assert.Greater(weatherItemslist.Count, 0);


        }
    }
}
