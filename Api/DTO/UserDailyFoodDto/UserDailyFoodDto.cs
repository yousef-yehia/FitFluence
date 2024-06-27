using Api.DTO.FoodDto;
using Core.Models;

namespace Api.DTO.UserDailyFoodDto
{
    public class UserDailyFoodDto
    {
        public List<UserDailyFood> Foods { get; set; }
        public double CaloriesSum { get; set; }
        public double ProtiensSum { get; set; }
        public double CarbohydratesSum { get; set; }
        public double FibersSum { get; set; }
        public double FatsSum { get; set; }
    }
}
