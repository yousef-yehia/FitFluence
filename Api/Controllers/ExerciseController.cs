using Api.ApiResponses;
using Api.DTO.ExerciseDto;
using Api.DTO.FoodDto;
using Api.Helper;
using AutoMapper;
using Azure;
using Core.Interfaces;
using Core.Models;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExerciseController : ControllerBase
    {
        private readonly IExerciseRepository _exerciseRepository;
        private readonly IRepository<Muscle> _muscleRepository;
        private readonly ApiResponse _response;
        private readonly IMapper _mapper;

        public ExerciseController(IExerciseRepository exerciseRepository, IRepository<Muscle> muscleRepositoru, ApiResponse apiResponse, IMapper mapper)
        {
            _exerciseRepository = exerciseRepository;
            _muscleRepository = muscleRepositoru;
            _response = apiResponse;
            _mapper = mapper;
        }


        [HttpGet("GetAllExercises", Name = "GetAllExercises")]
        [ResponseCache(Duration = 10)]
        public async Task<ActionResult<ApiResponse>> GetAllExercises(string? search, int pageSize = 0, int pageNumber = 1)
        {
            try
            {
                var exercises = await _exerciseRepository.GetAllAsync(search);
                var exercisesResponse = _mapper.Map<List<ExerciseReturnDto>>(exercises);

                if(pageSize > 0)
                {
                    var paginatedExercises = Pagination<ExerciseReturnDto>.Paginate(exercisesResponse, pageNumber, pageSize);

                    return Ok(_response.OkResponse(paginatedExercises));
                }
                return Ok(_response.OkResponse(exercisesResponse));

            }
            catch (Exception ex)
            {
                return BadRequest(_response.BadRequestResponse(ex.Message));
            }
        }

        [HttpGet("GetAllExercisesByMuscle", Name = "GetAllExercisesByMuscle")]
        [ResponseCache(Duration = 10)]
        public async Task<ActionResult<ApiResponse>> GetAllExercisesByMuscle(int muscleId, string? search = null, int pageSize = 0, int pageNumber = 1)
        {
            try
            {
                if (!await _muscleRepository.DoesExistAsync(e=> e.Id == muscleId))
                {
                    return NotFound(_response.NotFoundResponse("Muscle ID is wrong"));
                }
                var exercises = await _exerciseRepository.GetAllByMuscleAsync(muscleId, search);
                var exercisesResponse = _mapper.Map<List<ExerciseReturnDto>>(exercises);

                if(pageSize > 0)
                {
                    var paginatedExercises = Pagination<ExerciseReturnDto>.Paginate(exercisesResponse, pageNumber, pageSize);

                    return Ok(_response.OkResponse(paginatedExercises));
                }
                return Ok(_response.OkResponse(exercisesResponse));

            }
            catch (Exception ex)
            {
                return BadRequest(_response.BadRequestResponse(ex.Message));
            }
        }


        [HttpGet("GetExercise", Name = "GetExercise")]
        [ResponseCache(Duration = 10)]
        public async Task<ActionResult<ApiResponse>> GetExercise(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest(_response.BadRequestResponse("Id is required"));
                }

                var exercise = await _exerciseRepository.GetExerciseByIdAsync(id);

                if (exercise == null)
                {
                    return NotFound(_response.NotFoundResponse("exercise not found"));
                }
                return Ok(_response.OkResponse(_mapper.Map<ExerciseReturnDto>(exercise)));

            }
            catch (Exception ex)
            {
                return BadRequest(_response.BadRequestResponse(ex.Message));
            }
        }

        [HttpPost("AddExercise", Name = "AddExercise")]
        [ResponseCache(Duration = 10)]
        public async Task<ActionResult<ApiResponse>> AddExercise(CreateExerciseDto model)
        {
            try
            {
                if (model.MuscleId == 0)
                {
                    return BadRequest(_response.BadRequestResponse("Muscle Id is required"));
                }

                if(! await _muscleRepository.DoesExistAsync(m=> m.Id == model.MuscleId))
                {
                    return BadRequest(_response.BadRequestResponse("Muscle Id is wrong"));
                }
                var exercise = _mapper.Map<Exercise>(model);
                await _exerciseRepository.CreateExerciseAsync(exercise);
                return Ok(_response.OkResponse("Created"));

            }
            catch (Exception ex)
            {
                return BadRequest(_response.BadRequestResponse(ex.Message));
            }
        }


        [HttpPut("UpdateExercise", Name = "UpdateExercise")]
        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateExercise(int id, [FromBody] CreateExerciseDto model)
        {
            try
            {
                if (!await _exerciseRepository.DeesExerciseExistsAsync(id))
                {
                    return NotFound(_response.NotFoundResponse("exercise not found"));
                }
                var exercise = _mapper.Map<Exercise>(model);
                exercise.Id = id;   

                await _exerciseRepository.UpdateAsync(exercise);

                return Ok(_response.OkResponse("exercise Updated Success"));

            }
            catch (Exception ex)
            {
                return BadRequest(_response.BadRequestResponse(ex.Message));
            }
        }


        [HttpDelete("DeleteExercise", Name = "DeleteExercise")]
        //[Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<ActionResult<ApiResponse>> DeleteExercise(int id)
        {
            try
            {
                if (!await _exerciseRepository.DeesExerciseExistsAsync(id))
                {
                    return NotFound(_response.NotFoundResponse("exercise not found"));
                }

                var exercise = await _exerciseRepository.GetExerciseByIdAsync(id);
                await _exerciseRepository.DeleteAsync(exercise);

                return Ok(_response.OkResponse("exercise Deleted Success"));
            }
            catch (Exception ex)
            {
                return BadRequest(_response.BadRequestResponse(ex.Message));
            }
        }
    }
}
