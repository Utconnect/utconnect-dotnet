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
COPY ["src/Oidc/Oidc.Presentation/Oidc.Presentation.csproj", "src/Oidc/Oidc.Presentation/"]
COPY ["src/Oidc/Oidc.Application/Oidc.Application.csproj", "src/Oidc/Oidc.Application/"]
COPY ["src/Oidc/Oidc.Infrastructure/Oidc.Infrastructure.csproj", "src/Oidc/Oidc.Infrastructure/"]
COPY ["src/Shared/Shared.Authentication/Shared.Authentication.csproj", "src/Shared/Shared.Authentication/"]
COPY ["src/Identity/IdentityProvider.Domain/IdentityProvider.Domain.csproj", "src/Identity/IdentityProvider.Domain/"]
RUN dotnet nuget update source utconnect --username $UTCONNECT_REGISTRY_USERNAME --password $UTCONNECT_REGISTRY_PAT --store-password-in-clear-text
RUN dotnet restore "src/Oidc/Oidc.Presentation/Oidc.Presentation.csproj"
COPY . .
WORKDIR "/src/src/Oidc/Oidc.Presentation"
RUN dotnet build "Oidc.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Oidc.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Oidc.Presentation.dll"]
