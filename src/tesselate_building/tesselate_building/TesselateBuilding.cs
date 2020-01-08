using System;
using Wkx;

namespace tesselate_building
{
    public static class TesselateBuilding
    {
        public static PolyhedralSurface Tesselate(Polygon footprint, double height)
        {
            var points = footprint.ExteriorRing.Points;
            // todo: tesselate
            return new PolyhedralSurface();
        }
    }
}
