using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO.FoodDto;
using Core.Models;
using MediatR;

namespace Application.Foods.Update
{
    public record UpdateFoodCommand(int Id, CreateFoodDto FoodDto) : IRequest<Food>;
}
