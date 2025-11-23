# Dockerfile for SmartServiceHub (.NET 9)
# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# copy csproj and restore first for caching
COPY ["SmartServiceHub.csproj", "./"]
RUN dotnet restore "SmartServiceHub.csproj"

# copy everything and publish
COPY . .
RUN dotnet publish "SmartServiceHub.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# create non-root user (security)
RUN useradd -ms /bin/bash appuser
USER appuser

# copy published app
COPY --from=build /app/publish .

# Expose port
EXPOSE 8080

# Environment: Render forwards PORT variable; default fallback to 8080
ENV ASPNETCORE_URLS=http://*:${PORT:-8080}
ENV DOTNET_RUNNING_IN_CONTAINER=true

ENTRYPOINT ["dotnet", "SmartServiceHub.dll"]
