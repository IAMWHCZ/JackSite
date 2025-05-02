using Microsoft.EntityFrameworkCore.Diagnostics;

namespace JackSite.Shared.EntityFrameworkCore.Interceptors;

/// <summary>
/// 审计保存拦截器
/// </summary>
public class AuditSaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly Func<string?> _currentUserProvider;

    public AuditSaveChangesInterceptor(Func<string?> currentUserProvider)
    {
        _currentUserProvider = currentUserProvider;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateEntities(DbContext? context)
    {
        if (context == null) return;

        var currentUser = _currentUserProvider();
        var now = DateTime.UtcNow;

        foreach (var entry in context.ChangeTracker.Entries())
        {
            if (entry.Entity is Entities.AuditableEntityBase<int> auditableEntity)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        auditableEntity.CreatedAt = now;
                        auditableEntity.CreatedBy = currentUser;
                        break;
                    
                    case EntityState.Modified:
                        auditableEntity.LastModifiedAt = now;
                        auditableEntity.LastModifiedBy = currentUser;
                        break;
                }
            }
            
            // 处理软删除
            if (entry.Entity is Entities.SoftDeleteEntityBase<int> softDeleteEntity && entry.State == EntityState.Deleted)
            {
                entry.State = EntityState.Modified;
                softDeleteEntity.IsDeleted = true;
                softDeleteEntity.DeletedAt = now;
                softDeleteEntity.DeletedBy = currentUser;
            }
        }
    }
}