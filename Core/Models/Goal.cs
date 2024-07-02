using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class Goal
    {
        [Key]
        public string Name { get; set; }
        public ICollection<AppUser> AppUsers  { get; set; }

    }
}
