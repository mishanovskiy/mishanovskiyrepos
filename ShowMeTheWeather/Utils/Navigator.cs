namespace ShowMeTheWeather.Utils
{
    using System.Windows.Navigation;
    
    /// <summary>
    /// Helps not to run out from MVVM pattern with allowing to handle navigation in ViewModel class 
    /// </summary>
    public static class Navigator
    {
        public static NavigationService NavigationService { get; set; } // navigation for weather data displaying switch
    }
}
