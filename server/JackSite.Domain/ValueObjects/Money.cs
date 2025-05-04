using System.Globalization;
using JackSite.Domain.ValueObjects;

namespace JackSite.Domain.ValueObjects;

public class Money : ValueObject
{
    public decimal Amount { get; }
    public string Currency { get; }
    
    // 常用货币代码
    public static class Currencies
    {
        public const string CNY = "CNY";
        public const string USD = "USD";
        public const string EUR = "EUR";
        public const string GBP = "GBP";
        public const string JPY = "JPY";
    }
    
    // 私有构造函数供EF Core使用
    private Money() { }
    
    public Money(decimal amount, string currency)
    {
        if (string.IsNullOrWhiteSpace(currency))
            throw new ArgumentException("Currency cannot be empty", nameof(currency));
            
        Amount = amount;
        Currency = currency;
    }
    
    // 创建常用货币的便捷方法
    public static Money FromCNY(decimal amount) => new Money(amount, Currencies.CNY);
    public static Money FromUSD(decimal amount) => new Money(amount, Currencies.USD);
    public static Money FromEUR(decimal amount) => new Money(amount, Currencies.EUR);
    public static Money FromGBP(decimal amount) => new Money(amount, Currencies.GBP);
    public static Money FromJPY(decimal amount) => new Money(amount, Currencies.JPY);
    
    // 创建新实例的方法（保持不可变性）
    public Money WithAmount(decimal newAmount) => 
        new Money(newAmount, Currency);
        
    public Money WithCurrency(string newCurrency) => 
        new Money(Amount, newCurrency);
    
    // 数学运算
    public Money Add(Money other)
    {
        if (other.Currency != Currency)
            throw new InvalidOperationException("Cannot add money with different currencies");
            
        return new Money(Amount + other.Amount, Currency);
    }
    
    public Money Subtract(Money other)
    {
        if (other.Currency != Currency)
            throw new InvalidOperationException("Cannot subtract money with different currencies");
            
        return new Money(Amount - other.Amount, Currency);
    }
    
    public Money Multiply(decimal factor) => 
        new Money(Amount * factor, Currency);
        
    public Money Divide(decimal divisor)
    {
        if (divisor == 0)
            throw new DivideByZeroException();
            
        return new Money(Amount / divisor, Currency);
    }
    
    // 格式化为货币字符串
    public string Format()
    {
        return Currency switch
        {
            Currencies.CNY => $"¥{Amount.ToString("N2", CultureInfo.InvariantCulture)}",
            Currencies.USD => $"${Amount.ToString("N2", CultureInfo.InvariantCulture)}",
            Currencies.EUR => $"€{Amount.ToString("N2", CultureInfo.InvariantCulture)}",
            Currencies.GBP => $"£{Amount.ToString("N2", CultureInfo.InvariantCulture)}",
            Currencies.JPY => $"¥{Amount.ToString("N0", CultureInfo.InvariantCulture)}",
            _ => $"{Amount.ToString("N2", CultureInfo.InvariantCulture)} {Currency}"
        };
    }
    
    public override string ToString() => Format();
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }
}