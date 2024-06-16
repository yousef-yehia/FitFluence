using Microsoft.AspNetCore.Identity;
namespace Core.Models
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; }
        //public string? ImageUrl { get; set; }
        public Client? Client { get; set; }
        public Coach? Coach { get; set; }    
        public List<UserGoals> UserGoals { get; set; }
        public List<UserFoods> UserFoods  { get; set; }

    }
}
