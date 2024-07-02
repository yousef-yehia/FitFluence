//using System.Net;
//using System.Text.Json;
//using Api.DTO;
//using AutoMapper;
//using Core.Interfaces;
//using Core.Models;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Api.ApiResponses;
//using Infrastructure.Repository;
//using Microsoft.AspNetCore.Identity;
//using System.Security.Claims;
//using Api.Extensions;
//using Api.Helper;

//namespace Api.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class GoalController : ControllerBase
//    {
//        private readonly IGoalRepository _goalRepository;
//        private readonly UserManager<AppUser> _userManager;
//        private readonly IMapper _mapper;
//        protected ApiResponse _response;

//        public GoalController(IGoalRepository goalRepository, IMapper mapper, ApiResponse response, UserManager<AppUser> userManager)
//        {
//            _goalRepository = goalRepository;
//            _mapper = mapper;
//            _response = response;
//            _userManager = userManager;
//        }

//        [HttpGet("GetAllGoals", Name ="GetAllGoals")]
//        [ResponseCache(Duration = 10)]
//        public async Task<ActionResult<ApiResponse>> GetGoals()
//        {
//            try
//            {
//                IList<Goal> GoalList;

//                GoalList = await _goalRepository.GetAllAsync();

//                return Ok(_response.OkResponse(GoalList));
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(_response.BadRequestResponse(ex.Message));
//            }
//        }


//        [HttpGet("GetGoal {id:int}", Name = "GetGoal")]
//        [ResponseCache(Duration = 10)]
//        public async Task<ActionResult<ApiResponse>> GetGoal(int id)
//        {
//            if (id == 0)
//            {
//                return BadRequest(_response.BadRequestResponse("Id is required"));
//            }

//            var goal = await _goalRepository.GetAsync(G => G.Id == id);

//            if (goal == null)
//            {
//                return NotFound(_response.NotFoundResponse("Goal not found"));
//            }
//            return Ok(_response.OkResponse(_mapper.Map<GoalDto>(goal)));
//        }

//        [HttpPost("CreateGoal", Name ="CreateGoal")]
//        [Authorize(Roles = "admin")]
//        public async Task<ActionResult<ApiResponse>> CreateGoal([FromBody] CreateGoalDto createGoalDto)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(_response.BadRequestResponse(""));
//            }
//            if (createGoalDto == null)
//            {
//                return BadRequest(_response.BadRequestResponse("Invalid request body"));
//            }
//            if (await _goalRepository.GetAsync(u => u.Name.ToLower() == createGoalDto.Name.ToLower()) != null)
//            {
//                return BadRequest(_response.BadRequestResponse("Goal already exists"));
//            }

//            Goal goal = _mapper.Map<Goal>(createGoalDto);


//            await _goalRepository.CreateAsync(goal);

//            return Ok(_response.OkResponse(_mapper.Map<GoalDto>(goal)));
//        }

//        [HttpDelete("DeleteGoal {id:int}", Name = "DeleteGoal")]
//        [Authorize(Roles = "admin")]
//        public async Task<ActionResult<ApiResponse>> DeleteGoal(int id)
//        {
//            var goal = await _goalRepository.GetAsync(u => u.Id == id);
//            if (goal == null)
//            {
//                return NotFound(_response.NotFoundResponse("Goal not found"));  
//            }
//            await _goalRepository.DeleteAsync(goal);

//            return Ok(_response.OkResponse("Goal deleted successfully"));   
//        }

//        [HttpPut("UpdateGoal {id:int}", Name = "UpdateGoal")]
//        [Authorize(Roles = "admin")]
//        public async Task<IActionResult> UpdateGoal(int id, [FromBody] GoalDto goalDto)
//        {
//            try {
//                var b = await _goalRepository.DoesExistAsync(V => V.Id == id);
//                if (!b)
//                {
//                    return NotFound(_response.NotFoundResponse("Goal not found"));
//                }

//                if (goalDto == null || id != goalDto.Id)
//                {
//                    return BadRequest(_response.BadRequestResponse("Invalid request body"));
//                }

//                Goal goal = _mapper.Map<Goal>(goalDto);

//                await _goalRepository.UpdateAsync(goal);

//                return Ok(_response.OkResponse("Goal updated successfully"));
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(_response.BadRequestResponse(ex.Message));
//            }
//        }

//        [HttpPost("AddGoals", Name = "AddGoals")]
//        [Authorize]
//        public async Task<ActionResult<ApiResponse>> AddGoalsToUser([FromBody] List<int> goalIds)
//        {
//            try
//            {
//                var user = await _userManager.FindByEmailFromClaimsPrincipal(User);

//                if (user == null)
//                {
//                    return NotFound(_response.NotFoundResponse("user not found"));
//                }

//                // Check and initialize the UserGoals collection if it's null
//                if (user.UserGoals == null)
//                {
//                    user.UserGoals = new List<UserGoals>();
//                }


//                await _goalRepository.AddGoalToUserAsync(user, goalIds);
//                return Ok(_response.OkResponse("Goals added"));
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(_response.BadRequestResponse(ex.Message));
//            }
//        }

//        [HttpGet("GetAllUserGoals", Name = "GetAllUserGoals")]
//        [ResponseCache(Duration = 10)]
//        [Authorize]

//        public async Task<ActionResult<ApiResponse>> GetAllUserGoals(int pageSize = 0, int pageNumber = 1)
//        {
//            try
//            {
//                var user = await _userManager.FindByEmailFromClaimsPrincipal(User);

//                var AppUserGoalsList = await _goalRepository.GetAllUserGoalsAsync(user);

//                return Ok(_response.OkResponse(AppUserGoalsList));
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(_response.BadRequestResponse(ex.Message));
//            }
//        }



//        [HttpDelete("DeleteUserGoal", Name = "DeleteUserGoal")]
//        [Authorize]

//        public async Task<ActionResult<ApiResponse>> DeleteUserGoal(int goalId)
//        {
//            var user = await _userManager.FindByEmailFromClaimsPrincipal(User);

//            var goal = await _goalRepository.GetAsync(u => u.Id == goalId);
   
//            if (goal == null)
//            {
//                return NotFound(_response.NotFoundResponse("the goal id is wrong"));
//            }
//            try
//            {
//                await _goalRepository.DeleteUserGoalAsync(user, goal);
//                return Ok(_response.OkResponse("Deleted"));
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(_response.BadRequestResponse(ex.Message));
//            }
//        }


//    }
//}
