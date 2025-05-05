using Microsoft.AspNetCore.Identity;

namespace JackSite.Identity.Server.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? LastPasswordChangeDate { get; set; }
        public bool IsExternalUser { get; set; }
        public string ExternalUserId { get; set; }
        
        // MFA 相关字段
        public bool IsMfaEnabled { get; set; }
        public string MfaType { get; set; } // "TOTP", "SMS", "Email", etc.
        public string MfaKey { get; set; } // 加密的 TOTP 密钥或手机号码
        public DateTime? MfaRegistrationDate { get; set; }
        public bool IsMfaRegistrationComplete { get; set; }
        public string PhoneNumberCountryCode { get; set; }
        
        // 社交登录相关字段
        public string SocialProvider { get; set; } // "Google", "Microsoft", "Facebook", "GitHub"
        public string SocialProviderId { get; set; } // 社交平台上的用户ID
        public DateTime? SocialLoginDate { get; set; } // 最后一次社交登录时间
    }
    
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public string UserId { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsRevoked { get; set; }
    }
}