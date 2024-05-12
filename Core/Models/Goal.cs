using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class Goal
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public List<UserGoals> UserGoals { get; set; }

    }
}
