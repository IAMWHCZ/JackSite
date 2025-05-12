using JackSite.Identity.Server.Entities.Bases;

namespace JackSite.Identity.Server.Entities.Clients
{
    public class Client:BaseEntity
    {
        public string ClientFlag { get; set; } = default!;

        public string ClientName { get; set;} = default!;
    }
}