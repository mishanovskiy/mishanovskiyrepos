namespace ShowMeTheWeather.ViewModel
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Configuration;
    using System.Net;
    using System.Xml;
    /// <summary>
    /// Regulates relationship among controls on MainWindow
    /// </summary>
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        // city from search string name
        private string _cityName;
        public string CityName
        {
            get
            {
                return _cityName;
            }
            set
            {
                _cityName = value;
                OnPropertyChanged("CityName");
            }
        }

        // cities collection(cities names collection)
        private ObservableCollection<string> _cities;

        // page that displays current weather or forecast(user choice)
        private System.Windows.Controls.Page _displayedPage;

        public System.Windows.Controls.Page DisplayedPage
        {
            get
            {
                return _displayedPage;
            }
            set
            {
                if (_displayedPage == value)
                {
                    return;
                }
                _displayedPage = value;
                OnPropertyChanged("DisplayedPage");
            }
        }
        // command that happends after some button clicking to display user actual information about current weather in selected city/country
        private Utils.DelegateCommand _activateCurrentWeatherCommand;
        public Utils.DelegateCommand ActivateCurrentWeatherCommand
        {
            get
            {
                return _activateCurrentWeatherCommand ?? (_activateCurrentWeatherCommand
                    = new Utils.DelegateCommand((object arg) =>
                    {
                        if (_selectedCity == null) return;
                        _displayedPage = new View.CurrentWeatherPage();
                        _displayedPage.DataContext = new CurrentWeatherPageViewModel(GetXmlDocument(CurrentWeatherUrl
                            .Replace("@LOC@", _selectedCity)));
                        Utils.Navigator.NavigationService.Navigate(_displayedPage);
                    }));
            }
        }
        // command that happends after some button clicking to display user forecast in selected city/country
        private Utils.DelegateCommand _activateForecastsCommand;
        public Utils.DelegateCommand ActivateForecastsCommand
        {
            get
            {
                return _activateForecastsCommand ?? (_activateForecastsCommand
                    = new Utils.DelegateCommand((object arg) =>
                    {
                        if (_selectedCity == null) return;
                        _displayedPage = new View.ForecastsPage();
                        _displayedPage.DataContext = new ForecastsPageViewModel(GetXmlDocument(ForecastsWeatherUrl
                            .Replace("@LOC@", _selectedCity)));
                        Utils.Navigator.NavigationService.Navigate(_displayedPage);
                    }));
            }
        }

        public MainWindowViewModel()
        {
            _cities = new ObservableCollection<string>();
        }
        // string collection with cities names
        public ObservableCollection<string> Cities
        {
            get
            {
                return _cities;
            }
            set
            {
                _cities = value;
                OnPropertyChanged("Cities");
            }
        }
        // executes after selection chaging in list box control
        private Utils.DelegateCommand _listBoxSelectionChangedCommand;
        public Utils.DelegateCommand ListBoxSelectionChangedCommand => _listBoxSelectionChangedCommand
            ?? (_listBoxSelectionChangedCommand = new Utils.DelegateCommand(ListBoxSelectionChanged));

        public void ListBoxSelectionChanged(object arg)
        {
            if (_selectedCity != null)
            {
                _displayedPage = new View.CurrentWeatherPage();
                _displayedPage.DataContext = new CurrentWeatherPageViewModel(GetXmlDocument(CurrentWeatherUrl
                    .Replace("@LOC@", _selectedCity)));
                Utils.Navigator.NavigationService.Navigate(_displayedPage);
            }
        }
        // selected in list box city to show the necessary information about it
        private string _selectedCity;

        public string SelectedCity
        {
            get
            {
                return _selectedCity;
            }
            set
            {
                _selectedCity = value;
                OnPropertyChanged("SelectedCity");
                RemoveCityCommand.RaiseCanExecuteChanged();

            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        // executes after clicking "+" button to add new existing city to list box
        #region Add new city
        private Utils.DelegateCommand _addCityCommand;
        public Utils.DelegateCommand AddCityCommand => _addCityCommand ?? (_addCityCommand = new Utils.DelegateCommand(AddNewCity));

        private void AddNewCity(object arg)
        {
            if (string.IsNullOrEmpty(_cityName) || _cities.FirstOrDefault((string city)
                =>
            { return string.Equals(city, _cityName, System.StringComparison.OrdinalIgnoreCase); }) != null)
            {
                return;
            }
            try
            {
                _client.DownloadString(CurrentWeatherUrl.Replace("@LOC@", _cityName));
            }
            catch
            {
                return;
            }
            _cities.Add(_cityName);
        }
        #endregion
        // executes after clicking "-" button when some city is selected in list box, if not selected or list box is empty - button disabled
        #region Remove a city
        private Utils.DelegateCommand _removeCityCommand;

        public Utils.DelegateCommand RemoveCityCommand => _removeCityCommand ?? (_removeCityCommand
            = new Utils.DelegateCommand((object arguments)
                =>
            {
                _cities.Remove(_selectedCity);
                if (_cities.Count == 0)
                {
                    Utils.Navigator.NavigationService.Content = null;
                }
                else
                {
                    _selectedCity = _cities[0];
                    ListBoxSelectionChanged(new object());
                }
            }
                , CanRemoveCity));

        private bool CanRemoveCity(object argument)
        {
            if (SelectedCity == null)
            {
                return false;
            }
            var checkedCity = _cities?.FirstOrDefault(city => city == _selectedCity);
            if (checkedCity == null)
            {
                return false;
            }
            return true;
        }
        #endregion
        #region Web items
        // web client instance to get weather information from internet
        private WebClient _client = new WebClient();
        // web site API KEY that allows to get info from site
        // See App.config
        private static string API_KEY = ConfigurationManager.AppSettings.Get("API_KEY");
        // web site url to get infromation about current weather
        private string CurrentWeatherUrl = string.Format("{0}{1}{2}", "http://api.openweathermap.org/data/2.5/weather?"
            , "q=@LOC@&mode=xml&units=metric&APPID=", API_KEY);
        // web site url to get infromation about forecast
        private string ForecastsWeatherUrl = string.Format("{0}{1}{2}", "http://api.openweathermap.org/data/2.5/forecast?"
            , "q=@LOC@&mode=xml&units=metric&APPID=", API_KEY);
        // gets xml document from server to parse it and get display info
        private XmlDocument GetXmlDocument(string url)
        {
            using (_client)
            {
                string xml = _client.DownloadString(url);
                XmlDocument xml_document = new XmlDocument();
                xml_document.LoadXml(xml);
                return xml_document;
            }
        }
        #endregion
    }
}
