﻿using Wifi.WeatherForeCast.Model;

namespace Wifi.WeatherForeCast.Geodata
{
    public interface IGeodataApi
    {
        Task<IQueryable<Coordinate>> GetCoordinates(string city, CancellationToken cancellationToken);
        Task<string> GetCity(double longitude, double latitude, CancellationToken cancellationToken);
    }
}
