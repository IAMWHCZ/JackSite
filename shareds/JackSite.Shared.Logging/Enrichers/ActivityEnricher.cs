using System.Diagnostics;
using Serilog.Core;
using Serilog.Events;

namespace JackSite.Shared.Logging.Enrichers;

/// <summary>
/// 活动丰富器
/// </summary>
public class ActivityEnricher : ILogEventEnricher
{
    /// <summary>
    /// 丰富日志事件
    /// </summary>
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var activity = Activity.Current;
        
        if (activity == null)
            return;
            
        // 添加跟踪 ID
        if (!string.IsNullOrEmpty(activity.TraceId.ToString()))
        {
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(
                "TraceId", activity.TraceId.ToString()));
        }
        
        // 添加跨度 ID
        if (!string.IsNullOrEmpty(activity.SpanId.ToString()))
        {
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(
                "SpanId", activity.SpanId.ToString()));
        }
        
        // 添加父跨度 ID
        if (!string.IsNullOrEmpty(activity.ParentSpanId.ToString()))
        {
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(
                "ParentSpanId", activity.ParentSpanId.ToString()));
        }
    }
}