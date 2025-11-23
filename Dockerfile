# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# COPY correct .csproj location
COPY ["SmartServiceHub/SmartServiceHub.csproj", "./"]

RUN dotnet restore "SmartServiceHub.csproj"

# Copy everything
COPY . .

RUN dotnet publish "SmartServiceHub.csproj" -c Release -o /app/publish

# Stage 2: Run
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "SmartServiceHub.dll"]
