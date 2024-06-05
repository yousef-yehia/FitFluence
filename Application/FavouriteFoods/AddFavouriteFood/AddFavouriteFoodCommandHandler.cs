using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Exceptions;
using AutoMapper;
using Core.Interfaces;
using Infrastructure.Repository;
using MediatR;

namespace Application.FavouriteFoods.AddFavouriteFood
{
    internal class AddFavouriteFoodCommandHandler : IRequestHandler<AddFavouriteFoodCommand>
    {
        private readonly IFoodRepository _foodRepository;
        private readonly IFavouriteFoodRepository _favouriteFoodRepository;

        public AddFavouriteFoodCommandHandler(IFavouriteFoodRepository favouriteFoodRepository, IFoodRepository foodRepository)
        {
            _favouriteFoodRepository = favouriteFoodRepository;
            _foodRepository = foodRepository;
        }

        public async Task Handle(AddFavouriteFoodCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var food = await _foodRepository.GetAsync(f => f.Id == request.FoodId);

                if (food == null)
                {
                    throw new NotFoundExeption("this food id does not exist");
                }
                await _favouriteFoodRepository.AddFavouriteFoodAsync(request.User, food, cancellationToken);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
