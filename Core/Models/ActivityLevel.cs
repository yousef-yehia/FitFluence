﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class ActivityLevel
    {
        [Key]
        public string Name { get; set; }
        public ICollection<AppUser> AppUsers { get; set; }
    }
}
