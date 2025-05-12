using JackSite.Identity.Server.Enums;

namespace JackSite.Identity.Server.Entities;

public class LanguageDataSet
{
    public int Id { get; set; }

    public LanguageModuleType Type { get; set; }

    public LanguageType LanguageCode { get; set; }

    public string Value { get; set; } = string.Empty;

    public string UpdateBy { get; set; } = string.Empty;

    public DateTime UpdateAt { get; set; }

    public bool IsReadOnly { get; set; }
}

