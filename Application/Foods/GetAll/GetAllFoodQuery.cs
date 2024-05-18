using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO.FoodDto;
using Application.Helper;
using MediatR;

namespace Application.Foods.GetAll
{
    public record GetAllFoodQuery(
        string? Search, 
        string? Order, 
        int PageSize = 0, 
        int PageNumber = 1): IRequest<Pagination<FoodDto>>;
}
