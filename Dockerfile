# Dockerfile at repo root (same level as SmartServiceHub folder)

# 1) Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# copy csproj (use relative path to where csproj actually is)
COPY ["SmartServiceHub/SmartServiceHub.csproj", "SmartServiceHub/"]
RUN dotnet restore "SmartServiceHub/SmartServiceHub.csproj"

# copy everything
COPY . .
WORKDIR /src/SmartServiceHub
RUN dotnet publish "SmartServiceHub.csproj" -c Release -o /app/publish

# 2) Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "SmartServiceHub.dll"]
