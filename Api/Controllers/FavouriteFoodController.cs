using System.Net;
using Api.ApiResponses;
using Api.Extensions;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Core.Interfaces;
using AutoMapper;
using Api.Helper;
using Api.DTO.FoodDto;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavouriteFoodController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IFavouriteFoodRepository _favouriteFoodRepository;
        private readonly IMapper _mapper;
        private readonly IFoodRepository _foodRepository;
        private readonly ApiResponse _response;

        public FavouriteFoodController(ApiResponse response, UserManager<AppUser> userManager, IFavouriteFoodRepository favouriteFoodRepository, IFoodRepository foodRepository, IMapper mapper)
        {
            _response = response;
            _userManager = userManager;
            _favouriteFoodRepository = favouriteFoodRepository;
            _foodRepository = foodRepository;
            _mapper = mapper;
        }

        [HttpGet("GetAllUserFoods", Name = "GetAllUserFoods")]
        [ResponseCache(Duration = 10)]
        [Authorize]

        public async Task<ActionResult<ApiResponse>> GetAllUserFoods(int pageSize = 0, int pageNumber = 1)
        {
            try
            {
                var user = await _userManager.FindByEmailFromClaimsPrincipalWithFoods(User);
                var foods = await _favouriteFoodRepository.GetAllFavouriteFoodsAsync(user);
                var foodsResponse = _mapper.Map<List<FoodDto>>(foods);
                var result = Pagination<FoodDto>.Paginate(foodsResponse, pageNumber, pageSize); 
                return Ok(_response.OkResponse(result));
            }
            catch (Exception ex)
            {
                return BadRequest(_response.BadRequestResponse(ex.Message));
            }
        }

        [HttpPost("AddFavouriteFood", Name = "AddFavouriteFood")]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> AddFavouriteFood(int foodId)
        {
            try
            {
                var user = await _userManager.FindByEmailFromClaimsPrincipalWithFoods(User);

                if (user.UserFoods == null)
                {
                    user.UserFoods = new List<UserFoods>();
                }

                if(! await _foodRepository.DoesExistAsync(f=> f.Id == foodId))
                {
                    return NotFound(_response.NotFoundResponse("food id doesnt exist"));
                }

                if (_favouriteFoodRepository.IsFoodInFavouriteFoods(user, foodId))
                {
                    return BadRequest(_response.BadRequestResponse("food is already in the favourite food list"));
                }

                await _favouriteFoodRepository.AddFavouriteFoodAsync(user, foodId);

                return Ok(_response.OkResponse("Favourite Food Added"));

            }
            catch (Exception ex)
            {
                return BadRequest(_response.BadRequestResponse(ex.Message));
            }
        }

        [HttpDelete("DeleteUserFavouriteFood", Name = "DeleteUserFavouriteFood")]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> DeleteUserFavouriteFood(int foodId)
        {
            try
            {
                var user = await _userManager.FindByEmailFromClaimsPrincipalWithFoods(User);

                if (user.UserFoods == null)
                {
                    user.UserFoods = new List<UserFoods>();
                }

                if (! await _foodRepository.DoesExistAsync(f => f.Id == foodId))
                {
                    return NotFound(_response.NotFoundResponse("food id doesnt exist"));
                }

                if (! _favouriteFoodRepository.IsFoodInFavouriteFoods(user, foodId))
                {
                    return BadRequest(_response.BadRequestResponse("food is already in the favourite food list"));
                }

                await _favouriteFoodRepository.RemoveFavouriteFoodAsync(user, foodId);

                return Ok(_response.OkResponse("Food is deleted from favourite food"));

            }
            catch (Exception ex)
            {
                return BadRequest(_response.BadRequestResponse(ex.Message));
            }
        }
    }
}
