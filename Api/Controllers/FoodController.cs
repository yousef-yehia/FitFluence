using System.Net;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Core.Interfaces;
using Core.Models;
using Api.Helper;
using Api.ApiResponses;
using Api.DTO.FoodDto;


namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodController : ControllerBase
    {
        private readonly IFoodRepository _foodRepository;
        private readonly IMapper _mapper;
        protected ApiResponse _response;


        public FoodController(ApiResponse response, IMapper mapper, IFoodRepository foodRepository)
        {

            _response = response;
            _mapper = mapper;
            _foodRepository = foodRepository;
        }

        [HttpGet("GetAllFoods", Name = "GetAllFoods")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ResponseCache(Duration = 10)]
        public async Task<ActionResult<ApiResponse>> GetAllFoods(string? search = null, string? order = null, int pageSize = 0, int pageNumber = 1)
        {
            try
            {
                var foods = await _foodRepository.GetAllAsync(search, order);
                var foodsResponse = _mapper.Map<List<FoodDto>>(foods);
                var paginatedFoods = Pagination<FoodDto>.Paginate(foodsResponse, pageNumber, pageSize);

                return Ok(_response.OkResponse(paginatedFoods));
            }
            catch (Exception ex)
            {
                return BadRequest(_response.BadRequestResponse(ex.Message));
            }
        }



        [HttpGet("GetFood {id:int}", Name = "GetFood")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ResponseCache(Duration = 10)]
        public async Task<ActionResult<ApiResponse>> GetFood(int id)
        {
            try 
            {
                if (id == 0)
                {
                    return BadRequest(_response.BadRequestResponse("Id is required"));
                }

                var food = await _foodRepository.GetAsync(f=> f.Id == id);

                if (food == null)
                {
                    return NotFound(_response.NotFoundResponse("Food not found"));
                }
                return Ok(_response.OkResponse(_mapper.Map<FoodDto>(food)));

            }
            catch (Exception ex) 
            {
                return BadRequest(_response.BadRequestResponse(ex.Message));
            }

        }

        [HttpPost("CreateFood", Name = "CreateFood")]
        //[Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]

        public async Task<ActionResult<ApiResponse>> CreateFood([FromBody] CreateFoodDto createFoodDto)
        {
            try 
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(_response.BadRequestResponse(""));
                }
                if (createFoodDto == null)
                {
                    return BadRequest(_response.BadRequestResponse("Food is required"));
                }
                if( await _foodRepository.DoesExistAsync(f=> f.Name == createFoodDto.Name))
                {
                    return BadRequest(_response.BadRequestResponse("Food already exists"));
                }

                await _foodRepository.CreateAsync(_mapper.Map<Food>(createFoodDto));    

                return Ok(_response.OkResponse("Food Created Success"));
            }
            catch (Exception ex) 
            {
                return BadRequest(_response.BadRequestResponse(ex.Message));
            }

        }

        [HttpDelete("DeleteFood {id:int}", Name = "DeleteFood")]
        //[Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<ActionResult<ApiResponse>> DeleteFood(int id)
        {
            try
            {
                if (!await _foodRepository.DoesExistAsync(f => f.Id == id))
                {
                    return NotFound(_response.NotFoundResponse("Food not found"));
                }

                var food = await _foodRepository.GetAsync(f => f.Id == id);
                await _foodRepository.DeleteAsync(food);

                return Ok(_response.OkResponse("Food Deleted Success"));
            }
            catch (Exception ex)
            {
                return BadRequest(_response.BadRequestResponse(ex.Message));
            }
        }

        [HttpDelete("DeleteAllFood", Name = "DeleteAllFood")]
        //[Authorize(Roles = "admin")]
        public async Task<ActionResult<ApiResponse>> DeleteAllFood()
        {

            await _foodRepository.DeleteAllFoods();
            _response.StatusCode = HttpStatusCode.NoContent;
            _response.IsSuccess = true;
            return Ok(_response);
        }

        [HttpPut("UpdateFood {id:int}", Name = "UpdateFood")]
        //[Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateFood(int id, [FromBody] CreateFoodDto foodDto)
        {
            try
            {
                if(! await _foodRepository.DoesExistAsync(f => f.Id == id))
                {
                    return NotFound(_response.NotFoundResponse("Food not found"));
                }

                await _foodRepository.UpdateAsync(_mapper.Map<Food>(foodDto));

                return Ok(_response.OkResponse("Food Updated Success"));

            }
            catch (Exception ex)
            {
                return BadRequest(_response.BadRequestResponse(ex.Message));
            }
        }
    }
}
