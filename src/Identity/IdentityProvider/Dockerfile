FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
ARG UTCONNECT_REGISTRY_USERNAME
ARG UTCONNECT_REGISTRY_PAT
WORKDIR /src
COPY ["nuget.config", "nuget.config"]
COPY ["src/Identity/IdentityProvider/IdentityProvider.csproj", "src/Identity/IdentityProvider/"]
COPY ["src/Identity/IdentityProvider.Application/IdentityProvider.Application.csproj", "src/Identity/IdentityProvider.Application/"]
COPY ["src/Identity/IdentityProvider.Domain/IdentityProvider.Domain.csproj", "src/Identity/IdentityProvider.Domain/"]
COPY ["src/Identity/IdentityProvider.Infrastructure/IdentityProvider.Infrastructure.csproj", "src/Identity/IdentityProvider.Infrastructure/"]
COPY ["src/Shared/Shared.Application.Localization/Shared.Application.Localization.csproj", "src/Shared/Shared.Application.Localization/"]
COPY ["src/Shared/Shared.Authentication/Shared.Authentication.csproj", "src/Shared/Shared.Authentication/"]
COPY ["src/Oidc/Oidc.Domain/Oidc.Domain.csproj", "src/Oidc/Oidc.Domain/"]
COPY ["src/Shared/Shared.Application.Configuration/Shared.Application.Configuration.csproj", "src/Shared/Shared.Application.Configuration/"]
COPY ["src/Shared/Shared.Infrastructure.Email/Shared.Infrastructure.Email.csproj", "src/Shared/Shared.Infrastructure.Email/"]
COPY ["src/Shared/Shared.Swashbuckle/Shared.Swashbuckle.csproj", "src/Shared/Shared.Swashbuckle/"]
RUN dotnet nuget update source utconnect --username $UTCONNECT_REGISTRY_USERNAME --password $UTCONNECT_REGISTRY_PAT --store-password-in-clear-text
RUN dotnet restore "src/Identity/IdentityProvider/IdentityProvider.csproj"
COPY . .
WORKDIR "/src/src/Identity/IdentityProvider"
RUN dotnet build "IdentityProvider.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "IdentityProvider.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IdentityProvider.dll"]
