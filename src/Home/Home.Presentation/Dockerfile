﻿FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
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
COPY ["src/Home/Home.Presentation/Home.Presentation.csproj", "src/Home/Home.Presentation/"]
COPY ["src/Home/Home.Application/Home.Application.csproj", "src/Home/Home.Application/"]
COPY ["src/Oidc/Oidc.Domain/Oidc.Domain.csproj", "src/Oidc/Oidc.Domain/"]
COPY ["src/Shared/Shared.Application.Configuration/Shared.Application.Configuration.csproj", "src/Shared/Shared.Application.Configuration/"]
COPY ["src/Shared/Shared.Authentication/Shared.Authentication.csproj", "src/Shared/Shared.Authentication/"]
COPY ["src/Identity/IdentityProvider.Domain/IdentityProvider.Domain.csproj", "src/Identity/IdentityProvider.Domain/"]
COPY ["src/Shared/Shared.Application.Localization/Shared.Application.Localization.csproj", "src/Shared/Shared.Application.Localization/"]
RUN dotnet nuget update source utconnect --username $UTCONNECT_REGISTRY_USERNAME --password $UTCONNECT_REGISTRY_PAT --store-password-in-clear-text
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
