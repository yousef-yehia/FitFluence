using Api.ApiResponses;
using Api.DTO.FoodDto;
using Api.DTO.UserDailyFoodDto;
using Api.Extensions;
using AutoMapper;
using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserDailyFoodsController : ControllerBase
    {
        private readonly IUserDailyFoodsRepository _userDailyFoodsRepository;
        private readonly IFoodRepository _foodRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly ApiResponse _response;
        private readonly IMapper _mapper;

        public UserDailyFoodsController(IUserDailyFoodsRepository userFoodService, UserManager<AppUser> userManager, IFoodRepository foodRepository, ApiResponse response, IMapper mapper)
        {
            _userDailyFoodsRepository = userFoodService;
            _userManager = userManager;
            _foodRepository = foodRepository;
            _response = response;
            _mapper = mapper;
        }

        [HttpPost("addFood")]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> AddFoodSelection(UserDailyFoodRequestDto foodRequestDto)
        {
            try
            {
                var user = await _userManager.FindByEmailFromClaimsPrincipal(User);
                var food = await _foodRepository.GetAsync(f => f.Id == foodRequestDto.FoodId);

                if (food == null)
                {
                    return NotFound(_response.NotFoundResponse("food not found"));
                }
                var foodToAdd = _mapper.Map<UserDailyFood>(food);
                foodToAdd.Weight = foodRequestDto.Weight;

                await _userDailyFoodsRepository.AddFoodSelectionAsync(user.Id, foodToAdd);

                return Ok(_response.OkResponse("food added"));
            }
            catch(Exception ex)
            {
                return BadRequest(_response.BadRequestResponse(ex.Message));    
            }

        }

        [HttpGet("GetUserDailyFoods")]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> GetFoodSelections()
        {
            var user = await _userManager.FindByEmailFromClaimsPrincipal(User);

            var userFoods = _userDailyFoodsRepository.GetFoodSelections(user.Id);
            var totalCalories = _userDailyFoodsRepository.GetTotalCalories(userFoods);
            var totalProtiens = _userDailyFoodsRepository.GetTotalProtiens(userFoods);
            var totalFats = _userDailyFoodsRepository.GetTotalFats(userFoods);
            var totalCarbohydrates = _userDailyFoodsRepository.GetTotalCarbohydrates(userFoods);
            var totalfibers = _userDailyFoodsRepository.GetTotalFibers(userFoods);

            var userfoodsToReturn = new UserDailyFoodDto
            {
                Foods = userFoods,
                CaloriesSum = totalCalories,
                ProtiensSum = totalProtiens,
                CarbohydratesSum = totalCarbohydrates,
                FatsSum= totalFats,
                FibersSum = totalfibers,
            };


            return Ok(_response.OkResponse(userfoodsToReturn));
        }

        //[HttpGet("GetUserDailyFoodsCalories")]
        //[Authorize]
        //public async Task<IActionResult> GetTotalCalories()
        //{
        //    var user = await _userManager.FindByEmailFromClaimsPrincipal(User);

        //    var totalCalories = await _userDailyFoodsRepository.GetTotalCaloriesAsync(user.Id);
        //    return Ok(totalCalories);
        //}
    }
}
