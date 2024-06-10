using Core.Interfaces;
using Core.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class FoodRepository : Repository<Food>, IFoodRepository
    {
        private readonly AppDbContext _appDbContext;
        public FoodRepository(AppDbContext appDb, AppDbContext appDbContext) : base(appDb)
        {
            _appDbContext = appDbContext;
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
                foods = foods.Where(f => f.Name.Contains(search) || f.Serving.Contains(search)).ToList();
            }

            if (!string.IsNullOrEmpty(orderBy))
            {
               switch (orderBy)
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
    }
}
