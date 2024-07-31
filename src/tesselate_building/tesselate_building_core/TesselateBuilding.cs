using System.Collections.Generic;
using Wkx;

namespace tesselate_building_core
{
    public static class TesselateBuilding
    {
        public static (Polygon floor, Polygon roof, PolyhedralSurface walls) MakeBuilding(Polygon footprint, double fromZ, double height)
        {
            var floor = GetPolygonZ(footprint, fromZ);
            var roof = GetPolygonZ(footprint, fromZ + height);
            var wallPolygons = MakeWalls(footprint, fromZ, height - fromZ);

            var walls = new PolyhedralSurface();
            walls.Dimension = Dimension.Xyz;
            walls.Geometries.AddRange(wallPolygons);

            return (floor, roof, walls);
        }

        private static Polygon GetPolygonZ(Polygon polygon, double z)
        {
            var newPoints = new List<Point>();
            foreach (var p in polygon.ExteriorRing.Points)
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
            for (var i = 1; i < footprint.ExteriorRing.Points.Count; i++)
            {
                var p0 = footprint.ExteriorRing.Points[i - 1];
                var p1 = footprint.ExteriorRing.Points[i];

                var t1 = new Polygon();
                t1.Dimension = Dimension.Xyz;

                t1.ExteriorRing.Points.Add(new Point((double)p0.X, (double)p0.Y, fromZ));
                t1.ExteriorRing.Points.Add(new Point((double)p0.X, (double)p0.Y, fromZ + storeyheight));
                t1.ExteriorRing.Points.Add(new Point((double)p1.X, (double)p1.Y, fromZ + storeyheight));
                t1.ExteriorRing.Points.Add(new Point((double)p1.X, (double)p1.Y, fromZ));
                t1.ExteriorRing.Points.Add(new Point((double)p0.X, (double)p0.Y, fromZ));

                polygons.Add(t1);
            }

            return polygons;
        }
    }
}
