using System;
using System.Numerics;

namespace SyntheseImage
{
    public class Sphere : Shape
    {
        public Vector3 center;

        public float radius;
        

        public Sphere(Vector3 _p, float _radius, Material _material)
        {
            center = _p;
            radius = _radius;
            material = _material;
        }

        public override Box GetBoundingBox()
        {
            Vector3 pMin, pMax;
            pMin = new Vector3(center.X - radius, center.Y - radius, center.Z - radius);
            pMax = new Vector3(center.X + radius, center.Y + radius, center.Z + radius);
            Box box = new Box(pMin, pMax);
            return box;
        }

        public override Vector3 GetNormal(Vector3 point)
        {
            return Vector3.Normalize(Vector3.Subtract(point, center));
        }

        public override float RayIntersect(Rayon ray, out Shape returnShape)
        {
            returnShape = null;
            float A = Vector3.Dot(ray.direction, ray.direction);
            float B = 2 * (Vector3.Dot(ray.origine, ray.direction) - Vector3.Dot(center, ray.direction));
            float C = Vector3.Dot(Vector3.Subtract(center, ray.origine), Vector3.Subtract(center, ray.origine)) - (radius * radius);
            float D = B * B - 4 * A * C;
            if (D < 0)
                return -1.0f;
            else
            {
                float i1 = ((-B) + (float)Math.Sqrt(D)) / (2 * A);
                float i2 = ((-B) - (float)Math.Sqrt(D)) / (2 * A);
                if (i2 > 0)
                {
                    returnShape = this;
                    return i2;
                }

                else if (i1 > 0)
                {
                    returnShape = this;
                    return i1;
                }

                else
                    return -1.0f;
            }

        }
    }
}