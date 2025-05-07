namespace JackSite.Domain.Base;

public interface ISoftDeletable
{
    bool IsDeleted { get;  set; }

    DateTime? DeletedOnUtc { get; set; }
}