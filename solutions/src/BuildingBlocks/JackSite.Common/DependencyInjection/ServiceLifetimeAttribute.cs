using Microsoft.Extensions.DependencyInjection;

namespace JackSite.Common.DependencyInjection;

[AttributeUsage(AttributeTargets.Class)]
public class ServiceLifetimeAttribute(ServiceLifetime lifetime) : Attribute
{
    public ServiceLifetime Lifetime { get; } = lifetime;
}