namespace JackSite.Shared.MongoDB.Tools;

/// <summary>
/// MongoDB 数据导入导出工具
/// </summary>
public class MongoImportExport(IMongoDbClientFactory clientFactory)
{
    /// <summary>
    /// 导出集合数据到 JSON 文件
    /// </summary>
    public async Task ExportToJsonFileAsync<TDocument>(
        string collectionName,
        string filePath,
        FilterDefinition<TDocument>? filter = null,
        CancellationToken cancellationToken = default)
    {
        var collection = clientFactory.GetCollection<TDocument>(collectionName);
        
        // 构建查询
        filter ??= Builders<TDocument>.Filter.Empty;
        var documents = await collection.Find(filter).ToListAsync(cancellationToken);
        
        // 创建 JSON 写入器设置
        var jsonWriterSettings = new JsonWriterSettings
        {
            OutputMode = JsonOutputMode.Shell,
            Indent = true
        };
        
        // 写入文件
        await using var streamWriter = new StreamWriter(filePath, false, Encoding.UTF8);
        await streamWriter.WriteAsync("[");
        
        for (var i = 0; i < documents.Count; i++)
        {
            var json = documents[i].ToJson(jsonWriterSettings);
            await streamWriter.WriteAsync(json);
            
            if (i < documents.Count - 1)
            {
                await streamWriter.WriteAsync(",");
            }
            
            await streamWriter.WriteLineAsync();
        }
        
        await streamWriter.WriteAsync("]");
    }
    
    /// <summary>
    /// 从 JSON 文件导入数据到集合
    /// </summary>
    public async Task ImportFromJsonFileAsync<TDocument>(
        string collectionName,
        string filePath,
        bool dropExisting = false,
        CancellationToken cancellationToken = default)
    {
        var collection = clientFactory.GetCollection<TDocument>(collectionName);
        
        // 如果需要，删除现有集合
        if (dropExisting)
        {
            await collection.Database.DropCollectionAsync(collectionName, cancellationToken);
        }
        
        // 读取 JSON 文件
        string json;
        using (var streamReader = new StreamReader(filePath, Encoding.UTF8))
        {
            json = await streamReader.ReadToEndAsync(cancellationToken);
        }
        
        // 解析 JSON 数组
        var documents = BsonSerializer.Deserialize<List<TDocument>>(json);
        
        // 批量插入文档
        if (documents.Count > 0)
        {
            await collection.InsertManyAsync(documents, null, cancellationToken);
        }
    }
    
    /// <summary>
    /// 导出集合数据到 CSV 文件
    /// </summary>
    public async Task ExportToCsvFileAsync<TDocument>(
        string collectionName,
        string filePath,
        IEnumerable<string> fields,
        FilterDefinition<TDocument>? filter = null,
        CancellationToken cancellationToken = default)
    {
        // 获取 BsonDocument 类型的集合
        var collection = clientFactory.GetCollection<BsonDocument>(collectionName);
        
        // 构建查询 - 简化处理
        var filterDefinition = Builders<BsonDocument>.Filter.Empty;
        var documents = await collection.Find(filterDefinition).ToListAsync(cancellationToken);
        
        // 写入 CSV 文件
        await using var streamWriter = new StreamWriter(filePath, false, Encoding.UTF8);
        
        // 写入标题行
        var enumerable = fields as string[] ?? fields.ToArray();
        await streamWriter.WriteLineAsync(string.Join(",", enumerable));
        
        // 写入数据行
        foreach (var document in documents)
        {
            var values = new List<string>();
            
            foreach (var field in enumerable)
            {
                if (document.TryGetValue(field, out var value))
                {
                    // 处理特殊字符
                    var stringValue = value.ToString()?.Replace("\"", "\"\"") ?? "";
                    values.Add($"\"{stringValue}\"");
                }
                else
                {
                    values.Add("\"\"");
                }
            }
            
            await streamWriter.WriteLineAsync(string.Join(",", values));
        }
    }
}