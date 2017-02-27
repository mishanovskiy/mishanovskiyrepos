using System.Windows;

namespace ShowMeTheWeather.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Utils.Navigator.NavigationService = weatherFrame.NavigationService;
        }
    }
}
