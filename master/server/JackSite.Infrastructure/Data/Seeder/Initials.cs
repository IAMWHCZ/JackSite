using JackSite.Domain.Entities.Permissions;
using Role = JackSite.Domain.Entities.Roles.Role;

namespace JackSite.Infrastructure.Data.Seeder;

public class Initials
{
    public static IReadOnlyList<UserBasic> AddUserBasics(string salt) =>
    [
        new("administrator","2545481217@qq.com","Cz18972621866",salt)
    ];

    public static IReadOnlyList<Role> AddRoles() =>
    [
        new("Super", "系统超级管理员"),
        new("Admin", "系统管理员")
    ];
    
    public static IReadOnlyList<Permission> AddPermissions() =>
    [
        new("Super", "Super"),
        new("Admin", "Admin")
    ];
}