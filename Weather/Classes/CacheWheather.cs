using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Weather.Classes
{
    public class CacheWheather
    {
        public int Id { get; set; }

        public string City { get; set; }

        public float Lat { get; set; }
        public float Lon { get; set; }

        public string JsonData { get; set; }

        public DateTime SavedAt { get; set; }

        public bool IsExpired
        {
            get
            {
                var timeSinceSaved = DateTime.Now - SavedAt;
                return timeSinceSaved.TotalHours >= 1;
            }
        }
    }
}