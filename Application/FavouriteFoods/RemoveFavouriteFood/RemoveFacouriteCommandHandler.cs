using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Exceptions;
using Core.Interfaces;
using MediatR;

namespace Application.FavouriteFoods.RemoveFavouriteFood
{
    internal class RemoveFacouriteCommandHandler : IRequestHandler<RemoveFavouriteFoodCommand>
    {
        private readonly IFoodRepository _foodRepository;
        private readonly IFavouriteFoodRepository _favouriteFoodRepository;
        public RemoveFacouriteCommandHandler(IFoodRepository foodRepository, IFavouriteFoodRepository favouriteFoodRepository)
        {
            _foodRepository = foodRepository;
            _favouriteFoodRepository = favouriteFoodRepository;
        }

        public async Task Handle(RemoveFavouriteFoodCommand request, CancellationToken cancellationToken)
        {
            //var food = await _foodRepository.GetAsync(f => f.Id == request.foadId);
            //if (food == null)
            //{
            //    throw new NotFoundExeption("The food id is wrong");
            //}
            //if(!_favouriteFoodRepository.IsFoodInFavouriteFoods(request.AppUser, request.foadId))
            //{
            //    throw new NotFoundExeption("This food is not in the favourite foods list");
            //}
            //await _favouriteFoodRepository.RemoveFavouriteFoodAsync(request.AppUser, food);
        }
    }
}
