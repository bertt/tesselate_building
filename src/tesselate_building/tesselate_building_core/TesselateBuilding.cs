using EarcutNet;
using System.Collections.Generic;
using System.Linq;
using Wkx;

namespace tesselate_building
{
    public static class TesselateBuilding
    {
        public static (PolyhedralSurface polyhedral, List<string> colors) MakePolyHedral(Polygon footprint, double height, BuildingStyle buildingStyle)
        {
            var polyhedral = new PolyhedralSurface();
            
            var res = MakeBuilding(footprint, height, buildingStyle);
            foreach(var t in res.polygons)
            {
                polyhedral.Geometries.Add(t);
            }

            return (polyhedral, res.colors);
        }

        public static (List<Polygon> polygons, List<string> colors) MakeBuilding(Polygon footprint, double height, BuildingStyle buildingStyle)
        {
            var result = new List<Polygon>();
            var colors = new List<string>();

            var walls = MakeWalls(footprint, height);
            var floor = Tesselate(footprint, 0);
            var roof = Tesselate(footprint, height);

            colors.AddRange(GetColors(floor, buildingStyle.FloorColor));
            colors.AddRange(GetColors(roof, buildingStyle.RoofColor));
            colors.AddRange(GetColors(walls, buildingStyle.WallsColor));

            result.AddRange(floor);
            result.AddRange(roof);
            result.AddRange(walls);
            return (result, colors);
        }

        private static List<string> GetColors(List<Polygon> polygons, string Color )
        {
            return Enumerable.Repeat(Color, polygons.Count).ToList();
        }

        public static List<Polygon> MakeWalls(Polygon footprint, double height)
        {
            var polygons = new List<Polygon>();
            for(var i = 1; i < footprint.ExteriorRing.Points.Count; i++)
            {
                var p0 = footprint.ExteriorRing.Points[i - 1];
                var p1 = footprint.ExteriorRing.Points[i];

                var t1 = new Polygon();
                t1.ExteriorRing.Points.Add(new Point((double)p0.X, (double)p0.Y, p0.Z));
                t1.ExteriorRing.Points.Add(new Point((double)p1.X, (double)p1.Y, p1.Z));
                t1.ExteriorRing.Points.Add(new Point((double)p0.X, (double)p0.Y, p0.Z + height));
                t1.ExteriorRing.Points.Add(new Point((double)p0.X, (double)p0.Y, p0.Z));


                polygons.Add(t1);

                var t2 = new Polygon();
                t2.ExteriorRing.Points.Add(new Point((double)p0.X, (double)p0.Y, p0.Z + height));
                t2.ExteriorRing.Points.Add(new Point((double)p1.X, (double)p1.Y, p1.Z + height));
                t2.ExteriorRing.Points.Add(new Point((double)p1.X, (double)p1.Y, p1.Z));
                t2.ExteriorRing.Points.Add(new Point((double)p0.X, (double)p0.Y, p0.Z + height));

                polygons.Add(t2);
            }

            return polygons;
        }

        public static List<Polygon> Tesselate(Polygon footprint, double height)
        {
            var points = footprint.ExteriorRing.Points;
            var z = footprint.ExteriorRing.Points[0].Z;

            var data = new List<double>();
            var holeIndices = new List<int>();

            foreach (var p in points)
            {
                data.Add((double)p.X);
                data.Add((double)p.Y);
            }

            var trianglesIndices = Earcut.Tessellate(data, holeIndices);


            var polygons = new List<Polygon>();
            for(var i = 0; i < trianglesIndices.Count / 3; i++)
            {
                var t = new Polygon();

                var a = trianglesIndices[i*3];
                var b = trianglesIndices[i*3+1];
                var c = trianglesIndices[i*3 +2];

                t.ExteriorRing.Points.Add(GetPoint(data, a, z, height));
                t.ExteriorRing.Points.Add(GetPoint(data, b, z, height));
                t.ExteriorRing.Points.Add(GetPoint(data, c, z, height));
                t.ExteriorRing.Points.Add(GetPoint(data, a, z, height));

                polygons.Add(t);
            }
            return polygons;
        }

        private static Point GetPoint(List<double> data, int index, double? z, double height)
        {
            var x = data[index*2];
            var y = data[index*2 + 1];
            var p = new Point(x, y, z+ height);
            return p;
        }
    }
}
