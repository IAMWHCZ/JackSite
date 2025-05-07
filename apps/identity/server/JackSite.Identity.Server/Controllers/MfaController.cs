using JackSite.Identity.Server.Interfaces;
using JackSite.Identity.Server.Models;
using JackSite.Identity.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JackSite.Identity.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MfaController(
        UserManager<ApplicationUser> userManager,
        IMfaService mfaService,
        ILogger<MfaController> logger)
        : ControllerBase
    {
        private readonly ILogger<MfaController> _logger = logger;

        [HttpGet("status")]
        public async Task<IActionResult> GetMfaStatus()
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return Unauthorized();
            }

            var isMfaEnabled = await mfaService.IsMfaEnabledAsync(user);
            var mfaType = await mfaService.GetMfaTypeAsync(user);

            return Ok(new
            {
                isMfaEnabled,
                mfaType,
                isRegistrationComplete = user.IsMfaRegistrationComplete
            });
        }

        [HttpPost("totp/setup")]
        public async Task<IActionResult> SetupTotp()
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return Unauthorized();
            }

            var (secretKey, qrCodeUri) = await mfaService.GenerateTotpSetupAsync(user);

            return Ok(new
            {
                secretKey,
                qrCodeUri
            });
        }

        [HttpPost("totp/verify")]
        public async Task<IActionResult> VerifyTotp([FromBody] VerifyTotpRequest request)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return Unauthorized();
            }

            var isValid = await mfaService.VerifyTotpCodeAsync(user, request.Code);

            return Ok(new
            {
                isValid
            });
        }

        [HttpPost("totp/enable")]
        public async Task<IActionResult> EnableTotp([FromBody] EnableTotpRequest request)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return Unauthorized();
            }

            var success = await mfaService.EnableTotpMfaAsync(user, request.Code);

            return Ok(new
            {
                success
            });
        }

        [HttpPost("sms/setup")]
        public async Task<IActionResult> SetupSms([FromBody] SetupSmsRequest request)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return Unauthorized();
            }

            // 更新用户电话号码
            user.PhoneNumber = request.PhoneNumber;
            await userManager.UpdateAsync(user);

            // 发送验证码
            var success = await mfaService.SendSmsVerificationCodeAsync(user);

            return Ok(new
            {
                success
            });
        }

        [HttpPost("sms/verify")]
        public async Task<IActionResult> VerifySms([FromBody] VerifySmsRequest request)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return Unauthorized();
            }

            var isValid = await mfaService.VerifySmsCodeAsync(user, request.Code);

            return Ok(new
            {
                isValid
            });
        }

        [HttpPost("sms/enable")]
        public async Task<IActionResult> EnableSms([FromBody] EnableSmsRequest request)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return Unauthorized();
            }

            var success = await mfaService.EnableSmsMfaAsync(user, request.PhoneNumber, request.Code);

            return Ok(new
            {
                success
            });
        }

        [HttpPost("disable")]
        public async Task<IActionResult> DisableMfa()
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return Unauthorized();
            }

            var success = await mfaService.DisableMfaAsync(user);

            return Ok(new
            {
                success
            });
        }

        [HttpPost("sms/resend")]
        public async Task<IActionResult> ResendSmsCode()
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return Unauthorized();
            }

            var success = await mfaService.SendSmsVerificationCodeAsync(user);

            return Ok(new
            {
                success
            });
        }

        [HttpPost("email/setup")]
        public async Task<IActionResult> SetupEmail([FromBody] SetupEmailRequest request)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return Unauthorized();
            }

            // 更新用户邮箱
            if (!string.IsNullOrEmpty(request.Email) && request.Email != user.Email)
            {
                user.Email = request.Email;
                await userManager.UpdateAsync(user);
            }

            // 发送验证码
            var success = await mfaService.SendEmailVerificationCodeAsync(user);

            return Ok(new
            {
                success
            });
        }

        [HttpPost("email/verify")]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailRequest request)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return Unauthorized();
            }

            var isValid = await mfaService.VerifyEmailCodeAsync(user, request.Code);

            return Ok(new
            {
                isValid
            });
        }

        [HttpPost("email/enable")]
        public async Task<IActionResult> EnableEmail([FromBody] EnableEmailRequest request)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return Unauthorized();
            }

            var success = await mfaService.EnableEmailMfaAsync(user, request.Email, request.Code);

            return Ok(new
            {
                success
            });
        }

        [HttpPost("email/resend")]
        public async Task<IActionResult> ResendEmailCode()
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return Unauthorized();
            }

            var success = await mfaService.SendEmailVerificationCodeAsync(user);

            return Ok(new
            {
                success
            });
        }

        private async Task<ApplicationUser> GetCurrentUserAsync()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return null;
            }

            return await userManager.FindByIdAsync(userId) ?? throw new InvalidOperationException();
        }
    }

    public record VerifyTotpRequest(string Code);

    public record EnableTotpRequest(string Code);

    public record SetupSmsRequest(string PhoneNumber);

    public record VerifySmsRequest(string Code);

    public record EnableSmsRequest(string PhoneNumber, string Code);

    public record SetupEmailRequest(string Email);

    public record VerifyEmailRequest(string Code);

    public record EnableEmailRequest(string Email, string Code);
}