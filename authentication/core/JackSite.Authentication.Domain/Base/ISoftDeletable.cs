namespace JackSite.Authentication.Base;

public interface ISoftDeletable
{
    bool IsDeleted { get;  set; }

    DateTimeOffset? DeletedOnUtc { get; set; }
}