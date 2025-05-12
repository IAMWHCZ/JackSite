using JackSite.Identity.Server.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace JackSite.Identity.Server.Entities.Users
{
    public class IdentityUserProfile
    {
        [Required]
        [MaxLength(50)]
        [Comment("用户ID")]
        public string UserId { get; set; } = string.Empty;
        
        [Required]
        [Comment("系统主题")]
        public ThemeType SystemTheme { get; set; }

        [Required]
        [Comment("语言设置")]
        public LanguageType Language { get; set; }
    }
}
