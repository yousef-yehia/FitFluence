using Api.ApiResponses;
using Api.Extensions;
using Api.Helper;
using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
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

                WorkoutHistory workoutHistory =await _workoutHistoryRepository.GetWorkoutHistoryByDateAsync(appUser.Id, DateTime.UtcNow.Date);

                var exercise = await _exerciseRepository.GetExerciseByIdAsync(exerciseId);

                if (workoutHistory == null)
                {
                    if(_workoutHistoryRepository.GetWorkHistoriesCount(appUser) >= 10)
                    {
                        await _workoutHistoryRepository.DeleteWorkoutHistoryAsync(appUser.Id);
                    }

                    workoutHistory = await _workoutHistoryRepository.CreateWorkoutHistoryAsync(appUser.Id);
                }

                if(_workoutHistoryRepository.IsExerciseInWorkoutHistory(exercise, workoutHistory))
                {
                    return BadRequest(_response.BadRequestResponse("Exercise already in workout history"));
                }

                await _workoutHistoryRepository.AddExerciseToWorkoutHisterAsync(workoutHistory, exercise, numberOfReps, weight);

                return Ok(_response.OkResponse("added exercise to workout history"));
            }
            catch(Exception ex)
            {
                return BadRequest(_response.BadRequestResponse(ex.Message));
            }
        }
        
        [HttpDelete("RemoveExerciseFromWorkoutHistory", Name = "RemoveExerciseFromWorkoutHistory")]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> RemoveExerciseFromWorkoutHistory(int exerciseId)
        {
            try
            {
                var appUser = await _userManager.FindByEmailFromClaimsPrincipal(User);

                WorkoutHistory workoutHistory = await _workoutHistoryRepository.GetWorkoutHistoryByDateAsync(appUser.Id, DateTime.UtcNow.Date);

                var exercise = await _exerciseRepository.GetExerciseByIdAsync(exerciseId);

                if (! _workoutHistoryRepository.IsExerciseInWorkoutHistory(exercise, workoutHistory))
                {
                    return BadRequest(_response.BadRequestResponse("Exercise is not in workout history"));
                }

                await _workoutHistoryRepository.RemoveExerciseFromWorkoutHistoryAsync(workoutHistory, exercise);

                return Ok(_response.OkResponse("exercise is removed from workout history"));
            }
            catch(Exception ex)
            {
                return BadRequest(_response.BadRequestResponse(ex.Message));
            }
        }
    }
}
