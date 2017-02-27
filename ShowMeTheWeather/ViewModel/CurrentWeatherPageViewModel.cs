namespace ShowMeTheWeather.ViewModel
{
    using System.Xml;
    /// <summary>
    /// Displayes detailed info about current weather conditions - see CurrentWeatherPage, DetailedForecast
    /// </summary>
    public class CurrentWeatherPageViewModel
    {
        // title of the page(country + city or date and time)
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
        // displayed to user weather conditions
        private Model.WeatherConditions _currentConditions;
        public Model.WeatherConditions CurrentConditions
        {
            get
            {
                return _currentConditions;
            }
            set
            {
                _currentConditions = value;
            }
        }
        public CurrentWeatherPageViewModel(XmlDocument xmlWeatherConditions)
        {
            _currentConditions = Utils.XmlWeatherReader.BuildCurrentWeatherConditionsFromXmlDocument(xmlWeatherConditions, out _titleString);
        }

        public CurrentWeatherPageViewModel(Model.WeatherConditions weatherConditions, string titleString)
        {
            _currentConditions = weatherConditions;
            _titleString = titleString;
        } 
    }
}
