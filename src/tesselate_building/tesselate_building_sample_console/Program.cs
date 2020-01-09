using Dapper;
using Npgsql;
using System;
using System.Diagnostics;
using tesselate_building;
using Wkx;

namespace tesselate_building_sample_console
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Tesselate buildings");
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var connectionString = $"Host=localhost;Username=postgres;Password=postgres;Database=postgres";
            var conn = new NpgsqlConnection(connectionString);
            SqlMapper.AddTypeHandler(new GeometryTypeHandler());
            conn.Open();

            var buildings = conn.Query<Building>("select ST_AsBinary(wkb_geometry) as geometry, height, ogc_fid as id from delaware_buildings");

            var i = 0;
            foreach (var building in buildings)
            {
                var polygon = (Polygon)building.Geometry;
                var height = building.Height;
                var points = polygon.ExteriorRing.Points;

                var polyhedralsurface = TesselateBuilding.MakePolyHedral(polygon, height);
                var wkt = polyhedralsurface.SerializeString<WktSerializer>();
                var updateSql = $"update delaware_buildings set geom = ST_GeomFromText('{wkt}') where ogc_fid={building.Id}";
                conn.Execute(updateSql);
                var perc = Math.Round((double)i / buildings.AsList().Count * 100, 2);
                Console.Write($"\rProgress: {perc.ToString("F")}%");
                i++;
            }
            conn.Close();

            stopWatch.Stop();
            Console.WriteLine();
            Console.WriteLine($"Elapsed: {stopWatch.ElapsedMilliseconds / 1000} seconds");
            Console.WriteLine("Program finished.");
        }
    }
}
