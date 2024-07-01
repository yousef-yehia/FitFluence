namespace Api.DTO.FoodDto
{
    public class FoodReturnDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Serving { get; set; }
        public string Description { get; set; }
        public bool Verified { get; set; }
        public double Calories { get; set; }
        public double Fat { get; set; }
        public double Carbohydrates { get; set; }
        public double Protein { get; set; }
        public double Fiber { get; set; }
        public string ImageUrl { get; set; }
        public double? AvgRating { get; set; }
        public bool IsFavourite { get; set; } = false;
        public int? Rate { get; set; }

    }
}
