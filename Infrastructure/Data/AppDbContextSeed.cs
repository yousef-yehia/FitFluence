using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Models;
using Core.UtilityModels;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public static class AppDbContextSeed
    {
        public static async Task SeedDataAsync(AppDbContext _dbContext)
        {
            //seed foods
            if (!_dbContext.Foods.Any())
            {
                var FoodsData = File.ReadAllText("../wwwroot/wwwroot/SeedData/Food1.json");
                var Foods = JsonSerializer.Deserialize<List<Food>>(FoodsData);
                _dbContext.Foods.AddRange(Foods);
                if (_dbContext.ChangeTracker.HasChanges()) await _dbContext.SaveChangesAsync();

            }
            //seed muscles
            if (!_dbContext.Muscles.Any())
            {
                var musclesData = File.ReadAllText("../wwwroot/wwwroot/SeedData/Muscles.json");
                var muscles = JsonSerializer.Deserialize<List<Muscle>>(musclesData);
                _dbContext.Muscles.AddRange(muscles);
                if (_dbContext.ChangeTracker.HasChanges()) await _dbContext.SaveChangesAsync();
            }
            //seed exercises
            if (!_dbContext.Exercises.Any())
            {
                var exerciseData = File.ReadAllText("../wwwroot/wwwroot/SeedData/Exercises.json");
                var exercise = JsonSerializer.Deserialize<List<Exercise>>(exerciseData);
                _dbContext.Exercises.AddRange(exercise);
                if (_dbContext.ChangeTracker.HasChanges()) await _dbContext.SaveChangesAsync();

            } 
            //seed avg ratings from kaggleRatings
            //if(_dbContext.Foods.Any(f => f.AvgRating == null))
            //{
            //    var ratingsData = File.ReadAllText("../wwwroot/wwwroot/SeedData/Ratings.json");
            //    List<KaggleRating> ratings = JsonSerializer.Deserialize<List<KaggleRating>>(ratingsData);

            //    // Calculate the average ratings
            //    var avgRatings = ratings
            //        .GroupBy(r => r.FoodId)
            //        .Select(g => new
            //        {
            //            FoodId = g.Key,
            //            AvgRating = g.Average(r => r.Rate)
            //        }).ToList();

            //    // Update the database
            //    foreach (var avgRating in avgRatings)
            //    {
            //        var food = _dbContext.Foods.SingleOrDefault(f => f.Id == avgRating.FoodId);
            //        if (food != null)
            //        {
            //            food.AvgRating = avgRating.AvgRating;
            //        }
            //    }
            //    _dbContext.SaveChanges();
            //}



            //if (_dbContext.ChangeTracker.HasChanges()) await _dbContext.SaveChangesAsync();

        }

    }
}
