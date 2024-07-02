using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Disease
    {
        [Key]
        public string Name { get; set; }
        public List<UserDisease>? UserDiseases { get; set; }
    }
}
