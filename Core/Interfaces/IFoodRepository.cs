using Core.Models;

namespace Core.Interfaces
{
    public interface IFoodRepository : IRepository<Food>
    {
        public Task DeleteAllFoods();
        public Task<Food> UpdateAsync(Food food);
        public Task<List<Food>> SearchAsync(string name, int pageSize = 0, int pageNumber = 1);
        public Task<List<Food>> GetAllFoodsAsync(string? search = null, string? orderBy = null);

    }
}
