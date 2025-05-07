using JackSite.Identity.Server.Enums;
using Microsoft.AspNetCore.Identity;


namespace JackSite.Identity.Server.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime? LastPasswordChangeDate { get; set; }
        public bool IsExternalUser { get; set; }
        public string ExternalUserId { get; set; } = string.Empty;
        
        // MFA 相关字段
        public bool IsMfaEnabled { get; set; }
        public MfaType MfaType { get; set; } = MfaType.None;
        public string MfaKey { get; set; } = string.Empty; // 加密的 TOTP 密钥或手机号码
        public DateTime? MfaRegistrationDate { get; set; }
        public bool IsMfaRegistrationComplete { get; set; }
        public string PhoneNumberCountryCode { get; set; } = string.Empty;
        
        // 社交登录相关字段
        public SocialProvider SocialProvider { get; set; } = SocialProvider.None;
        public string SocialProviderId { get; set; } = string.Empty;
        public DateTime? SocialLoginDate { get; set; } // 最后一次社交登录时间
    }
}
