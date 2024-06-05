using System.Net;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.Text.Json;
using Core.Interfaces;
using Api.DTO;
using Core.Models;
using Api.Helper;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodController : ControllerBase
    {
        private readonly IFoodRepository _foodRepository;
        private readonly IMapper _mapper;
        protected ApiResponse _response;

        public FoodController(IFoodRepository foodRepository, IMapper mapper, ApiResponse response)
        {
            _foodRepository = foodRepository;
            _mapper = mapper;
            _response = response;
        }

        [HttpGet("GetAllFoods", Name = "GetAllFoods")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ResponseCache(Duration = 10)]
        public async Task<ActionResult<ApiResponse>> GetAllFoods(string? search, string? order, int pageSize = 10, int pageNumber = 1)
        {
            try
            {
                IEnumerable<Food> foodList;

                foodList = await _foodRepository.GetAllFoodsAsync(search, order);
                var foodListToReturn = _mapper.Map<List<FoodDto>>(foodList);
                var PaginatedFoodsResponse = Pagination<FoodDto>.Paginate(foodListToReturn, pageNumber, pageSize);

                return Ok(_response.OkResponse(PaginatedFoodsResponse));
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;

        }

        //[HttpGet("SearchForFood", Name = "SearchForFood")]
        //[ProducesResponseType(StatusCodes.Status403Forbidden)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ResponseCache(Duration = 30)]
        //public async Task<ActionResult<ApiResponse>> SearchForFood(string name, int pageSize = 0, int pageNumber = 1)
        //{
        //    try
        //    {
        //        IEnumerable<Food> foodList;

        //        foodList = await _foodRepository.SearchAsync(name,pageSize: pageSize, pageNumber: pageNumber);

        //        //Pagination pagination = new() { PageNumber = pageNumber, PageSize = pageSize };
        //        //Response.Headers["X-Pagination"] = JsonSerializer.Serialize(pagination);

        //        _response.Result = _mapper.Map<List<FoodDto>>(foodList);
        //        _response.StatusCode = HttpStatusCode.OK;
        //        return Ok(_response);
        //    }
        //    catch (Exception ex)
        //    {
        //        _response.StatusCode=HttpStatusCode.BadRequest;
        //        _response.IsSuccess = false;
        //        _response.ErrorMessages
        //             = new List<string>() { ex.ToString() };
        //    }
        //    return _response;

        //}


        [HttpGet("GetFood {id:int}", Name = "GetFood")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ResponseCache(Duration = 10)]
        public async Task<ActionResult<ApiResponse>> GetFood(int id)
        {
            if (id == 0)
            {
                return BadRequest(_response.BadRequestResponse("Id is required"));
            }

            var food = await _foodRepository.GetAsync(G => G.Id == id);

            if (food == null)
            {
                return NotFound(_response.NotFoundResponse("No food found"));
            }
            var result = _mapper.Map<FoodDto>(food);
            return Ok(_response.OkResponse(result));
        }

        [HttpPost("CreateFood", Name = "CreateFood")]
        //[Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]

        public async Task<ActionResult<ApiResponse>> CreateFood([FromBody] FoodDto foodDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(_response.BadRequestResponse(""));
            }
            if (foodDto == null)
            {
                return BadRequest(_response.BadRequestResponse("Food is required"));
            }
            if (await _foodRepository.GetAsync(u => u.Name.ToLower() == foodDto.Name.ToLower()) != null)
            {
                return BadRequest(_response.BadRequestResponse("Food already exists"));
            }
            var food = _mapper.Map<Food>(foodDto);


            await _foodRepository.CreateAsync(food);

            var result = _mapper.Map<FoodDto>(food);

            return Ok(_response.OkResponse(result));
        }

        [HttpDelete("DeleteFood {id:int}", Name = "DeleteFood")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<ActionResult<ApiResponse>> DeleteFood(int id)
        {
            var food = await _foodRepository.GetAsync(u => u.Id == id);
            if (food == null)
            {
                return NotFound(_response.NotFoundResponse("No food found"));
            }
            await _foodRepository.DeleteAsync(food);

            return Ok(_response.OkResponse("Food deleted"));
        }

        [HttpDelete("DeleteAllFood", Name = "DeleteAllFood")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]

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
        public async Task<IActionResult> UpdateFood(int id, [FromBody] FoodDto foodDto)
        {
            var b = await _foodRepository.DoesExistAsync(V => V.Id == id);
            if (!b)
            {
                return NotFound(_response.NotFoundResponse("No food found"));
            }

            if (foodDto == null || id != foodDto.Id)
            {
                return BadRequest(_response.BadRequestResponse("Food is required"));
            }
            var food = _mapper.Map<Food>(foodDto);

            await _foodRepository.UpdateAsync(food);

            return Ok(_response.OkResponse(food));


        }
    }
}
