using Wifi.WeatherForeCast.Geodata;
using Wifi.WeatherForeCast.Model;

namespace Wifi.WeatherForeCast.Business;

public class GeoDataService
{
    private IGeodataApi _IGeodataApi;

    public GeoDataService()
    {
        _IGeodataApi = new GeodataApi();
    }

    public async Task<IQueryable<Coordinate>> GetCoordinates(string city, CancellationToken cancellationToken)
    {
        return await _IGeodataApi.GetCoordinates(city, cancellationToken);
    }

    public async Task<string> GetCity(double longitude, double latitude, CancellationToken cancellationToken)
    {
        return await _IGeodataApi.GetCity(longitude, latitude, cancellationToken);
    }
}
