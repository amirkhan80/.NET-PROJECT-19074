# build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# copy csproj and restore first (for good cache)
COPY ["SmartServiceHub.csproj", "./"]
RUN dotnet restore "SmartServiceHub.csproj"

# copy everything and publish
COPY . .
RUN dotnet publish "SmartServiceHub.csproj" -c Release -o /app/publish

# runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "SmartServiceHub.dll"]
