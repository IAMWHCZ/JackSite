namespace JackSite.Authentication.Base;

public class SoftDeleteEntity:Entity
{
    public bool IsDeleted { get; set; }

    public DateTime Type { get; set; }
}