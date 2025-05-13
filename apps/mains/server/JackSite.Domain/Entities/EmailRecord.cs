namespace JackSite.Domain.Entities;

public class EmailRecord
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public string Email { get; set; } = null!;

    public string Message { get; set; } = null!;

    public string Sender { get; set; } = null!;

    public DateTime SendTime { get; set; } 

    public string Receiver { get; set; } = null!;
}