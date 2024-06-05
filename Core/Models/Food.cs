using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class Food
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Verified { get; set; }
        public string Serving { get; set; }
        public double Calories { get; set; }
        public double Fat { get; set; }
        public double Carbohydrates { get; set; }
        public double Protein { get; set; }
        public double Fiber { get; set; }
        public List<UserFoods> UserFoods { get; set; }

    }
}
