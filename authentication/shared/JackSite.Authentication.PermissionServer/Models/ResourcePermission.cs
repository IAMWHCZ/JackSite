namespace JackSite.Authentication.PermissionServer.Models
{
    public class ResourcePermission
    {
        public string ResourceId { get; set; }

        public string ActionId { get; set; }

        public long? PolicyId { get; set; }
        public string Path { get; set; }

        public bool HasPermission { get; set; }
        public string DataResource { get; set; }
    }
}