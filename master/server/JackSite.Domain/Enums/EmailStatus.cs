namespace JackSite.Domain.Enums;

public enum EmailStatus:byte
{
    Finished = 1,
    Failed,
    Pending,
    Sending
}