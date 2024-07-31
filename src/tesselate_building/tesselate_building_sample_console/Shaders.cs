using Newtonsoft.Json;

namespace tesselate_building_sample_console
{
    public class ShaderColor
    {
        public string EmissiveColor { get; set; }
        [JsonProperty(PropertyName = "PbrSpecularGlossiness")]
        public PbrSpecularGlossinessColors PbrSpecularGlossinessColors { get; set; }

        [JsonProperty(PropertyName = "PbrMetallicRoughness")]
        public PbrMetallicRoughnessColors PbrMetallicRoughnessColors { get; set; }
    }

    public class PbrSpecularGlossinessColors
    {
        public string DiffuseColors { get; set; }
        [JsonProperty(PropertyName = "SpecularGlossiness")]
        public string SpecularGlossinessColor { get; set; }
    }

    public class PbrMetallicRoughnessColors
    {
        [JsonProperty(PropertyName = "MetallicRoughness")]
        public string MetallicRoughnessColor { get; set; }
        public string BaseColor { get; set; }
    }

}
