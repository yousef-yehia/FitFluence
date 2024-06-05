using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO.FoodDto;
using Application.Foods.Create;
using Application.Helper;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Core.Models;
using Infrastructure.Data;

namespace Application.Foods.GetAll
{
    internal class GetAllFoodQueryHandler : IRequestHandler<GetAllFoodQuery, Pagination<FoodDto>>
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;

        public GetAllFoodQueryHandler(AppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        public async Task<Pagination<FoodDto>> Handle(GetAllFoodQuery request, CancellationToken cancellationToken)
        {
            var foods = await _appDbContext.Foods.ToListAsync();

            if (!string.IsNullOrEmpty(request.Search))
            {
                foods = foods.Where(f => f.Name.Contains(request.Search) || f.Serving.Contains(request.Search)).ToList();
            }

            if (!string.IsNullOrEmpty(request.Order))
            {
                switch (request.Order)
                {
                    case "name":
                        foods = foods.OrderBy(f => f.Name).ToList();
                        break;
                    case "serving":
                        foods = foods.OrderBy(f => f.Serving).ToList();
                        break;
                    case "calories":
                        foods = foods.OrderBy(f => f.Calories).ToList();
                        break;
                }
            }

            var foodListToReturn = _mapper.Map<List<FoodDto>>(foods);
            var PaginatedFoodsResponse = Pagination<FoodDto>.Paginate(foodListToReturn, request.PageNumber, request.PageSize);
            return PaginatedFoodsResponse;
        }
    }
}
