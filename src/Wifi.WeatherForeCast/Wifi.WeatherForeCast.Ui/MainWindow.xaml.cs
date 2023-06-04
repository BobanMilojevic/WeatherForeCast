using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Wifi.WeatherForeCast.Ui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            string language = "de-DE";
            language = "en-US";
            this.SetLanguageDictionary(language);


            
        }

        private void SetLanguageDictionary(string manualLanguage = null)
        {
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
            this.Resources.MergedDictionaries.Add(dict);
        }
    }
}