using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class FoodRating
    {
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public int FoodId { get; set; }
        public Food Food { get; set; }  
        public double Rate { get; set; }
    }
}
