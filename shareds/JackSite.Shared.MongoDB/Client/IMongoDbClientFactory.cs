namespace JackSite.Shared.MongoDB.Client;


/// <summary>
/// MongoDB 客户端工厂接口
/// </summary>
public interface IMongoDbClientFactory
{
    /// <summary>
    /// 获取 MongoDB 客户端
    /// </summary>
    IMongoClient GetClient();
    
    /// <summary>
    /// 获取 MongoDB 数据库
    /// </summary>
    IMongoDatabase GetDatabase();
    
    /// <summary>
    /// 获取集合
    /// </summary>
    IMongoCollection<TDocument> GetCollection<TDocument>(string collectionName);
}