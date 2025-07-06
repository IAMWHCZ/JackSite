FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["apps/gateway/JackSite.Gateway/JackSite.Gateway.csproj", "apps/gateway/JackSite.Gateway/"]
RUN dotnet restore "apps/gateway/JackSite.Gateway/JackSite.Gateway.csproj"
COPY . .
WORKDIR "/src/apps/gateway/JackSite.Gateway"
RUN dotnet build "./JackSite.Gateway.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./JackSite.Gateway.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JackSite.Gateway.dll"]
