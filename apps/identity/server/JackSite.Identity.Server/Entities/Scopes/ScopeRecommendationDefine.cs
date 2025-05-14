namespace JackSite.Identity.Server.Entities.Scopes
{
    public class ScopeRecommendationDefine : BaseEntity
    {
        [Required]
        [Comment("定义名称")]
        public int DefineName { get; set; }        
    }
}
