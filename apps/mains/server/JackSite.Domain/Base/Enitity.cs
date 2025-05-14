namespace JackSite.Domain.Base;

public abstract class Entity : BaseEntity<long>
{
    private static readonly SnowflakeIdGenerator IdGenerator = new(1, 1);

    protected Entity():base(IdGenerator.NextId())
    {
        Id = IdGenerator.NextId();
    }
}