namespace JackSite.Identity.Server.Entities.Clients
{
    public class ClientRedirectUri
    {
        public int Id { get; set; }

        public string RedirectUri { get; set;} = default!;
    }
}