using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Models;

namespace Infrastructure.Data
{
    public static class AppDbContextSeed
    {
        public static async Task SeedDataAsync(AppDbContext _dbContext)
        {
            if (!_dbContext.Foods.Any())
            {
                var FoodsData = File.ReadAllText("../Infrastructure/Data/SeedData/new_food.json");
                var Foods = JsonSerializer.Deserialize<List<Food>>(FoodsData);
                _dbContext.Foods.AddRange(Foods);
            }

            if (_dbContext.ChangeTracker.HasChanges()) await _dbContext.SaveChangesAsync();

        }

    }
}
