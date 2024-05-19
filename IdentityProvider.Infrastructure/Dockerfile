FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["IdentityProvider.Infrastructure/IdentityProvider.Infrastructure.csproj", "IdentityProvider.Infrastructure/"]
RUN dotnet restore "IdentityProvider.Infrastructure/IdentityProvider.Infrastructure.csproj"
COPY . .
WORKDIR "/src/IdentityProvider.Infrastructure"
RUN dotnet build "IdentityProvider.Infrastructure.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "IdentityProvider.Infrastructure.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IdentityProvider.Infrastructure.dll"]
