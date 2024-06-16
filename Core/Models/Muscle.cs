namespace Core.Models
{
    public class Muscle
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public ICollection<Exercise> Exercises { get; set; }
    }
}