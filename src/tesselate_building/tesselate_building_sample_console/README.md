# tesselate_building

Tool to tesselate buildings (footprint polygon, height) into 3D polysurfacehedral geometries.

Sample run:

```
$ tesselate_building -U postgres -h leda -d research -t bro.geotop3d
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

  --heightcolumn                (Default: height) height column

  --idcolumn                    (Default: id) Id column

  --help                        Display this help screen.

  --version                     Display version information.
  ```