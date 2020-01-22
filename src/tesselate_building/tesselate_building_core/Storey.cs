using System.Text.Json.Serialization;

namespace tesselate_building_core
{
    public class Storey
    {
        [JsonPropertyName("from")]
        public double From { get; set; }
        [JsonPropertyName("to")]
        public double To { get; set; }
        [JsonPropertyName("color")]
        public string Color { get; set; }
    }
}
