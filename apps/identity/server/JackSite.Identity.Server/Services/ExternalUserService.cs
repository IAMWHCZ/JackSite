using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using JackSite.Identity.Server.Interfaces;
using JackSite.Identity.Server.Models;

namespace JackSite.Identity.Server.Services
{
    
    
    public class ExternalUserService : IExternalUserService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ExternalUserService> _logger;
        
        public ExternalUserService(
            HttpClient httpClient,
            IConfiguration configuration,
            ILogger<ExternalUserService> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
            
            // Configure HttpClient
            _httpClient.BaseAddress = new Uri(_configuration["ExternalUserApi:BaseUrl"]);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Add("ApiKey", _configuration["ExternalUserApi:ApiKey"]);
            _httpClient.Timeout = TimeSpan.FromSeconds(_configuration.GetValue<int>("ExternalUserApi:Timeout"));
        }
        
        public async Task<ExternalUserInfo> GetUserByIdAsync(string externalUserId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/users/{externalUserId}");
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<ExternalUserInfo>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
                
                _logger.LogWarning("Failed to get external user by ID: {ExternalUserId}. Status: {StatusCode}", 
                    externalUserId, response.StatusCode);
                return null!;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting external user by ID: {ExternalUserId}", externalUserId);
                return null!;
            }
        }
        
        public async Task<ExternalUserInfo> GetUserByUsernameAsync(string username)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/users/by-username/{username}");
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<ExternalUserInfo>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
                
                _logger.LogWarning("Failed to get external user by username: {Username}. Status: {StatusCode}", 
                    username, response.StatusCode);
                return null!;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting external user by username: {Username}", username);
                return null!;
            }
        }
        
        public async Task<ExternalUserInfo> GetUserByEmailAsync(string email)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/users/by-email/{email}");
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<ExternalUserInfo>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
                
                _logger.LogWarning("Failed to get external user by email: {Email}. Status: {StatusCode}", 
                    email, response.StatusCode);
                return null!;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting external user by email: {Email}", email);
                return null!;
            }
        }
        
        public async Task<bool> ValidateUserCredentialsAsync(string username, string password)
        {
            try
            {
                var loginRequest = new
                {
                    Username = username,
                    Password = password
                };
                
                var content = new StringContent(
                    JsonSerializer.Serialize(loginRequest),
                    Encoding.UTF8,
                    "application/json");
                    
                var response = await _httpClient.PostAsync("api/auth/validate", content);
                
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating external user credentials for username: {Username}", username);
                return false;
            }
        }
    }
    
    
}