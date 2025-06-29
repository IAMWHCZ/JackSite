namespace JackSite.Authentication.Entities.Users;

public class UserProfile : Entity, ISoftDeletable
{
    [Required]
    [Description("用户ID")]
    public Guid UserId { get; private set; }

    [MaxLength(50)]
    [Column(TypeName = "varchar(50)")]
    [Description("真实姓名")]
    public string? RealName { get; private set; }

    [MaxLength(20)]
    [Column(TypeName = "varchar(20)")]
    [Description("性别")]
    public string? Gender { get; private set; }

    [Description("出生日期")]
    public DateTimeOffset? BirthDate { get; private set; }

// 数据库映射字段
    [MaxLength(200)]
    [Description("街道地址")]
    public string? Street { get; private set; }

    [MaxLength(100)]
    [Description("城市")]
    public string? City { get; private set; }

    [MaxLength(100)]
    [Description("省份/州")]
    public string? Province { get; private set; }

    [MaxLength(100)]
    [Description("国家")]
    public string? Country { get; private set; }

    [MaxLength(20)]
    [Description("邮政编码")]
    public string? PostalCode { get; private set; }

    // 不映射到数据库的值对象属性
    [NotMapped]
    public Address Address
    {
        get => new Address(Street, City, Province, Country, PostalCode);
        private set
        {
            Street = value.Street;
            City = value.City;
            Province = value.Province;
            Country = value.Country;
            PostalCode = value.PostalCode;
        }
    }

    // 导航属性
    public UserBasic User { get; private set; } = null!;

    // 软删除属性
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedOnUtc { get; set; }

    // 私有构造函数供 EF Core 使用
    private UserProfile()
    {
    }

    // 领域构造函数
    public UserProfile(Guid userId, string? realName = null, string? gender = null, DateTimeOffset? birthDate = null)
    {
        UserId = userId;
        RealName = realName;
        Gender = gender;
        BirthDate = birthDate;
    }

    // 领域行为
    public void UpdateBasicInfo(string? realName, string? gender, DateTimeOffset? birthDate)
    {
        RealName = realName;
        Gender = gender;
        BirthDate = birthDate;
    }

    public void UpdateAddress(Address address)
    {
        Address = address;
    }

    public void UpdateContactInfo(string? street, string? city, string? province, string? country, string? postalCode)
    {
        Street = street;
        City = city;
        Province = province;
        Country = country;
        PostalCode = postalCode;
    }
}