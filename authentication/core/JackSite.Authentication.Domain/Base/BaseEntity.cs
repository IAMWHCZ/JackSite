namespace JackSite.Authentication.Base;

public abstract class BaseEntity<TId>(TId id)
{
    public TId Id { get; protected set; } = id;

    public long CreateBy { get; set; } = 1;
    
    public DateTime CreateAt { get; set; } = DateTime.UtcNow;

    public long UpdateBy { get; set; } = 1;
    
    public DateTime UpdateAt { get; set; } = DateTime.UtcNow;
    
}