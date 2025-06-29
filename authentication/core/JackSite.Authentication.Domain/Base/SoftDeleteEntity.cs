namespace JackSite.Authentication.Base;

public class SoftDeleteEntity:Entity
{
    public bool IsDeleted { get; set; }

    public DateTimeOffset Type { get; set; }
}