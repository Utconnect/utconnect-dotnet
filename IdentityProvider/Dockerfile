FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["IdentityProvider/IdentityProvider.csproj", "IdentityProvider/"]
COPY ["IdentityProvider.Application/IdentityProvider.Application.csproj", "IdentityProvider.Application/"]
COPY ["Oidc.Domain/Oidc.Domain.csproj", "Oidc.Domain/"]
COPY ["Shared.Application.Configuration/Shared.Application.Configuration.csproj", "Shared.Application.Configuration/"]
COPY ["Shared.Authentication/Shared.Authentication.csproj", "Shared.Authentication/"]
COPY ["IdentityProvider.Domain/IdentityProvider.Domain.csproj", "IdentityProvider.Domain/"]
COPY ["Shared.UtconnectIdentity/Shared.UtconnectIdentity.csproj", "Shared.UtconnectIdentity/"]
COPY ["Shared.Application.Extensions/Shared.Application.Extensions.csproj", "Shared.Application.Extensions/"]
COPY ["Shared.Application.Exceptions/Shared.Application.Exceptions.csproj", "Shared.Application.Exceptions/"]
COPY ["Shared.Logging/Shared.Logging.csproj", "Shared.Logging/"]
COPY ["Shared.Infrastructure.Db/Shared.Infrastructure.Db.csproj", "Shared.Infrastructure.Db/"]
COPY ["Shared.Services/Shared.Services.csproj", "Shared.Services/"]
COPY ["IdentityProvider.Infrastructure/IdentityProvider.Infrastructure.csproj", "IdentityProvider.Infrastructure/"]
COPY ["Shared.Application.Localization/Shared.Application.Localization.csproj", "Shared.Application.Localization/"]
COPY ["Shared.Http/Shared.Http.csproj", "Shared.Http/"]
COPY ["Shared.Presentation/Shared.Presentation.csproj", "Shared.Presentation/"]
COPY ["Shared.Swashbuckle/Shared.Swashbuckle.csproj", "Shared.Swashbuckle/"]
RUN dotnet restore "IdentityProvider/IdentityProvider.csproj"
COPY . .
WORKDIR "/src/IdentityProvider"
RUN dotnet build "IdentityProvider.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "IdentityProvider.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IdentityProvider.dll"]
