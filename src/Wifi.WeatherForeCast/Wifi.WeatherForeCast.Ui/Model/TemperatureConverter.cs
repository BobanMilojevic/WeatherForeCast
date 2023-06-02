using System;
using System.Globalization;
using System.Windows.Data;

namespace Wifi.WeatherForeCast.Ui.Model;

public class TemperatureConverter : IMultiValueConverter
{
    public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
    {
        string testString = System.Convert.ToString(value[0]);
        if (testString != "{DisconnectedItem}")
        {
            bool isDegree = System.Convert.ToBoolean(value[1]);
            double temperature = System.Convert.ToDouble(value[0]);

            if (!isDegree)
            {
                return temperature * (9.0D / 5.0D) + 32;
            }
            else
            {
                return temperature;
            }
        }
        else
        {
            return null;
        }
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}