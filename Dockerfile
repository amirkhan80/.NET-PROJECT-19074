# Use official dotnet SDK image for build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# -- COPY csproj from subfolder to enable restore
# Adjust path if your .csproj is at SmartServiceHub/SmartServiceHub.csproj
COPY ["SmartServiceHub/SmartServiceHub.csproj", "./"]

# restore
RUN dotnet restore "./SmartServiceHub.csproj"

# copy everything and build/publish
COPY SmartServiceHub/ ./     # copy project folder content into /src
RUN dotnet publish "./SmartServiceHub.csproj" -c Release -o /app/publish

# runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet","SmartServiceHub.dll"]
