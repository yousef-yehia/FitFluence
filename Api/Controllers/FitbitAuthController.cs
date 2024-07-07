using Api.DTO;
using Core.Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FitbitAuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IFitbitTokenService _tokenStorageService;
        private readonly ILogger<FitbitAuthController> _logger;

        public FitbitAuthController(IConfiguration configuration, IFitbitTokenService tokenStorageService, ILogger<FitbitAuthController> logger)
        {
            _configuration = configuration;
            _tokenStorageService = tokenStorageService;
            _logger = logger;
        }

        [HttpGet("Callback")]
        public async Task<IActionResult> Callback(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return BadRequest("Authorization code is missing.");
            }

            var tokenResponse = await ExchangeAuthorizationCodeForToken(code);

            if (tokenResponse == null)
            {
                return BadRequest("Failed to exchange authorization code for token.");
            }

            await _tokenStorageService.StoreTokenAsync(tokenResponse.AccessToken);

            // Optionally, redirect the user back to the Flutter app with the access token
            return Ok("signed in");
        }

        private async Task<FitbitTokenResponse> ExchangeAuthorizationCodeForToken(string code)
        {
            using var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.fitbit.com/oauth2/token")
            {
                Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "client_id", _configuration["Fitbit:ClientId"] },
                { "grant_type", "authorization_code" },
                { "redirect_uri", _configuration["Fitbit:RedirectUri"] },
                { "code", code },
                { "client_secret", _configuration["Fitbit:ClientSecret"] }
            })
            };

            var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to exchange authorization code for token. Status code: {StatusCode}", response.StatusCode);
                return null;
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<FitbitTokenResponse>(responseContent);
        }
    }
}