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
        private readonly IFavouriteFoodRepository _favouriteFoodRepository;

        public AddFavouriteFoodCommandHandler(IFavouriteFoodRepository favouriteFoodRepository)
        {
            _favouriteFoodRepository = favouriteFoodRepository;
        }

        public async Task Handle(AddFavouriteFoodCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if(_favouriteFoodRepository.IsFoodInFavouriteFoods(request.User, request.FoodId))
                {
                    throw new Exception("already exists");
                }

                await _favouriteFoodRepository.AddFavouriteFoodAsync(request.User, request.FoodId);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
