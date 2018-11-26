using System.Collections.Generic;
using System.Numerics;

namespace SyntheseImage
{
    public class Box
    {
        public Vector3 pMin, pMax;

        public Box(Vector3 _pMin, Vector3 _pMax)
        {
            pMin = _pMin;
            pMax = _pMax;
        }

        public Box Fusion(Box b)
        {
            Vector3 min = pMin;
            Vector3 max = pMax;

            if (b.pMin.X < min.X) min.X = b.pMin.X;
            if (b.pMin.Y < min.Y) min.Y = b.pMin.Y;
            if (b.pMin.Z < min.Z) min.Z = b.pMin.Z;

            if (b.pMax.X > max.X) max.X = b.pMax.X;
            if (b.pMax.Y > max.Y) max.Y = b.pMax.Y;
            if (b.pMax.Z > max.Z) max.Z = b.pMax.Z;

            Box fusion = new Box(min, max);
            return fusion;
        }
    }


}