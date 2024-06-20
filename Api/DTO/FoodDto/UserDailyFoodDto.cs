namespace Api.DTO.FoodDto
{
    public class UserDailyFoodDto
    {
        public List<FoodReturnDto> Foods { get; set; }
        public double CaloriesSum { get; set; }
        public double ProtienSum { get; set; }
    }
}
