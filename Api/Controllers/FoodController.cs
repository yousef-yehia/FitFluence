using System.Net;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Core.Interfaces;
using Core.Models;
using Api.Helper;
using Api.ApiResponses;
using Api.DTO.FoodDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Api.Extensions;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;


namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IFoodRepository _foodRepository;
        private readonly IPhotoService _photoService;
        private readonly IFoodRecommendationService _foodRecommendationService;
        private readonly IMapper _mapper;
        protected ApiResponse _response;


        public FoodController(ApiResponse response, IMapper mapper, IFoodRepository foodRepository, UserManager<AppUser> userManager, IPhotoService photoService, IFoodRecommendationService foodRecommendationService)
        {

            _response = response;
            _mapper = mapper;
            _foodRepository = foodRepository;
            _userManager = userManager;
            _photoService = photoService;
            _foodRecommendationService = foodRecommendationService;
        }

        [HttpGet("GetAllFoods", Name = "GetAllFoods")]
        [Authorize]
        [ResponseCache(Duration = 10)]
        public async Task<ActionResult<ApiResponse>> GetAllFoods(string? search = null, string? order = null, int pageSize = 0, int pageNumber = 1)
        {
            try
            {
                var user = await _userManager.FindByEmailFromClaimsPrincipalWithFoods(User);
                var foods = await _foodRepository.GetAllAsync(search, order);

                var foodsResponse = CustomMappers.MapFoodToFoodReturnDto(foods, user.FavouriteFoods, user.FoodRatings);
                var paginatedFoods = Pagination<FoodReturnDto>.Paginate(foodsResponse, pageNumber, pageSize);

                return Ok(_response.OkResponse(paginatedFoods));
            }
            catch (Exception ex)
            {
                return BadRequest(_response.BadRequestResponse(ex.Message));
            }
        }

        [HttpGet("GetFood", Name = "GetFood")]
        [Authorize]
        [ResponseCache(Duration = 10)]
        public async Task<ActionResult<ApiResponse>> GetFood(int id)
        {
            try 
            {
                if (id == 0)
                {
                    return BadRequest(_response.BadRequestResponse("Id is required"));
                }

                var food = await _foodRepository.GetAsync(f=> f.Id == id);


                if (food == null)
                {
                    return NotFound(_response.NotFoundResponse("Food not found"));
                }

                var user = await _userManager.FindByEmailFromClaimsPrincipalWithFoods(User);
                var foodsResponse = CustomMappers.MapFoodToFoodReturnDto(food, user.FavouriteFoods, user.FoodRatings);

                return Ok(_response.OkResponse(foodsResponse));

            }
            catch (Exception ex) 
            {
                return BadRequest(_response.BadRequestResponse(ex.Message));
            }

        }

        [HttpGet("GetFoodRecommendations", Name = "GetFoodRecommendations")]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> GetFoodRecommendations(string foodName)
        {
            try 
            {
                var foodId = await _foodRepository.GetFoodIdByName(foodName);

                if (foodId == 0)
                {
                    return BadRequest(_response.BadRequestResponse("Food name is wrong please provide a correct name"));
                }

                var foodsId = await _foodRecommendationService.GetRecommendationsAsync(foodId);

                var foods = await _foodRepository.GetFoodsByListOfIdsAsync(foodsId);

                if (foods == null)
                {
                    return NotFound(_response.NotFoundResponse("No Foods are found"));
                }

                var user = await _userManager.FindByEmailFromClaimsPrincipalWithFoods(User);

                var foodsResponse = CustomMappers.MapFoodToFoodReturnDto(foods, user.FavouriteFoods, user.FoodRatings);

                return Ok(_response.OkResponse(foodsResponse));

            }
            catch (Exception ex) 
            {
                return BadRequest(_response.BadRequestResponse(ex.Message));
            }

        }

        [HttpPost("CreateFood", Name = "CreateFood")]
        [Authorize(Roles = "admin,coach")]

        public async Task<ActionResult<ApiResponse>> CreateFood(CreateFoodDto createFoodDto)
        {
            try 
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(_response.BadRequestResponse(""));
                }

                var user = await _userManager.FindByEmailFromClaimsPrincipal(User);

                if (createFoodDto == null)
                {
                    return BadRequest(_response.BadRequestResponse("Food is required"));
                }
                if( await _foodRepository.DoesExistAsync(f=> f.Name == createFoodDto.Name))
                {
                    return BadRequest(_response.BadRequestResponse("Food already exists"));
                }

                var food = _mapper.Map<Food>(createFoodDto);

                if( _userManager.GetRolesAsync(user).Result.FirstOrDefault() == Core.Models.Role.roleAdmin)
                {
                    food.Verified = true;
                }
                var imageResult = await _photoService.AddFoodPhotoAsync(createFoodDto.Image);
                food.ImageURL = imageResult.Url.ToString();

                await _foodRepository.CreateAsync(food);    

                return Ok(_response.OkResponse("Food Created Success"));
            }
            catch (Exception ex) 
            {
                return BadRequest(_response.BadRequestResponse(ex.Message));
            }

        }

        [HttpPost("GiveFoodRate", Name = "GiveFoodRate")]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> GiveFoodRate(int foodId, double rating)
        {
            try 
            {
                var user = await _userManager.FindByEmailFromClaimsPrincipal(User);

                if (foodId == 0)
                {
                    return BadRequest(_response.BadRequestResponse("Food is required"));
                }  
                if (rating > 10)
                {
                    return BadRequest(_response.BadRequestResponse("rate can not be bigger than 10"));
                }

                if(! await _foodRepository.DoesExistAsync(f=> f.Id == foodId))
                {
                    return BadRequest(_response.BadRequestResponse("Food Doesnot exists"));
                }

                if(await _foodRepository.IsFoodRated(user.Id, foodId))
                {
                    return BadRequest(_response.BadRequestResponse("Food is already rated"));
                }

                await _foodRepository.AddFoodRate(user.Id, foodId, rating);    

                return Ok(_response.OkResponse("Food Rated Success"));
            }
            catch (Exception ex) 
            {
                return BadRequest(_response.BadRequestResponse(ex.Message));
            }
        }

        [HttpPut("UpdateFoodRate", Name = "UpdateFoodRate")]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> UpdateFoodRate(int foodId, int rating)
        {
            try 
            {
                var user = await _userManager.FindByEmailFromClaimsPrincipal(User);

                if (foodId == 0)
                {
                    return BadRequest(_response.BadRequestResponse("Food is required"));
                }  
                if (rating > 10)
                {
                    return BadRequest(_response.BadRequestResponse("rate can not be bigger than 10"));
                }

                if(!await _foodRepository.DoesExistAsync(f=> f.Id == foodId))
                {
                    return BadRequest(_response.BadRequestResponse("Food Doesnot exists"));
                }

                if(!await _foodRepository.DoesExistAsync(f=> f.Id == foodId))
                {
                    return BadRequest(_response.BadRequestResponse("Food Doesnot exists"));
                }

                if(!await _foodRepository.IsFoodRated(user.Id, foodId))
                {
                    return BadRequest(_response.BadRequestResponse("Food isnot rated"));
                }

                FoodRating foodRating = new FoodRating
                {
                    AppUserId = user.Id,
                    FoodId = foodId,
                    Rate = rating
                };

                await _foodRepository.UpdateFoodRate(foodRating);    

                return Ok(_response.OkResponse("Food Rated Success"));
            }
            catch (Exception ex) 
            {
                return BadRequest(_response.BadRequestResponse(ex.Message));
            }
        }

        [HttpDelete("DeleteFood", Name = "DeleteFood")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<ApiResponse>> DeleteFood(int id)
        {
            try
            {
                if (!await _foodRepository.DoesExistAsync(f => f.Id == id))
                {
                    return NotFound(_response.NotFoundResponse("Food not found"));
                }

                var food = await _foodRepository.GetAsync(f => f.Id == id);
                await _foodRepository.DeleteAsync(food);

                return Ok(_response.OkResponse("Food Deleted Success"));
            }
            catch (Exception ex)
            {
                return BadRequest(_response.BadRequestResponse(ex.Message));
            }
        }

        //[HttpDelete("DeleteAllFood", Name = "DeleteAllFood")]
        //[Authorize(Roles = "admin")]
        //public async Task<ActionResult<ApiResponse>> DeleteAllFood()
        //{

        //    await _foodRepository.DeleteAllFoods();
        //    _response.StatusCode = HttpStatusCode.NoContent;
        //    _response.IsSuccess = true;
        //    return Ok(_response);
        //}

        [HttpPut("UpdateFood {id:int}", Name = "UpdateFood")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateFood(int id, [FromBody] CreateFoodDto foodDto)
        {
            try
            {
                if(! await _foodRepository.DoesExistAsync(f => f.Id == id))
                {
                    return NotFound(_response.NotFoundResponse("Food not found"));
                }

                await _foodRepository.UpdateAsync(_mapper.Map<Food>(foodDto));

                return Ok(_response.OkResponse("Food Updated Success"));

            }
            catch (Exception ex)
            {
                return BadRequest(_response.BadRequestResponse(ex.Message));
            }
        }
    }
}
