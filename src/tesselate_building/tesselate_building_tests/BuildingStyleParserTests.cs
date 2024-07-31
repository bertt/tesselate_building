using NUnit.Framework;
using tesselate_building_core;

namespace tesselate_building_tests
{
    public class BuildingStyleParserTests
    {
        [Test]
        public void BuildingStyleParsing()
        {
            // arrange
            var json = @"{ ""walls"": ""#00ff00"", ""roof"":""#ff0000"", ""floor"":""#D3D3D3""}";
            var building = new Building();
            building.Style = json;

            // act
            var buildingStyle = building.BuildingStyle;

            // assert
            Assert.That(buildingStyle.WallsColor == "#00ff00");
        }
    }
}
