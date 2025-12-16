using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Threading.Tasks;
using Weather.Classes;
using Weather.Models;

namespace Weather
{
    public partial class MainWindow : Window
    {
        DataResponse response;
        private string currentCity = "Пермь";
        private float currentLat = 58.009671f;
        private float currentLon = 56.226184f;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += async (s, e) => await LoadWeatherByCoords(currentLat, currentLon);

            CityBox.GotFocus += CityBox_GotFocus;
            CityBox.LostFocus += CityBox_LostFocus;
        }

        private async Task LoadWeatherByCoords(float lat, float lon)
        {
            try
            {
                response = await GetWeather.Get(lat, lon);
                UpdateUI();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task LoadWeatherByCity(string city)
        {
            try
            {
                var cords = await GeoCoder.GetCoords(city);

                currentCity = city;
                currentLat = cords.lat;
                currentLon = cords.lon;

                response = await GetWeather.Get(cords.lat, cords.lon);

                Title = $"Прогноз погоды - {currentCity}";

                UpdateUI();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                CityBox.Text = "Введите город...";
                CityBox.Foreground = System.Windows.Media.Brushes.Gray;
            }
        }

        private void UpdateUI()
        {
            if (response == null) return;

            Days.Items.Clear();
            foreach (Forecast forecast in response.forecasts)
            {
                Days.Items.Add(forecast.date.ToString("dd.MM.yyyy"));
            }

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
            await LoadWeatherByCoords(currentLat, currentLon);
        }

        private void CityBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (CityBox.Text == "Введите город...")
            {
                CityBox.Text = "";
                CityBox.Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        private void CityBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CityBox.Text))
            {
                CityBox.Text = "Введите город...";
                CityBox.Foreground = System.Windows.Media.Brushes.Gray;
            }
        }

        private async void CityBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string city = CityBox.Text.Trim();
                if (!string.IsNullOrEmpty(city) && city != "Введите город...")
                {
                    await LoadWeatherByCity(city);
                }
            }
        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string city = CityBox.Text.Trim();
            if (!string.IsNullOrEmpty(city) && city != "Введите город...")
            {
                await LoadWeatherByCity(city);
            }
        }
    }
}