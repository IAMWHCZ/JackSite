using JackSite.Identity.Server.Entities.Bases;

namespace JackSite.Identity.Server.Entities.Clients
{
    public class ClientAllowScreen:BaseEntity
    {
        public string ClientId { get; set; } = default!;

        public bool IsAllow { get; set; }

        public bool IsAllowRemember { get; set; }

        public string ClientUri { get; set; } = default!;

        public void Update() { 

        }
    }
}
