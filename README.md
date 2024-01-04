# tesselate_building

Console tool for creating triangulated polyhedralsurface from (building) footprint and height value. Shaders per triangle are written to the 'shaders' column. Buildings can have multiple storeys.

This tool is designed to create the correct input information for creating 3D tiles with pg2b3dm (https://github.com/Geodan/pg2b3dm). This tool is used in the pg2b3dm 'getting started' sample see https://github.com/Geodan/pg2b3dm/blob/master/getting_started.md

## Installation


Prerequisite: .NET 6.0 SDK is installed https://dotnet.microsoft.com/download/dotnet/6.0

```
$ dotnet tool install -g tesselate_building
```

Or update

```
$ dotnet tool update -g tesselate_building
```


## Running

```
$ tesselate_building -U postgres -h localhost -d research -t bro.geotop3d
```

## command line options

All parameters are optional, except the -t --table option.

If --username and/or --dbname are not specified the current username is used as default.

```
 -U, --username                Database user

  -h, --host                    (Default: localhost) Database host

  -d, --dbname                  Database name

  -p, --port                    (Default: 5432) Database port

  -t, --table                   Required. Database table, include database schema if needed

  -i, --inputgeometrycolumn     (Default: geom) Input geometry column

  -o, --outputgeometrycolumn    (Default: geom3d) Output geometry column

  -f, --format                  (Default: mapbox) Output format mapbox/cesium

  --heightcolumn                (Default: height) height column

  --idcolumn                    (Default: id) Id column

  --stylecolumn                 (Default: style) Style column

  --shaderscolumn                (Default: shaders) Shaders column 

  --help                        Display this help screen.

  --version                     Display version information.
  ```

## Building Styling

For building styling a json column is used. Please note the the json keys are case sensitive. 

Simple content (without storeys):

```
{
  "walls": "#00ff00",
  "roof": " #ff0000",
  "floor": "#D3D3D3",
}
```

Style content with storeys:

```
{
  "roof": " #ff0000",
  "floor": "#D3D3D3",
  "storeys": [
    {
      "from": 0,
      "to": 0.5,
      "color": "#D3D3D3"
    },
    {
      "from": 0.5,
      "to": 1,
      "color": "#D3SS3D3"
    },
    {
      "from": 1,
      "to": 1.5,
      "color": "#D354S3D3"
    }
  ]
}
```

If both element 'walls' and 'storeys' are defined the 'walls' property will be used.

## Docker 

Image on Docker hub: https://hub.docker.com/repository/docker/bertt/tesselate_building

Run app in Docker:

```
$ docker run -it bertt/tesselate_building -U postgres -d postgres -t delaware_buildings -f mapbox -i geom -o geom_triangle --idcolumn ogc_fid --stylecolumn style --shaderscolumn shaders
```

Build sample application in Docker:

```
$ docker build -t bertt/tesselate_building .
```

## Dependencies

- CommandLineParser https://github.com/commandlineparser/commandline
- Dapper https://github.com/StackExchange/Dapper
- Npgsql https://github.com/npgsql/npgsql
- Wkx https://github.com/cschwarz/wkx-sharp
- Triangulator https://github.com/bertt/triangulator

## History

2023-01-04: release 0.3.0, make compatible with pg2b3dm 2.0.0

2023-09-26: release 0.2.2, fix normals

2023-08-18: release 0.2.1, set MetallicRoughness to 0 (gives white buildings in Cesium)

2022-01-24: release 0.2 to from .NET 5 to .NET 6
