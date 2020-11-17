using Newtonsoft.Json;
using System.Collections.Generic;

namespace tesselate_building_sample_console
{
    public class ShaderColors
    {
        public List<string> EmissiveColors { get; set; }
        [JsonProperty(PropertyName = "PbrSpecularGlossiness")]
        public PbrSpecularGlossinessColors PbrSpecularGlossinessColors { get; set; }

        [JsonProperty(PropertyName = "PbrMetallicRoughness")]
        public PbrMetallicRoughnessColors PbrMetallicRoughnessColors { get; set; }
    }

    public class PbrSpecularGlossinessColors
    {
        public List<string> DiffuseColors { get; set; }
        [JsonProperty(PropertyName = "SpecularGlossiness")]
        public List<string> SpecularGlossinessColors { get; set; }
    }

    public class PbrMetallicRoughnessColors
    {
        [JsonProperty(PropertyName = "MetallicRoughness")]
        public List<string> MetallicRoughnessColors { get; set; }
        public List<string> BaseColors { get; set; }
    }

}
