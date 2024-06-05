using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Abstractions.Messaging;
using Application.DTO.FoodDto;
using Application.Helper;
using Core.Models;
using MediatR;

namespace Application.FavouriteFoods.AddFavouriteFood
{
    public record AddFavouriteFoodCommand(
        AppUser User,
        int FoodId
        ) : IRequest;
}
