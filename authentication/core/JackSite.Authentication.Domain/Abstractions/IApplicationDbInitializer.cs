namespace JackSite.Authentication.Abstractions;

public interface IApplicationDbInitializer
{
    Task SeedAsync();
}