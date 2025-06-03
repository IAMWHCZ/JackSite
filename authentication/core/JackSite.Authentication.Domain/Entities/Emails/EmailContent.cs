namespace JackSite.Authentication.Entities.Emails;

public class EmailContent:DraftableEntity
{
    public string? Content { get; set; }

    public bool IsHtml { get; set; } = false;
    
    public string? Subject { get; set; }

    public long EmailId { get; set; }

    public virtual EmailBasic EmailBasic { get; set; } = null!;
}