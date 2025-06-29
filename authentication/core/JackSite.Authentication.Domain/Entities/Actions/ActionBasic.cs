namespace JackSite.Authentication.Entities.Actions;
public class ActionBasic : Entity, IOrderable
{
    [Description("操作名称")]
    public string ActionName { get; set; } = string.Empty;

    [Description("操作描述")]
    public string? ActionDescription { get; set; }

    [Description("排序顺序")]
    public int Order { get; set; }
}