namespace JackSite.Identity.Server.Entities.Bases
{
    public class BaseKeyEntity:BaseEntity
    {
        public SecretType SecretType { get; set; }
        public string SecretValue { get; set; } = default!;
        public HashType HashType { get; set; }
        public DateTime ExpiredTime { get; set; }
        public string? Description { get; set; }
    }
}