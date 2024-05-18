using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO.FoodDto;
using MediatR;
using Core.Models;
using Core.Interfaces;
using AutoMapper;
using Application.Exceptions;


namespace Application.Foods.Create
{
    internal class CreateFoodCommandHandler : IRequestHandler<CreateFoodCommand, FoodDto>
    {
        private readonly IFoodRepository _foodRepository;
        private readonly IMapper _mapper;

        public CreateFoodCommandHandler(IFoodRepository foodRepository, IMapper mapper)
        {
            _foodRepository = foodRepository;
            _mapper = mapper;
        }

        public async Task<FoodDto> Handle(CreateFoodCommand request, CancellationToken cancellationToken)
        {
            if (await _foodRepository.GetAsync(u => u.Name.ToLower() == request.createFoodDto.Name.ToLower()) != null)
            {
                throw new AlreadyExistsException("This Food Already Exists");
            }
            Food food = _mapper.Map<Food>(request.createFoodDto);
            await _foodRepository.CreateAsync(food);

            var result = _mapper.Map<FoodDto>(food);
            return result;
        }


    }
}
