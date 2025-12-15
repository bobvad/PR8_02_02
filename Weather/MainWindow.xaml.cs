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
            Init();
        }
        public async void Init()
        {
            response = await GetWeather.Get(58.009671f, 56.226184f);
            foreach(Forecast forecast in response.forecast)
                Days.Items.Add(forecast.date.ToString("dd.MM.yyyy"));
            Create(0);    
        }
        public void Create(int id)
        {
            parent.Children.Clear();

            foreach(Hour hour in response.forecast[id].hours)
            {
                parent.Children.Add(new Elements.Item(hour));
            }
        }

        private void SelectDay(object sender, RoutedEventArgs e)
        {
            Create(Days.SelectedIndex);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UpdateWeather(object sender, RoutedEventArgs e)
        {
            Init();
        }
    }
}