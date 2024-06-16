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
        //public void Update(string? name, string? serving, double calories = 0, double fat = 0, double carbohydrates = 0, double protein = 0, double fiber = 0 )
        //{
        //    Name = !string.IsNullOrEmpty(name) ? name : Name;
        //    Serving = !string.IsNullOrEmpty(serving) ? serving : Serving;
        //    Calories = calories != 0 ? calories : Calories;
        //    Fat = fat != 0 ? fat : Fat;
        //    Carbohydrates = carbohydrates != 0 ? carbohydrates : Carbohydrates;
        //    Protein = protein != 0 ? protein : Protein;
        //    Fiber = fiber != 0 ? fiber : Fiber;
        //}
    }
}
