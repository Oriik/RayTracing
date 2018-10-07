using System;
using System.Numerics;

namespace SyntheseImage
{
    public class Rayon
    {

        public Vector3 o;
        public Vector3 d;

        public Rayon(Vector3 _o, Vector3 _d)
        {
            o = _o;
            d = _d;

        }



        public float IntersectASphere(Sphere sphere)
        //RETURN -1 IF NO INTERSECT
        {
            float A = Vector3.Dot(d, d);
            float B = 2 * (Vector3.Dot(o, d) - Vector3.Dot(sphere.center, d));
            float C = Vector3.Dot(Vector3.Subtract(sphere.center, o), Vector3.Subtract(sphere.center, o)) - (sphere.radius * sphere.radius);
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
    }
}