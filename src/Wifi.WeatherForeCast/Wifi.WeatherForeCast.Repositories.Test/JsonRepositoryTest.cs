using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wifi.WeatherForeCast.Model;

namespace Wifi.WeatherForeCast.Repositories.Test
{
    [TestFixture]
    public class JsonRepositoryTest
    {
        UiSettings Settings = new UiSettings
        {
            IsDegree = true,
            Location = "Feldkirch Austria",
            ForecastDays = 5,
            Language = "de"
        };

        JsonRepository jRepo = new JsonRepository();



        [SetUp]
        protected void SetUp()
        {

        }

        [Test]
        public async Task SaveTest()
        {
            jRepo.SaveSettings(Settings);
            string fromFile = File.ReadAllText("c:\\temp\\WeatherForecastSettings.json");
            string original = "{\"IsDegree\":true,\"Location\":\"Feldkirch Austria\",\"ForecastDays\":5,\"Language\":\"de\"}";

            Assert.AreEqual(original, fromFile);
        }

        [Test]
        public async Task ReadTest()
        {
            UiSettings settingsFromFile = jRepo.LoadSettings();
            

            Assert.AreEqual(settingsFromFile.Language, Settings.Language);
            Assert.AreEqual(settingsFromFile.IsDegree, Settings.IsDegree);
            Assert.AreEqual(settingsFromFile.Location, Settings.Location);
            Assert.AreEqual(settingsFromFile.ForecastDays, Settings.ForecastDays);
        }
    }
}



