﻿namespace Core.Models
{
    public class UserFoods
    {
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public int FoodId { get; set; }
        public Food Food { get; set; }

    }
}
