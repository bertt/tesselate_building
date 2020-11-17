FROM mcr.microsoft.com/dotnet/sdk:5.0 as build
WORKDIR /src
COPY . .
RUN dotnet build "tesselate_building_sample_console/tesselate_building_sample_console.csproj" -c Release
RUN dotnet publish "tesselate_building_sample_console/tesselate_building_sample_console.csproj" -c Release -o /app

FROM mcr.microsoft.com/dotnet/runtime:5.0
WORKDIR /app
COPY --from=build /app /app/
ENTRYPOINT ["dotnet", "tesselate_building.dll"]