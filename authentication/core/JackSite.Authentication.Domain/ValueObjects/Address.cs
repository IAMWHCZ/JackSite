namespace JackSite.Authentication.ValueObjects;

public class Address : ValueObject
{
    public string? Street { get; private set; }
    public string? City { get; private set; }
    public string? Province { get; private set; }
    public string? Country { get; private set; }
    public string? PostalCode { get; private set; }

    // 私有构造函数供EF Core使用
    private Address() { }

    public Address(string? street, string? city, string? province, string? country, string? postalCode)
    {
        Street = street;
        City = city;
        Province = province;
        Country = country;
        PostalCode = postalCode;
    }

    // 创建新实例的方法（保持不可变性）
    public Address WithStreet(string? street) => 
        new Address(street, City, Province, Country, PostalCode);

    public Address WithCity(string? city) => 
        new Address(Street, city, Province, Country, PostalCode);

    public Address WithProvince(string? province) => 
        new Address(Street, City, province, Country, PostalCode);

    public Address WithCountry(string? country) => 
        new Address(Street, City, Province, country, PostalCode);

    public Address WithPostalCode(string? postalCode) => 
        new Address(Street, City, Province, Country, postalCode);
    
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Street;
        yield return City;
        yield return Province;
        yield return Country;
        yield return PostalCode;
    }
}