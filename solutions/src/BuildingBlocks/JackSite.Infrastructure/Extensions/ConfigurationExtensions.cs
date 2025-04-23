namespace JackSite.Infrastructure.Extensions;

public static class ConfigurationExtensions
{
    public static T GetOptions<T>(this IConfiguration configuration, string sectionName) where T : class, new()
    {
        var options = new T();
        configuration.GetSection(sectionName).Bind(options);
        return options;
    }

    public static IServiceCollection ConfigureAndValidate<T>(
        this IServiceCollection services,
        IConfiguration configuration,
        string sectionName) where T : class
    {
        services.Configure<T>(configuration.GetSection(sectionName));
        services.AddOptions<T>()
            .Bind(configuration.GetSection(sectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return services;
    }
}
