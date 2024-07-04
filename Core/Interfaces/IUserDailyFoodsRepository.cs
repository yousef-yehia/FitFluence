using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;

namespace Core.Interfaces
{
    public interface IUserDailyFoodsRepository
    {
        public Task AddFoodSelectionAsync(string userId, UserDailyFood food);
        public List<UserDailyFood> GetFoodSelections(string userId);
        public Task<double> GetTotalCaloriesByUserIdAsync(string userId);
        public double GetTotalCalories(List<UserDailyFood> userDailyFoods);
        public double GetTotalCarbohydrates(List<UserDailyFood> userDailyFoods);
        public double GetTotalFats(List<UserDailyFood> userDailyFoods);
        public double GetTotalProtiens(List<UserDailyFood> userDailyFoods);
        public double GetTotalFibers(List<UserDailyFood> userDailyFoods);

    }
}
