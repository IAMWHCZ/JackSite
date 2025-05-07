using JackSite.Identity.Server.Enums;
using JackSite.Identity.Server.Models;

namespace JackSite.Identity.Server.Interfaces
{
    public interface IMfaService
    {
        // TOTP 相关方法
        Task<(string secretKey, string qrCodeUri)> GenerateTotpSetupAsync(ApplicationUser user);
        Task<bool> VerifyTotpCodeAsync(ApplicationUser user, string code);
        Task<bool> EnableTotpMfaAsync(ApplicationUser user, string code);
        
        // SMS 相关方法
        Task<bool> SendSmsVerificationCodeAsync(ApplicationUser user);
        Task<bool> VerifySmsCodeAsync(ApplicationUser user, string code);
        Task<bool> EnableSmsMfaAsync(ApplicationUser user, string phoneNumber, string code);
        
        // Email 相关方法
        Task<bool> SendEmailVerificationCodeAsync(ApplicationUser user);
        Task<bool> VerifyEmailCodeAsync(ApplicationUser user, string code);
        Task<bool> EnableEmailMfaAsync(ApplicationUser user, string email, string code);
        
        // 通用方法
        Task<bool> DisableMfaAsync(ApplicationUser user);
        Task<bool> IsMfaEnabledAsync(ApplicationUser user);
        Task<MfaType> GetMfaTypeAsync(ApplicationUser user);
    }
}