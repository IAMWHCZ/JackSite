FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["services/users/JackSite.User.WebAPI/JackSite.User.WebAPI.csproj", "services/users/JackSite.User.WebAPI/"]
RUN dotnet restore "services/users/JackSite.User.WebAPI/JackSite.User.WebAPI.csproj"
COPY . .
WORKDIR "/src/services/users/JackSite.User.WebAPI"
RUN dotnet build "./JackSite.User.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./JackSite.User.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JackSite.User.WebAPI.dll"]
