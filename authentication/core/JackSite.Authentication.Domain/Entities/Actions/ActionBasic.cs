namespace JackSite.Authentication.Entities.Actions;

public class ActionBasic : Entity, IOrderable
{
    public string ActionName { get; set; } = string.Empty;
    public string? ActionDescription { get; set; }
    public int Order { get; set; }
}