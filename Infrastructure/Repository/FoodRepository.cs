using System.Text.Json;
using Core.Interfaces;
using Core.Models;
using Core.UtilityModels;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class FoodRepository : Repository<Food>, IFoodRepository
    {
        private readonly AppDbContext _appDbContext;
        public FoodRepository(AppDbContext appDb) : base(appDb)
        {
            _appDbContext = appDb;
        }

        public async Task<Food> UpdateAsync(Food food)
        {
            _appDbContext.Update(food);
            await _appDbContext.SaveChangesAsync();
            return food;
        }
        public async Task DeleteAllFoods()
        {
            _appDbContext.Foods.RemoveRange(_appDbContext.Foods);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<List<Food>> GetAllAsync(string? search = null, string? orderBy = null)
        {
            var foods = await _appDbContext.Foods.ToListAsync();

            if (!string.IsNullOrEmpty(search))
            {
                foods = foods.Where(f => f.Name.ToLower().Contains(search.ToLower()) || f.Description.ToLower().Contains(search.ToLower())).ToList();
            }

            if (!string.IsNullOrEmpty(orderBy))
            {
               switch (orderBy.ToLower())
               {
                    case "name":
                        foods = foods.OrderBy(f => f.Name).ToList();
                        break;
                    case "serving":
                        foods = foods.OrderBy(f => f.Serving).ToList();
                        break;
                    case "calories":
                        foods = foods.OrderBy(f => f.Calories).ToList();
                        break;
                    case "ourfood":
                        foods = foods.OrderByDescending(f=> f.Id).ToList();
                        break;
               }
            }   

            return foods;

        }

        public async Task<List<Food>> SearchAsync(string name, int pageSize = 0, int pageNumber=1 )
        {
            var foods = await GetAllAsync();

            var foodsToList = foods.Where(f=> f.Name == name || f.Serving == name).AsQueryable();

            if (pageSize > 0)
            {
                if (pageSize > 100)
                {
                    pageSize = 100;
                }
                foodsToList = foodsToList.Skip(pageSize * (pageNumber - 1)).Take(pageSize);
            }

            return foodsToList.ToList();
        }

        public async Task AddFoodRate(string appUserId, int foodId, int rate)
        {
            await _appDbContext.FoodRatings.AddAsync(new FoodRating
            {
                AppUserId = appUserId,
                FoodId = foodId,
                Rate = rate
            });
            await UpdateFoodsRate(foodId, rate);
        }
        public async Task UpdateFoodRate(FoodRating foodRating)
        {
            _appDbContext.FoodRatings.Update(foodRating);
            await UpdateFoodsRate(foodRating.FoodId, foodRating.Rate);
        }

        public async Task UpdateFoodsRate(int foodId, int rate)
        {
            var ratingsData = File.ReadAllText("../wwwroot/wwwroot/SeedData/Ratings.json");
            List<KaggleRating> ratings = JsonSerializer.Deserialize<List<KaggleRating>>(ratingsData);

            ratings.Add(new KaggleRating {UserId = 999, FoodId = foodId, Rate = rate });

            var avgRatings = ratings
                .Where(r => r.FoodId == foodId)
                .Select(r => r.Rate)
                .Average();

            var food = await _appDbContext.Foods.FirstOrDefaultAsync(f => f.Id == foodId);
            food.AvgRating = avgRatings;
            
            await UpdateAsync(food);

        }

        public async Task<bool> IsFoodRated(string appUserId, int foodId)
        {
            var flag = await _appDbContext.FoodRatings.AnyAsync(f => f.AppUserId == appUserId && f.FoodId == foodId);
            return flag;
        }

    }
}
