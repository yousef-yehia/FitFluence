﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;

namespace Core.Interfaces
{
    public interface IUserDailyFoodsRepository
    {
        void AddFoodSelection(string userId, Food food);
        List<Food> GetFoodSelections(string userId);
        double GetTotalCalories(string userId);
    }
}
