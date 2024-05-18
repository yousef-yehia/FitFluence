using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Exceptions;
using Core.Interfaces;
using Core.Models;
using MediatR;

namespace Application.Foods.Update
{
    internal class UpdateFoodCommandHandler : IRequestHandler<UpdateFoodCommand, Food>
    {
        private readonly IFoodRepository _foodRepository;

        public UpdateFoodCommandHandler(IFoodRepository foodRepository)
        {
            _foodRepository = foodRepository;
        }

        public async Task<Food> Handle(UpdateFoodCommand request, CancellationToken cancellationToken)
        {
            var food = await _foodRepository.GetAsync(f => f.Id == request.Id);

            if (food == null) 
            {
                throw new NotFoundExeption("Food not found");
            }

            food.Update(request.FoodDto.Name, request.FoodDto.Serving , request.FoodDto.Calories, request.FoodDto.Protein, request.FoodDto.Fat, request.FoodDto.Carbohydrates);

            await _foodRepository.UpdateAsync(food);
            return food;
        }
    }
}
