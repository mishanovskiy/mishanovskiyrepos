namespace ShowMeTheWeather.ViewModel
{
    using System.Collections.Generic;
    using System.Xml;

    /// <summary>
    /// Backend for Forecasts page, that allow to see list of detailed dates to invoke new window with detailed info about weather 
    /// </summary>
    public class ForecastsPageViewModel
    {
        // Country + city information
        private string _titleString;
        public string TitleString
        {
            get
            {
                return _titleString;
            }

            set
            {
                _titleString = value;
            }
        }
        // Time range(3 hours) in some days of 5
        // Selection changing from listbox value invokes new Window with detailed info 
        private string _selectedTimeRange;
        public string SelectedTimeRange
        {
            get
            {
                return _selectedTimeRange;
            }
            set
            {
                _selectedTimeRange = value;
                Model.WeatherConditions selectedWeather = null;
                if (_forecastsDictionary.TryGetValue(_selectedTimeRange, out selectedWeather))
                {
                    View.DetailedForecast detailedForecast = new View.DetailedForecast();
                    View.CurrentWeatherPage currentWeatherPage = new View.CurrentWeatherPage();
                    currentWeatherPage.DataContext = new CurrentWeatherPageViewModel(selectedWeather, _selectedTimeRange);
                    detailedForecast.detailedForecastFrame.NavigationService.Navigate(currentWeatherPage);
                    detailedForecast.ShowDialog();
                }
            }
        }
        // dictionary with date and time of weather forecast as a key, weather condition as a value
        private Dictionary<string, Model.WeatherConditions> _forecastsDictionary;
        public Dictionary<string, Model.WeatherConditions> ForecastsDictionary
        {
            get
            {
                return _forecastsDictionary;
            }
            set
            {
                _forecastsDictionary = value;
            }
        }

        public ForecastsPageViewModel(XmlDocument xmlForecasts)
        {
            _forecastsDictionary = Utils.XmlWeatherReader.BuildForecastsFromXmlDocument(xmlForecasts, out _titleString);
        }
    }
}
