using EarcutNet;
using System.Collections.Generic;
using Wkx;

namespace tesselate_building
{
    public static class TesselateBuilding
    {
        public static PolyhedralSurface MakePolyHedral(Polygon footprint, double height)
        {
            var polyhedral = new PolyhedralSurface();
            
            var polygons = MakeBuilding(footprint, height);
            foreach(var t in polygons)
            {
                polyhedral.Geometries.Add(t);
            }

            return polyhedral;
        }

        public static List<Polygon> MakeBuilding(Polygon footprint, double height)
        {
            var result = new List<Polygon>();
            var walls = MakeWalls(footprint, height);
            var floor = Tesselate(footprint, 0);
            var roof = Tesselate(footprint, height);

            result.AddRange(floor);
            result.AddRange(roof);
            result.AddRange(walls);
            return result;
        }


        public static List<Polygon> MakeWalls(Polygon footprint, double height)
        {
            var polygons = new List<Polygon>();
            for(var i = 1; i < footprint.ExteriorRing.Points.Count; i++)
            {
                var p0 = footprint.ExteriorRing.Points[i - 1];
                var p1 = footprint.ExteriorRing.Points[i];

                var t1 = new Polygon();
                t1.ExteriorRing.Points.Add(new Point((double)p0.X, (double)p0.Y, 0));
                t1.ExteriorRing.Points.Add(new Point((double)p1.X, (double)p1.Y, 0));
                t1.ExteriorRing.Points.Add(new Point((double)p0.X, (double)p0.Y, height));

                polygons.Add(t1);

                var t2 = new Polygon();
                t2.ExteriorRing.Points.Add(new Point((double)p0.X, (double)p0.Y, height));
                t2.ExteriorRing.Points.Add(new Point((double)p1.X, (double)p1.Y, height));
                t2.ExteriorRing.Points.Add(new Point((double)p1.X, (double)p1.Y, 0));

                polygons.Add(t2);
            }

            return polygons;
        }

        public static List<Polygon> Tesselate(Polygon footprint, double height)
        {
            var points = footprint.ExteriorRing.Points;

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
                t.ExteriorRing.Points.Add(GetPoint(data, i, height));
                t.ExteriorRing.Points.Add(GetPoint(data, i+1, height));
                t.ExteriorRing.Points.Add(GetPoint(data, i + 2, height));
               polygons.Add(t);
            }
            return polygons;
        }

        private static Point GetPoint(List<double> data, int index, double height)
        {
            var x = data[index];
            var y = data[index + 1];
            var p = new Point(x, y, height);
            return p;
        }
    }
}
