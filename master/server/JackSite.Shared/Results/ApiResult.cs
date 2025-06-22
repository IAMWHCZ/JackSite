namespace JackSite.Authentication.Application.Results;

/// <summary>
/// API统一返回结果
/// </summary>
public class ApiResult
{
    /// <summary>
    /// 是否成功
    /// </summary>
    public bool Success { get; set; }
    
    /// <summary>
    /// 状态码
    /// </summary>
    public int Code { get; set; }
    
    /// <summary>
    /// 消息
    /// </summary>
    public string Message { get; set; } = string.Empty;
    
    /// <summary>
    /// 时间戳
    /// </summary>
    public long Timestamp { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    
    /// <summary>
    /// 创建成功结果
    /// </summary>
    /// <param name="message">消息</param>
    /// <returns>API结果</returns>
    public static ApiResult Ok(string message = "操作成功")
    {
        return new ApiResult
        {
            Success = true,
            Code = 200,
            Message = message
        };
    }
    
    /// <summary>
    /// 创建失败结果
    /// </summary>
    /// <param name="message">错误消息</param>
    /// <param name="code">错误码</param>
    /// <returns>API结果</returns>
    public static ApiResult Fail(string message = "操作失败", int code = 400)
    {
        return new ApiResult
        {
            Success = false,
            Code = code,
            Message = message
        };
    }
    
    /// <summary>
    /// 创建未授权结果
    /// </summary>
    /// <param name="message">错误消息</param>
    /// <returns>API结果</returns>
    public static ApiResult Unauthorized(string message = "未授权")
    {
        return new ApiResult
        {
            Success = false,
            Code = 401,
            Message = message
        };
    }
    
    /// <summary>
    /// 创建禁止访问结果
    /// </summary>
    /// <param name="message">错误消息</param>
    /// <returns>API结果</returns>
    public static ApiResult Forbidden(string message = "禁止访问")
    {
        return new ApiResult
        {
            Success = false,
            Code = 403,
            Message = message
        };
    }
    
    /// <summary>
    /// 创建资源不存在结果
    /// </summary>
    /// <param name="message">错误消息</param>
    /// <returns>API结果</returns>
    public static ApiResult NotFound(string message = "资源不存在")
    {
        return new ApiResult
        {
            Success = false,
            Code = 404,
            Message = message
        };
    }
    
    /// <summary>
    /// 创建服务器错误结果
    /// </summary>
    /// <param name="message">错误消息</param>
    /// <returns>API结果</returns>
    public static ApiResult Error(string message = "服务器错误")
    {
        return new ApiResult
        {
            Success = false,
            Code = 500,
            Message = message
        };
    }
    
    /// <summary>
    /// 创建带数据的成功结果（支持类型推断）
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    /// <param name="data">数据</param>
    /// <param name="message">消息</param>
    /// <returns>API结果</returns>
    public static ApiResult<T> Ok<T>(T data, string message = "操作成功")
    {
        return ApiResult<T>.Ok(data, message);
    }

    /// <summary>
    /// 创建带数据的失败结果（支持类型推断）
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    /// <param name="message">错误消息</param>
    /// <param name="code">错误码</param>
    /// <returns>API结果</returns>
    public static ApiResult<T> Fail<T>(string message = "操作失败", int code = 400)
    {
        return ApiResult<T>.Fail(message, code);
    }

    /// <summary>
    /// 创建带数据的未授权结果（支持类型推断）
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    /// <param name="message">错误消息</param>
    /// <returns>API结果</returns>
    public static ApiResult<T> Unauthorized<T>(string message = "未授权")
    {
        return ApiResult<T>.Unauthorized(message);
    }

    /// <summary>
    /// 创建带数据的禁止访问结果（支持类型推断）
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    /// <param name="message">错误消息</param>
    /// <returns>API结果</returns>
    public static ApiResult<T> Forbidden<T>(string message = "禁止访问")
    {
        return ApiResult<T>.Forbidden(message);
    }

    /// <summary>
    /// 创建带数据的资源不存在结果（支持类型推断）
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    /// <param name="message">错误消息</param>
    /// <returns>API结果</returns>
    public static ApiResult<T> NotFound<T>(string message = "资源不存在")
    {
        return ApiResult<T>.NotFound(message);
    }

    /// <summary>
    /// 创建带数据的服务器错误结果（支持类型推断）
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    /// <param name="message">错误消息</param>
    /// <returns>API结果</returns>
    public static ApiResult<T> Error<T>(string message = "服务器错误")
    {
        return ApiResult<T>.Error(message);
    }
}

/// <summary>
/// 带数据的API统一返回结果
/// </summary>
/// <typeparam name="T">数据类型</typeparam>
public class ApiResult<T> : ApiResult
{
    /// <summary>
    /// 数据
    /// </summary>
    public T? Data { get; set; }
    
    /// <summary>
    /// 创建成功结果
    /// </summary>
    /// <param name="data">数据</param>
    /// <param name="message">消息</param>
    /// <returns>API结果</returns>
    public static ApiResult<T> Ok(T data, string message = "操作成功")
    {
        return new ApiResult<T>
        {
            Success = true,
            Code = 200,
            Message = message,
            Data = data
        };
    }
    
    /// <summary>
    /// 创建失败结果
    /// </summary>
    /// <param name="message">错误消息</param>
    /// <param name="code">错误码</param>
    /// <returns>API结果</returns>
    public new static ApiResult<T> Fail(string message = "操作失败", int code = 400)
    {
        return new ApiResult<T>
        {
            Success = false,
            Code = code,
            Message = message,
            Data = default
        };
    }
    
    /// <summary>
    /// 创建未授权结果
    /// </summary>
    /// <param name="message">错误消息</param>
    /// <returns>API结果</returns>
    public new static ApiResult<T> Unauthorized(string message = "未授权")
    {
        return new ApiResult<T>
        {
            Success = false,
            Code = 401,
            Message = message,
            Data = default
        };
    }
    
    /// <summary>
    /// 创建禁止访问结果
    /// </summary>
    /// <param name="message">错误消息</param>
    /// <returns>API结果</returns>
    public new static ApiResult<T> Forbidden(string message = "禁止访问")
    {
        return new ApiResult<T>
        {
            Success = false,
            Code = 403,
            Message = message,
            Data = default
        };
    }
    
    /// <summary>
    /// 创建资源不存在结果
    /// </summary>
    /// <param name="message">错误消息</param>
    /// <returns>API结果</returns>
    public new static ApiResult<T> NotFound(string message = "资源不存在")
    {
        return new ApiResult<T>
        {
            Success = false,
            Code = 404,
            Message = message,
            Data = default
        };
    }
    
    /// <summary>
    /// 创建服务器错误结果
    /// </summary>
    /// <param name="message">错误消息</param>
    /// <returns>API结果</returns>
    public new static ApiResult<T> Error(string message = "服务器错误")
    {
        return new ApiResult<T>
        {
            Success = false,
            Code = 500,
            Message = message,
            Data = default
        };
    }
}
