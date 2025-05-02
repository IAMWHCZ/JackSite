namespace JackSite.AppHost.Interfaces;

public interface IComponentConfigurator
{
    void Configure(IDistributedApplicationBuilder builder, IConfiguration config);
    string ResourceName { get; }
    object Resource { get; }
}