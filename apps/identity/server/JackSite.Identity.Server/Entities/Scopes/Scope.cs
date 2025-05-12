using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace JackSite.Identity.Server.Entities.Scopes;

public class Scope
{
    [Key]
    [Required]
    [Comment("主键ID")]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    [Comment("作用域名称")]
    public string ScopeName { get; set; } = string.Empty;
    
    [MaxLength(200)]
    [Comment("显示名称")]
    public string? DisplayName { get; set; }

    [Required]
    [Comment("是否必需")]
    public bool IsRequired { get; set; }

    [Required]
    [Comment("是否强调")]
    public bool IsEmphasize { get; set; }
}

