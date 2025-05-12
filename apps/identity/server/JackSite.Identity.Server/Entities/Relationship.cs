using JackSite.Identity.Server.Enums;

namespace JackSite.Identity.Server.Entities
{
    public class Relationship
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string SourceId { get; set; } = string.Empty;

        public string TargetId { get; set; } = string.Empty;

        public RelationshipType Type { get; set; }

        public DateTime UpdateAt { get; set; } = DateTime.UtcNow;

    }
}
