using System.Net;
using Api.DTO;
using Api.Extensions;
using Application.Foods.GetAll;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Core.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Application.FavouriteFoods.GetAllFavouriteFoodsByUserId;
using Application.FavouriteFoods.AddFavouriteFood;
using Application.Exceptions;
using System.Security.Claims;
using Application.FavouriteFoods.RemoveFavouriteFood;
using Infrastructure.Repository;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavouriteFoodController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        protected ApiResponse _response;
        private readonly ISender _sender;

        public FavouriteFoodController(ApiResponse response, ISender sender, UserManager<AppUser> userManager)
        {
            _response = response;
            _sender = sender;
            _userManager = userManager;
        }

        [HttpGet("GetAllUserFoods", Name = "GetAllUserFoods")]
        [ResponseCache(Duration = 20)]
        [Authorize]

        public async Task<ActionResult<ApiResponse>> GetAllUserFoods(int pageSize = 0, int pageNumber = 1)
        {
            try
            {
                var user = await _userManager.FindByEmailFromClaimsPrincipal(User);
                var command = new GetAllFavouriteFoodsByUserIdQuery(user.Id, pageSize, pageNumber);
                var result = await _sender.Send(command);

                return Ok(_response.OkResponse(result));
            }
            catch (Exception ex)
            {
                return BadRequest(_response.BadRequestResponse(ex.Message));
            }
        }

        [HttpPost("AddFavouriteFood", Name = "AddFavouriteFood")]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> AddFavouriteFood(int foodId)
        {
            try
            {
                var user = await _userManager.FindByEmailFromClaimsPrincipal(User);

                if (user.UserFoods == null)
                {
                    user.UserFoods = new List<UserFoods>();
                }

                var command = new AddFavouriteFoodCommand(user, foodId);
                var result = _sender.Send(command);


                return Ok(_response.OkResponse("Favourite Food Added"));

            }
            catch (NotFoundExeption ex)
            {
                return BadRequest(_response.BadRequestResponse(ex.Message));

            }
            catch (Exception ex)
            {
                return BadRequest(_response.BadRequestResponse(ex.Message));
            }
        }

        [HttpDelete("DeleteUserFavouriteFood", Name = "DeleteUserFavouriteFood")]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> DeleteUserFavouriteFood(int foadId)
        {
            try
            {
                var user = await _userManager.FindByEmailFromClaimsPrincipal(User);

                if (user.UserFoods == null)
                {
                    user.UserFoods = new List<UserFoods>();
                }

                var command = new RemoveFavouriteFoodCommand(user,foadId);
                var result = _sender.Send(command);
                return Ok(_response.OkResponse("Food is deleted from favourite food"));

            }
            catch (NotFoundExeption ex)
            {
                return BadRequest(_response.BadRequestResponse(ex.Message));

            }
            catch (Exception ex)
            {
                return BadRequest(_response.BadRequestResponse(ex.Message));
            }
        }
    }
}
