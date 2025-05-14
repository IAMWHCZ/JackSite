namespace JackSite.Identity.Server.Entities.Sources.Identity
{
     public class IdentitySource:BaseEntity
    {
        public string SourceName { get; set; } = default!;
        public string DisplayName { get; set; } = default!;
        public string? Description { get; set; }
        public bool IsEnable { get; set; }
        public bool IsShowInDoc { get; set; }
        public bool IsRequired { get; set; }
        public bool IsEmphasize { get; set; }
        public virtual ICollection<UserClaim>? UserClaims { get; set; }
        public virtual ICollection<BaseProprietyEntity>? IdentitySourceProperties { get; set; }
    }
}