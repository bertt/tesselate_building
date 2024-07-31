using System.Text.Json.Serialization;

namespace tesselate_building_core
{
    public class BuildingStyle
    {
        [JsonPropertyName("walls")]
        public string WallsColor { get; set; }
        [JsonPropertyName("roof")]
        public string RoofColor { get; set; }
        [JsonPropertyName("floor")]
        public string FloorColor { get; set; }
    }
}
