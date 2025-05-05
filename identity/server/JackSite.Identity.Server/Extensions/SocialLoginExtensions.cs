using JackSite.Identity.Server.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using System.Security.Claims;

namespace JackSite.Identity.Server.Extensions
{
    public static class SocialLoginExtensions
    {
        public static AuthenticationBuilder AddSocialLogins(this AuthenticationBuilder builder, IConfiguration configuration)
        {
            var socialLoginOptions = configuration.GetSection("SocialLogin").Get<SocialLoginOptions>();
            
            // 添加 Google 登录
            if (socialLoginOptions?.Google?.Enabled == true)
            {
                builder.AddGoogle(options =>
                {
                    options.ClientId = socialLoginOptions.Google.ClientId;
                    options.ClientSecret = socialLoginOptions.Google.ClientSecret;
                    options.SaveTokens = true;
                });
            }
            
            // 添加 Microsoft 登录
            if (socialLoginOptions?.Microsoft?.Enabled == true)
            {
                builder.AddMicrosoftAccount(options =>
                {
                    options.ClientId = socialLoginOptions.Microsoft.ClientId;
                    options.ClientSecret = socialLoginOptions.Microsoft.ClientSecret;
                    options.SaveTokens = true;
                });
            }
            
            // 添加 Facebook 登录
            if (socialLoginOptions?.Facebook?.Enabled == true)
            {
                builder.AddFacebook(options =>
                {
                    options.AppId = socialLoginOptions.Facebook.AppId;
                    options.AppSecret = socialLoginOptions.Facebook.AppSecret;
                    options.SaveTokens = true;
                });
            }
            
            // 添加 GitHub 登录
            if (socialLoginOptions?.GitHub?.Enabled == true)
            {
                builder.AddGitHub(options =>
                {
                    options.ClientId = socialLoginOptions.GitHub.ClientId;
                    options.ClientSecret = socialLoginOptions.GitHub.ClientSecret;
                    options.SaveTokens = true;
                    options.Scope.Add("user:email");
                });
            }
            
            return builder;
        }
    }
    
    // GitHub 认证扩展方法
    public static class GitHubAuthenticationExtensions
    {
        public static AuthenticationBuilder AddGitHub(this AuthenticationBuilder builder, Action<OAuthOptions> configureOptions)
        {
            return builder.AddOAuth("GitHub", "GitHub", options =>
            {
                options.AuthorizationEndpoint = "https://github.com/login/oauth/authorize";
                options.TokenEndpoint = "https://github.com/login/oauth/access_token";
                options.UserInformationEndpoint = "https://api.github.com/user";
                
                options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
                options.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
                options.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
                
                options.Events = new OAuthEvents
                {
                    OnCreatingTicket = async context =>
                    {
                        var request = new HttpRequestMessage(HttpMethod.Get, options.UserInformationEndpoint);
                        request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", context.AccessToken);
                        
                        var response = await context.Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, context.HttpContext.RequestAborted);
                        response.EnsureSuccessStatusCode();
                        
                        var user = await response.Content.ReadFromJsonAsync<System.Text.Json.JsonElement>();
                        
                        context.RunClaimActions(user);
                    }
                };
                
                configureOptions(options);
            });
        }
    }
}