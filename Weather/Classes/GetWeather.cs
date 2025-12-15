using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Weather.Models;

namespace Weather.Classes
{
    public class GetWeather
    {
        public static string UPL = "https://api.weather.yandex.ru/v2/forecast";
        public static string Key = "demo_yandex_weather_api_key_ca6d09349ba0";

        public static async Task<DataResponse>  Get(float lat, float lan)
        {
            DataResponse DataResponse = null; 
            string url = $"{UPL}?lat={lat}&lon={lan}".Replace(",", ".");

            using (HttpClient client = new HttpClient())
            {
                using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url))
                {
                    request.Headers.Add("X-Yandex-Weather-Key", Key);

                    using (var response = await client.SendAsync(request))
                    {
                        string dataResponse = await response.Content.ReadAsStringAsync();
                    }
                }
            }
            return DataResponse;
        }
    }
}
