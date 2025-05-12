namespace JackSite.Identity.Server.Entities.Scopes;

public class Scope
{
    public int Id { get; set; }
    public string ScopeName { get; set; } = string.Empty;
    public string? DisplayName { get; set; }

    public bool IsRequired { get; set; }

    public bool IsEmphasize { get; set; }
}

