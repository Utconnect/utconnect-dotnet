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
COPY ["src/Gateway/Gateway.Presentation/Gateway.Presentation.csproj", "src/Gateway/Gateway.Presentation/"]
RUN dotnet nuget update source utconnect --username $UTCONNECT_REGISTRY_USERNAME --password $UTCONNECT_REGISTRY_PAT --store-password-in-clear-text
RUN dotnet restore "src/Gateway/Gateway.Presentation/Gateway.Presentation.csproj"
COPY . .
WORKDIR "/src/src/Gateway/Gateway.Presentation"
RUN dotnet build "Gateway.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Gateway.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Gateway.Presentation.dll"]
