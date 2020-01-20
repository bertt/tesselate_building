using CommandLine;

namespace tesselate_building_sample_console
{
    public class Options
    {
        [Option('U', "username", Required = false, HelpText = "Database user")]
        public string User { get; set; }

        [Option('h', "host", Required = false, Default = "localhost", HelpText = "Database host")]
        public string Host { get; set; }

        [Option('d', "dbname", Required = false, HelpText = "Database name")]
        public string Database { get; set; }
        [Option('p', "port", Required = false, Default = "5432", HelpText = "Database port")]
        public string Port { get; set; }

        [Option('t', "table", Required = true, HelpText = "Database table, include database schema if needed")]
        public string Table { get; set; }

        [Option('i', "inputgeometrycolumn", Required = false, Default = "geom", HelpText = "Input geometry column")]
        public string InputGeometryColumn { get; set; }

        [Option('o', "outputgeometrycolumn", Required = false, Default = "geom3d", HelpText = "Output geometry column")]
        public string OutputGeometryColumn { get; set; }

        [Option("heightcolumn", Required = false, Default = "height", HelpText = "height column")]
        public string HeightColumn { get; set; }

        [Option("idcolumn", Required = false, Default = "id", HelpText = "Id column")]
        public string IdColumn { get; set; }
    }
}
