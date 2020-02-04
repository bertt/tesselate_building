using EarcutNet;
using System;
using System.Collections.Generic;
using System.Linq;
using Wkx;

namespace tesselate_building_core
{
    public static class TesselateBuilding
    {
        public static (PolyhedralSurface polyhedral, List<string> colors) MakePolyHedral(Polygon footprint, double fromZ, double height, BuildingStyle buildingStyle)
        {
            var polyhedral = new PolyhedralSurface();
            
            var res = MakeBuilding(footprint, fromZ, height, buildingStyle);
            foreach(var t in res.polygons)
            {
                polyhedral.Geometries.Add(t);
            }

            return (polyhedral, res.colors);
        }

        public static (List<Polygon> polygons, List<string> colors) MakeBuilding(Polygon footprint, double fromZ, double height, BuildingStyle buildingStyle)
        {
            var result = new List<Polygon>();
            var colors = new List<string>();

            var floor = Tesselate(footprint, fromZ);
            var roof = Tesselate(footprint, fromZ + height);

            colors.AddRange(GetColors(floor, buildingStyle.FloorColor));
            colors.AddRange(GetColors(roof, buildingStyle.RoofColor));

            result.AddRange(floor);
            result.AddRange(roof);

            if (buildingStyle.Storeys == null)
            {
                var walls = MakeWalls(footprint, fromZ, height-fromZ);
                colors.AddRange(GetColors(walls, buildingStyle.WallsColor));
                result.AddRange(walls);
            }
            else
            {
                foreach(var storey in buildingStyle.Storeys)
                {
                    var walls = MakeWalls(footprint, fromZ + storey.From, storey.To - storey.From);
                    colors.AddRange(GetColors(walls, storey.Color));
                    result.AddRange(walls);
                }
            }

            return (result, colors);
        }

        private static List<string> GetColors(List<Polygon> polygons, string Color )
        {
            return Enumerable.Repeat(Color, polygons.Count).ToList();
        }

        public static List<Polygon> MakeWalls(Polygon footprint, double fromZ, double storeyheight)
        {
            var polygons = new List<Polygon>();
            for(var i = 1; i < footprint.ExteriorRing.Points.Count; i++)
            {
                var p0 = footprint.ExteriorRing.Points[i - 1];
                var p1 = footprint.ExteriorRing.Points[i];

                var t1 = new Polygon();
                t1.ExteriorRing.Points.Add(new Point((double)p0.X, (double)p0.Y, fromZ));
                t1.ExteriorRing.Points.Add(new Point((double)p1.X, (double)p1.Y, fromZ));
                t1.ExteriorRing.Points.Add(new Point((double)p0.X, (double)p0.Y, fromZ + storeyheight));
                t1.ExteriorRing.Points.Add(new Point((double)p0.X, (double)p0.Y, fromZ));

                polygons.Add(t1);

                var t2 = new Polygon();
                t2.ExteriorRing.Points.Add(new Point((double)p0.X, (double)p0.Y, fromZ + storeyheight));
                t2.ExteriorRing.Points.Add(new Point((double)p1.X, (double)p1.Y, fromZ + storeyheight));
                t2.ExteriorRing.Points.Add(new Point((double)p1.X, (double)p1.Y, fromZ));
                t2.ExteriorRing.Points.Add(new Point((double)p0.X, (double)p0.Y, fromZ + storeyheight));

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

                var a = trianglesIndices[i*3];
                var b = trianglesIndices[i*3+1];
                var c = trianglesIndices[i*3 +2];

                t.ExteriorRing.Points.Add(GetPoint(data, a, height));
                t.ExteriorRing.Points.Add(GetPoint(data, b, height));
                t.ExteriorRing.Points.Add(GetPoint(data, c, height));
                t.ExteriorRing.Points.Add(GetPoint(data, a, height));

                polygons.Add(t);
            }
            return polygons;
        }

        private static Point GetPoint(List<double> data, int index, double height)
        {
            var x = data[index*2];
            var y = data[index*2 + 1];
            var p = new Point(x, y, height);
            return p;
        }
    }
}
