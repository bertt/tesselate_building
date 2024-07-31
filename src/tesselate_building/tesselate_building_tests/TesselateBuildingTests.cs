using NUnit.Framework;
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
        public void MakeBuildingTest()
        {
            var res = TesselateBuilding.MakeBuilding(footprint, 0, height);
            Assert.That(res.floor !=null);
            
        }

        [Test]
        public void MakeWallsTest()
        {
            var walls = TesselateBuilding.MakeWalls(footprint, 0, height);
            Assert.That(walls.Count == (footprint.ExteriorRing.Points.Count-1));
        }
    }
}