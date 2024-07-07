using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services
{
    public class FoodRecommendationService : IFoodRecommendationService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;

        public FoodRecommendationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<int>> GetRecommendationsAsync(int foodId)
        {
            string url = $"https://fitfluence.pythonanywhere.com/recommend?food_id={foodId}";
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<int>>(url);
                return response ?? new List<int>();
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}");
                return new List<int>();
            }
        }
    }
}
