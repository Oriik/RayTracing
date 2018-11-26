using System;
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
        public bool RayIntersect(Rayon ray)
        {
            Double rinvx = 1 / ray.direction.X;
            Double rinvy = 1 / ray.direction.Y;
            Double rinvz = 1 / ray.direction.Z;

            // X slab Max box size
            Double tx1 = (pMin.X - ray.origine.X) * rinvx;
            Double tx2 = (pMax.X - ray.origine.X) * rinvx;

            Double tminX = Math.Min(tx1, tx2);
            Double tmaxX = Math.Max(tx1, tx2);

            // Y slab
            Double ty1 = (pMin.Y - ray.origine.Y) * rinvy;
            Double ty2 = (pMax.Y - ray.origine.Y) * rinvy;

            Double tminY = Math.Max(tminX, (Math.Min(ty1, ty2)));
            Double tmaxY = Math.Min(tmaxX, (Math.Max(ty1, ty2)));

            // Z slab
            Double tz1 = (pMin.Z - ray.origine.Z) * rinvz;
            Double tz2 = (pMax.Z - ray.origine.Z) * rinvz;

            Double tminZ = Math.Max(tminY, (Math.Min(tz1, tz2)));
            Double tmaxZ = Math.Min(tmaxY, (Math.Max(tz1, tz2)));

            if (tmaxZ >= tminZ)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }


}