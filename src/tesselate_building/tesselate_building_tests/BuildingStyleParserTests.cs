using NUnit.Framework;
using tesselate_building_core;

namespace tesselate_building_tests
{
    public class BuildingStyleParserTests
    {
        [Test]
        public void BuildingStyleWithoutStoreyParsing()
        {
            // arrange
            var json = @"{ ""walls"": ""#00ff00"", ""roof"":""#ff0000"", ""floor"":""#D3D3D3""}";
            var building = new Building();
            building.Style = json;

            // act
            var buildingStyle = building.BuildingStyle;

            // assert
            Assert.That(buildingStyle.WallsColor == "#00ff00");
            Assert.That(buildingStyle.Storeys == null);

        }

        [Test]
        public void BuildingStyleWithStoreyParsing()
        {
            // arrange
            var json = @"{ ""walls"": ""#00ff00"", ""roof"":"" #ff0000"", ""floor"":""#D3D3D3"", ""storeys"":[{""from"":0, ""to"":0.5, ""color"":""#D3D3D3""},{""from"":0.5, ""to"":1.0, ""color"":""#D3SS3D3""},{""from"":1.0, ""to"":1.5, ""color"":""#D354S3D3""}]}";
            var building = new Building();
            building.Style = json;

            // act
            var buildingStyle = building.BuildingStyle;

            // assert
            Assert.That(buildingStyle.WallsColor == "#00ff00");
            Assert.That(buildingStyle.Storeys.Count==3);
            Assert.That(buildingStyle.Storeys[0].From== 0);
            Assert.That(buildingStyle.Storeys[0].To == 0.5);
            Assert.That(buildingStyle.Storeys[0].Color == "#D3D3D3");
        }
    }
}
