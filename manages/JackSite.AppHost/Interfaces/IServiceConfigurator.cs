namespace JackSite.AppHost.Interfaces;

public interface IServiceConfigurator
{
    IResourceBuilder<IResource> Configure(IDistributedApplicationBuilder builder, IConfiguration config, Dictionary<string, object> resources);
    string ServiceName { get; }
}