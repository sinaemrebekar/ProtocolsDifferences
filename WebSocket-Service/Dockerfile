FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5003
EXPOSE 8083

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["WebSocket-Service.csproj", "."]
RUN dotnet restore "WebSocket-Service.csproj"

COPY . .
RUN dotnet build "WebSocket-Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebSocket-Service.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebSocket-Service.dll"]

