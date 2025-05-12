using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace JackSite.Identity.Server.Entities.Users
{
    public class IdentityUser
    {
        [Key]
        [Required]
        [MaxLength(50)]
        [Comment("用户唯一标识")]
        public string Id { get; set; } = string.Empty;
        
        [Required]
        [Comment("更新时间")]
        public DateTime UpdateAt { get; set; } = DateTime.UtcNow;
        
        [Required]
        [Comment("创建时间")]
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;
    }
}
