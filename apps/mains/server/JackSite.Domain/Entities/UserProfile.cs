using JackSite.Domain.ValueObjects;

namespace JackSite.Domain.Entities;

public class UserProfile : Entity, ISoftDeletable
{
    [Required]
    public long UserId { get; private set; }
    
    [MaxLength(50)]
    [Column(TypeName = "varchar(50)")]
    public string? RealName { get; private set; }
    
    [MaxLength(20)]
    [Column(TypeName = "varchar(20)")]
    public string? Gender { get; private set; }
    
    public DateTime? BirthDate { get; private set; }
    
    // 数据库映射字段
    [MaxLength(200)]
    public string? Street { get; private set; }
    
    [MaxLength(100)]
    public string? City { get; private set; }
    
    [MaxLength(100)]
    public string? Province { get; private set; }
    
    [MaxLength(100)]
    public string? Country { get; private set; }
    
    [MaxLength(20)]
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
    public DateTime? DeletedOnUtc { get; set; }
    
    // 私有构造函数供 EF Core 使用
    private UserProfile() { }
    
    // 领域构造函数
    public UserProfile(long userId, string? realName = null, string? gender = null, DateTime? birthDate = null)
    {
        UserId = userId;
        RealName = realName;
        Gender = gender;
        BirthDate = birthDate;
    }
    
    // 领域行为
    public void UpdateBasicInfo(string? realName, string? gender, DateTime? birthDate)
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