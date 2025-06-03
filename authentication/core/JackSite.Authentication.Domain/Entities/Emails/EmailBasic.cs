using JackSite.Domain.Enums;

namespace JackSite.Authentication.Entities.Emails;

public class EmailBasic:DraftableEntity
{
    public string Title { get; set; } = null!;
    
    public SendEmailType Type { get; set; }
    
    public virtual ICollection<EmailRecipient> EmailRecipients { get; set; } = [];

    public virtual EmailContent? EmailContent { get; set; }
    
    public virtual EmailAttachment? EmailAttachment { get; set; }
}