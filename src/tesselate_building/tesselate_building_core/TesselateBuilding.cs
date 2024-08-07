using System;
using System.Collections.Generic;
using System.Linq;
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
            colors.Add(buildingStyle.FloorColor);
            polyhedral.Geometries.Add(GetPolygonZ(footprint, fromZ + height));
            colors.Add(buildingStyle.RoofColor);
            if (buildingStyle.Storeys == null)
            {
                var walls = MakeWalls(footprint, fromZ, height - fromZ);
                polyhedral.Geometries.AddRange(walls);
                foreach(var wall in walls)
                {
                    colors.Add(buildingStyle.WallsColor);
                }
            }
            else
            {
                {
                    foreach (var storey in buildingStyle.Storeys)
                    {
                        var walls = MakeWalls(footprint, fromZ + storey.From, storey.To - storey.From);
                        foreach (var wall in walls)
                        {
                            colors.Add(storey.Color);
                        }
                        polyhedral.Geometries.AddRange(walls);
                    }
                }
            }

            return (polyhedral, colors);


        }
        private static string GetStoreyColor(Polygon polygon, List<Storey> storeys)
        {
            var minz = Double.MaxValue;
            var maxz = Double.MinValue;

            foreach (var p in polygon.ExteriorRing.Points)
            {
                if (p.Z > maxz) { maxz = (double)p.Z; };
                if (p.Z < minz) { minz = (double)p.Z; };
            }

            var storey = storeys.Where(storey => storey.From == minz && storey.To == maxz).FirstOrDefault();
            return storey.Color;
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
