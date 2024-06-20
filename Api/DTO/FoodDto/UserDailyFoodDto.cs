namespace Api.DTO.FoodDto
{
    public class UserDailyFoodDto
    {
        public List<FoodReturnDto> Foods { get; set; }
        public double Calories { get; set; }
        public double Protien { get; set; }
    }
}
