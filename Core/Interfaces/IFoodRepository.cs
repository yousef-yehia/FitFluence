using Core.Models;

namespace Core.Interfaces
{
    public interface IFoodRepository : IRepository<Food>
    {
        public Task DeleteAllFoods();
        public Task<Food> UpdateAsync(Food food);
        public Task<List<Food>> GetAllAsync(string? search = null, string? orderBy = null);
        public Task<bool> IsFoodRated(string appUserId, int foodId);
        public Task AddFoodRate(string appUserId, int foodId, int rate);
        public Task UpdateFoodRate(FoodRating foodRating);
        public Task UpdateFoodsRate(int foodId, int rate);
        public Task<List<Food>> GetFoodsByListOfIdsAsync(List<int> foodsId);
        public Task<int> GetFoodIdByName(string name);

    }
}
