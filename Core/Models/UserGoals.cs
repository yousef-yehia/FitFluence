namespace Core.Models
{
    public class UserGoals
    {
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public int GoalId { get; set; }
        public Goal Goal { get; set; }
    }
}
