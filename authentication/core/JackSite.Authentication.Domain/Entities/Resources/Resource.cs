namespace JackSite.Authentication.Entities.Resources;

public class Resource : Entity
{
    [Description("资源名称")]
    public string ResourceName { get; set; } = string.Empty;

    [Description("资源描述")]
    public string? Description { get; set; }

    [Description("资源路径")]
    public string Path { get; set; } = string.Empty;

    [Description("父级资源ID")]
    public Guid ParentId { get; set; } = Guid.Empty;
}