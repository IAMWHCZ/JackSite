namespace JackSite.Authentication.Entities.Emails;

public class EmailContent:DraftableEntity
{
    public string Content { get; set; } = null!;

    public bool IsHtml { get; set; } = false;
    
    public string? Subject { get; set; }
}