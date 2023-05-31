namespace Wifi.WeatherForeCast.Geodata.HelperJsonObject;

public class GeoDataApiJsonModel
{
    public int place_id { get; set; }
    public string licence { get; set; }
    public string osm_type { get; set; }
    public int osm_id { get; set; }
    public List<string> boundingbox { get; set; }
    public double lat { get; set; }
    public double lon { get; set; }
    public string display_name { get; set; }
    public int place_rank { get; set; }
    public string category { get; set; }
    public string type { get; set; }
    public double importance { get; set; }
    public string icon { get; set; }
}