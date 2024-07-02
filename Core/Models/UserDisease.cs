using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class UserDisease
    {
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public string DiseaseName { get; set; }
        public Disease Disease { get; set; }
    }
}
