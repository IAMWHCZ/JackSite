using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace JackSite.Identity.Server.Entities.Users
{
    public class IdentityUserSecurity
    {
        [Required]
        [MaxLength(50)]
        [Comment("用户ID")]
        public string UserId { get; set;} = default!;
        
        [Required]
        [Comment("邮箱是否已确认")]
        public bool IsEmailConfirmed { get; set; }
        
        [Required]
        [Comment("手机号是否已确认")]
        public bool IsPhoneNumberConfirmed { get; set; }
        
        [Required]
        [Comment("是否锁定")]
        public bool IsLocked { get; set; }
        
        [Required]
        [Comment("是否启用两步验证")]
        public bool IsTwoStepVerification { get; set; }
        
        [Required]
        [Comment("失败尝试次数")]
        public byte FailedAttempts { get; set; }
        
        [Required]
        [Comment("解锁时间")]
        public DateTime UnlockedTime { get; set; }
    }
}
