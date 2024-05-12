using System.Net;
using System.Text.Json;
using Api.DTO;
using AutoMapper;
using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult<ApiResponse>> GetGoals(int pageSize = 0, int pageNumber = 1)
        {
            try
            {
                IEnumerable<Goal> GoalList;

                GoalList = await _goalRepository.GetAllAsync(pageSize: pageSize, pageNumber: pageNumber);
                //Pagination pagination = new() { PageNumber = pageNumber, PageSize = pageSize };

                //Response.Headers["X-Pagination"] = JsonSerializer.Serialize(pagination);
                //_response.Result = _mapper.Map<List<GoalDto>>(GoalList);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;

        }


        [HttpGet("GetGoal {id:int}", Name = "GetGoal")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ResponseCache(Duration = 60)]
        public async Task<ActionResult<ApiResponse>> GetGoal(int id)
        {
            if (id == 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                return BadRequest(_response);
            }

            var goal = await _goalRepository.GetAsync(G => G.Id == id);

            if (goal == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                return NotFound(_response);
            }
            _response.Result = _mapper.Map<GoalDto>(goal);
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
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
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Error");
                return BadRequest(_response);
            }
            if (createGoalDto == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Error No goal was given");
                return BadRequest(_response);
            }
            if (await _goalRepository.GetAsync(u => u.Name.ToLower() == createGoalDto.Name.ToLower()) != null)
            {
                //ModelState.AddModelError("CustomError", "Villa already Exists!");
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Goal already exists");
                return BadRequest(_response);
            }

            Goal goal = _mapper.Map<Goal>(createGoalDto);


            await _goalRepository.CreateAsync(goal);
            _response.Result = _mapper.Map<CreateGoalDto>(goal);
            _response.StatusCode = HttpStatusCode.Created;

            return CreatedAtRoute("GetGoal", new { id = goal.Id }, _response);
        }

        [HttpDelete("DeleteGoal {id:int}", Name = "DeleteGoal")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<ActionResult<ApiResponse>> DeleteGoal(int id)
        {
            var goal = await _goalRepository.GetAsync(u => u.Id == id);
            if (goal == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Error this goal doesnt exists");
                return BadRequest(_response);
            }
            await _goalRepository.DeleteAsync(goal);
            _response.StatusCode = HttpStatusCode.NoContent;
            _response.IsSuccess = true;
            return Ok(_response);
        }

        [HttpPut("UpdateGoal {id:int}", Name = "UpdateGoal")]
        //[Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateGoal(int id, [FromBody] GoalDto goalDto)
        {
            var b = await _goalRepository.DoesExistAsync(V => V.Id == id);
            if (!b)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Error this goal doesnt exists");
                return BadRequest(_response);
            }


            {
                if (goalDto == null || id != goalDto.Id)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Error this goal doesnt exists");
                    return BadRequest(_response);
                }

                Goal goal = _mapper.Map<Goal>(goalDto);

                await _goalRepository.UpdateAsync(goal);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);

            }
        }

    }
}
