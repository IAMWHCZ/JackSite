using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using JackSite.Identity.Server.Enums;
using JackSite.Identity.Server.Interfaces;
using JackSite.Identity.Server.Models;
using Microsoft.AspNetCore.Identity;

namespace JackSite.Identity.Server.Services
{

    
    public class SocialLoginService : ISocialLoginService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<SocialLoginService> _logger;
        
        public SocialLoginService(
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory,
            ILogger<SocialLoginService> logger)
        {
            _userManager = userManager;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }
        
        public string GetAuthorizationUrl(string provider, string redirectUri, string state)
        {
            return provider.ToLower() switch
            {
                "google" => GetGoogleAuthorizationUrl(redirectUri, state),
                "microsoft" => GetMicrosoftAuthorizationUrl(redirectUri, state),
                "facebook" => GetFacebookAuthorizationUrl(redirectUri, state),
                "github" => GetGitHubAuthorizationUrl(redirectUri, state),
                _ => throw new ArgumentException($"Unsupported provider: {provider}")
            };
        }
        
        public async Task<ApplicationUser> ProcessExternalLoginAsync(SocialProvider provider, string code, string redirectUri)
        {
            var userInfo = provider switch
            {
                SocialProvider.Google => await GetGoogleUserInfoAsync(code, redirectUri),
                SocialProvider.Microsoft => await GetMicrosoftUserInfoAsync(code, redirectUri),
                SocialProvider.Facebook => await GetFacebookUserInfoAsync(code, redirectUri),
                SocialProvider.GitHub => await GetGitHubUserInfoAsync(code, redirectUri),
                _ => throw new ArgumentException($"Unsupported provider: {provider}")
            };
            
            if (userInfo == null)
            {
                return null;
            }
            
            // 查找或创建用户
            var user = await _userManager.FindByEmailAsync(userInfo.Email);
            
            if (user == null)
            {
                // 创建新用户
                user = new ApplicationUser
                {
                    UserName = userInfo.Email,
                    Email = userInfo.Email,
                    EmailConfirmed = true,
                    FirstName = userInfo.FirstName,
                    LastName = userInfo.LastName,
                    SecurityStamp = Guid.NewGuid().ToString()
                };
                
                var result = await _userManager.CreateAsync(user);
                
                if (!result.Succeeded)
                {
                    _logger.LogError("Failed to create user from social login: {Errors}", 
                        string.Join(", ", result.Errors.Select(e => e.Description)));
                    return null;
                }
            }
            
            // 添加或更新外部登录信息
            var externalLoginInfo = new UserLoginInfo(provider.ToString(), userInfo.Id.ToString(), provider.ToString());
            
            var existingLogins = await _userManager.GetLoginsAsync(user);
            var existingLogin = existingLogins.FirstOrDefault(l => l.LoginProvider == provider.ToString());
            
            if (existingLogin == null)
            {
                var addLoginResult = await _userManager.AddLoginAsync(user, externalLoginInfo);
                
                if (!addLoginResult.Succeeded)
                {
                    _logger.LogError("Failed to add external login for user: {Errors}", 
                        string.Join(", ", addLoginResult.Errors.Select(e => e.Description)));
                }
            }
            
            return user;
        }
        
        #region Google
        
        private string GetGoogleAuthorizationUrl(string redirectUri, string state)
        {
            var options = _configuration.GetSection("SocialLogin:Google").Get<GoogleOptions>();
            
            var queryParams = new Dictionary<string, string>
            {
                { "client_id", options.ClientId },
                { "redirect_uri", redirectUri },
                { "response_type", "code" },
                { "scope", "openid email profile" },
                { "state", state }
            };
            
            return $"https://accounts.google.com/o/oauth2/v2/auth?{BuildQueryString(queryParams)}";
        }
        
