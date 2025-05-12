using System.ComponentModel.DataAnnotations;
using JackSite.Identity.Server.Entities.Bases;
using Microsoft.EntityFrameworkCore;

namespace JackSite.Identity.Server.Entities.Scopes
{
    public class RecommendationDefine:BaseEntity
    {
        [Required]
        [Comment("定义名称")]
        public int DefineName { get; set; }        
    }
}
