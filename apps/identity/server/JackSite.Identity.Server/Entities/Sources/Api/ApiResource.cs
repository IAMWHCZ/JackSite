namespace JackSite.Identity.Server.Entities.Sources.Api
{
    public class ApiResource:BaseEntity
    {
        public string Name { get; set; } = null!;

        public string DisplayName { get; set; } = null!;

        public string? Description { get; set; }

        public bool IsShowInDoc { get; set; }

        public bool IsEnable { get; set; }

        public virtual ICollection<BaseKeyEntity>? ApiResourceKeys { get; set; }

        public virtual ICollection<BaseProprietyEntity>? ApiResourceProperties { get; set; }

        public virtual ICollection<SigningAlgorithm>? AllowedIdentityTokenSigningAlgorithms { get; set; }

        public virtual ICollection<UserClaim>? UserClaims { get; set; }

        public virtual ICollection<Scope>? Scopes { get; set; }
    }
}