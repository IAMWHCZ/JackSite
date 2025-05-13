using JackSite.Identity.Server.Entities.Bases;

namespace JackSite.Identity.Server.Entities.Clients
{
    public class ClientCrossPolicy:BaseEntity
    {
        public string Uri { get; set; } = default!;
    }
}
