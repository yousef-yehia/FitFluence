using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Core.Interfaces;
using Core.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class UserDailyFoodsRepository : IUserDailyFoodsRepository
    {
        private readonly IMemoryCache _cache;
        private const string UserFoodKeyPrefix = "UserFood_";
        private readonly AppDbContext _context;

        public UserDailyFoodsRepository(IMemoryCache cache, AppDbContext context)
        {
            _cache = cache;
            _context = context;
        }
        private TimeSpan GetTimeUntilMidnight()
        {
            var now = DateTime.Now;
            var midnight = DateTime.Today.AddDays(1);
            return midnight - now;
        }
        public async Task AddFoodSelectionAsync(string userId, UserDailyFood food)
        {
            var cacheKey = $"{UserFoodKeyPrefix}{userId}";

            if (!_cache.TryGetValue(cacheKey, out List<UserDailyFood> userFoods))
            {
                userFoods = new List<UserDailyFood>();
            }

            userFoods.Add(food);
            var timeSpan = GetTimeUntilMidnight();
            await Task.Run(() => _cache.Set(cacheKey, userFoods, timeSpan));
        }

        public List<UserDailyFood> GetFoodSelections(string userId)
        {
            var cacheKey = $"{UserFoodKeyPrefix}{userId}";
            if (!_cache.TryGetValue(cacheKey, out List<UserDailyFood> userFoods))
            {
                userFoods = new List<UserDailyFood>();
            }

            return userFoods;
        }

        //public async Task<double> GetTotalCaloriesAsync(string userId)
        //{
        //    var cacheKey = $"{UserFoodKeyPrefix}{userId}";
        //    if (_cache.TryGetValue(cacheKey, out List<Food> userFoods))
        //    {
        //        return await Task.Run(() => userFoods.Sum(food => food.Calories));
        //    }
        //    return await Task.FromResult(0d);
        //}

        public double GetTotalCalories(List<UserDailyFood> userDailyFoods)
        {

            double totalCalories = userDailyFoods.Sum(udf =>
            {
                double caloriesPerGram = udf.Calories / 100; // Calories per gram
                return caloriesPerGram * udf.Weight; // Total calories for this food item
            });

            return totalCalories;
        }
        public double GetTotalProtiens(List<UserDailyFood> userDailyFoods)
        {

            double totalProtiens = userDailyFoods.Sum(udf =>
            {
                double protiensPerGram = udf.Protein / 100; 
                return protiensPerGram * udf.Weight; 
            });

            return totalProtiens;
        }
        public double GetTotalFats(List<UserDailyFood> userDailyFoods)
        {

            double totalFats = userDailyFoods.Sum(udf =>
            {
                double fatsPerGram = udf.Fat / 100; // fats per gram
                return fatsPerGram * udf.Weight; // Total fats for this food item
            });

            return totalFats;
        }
        public double GetTotalCarbohydrates(List<UserDailyFood> userDailyFoods)
        {

            double totalCarbohydrates = userDailyFoods.Sum(udf =>
            {
                double CarbohydratesPerGram = udf.Carbohydrates / 100; // Carbohydrates per gram
                return CarbohydratesPerGram * udf.Weight; // Total Carbohydrates for this food item
            });

            return totalCarbohydrates;
        }
        public double GetTotalFibers(List<UserDailyFood> userDailyFoods)
        {

            double totalFibers = userDailyFoods.Sum(udf =>
            {
                double FibersPerGram = udf.Fiber / 100; // Fibers per gram
                return FibersPerGram * udf.Weight; // Total Fibers for this food item
            });

            return totalFibers;
        }
    }
}
