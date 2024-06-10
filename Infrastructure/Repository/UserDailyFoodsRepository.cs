using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Core.Interfaces;
using Core.Models;

namespace Infrastructure.Repository
{
    public class UserDailyFoodsRepository : IUserDailyFoodsRepository
    {
        private readonly IMemoryCache _cache;
        private const string UserFoodKeyPrefix = "UserFood_";

        public UserDailyFoodsRepository(IMemoryCache cache)
        {
            _cache = cache;
        }
        private TimeSpan GetTimeUntilMidnight()
        {
            var now = DateTime.Now;
            var midnight = DateTime.Today.AddDays(1);
            return midnight - now;
        }
        public async Task AddFoodSelectionAsync(string userId, Food food)
        {
            var cacheKey = $"{UserFoodKeyPrefix}{userId}";

            if (!_cache.TryGetValue(cacheKey, out List<Food> userFoods))
            {
                userFoods = new List<Food>();
            }

            userFoods.Add(food);
            var timeSpan = GetTimeUntilMidnight();
            await Task.Run(() => _cache.Set(cacheKey, userFoods, timeSpan));
        }

        public UserDailyFoods GetFoodSelections(string userId)
        {
            var cacheKey = $"{UserFoodKeyPrefix}{userId}";
            if (!_cache.TryGetValue(cacheKey, out List<Food> userFoods))
            {
                userFoods = new List<Food>();
            }
            var result = new UserDailyFoods 
            {
                Foods = userFoods,
                Calories = GetTotalCaloriesAsync(userFoods),
                Protien =  GetTotalProtienAsync(userFoods)
            };
            return result;
        }

        public async Task<double> GetTotalCaloriesAsync(string userId)
        {
            var cacheKey = $"{UserFoodKeyPrefix}{userId}";
            if (_cache.TryGetValue(cacheKey, out List<Food> userFoods))
            {
                return await Task.Run(() => userFoods.Sum(food => food.Calories));
            }
            return await Task.FromResult(0d);
        } 
        public double GetTotalCaloriesAsync(List<Food> userFoods)
        {
            return userFoods.Sum(food => food.Calories);
        }
        public double GetTotalProtienAsync(List<Food> userFoods)
        {
            return userFoods.Sum(food => food.Protein);
        }
    }
}
