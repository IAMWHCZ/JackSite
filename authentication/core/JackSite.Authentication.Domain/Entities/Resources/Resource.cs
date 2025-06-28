namespace JackSite.Authentication.Entities.Resources;

public class Resource:Entity
{
    public string ResourceName { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string Path { get; set; } = string.Empty;

    public Guid ParentId { get; set; } = Guid.Empty;
}