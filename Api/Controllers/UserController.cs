using System.Linq;
using Api.ApiResponses;
using Api.DTO.AuthDto;
using Api.DTO.ClientDto;
using Api.DTO.CoachDto;
using Api.Extensions;
using Api.Helper;
using AutoMapper;
using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly ApiResponse _response;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;
        public UserController(UserManager<AppUser> userManager, IUserRepository userRepository, ApiResponse response, IMapper mapper, IPhotoService photoService)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _response = response;
            _mapper = mapper;
            _photoService = photoService;
        }


        [HttpGet("GetAllUsers", Name = "GetAllUsers")]
        [ResponseCache(Duration = 10)]
        [Authorize]
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


        [HttpGet("GetAllClients", Name = "GetAllClients")]
        [ResponseCache(Duration = 10)]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> GetAllClients(int pageSize = 0, int pageNumber = 1)
        {
            try
            {
                var user = await _userManager.FindByEmailFromClaimsPrincipal(User);

                var appUserList = await _userManager.GetUsersInRoleAsync(Role.roleClient);
                appUserList.Remove(user);
                var clients = _mapper.Map<List<ClientReturnDto>>(appUserList.ToList());
                var paginatedClients = Pagination<ClientReturnDto>.Paginate(clients, pageNumber, pageSize);

                return Ok(_response.OkResponse(paginatedClients));
            }
            catch (Exception ex)
            {
                return BadRequest(_response.BadRequestResponse(ex.Message));
            }
        }

        [HttpPost("UpdateProfilePhoto", Name = "UpdateProfilePhoto")]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> UpdateProfilePhoto(IFormFile file)
        {
            var user = await _userManager.FindByEmailFromClaimsPrincipal(User);

            if (file == null || file.Length == 0)
            {
                return BadRequest(_response.BadRequestResponse("the file is null or empty"));
            }

            var fileExtension = Path.GetExtension(file.FileName)?.ToLowerInvariant();

            if (fileExtension != ".jpg" && fileExtension != ".png" && fileExtension != ".jpeg")
            {
                return BadRequest( _response.BadRequestResponse("Only JPG, PNG and jpeg files are allowed."));
            }

            var result = await _photoService.AddProfilePhotoAsync(file);

            user.ImageUrl = result.Url.ToString();
            await _userManager.UpdateAsync(user);
           
            return Ok(_response.OkResponse(result.Url.ToString()));
        }

        [HttpPut("UpdateUser", Name = "UpdateUser")]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> UpdateUser(UpdateUserDto model)
        {
            try
            {
                var user = await _userManager.FindByEmailFromClaimsPrincipal(User);

                if (model.ImageUrl != null) user.ImageUrl = model.ImageUrl;
                if (model.Age != null) user.Age = model.Age.Value;
                if (model.Name != null) user.Name = model.Name;
                if (model.Height != null) user.Height = model.Height.Value;
                if (model.Weight != null) user.Weight = model.Weight.Value;
                if (model.FatWeight != null) user.FatWeight = model.FatWeight.Value;
                if (model.MuscleWeight != null) user.MuscleWeight = model.MuscleWeight.Value;
                if (model.MainGoal != null) user.MainGoal = model.MainGoal;
                if (model.ActivityLevel != null) user.ActivityLevelName = model.ActivityLevel;
                if (model.GoalWeight != null) user.GoalWeight = model.GoalWeight.Value;

                if (model.ActivityLevel != null || model.MainGoal != null || model.Weight != null || model.Height != null || model.Age != null){ user.RecommendedCalories = _userRepository.CalculateRecommendedCalories(user); }

                await _userManager.UpdateAsync(user);

                return Ok(_response.OkResponse("User Updated Successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(_response.BadRequestResponse(ex.Message));
            }

        }

        [HttpGet("GetUser", Name = "GetUser")]
        [ResponseCache(Duration = 10)]
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
        [Authorize(Roles = "admin")]
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

        [HttpDelete("DeleteMyAccount", Name = "DeleteMyAccount")]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> DeleteMyAccount()
        {
            var user = await _userManager.FindByEmailFromClaimsPrincipal(User);
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

    }
}
