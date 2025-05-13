namespace JackSite.Identity.Server.Entities.Clients
{
    public class ClientDeviceWorkflow
    {
        public string Id { get; set; } = default!;

        public string ClientId { get; set; } = default!;

        public string UserCode { get; set; } = default!;

        public int CodeCycle { get; set; }
    }
}
