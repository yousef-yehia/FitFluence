using System.Net;
using System.Security.Claims;
using Api.ApiResponses;
using Api.DTO.AuthDto;
using Api.Extensions;
using Api.Helper;
using AutoMapper;
using Core.Interfaces;
using Core.Models;
using FitFluence.Repository;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApiResponse _response;
        private readonly IAuthRepository _authService;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IPhotoService _photoService;

        public AuthController(
            UserManager<AppUser> userManager,
            IAuthRepository authService,
            SignInManager<AppUser> signInManager,
            IMapper mapper,
            ITokenService tokenService,
            IPhotoService photoService,
            IUserRepository userRepository)
        {
            _response = new ApiResponse();
            _userManager = userManager;
            _authService = authService;
            _signInManager = signInManager;
            _mapper = mapper;
            _tokenService = tokenService;
            _photoService = photoService;
            _userRepository = userRepository;
        }

        [Authorize]
        [HttpGet("getUser")]
        public async Task<ActionResult<ApiResponse>> GetCurrentUser()
        {
            var user = await _userManager.FindByEmailFromClaimsPrincipal(User);

            var userToReturn = new UserReturnDto
            {
                UserName = user.UserName,
                ImageUrl = user.ImageUrl,
                Email = user.Email,
                UserId = user.Id,
                Age = user.Age,
                Gender = user.Gender,
                Height = user.Height,
                Weight = user.Weight,
                ActivityLevel = user.ActivityLevelName,
                GoalWeight = user.GoalWeight,
                MainGoal = user.MainGoal,
                RecommendedCalories = user.RecommendedCalories,
                FatWeight = user.FatWeight,
                MuscleWeight = user.MuscleWeight,
                Role = _userManager.GetRolesAsync(user).Result.FirstOrDefault(),
                UserDisease = await _userRepository.GetUserDiseases(user.Id) ?? null,

            };

            var response = _response.OkResponse(userToReturn);
            return Ok(response);
        }

        [HttpPost("registerClient")]
        public async Task<ActionResult<ApiResponse>> RegisterClient(ClientRegisterRequestDto model)
        {
            try
            {
                if (CheckEmailExistsAsync(model.Email).Result.Value)
                {

                    return BadRequest(_response.BadRequestResponse("Email address is in use"));
                }
                
                if (CheckUserNameExistsAsync(model.UserName).Result.Value)
                {

                    return BadRequest(_response.BadRequestResponse("Username is in use"));
                }

                //var fileExtension = Path.GetExtension(model.Photo.FileName)?.ToLowerInvariant();

                //Check if the file extension is jpg or png
                //if (fileExtension != ".jpg" && fileExtension != ".png" && fileExtension != ".jpeg")
                //{
                //    return BadRequest(_response.BadRequestResponse("only jpg and png and jpeg allowed"));
                //}


                //var uploadResult = await _photoService.AddPhotoAsync(model.Photo);

                var newUser = new AppUser
                {
                    Name = model.Name,
                    Email = model.Email,
                    UserName = model.UserName,
                    PhoneNumber = model.PhoneNumber,
                    Weight = model.Weight,
                    FatWeight = model.FatWeight,
                    MuscleWeight = model.MuscleWeight,
                    Height = model.Height,
                    Age = model.Age,
                    Gender = model.Gender,
                    MainGoal = model.MainGoal,
                    ActivityLevelName = model.ActivityLevel,
                    GoalWeight = model.GoalWeight,
                };
                newUser.Client = new Client
                {
                    AppUser = newUser,
                    AppUserId = newUser.Id,
                }; 

                newUser.RecommendedCalories = _userRepository.CalculateRecommendedCalories(newUser);
                //newUser.ImageUrl = uploadResult.Url.ToString();

                var result = await _userManager.CreateAsync(newUser, model.Password);

                await _userManager.AddToRoleAsync(newUser, Role.roleClient);

                await _userRepository.AddUserDisease(newUser.Id, model.Diseases);

                if (!result.Succeeded) return BadRequest(_response.BadRequestResponse("error happend while register"));

                await _authService.SendVerificationEmailAsync(newUser);

                var userToReturn = new RegisterResponseDto
                {
                    UserName = model.UserName,
                    ImageUrl = newUser.ImageUrl,
                    Token = _tokenService.CreateToken(newUser)
                };

                var response = _response.OkResponse(userToReturn);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return (_response.BadRequestResponse(ex.Message));
            }
        }
        //[HttpPost("registerAdmin")]
        //public async Task<ActionResult<ApiResponse>> RegisterAdmin(ClientRegisterRequestDto model)
        //{
        //    try
        //    {
        //        if (CheckEmailExistsAsync(model.Email).Result.Value)
        //        {

        //            return BadRequest(_response.BadRequestResponse("Email address is in use"));
        //        }
                
        //        if (CheckUserNameExistsAsync(model.UserName).Result.Value)
        //        {

        //            return BadRequest(_response.BadRequestResponse("Username is in use"));
        //        }

        //        //var fileExtension = Path.GetExtension(model.Photo.FileName)?.ToLowerInvariant();

        //        //Check if the file extension is jpg or png
        //        //if (fileExtension != ".jpg" && fileExtension != ".png" && fileExtension != ".jpeg")
        //        //{
        //        //    return BadRequest(_response.BadRequestResponse("only jpg and png and jpeg allowed"));
        //        //}


        //        //var uploadResult = await _photoService.AddPhotoAsync(model.Photo);

        //        var newUser = new AppUser
        //        {
        //            Name = model.Name,
        //            Email = model.Email,
        //            UserName = model.UserName,
        //            PhoneNumber = model.PhoneNumber,
        //            Weight = model.Weight,
        //            FatWeight = model.FatWeight,
        //            MuscleWeight = model.MuscleWeight,
        //            Height = model.Height,
        //            Age = model.Age,
        //            Gender = model.Gender,
        //        };               
        //        //newUser.ImageUrl = uploadResult.Url.ToString();

        //        var result = await _userManager.CreateAsync(newUser, model.Password);
        //        await _userManager.AddToRoleAsync(newUser, Role.roleAdmin);

        //        if (!result.Succeeded) return BadRequest(_response.BadRequestResponse(""));

        //        await _authService.SendVerificationEmailAsync(newUser);

        //        var userToReturn = new RegisterResponseDto
        //        {
        //            UserName = model.UserName,
        //            ImageUrl = newUser.ImageUrl,
        //            Token = _tokenService.CreateToken(newUser)
        //        };

        //        var response = _response.OkResponse(userToReturn);
        //        return Ok(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        return (_response.BadRequestResponse(ex.Message));
        //    }
        //}

        [HttpPost("registerCoach")]
        public async Task<ActionResult<ApiResponse>> RegisterCoach(CoachRegisterRequestDto model)
        {
            try
            {
                if (CheckEmailExistsAsync(model.Email).Result.Value)
                {

                    return BadRequest(_response.BadRequestResponse("Email address is in use"));
                }

                if (CheckUserNameExistsAsync(model.UserName).Result.Value)
                {

                    return BadRequest(_response.BadRequestResponse("Username is in use"));
                }

                var newUser = new AppUser
                {
                    Name = model.Name,
                    Email = model.Email,
                    UserName = model.UserName,
                    PhoneNumber = model.PhoneNumber,
                    Weight = model.Weight ,
                    FatWeight = model.FatWeight,
                    MuscleWeight = model.MuscleWeight,
                    Height = model.Height,
                    Age = model.Age ?? 0,
                    Gender = model.Gender,
                    MainGoal = model.MainGoal,
                    ActivityLevelName = model.ActivityLevel,
                    GoalWeight = model.GoalWeight,
                };
                newUser.Coach = new Coach
                {
                    AppUser = newUser,
                    AppUserId = newUser.Id,
                };

                newUser.RecommendedCalories = _userRepository.CalculateRecommendedCalories(newUser);

                if (model.Cv != null)
                {
                    var fileExtension = Path.GetExtension(model.Cv.FileName)?.ToLowerInvariant();
                    if (fileExtension != ".pdf")
                    {
                        return BadRequest(_response.BadRequestResponse("only pdf allowed"));
                    }
                    var uploadResult = _photoService.UploadPdfAsync(model.Cv);
                    newUser.Coach.CvUrl = uploadResult.Result.Url.ToString();
                }

                var result = await _userManager.CreateAsync(newUser, model.Password);

                await _userManager.AddToRoleAsync(newUser, Role.roleCoach);

                await _userRepository.AddUserDisease(newUser.Id, model.Diseases);

                if (!result.Succeeded) return BadRequest(_response.BadRequestResponse("error happend while register"));

                var userToReturn = new RegisterResponseDto
                {
                    UserName = model.UserName,
                    ImageUrl = newUser.ImageUrl,
                    Token = _tokenService.CreateToken(newUser)
                };

                var response = _response.OkResponse(userToReturn);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return (_response.BadRequestResponse(ex.Message));
            }
        }


        [HttpGet("verify")]
        [Authorize]
        public async Task<IActionResult> VerifyEmail(string userId, string token)
        {
            // Find the user based on the verification token
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return BadRequest();
            }
            await _userManager.ConfirmEmailAsync(user, token);
            // Verify the user by updating the verification status

            return Ok("Email verified successfully.");
        }

        [HttpGet("ForgetPassword", Name = "ForgetPassword")]
        public async Task<ActionResult<ApiResponse>> ForgetPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return BadRequest(_response.BadRequestResponse("this email is wrong"));
            }
            var result = await _authService.SendResetPasswordEmailAsync(user);
            return Ok(_response.OkResponse(result));
        }

        [HttpPut("ResetPassword", Name = "ResetPassword")]
        public async Task<IActionResult> ResetPassword(ChangePasswordDto model)
        {
            AppUser user = await _userManager.FindByIdAsync(model.Id);
                
            if (user == null)
            {
                return BadRequest();
            }

            var result = await _userManager.ResetPasswordAsync(user, model.ChangePasswordTokken, model.NewPassword);

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse>> Login([FromBody] LogInRequestDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null) return Unauthorized(_response.UnauthorizedResponse("The Email is Wrong"));

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if (!result.Succeeded) return Unauthorized(_response.UnauthorizedResponse("The Password is Wrong"));

            var userToReturn = new LoginResponseDto
            {
                UserName = user.UserName,
                ImageUrl = user.ImageUrl,
                Email = user.Email,
                UserId = user.Id,
                Token = _tokenService.CreateToken(user),
                Age = user.Age,
                Gender = user.Gender,
                Height = user.Height,
                Weight = user.Weight,
                Role = _userManager.GetRolesAsync(user).Result.FirstOrDefault(),
                GoalWeight = user.GoalWeight,
                ActivityLevel = user.ActivityLevelName,
                MainGoal = user.MainGoal,
                RecommendedCalories = user.RecommendedCalories,
                UserDisease = await _userRepository.GetUserDiseases(user.Id),
            };

            var response = _response.OkResponse(userToReturn);
            return Ok(response);
        }

        //[HttpPost("FacebookLogin")]
        //public async Task<ActionResult<ApiResponse>> FacebookLogin(string accessToken)
        //{
        //    try
        //    {
        //        var result = await _authService.FacebookLogin(accessToken);

        //        if (result != "")
        //        {
        //            return BadRequest(result);
        //        }
        //        else
        //        {
        //            return Ok(_response.OkResponse(result));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(_response.BadRequestResponse(ex.Message));
        //    }

        //}


        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }
        [HttpGet("usernameexists")]
        public async Task<ActionResult<bool>> CheckUserNameExistsAsync([FromQuery] string userName)
        {
            return await _userManager.FindByNameAsync(userName) != null;
        }
    }



}
