using CommandLine;
using Dapper;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Diagnostics;
using System.Reflection;
using tesselate_building_core;
using Wkx;

namespace tesselate_building_sample_console
{
    class Program
    {
        static string password = string.Empty;
        static void Main(string[] args)
        {
            var colorRoof = "#FF0000";
            var colorFloor = "#008000";
            var colorWalls = "#EEC900";
            var metallicRoughness = "#000000";


            var version = Assembly.GetEntryAssembly().GetName().Version;
            Console.WriteLine($"Tool: Tesselate buildings {version}");
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            Parser.Default.ParseArguments<Options>(args).WithParsed(o =>
            {
                o.User = string.IsNullOrEmpty(o.User) ? Environment.UserName : o.User;
                o.Database = string.IsNullOrEmpty(o.Database) ? Environment.UserName : o.Database;

                var connectionString = $"Host={o.Host};Username={o.User};Database={o.Database};Port={o.Port}";

                var istrusted = TrustedConnectionChecker.HasTrustedConnection(connectionString);

                if (!istrusted)
                {
                    Console.Write($"Password for user {o.User}: ");
                    password = PasswordAsker.GetPassword();
                    connectionString += $";password={password}";
                    Console.WriteLine();
                }
                var conn = new NpgsqlConnection(connectionString);
                SqlMapper.AddTypeHandler(new GeometryTypeHandler());
                conn.Open();

                Console.WriteLine(Console.Out.NewLine + "Connected to database.");
                Console.WriteLine();
                // write tablename to console
                Console.WriteLine($"Table: {o.Table}");

                // create a new output table, with name of the input table + "_3d"
                var outputTable = $"{o.Table}_3d";
                // drop table if exists
                var dropTable = $"drop table if exists {outputTable}";
                conn.Execute(dropTable);
                var createTable = $"create table {outputTable} as select * from {o.Table} where 1=0";
                conn.Execute(createTable);
                var addShadersColumn = $"alter table {outputTable} add column {o.ShadersColumn} json";
                conn.Execute(addShadersColumn);
                var addTypeColumn = $"alter table {outputTable} add column type text";
                conn.Execute(addTypeColumn);

                var addGeometryColumn = $"alter table {outputTable} add column {o.OutputGeometryColumn} geometry";
                conn.Execute(addGeometryColumn);


                var select = $"select ST_AsBinary({o.InputGeometryColumn}) as geometry, {o.HeightColumn} as height";
                var sql = $"{select} from {o.Table}";
                var buildings = conn.Query<Building>(sql);

                var i = 1;
                foreach (var building in buildings)
                {
                    var polygon = (Polygon)building.Geometry;
                    var wktFootprint = polygon.SerializeString<WktSerializer>();
                    var height = building.Height;
                    var points = polygon.ExteriorRing.Points;

                    var buildingZ = 0.0; //put everything on the ground
                    var buildingParts = TesselateBuilding.MakeBuilding(polygon, buildingZ, height);

                    // write floor shader to the database
                    string jsonFloor = GetShader(colorFloor, metallicRoughness);

                    // insert record into output table
                    var insertFloor = $"insert into {outputTable} ({o.OutputGeometryColumn}, {o.ShadersColumn}, type) " +
                        $"values (ST_Force3D(St_SetSrid(ST_GeomFromText('{wktFootprint}'), 4326)), '{jsonFloor}', 'floor')";

                    conn.Execute(insertFloor);

                    string jsonRoof = GetShader(colorRoof, metallicRoughness);
                    var wktRoof = buildingParts.roof.SerializeString<WktSerializer>();
                    var insertRoof = $"insert into {outputTable} ({o.OutputGeometryColumn}, {o.ShadersColumn}, type) " +
                        $"values (ST_Force3D(St_SetSrid(ST_GeomFromText('{wktRoof}'), 4326)), '{jsonRoof}', 'roof')";

                    conn.Execute(insertRoof);


                    string jsonWalls = GetShader(colorWalls, metallicRoughness);
                    var wktWall = buildingParts.walls.SerializeString<WktSerializer>();
                    var insertWalls = $"insert into {outputTable} ({o.OutputGeometryColumn}, {o.ShadersColumn}, type) " +
                        $"values (ST_Force3D(St_SetSrid(ST_GeomFromText('{wktWall}'), 4326)), '{jsonWalls}', 'walls')";

                    conn.Execute(insertWalls);



                    var perc = Math.Round((double)i / buildings.AsList().Count * 100, 2);
                    Console.Write($"\rProgress: {perc.ToString("F")}%");
                    i++;
                }

                conn.Close();

                stopWatch.Stop();
                Console.WriteLine();
                Console.WriteLine($"Elapsed: {stopWatch.ElapsedMilliseconds / 1000} seconds");
                Console.WriteLine("Program finished.");
            });
        }

        private static string GetShader(string colorFloor, string metallicRoughness)
        {
            var shader = new ShaderColor();
            shader.PbrMetallicRoughnessColors = new PbrMetallicRoughnessColors() { BaseColor = colorFloor, MetallicRoughnessColor = metallicRoughness };

            var json = JsonConvert.SerializeObject(shader,
                Formatting.None, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
            return json;
        }
    }
}
