namespace JackSite.Authentication.Base;

public abstract class BaseEntity<TId>(TId id)
{
    public TId Id { get; protected set; } = id;

    public Guid CreateBy { get; set; } = Guid.Empty;
    
    public DateTimeOffset CreateAt { get; set; } = DateTimeOffset.UtcNow;

    public Guid UpdateBy { get; set; } = Guid.Empty;
    
    public DateTimeOffset UpdateAt { get; set; } = DateTimeOffset.UtcNow;
    
}