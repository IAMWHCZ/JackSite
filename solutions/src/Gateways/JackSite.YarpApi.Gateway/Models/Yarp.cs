namespace JackSite.YarpApi.Gateway.Models;

public class YarpConfig
{
    public Dictionary<string, RouteConfig> Routes { get; set; } = new();
    public Dictionary<string, ClusterConfig> Clusters { get; set; } = new();
}

public class RouteConfig
{
    public string ClusterId { get; set; } = string.Empty;
    public RouteMatch Match { get; set; } = new();
    public List<Transform> Transforms { get; set; } = new();
}

public class RouteMatch
{
    public string Path { get; set; } = string.Empty;
}

public class Transform
{
    public string? PathPattern { get; set; }
}

public class ClusterConfig
{
    public Dictionary<string, Destination> Destinations { get; set; } = new();
}

public class Destination
{
    public string Address { get; set; } = string.Empty;
}