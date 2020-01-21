FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine as build
WORKDIR /src
COPY . .
RUN dotnet build "tesselate_building_sample_console/tesselate_building_sample_console.csproj" -c Release
RUN dotnet publish "tesselate_building_sample_console/tesselate_building_sample_console.csproj" -c Release -o /app

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-alpine
WORKDIR /app
COPY --from=build /app /app/
ENTRYPOINT ["dotnet", "tesselate_building.dll"]