        private async Task<ExternalUserInfo> GetGoogleUserInfoAsync(string code, string redirectUri)
        {
            try
            {
                var options = _configuration.GetSection("SocialLogin:Google").Get<GoogleOptions>();
                var client = _httpClientFactory.CreateClient();
                
                // 获取访问令牌
                var tokenRequestParams = new Dictionary<string, string>
                {
                    { "code", code },
                    { "client_id", options.ClientId },
                    { "client_secret", options.ClientSecret },
                    { "redirect_uri", redirectUri },
                    { "grant_type", "authorization_code" }
                };
                
                var tokenResponse = await client.PostAsync(
                    "https://oauth2.googleapis.com/token",
                    new FormUrlEncodedContent(tokenRequestParams));
                
                if (!tokenResponse.IsSuccessStatusCode)
                {
                    _logger.LogError("Failed to get Google access token. Status: {StatusCode}", tokenResponse.StatusCode);
                    return null;
                }
                
                var tokenContent = await tokenResponse.Content.ReadAsStringAsync();
                var tokenData = JsonSerializer.Deserialize<JsonElement>(tokenContent);
                var accessToken = tokenData.GetProperty("access_token").GetString();
                
                // 获取用户信息
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var userInfoResponse = await client.GetAsync("https://www.googleapis.com/oauth2/v3/userinfo");
                
                if (!userInfoResponse.IsSuccessStatusCode)
                {
                    _logger.LogError("Failed to get Google user info. Status: {StatusCode}", userInfoResponse.StatusCode);
                    return null;
                }
                
                var userInfoContent = await userInfoResponse.Content.ReadAsStringAsync();
                var userInfo = JsonSerializer.Deserialize<JsonElement>(userInfoContent);
                
                return new ExternalUserInfo
                {
                    Id = Convert.ToInt64(userInfo.GetProperty("sub")),
                    Email = userInfo.GetProperty("email").GetString(),
                    FirstName = userInfo.TryGetProperty("given_name", out var firstName) ? firstName.GetString() : "",
                    LastName = userInfo.TryGetProperty("family_name", out var lastName) ? lastName.GetString() : "",
                    Provider = "Google"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing Google login");
                return null;
            }
        }
        
        #endregion
        
        #region Microsoft
        
        private string GetMicrosoftAuthorizationUrl(string redirectUri, string state)
        {
            var options = _configuration.GetSection("SocialLogin:Microsoft").Get<MicrosoftOptions>();
            
            var queryParams = new Dictionary<string, string>
            {
                { "client_id", options.ClientId },
                { "redirect_uri", redirectUri },
                { "response_type", "code" },
                { "scope", "openid email profile User.Read" },
                { "state", state },
                { "response_mode", "query" }
            };
            
            return $"https://login.microsoftonline.com/{options.TenantId}/oauth2/v2.0/authorize?{BuildQueryString(queryParams)}";
        }
        
        private async Task<ExternalUserInfo> GetMicrosoftUserInfoAsync(string code, string redirectUri)
        {
            try
            {
                var options = _configuration.GetSection("SocialLogin:Microsoft").Get<MicrosoftOptions>();
                var client = _httpClientFactory.CreateClient();
                
                // 获取访问令牌
                var tokenRequestParams = new Dictionary<string, string>
                {
                    { "code", code },
                    { "client_id", options.ClientId },
                    { "client_secret", options.ClientSecret },
                    { "redirect_uri", redirectUri },
                    { "grant_type", "authorization_code" }
                };
                
                var tokenResponse = await client.PostAsync(
                    $"https://login.microsoftonline.com/{options.TenantId}/oauth2/v2.0/token",
                    new FormUrlEncodedContent(tokenRequestParams));
                
                if (!tokenResponse.IsSuccessStatusCode)
                {
                    _logger.LogError("Failed to get Microsoft access token. Status: {StatusCode}", tokenResponse.StatusCode);
                    return null;
                }
                
                var tokenContent = await tokenResponse.Content.ReadAsStringAsync();
                var tokenData = JsonSerializer.Deserialize<JsonElement>(tokenContent);
                var accessToken = tokenData.GetProperty("access_token").GetString();
                
                // 获取用户信息
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var userInfoResponse = await client.GetAsync("https://graph.microsoft.com/v1.0/me");
                
                if (!userInfoResponse.IsSuccessStatusCode)
                {
                    _logger.LogError("Failed to get Microsoft user info. Status: {StatusCode}", userInfoResponse.StatusCode);
                    return null;
                }
                
                var userInfoContent = await userInfoResponse.Content.ReadAsStringAsync();
                var userInfo = JsonSerializer.Deserialize<JsonElement>(userInfoContent);
                
                return new ExternalUserInfo
                {
                    Id = Convert.ToInt64(userInfo.GetProperty("id")),
                    Email = userInfo.GetProperty("userPrincipalName").GetString(),
                    FirstName = userInfo.TryGetProperty("givenName", out var firstName) ? firstName.GetString() : "",
                    LastName = userInfo.TryGetProperty("surname", out var lastName) ? lastName.GetString() : "",
                    Provider = "Microsoft"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing Microsoft login");
                return null;
            }
        }
        
        #endregion
        
        #region Facebook
        
        private string GetFacebookAuthorizationUrl(string redirectUri, string state)
        {
            var options = _configuration.GetSection("SocialLogin:Facebook").Get<FacebookOptions>();
            
            var queryParams = new Dictionary<string, string>
            {
                { "client_id", options.AppId },
                { "redirect_uri", redirectUri },
                { "response_type", "code" },
                { "scope", "email public_profile" },
                { "state", state }
            };
            
            return $"https://www.facebook.com/v12.0/dialog/oauth?{BuildQueryString(queryParams)}";
        }
        
        private async Task<ExternalUserInfo> GetFacebookUserInfoAsync(string code, string redirectUri)
        {
            try
            {
                var options = _configuration.GetSection("SocialLogin:Facebook").Get<FacebookOptions>();
                var client = _httpClientFactory.CreateClient();
                
                // 获取访问令牌
                var tokenRequestUrl = $"https://graph.facebook.com/v12.0/oauth/access_token" +
                                     $"?client_id={options.AppId}" +
                                     $"&client_secret={options.AppSecret}" +
                                     $"&code={code}" +
                                     $"&redirect_uri={Uri.EscapeDataString(redirectUri)}";
                
                var tokenResponse = await client.GetAsync(tokenRequestUrl);
                
                if (!tokenResponse.IsSuccessStatusCode)
                {
                    _logger.LogError("Failed to get Facebook access token. Status: {StatusCode}", tokenResponse.StatusCode);
                    return null;
                }
                
                var tokenContent = await tokenResponse.Content.ReadAsStringAsync();
                var tokenData = JsonSerializer.Deserialize<JsonElement>(tokenContent);
                var accessToken = tokenData.GetProperty("access_token").GetString();
                
                // 获取用户信息
                var userInfoResponse = await client.GetAsync(
                    $"https://graph.facebook.com/v12.0/me?fields=id,email,first_name,last_name&access_token={accessToken}");
                
                if (!userInfoResponse.IsSuccessStatusCode)
                {
                    _logger.LogError("Failed to get Facebook user info. Status: {StatusCode}", userInfoResponse.StatusCode);
                    return null;
                }
                
                var userInfoContent = await userInfoResponse.Content.ReadAsStringAsync();
                var userInfo = JsonSerializer.Deserialize<JsonElement>(userInfoContent);
                
                return new ExternalUserInfo
                {
                    Id = Convert.ToInt64(userInfo.GetProperty("id")),
                    Email = userInfo.TryGetProperty("email", out var email) ? email.GetString() : null,
                    FirstName = userInfo.TryGetProperty("first_name", out var firstName) ? firstName.GetString() : "",
                    LastName = userInfo.TryGetProperty("last_name", out var lastName) ? lastName.GetString() : "",
                    Provider = "Facebook"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing Facebook login");
                return null;
            }
        }
        
        #endregion
        
        #region GitHub
        
        private string GetGitHubAuthorizationUrl(string redirectUri, string state)
        {
            var options = _configuration.GetSection("SocialLogin:GitHub").Get<GitHubOptions>();
            
            var queryParams = new Dictionary<string, string>
            {
                { "client_id", options.ClientId },
                { "redirect_uri", redirectUri },
                { "scope", "user:email" },
                { "state", state }
            };
            
            return $"https://github.com/login/oauth/authorize?{BuildQueryString(queryParams)}";
        }
        
        private async Task<ExternalUserInfo> GetGitHubUserInfoAsync(string code, string redirectUri)
        {
            try
            {
                var options = _configuration.GetSection("SocialLogin:GitHub").Get<GitHubOptions>();
                var client = _httpClientFactory.CreateClient();
                
                // 获取访问令牌
                var tokenRequestParams = new Dictionary<string, string>
                {
                    { "client_id", options.ClientId },
                    { "client_secret", options.ClientSecret },
                    { "code", code },
                    { "redirect_uri", redirectUri }
                };
                
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                
                var tokenResponse = await client.PostAsync(
                    "https://github.com/login/oauth/access_token",
                    new FormUrlEncodedContent(tokenRequestParams));
                
                if (!tokenResponse.IsSuccessStatusCode)
                {
                    _logger.LogError("Failed to get GitHub access token. Status: {StatusCode}", tokenResponse.StatusCode);
                    return null;
                }
                
                var tokenContent = await tokenResponse.Content.ReadAsStringAsync();
                var tokenData = JsonSerializer.Deserialize<JsonElement>(tokenContent);
                var accessToken = tokenData.GetProperty("access_token").GetString();
                
                // 获取用户信息
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("JackSite", "1.0"));
                
                var userInfoResponse = await client.GetAsync("https://api.github.com/user");
                
                if (!userInfoResponse.IsSuccessStatusCode)
                {
                    _logger.LogError("Failed to get GitHub user info. Status: {StatusCode}", userInfoResponse.StatusCode);
                    return null;
                }
                
                var userInfoContent = await userInfoResponse.Content.ReadAsStringAsync();
                var userInfo = JsonSerializer.Deserialize<JsonElement>(userInfoContent);
                
                // GitHub 可能不会直接返回邮箱，需要额外请求
                string email = null;
                if (userInfo.TryGetProperty("email", out var emailElement) && !emailElement.ValueKind.Equals(JsonValueKind.Null))
                {
                    email = emailElement.GetString();
                }
                else
                {
                    // 获取用户邮箱
                    var emailsResponse = await client.GetAsync("https://api.github.com/user/emails");
                    
                    if (emailsResponse.IsSuccessStatusCode)
                    {
                        var emailsContent = await emailsResponse.Content.ReadAsStringAsync();
                        var emails = JsonSerializer.Deserialize<JsonElement>(emailsContent);
                        
                        foreach (var emailObj in emails.EnumerateArray())
                        {
                            if (emailObj.TryGetProperty("primary", out var primary) && primary.GetBoolean())
                            {
                                email = emailObj.GetProperty("email").GetString();
                                break;
                            }
                        }
                        
                        // 如果没有找到主邮箱，使用第一个
                        if (string.IsNullOrEmpty(email) && emails.GetArrayLength() > 0)
                        {
                            email = emails[0].GetProperty("email").GetString();
                        }
                    }
                }
                
                // 分割名字
                string fullName = userInfo.TryGetProperty("name", out var nameElement) && !nameElement.ValueKind.Equals(JsonValueKind.Null)
                    ? nameElement.GetString()
                    : "";
                
                string firstName = "", lastName = "";
                if (!string.IsNullOrEmpty(fullName))
                {
                    var nameParts = fullName.Split(' ', 2);
                    firstName = nameParts[0];
                    lastName = nameParts.Length > 1 ? nameParts[1] : "";
                }
                
                return new ExternalUserInfo
                {
                    Id = Convert.ToInt64(userInfo.GetProperty("id")),
                    Email = email,
                    FirstName = firstName,
                    LastName = lastName,
                    Provider = "GitHub"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing GitHub login");
                return null;
            }
        }
        
        #endregion
        
        private string BuildQueryString(Dictionary<string, string> parameters)
        {
            return string.Join("&", parameters.Select(p => $"{Uri.EscapeDataString(p.Key)}={Uri.EscapeDataString(p.Value)}"));
        }
    }
}