namespace JackSite.Common.Configs;

public class CorsPolicyConfig
{
    public IList<string> Origins { get; set; }  = [];
    public IList<string> Methods { get; set; } = [];
    public IList<string> Headers { get; set; } = [];
    public IList<string> ExposedHeaders { get; set; } = [];
    public bool AllowCredentials { get; set; }
}