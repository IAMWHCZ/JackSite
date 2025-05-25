namespace JackSite.Authentication.Base;

public interface ISoftDeletable
{
    bool IsDeleted { get;  set; }

    DateTime? DeletedOnUtc { get; set; }
}