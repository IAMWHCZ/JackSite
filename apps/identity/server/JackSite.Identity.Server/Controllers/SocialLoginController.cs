using JackSite.Identity.Server.Enums;
using JackSite.Identity.Server.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace JackSite.Identity.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SocialLoginController(
        ISocialLoginService socialLoginService,
        ITokenService tokenService,
        ILogger<SocialLoginController> logger) : ControllerBase
    {
        [HttpGet("providers")]
        public IActionResult GetProviders()
        {
            var providers = new List<string> { "Google", "Microsoft", "Facebook", "GitHub" };
            return Ok(providers);
        }
        
        [HttpGet("authorize/{provider}")]
        public IActionResult Authorize(string provider, [FromQuery] string redirectUri)
        {
            try
            {
                // 生成状态参数，防止CSRF攻击
                var state = GenerateState();
                
                // 将状态存储在会话中
                HttpContext.Session.SetString("SocialLoginState", state);
                HttpContext.Session.SetString("SocialLoginRedirectUri", redirectUri);
                
                // 获取授权URL
                var authorizationUrl = socialLoginService.GetAuthorizationUrl(provider, redirectUri, state);
                
                return Ok(new { url = authorizationUrl });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error generating authorization URL for provider: {Provider}", provider);
                return BadRequest(new { message = "Failed to generate authorization URL" });
            }
        }
        
        [HttpGet("callback/{provider}")]
        public async Task<IActionResult> Callback(SocialProvider provider, [FromQuery] string code, [FromQuery] string state, [FromQuery] string error)
        {
            try
            {
                // 检查是否有错误
                if (!string.IsNullOrEmpty(error))
                {
                    logger.LogWarning("Error returned from provider {Provider}: {Error}", provider, error);
                    return BadRequest(new { message = "Authentication failed" });
                }
                
                // 验证状态参数
                var savedState = HttpContext.Session.GetString("SocialLoginState");
                if (string.IsNullOrEmpty(savedState) || savedState != state)
                {
                    logger.LogWarning("Invalid state parameter for provider {Provider}", provider);
                    return BadRequest(new { message = "Invalid state parameter" });
                }
                
                // 获取重定向URI
                var redirectUri = HttpContext.Session.GetString("SocialLoginRedirectUri");
                
                // 处理社交登录
                var user = await socialLoginService.ProcessExternalLoginAsync(provider, code, redirectUri);
                
                if (user == null)
                {
                    return BadRequest(new { message = "Failed to authenticate with provider" });
                }
                
                // 生成令牌
                var (accessToken, refreshToken) = await tokenService.GenerateTokensAsync(user);
                
                // 清除会话数据
                HttpContext.Session.Remove("SocialLoginState");
                HttpContext.Session.Remove("SocialLoginRedirectUri");
                
                return Ok(new
                {
                    accessToken,
                    refreshToken,
                    expiresIn = 900 // 15分钟（秒）
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error processing callback for provider: {Provider}", provider);
                return BadRequest(new { message = "Authentication failed" });
            }
        }
        
        private static string GenerateState()
        {
            using var rng = RandomNumberGenerator.Create();
            var stateBytes = new byte[32];
            rng.GetBytes(stateBytes);
            return Convert.ToBase64String(stateBytes);
        }
    }
}