using Core.Models;
using CsvHelper.Configuration;

namespace Infrastructure.Helper
{
    public class CsvMap : ClassMap<Food>
    {
        public CsvMap()
        {
            // Ignore the first column (ID)
            Map(m => m.Id).Index(0).Ignore();
            Map(m => m.Name).Index(1);
            Map(m => m.Verified).Index(2);
            Map(m => m.Serving).Index(3);
            Map(m => m.Calories).Index(4);
            Map(m => m.Fat).Index(5);
            Map(m => m.Carbohydrates).Index(6);
            Map(m => m.Protein).Index(7);
            Map(m => m.Fiber).Index(8);
        }
    }
}
