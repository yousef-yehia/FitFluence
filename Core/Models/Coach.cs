namespace Core.Models
{
    public class Coach
    {
        public int CoachId { get; set; }
        public string? CvUrl { get; set; }
        public double MonthlyPrice { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public List<CoachAndClient> CoachsAndClients { get; set; }

    }
}
