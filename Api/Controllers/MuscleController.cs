using Api.ApiResponses;
using Api.DTO.ExerciseDto;
using Api.DTO.MuscleDto;
using AutoMapper;
using Core.Interfaces;
using Core.Models;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MuscleController : ControllerBase
    {
        private readonly IMuscleRepository _muscleRepository;
        private readonly ApiResponse _response;
        private readonly IMapper _mapper;

        public MuscleController(IMuscleRepository muscleRepository, ApiResponse response, IMapper mapper)
        {
            _muscleRepository = muscleRepository;
            _response = response;
            _mapper = mapper;
        }


        [HttpGet("GetAllMusclesWithExercises", Name = "GetAllMusclesWithExercises")]
        [ResponseCache(Duration = 10)]
        public async Task<ActionResult<ApiResponse>> GetAllMusclesWithExercises()
        {
            var muscles = await _muscleRepository.GetAllAsync(includeProperties: "Exercises");
            return Ok(_response.OkResponse(muscles));
        }

        [HttpGet("GetAllMuscles", Name = "GetAllMuscles")]
        [ResponseCache(Duration = 10)]
        public async Task<ActionResult<ApiResponse>> GetAllMuscles()
        {
            var muscles = await _muscleRepository.GetAllAsync();
            var musclesToReturn = _mapper.Map<List<MuscleReturnDto>>(muscles);
            return Ok(_response.OkResponse(musclesToReturn));
        }

        [HttpPost("CreateMuscle", Name = "CreateMuscle")]
        [Authorize(Roles = "admin")]

        public async Task<ActionResult<ApiResponse>> CreateMuscle(MuscleCreateDto model)
        {
            try
            {
                var muscle = _mapper.Map<Muscle>(model);

                var muscelCreated = await _muscleRepository.CreateMuscleAsync(muscle, model.Image);

                var response = _mapper.Map<MuscleReturnDto>(muscelCreated);
                return Ok(_response.OkResponse(response));

            }
            catch (Exception ex)
            {
                return BadRequest(_response.BadRequestResponse(ex.Message));
            }
        }

        [HttpDelete("DeleteMuscle", Name = "DeleteMuscle")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<ApiResponse>> DeleteMuscle(int id)
        {
            try
            {
                if (!await _muscleRepository.DoesExistAsync(m=> m.Id == id))
                {
                    return NotFound(_response.NotFoundResponse("muscle not found"));
                }

                var muscle = await _muscleRepository.GetAsync(m=> m.Id == id);
                await _muscleRepository.DeleteAsync(muscle);

                return Ok(_response.OkResponse("muscle Deleted Success"));
            }
            catch (Exception ex)
            {
                return BadRequest(_response.BadRequestResponse(ex.Message));
            }
        }
    }
}
