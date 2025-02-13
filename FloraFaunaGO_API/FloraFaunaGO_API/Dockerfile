FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /FloraFaunaGO_API
COPY . .

WORKDIR /FloraFaunaGO_API/FloraFaunaGO_API
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "FloraFaunaGO_API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080
EXPOSE 8081

ENTRYPOINT ["dotnet", "FloraFaunaGO_API.dll"]