using Api.Extensions;
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

        public UserDailyFoodsController(IUserDailyFoodsRepository userFoodService, UserManager<AppUser> userManager, IFoodRepository foodRepository)
        {
            _userDailyFoodsRepository = userFoodService;
            _userManager = userManager;
            _foodRepository = foodRepository;
        }

        [HttpPost("addFood")]
        [Authorize]
        public async Task<IActionResult> AddFoodSelection(int id)
        {
            var user = await _userManager.FindByEmailFromClaimsPrincipal(User);
            var food = await _foodRepository.GetAsync(f=> f.Id == id);
            _userDailyFoodsRepository.AddFoodSelection(user.Id, food);
            return Ok();
        }

        [HttpGet("GetUserDailyFoods")]
        [Authorize]
        public async Task<IActionResult> GetFoodSelections()
        {
            var user = await _userManager.FindByEmailFromClaimsPrincipal(User);

            var foods = _userDailyFoodsRepository.GetFoodSelections(user.Id);
            return Ok(foods);
        }

        [HttpGet("GetUserDailyFoodsCalories")]
        [Authorize]
        public async Task<IActionResult> GetTotalCalories()
        {
            var user = await _userManager.FindByEmailFromClaimsPrincipal(User);

            var totalCalories = _userDailyFoodsRepository.GetTotalCalories(user.Id);
            return Ok(totalCalories);
        }
    }
}
