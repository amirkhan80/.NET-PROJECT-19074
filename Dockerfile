# ----- STEP 1: Build Stage -----
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy only CSPROJ first
COPY SmartServiceHub/SmartServiceHub.csproj ./SmartServiceHub.csproj

# Restore dependencies
RUN dotnet restore "./SmartServiceHub.csproj"

# Copy entire project
COPY SmartServiceHub/ .

# Build app
RUN dotnet publish "SmartServiceHub.csproj" -c Release -o /app/publish

# ----- STEP 2: Run Stage -----
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 7001
ENTRYPOINT ["dotnet", "SmartServiceHub.dll"]
