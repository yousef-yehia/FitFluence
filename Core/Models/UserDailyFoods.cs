using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class UserDailyFoods
    {
        public List<Food> Foods { get; set; }
        public double Calories { get; set; }
        public double Protien { get; set; }
    }
}
