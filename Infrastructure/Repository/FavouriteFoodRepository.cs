﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class FavouriteFoodRepository : IFavouriteFoodRepository
    {
        private readonly AppDbContext _appDbContext;

        public FavouriteFoodRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task AddFavouriteFoodAsync(AppUser user, Food food)
        {
            user.FavouriteFoods.Add(new FavouriteFood { AppUserId = user.Id, FoodId = food.Id });
            await _appDbContext.SaveChangesAsync();
        }
        public async Task AddFavouriteFoodAsync(AppUser user, int foodId)
        {
            user.FavouriteFoods.Add(new FavouriteFood { AppUserId = user.Id, FoodId = foodId });
            await _appDbContext.SaveChangesAsync();
        }

        //public async Task RemoveFavouriteFoodAsync(AppUser appUser, Food food)
        //{
        //    var user = await _appDbContext.Users
        //        .Include(u => u.UserFoods)
        //        .ThenInclude(ug => ug.Food)
        //        .FirstOrDefaultAsync(u => u.Id == appUser.Id);

        //    var userFood = user.UserFoods.FirstOrDefault(a => a.AppUserId == user.Id && a.FoodId == food.Id);

        //    user.UserFoods.Remove(userFood);

        //    await _appDbContext.SaveChangesAsync();
        //}
        public async Task RemoveFavouriteFoodAsync(AppUser appUser, int foodId)
        {

            var userFood = appUser.FavouriteFoods.FirstOrDefault(a => a.AppUserId == appUser.Id && a.FoodId == foodId);

            appUser.FavouriteFoods.Remove(userFood);

            await _appDbContext.SaveChangesAsync();
        }

        public async Task<List<Food>> GetAllFavouriteFoodsAsync(AppUser appUser)
        {
            var userFoods = await _appDbContext.FavouriteFoods.Where(u => u.AppUserId == appUser.Id).Select(x => x.Food).ToListAsync();
            return userFoods;
        }

        public bool IsFoodInFavouriteFoods(AppUser appUser, int foodId)
        {
            return (appUser.FavouriteFoods.Any(uf => uf.FoodId == foodId));
        }


    }
}
