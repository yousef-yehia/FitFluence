using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Infrastructure.Services
{
    public class FitbitTokenService : IFitbitTokenService
    {
        private readonly string _filePath;
        private readonly ILogger<FitbitTokenService> _logger;

        public FitbitTokenService(IConfiguration configuration, ILogger<FitbitTokenService> logger)
        {
            _filePath = "../wwwroot/wwwroot/FitbitToken.json";
            _logger = logger;
        }

        public async Task StoreTokenAsync(string token)
        {
            try
            {
                var json = JsonConvert.SerializeObject(token, Formatting.Indented);
                await File.WriteAllTextAsync(_filePath, json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to store token.");
                throw;
            }
        }

        public async Task<string> RetrieveTokenAsync()
        {
            try
            {
                if (!File.Exists(_filePath))
                {
                    return null;
                }

                var json = await File.ReadAllTextAsync(_filePath);
                return JsonConvert.DeserializeObject<string>(json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve token.");
                throw;
            }
        }
    }
}
