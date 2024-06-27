using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;

namespace Core.UtilityModels
{
    public class KaggleRating
    {
        public int UserId { get; set; }
        public int FoodId { get; set; }
        public int Rate { get; set; }
    }
}
