using System.Net;
using System.Text.Json;
using Api.DTO;
using AutoMapper;
using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Api.ApiResponses;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoalController : ControllerBase
    {
        private readonly IGoalRepository _goalRepository;
        private readonly IMapper _mapper;
        protected ApiResponse _response;

        public GoalController(IGoalRepository goalRepository, IMapper mapper, ApiResponse response)
        {
            _goalRepository = goalRepository;
            _mapper = mapper;
            _response = response;
        }

        [HttpGet("GetAllGoals", Name ="GetAllGoals")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ResponseCache(Duration = 10)]
        public async Task<ActionResult<ApiResponse>> GetGoals(int pageSize = 10, int pageNumber = 1)
        {
            try
            {
                IEnumerable<Goal> GoalList;

                GoalList = await _goalRepository.GetAllAsync(pageSize: pageSize, pageNumber: pageNumber);
                //Pagination pagination = new() { PageNumber = pageNumber, PageSize = pageSize };

                //Response.Headers["X-Pagination"] = JsonSerializer.Serialize(pagination);
                var result = _mapper.Map<List<GoalDto>>(GoalList);

                return Ok(_response.OkResponse(result));
            }
            catch (Exception ex)
            {
                return BadRequest(_response.BadRequestResponse(ex.Message));
            }
        }


        [HttpGet("GetGoal {id:int}", Name = "GetGoal")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ResponseCache(Duration = 10)]
        public async Task<ActionResult<ApiResponse>> GetGoal(int id)
        {
            if (id == 0)
            {
                return BadRequest(_response.BadRequestResponse("Id is required"));
            }

            var goal = await _goalRepository.GetAsync(G => G.Id == id);

            if (goal == null)
            {
                return NotFound(_response.NotFoundResponse("Goal not found"));
            }
            return Ok(_response.OkResponse(_mapper.Map<GoalDto>(goal)));
        }

        [HttpPost("CreateGoal", Name ="CreateGoal")]
        //[Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]

        public async Task<ActionResult<ApiResponse>> CreateGoal([FromBody] CreateGoalDto createGoalDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(_response.BadRequestResponse(""));
            }
            if (createGoalDto == null)
            {
                return BadRequest(_response.BadRequestResponse("Invalid request body"));
            }
            if (await _goalRepository.GetAsync(u => u.Name.ToLower() == createGoalDto.Name.ToLower()) != null)
            {
                return BadRequest(_response.BadRequestResponse("Goal already exists"));
            }

            Goal goal = _mapper.Map<Goal>(createGoalDto);


            await _goalRepository.CreateAsync(goal);

            return Ok(_response.OkResponse(_mapper.Map<GoalDto>(goal)));
        }

        [HttpDelete("DeleteGoal {id:int}", Name = "DeleteGoal")]
        //[Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<ActionResult<ApiResponse>> DeleteGoal(int id)
        {
            var goal = await _goalRepository.GetAsync(u => u.Id == id);
            if (goal == null)
            {
                return NotFound(_response.NotFoundResponse("Goal not found"));  
            }
            await _goalRepository.DeleteAsync(goal);

            return Ok(_response.OkResponse("Goal deleted successfully"));   
        }

        [HttpPut("UpdateGoal {id:int}", Name = "UpdateGoal")]
        //[Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateGoal(int id, [FromBody] GoalDto goalDto)
        {
            try {
                var b = await _goalRepository.DoesExistAsync(V => V.Id == id);
                if (!b)
                {
                    return NotFound(_response.NotFoundResponse("Goal not found"));
                }

                if (goalDto == null || id != goalDto.Id)
                {
                    return BadRequest(_response.BadRequestResponse("Invalid request body"));
                }

                Goal goal = _mapper.Map<Goal>(goalDto);

                await _goalRepository.UpdateAsync(goal);

                return Ok(_response.OkResponse("Goal updated successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(_response.BadRequestResponse(ex.Message));
            }


            
        }

    }
}
