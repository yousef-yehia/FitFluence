namespace Api.DTO.DietPlanDto
{
    public class DietPlanFoodReturnDto
    {
        public int DietPlanFoodId { get; set; }
        public int FoodId { get; set; }
        public string FoodName { get; set; }
        public string FoodDescription { get; set; }
        public string FoodImageUrl { get; set; }
        public double FoodWeight { get; set; }
    }
}
