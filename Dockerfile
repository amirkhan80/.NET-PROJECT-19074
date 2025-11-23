# Stage 1 - Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy only the project file
COPY SmartServiceHub/SmartServiceHub.csproj ./SmartServiceHub.csproj

RUN dotnet restore "SmartServiceHub.csproj"

# Copy entire code
COPY SmartServiceHub/ .

RUN dotnet publish "SmartServiceHub.csproj" -c Release -o /app/publish

# Stage 2 - Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "SmartServiceHub.dll"]
