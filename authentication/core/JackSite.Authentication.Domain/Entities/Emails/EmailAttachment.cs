namespace JackSite.Authentication.Entities.Emails;

public class EmailAttachment: DraftableEntity
{
    public string ObjectKey { get; set; } = null!;

    public string? Description { get; set; }

    public Guid EmailRecordId { get; set; }

    public virtual EmailRecord EmailRecord { get; set; } = null!;
}