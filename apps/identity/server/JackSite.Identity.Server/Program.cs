using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using JackSite.Identity.Server.Data;
using JackSite.Identity.Server.Models;
using JackSite.Identity.Server.Services;
using JackSite.Identity.Server.Services.BackgroundServices;
using Microsoft.AspNetCore.DataProtection;
using JackSite.Identity.Server.Extensions;
using JackSite.Identity.Server.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configure DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("IdentityServerDb")));

// Configure HttpClient for external user service
builder.Services.AddHttpClient<IExternalUserService, ExternalUserService>();
builder.Services.AddHttpClient<ISmsService, SmsService>();

// Configure Data Protection
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(builder.Configuration["DataProtection:KeysDirectory"] ?? Path.Combine(Directory.GetCurrentDirectory(), "keys")));

// Register services
builder.Services.AddScoped<IExternalUserService, ExternalUserService>();
builder.Services.AddScoped<IUserStore<ApplicationUser>, ExternalUserStore>();
builder.Services.AddScoped<IPasswordValidator<ApplicationUser>, ExternalPasswordValidator>();
builder.Services.AddScoped<IUserSyncService, UserSyncService>();
builder.Services.AddScoped<IEmailService, EmailService>();

// Register background services
builder.Services.AddHostedService<UserSyncBackgroundService>();

// Configure Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Password settings from configuration
    var passwordPolicy = builder.Configuration.GetSection("PasswordPolicy");
    options.Password.RequiredLength = passwordPolicy.GetValue<int>("MinLength");
    options.Password.RequireUppercase = passwordPolicy.GetValue<bool>("RequireUppercase");
    options.Password.RequireLowercase = passwordPolicy.GetValue<bool>("RequireLowercase");
    options.Password.RequireDigit = passwordPolicy.GetValue<bool>("RequireDigit");
    options.Password.RequireNonAlphanumeric = passwordPolicy.GetValue<bool>("RequireSpecialChar");

    // Lockout settings
    var lockoutPolicy = builder.Configuration.GetSection("AccountLockout");
    options.Lockout.MaxFailedAccessAttempts = lockoutPolicy.GetValue<int>("MaxFailedAttempts");
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(lockoutPolicy.GetValue<int>("LockoutMinutes"));
})
.AddDefaultTokenProviders();

// Configure JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
})
.AddSocialLogins(builder.Configuration);

// Add Redis cache
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});

// Register services
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserService, UserService>();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("DefaultPolicy", policy =>
    {
        policy.WithOrigins(builder.Configuration.GetSection("AllowedOrigins").Get<string[]>())
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{

}

app.UseHttpsRedirection();
app.UseCors("DefaultPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();