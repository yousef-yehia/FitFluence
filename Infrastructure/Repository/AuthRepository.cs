using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Web;
using AutoMapper;
using Core.Interfaces;
using Core.Models;
using Core.UtilityModels;
using Infrastructure;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace FitFluence.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;

        public AuthRepository(UserManager<AppUser> userManager, IEmailService emailService, IHttpClientFactory httpClientFactory, SignInManager<AppUser> signInManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _emailService = emailService;
            _httpClientFactory = httpClientFactory;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        public async Task<bool> SendVerificationEmailAsync(AppUser appUser)
        {
            try
            {
                var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);
                var verificationLink = $"https://localhost:7125/api/auth/verify?userId={(appUser.Id)}&token={HttpUtility.UrlEncode(confirmationToken)}";
                var emailContent = $"Please click <a href='{verificationLink}'>here</a> to verify your email address.";
                await _emailService.SendAsync("vicious@gmail.com", appUser.Email, "Please confirm your email", emailContent);

                return true;

            }
            catch
            {
                return false;
            }

        }
        public async Task<string> SendResetPasswordEmailAsync(AppUser appUser)
        {

            var confirmationToken = await _userManager.GeneratePasswordResetTokenAsync(appUser);
            var verificationLink = $"https://localhost:7125/api/auth/changepassword?userId={(appUser.Id)}&token={HttpUtility.UrlEncode(confirmationToken)}";
            var emailContent = $"Please click <a href='{verificationLink}'>here</a> to change your password.";
            await _emailService.SendAsync("vicious@gmail.com", appUser.Email, "Reset your password", emailContent);

            return confirmationToken;

        }


        public async Task<string> FacebookLogin(string accessToken)
        {
            var httpClient = _httpClientFactory.CreateClient();

            var facebookRequestUrl = $"https://graph.facebook.com/me?fields=id,email,name&access_token={accessToken}";

                var response = await httpClient.GetAsync(facebookRequestUrl);

                if (response.IsSuccessStatusCode)
                {
                    var userInfo = await response.Content.ReadAsStringAsync();
                    var facebookUser = JsonConvert.DeserializeObject<FacebookUser>(userInfo);


                    var user = await _userManager.FindByEmailAsync(facebookUser.Email);
                    if (user == null)
                    {
                        // User doesn't exist, create a new user account
                        user = new AppUser { UserName = facebookUser.Email, Email = facebookUser.Email };
                        var result = await _userManager.CreateAsync(user);
                        if (!result.Succeeded)
                        {
                            // Handle user creation failure
                        }
                    }

                    await _signInManager.SignInAsync(user, isPersistent: false);

                    var token = _tokenService.CreateToken(user);
                    return token;
                }
            else
            {
                return "";
            }

        }
    }
}
