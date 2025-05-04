using JackSite.Domain.Services;

namespace JackSite.Infrastructure.Services;

public class RoleService(
    IRoleRepository roleRepository,
    IPermissionRepository permissionRepository)
    : IRoleService
{
    public async Task<Role> CreateRoleAsync(string name, string description, CancellationToken cancellationToken = default)
    {
        // 检查角色名是否已存在
        var existingRole = await roleRepository.GetByNameAsync(name, cancellationToken);
        if (existingRole != null)
        {
            throw new InvalidOperationException($"Role name '{name}' is already taken.");
        }

        // 创建新角色
        var role = new Role(name, description);
        return await roleRepository.AddAsync(role, cancellationToken);
    }

    public async Task<bool> UpdateRoleAsync(long roleId, string name, string description, CancellationToken cancellationToken = default)
    {
        var role = await roleRepository.GetByIdAsync(roleId, cancellationToken);
        if (role == null)
        {
            return false;
        }

        // 如果角色名已更改，检查新名称是否已存在
        if (role.Name != name)
        {
            var existingRole = await roleRepository.GetByNameAsync(name, cancellationToken);
            if (existingRole != null && existingRole.Id != roleId)
            {
                throw new InvalidOperationException($"Role name '{name}' is already taken.");
            }
        }
        
        // 更新角色
        role.UpdateName(name);
        role.UpdateDescription(description);
        await roleRepository.UpdateAsync(role, cancellationToken);
        return true;
    }

    public async Task<bool> DeleteRoleAsync(long roleId, CancellationToken cancellationToken = default)
    {
        var role = await roleRepository.GetByIdAsync(roleId, cancellationToken);
        if (role == null)
        {
            return false;
        }

        await roleRepository.DeleteAsync(role, cancellationToken);
        return true;
    }

    public async Task<Role?> GetRoleByIdAsync(long roleId, CancellationToken cancellationToken = default)
    {
        return await roleRepository.GetByIdAsync(roleId, cancellationToken);
    }

    public async Task<Role?> GetRoleByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await roleRepository.GetByNameAsync(name, cancellationToken);
    }

    public async Task<IEnumerable<Role>> GetAllRolesAsync(CancellationToken cancellationToken = default)
    {
        return await roleRepository.GetAllAsync(cancellationToken);
    }

    public async Task<bool> AddPermissionToRoleAsync(long roleId, long permissionId, CancellationToken cancellationToken = default)
    {
        // 检查角色和权限是否存在
        var role = await roleRepository.GetByIdAsync(roleId, cancellationToken);
        if (role == null)
        {
            return false;
        }

        var permission = await permissionRepository.GetByIdAsync(permissionId, cancellationToken);
        if (permission == null)
        {
            return false;
        }

        // 添加权限到角色
        await roleRepository.AddPermissionToRoleAsync(roleId, permissionId, cancellationToken);
        return true;
    }

    public async Task<bool> RemovePermissionFromRoleAsync(long roleId, long permissionId, CancellationToken cancellationToken = default)
    {
        // 检查角色和权限是否存在
        var role = await roleRepository.GetByIdAsync(roleId, cancellationToken);
        if (role == null)
        {
            return false;
        }

        var permission = await permissionRepository.GetByIdAsync(permissionId, cancellationToken);
        if (permission == null)
        {
            return false;
        }

        // 从角色中移除权限
        await roleRepository.RemovePermissionFromRoleAsync(roleId, permissionId, cancellationToken);
        return true;
    }

    public async Task<IEnumerable<Permission>> GetRolePermissionsAsync(long roleId, CancellationToken cancellationToken = default)
    {
        return await roleRepository.GetRolePermissionsAsync(roleId, cancellationToken);
    }

    public async Task<IEnumerable<UserBasic>> GetRoleUsersAsync(long roleId, CancellationToken cancellationToken = default)
    {
        return await roleRepository.GetRoleUsersAsync(roleId, cancellationToken);
    }
}