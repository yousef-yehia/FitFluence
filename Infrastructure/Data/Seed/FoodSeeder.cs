using CsvHelper;
using System.Globalization;
using Core.Models;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Helper;

namespace Infrastructure.Data.Seed
{
    public class FoodSeeder
    {
        private readonly IServiceProvider _serviceProvider;

        public FoodSeeder(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Seed()
        {

            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var csvFilePath = "../Infrastructure/Data/SeedData/new_food.csv";

                if (dbContext.Foods.Any())
                {
                    return;
                }

                using (var reader = new StreamReader(csvFilePath))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Context.RegisterClassMap<CsvMap>();

                    var records = csv.GetRecords<Food>().ToList();

                    // Add the records to the database
                    dbContext.AddRange(records);
                    dbContext.SaveChanges();
                }
            }
        }
    }
}
