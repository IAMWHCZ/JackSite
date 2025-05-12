using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace JackSite.Identity.Server.Entities.Scopes;

public class ScopeUserDefine
{
    [Required]
    [MaxLength(50)]
    [Comment("用户ID")]
    public string UserId { get; set; } = default!;

    [Required]
    [MaxLength(100)]
    [Comment("定义名称")]
    public string DefineName { get; set; } = default!;
}

