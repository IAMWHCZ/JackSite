using System.Security.Cryptography;
using JackSite.Identity.Server.Models;
using Microsoft.AspNetCore.Identity;
using OtpNet;

namespace JackSite.Identity.Server.Services
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
        Task<string> GetMfaTypeAsync(ApplicationUser user);
    }
    
    public class MfaService : IMfaService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<MfaService> _logger;
        private readonly ISmsService _smsService;
        private readonly IEmailService _emailService;
        private readonly IDataProtectionService _dataProtectionService;
        
        public MfaService(
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration,
            ILogger<MfaService> logger,
            ISmsService smsService,
            IEmailService emailService,
            IDataProtectionService dataProtectionService)
        {
            _userManager = userManager;
            _configuration = configuration;
            _logger = logger;
            _smsService = smsService;
            _emailService = emailService;
            _dataProtectionService = dataProtectionService;
        }
        
        #region TOTP Methods
        
        public async Task<(string secretKey, string qrCodeUri)> GenerateTotpSetupAsync(ApplicationUser user)
        {
            // 生成一个新的随机密钥
            var secretKey = GenerateSecretKey();
            
            // 创建 Base32 编码的密钥
            var base32Secret = Base32Encoding.ToString(secretKey);
            
            // 创建 QR 码 URI
            var appName = _configuration["Mfa:AppName"] ?? "JackSite";
            var qrCodeUri = $"otpauth://totp/{appName}:{user.Email}?secret={base32Secret}&issuer={appName}";
            
            // 加密并存储密钥（但不启用 MFA，直到验证成功）
            user.MfaKey = _dataProtectionService.Protect(base32Secret);
            user.MfaType = "TOTP";
            user.IsMfaRegistrationComplete = false;
            
            await _userManager.UpdateAsync(user);
            
            return (base32Secret, qrCodeUri);
        }
        
        public async Task<bool> VerifyTotpCodeAsync(ApplicationUser user, string code)
        {
            if (string.IsNullOrEmpty(user.MfaKey) || user.MfaType != "TOTP")
            {
                return false;
            }
            
            try
            {
                var secretKey = _dataProtectionService.Unprotect(user.MfaKey);
                var secretBytes = Base32Encoding.ToBytes(secretKey);
                
                var totp = new Totp(secretBytes);
                return totp.VerifyTotp(code, out _);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verifying TOTP code for user {UserId}", user.Id);
                return false;
            }
        }
        
        public async Task<bool> EnableTotpMfaAsync(ApplicationUser user, string code)
        {
            if (await VerifyTotpCodeAsync(user, code))
            {
                user.IsMfaEnabled = true;
                user.IsMfaRegistrationComplete = true;
                user.MfaRegistrationDate = DateTime.UtcNow;
                
                var result = await _userManager.UpdateAsync(user);
                return result.Succeeded;
            }
            
            return false;
        }
        
        #endregion
        
        #region SMS Methods
        
        public async Task<bool> SendSmsVerificationCodeAsync(ApplicationUser user)
        {
            if (string.IsNullOrEmpty(user.PhoneNumber))
            {
                return false;
            }
            
            try
            {
                // 生成 6 位数字验证码
                var verificationCode = GenerateNumericCode(6);
                
                // 加密并临时存储验证码
                var encryptedCode = _dataProtectionService.Protect(verificationCode);
                user.MfaKey = encryptedCode;
                user.MfaType = "SMS";
                user.IsMfaRegistrationComplete = false;
                
                await _userManager.UpdateAsync(user);
                
                // 发送短信
                var phoneNumber = $"{user.PhoneNumberCountryCode}{user.PhoneNumber}";
                var message = $"Your verification code is: {verificationCode}";
                
                return await _smsService.SendSmsAsync(phoneNumber, message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending SMS verification code for user {UserId}", user.Id);
                return false;
            }
        }
        
        public async Task<bool> VerifySmsCodeAsync(ApplicationUser user, string code)
        {
            if (string.IsNullOrEmpty(user.MfaKey) || user.MfaType != "SMS")
            {
                return false;
            }
            
            try
            {
                var storedCode = _dataProtectionService.Unprotect(user.MfaKey);
                return storedCode == code;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verifying SMS code for user {UserId}", user.Id);
                return false;
            }
        }
        
        public async Task<bool> EnableSmsMfaAsync(ApplicationUser user, string phoneNumber, string code)
        {
            // 验证手机号码格式
            if (!IsValidPhoneNumber(phoneNumber))
            {
                return false;
            }
            
            // 解析国家代码和电话号码
            var (countryCode, number) = ParsePhoneNumber(phoneNumber);
            
            // 更新用户电话号码
            user.PhoneNumber = number;
            user.PhoneNumberCountryCode = countryCode;
            user.PhoneNumberConfirmed = false;
            
            await _userManager.UpdateAsync(user);
            
            // 发送验证码
            await SendSmsVerificationCodeAsync(user);
            
            // 验证用户输入的验证码
            if (await VerifySmsCodeAsync(user, code))
            {
                user.IsMfaEnabled = true;
                user.IsMfaRegistrationComplete = true;
                user.MfaRegistrationDate = DateTime.UtcNow;
                user.PhoneNumberConfirmed = true;
                
                var result = await _userManager.UpdateAsync(user);
                return result.Succeeded;
            }
            
            return false;
        }
        
        #endregion
        
        #region Email Methods
        
        public async Task<bool> SendEmailVerificationCodeAsync(ApplicationUser user)
        {
            if (string.IsNullOrEmpty(user.Email))
            {
                return false;
            }
            
            try
            {
                // 生成 6 位数字验证码
                var verificationCode = GenerateNumericCode(6);
                
                // 加密并临时存储验证码
                var encryptedCode = _dataProtectionService.Protect(verificationCode);
                user.MfaKey = encryptedCode;
                user.MfaType = "Email";
                user.IsMfaRegistrationComplete = false;
                
                await _userManager.UpdateAsync(user);
                
                // 构建邮件内容
                var appName = _configuration["Mfa:AppName"] ?? "JackSite";
                var subject = $"{appName} - 您的验证码";
                var body = $@"
                <html>
                <body>
                    <h2>验证码</h2>
                    <p>您的验证码是: <strong>{verificationCode}</strong></p>
                    <p>此验证码将在 5 分钟内有效。</p>
                    <p>如果您没有请求此验证码，请忽略此邮件。</p>
                </body>
                </html>";
                
                // 发送邮件
                return await _emailService.SendEmailAsync(user.Email, subject, body, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email verification code for user {UserId}", user.Id);
                return false;
            }
        }
        
        public async Task<bool> VerifyEmailCodeAsync(ApplicationUser user, string code)
        {
            if (string.IsNullOrEmpty(user.MfaKey) || user.MfaType != "Email")
            {
                return false;
            }
            
            try
            {
                var storedCode = _dataProtectionService.Unprotect(user.MfaKey);
                return storedCode == code;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verifying email code for user {UserId}", user.Id);
                return false;
            }
        }
        
        public async Task<bool> EnableEmailMfaAsync(ApplicationUser user, string email, string code)
        {
            // 验证邮箱格式
            if (!IsValidEmail(email))
            {
                return false;
            }
            
            // 更新用户邮箱
            var currentEmail = user.Email;
            if (email != currentEmail)
            {
                user.Email = email;
                user.EmailConfirmed = false;
                
                await _userManager.UpdateAsync(user);
                
                // 发送验证码
                await SendEmailVerificationCodeAsync(user);
            }
            
            // 验证用户输入的验证码
            if (await VerifyEmailCodeAsync(user, code))
            {
                user.IsMfaEnabled = true;
                user.IsMfaRegistrationComplete = true;
                user.MfaRegistrationDate = DateTime.UtcNow;
                user.EmailConfirmed = true;
                
                var result = await _userManager.UpdateAsync(user);
                return result.Succeeded;
            }
            
            return false;
        }
        
        #endregion
        
        #region Common Methods
        
        public async Task<bool> DisableMfaAsync(ApplicationUser user)
        {
            user.IsMfaEnabled = false;
            user.IsMfaRegistrationComplete = false;
            user.MfaKey = null;
            user.MfaType = null;
            
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }
        
        public Task<bool> IsMfaEnabledAsync(ApplicationUser user)
        {
            return Task.FromResult(user.IsMfaEnabled && user.IsMfaRegistrationComplete);
        }
        
        public Task<string> GetMfaTypeAsync(ApplicationUser user)
        {
            return Task.FromResult(user.MfaType);
        }
        
        #endregion
        
        #region Helper Methods
        
        private byte[] GenerateSecretKey()
        {
            var key = new byte[20]; // 160 bits
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(key);
            }
            return key;
        }
        
        private string GenerateNumericCode(int length)
        {
            var random = new Random();
            var code = string.Empty;
            
            for (int i = 0; i < length; i++)
            {
                code += random.Next(0, 10).ToString();
            }
            
            return code;
        }
        
        private bool IsValidPhoneNumber(string phoneNumber)
        {
            // 简单验证，实际应用中可能需要更复杂的验证
            return !string.IsNullOrEmpty(phoneNumber) && 
                   phoneNumber.Length >= 8 && 
                   phoneNumber.StartsWith("+");
        }
        
        private (string countryCode, string number) ParsePhoneNumber(string phoneNumber)
        {
            // 简单解析，实际应用中可能需要更复杂的解析
            if (phoneNumber.StartsWith("+"))
            {
                var parts = phoneNumber.Substring(1).Split(' ', 2);
                if (parts.Length == 2)
                {
                    return ($"+{parts[0]}", parts[1]);
                }
            }
            
            // 默认返回
            return ("+1", phoneNumber);
        }
        
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        
        #endregion
    }
}