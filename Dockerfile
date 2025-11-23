# use official microsoft images
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# copy csproj which is inside subfolder in repo
COPY ["SmartServiceHub/SmartServiceHub.csproj", "./"]
RUN dotnet restore "SmartServiceHub.csproj"

# copy everything and build
COPY . .
WORKDIR /src
RUN dotnet publish "SmartServiceHub.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "SmartServiceHub.dll"]
