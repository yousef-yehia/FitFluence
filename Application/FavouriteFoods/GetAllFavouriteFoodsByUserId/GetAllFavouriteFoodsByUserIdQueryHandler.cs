using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO.FoodDto;
using Application.Helper;
using AutoMapper;
using Core.Interfaces;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.FavouriteFoods.GetAllFavouriteFoodsByUserId
{
    internal class GetAllFavouriteFoodsByUserIdQueryHandler : IRequestHandler<GetAllFavouriteFoodsByUserIdQuery, Pagination<FoodDto>>
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;

        public GetAllFavouriteFoodsByUserIdQueryHandler(AppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        public async Task<Pagination<FoodDto>> Handle(GetAllFavouriteFoodsByUserIdQuery request, CancellationToken cancellationToken)
        {
            var userFoods = await _appDbContext.UserFoods.Where(u => u.AppUserId == request.userId).Select(x => x.Food).ToListAsync();
            var foodListToReturn = _mapper.Map<List<FoodDto>>(userFoods);
            var PaginatedFoodsResponse = Pagination<FoodDto>.Paginate(foodListToReturn, request.PageNumber, request.PageSize);
            return PaginatedFoodsResponse;
        }
    }
}
