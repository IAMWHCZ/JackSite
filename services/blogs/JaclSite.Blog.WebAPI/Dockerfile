FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["services/blogs/JaclSite.Blog.WebAPI/JaclSite.Blog.WebAPI.csproj", "services/blogs/JaclSite.Blog.WebAPI/"]
RUN dotnet restore "services/blogs/JaclSite.Blog.WebAPI/JaclSite.Blog.WebAPI.csproj"
COPY . .
WORKDIR "/src/services/blogs/JaclSite.Blog.WebAPI"
RUN dotnet build "./JaclSite.Blog.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./JaclSite.Blog.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JaclSite.Blog.WebAPI.dll"]
