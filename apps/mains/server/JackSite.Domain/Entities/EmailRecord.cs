namespace JackSite.Domain.Entities;

public class EmailRecord
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Message { get; set; } = null!;
    public string Subject { get; set; } = "No Subject";
    
    public string Sender { get; set; } = null!;
    public string Receiver { get; set; } = null!;
    
    public DateTime SendTime { get; set; } = DateTime.UtcNow;
    public DateTime? SentDate { get; set; }
    public EmailStatus Status { get; set; } = EmailStatus.Pending;
    public bool IsHtml { get; set; } = false;
    public EmailPriorityType Priority { get; set; } = EmailPriorityType.Normal;
    public string? ErrorMessage { get; set; }
}