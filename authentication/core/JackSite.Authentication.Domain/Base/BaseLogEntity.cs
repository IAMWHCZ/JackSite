namespace JackSite.Authentication.Base;

public class BaseLogEntity:Entity
{
    public int StatusCode { get; set; }

    public string? Exception { get; set; }

    public long ElapsedMilliseconds { get; set; }
}