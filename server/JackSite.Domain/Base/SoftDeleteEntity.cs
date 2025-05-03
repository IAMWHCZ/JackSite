namespace JackSite.Domain.Base;

public class SoftDeleteEntity:BaseEntity
{
    public bool IsDeleted { get; set; }

    public DateTime Type { get; set; }
}