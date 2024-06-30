using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;

namespace Core.Interfaces
{
    public interface IDietPlanRepository : IRepository<DietPlan>
    {
        public Task UpdateDietPlanAsync(DietPlan dietPlan);
        public Task AddFoodToDietPLanAsync(int foodId, int dietPlanId, double weight);
        public Task RemoveFoodFromDietPlanAsync(int foodId, int dietPlanId);
    }
}
