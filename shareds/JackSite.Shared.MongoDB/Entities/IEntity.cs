

namespace JackSite.Shared.MongoDB.Entities;

/// <summary>
/// 实体接口
/// </summary>
public partial interface IEntity
{
}

/// <summary>
/// 带 ID 的实体接口
/// </summary>
public interface IEntity<TKey> : IEntity
{
    /// <summary>
    /// 实体 ID
    /// </summary>
    TKey Id { get; set; }
}

/// <summary>
/// 带 ObjectId 的实体基类
/// </summary>
public abstract class MongoEntityBase : IEntity<string>
{
    /// <summary>
    /// 实体 ID
    /// </summary>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
}

/// <summary>
/// 带审计信息的实体基类
/// </summary>
public abstract class AuditableMongoEntityBase : MongoEntityBase
{
    /// <summary>
    /// 创建时间
    /// </summary>
    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// 创建者 ID
    /// </summary>
    [BsonElement("createdBy")]
    public string? CreatedBy { get; set; }
    
    /// <summary>
    /// 最后更新时间
    /// </summary>
    [BsonElement("updatedAt")]
    public DateTime? UpdatedAt { get; set; }
    
    /// <summary>
    /// 最后更新者 ID
    /// </summary>
    [BsonElement("updatedBy")]
    public string? UpdatedBy { get; set; }
    
    /// <summary>
    /// 是否已删除
    /// </summary>
    [BsonElement("isDeleted")]
    public bool IsDeleted { get; set; } = false;
    
    /// <summary>
    /// 删除时间
    /// </summary>
    [BsonElement("deletedAt")]
    public DateTime? DeletedAt { get; set; }
    
    /// <summary>
    /// 删除者 ID
    /// </summary>
    [BsonElement("deletedBy")]
    public string? DeletedBy { get; set; }
}