using System.Net;
using System.Security.Claims;
using System.Text.Json;
using Api.ApiResponses;
using Api.DTO;
using Api.Extensions;
using Api.Helper;
using AutoMapper;
using Core.Interfaces;
using Core.Models;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly ApiResponse _response;
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;
        public UserController(UserManager<AppUser> userManager, IUserRepository userRepository, ApiResponse response, AppDbContext appDbContext, IMapper mapper, IPhotoService photoService)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _response = response;
            _appDbContext = appDbContext;
            _mapper = mapper;
            _photoService = photoService;
        }


        [HttpGet("GetAllUsers", Name = "GetAllUsers")]
        [ResponseCache(Duration = 10)]
        public async Task<ActionResult<ApiResponse>> GetAllUsers(int pageSize = 0, int pageNumber = 1)
        {
            try
            {
                var AppUserList = await _userRepository.GetAllUsersAsync();
                var paginatedUsers = Pagination<AppUser>.Paginate(AppUserList, pageNumber, pageSize);

                return Ok(_response.OkResponse(paginatedUsers));
            }
            catch (Exception ex)
            {
                return BadRequest(_response.BadRequestResponse(ex.Message));
            }
        }

        [HttpGet("GetAllCoaches", Name = "GetAllCoaches")]
        [ResponseCache(Duration = 10)]
        public async Task<ActionResult<ApiResponse>> GetAllCoaches(int pageSize = 0, int pageNumber = 1)
        {
            try
            {
                var AppUserList = await _userManager.GetUsersInRoleAsync(Role.roleCoach);
                var paginatedUsers = Pagination<AppUser>.Paginate(AppUserList.ToList(), pageNumber, pageSize);

                return Ok(_response.OkResponse(paginatedUsers));
            }
            catch (Exception ex)
            {
                return BadRequest(_response.BadRequestResponse(ex.Message));
            }
        }
        [HttpGet("GetAllClients", Name = "GetAllClients")]
        [ResponseCache(Duration = 10)]
        public async Task<ActionResult<ApiResponse>> GetAllClients(int pageSize = 0, int pageNumber = 1)
        {
            try
            {
                var AppUserList = await _userManager.GetUsersInRoleAsync(Role.roleClient);
                var paginatedUsers = Pagination<AppUser>.Paginate(AppUserList.ToList(), pageNumber, pageSize);

                return Ok(_response.OkResponse(paginatedUsers));
            }
            catch (Exception ex)
            {
                return BadRequest(_response.BadRequestResponse(ex.Message));
            }
        }

        [HttpPost("UploadPhoto", Name = "UploadPhoto")]
        public async Task<ActionResult<ApiResponse>> UploadPhoto(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                return BadRequest(_response);
            }

            // Get the file extension
            var fileExtension = Path.GetExtension(file.FileName)?.ToLowerInvariant();

            // Check if the file extension is jpg or png
            if (fileExtension != ".jpg" && fileExtension != ".png" && fileExtension != ".jpeg")
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                return BadRequest("Only JPG and PNG files are allowed.");
            }

            var result = await _photoService.AddPhotoAsync(file);

            _response.Result = result;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);

        }

        //[HttpGet("{photoName}")]
        //public async Task<ActionResult<ApiResponse>> GetPhoto(string photoName)
        //{
        //    var result = await _photoService.GetPhotoAsync(photoName);

        //    if (result == null)
        //    {
        //        _response.StatusCode = HttpStatusCode.NotFound;
        //        _response.IsSuccess = false;
        //        return NotFound(_response);
        //    }

        //    _response.Result = result;
        //    _response.StatusCode = HttpStatusCode.OK;
        //    return Ok(_response);
        //}

        [HttpGet("GetUser", Name = "GetUser")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<ApiResponse>> GetUser(string id)
        {
            if (id == null)
            {
                return BadRequest(_response.BadRequestResponse("The id is null"));
            }

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound(_response.NotFoundResponse("The user is not found"));
            }
            return Ok(_response.OkResponse(user));
        }



        [HttpDelete("DeleteUser", Name = "DeleteUser")]
        public async Task<ActionResult<ApiResponse>> DeleteUser(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return NotFound(_response.NotFoundResponse("username doesnt exist"));
            }
            try
            {
                await _userManager.DeleteAsync(user);
                return Ok(_response.OkResponse("Deleted"));
            }
            catch (Exception ex)
            {
                return BadRequest(_response.BadRequestResponse(ex.Message));
            }
        }


        [HttpPut("UpdateClient {id}", Name = "UpdateClient")]
        //[Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]

        public async Task<IActionResult> UpdateClient(int clientid, [FromBody] UpdateClientDto updateClientDto)
        {
            try
            {
                var user = await _userManager.FindByEmailFromClaimsPrincipal(User);


                if (updateClientDto == null)
                {
                    return BadRequest(_response.BadRequestResponse("Null"));

                }

                AppUser newUser = user;
                newUser.Name = updateClientDto.Name;
                newUser.UserName = updateClientDto.UserName;
                newUser.Email = updateClientDto.Email;
                newUser.NormalizedEmail = updateClientDto.Email.ToUpper();


                //var updatedClient = _mapper.Map<Client>(updateClientDto);
                //updatedClient.AppUserId = user.Id;
                //updatedClient.AppUser = newUser;
                //updatedClient.ClientId = clientid;
                //newUser.Client = updatedClient;

                //await _clientRepository.UpdateAsync(updatedClient);

                await _userRepository.UpdateAsync(newUser);
                return Ok(_response.OkResponse(newUser));
            }
            catch (Exception ex)
            {
                return BadRequest(_response.BadRequestResponse(ex.Message));
            }


        }

        [HttpPut("UpdateCoach {id}", Name = "UpdateCoach")]
        //[Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> UpdateCoach(int Coachid, [FromBody] UpdateClientDto updateCoachDto)
        {
            try
            {
                var user = await _userManager.FindByEmailFromClaimsPrincipal(User);

                if (updateCoachDto == null)
                {
                    return BadRequest(_response.BadRequestResponse("Null"));
                }

                AppUser newUser = user;
                newUser.Name = updateCoachDto.Name;
                newUser.UserName = updateCoachDto.UserName;


                //var updatedCoach = _mapper.Map<Coach>(updateCoachDto);
                //updatedCoach.AppUserId = user.Id;
                //updatedCoach.AppUser = newUser;
                //updatedCoach.CoachId = Coachid;
                //newUser.Coach = updatedCoach;

                //await _clientRepository.UpdateAsync(updatedClient);
                await _userRepository.UpdateAsync(newUser);
                return Ok(_response.OkResponse(newUser));
            }
            catch(Exception ex)
            {
                return BadRequest(_response.BadRequestResponse(ex.Message));
            }

        }

    }
}
