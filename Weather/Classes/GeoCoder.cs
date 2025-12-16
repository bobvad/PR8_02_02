using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Weather.Classes
{
    public class GeoCoder
    {
        private const string BaseUrl = "https://geocode-maps.yandex.ru/v1/";
        private const string Key = "553b3d1e-e3aa-4d57-ac38-c0b305a219e8";

        public static async Task<(float lat, float lon)> GetCoords(string city)
        {
            string url = $"{BaseUrl}?apikey={Key}&geocode={Uri.EscapeDataString(city)}&lang=ru_RU&format=json&results=1";

            using (HttpClient client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(15);
                client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64)");

                HttpResponseMessage httpResponse = await client.GetAsync(url);

                if (!httpResponse.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Геокодер вернул ошибку: {httpResponse.StatusCode}");
                }

                string content = await httpResponse.Content.ReadAsStringAsync();
                JObject json = JObject.Parse(content);

                string pos = json["response"]?["GeoObjectCollection"]?["featureMember"]?[0]?["GeoObject"]?["Point"]?["pos"]?.ToString();

                if (string.IsNullOrWhiteSpace(pos))
                    throw new Exception("Город не найден.");

                var parts = pos.Split(' ');
                float lon = float.Parse(parts[0], System.Globalization.CultureInfo.InvariantCulture);
                float lat = float.Parse(parts[1], System.Globalization.CultureInfo.InvariantCulture);

                return (lat, lon);
            }
        }
    }
}