FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5002

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["gRPC-Service.csproj", "."]
RUN dotnet restore "gRPC-Service.csproj"
COPY . .
RUN dotnet build "gRPC-Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "gRPC-Service.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "gRPC-Service.dll"]
