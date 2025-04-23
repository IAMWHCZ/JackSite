using JackSite.Common.Domain;
using System.Text.Json;

namespace JackSite.YarpApi.Gateway.Entities;

public class YarpConfig : EntityBase
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ConfigJson { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime LastModified { get; set; }

    // 用于序列化和反序列化配置
    public Models.YarpConfig GetConfig()
    {
        return string.IsNullOrEmpty(ConfigJson) 
            ? new Models.YarpConfig() 
            : JsonSerializer.Deserialize<Models.YarpConfig>(ConfigJson) ?? new Models.YarpConfig();
    }

    public void SetConfig(Models.YarpConfig config)
    {
        ConfigJson = JsonSerializer.Serialize(config);
        LastModified = DateTime.UtcNow;
    }
}