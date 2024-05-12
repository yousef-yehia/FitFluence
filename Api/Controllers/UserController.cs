using System.Net;
using System.Security.Claims;
using System.Text.Json;
using Api.DTO;
using Api.Extensions;
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
        private readonly IGoalRepository _goalRepository;
        private readonly IFoodRepository _foodRepository;
        private readonly ApiResponse _response;
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;
        public UserController(UserManager<AppUser> userManager, IUserRepository userRepository, ApiResponse response, AppDbContext appDbContext, IMapper mapper, IFoodRepository foodRepository, IGoalRepository goalRepository, IPhotoService photoService)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _response = response;
            _appDbContext = appDbContext;
            _mapper = mapper;
            _foodRepository = foodRepository;
            _goalRepository = goalRepository;
            _photoService = photoService;
        }


        [HttpGet("GetAllUsers", Name = "GetAllUsers")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ResponseCache(Duration = 10)]
        public async Task<ActionResult<ApiResponse>> GetAllUsers(int pageSize = 0, int pageNumber = 1)
        {
            try
            {
                IEnumerable<AppUser> AppUserList;

                AppUserList = await _userRepository.GetAllUsersAsync(pageSize: pageSize, pageNumber: pageNumber);
                //AppUserList = await _userRepository.GetAllUsersAsync(pageSize: pageSize, pageNumber: pageNumber, ordering: (u=> u.Name) );
                //Pagination pagination = new() { PageNumber = pageNumber, PageSize = pageSize };

                //Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagination));
                _response.Result = AppUserList;
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
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                return BadRequest(_response);
            }

            var user = await _userRepository.GetAsync( V => V.Id == id);

            if (user == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                return NotFound(_response);
            }
            _response.Result = user;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }



        [HttpDelete("DeleteUser", Name = "DeleteUser")]
        public async Task<ActionResult<ApiResponse>> DeleteUser(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Username is incorrect");
                return _response;
            }
            try
            {
                await _userRepository.DeleteUserAsync(user);
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = "Deleted";
                return (_response);
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add(ex.Message);
                return (_response);
            }
        }

        [HttpPost("AddGoals", Name = "AddGoals")]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> AddGoalsToUser([FromBody] List<int> goalIds)
        {
            var userIdClaim = (HttpContext.User.Identity as ClaimsIdentity)?.Claims.FirstOrDefault(c => c.Type == "uid");
            var userId = userIdClaim.Value;

            var user = await _userRepository.GetAsync(u => u.Id == userId);

            if (user == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("User id is incorrect");
                return _response;
            }

            // Check and initialize the UserGoals collection if it's null
            if (user.UserGoals == null)
            {
                user.UserGoals = new List<UserGoals>();
            }

            try
            {
                await _userRepository.AddGoalToUserAsync(user, goalIds);
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = "Goals added to user successfully";
                return (_response);
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add(ex.Message);
                return (_response);
            }
        }

        [HttpPost("AddFavouriteFood", Name = "AddFavouriteFood")]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> AddFavouriteFood(int foodId)
        {
            var user = await _userManager.FindByEmailFromClaimsPrincipal(User);

            if (user == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("User id is incorrect");
                return _response;
            }

            // Check and initialize the UserGoals collection if it's null
            if (user.UserFoods == null)
            {
                user.UserFoods = new List<UserFoods>();
            }

            try
            {
                await _userRepository.AddFavouriteFoodAsync(user, foodId);
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = "Favourite food added to user successfully";
                return (_response);
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add(ex.Message);
                return (_response);
            }
        }

        [HttpGet("GetAllUserGoals", Name = "GetAllUserGoals")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ResponseCache(Duration = 30)]
        [Authorize]

        public async Task<ActionResult<ApiResponse>> GetAllUserGoals(int pageSize = 0, int pageNumber = 1)
        {
            try
            {
                var userIdClaim = (HttpContext.User.Identity as ClaimsIdentity)?.Claims.FirstOrDefault(c => c.Type == "uid");
                var userId = userIdClaim.Value;

                var user = await _userRepository.GetAsync(u => u.Id == userId);

                var AppUserList = await _userRepository.GetAllUserGoalsAsync(user, pageSize: pageSize, pageNumber: pageNumber);
                //Pagination pagination = new() { PageNumber = pageNumber, PageSize = pageSize };

                //Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagination));
                _response.Result = AppUserList;
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

        [HttpGet("GetAllUserFoods", Name = "GetAllUserFoods")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ResponseCache(Duration = 60)]
        [Authorize]

        public async Task<ActionResult<ApiResponse>> GetAllUserFoods(int pageSize = 0, int pageNumber = 1)
        {
            try
            {
                var user = await _userManager.FindByEmailFromClaimsPrincipal(User);


                var AppUserList = await _userRepository.GetAllFavouriteFoodsAsync(user, pageSize: pageSize, pageNumber: pageNumber);
                //Pagination pagination = new() { PageNumber = pageNumber, PageSize = pageSize };

                //Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagination));
                _response.Result = AppUserList;
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

        [HttpDelete("DeleteUserGoal", Name = "DeleteUserGoal")]
        [Authorize]

        public async Task<ActionResult<ApiResponse>> DeleteUserGoal(int goalId)
        {
            var userIdClaim = (HttpContext.User.Identity as ClaimsIdentity)?.Claims.FirstOrDefault(c => c.Type == "uid");
            var userId = userIdClaim.Value;

            var user = await _userRepository.GetAsync(u => u.Id == userId);
            var goal = await _goalRepository.GetAsync(u => u.Id == goalId);

            if (user == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("User ID is incorrect");
                return _response;
            }
            if (goal == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Goal ID is incorrect");
                return _response;
            }
            try
            {
                await _userRepository.DeleteUserGoalAsync(user, goal);
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = "Deleted";
                return (_response);
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add(ex.Message);
                return (_response);
            }
        }

        [HttpDelete("DeleteUserFavouriteFood", Name = "DeleteUserFavouriteFood")]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> DeleteUserFavouriteFood(int foadId)
        {
            var userIdClaim = (HttpContext.User.Identity as ClaimsIdentity)?.Claims.FirstOrDefault(c => c.Type == "uid");
            var userId = userIdClaim.Value;

            var user = await _userRepository.GetAsync(u => u.Id == userId);
            var food = await _foodRepository.GetAsync(u => u.Id == foadId);

            if (user == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("User ID is incorrect");
                return _response;
            }
            if (food == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Food ID is incorrect");
                return _response;
            }
            try
            {
                await _userRepository.DeleteFavouriteFoodAsync(user, food);
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = "Deleted";
                return (_response);
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add(ex.Message);
                return (_response);
            }
        }

        [HttpPut("UpdateClient {id}", Name = "UpdateClient")]
        //[Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]

        public async Task<IActionResult> UpdateClient(int clientid, [FromBody] UpdateClientDto updateClientDto)
        {
            var userIdClaim = (HttpContext.User.Identity as ClaimsIdentity)?.Claims.FirstOrDefault(c => c.Type == "uid");
            var userId = userIdClaim.Value;

            var flag = await _userRepository.DoesUserExists(userId);

            if (!flag)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Error this user doesnt exists");
                return BadRequest(_response);
            }


            if (updateClientDto == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Error this user doesnt exists");
                return BadRequest(_response);
            }

            var user = await _userRepository.GetAsync(u => u.Id == userId);
            AppUser newUser = user;
            newUser.Name = updateClientDto.Name;
            newUser.UserName = updateClientDto.UserName;
            newUser.Email = updateClientDto.Email;
            newUser.NormalizedEmail= updateClientDto.Email.ToUpper();


            var updatedClient = _mapper.Map<Client>(updateClientDto);
            updatedClient.AppUserId = userId;
            updatedClient.AppUser = newUser;
            updatedClient.ClientId = clientid;
            newUser.Client = updatedClient;

            //await _clientRepository.UpdateAsync(updatedClient);

            await _userRepository.UpdateAsync(newUser);
            _response.StatusCode = HttpStatusCode.NoContent;
            _response.IsSuccess = true;
            return Ok(_response);
        }

        [HttpPut("UpdateCoach {id}", Name = "UpdateCoach")]
        //[Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> UpdateCoach(int Coachid, [FromBody] UpdateClientDto updateCoachDto)
        {
            var userIdClaim = (HttpContext.User.Identity as ClaimsIdentity)?.Claims.FirstOrDefault(c => c.Type == "uid");
            var userId = userIdClaim.Value;

            var flag = await _userRepository.DoesUserExists(userId);

            if (!flag)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Error this user doesnt exists");
                return BadRequest(_response);
            }


            if (updateCoachDto == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Error this user doesnt exists");
                return BadRequest(_response);
            }

            var user = await _userRepository.GetAsync(u => u.Id == userId);
            AppUser newUser = user;
            newUser.Name = updateCoachDto.Name;
            newUser.UserName = updateCoachDto.UserName;


            var updatedCoach = _mapper.Map<Coach>(updateCoachDto);
            updatedCoach.AppUserId = userId;
            updatedCoach.AppUser = newUser;
            updatedCoach.CoachId = Coachid;
            newUser.Coach = updatedCoach;

            //await _clientRepository.UpdateAsync(updatedClient);

            await _userRepository.UpdateAsync(newUser);
            _response.StatusCode = HttpStatusCode.NoContent;
            _response.IsSuccess = true;
            return Ok(_response);
        }

    }
}
