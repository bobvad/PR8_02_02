using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weather.Classes
{
    public class UseAPI
    {
        public int Id { get; set; }
        public DateTime Day { get; set; }
        public int Count { get; set; } = 0;
    }
}
