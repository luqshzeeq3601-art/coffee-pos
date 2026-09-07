# Multi-stage build for Coffee POS .NET 8 ApiHost
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files and restore dependencies
COPY ["src/CoffeePos.Domain/CoffeePos.Domain.csproj", "src/CoffeePos.Domain/"]
COPY ["src/CoffeePos.Application/CoffeePos.Application.csproj", "src/CoffeePos.Application/"]
COPY ["src/CoffeePos.Infrastructure/CoffeePos.Infrastructure.csproj", "src/CoffeePos.Infrastructure/"]
COPY ["src/CoffeePos.ApiHost/CoffeePos.ApiHost.csproj", "src/CoffeePos.ApiHost/"]

RUN dotnet restore "src/CoffeePos.ApiHost/CoffeePos.ApiHost.csproj"

# Copy remaining source code and publish release binary
COPY src/ src/
WORKDIR "/src/src/CoffeePos.ApiHost"
RUN dotnet publish "CoffeePos.ApiHost.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final
WORKDIR /app
EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "CoffeePos.ApiHost.dll"]
