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
        Task AddFoodSelectionAsync(string userId, Food food);
        UserDailyFoods GetFoodSelections(string userId);
        Task<double> GetTotalCaloriesAsync(string userId);
    }
}
