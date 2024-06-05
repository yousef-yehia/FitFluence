using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;
using MediatR;

namespace Application.FavouriteFoods.RemoveFavouriteFood
{
    public record RemoveFavouriteFoodCommand(
        AppUser AppUser,
        int foadId
        ) : IRequest;
}
