using JackSite.Identity.Server.Entities.Bases;

namespace JackSite.Identity.Server.Entities.Clients
{
    public class ClientKey : BaseEntity
    {
        public string SecretValue { get; set; } = default!;

        public DateTime ExpiredTime { get; set; }

        public string? Description { get; set; }

        

        public void Update(string secretValue, string updateBy,DateTime expiredTime, string? description )
        {
            SecretValue = secretValue;
            ExpiredTime = expiredTime;
            Description = description;
            UpdateAt = DateTime.UtcNow;
            UpdateBy = updateBy;
        }
    }
}