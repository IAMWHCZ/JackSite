FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Gateways/JackSite.YarpApi.Gateway/JackSite.YarpApi.Gateway.csproj", "src/Gateways/JackSite.YarpApi.Gateway/"]
RUN dotnet restore "src/Gateways/JackSite.YarpApi.Gateway/JackSite.YarpApi.Gateway.csproj"
COPY . .
WORKDIR "/src/src/Gateways/JackSite.YarpApi.Gateway"
RUN dotnet build "JackSite.YarpApi.Gateway.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "JackSite.YarpApi.Gateway.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JackSite.YarpApi.Gateway.dll"]
