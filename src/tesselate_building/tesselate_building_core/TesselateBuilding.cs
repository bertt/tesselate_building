using System.Collections.Generic;
using System.IO;
using Wkx;

namespace tesselate_building_core
{
    public static class TesselateBuilding
    {
        public static (PolyhedralSurface polyhedral, List<string> colors) MakeBuilding(Polygon footprint, double fromZ, double height, BuildingStyle buildingStyle)
        {
            var colors = new List<string>();
            var polyhedral = new PolyhedralSurface();
            polyhedral.Dimension = Dimension.Xyz;

            polyhedral.Geometries.Add(GetPolygonZ(footprint, fromZ));
            polyhedral.Geometries.Add(GetPolygonZ(footprint, fromZ + height));
            if (buildingStyle.Storeys == null)
            {
                var walls = MakeWalls(footprint, fromZ, height - fromZ);
                polyhedral.Geometries.AddRange(walls);
            }
            else
            {
                {
                    foreach (var storey in buildingStyle.Storeys)
                    {
                        var walls = MakeWalls(footprint, fromZ + storey.From, storey.To - storey.From);
                        polyhedral.Geometries.AddRange(walls);
                    }
                }
            }

            var stream = new MemoryStream();
            polyhedral.Serialize<WkbSerializer>(stream);
            var wkb = stream.ToArray();
            var triangulatedWkb = Triangulator.Triangulator.Triangulate(wkb);
            var polyhedralNew = (PolyhedralSurface)Geometry.Deserialize<WkbSerializer>(triangulatedWkb);

            for (var i = 0; i < polyhedralNew.Geometries.Count; i++)
            {
                // todo: fix colors
                colors.Add("#0000ff");
            }
            return (polyhedralNew, colors);
        }

        private static Polygon GetPolygonZ(Polygon polygon, double z)
        {
            var newPoints = new List<Point>();
            foreach(var p in polygon.ExteriorRing.Points)
            {
                var newPoint = new Point((double)p.X, (double)p.Y, z);
                newPoints.Add(newPoint);
            }
            var res = new Polygon(newPoints);
            res.Dimension = Dimension.Xyz;
            return res;
        }

        public static List<Polygon> MakeWalls(Polygon footprint, double fromZ, double storeyheight)
        {
            var polygons = new List<Polygon>();
            for(var i = 1; i < footprint.ExteriorRing.Points.Count; i++)
            {
                var p0 = footprint.ExteriorRing.Points[i - 1];
                var p1 = footprint.ExteriorRing.Points[i];

                var t1 = new Polygon();
                t1.Dimension = Dimension.Xyz;

                t1.ExteriorRing.Points.Add(new Point((double)p0.X, (double)p0.Y, fromZ));
                t1.ExteriorRing.Points.Add(new Point((double)p0.X, (double)p0.Y, fromZ+storeyheight));
                t1.ExteriorRing.Points.Add(new Point((double)p1.X, (double)p1.Y, fromZ + storeyheight));
                t1.ExteriorRing.Points.Add(new Point((double)p1.X, (double)p1.Y, fromZ));
                t1.ExteriorRing.Points.Add(new Point((double)p0.X, (double)p0.Y, fromZ));

                polygons.Add(t1);
            }

            return polygons;
        }
    }
}
