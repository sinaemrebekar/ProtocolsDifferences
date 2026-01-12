FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5000
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Proje dosyasını kopyala
COPY ["Orchestrator.csproj", "."]
RUN dotnet restore "Orchestrator.csproj"

# Tüm dosyaları kopyala
COPY . .
RUN dotnet build "Orchestrator.csproj" -c Release -o /app/build

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/build .
ENTRYPOINT ["dotnet", "Orchestrator.dll"]
