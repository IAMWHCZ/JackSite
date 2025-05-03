using JackSite.Domain.Entities;
using JackSite.Domain.Services;

namespace JackSite.Infrastructure.Services;

public class PermissionService(IPermissionRepository permissionRepository) : IPermissionService
{
    public async Task<Permission> CreatePermissionAsync(string name, string code, string description, CancellationToken cancellationToken = default)
    {
        // 检查权限代码是否已存在
        var existingPermission = await permissionRepository.GetByCodeAsync(code, cancellationToken);
        if (existingPermission != null)
        {
            throw new InvalidOperationException($"Permission code '{code}' is already taken.");
        }

        // 创建新权限
        var permission = new Permission
        {
            Name = name,
            Code = code,
            Description = description
        };
        
        return await permissionRepository.AddAsync(permission, cancellationToken);
    }

    public async Task<bool> UpdatePermissionAsync(long permissionId, string name, string code, string description, CancellationToken cancellationToken = default)
    {
        var permission = await permissionRepository.GetByIdAsync(permissionId, cancellationToken);
        if (permission == null)
        {
            return false;
        }

        // 如果权限代码已更改，检查新代码是否已存在
        if (permission.Code != code)
        {
            var existingPermission = await permissionRepository.GetByCodeAsync(code, cancellationToken);
            if (existingPermission != null && existingPermission.Id != permissionId)
            {
                throw new InvalidOperationException($"Permission code '{code}' is already taken.");
            }
        }
        
        // 更新权限
        permission.Name = name;
        permission.Code = code;
        permission.Description = description;
        
        await permissionRepository.UpdateAsync(permission, cancellationToken);
        return true;
    }

    public async Task<bool> DeletePermissionAsync(long permissionId, CancellationToken cancellationToken = default)
    {
        var permission = await permissionRepository.GetByIdAsync(permissionId, cancellationToken);
        if (permission == null)
        {
            return false;
        }

        await permissionRepository.DeleteAsync(permission, cancellationToken);
        return true;
    }

    public async Task<Permission?> GetPermissionByIdAsync(long permissionId, CancellationToken cancellationToken = default)
    {
        return await permissionRepository.GetByIdAsync(permissionId, cancellationToken);
    }

    public async Task<Permission?> GetPermissionByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        return await permissionRepository.GetByCodeAsync(code, cancellationToken);
    }

    public async Task<IEnumerable<Permission>> GetAllPermissionsAsync(CancellationToken cancellationToken = default)
    {
        return await permissionRepository.GetAllAsync(cancellationToken);
    }

    public async Task<IEnumerable<Role>> GetPermissionRolesAsync(long permissionId, CancellationToken cancellationToken = default)
    {
        return await permissionRepository.GetPermissionRolesAsync(permissionId, cancellationToken);
    }
}