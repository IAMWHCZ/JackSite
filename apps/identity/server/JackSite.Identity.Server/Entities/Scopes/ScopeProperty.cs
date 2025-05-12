using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace JackSite.Identity.Server.Entities.Scopes;

public class ScopeProperty
{
    [Key]
    [Required]
    [Comment("主键ID")]
    public int Id { get; set; }

    [Required]
    [Comment("属性值")]
    public int MyProperty { get; set; }
}

