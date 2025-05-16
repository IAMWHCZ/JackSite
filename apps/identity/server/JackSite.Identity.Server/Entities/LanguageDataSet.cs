namespace JackSite.Identity.Server.Entities;

public class LanguageDataSet
{
    [Key]
    [Required]
    [Comment("主键ID")]
    public int Id { get; set; }

    [Required]
    [Comment("模块类型")]
    public LanguageModuleType Type { get; set; }

    [Required]
    [Comment("语言代码")]
    public LanguageType LanguageCode { get; set; }

    [Required]
    [MaxLength(500)]
    [Comment("翻译值")]
    public string Value { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    [Comment("更新人")]
    public string UpdateBy { get; set; } = string.Empty;

    [Required]
    [Comment("更新时间")]
    public DateTime UpdateAt { get; set; }

    [Required]
    [Comment("是否只读")]
    public bool IsReadOnly { get; set; }
}

