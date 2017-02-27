namespace ShowMeTheWeather.Model
{
    using System.ComponentModel;
    /// <summary>
    /// Describes "Weather conditions" with specific fields to display it to the user
    /// </summary>
    public class WeatherConditions : INotifyPropertyChanged
    {
        private string _temperature; // in celsius
        private string _humidity; // in percent
        private string _pressure; // in hPa
        private string _windDescription; // speed and direction
        private string _visibility; // in metres

        public string Temperature
        {
            get
            {
                return _temperature;
            }

            set
            {
                _temperature = value;
                OnPropertyChanged("Temperature");
            }
        }

        public string Humidity
        {
            get
            {
                return _humidity;
            }

            set
            {
                _humidity = value;
                OnPropertyChanged("Humidity");
            }
        }

        public string Pressure
        {
            get
            {
                return _pressure;
            }

            set
            {
                _pressure = value;
                OnPropertyChanged("Pressure");
            }
        }

        public string WindDescription
        {
            get
            {
                return _windDescription;
            }

            set
            {
                _windDescription = value;
                OnPropertyChanged("WindDescription");
            }
        }

        public string Visibility
        {
            get
            {
                return _visibility;
            }

            set
            {
                _visibility = value;
                OnPropertyChanged("Visibility");
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
