using Api.ApiResponses;
using Api.Extensions;
using Api.Helper;
using Core.Interfaces;
using Core.Models;
using CsvHelper.Configuration.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkoutHistoryController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IWorkoutHistoryRepository _workoutHistoryRepository;
        private readonly IExerciseRepository _exerciseRepository;
        private readonly ApiResponse _response;
        public WorkoutHistoryController(IWorkoutHistoryRepository workoutHistoryRepository, ApiResponse response, UserManager<AppUser> userManager, IExerciseRepository exerciseRepository)
        {
            _workoutHistoryRepository = workoutHistoryRepository;
            _response = response;
            _userManager = userManager;
            _exerciseRepository = exerciseRepository;
        }

        [HttpGet("GetAllWorkoutHistories", Name = "GetAllWorkoutHistories")]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> GetAllWorkoutHistories()
        {
            var appUser = await _userManager.FindByEmailFromClaimsPrincipal(User);
            var workoutHistories = await _workoutHistoryRepository.GetAllWorkoutHistoriesAsync(appUser.Id);
            var result = CustomMappers.MapWorkoutHistoryToWorkoutHistoryReturnDto(workoutHistories);
            return Ok(_response.OkResponse(result));
        }
        
        [HttpPost("AddExerciseToWorkoutHistory", Name = "AddExerciseToWorkoutHistory")]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> AddExerciseToWorkoutHistory(int exerciseId, int numberOfReps, double weight)
        {
            try
            {
                var appUser = await _userManager.FindByEmailFromClaimsPrincipalWithWorkoutHistories(User);

                WorkoutHistory workoutHistory = _workoutHistoryRepository.GetWorkHistoriesByDate(appUser, DateTime.UtcNow.Date);

                if (workoutHistory == null)
                {
                    workoutHistory = await _workoutHistoryRepository.CreateWorkoutHistoryAsync(appUser.Id);
                }
                var exercise = await _exerciseRepository.GetExerciseByIdAsync(exerciseId);

                await _workoutHistoryRepository.AddExerciseToWorkoutHisterAsync(workoutHistory, exercise, numberOfReps, weight);

                return Ok(_response.OkResponse("added exercise to workout history"));
            }
            catch(Exception ex)
            {
                return BadRequest(_response.BadRequestResponse(ex.Message));
            }

        }
    }
}
