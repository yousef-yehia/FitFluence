namespace Core.Models
{
    public class Client
    {
        public int ClientId { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public List<CoachsAndClients> CoachsAndClients { get; set; }

    }
}
