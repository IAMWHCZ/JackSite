namespace JackSite.Identity.Server.Entities.Bases
{
    public class BaseEntity
    {
        public string Id { get; set; } = string.Empty;
        public DateTime UpdateAt { get; set; } = DateTime.UtcNow;
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;
        public string CreateBy { get; set; } = string.Empty;
        public string UpdateBy { get; set; } = string.Empty;
    }
}