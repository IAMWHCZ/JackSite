FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["core/JackSite.Authentication.UI/JackSite.Authentication.UI.csproj", "core/JackSite.Authentication.UI/"]
RUN dotnet restore "core/JackSite.Authentication.UI/JackSite.Authentication.UI.csproj"
COPY . .
WORKDIR "/src/core/JackSite.Authentication.UI"
RUN dotnet build "./JackSite.Authentication.UI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./JackSite.Authentication.UI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JackSite.Authentication.UI.dll"]
