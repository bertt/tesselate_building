# tesselate_building

Console tool for creating a LOD1.2 triangulated polyhedralsurface from (building) footprint and height value. Colors per triangle are written to the 'colors' column. Buildings can have multiple storeys.

The buildings consist of 3 parts:

- Floor: Drawn at z=0, tesselated footprint geometry;

- Roof: Drawn at z=height, tesselated footprint geometry

- Walls: Vertically drawn triangles. As input is used the footprint vertices and from/to values per storey/building  

Parts can be made invisible by using transparent colors.

This tool is designed to create the correct input information for creating 3D tiles with pg2b3dm (https://github.com/Geodan/pg2b3dm)

## Samples

Visualizing buildings in Delaware

![delaware](https://user-images.githubusercontent.com/538812/73743668-6292d580-474f-11ea-9b4f-d64549a3a77b.png)

Visualizing subsurface layers in MapBox GL. Floor and roof parts are made transparent.

![2020-02-04](https://user-images.githubusercontent.com/538812/73743253-696d1880-474e-11ea-8c4d-8983d65a1b62.png)

## Running 

Image on Docker hub: https://hub.docker.com/repository/docker/bertt/tesselate_building

Run app in Docker:

```
$ docker run -it bertt/tesselate_building -U postgres -d postgres -t delaware_buildings -i geom_3857 -o geom_triangle_3857 --idcolumn ogc_fid --stylecolumn style --colorscolumn colors
```

Building Docker image:

```
$ docker build -t bertt/tesselate_building .
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

  --heightcolumn                (Default: height) height columndocker run -v $(pwd)/output:/app/output -it --network mynetwork geodan/pg2b3dm -h some-postgis -U postgres -c geom_triangle_3857 -t delaware_buildings -d postgres -i id

  --idcolumn                    (Default: id) Id column

  --stylecolumn                 (Default: style) Style column

  --colorscolumn                (Default: colors) Colors column 

  --help                        Display this help screen.

  --version                     Display version information.
  ```

Expected types of columns:

- idcolumn : type int

- stylecolumn: type json

- geom3d: type geometry

- geom: type geometry

- colors: type string[]


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
