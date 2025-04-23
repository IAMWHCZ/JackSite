using JackSite.Common.Configs;
using JackSite.Common.CQRS;
using JackSite.Common.Results;
using JackSite.Infrastructure.Caching;
using JackSite.YarpApi.Gateway.Data;
using JackSite.YarpApi.Gateway.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;

namespace JackSite.YarpApi.Gateway.Queries;

public record GetGatewayRequestQuery(
    string? Path,
    string? Method,
    string? QueryString,
    string? RequestBody,
    string? RequestHeaders,
    string? ResponseHeaders,
    string? ResponseBody,
    int? StatusCode,
    string? ErrorMessage,
    string? StackTrace,
    string? ClientIp,
    string? UserAgent,
    long? ExecutionTime,
    string? TargetService,
    List<DateTime>? RequestTimeRange, // 直接使用这个名称
    List<DateTime>? ResponseTimeRange, // 直接使用这个名称
    int PageIndex = 1,
    int PageSize = 10
) : IQuery<Result<PagedResult<RequestLog>>>;

internal sealed class GetGatewayRequestHandler(
    ICacheService cacheService,
    GatewayDbContext dbContext,
    IOptions<ApplicationDomain> applicationDomain,
    ILogger<GetGatewayRequestHandler> logger
) : IQueryHandler<GetGatewayRequestQuery, Result<PagedResult<RequestLog>>>
{
    public async Task<Result<PagedResult<RequestLog>>> Handle(GetGatewayRequestQuery request,
        CancellationToken cancellationToken)
    {
        var key =
            $"{applicationDomain.Value.Host}-{applicationDomain.Value.Port}-{applicationDomain.Value.Name}-GetGatewayRequestHandler";

        IQueryable<RequestLog> query;
        bool isFromCache = false;

        try
        {
            var gatewayRequests = await cacheService.GetAsync<IList<RequestLog>>(key);
            if (gatewayRequests != null)
            {
                query = gatewayRequests.AsQueryable();
                isFromCache = true;
            }
            else
            {
                query = dbContext.RequestLogs
                    .AsNoTracking()
                    .AsQueryable();

                // Move cache operation to a separate try-catch block
                try
                {
                    var allRequests = await query.ToListAsync(cancellationToken);
                    // Set cache with a timeout of 5 minutes
                    await cacheService.SetAsync(key, allRequests, TimeSpan.FromMinutes(5));
                }
                catch (Exception cacheEx)
                {
                    logger.LogWarning(cacheEx, "Failed to set cache for gateway requests");
                }
            }
        }
        catch (Exception cacheEx)
        {
            logger.LogWarning(cacheEx, "Failed to get cache for gateway requests, falling back to database");
            query = dbContext.RequestLogs
                .AsNoTracking()
                .AsQueryable();
        }

        // Apply filters
        if (!string.IsNullOrEmpty(request.Path))
            query = query.Where(x => x.Path.Contains(request.Path));

        if (!string.IsNullOrEmpty(request.Method))
            query = query.Where(x => x.Method == request.Method);

        if (!string.IsNullOrEmpty(request.QueryString))
            query = query.Where(x => x.QueryString.Contains(request.QueryString));

        if (!string.IsNullOrEmpty(request.RequestBody))
            query = query.Where(x => x.RequestBody.Contains(request.RequestBody));

        if (!string.IsNullOrEmpty(request.RequestHeaders))
            query = query.Where(x => x.RequestHeaders!.Contains(request.RequestHeaders));

        if (!string.IsNullOrEmpty(request.ResponseHeaders))
            query = query.Where(x => x.ResponseHeaders!.Contains(request.ResponseHeaders));

        if (!string.IsNullOrEmpty(request.ResponseBody))
            query = query.Where(x => x.ResponseBody!.Contains(request.ResponseBody));

        if (request.StatusCode.HasValue)
            query = query.Where(x => x.StatusCode == request.StatusCode);

        if (!string.IsNullOrEmpty(request.ErrorMessage))
            query = query.Where(x => x.ErrorMessage!.Contains(request.ErrorMessage));

        if (!string.IsNullOrEmpty(request.StackTrace))
            query = query.Where(x => x.StackTrace!.Contains(request.StackTrace));

        if (!string.IsNullOrEmpty(request.ClientIp))
            query = query.Where(x => x.ClientIp == request.ClientIp);

        if (!string.IsNullOrEmpty(request.UserAgent))
            query = query.Where(x => x.UserAgent!.Contains(request.UserAgent));

        if (request.RequestTimeRange is { Count: 2 })

            query = query.Where(x => x.RequestTime >= request.RequestTimeRange[0]
                                     && x.RequestTime <= request.RequestTimeRange[1]);

        if (request.ResponseTimeRange is { Count: 2 })
            query = query.Where(x => x.RequestTime >= request.ResponseTimeRange[0]
                                     && x.RequestTime <= request.ResponseTimeRange[1]);

        if (request.ExecutionTime.HasValue)
            query = query.Where(x => x.ExecutionTime >= request.ExecutionTime);

        if (!string.IsNullOrEmpty(request.TargetService))
            query = query.Where(x => x.TargetService == request.TargetService);

        try
        {
            List<RequestLog> result;
            int totalCount;

            if (isFromCache)
            {
                var filteredList = query.ToList();
                totalCount = filteredList.Count;
                result = filteredList
                    .Skip((request.PageIndex - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToList();
            }
            else
            {
                totalCount = await query.CountAsync(cancellationToken);
                result = await query
                    .Skip((request.PageIndex - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync(cancellationToken);
            }

            if (!result.Any())
            {
                return Result.Success(new PagedResult<RequestLog>(
                    new List<RequestLog>(),
                    0,
                    request.PageIndex,
                    request.PageSize));
            }

            return Result.Success(new PagedResult<RequestLog>(
                result,
                totalCount,
                request.PageIndex,
                request.PageSize));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error executing gateway request query");
            return Result.Failure<PagedResult<RequestLog>>("Failed to retrieve gateway requests");
        }
    }
}