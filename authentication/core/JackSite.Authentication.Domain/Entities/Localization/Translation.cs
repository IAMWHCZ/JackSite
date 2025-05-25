using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JackSite.Authentication.Base;

namespace JackSite.Authentication.Entities.Localization;

public class Translation : Entity
{
    [Required]
    [MaxLength(100)]
    [Column(TypeName = "varchar(100)")]
    public string KeyName { get; private set; } = string.Empty;
    
    public string? EnglishText { get; private set; }
    
    public string? ChineseText { get; private set; }
    
    [MaxLength(50)]
    public string? Category { get; private set; }
    
    // 私有构造函数供EF Core使用
    private Translation() { }
    
    // 领域构造函数
    public Translation(string keyName, string? englishText = null, string? chineseText = null, string? category = null)
    {
        KeyName = keyName;
        EnglishText = englishText;
        ChineseText = chineseText;
        Category = category;
    }
    
    // 更新方法
    public void UpdateEnglishText(string text)
    {
        EnglishText = text;
    }
    
    public void UpdateChineseText(string text)
    {
        ChineseText = text;
    }
    
    public void UpdateCategory(string? category)
    {
        Category = category;
    }
}