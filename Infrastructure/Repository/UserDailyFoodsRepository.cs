using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.Models;
using Microsoft.Extensions.Caching.Memory;

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

        public void AddFoodSelection(string userId, Food food)
        {
            var cacheKey = $"{UserFoodKeyPrefix}{userId}";
            if (!_cache.TryGetValue(cacheKey, out List<Food> userFoods))
            {
                userFoods = new List<Food>();
            }
            userFoods.Add(food);
            _cache.Set(cacheKey, userFoods);
        }

        public List<Food> GetFoodSelections(string userId)
        {
            //var cacheKey = $"{UserFoodKeyPrefix}{userId}";
            //if (_cache.TryGetValue(cacheKey, out List<Food> userFoods))
            //{
            //    return userFoods;
            //}
            //return new List<Food>();
            var cacheKey = $"{UserFoodKeyPrefix}{userId}";
            var cachedUserFoods = _cache.Get(cacheKey) as List<Food>;
            return cachedUserFoods ?? new List<Food>();
        }

        public double GetTotalCalories(string userId)
        {
            var cacheKey = $"{UserFoodKeyPrefix}{userId}";
            if (_cache.TryGetValue(cacheKey, out List<Food> userFoods))
            {
                return userFoods.Sum(food => food.Calories);
            }
            return 0;
        }
    }
}
