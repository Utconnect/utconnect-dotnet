FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Oidc/Oidc.Presentation/Oidc.Presentation.csproj", "src/Oidc/Oidc.Presentation/"]
COPY ["src/Oidc/Oidc.Application/Oidc.Application.csproj", "src/Oidc/Oidc.Application/"]
COPY ["src/Shared/Shared.Application.MediatR/Shared.Application.MediatR.csproj", "src/Shared/Shared.Application.MediatR/"]
COPY ["src/Oidc/Oidc.Infrastructure/Oidc.Infrastructure.csproj", "src/Oidc/Oidc.Infrastructure/"]
COPY ["src/Shared/Shared.Authentication/Shared.Authentication.csproj", "src/Shared/Shared.Authentication/"]
COPY ["src/Identity/IdentityProvider.Domain/IdentityProvider.Domain.csproj", "src/Identity/IdentityProvider.Domain/"]
COPY ["src/Shared/Shared.UtconnectIdentity/Shared.UtconnectIdentity.csproj", "src/Shared/Shared.UtconnectIdentity/"]
COPY ["src/Shared/Shared.Application.Extensions/Shared.Application.Extensions.csproj", "src/Shared/Shared.Application.Extensions/"]
COPY ["src/Shared/Shared.Application.Exceptions/Shared.Application.Exceptions.csproj", "src/Shared/Shared.Application.Exceptions/"]
COPY ["src/Shared/Shared.Logging/Shared.Logging.csproj", "src/Shared/Shared.Logging/"]
COPY ["src/Shared/Shared.Infrastructure.Db/Shared.Infrastructure.Db.csproj", "src/Shared/Shared.Infrastructure.Db/"]
COPY ["src/Shared/Shared.Services/Shared.Services.csproj", "src/Shared/Shared.Services/"]
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
