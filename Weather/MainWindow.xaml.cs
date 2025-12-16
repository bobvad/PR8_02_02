using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Weather.Classes;
using Weather.Models;

namespace Weather
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DataResponse response;
        public MainWindow()
        {
            InitializeComponent();
            Loaded += async (s, e) => await Init();
        }

        public async Task Init()
        {
            response = await GetWeather.Get(58.009671f, 56.226184f);
            Days.Items.Clear();
            foreach (Forecast forecast in response.forecasts)
                Days.Items.Add(forecast.date.ToString("dd.MM.yyyy"));

            if (Days.Items.Count > 0)
            {
                Days.SelectedIndex = 0;
                Create(0);
            }
        }

        public void Create(int id)
        {
            if (response == null || id < 0 || id >= response.forecasts.Count)
                return;

            parent.Children.Clear();

            foreach (Hour hour in response.forecasts[id].hours)
            {
                parent.Children.Add(new Elements.Item(hour));
            }
        }

        private void SelectDay(object sender, SelectionChangedEventArgs e)
        {
            if (Days.SelectedIndex >= 0)
                Create(Days.SelectedIndex);
        }

        private async void UpdateWeather(object sender, RoutedEventArgs e)
        {
            await Init();
        }
    }
}