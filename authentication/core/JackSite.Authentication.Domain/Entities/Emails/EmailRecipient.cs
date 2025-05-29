namespace JackSite.Authentication.Entities.Emails;

public class EmailRecipient:DraftableEntity
{
    public string RecipientEmail { get; set; } = null!;
    
    public string? RecipientName { get; set; }
    
    public Guid EmailRecordId { get; set; }
    
    public virtual EmailRecord EmailRecord { get; set; } = null!;
}