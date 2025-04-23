namespace JackSite.Common.Domain;

public abstract class EntityBase
{
    public SnowflakeId Id { get; set; } = new SnowflakeId();
    public DateTime CreatedTime { get; set; }

    public SnowflakeId CreateAt { get; set; }
    public DateTime? UpdatedTime { get; set; }
    public SnowflakeId? UpdateAt { get; set; }
}