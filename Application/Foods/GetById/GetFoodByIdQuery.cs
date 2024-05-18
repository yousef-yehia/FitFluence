using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO.FoodDto;
using MediatR;

namespace Application.Foods.GetById
{
    public record GetFoodByIdQuery(int Id): IRequest<FoodDto>;
}
