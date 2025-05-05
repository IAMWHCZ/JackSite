using System.Security.Claims;
using JackSite.Identity.Server.Models;
using JackSite.Identity.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace JackSite.Identity.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(
        IUserService userService,
        ITokenService tokenService,
        IMfaService mfaService,
        ILogger<AuthController> logger,
        ISocialLoginService socialLoginService,
        UserManager<ApplicationUser> userManager)
        : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            // 验证用户凭据
            var isValid = await userService.ValidateUserCredentialsAsync(request.Username, request.Password);
            
            if (!isValid)
            {
                return Unauthorized(new { message = "Invalid username or password" });
            }
            
            var user = await userService.GetUserByUsernameAsync(request.Username);
            
            // 检查是否启用了 MFA
            var isMfaEnabled = await mfaService.IsMfaEnabledAsync(user);
            
            if (isMfaEnabled)
            {
                // 如果启用了 MFA，返回 MFA 类型和临时令牌
                var mfaType = await mfaService.GetMfaTypeAsync(user);
                var mfaToken = await tokenService.GenerateMfaTokenAsync(user);
                
                // 如果是 SMS 类型，自动发送验证码
                if (mfaType == "SMS")
                {
                    await mfaService.SendSmsVerificationCodeAsync(user);
                }
                // 如果是 Email 类型，自动发送验证码
                else if (mfaType == "Email")
                {
                    await mfaService.SendEmailVerificationCodeAsync(user);
                }
                
                return Ok(new
                {
                    requiresMfa = true,
                    mfaType,
                    mfaToken
                });
            }
            
            // 如果没有启用 MFA，直接生成访问令牌和刷新令牌
            var (accessToken, refreshToken) = await tokenService.GenerateTokensAsync(user);
            
            return Ok(new
            {
                accessToken,
                refreshToken,
                expiresIn = 900 // 15 分钟（秒）
            });
        }
        
        [HttpPost("mfa/verify")]
        public async Task<IActionResult> VerifyMfa([FromBody] VerifyMfaRequest request)
        {
            try
            {
                // 验证 MFA 令牌
                var userId = tokenService.ValidateMfaToken(request.MfaToken);
                var user = await userService.GetUserByIdAsync(userId);
                
                if (user == null)
                {
                    return Unauthorized(new { message = "Invalid MFA token" });
                }
                
                // 获取 MFA 类型
                var mfaType = await mfaService.GetMfaTypeAsync(user);
                
                // 根据 MFA 类型验证代码
                bool isValid = false;
                
                if (mfaType == "TOTP")
                {
                    isValid = await mfaService.VerifyTotpCodeAsync(user, request.Code);
                }
                else if (mfaType == "SMS")
                {
                    isValid = await mfaService.VerifySmsCodeAsync(user, request.Code);
                }
                else if (mfaType == "Email")
                {
                    isValid = await mfaService.VerifyEmailCodeAsync(user, request.Code);
                }
                
                if (!isValid)
                {
                    return Unauthorized(new { message = "Invalid verification code" });
                }
                
                // 验证成功，生成访问令牌和刷新令牌
                var (accessToken, refreshToken) = await tokenService.GenerateTokensAsync(user);
                
                return Ok(new
                {
                    accessToken,
                    refreshToken,
                    expiresIn = 900 // 15 分钟（秒）
                });
            }
            catch (SecurityTokenException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }
        
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
        {
            try
            {
                var (accessToken, refreshToken) = await tokenService.RefreshTokenAsync(request.RefreshToken);
                
                return Ok(new
                {
                    accessToken,
                    refreshToken,
                    expiresIn = 900 // 15 分钟（秒）
                });
            }
            catch (SecurityTokenException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }
        
        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout([FromBody] LogoutRequest request)
        {
            await tokenService.RevokeTokenAsync(request.RefreshToken);
            return Ok(new { message = "Logged out successfully" });
        }

        [HttpPost("link-social")]
        [Authorize]
        public async Task<IActionResult> LinkSocialAccount([FromBody] LinkSocialRequest request)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var user = await userService.GetUserByIdAsync(userId);
                
                if (user == null)
                {
                    return Unauthorized();
                }
                
                // 处理社交登录
                var socialUser = await socialLoginService.ProcessExternalLoginAsync(
                    request.Provider, request.Code, request.RedirectUri);
                
                if (socialUser == null)
                {
                    return BadRequest(new { message = "Failed to authenticate with provider" });
                }
                
                // 更新用户社交登录信息
                user.SocialProvider = request.Provider;
                user.SocialProviderId = socialUser.Id;
                user.SocialLoginDate = DateTime.UtcNow;
                
                await userManager.UpdateAsync(user);
                
                return Ok(new { message = "Social account linked successfully" });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error linking social account");
                return BadRequest(new { message = "Failed to link social account" });
            }
        }
    }
    
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
    
    public class RefreshTokenRequest
    {
        public string RefreshToken { get; set; }
    }
    
    public class LogoutRequest
    {
        public string RefreshToken { get; set; }
    }
    
    public class VerifyMfaRequest
    {
        public string MfaToken { get; set; }
        public string Code { get; set; }
    }

    public class LinkSocialRequest
    {
        public string Provider { get; set; }
        public string Code { get; set; }
        public string RedirectUri { get; set; }
    }
}