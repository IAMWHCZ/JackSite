using System.ComponentModel.DataAnnotations;
using JackSite.Identity.Server.Enums;
using Microsoft.EntityFrameworkCore;

namespace JackSite.Identity.Server.Entities
{
    [Index(nameof(UserId), IsUnique = true)]
    public class Relationship
    {
        public Guid Id { get; set; } = Guid.NewGuid();

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

        [Required]
        [MaxLength(36)]
        [Comment("源ID")]
        public string SourceId { get; set; } = string.Empty;

        [Required]
        [MaxLength(36)]
        [Comment("目标ID")]
        public string TargetId { get; set; } = string.Empty;

        [Required]
        [Comment("关系类型")]
        public RelationshipType Type { get; set; }
        
        [Required]
        public DateTime UpdateAt { get; set; } = DateTime.UtcNow;
    }
}
