namespace JackSite.Domain.Base;

public abstract class BaseEntity(long id)
{
    public long Id { get; protected set; } = id;

    public long CreateBy { get; set; }
    
    public DateTime CreateAt { get; set; } = DateTime.UtcNow;
    
    public long UpdateBy { get; set; }
    
    public DateTime UpdateAt { get; set; } = DateTime.UtcNow;

    private static readonly SnowflakeIdGenerator IdGenerator = new(1, 1);
    
    protected BaseEntity() : this(IdGenerator.NextId())
    {
    }
}