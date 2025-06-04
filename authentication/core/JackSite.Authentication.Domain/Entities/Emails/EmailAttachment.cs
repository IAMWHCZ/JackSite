namespace JackSite.Authentication.Entities.Emails;

public class EmailAttachment: DraftableEntity
{
    
    public string ObjectKey { get; set; } = null!;

    public string? Description { get; set; }

    public long EmailRecordId { get; set; }

    public virtual EmailBasic EmailBasic { get; set; } = null!;
}