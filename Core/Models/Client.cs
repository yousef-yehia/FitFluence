namespace Core.Models
{
    public class Client 
    {
        public int ClientId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Weight { get; set; }
        public int Height { get; set; }
        public int Age { get; set; }
        public int MuscleWeight { get; set; }
        public int FatWeight { get; set; }
        public string? ImageUrl { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
