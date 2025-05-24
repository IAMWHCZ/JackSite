using JackSite.Domain.ValueObjects;

namespace JackSite.Domain.ValueObjects;

public class DateRange : ValueObject
{
    public DateTime StartDate { get; }
    public DateTime EndDate { get; }
    
    // 私有构造函数供EF Core使用
    private DateRange() { }
    
    public DateRange(DateTime startDate, DateTime endDate)
    {
        if (endDate < startDate)
            throw new ArgumentException("End date must be greater than or equal to start date");
            
        StartDate = startDate;
        EndDate = endDate;
    }
    
    // 创建新实例的方法（保持不可变性）
    public DateRange WithStartDate(DateTime newStartDate) => 
        new DateRange(newStartDate, EndDate > newStartDate ? EndDate : newStartDate);
        
    public DateRange WithEndDate(DateTime newEndDate) => 
        new DateRange(StartDate < newEndDate ? StartDate : newEndDate, newEndDate);
    
    // 计算日期范围的天数
    public int GetDays() => (EndDate - StartDate).Days + 1;
    
    // 检查日期是否在范围内
    public bool Includes(DateTime date) => 
        date >= StartDate && date <= EndDate;
    
    // 检查两个日期范围是否重叠
    public bool Overlaps(DateRange other) => 
        StartDate <= other.EndDate && EndDate >= other.StartDate;
    
    // 获取两个日期范围的交集
    public DateRange? GetOverlap(DateRange other)
    {
        if (!Overlaps(other))
            return null;
            
        var maxStart = StartDate > other.StartDate ? StartDate : other.StartDate;
        var minEnd = EndDate < other.EndDate ? EndDate : other.EndDate;
        
        return new DateRange(maxStart, minEnd);
    }
    
    // 格式化日期范围为字符串
    public string GetFormattedRange(string format = "yyyy-MM-dd") => 
        $"{StartDate.ToString(format)} to {EndDate.ToString(format)}";
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return StartDate;
        yield return EndDate;
    }
}