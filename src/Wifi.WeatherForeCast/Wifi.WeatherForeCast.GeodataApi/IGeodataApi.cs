using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wifi.WeatherForeCast.Model;

namespace Wifi.WeatherForeCast.Geodata.Interfaces
{
    public interface IGeodataApi
    {
        Coordinates MainApiForBoban(string city);

        Coordinates GetCoordinates(string city);

        string GetRequest(string url);

        Coordinates GetCoordinatesFromPlace(Place place);

    }
}
