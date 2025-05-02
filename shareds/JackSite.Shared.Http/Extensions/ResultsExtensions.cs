using JackSite.Shared.Http.Models;
using Microsoft.AspNetCore.Http;

namespace JackSite.Shared.Http.Extensions;

/// <summary>
/// Results 扩展方法
/// </summary>
public static class ResultsExtensions
{
    /// <summary>
    /// 返回成功响应
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    /// <param name="data">数据</param>
    /// <param name="message">消息</param>
    /// <returns>IResult</returns>
    public static IResult Success<T>(T? data, string? message = null)
    {
        return Results.Ok(new ApiResponse<T>(data, true, message));
    }

    /// <summary>
    /// 返回失败响应
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    /// <param name="message">错误消息</param>
    /// <param name="data">数据</param>
    /// <returns>IResult</returns>
    public static IResult Fail<T>(string message, T? data = default)
    {
        return Results.BadRequest(new ApiResponse<T>(data, false, message));
    }

    /// <summary>
    /// 返回错误响应
    /// </summary>
    /// <param name="message">错误消息</param>
    /// <param name="statusCode">HTTP状态码</param>
    /// <param name="details">错误详情</param>
    /// <returns>IResult</returns>
    public static IResult Error(string message, int statusCode = 500, object? details = null)
    {
        return Results.Problem(
            detail: details?.ToString(),
            statusCode: statusCode,
            title: message);
    }

    /// <summary>
    /// 返回创建成功响应
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    /// <param name="data">数据</param>
    /// <param name="uri">资源URI</param>
    /// <param name="message">消息</param>
    /// <returns>IResult</returns>
    public static IResult Created<T>(T data, string uri, string? message = null)
    {
        return Results.Created(uri, new ApiResponse<T>(data, true, message ?? "创建成功"));
    }

    /// <summary>
    /// 返回分页响应
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    /// <param name="items">数据项</param>
    /// <param name="total">总数</param>
    /// <param name="page">页码</param>
    /// <param name="pageSize">页大小</param>
    /// <returns>IResult</returns>
    public static IResult Paged<T>(IEnumerable<T> items, long total, int page, int pageSize)
    {
        var pagedResult = new
        {
            items,
            total,
            page,
            pageSize,
            totalPages = (int)Math.Ceiling((double)total / pageSize)
        };

        return Results.Ok(new ApiResponse<object>(pagedResult));
    }
}