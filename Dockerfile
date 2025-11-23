# Stage 1 - Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# COPY correct csproj path
COPY SmartServiceHub/SmartServiceHub.csproj ./SmartServiceHub.csproj

RUN dotnet restore "SmartServiceHub.csproj"

# Now copy full source
COPY SmartServiceHub/ .

RUN dotnet publish "SmartServiceHub.csproj" -c Release -o /app/publish

# Stage 2 - Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "SmartServiceHub.dll"]
