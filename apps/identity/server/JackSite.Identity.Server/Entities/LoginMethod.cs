namespace JackSite.Identity.Server.Entities;
public class LoginMethod
{
    public int Id { get; set; }

    public string DisplayName { get; set; } = string.Empty;

    public string? Description { get; set; }
}

