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
            d = Vector3.Normalize(_d);

        }



        public float IntersectASphere(Sphere sphere)
        //RETURN -1 IF NO INTERSECT
        {
            float a = Vector3.Dot(d, d);
            float b = 2 * (Vector3.Dot(o, d) - Vector3.Dot(sphere.center, d));
            float c = Vector3.Dot(Vector3.Subtract(sphere.center, o), Vector3.Subtract(sphere.center, o)) - sphere.radius * sphere.radius;
            float delta = b * b - 4 * a * c;
            if (delta >= 0)
            {
                float i1 = (float)(-b + Math.Sqrt(delta)) / (2 * a);
                float i2 = (float)(-b - Math.Sqrt(delta)) / (2 * a);
                if (i2 > 0)
                {
                    return i2;
                }
                else if (i1 > 0)
                {
                    return i1;
                }
                else return -1;

            }
            else return -1;


        }
    }
}