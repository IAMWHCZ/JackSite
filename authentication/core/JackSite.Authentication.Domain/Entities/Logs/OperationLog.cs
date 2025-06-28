namespace JackSite.Authentication.Entities.Logs;

public class OperationLog:BaseLogEntity
{
    public string ApiName { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsAuthorization {
        get;
        set;
    }

    public Guid? UserId { get; set; }
    
    public string? IpAddress { get; set; }

    public string? UserAgent { get; set; }

    public string? Browser { get; set; }

    public string? Os { get; set; }
}