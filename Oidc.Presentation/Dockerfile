FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Oidc.Presentation/Oidc.Presentation.csproj", "Oidc.Presentation/"]
COPY ["Oidc.Infrastructure/Oidc.Infrastructure.csproj", "Oidc.Infrastructure/"]
COPY ["Oidc.Application/Oidc.Application.csproj", "Oidc.Application/"]
COPY ["Shared.Application.MediatR/Shared.Application.MediatR.csproj", "Shared.Application.MediatR/"]
RUN dotnet restore "Oidc.Presentation/Oidc.Presentation.csproj"
COPY . .
WORKDIR "/src/Oidc.Presentation"
RUN dotnet build "Oidc.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Oidc.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Oidc.Presentation.dll"]
