using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;

namespace Core.Interfaces
{
    public interface IFavouriteFoodRepository
    {
        public Task AddFavouriteFoodAsync(AppUser user, Food foodId, CancellationToken cancellationToken);
        public Task RemoveFavouriteFoodAsync(AppUser appUser, Food food);
        public Task<List<Food>> GetAllFavouriteFoodsAsync(AppUser appUser);
        public bool IsFoodInFavouriteFoods(AppUser appUser, int foodId);

    }
}
