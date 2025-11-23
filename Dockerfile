# Stage 1: build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# copy csproj and restore
COPY *.sln ./
COPY SmartServiceHub/*.csproj ./SmartServiceHub/
RUN dotnet restore

# copy everything and publish
COPY . .
WORKDIR /src/SmartServiceHub
RUN dotnet publish -c Release -o /app/publish

# Stage 2: runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "SmartServiceHub.dll"]
