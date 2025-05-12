using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace JackSite.Identity.Server.Entities
{
    public class Password
    {
        [Required]
        [MaxLength(50)]
        [Comment("用户ID")]
        public string UserId { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        [Comment("用户名")]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        [Comment("密码哈希")]
        public string PasswordHash { get; set; } = string.Empty;
    }
}
