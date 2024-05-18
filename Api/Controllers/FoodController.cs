using System.Net;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.Text.Json;
using Core.Interfaces;
using Application.DTO;
using Core.Models;
using Api.Helper;
using Microsoft.AspNetCore.Authorization;
using Application.Exceptions;
using Application.Foods.Create;
using MediatR;
using Api.DTO;
using Application.DTO.FoodDto;
using Application.Foods.GetAll;
using Application.Foods.GetById;
using Application.Foods.Delete;
using Application.Foods.Update;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodController : ControllerBase
    {
        //private readonly IFoodRepository _foodRepository;
        //private readonly IMapper _mapper;
        protected ApiResponse _response;
        private readonly ISender _sender;


        public FoodController(ApiResponse response, ISender sender)
        {

            _response = response;
            _sender = sender;
        }

        [HttpGet("GetAllFoods", Name = "GetAllFoods")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ResponseCache(Duration = 10)]
        public async Task<ActionResult<ApiResponse>> GetAllFoods(string? search, string? order, int pageSize = 0, int pageNumber = 1)
        {
            try
            {
                var command = new GetAllFoodQuery(search, order, pageSize , pageNumber );
                var result = await _sender.Send(command);

                return Ok(_response.OkResponse(result));
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

                var command = new GetFoodByIdQuery(id);
                var result = await _sender.Send(command);
                return Ok(_response.OkResponse(result));

            }
            catch (NotFoundExeption ex)
            {
                return NotFound(_response.NotFoundResponse(ex.Message));
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

                var command = new CreateFoodCommand(createFoodDto);
                var result = await _sender.Send(command);

                return Ok(_response.OkResponse(result));
            }
            catch(AlreadyExistsException ex)
            {
                return BadRequest(_response.BadRequestResponse(ex.Message));
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
                var command = new DeleteFoodCommand(id);
                await _sender.Send(command);
                return Ok(_response.OkResponse("Food Deleted Success"));
            }
            catch (NotFoundExeption ex)
            {
                return NotFound(_response.NotFoundResponse(ex.Message));    
            }
            catch (Exception ex) 
            {
                return BadRequest(_response.BadRequestResponse(ex.Message));
            }
        }

        //        [HttpDelete("DeleteAllFood", Name = "DeleteAllFood")]
        //        [Authorize(Roles = "admin")]
        //        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        //        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //        [ProducesResponseType(StatusCodes.Status200OK)]

        //        public async Task<ActionResult<ApiResponse>> DeleteAllFood()
        //        {

        //            await _foodRepository.DeleteAllFoods();
        //            _response.StatusCode = HttpStatusCode.NoContent;
        //            _response.IsSuccess = true;
        //            return Ok(_response);
        //        }

        [HttpPut("UpdateFood {id:int}", Name = "UpdateFood")]
        //[Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateFood(int id, [FromBody] CreateFoodDto foodDto)
        {
            try
            {
                var command = new UpdateFoodCommand(id, foodDto);
                var food = await _sender.Send(command);
                return Ok(_response.OkResponse(food));
            }
            catch(NotFoundExeption ex)
            {
                return NotFound(_response.NotFoundResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(_response.BadRequestResponse(ex.Message));
            }
        }
    }
}
