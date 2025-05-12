using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace JackSite.Identity.Server.Entities;
public class LoginMethod
{
    [Key]
    [Required]
    [Comment("主键ID")]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    [Comment("显示名称")]
    public string DisplayName { get; set; } = string.Empty;

    [MaxLength(500)]
    [Comment("描述信息")]
    public string? Description { get; set; }
}

