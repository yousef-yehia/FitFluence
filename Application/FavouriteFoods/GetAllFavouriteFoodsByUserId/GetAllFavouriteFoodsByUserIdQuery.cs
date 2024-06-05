using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO.FoodDto;
using Application.Helper;
using Core.Models;
using MediatR;

namespace Application.FavouriteFoods.GetAllFavouriteFoodsByUserId
{
    public record GetAllFavouriteFoodsByUserIdQuery(
        string userId,
        int PageSize = 0,
        int PageNumber = 1): IRequest<Pagination<FoodDto>>;
}
