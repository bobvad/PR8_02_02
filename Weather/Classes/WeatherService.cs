using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using Weather.Models;

namespace Weather.Classes
{
    public static class WeatherService
    {
        private const int DailyLimit = 50;

        public static async Task<DataResponse> GetWeatherCached(string city)
        {
            using (var db = new WheatherContext())
            {
                var cache = db.Cache.FirstOrDefault(x => x.City.ToLower() == city.ToLower());

                if (cache != null && !cache.IsExpired) 
                {
                    Console.WriteLine($"Используем кэш для города: {city}");
                    return JsonConvert.DeserializeObject<DataResponse>(cache.JsonData);
                }

                var today = DateTime.Today;
                var usage = db.ApiUsage.FirstOrDefault(x => x.Day == today);

                if (usage == null)
                {
                    usage = new UseAPI { Day = today, Count = 0 };
                    db.ApiUsage.Add(usage);
                }

                if (usage.Count >= DailyLimit)
                {
                    throw new Exception($"Лимит запросов на сегодня исчерпан ({DailyLimit}/день)");
                }

                Console.WriteLine($"Делаем API запрос для города: {city}");

                var (lat, lon) = await GeoCoder.GetCoords(city);
                var data = await GetWeather.Get(lat, lon);

                usage.Count++;
                db.SaveChanges();

                if (cache == null)
                {
                    cache = new CacheWheather
                    {
                        City = city,
                        Lat = lat,
                        Lon = lon,
                        JsonData = JsonConvert.SerializeObject(data),
                        SavedAt = DateTime.Now
                    };
                    db.Cache.Add(cache);
                }
                else
                {
                    cache.JsonData = JsonConvert.SerializeObject(data);
                    cache.SavedAt = DateTime.Now;
                    cache.Lat = lat;
                    cache.Lon = lon;
                }

                db.SaveChanges();
                Console.WriteLine($"Сохранено в кэш: {city}. Запросов сегодня: {usage.Count}/{DailyLimit}");

                return data;
            }
        }

        public static void CleanupOldCache()
        {
            using (var db = new WheatherContext())
            {
                var weekAgo = DateTime.Now.AddDays(-7);
                var oldCache = db.Cache.Where(x => x.SavedAt < weekAgo).ToList();

                if (oldCache.Any())
                {
                    db.Cache.RemoveRange(oldCache);
                    db.SaveChanges();
                    Console.WriteLine($"Удалено {oldCache.Count} устаревших записей кэша");
                }

                var monthAgo = DateTime.Now.AddDays(-30);
                var oldUsage = db.ApiUsage.Where(x => x.Day < monthAgo).ToList();

                if (oldUsage.Any())
                {
                    db.ApiUsage.RemoveRange(oldUsage);
                    db.SaveChanges();
                    Console.WriteLine($"Удалено {oldUsage.Count} старых записей использования API");
                }
            }
        }
    }
}