using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.Models;
using Infrastructure.Data;
using Infrastructure.Data.Migrations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class DietPlanRepository : Repository<DietPlan>, IDietPlanRepository
    {
        private readonly AppDbContext _appDbContext;
        public DietPlanRepository(AppDbContext appDb) : base(appDb)
        {
            _appDbContext = appDb;
        }
        public async Task AddFoodToDietPLanAsync(int foodId, int dietPlanId, double weight)
        {
            await _appDbContext.DietPlanFoods.AddAsync(new DietPlanFood
            {
                DietPlanId = dietPlanId,
                FoodId = foodId,
                Weight = weight
            });
            await SaveAsync();
        }

        public async Task RemoveFoodFromDietPlanAsync(int foodId, int dietPlanId)
        {
           var dietPlanFood = await GetDietPlanFoodAsync(foodId, dietPlanId);
           _appDbContext.DietPlanFoods.Remove(dietPlanFood);
            await SaveAsync();
        }

        public async Task UpdateDietPlanAsync(DietPlan dietPlan)
        {
            _appDbContext.DietPlans.Update(dietPlan);
            await SaveAsync();
        }
        private async Task<DietPlanFood> GetDietPlanFoodAsync(int foodId, int dietPlanId)
        {
            return await _appDbContext.DietPlanFoods.FirstOrDefaultAsync(dpf=> dpf.DietPlanId == dietPlanId && dpf.FoodId == foodId);
        }
    }
}
