using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO.FoodDto;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Core.Models;
using Application.Exceptions;
using Infrastructure.Data;

namespace Application.Foods.GetById
{
    internal class GetFoodByIdQueryHandler : IRequestHandler<GetFoodByIdQuery, FoodDto>
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;

        public GetFoodByIdQueryHandler(AppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        public async Task<FoodDto> Handle(GetFoodByIdQuery request, CancellationToken cancellationToken)
        {
            var food = await _appDbContext.Foods.FirstOrDefaultAsync( f=> f.Id == request.Id);

            if (food == null)
            {
                throw new NotFoundExeption("There is no food with this id");
            }
            return _mapper.Map<FoodDto>(food);
        }
    }
}   
