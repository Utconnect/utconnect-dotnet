FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Home/Home.Presentation/Home.Presentation.csproj", "src/Home/Home.Presentation/"]
COPY ["src/Home/Home.Application/Home.Application.csproj", "src/Home/Home.Application/"]
COPY ["src/Oidc/Oidc.Domain/Oidc.Domain.csproj", "src/Oidc/Oidc.Domain/"]
COPY ["src/Shared/Shared.Application.Configuration/Shared.Application.Configuration.csproj", "src/Shared/Shared.Application.Configuration/"]
COPY ["src/Shared/Shared.Authentication/Shared.Authentication.csproj", "src/Shared/Shared.Authentication/"]
COPY ["src/Identity/IdentityProvider.Domain/IdentityProvider.Domain.csproj", "src/Identity/IdentityProvider.Domain/"]
COPY ["src/Shared/Shared.UtconnectIdentity/Shared.UtconnectIdentity.csproj", "src/Shared/Shared.UtconnectIdentity/"]
COPY ["src/Shared/Shared.Application.Extensions/Shared.Application.Extensions.csproj", "src/Shared/Shared.Application.Extensions/"]
COPY ["src/Shared/Shared.Application.Exceptions/Shared.Application.Exceptions.csproj", "src/Shared/Shared.Application.Exceptions/"]
COPY ["src/Shared/Shared.Logging/Shared.Logging.csproj", "src/Shared/Shared.Logging/"]
COPY ["src/Shared/Shared.Infrastructure.Db/Shared.Infrastructure.Db.csproj", "src/Shared/Shared.Infrastructure.Db/"]
COPY ["src/Shared/Shared.Services/Shared.Services.csproj", "src/Shared/Shared.Services/"]
COPY ["src/Shared/Shared.Application.MediatR/Shared.Application.MediatR.csproj", "src/Shared/Shared.Application.MediatR/"]
COPY ["src/Shared/Shared.Application.Localization/Shared.Application.Localization.csproj", "src/Shared/Shared.Application.Localization/"]
RUN dotnet restore "src/Home/Home.Presentation/Home.Presentation.csproj"
COPY . .
WORKDIR "/src/src/Home/Home.Presentation"
RUN dotnet build "Home.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Home.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Home.Presentation.dll"]
