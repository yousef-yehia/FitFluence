﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.FoodDto
{
    public class CreateFoodDto
    {
        public string Name { get; set; }
        public bool Verified { get; set; }
        public string Serving { get; set; }
        public double Calories { get; set; }
        public double Fat { get; set; }
        public double Carbohydrates { get; set; }
        public double Protein { get; set; }
        public double Fiber { get; set; }
    }
}
