using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wifi.WeatherForeCast.YrApi
{
    [TestFixture]
    public class ApiTest
    {
        [SetUp]
        protected void SetUp()
        {
        }


        [Test]
        public async Task Test()
        {
            WeatherItem weatherItem = new WeatherItem();
            weatherItem = await HelperClass.GetInstantWeatherItem(47.37197, 9.8704901);

            Assert.AreEqual(weatherItem.DateTime.Date, DateTime.Now.Date);

            List<WeatherItem> weatherItems = new();
            weatherItems = await HelperClass.GetAllDataAtHourOfDayForTheNext_n_Days(47.37197, 9.8704901, 12, 5);
            
            Assert.AreEqual(weatherItems.Count, 5);


            weatherItems = await HelperClass.GetAllDataForRemainingDay(47.37197, 9.8704901);

            Assert.Greater(weatherItems.Count, 0);


        }
    }
}
