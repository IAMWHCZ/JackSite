namespace JackSite.Authentication.Infrastructure.Repositories;

public interface IMongoRepository<TEntity> where TEntity : class
{
    Task<TEntity> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(string id, CancellationToken cancellationToken = default);
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<TEntity>> FindAsync(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    IQueryable<TEntity> GetQueryable();
    Task<IEnumerable<TProjection>> AggregateAsync<TProjection>(MongoDB.Driver.PipelineDefinition<TEntity, TProjection> pipeline, CancellationToken cancellationToken = default);
    Task BulkInsertAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
    Task<MongoDB.Driver.BulkWriteResult<TEntity>> BulkUpdateAsync(IEnumerable<MongoDB.Driver.WriteModel<TEntity>> requests, CancellationToken cancellationToken = default);
    Task<MongoDB.Driver.DeleteResult> BulkDeleteAsync(MongoDB.Driver.FilterDefinition<TEntity> filter, CancellationToken cancellationToken = default);
    Task<long> CountAsync(MongoDB.Driver.FilterDefinition<TEntity> filter, CancellationToken cancellationToken = default);
}

