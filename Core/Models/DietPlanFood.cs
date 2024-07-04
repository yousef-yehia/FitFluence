namespace Core.Models
{
    public class DietPlanFood
    {
        public int Id { get; set; }
        public int DietPlanId { get; set; }
        public DietPlan DietPlan { get; set; }
        public int FoodId { get; set; }
        public Food Food { get; set; }
        public double Weight  { get; set; }
    }
}