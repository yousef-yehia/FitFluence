using Api.ApiResponses;
using Api.Extensions;
using Api.Helper;
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
    public class DietPlanController : ControllerBase
    {
        private readonly IDietPlanRepository _dietPlanRepository;
        private readonly IFoodRepository _foodRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ApiResponse _response;

        public DietPlanController(IDietPlanRepository dietPlanRepository, IFoodRepository foodRepository, UserManager<AppUser> userManager, IMapper mapper, ApiResponse response)
        {
            _dietPlanRepository = dietPlanRepository;
            _foodRepository = foodRepository;
            _userManager = userManager;
            _mapper = mapper;
            _response = response;
        }

        [HttpGet("GetAllUserDietPlans", Name = "GetAllUserDietPlans")]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> GetAllUserDietPlans()
        {
            var user = await _userManager.FindByEmailFromClaimsPrincipalWithDietPlans(User);

            var dietPlans = user.DietPlans;
            var dietPlanResponses = CustomMappers.MapDietPlanToDietPlanReturnDto(dietPlans);
            return Ok(dietPlanResponses);
        }

        [HttpPost("CreateDietPlan", Name = "CreateDietPlan")]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> CreateDietPlan(string name)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = await _userManager.FindByEmailFromClaimsPrincipal(User);

                if (await _dietPlanRepository.DoesExistAsync(d => d.AppUserId == user.Id && d.Name == name))
                {
                    return BadRequest(_response.BadRequestResponse("Diet plan with this name already exists"));
                }

                DietPlan newDietPlan = new DietPlan
                {
                    Name = name,
                    AppUserId = user.Id,
                    DateCreated = DateTime.Now.Date,
                };

                await _dietPlanRepository.CreateAsync(newDietPlan);

                return Ok(_response.OkResponse("created"));
            }
            catch (Exception ex)
            {
                return BadRequest(_response.BadRequestResponse(ex.Message));
            }
        }

        [HttpPost("AddFoodToDietPlan", Name = "AddFoodToDietPlan")]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> AddFoodToDietPlan(int foodId, int dietPlanId, double weight)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (!await _foodRepository.DoesExistAsync(f => f.Id == foodId))
                {
                    return BadRequest(_response.BadRequestResponse("Food does not exist"));
                }
                if (!await _dietPlanRepository.DoesExistAsync(d => d.Id == dietPlanId))
                {
                    return BadRequest(_response.BadRequestResponse("Diet plan does not exist"));
                }

                await _dietPlanRepository.AddFoodToDietPLanAsync(foodId, dietPlanId, weight);

                return Ok(_response.OkResponse("Food added to diet plan"));
            }
            catch (Exception ex)
            {
                return BadRequest(_response.BadRequestResponse(ex.Message));
            }
        }

        [HttpDelete("RemoveFoodFromDietPlan", Name = "RemoveFoodFromDietPlan")]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> RemoveFoodFromDietPlan(int foodId, int dietPlanId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (!await _dietPlanRepository.DoesExistAsync(d => d.Id == dietPlanId))
                {
                    return BadRequest(_response.BadRequestResponse("Diet plan does not exist"));
                }
                if (!await _foodRepository.DoesExistAsync(f => f.Id == foodId))
                {
                    return BadRequest(_response.BadRequestResponse("Food does not exist"));
                }

                await _dietPlanRepository.RemoveFoodFromDietPlanAsync(foodId, dietPlanId);

                return Ok(_response.OkResponse("Food removed from diet plan"));
            }
            catch (Exception ex)
            {
                return BadRequest(_response.BadRequestResponse(ex.Message));
            }
        }

        [HttpDelete("DeleteDietPlan", Name = "DeleteDietPlan")]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> DeleteDietPlan(int dietPlanId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var dietPlan = await _dietPlanRepository.GetAsync(d=> d.Id == dietPlanId);

                if (dietPlan == null)
                {
                    return BadRequest(_response.BadRequestResponse("Diet plan does not exist"));
                }

                await _dietPlanRepository.DeleteAsync(dietPlan);

                return Ok(_response.OkResponse("Diet plan deleted"));
            }
            catch (Exception ex)
            {
                return BadRequest(_response.BadRequestResponse(ex.Message));
            }
        }

        [HttpPut("UpdateDietPlanName", Name = "UpdateDietPlanName")]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> UpdateDietPlanName(int dietPlanId, string newName)
        {
            try
            {
                var dietPlan = await _dietPlanRepository.GetAsync(d => d.Id == dietPlanId);

                if (dietPlan == null)
                {
                    return BadRequest(_response.BadRequestResponse("Diet plan does not exist"));
                }


                if (dietPlan.Name == newName)
                {
                    return BadRequest(_response.BadRequestResponse("New name is the same as the old name"));
                }

                dietPlan.Name = newName;

                await _dietPlanRepository.UpdateDietPlanAsync(dietPlan);

                return Ok(_response.OkResponse("Diet plan name updated"));
            }
            catch (Exception ex)
            {
                return BadRequest(_response.BadRequestResponse(ex.Message));
            }
        }

    }
}
