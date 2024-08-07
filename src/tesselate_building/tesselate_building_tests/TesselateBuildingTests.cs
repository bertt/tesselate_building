using NUnit.Framework;
using System.Collections.Generic;
using tesselate_building_core;
using Wkx;

namespace NUnitTestProject1
{
    public class Tests
    {
        private string wktFootprint;
        private double height;
        private Polygon footprint;

        [SetUp]
        public void Setup()
        {
            wktFootprint = "POLYGON((-75.55478134 39.1632752950001,-75.55477116 39.163235817,-75.554760981 39.1631963390001,-75.554818218 39.163187394,-75.5548754549999 39.16317845,-75.5548856349999 39.1632179280001,-75.554896589 39.1632604100001,-75.554724403 39.163285407,-75.554724102 39.1632842400001,-75.55478134 39.1632752950001))";
            footprint = (Polygon)Geometry.Deserialize<WktSerializer>(wktFootprint);
            height = 11.55;
        }

        [Test]
        public void MakePolyHedralTest()
        {
            var bs = new BuildingStyle() { FloorColor = "#D3D3D3", RoofColor = "#ff0000", WallsColor = "#00ff00" };
            var res = TesselateBuilding.MakeBuilding(footprint, 0, height, bs);
            var wkt = res.polyhedral.SerializeString<WktSerializer>();
            Assert.That(wkt!=null);
            
        }

        [Test]
        public void TestId12()
        {
            var wktFootprint = "POLYGON((-75.554412769 39.1634003080001, -75.554480102 39.163362636, -75.554508552 39.1633934610001, -75.554552455 39.163368898, -75.554609356 39.1634305470001, -75.554505101 39.163488876, -75.554412769 39.1634003080001))";
            footprint = (Polygon)Geometry.Deserialize<WktSerializer>(wktFootprint);
            var height = 9.92000000000;
            var bs = new BuildingStyle() { FloorColor = "#D3D3D3", RoofColor = "#ff0000", WallsColor = "#00ff00" };

            var res = TesselateBuilding.MakeBuilding(footprint, 0, height, bs);
            Assert.That(res.polyhedral.Geometries.Count == 8);
            Assert.That(res.colors.Count == 8);
        }

        [Test]
        public void MakeBuildingWithStoreysTest()
        {
            // arrange
            var buildingHeight = 10;
            var storeys = new List<Storey>();
            storeys.Add(new Storey() { From = 0, To = 5, Color = "#ff0000" });
            storeys.Add(new Storey() { From = 5, To = 10, Color = "#D3D3D3" });

            var bs = new BuildingStyle() { FloorColor = "#D3D3D3", RoofColor = "#ff0000", WallsColor = "#00ff00" };
            bs.Storeys = storeys;
            
            // act
            var res = TesselateBuilding.MakeBuilding(footprint, 0, buildingHeight, bs);

            // assert
            Assert.That(res.polyhedral.Geometries.Count == 20);
        }

        [Test]
        public void TriangulateBuildingTest()
        {
            var bs = new BuildingStyle() { FloorColor = "#D3D3D3", RoofColor = "#ff0000", WallsColor = "#00ff00" };
            var res = TesselateBuilding.MakeBuilding(footprint, 0, height, bs);
            Assert.That(res.polyhedral.Geometries.Count == 11);
        }


        [Test]
        public void MakeWallsTest()
        {
            var walls = TesselateBuilding.MakeWalls(footprint, 0, height);
            Assert.That(walls.Count == (footprint.ExteriorRing.Points.Count-1));
        }
    }
}