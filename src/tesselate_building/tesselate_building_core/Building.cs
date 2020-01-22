using System.Text.Json;
using Wkx;

namespace tesselate_building_core
{
    public class Building
    {
        public Geometry Geometry { get; set; }
        public double Height { get; set; }

        public int Id { get; set; }

        public string Style { get; set; } 

        public BuildingStyle BuildingStyle
        {
            get
            {
                return JsonSerializer.Deserialize<BuildingStyle>(Style);
            }
        }
    }
}
