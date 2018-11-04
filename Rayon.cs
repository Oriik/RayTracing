using System;
using System.Numerics;

namespace SyntheseImage
{
    public class Rayon
    {

        public Vector3 origine;
        public Vector3 direction;

        public Rayon(Vector3 _origine, Vector3 _direction)
        {
            origine = _origine;
            direction = _direction;

        }

        public float IntersectAShape(Shape shape)
        {
            if(shape is Sphere)
            {
                return IntersectASphere((Sphere)shape);
            }
            if(shape is Triangle)
            {
                return IntersectATriangle((Triangle)shape);
            }
            return -1.0f;
        }


        private float IntersectASphere(Sphere sphere)
        //RETURN -1 IF NO INTERSECT
        {
            float A = Vector3.Dot(direction, direction);
            float B = 2 * (Vector3.Dot(origine, direction) - Vector3.Dot(sphere.center, direction));
            float C = Vector3.Dot(Vector3.Subtract(sphere.center, origine), Vector3.Subtract(sphere.center, origine)) - (sphere.radius * sphere.radius);
            float D = B * B - 4 * A * C;
            if (D < 0)
                return -1.0f;
            else
            {
                float i1 = ((-B) + (float)Math.Sqrt(D)) / (2 * A);
                float i2 = ((-B) - (float)Math.Sqrt(D)) / (2 * A);
                if (i2 > 0)
                    return i2;
                else if (i1 > 0)
                    return i1;
                else
                    return -1.0f;
            }


        }

        private float IntersectATriangle(Triangle triangle) //ALGO MOLLER TRUMBORE
            //RETURN -1 IF NO INTERSECT
        {
            Vector3 h = Vector3.Cross(direction, triangle.v);
            float a = Vector3.Dot(triangle.u, h);
            if (a > -float.Epsilon && a < float.Epsilon) { return -1.0f; }
            float f = 1.0f / a;
            Vector3 s = origine - triangle.a;
            float u = f * (Vector3.Dot(s, h));
            if(u<0 || u>1.0) { return -1.0f; }
            Vector3 q = Vector3.Cross(s, triangle.u);
            float v = f * Vector3.Dot(direction, q);
            if(v < 0 || u + v > 1) { return -1.0f; }
            float t = f * Vector3.Dot(triangle.v, q);
            if(t> float.Epsilon)
            {
                return t;
            }
            else { return -1.0f; }
        }

        public Vector3 GetPointAt(float coeff)
        {
            return Vector3.Add(origine, Vector3.Multiply(coeff,direction));
        }
    }
}