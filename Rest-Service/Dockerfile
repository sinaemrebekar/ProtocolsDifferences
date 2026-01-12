FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5001
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["Rest-Service.csproj", "."]
RUN dotnet restore "Rest-Service.csproj"

COPY . .
RUN dotnet build "Rest-Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Rest-Service.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Rest-Service.dll"]

