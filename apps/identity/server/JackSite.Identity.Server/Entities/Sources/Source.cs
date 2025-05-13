using JackSite.Identity.Server.Entities.Bases;
using JackSite.Identity.Server.Entities.Users;

namespace JackSite.Identity.Server.Entities.Sources
{
    public class Source:BaseEntity
    {
        public string SourceName { get; set; } = default!;
        public string DisplayName { get; set; } = default!;
        public string? Description { get; set; }
        public bool IsEnable { get; set; }
        public bool IsShowInDoc { get; set; }
        public bool IsRequired { get; set; }
        public bool IsEmphasize { get; set; }
        public ICollection<UserClaim>? UserClaims { get; set; }
    }
}
