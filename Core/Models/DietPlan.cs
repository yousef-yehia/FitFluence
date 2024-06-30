using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class DietPlan
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public List<DietPlanFood> DietPlanFoods { get; set; } = new List<DietPlanFood>();
    }
}
