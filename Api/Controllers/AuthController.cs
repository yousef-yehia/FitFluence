﻿using System.Net;
using System.Security.Claims;
using Api.ApiResponses;
using Api.Dto;
using Api.DTO;
using Api.Extensions;
using AutoMapper;
using Core.Interfaces;
using Core.Models;
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
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IPhotoService _photoService;

        public AuthController(
            UserManager<AppUser> userManager,
            IAuthRepository authService,
            SignInManager<AppUser> signInManager,
            IMapper mapper,
            ITokenService tokenService,
            IPhotoService photoService)
        {
            _response = new ApiResponse();
            _userManager = userManager;
            _authService = authService;
            _signInManager = signInManager;
            _mapper = mapper;
            _tokenService = tokenService;
            _photoService = photoService;
        }

        [Authorize]
        [HttpGet("getUser")]
        public async Task<ActionResult<ApiResponse>> GetCurrentUser()
        {
            var user = await _userManager.FindByEmailFromClaimsPrincipal(User);

            var userToReturn = new UserDto
            {
                Email = user.Email,
                Token = _tokenService.CreateToken(user),
                DisplayName = user.Name
            };

            var response = _response.OkResponse(userToReturn);
            return Ok(response);
        }
        [HttpGet]


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

                var newUser = _mapper.Map<AppUser>(model);
                //newUser.ImageUrl = uploadResult.Url.ToString();

                var result = await _userManager.CreateAsync(newUser, model.Password);
                await _userManager.AddToRoleAsync(newUser, Role.roleClient);

                if (!result.Succeeded) return BadRequest(_response.BadRequestResponse(""));

                await _authService.SendVerificationEmailAsync(newUser);

                var userToReturn = new UserDto
                {
                    Email = newUser.Email,
                    Token = _tokenService.CreateToken(newUser),
                    DisplayName = newUser.Name
                };

                var response = _response.OkResponse(userToReturn);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return (_response.BadRequestResponse(ex.Message));
            }
        }

        [HttpPost("registerCoach")]
        public async Task<ActionResult<ApiResponse>> RegisterCoach([FromBody] CoachRegisterRequestDto model)
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

                var newUser = _mapper.Map<AppUser>(model);


                var result = await _userManager.CreateAsync(newUser, model.Password);
                await _userManager.AddToRoleAsync(newUser, Role.roleCoach);

                if (!result.Succeeded) return BadRequest(_response.BadRequestResponse(""));
                var userToReturn = new UserDto
                {
                    Email = newUser.Email,
                    Token = _tokenService.CreateToken(newUser),
                    DisplayName = newUser.Name
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

            var userToReturn = new UserDto
            {
                Email = user.Email,
                Token = _tokenService.CreateToken(user),
                DisplayName = user.Name
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


        [HttpPost("addrole")]
        public async Task<IActionResult> AddRoleAsync([FromBody] AddRoleModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var user = await _userManager.FindByIdAsync(model.UserId);
            var result = await _userManager.AddToRoleAsync(user, model.Role);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<ActionResult<ApiResponse>> DeleteUser(string username)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(username);

                if (user == null)
                {
                    return BadRequest(_response.NotFoundResponse("no usr found"));
                }

                await _userManager.DeleteAsync(user);
                return Ok(_response.OkResponse("user deleted"));
            }
            catch (Exception ex) 
            {
                return BadRequest(_response.BadRequestResponse(ex.Message));
            }

        }

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
