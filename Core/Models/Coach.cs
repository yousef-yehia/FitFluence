namespace Core.Models
{
    public class Coach 
    {
        public int CoachId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Cv { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
