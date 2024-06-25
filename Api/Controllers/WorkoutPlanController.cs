using Api.ApiResponses;
using Api.DTO.WorkoutPlanDto;
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
    public class WorkoutPlanController : ControllerBase
    {
        private readonly IWorkoutPlanRepository _workoutPlanRepository;
        private readonly IExerciseRepository _exerciseRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ApiResponse _response;

        public WorkoutPlanController(IWorkoutPlanRepository workoutPlanRepository, IMapper mapper, ApiResponse apiResponse, UserManager<AppUser> userManager, IExerciseRepository exerciseRepository)
        {
            _workoutPlanRepository = workoutPlanRepository;
            _mapper = mapper;
            _response = apiResponse;
            _userManager = userManager;
            _exerciseRepository = exerciseRepository;
        }

        [HttpGet("GetAllWorkoutPlans")]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> GetAllWorkoutPlans(string? search = null, string ? sort = null, int pageSize = 0, int pageNumber = 1) 
        {
            try
            {
                var user = await _userManager.FindByEmailFromClaimsPrincipal(User);
                var workOutplans = await _workoutPlanRepository.GetAllWorkoutPlansWithExercisesAsync(user);

                var workoutPlansReturn = CustomMappers.MapWorkoutplanToWorkoutPLanReturnDto(workOutplans);

                return Ok(_response.OkResponse(workoutPlansReturn));
            }
            catch (Exception ex)
            {
                return BadRequest(_response.BadRequestResponse(ex.Message));
            }

        }

        [HttpPost("AddExerciseToWorkoutPlan")]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> AddExerciseToWorkoutPlan(int workoutPlanId, string exerciseName, int numberOfReps, double weight) 
        {
            try
            {
                var user = await _userManager.FindByEmailFromClaimsPrincipal(User);

                if(! await _workoutPlanRepository.DoesExistAsync(w=> w.Id == workoutPlanId)) 
                {
                    return NotFound(_response.NotFoundResponse("workout plan name is wrong"));
                }
                var exercise = await _exerciseRepository.GetExerciseByNameAsync(exerciseName.ToLower());

                if (exercise == null)
                {
                    return NotFound(_response.NotFoundResponse("exercise name is wrong"));
                }

                await _workoutPlanRepository.AddExerciseToWorkoutPLanAsync(workoutPlanId, exercise, numberOfReps, weight);
                return Ok(_response.OkResponse("added"));
            }
            catch (Exception ex)
            {
                return BadRequest(_response.BadRequestResponse(ex.Message));
            }

        }

        [HttpPost("CreateWorkoutPlan")]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> CreateWorkoutPlan(CreateWorkoutPlanDto createWorkoutPlanDto)
        {
            try
            {
                if( await _workoutPlanRepository.DoesExistAsync(w=> w.Name.ToLower() == createWorkoutPlanDto.Name.ToLower()))
                {
                    return BadRequest(_response.BadRequestResponse("The workout plane name is already exists"));
                }

                var user = await _userManager.FindByEmailFromClaimsPrincipal(User);
                WorkoutPlan workoutPlan = new WorkoutPlan
                {
                    Name = createWorkoutPlanDto.Name,
                    AppUser = user,
                    AppUserId = user.Id,
                    DateAdedd = DateTime.UtcNow,
                };
                await _workoutPlanRepository.CreateAsync(workoutPlan);

                return Ok(_response.OkResponse($"{workoutPlan.Name} success"));
            }
            catch(Exception ex) 
            {
                return BadRequest(_response.BadRequestResponse(ex.Message));
            }
        }

        [HttpDelete("DeleteWorkoutPlan")]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> DeleteWorkoutPlan(int id)
        {
            try
            {
                var workoutPlan = await _workoutPlanRepository.GetAsync(w => w.Id == id);
                if(workoutPlan == null)
                {
                    return NotFound(_response.NotFoundResponse("id is wrong"));
                }

                await _workoutPlanRepository.DeleteAsync(workoutPlan);

                return Ok(_response.OkResponse($"{workoutPlan.Name} deleted"));
            }
            catch(Exception ex) 
            {
                return BadRequest(_response.BadRequestResponse(ex.Message));
            }
        }
        
        [HttpDelete("DeleteExerciseFromWorkoutPlan")]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> DeleteExerciseFromWorkoutPlan(int workoutPlanId, int exerciseId)
        {
            try
            {
                if(! await _workoutPlanRepository.DoesExistAsync(w=> w.Id == workoutPlanId))
                {
                    return NotFound(_response.NotFoundResponse("the workoutId is wrong"));
                }
                if (!await _exerciseRepository.DeesExerciseExistsAsync(exerciseId))
                {
                    return NotFound(_response.NotFoundResponse("the exercise id is wrong"));
                }


                await _workoutPlanRepository.DeleteExerciseFromWorkoutPlanAsync(workoutPlanId, exerciseId);

                return Ok(_response.OkResponse(" exercise deleted"));
            }
            catch(Exception ex) 
            {
                return BadRequest(_response.BadRequestResponse(ex.Message));
            }
        }


    }
}
