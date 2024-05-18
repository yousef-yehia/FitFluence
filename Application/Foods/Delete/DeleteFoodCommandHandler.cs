using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Exceptions;
using Core.Interfaces;
using MediatR;

namespace Application.Foods.Delete
{
    internal class DeleteFoodCommandHandler : IRequestHandler<DeleteFoodCommand>
    {
        private readonly IFoodRepository _foodRepository;

        public DeleteFoodCommandHandler(IFoodRepository foodRepository)
        {
            _foodRepository = foodRepository;
        }

        public async Task Handle(DeleteFoodCommand request, CancellationToken cancellationToken)
        {
            var food = await _foodRepository.GetAsync( f => f.Id == request.Id);
            if (food == null) 
            {
                throw new NotFoundExeption("Food not found");
            }
            await _foodRepository.DeleteAsync(food);
        }
    }
}
