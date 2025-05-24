# 数据库迁移命令

## 安装 EF Core 工具
```bash
dotnet tool install --global dotnet-ef
```

## 删除数据库迁移
```bash
# 在 JackSite.Infrastructure 项目目录下运行
dotnet ef database drop --startup-project ../JackSite.Http/JackSite.Http.csproj
```

## 创建迁移
```bash
# 在 JackSite.Infrastructure 项目目录下运行
dotnet ef migrations add InitialCreate --startup-project ../JackSite.Http/JackSite.Http.csproj
```

## 更新数据库
```bash
# 在 JackSite.Infrastructure 项目目录下运行
dotnet ef database update --startup-project ../JackSite.Http/JackSite.Http.csproj
```

## 生成迁移脚本
```bash
# 在 JackSite.Infrastructure 项目目录下运行
dotnet ef migrations script --startup-project ../JackSite.Http/JackSite.Http.csproj
```

## 移除最后一次迁移
```bash
# 在 JackSite.Infrastructure 项目目录下运行
dotnet ef migrations remove --startup-project ../JackSite.Http/JackSite.Http.csproj
```

## 列出所有迁移
```bash
# 在 JackSite.Infrastructure 项目目录下运行
dotnet ef migrations list --startup-project ../JackSite.Http/JackSite.Http.csproj
```