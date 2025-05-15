namespace JackSite.Identity.Server.Entities.Persistence
{
    public class PersistenceAuthorization
    {
        public long Id { get; set; }
        public byte Status { get; set; }
        public DateTime ExpiredTime { get; set; }
        public string? Data { get; set; }
        public long ClientId { get; set; }
        public string? Description { get; set; }
        public long SessionId { get; set; }
        public DateTime ConsumedTime { get; set; }
    }
}