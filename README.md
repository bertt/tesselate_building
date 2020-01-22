# tesselate_building

Library for creating a LOD1.2 triangulated polyhedralsurface from (building) footprint and height value. Colors per triangle are written to the 'colors' column.

This tool is designed to create the correct input information for creating 3D tiles with pg2b3dm (https://github.com/Geodan/pg2b3dm)

## Sample code

```csharp
wktFootprint = "POLYGON((-75.55478134 39.1632752950001,-75.55477116 39.163235817,-75.554760981 39.1631963390001,-75.554818218 39.163187394,-75.5548754549999 39.16317845,-75.5548856349999 39.1632179280001,-75.554896589 39.1632604100001,-75.554724403 39.163285407,-75.554724102 39.1632842400001,-75.55478134 39.1632752950001))";
footprint = (Polygon)Geometry.Deserialize<WktSerializer>(wktFootprint);
height = 11.55;

var bs = new BuildingStyle() { FloorColor = "#D3D3D3", RoofColor = "#ff0000", WallsColor = "#00ff00" };
var res = TesselateBuilding.MakePolyHedral(footprint, height, bs);

var wkt = res.polyhedral.SerializeString<WktSerializer>();
Assert.IsTrue(wkt == "POLYHEDRALSURFACE(((-75.55478134 39.1632752950001 0,-75.554724102 39.1632842400001 0,-75.554724403 39.163285407 0,-75.55478134 39.1632752950001 0)) ... ((-75.554724102 39.1632842400001 11.55,-75.55478134 39.1632752950001 11.55,-75.55478134 39.1632752950001 0,-75.554724102 39.1632842400001 11.55)))");
Assert.IsTrue(res.colors.Count == 20);
```

## Building Styling

For building styling a json column is used.

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
  "walls": "#00ff00",
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

## Sample application

For application see 'tesselate_building_sample_console', reads footprint polygons/heights from PostGIS database and writes polysurfacehedral geometries.

## Docker 

Image on Docker hub: https://hub.docker.com/repository/docker/bertt/tesselate_building

Run app in Docker:

```
$ docker run -it bertt/tesselate_building -U postgres -d postgres -t delaware_buildings -i geom_3857 -o geom_triangle_3857 --idcolumn ogc_fid --stylecolumn style --colorscolumn colors
```

Build sample application in Docker:

```
$ docker build -t bertt/tesselate_building .
```
