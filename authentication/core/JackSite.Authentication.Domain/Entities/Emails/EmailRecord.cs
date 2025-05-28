using JackSite.Domain.Enums;

namespace JackSite.Authentication.Entities.Emails;

public class EmailRecord:DraftableEntity
{
    public string Title { get; set; } = null!;
    
    public SendEmailType Type { get; set; }
    
    public virtual ICollection<EmailRecipient> EmailRecipients { get; set; } = [];
}