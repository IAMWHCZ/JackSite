using JackSite.Common.Domain;

namespace JackSite.UserServer.Entities;

public class User : EntityBase
{
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public bool PhoneNumberConfirmed { get; set; }
    public bool EmailConfirmed { get; set; }
    public string? Avatar { get; set; }
    public string? Nickname { get; set; }
    public string? Bio { get; set; }
    public UserStatus Status { get; set; }
    public DateTime? LastLoginTime { get; set; }
    public string? LastLoginIp { get; set; }
    public int LoginAttempts { get; set; }
    public DateTime? LockoutEnd { get; set; }
    public bool TwoFactorEnabled { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}