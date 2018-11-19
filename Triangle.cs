using System.Numerics;

namespace SyntheseImage
{
    public class Triangle : Shape
    {

        public Vector3 a, b, c;
        public Vector3 normal;
        public Vector3 u, v;
        public Vector3 uPerp, vPerp;
        public float denominatorST;

        public Triangle(Vector3 _a, Vector3 _b, Vector3 _c, Material _material)
        {
            material = _material;
            a = _a;
            b = _b;
            c = _c;

            u = b - a;
            v = c - a;

            normal = -Vector3.Normalize(Vector3.Cross(u, v));

        }

        public override Box GetBoundingBox()
        {
            Vector3 pMin, pMax;
            pMin = a;
            if (b.X < pMin.X) pMin.X = b.X;
            if (b.Y < pMin.Y) pMin.Y = b.Y;
            if (b.Z < pMin.Z) pMin.Z = b.Z;

            pMax = a;
            if (b.X > pMax.X) pMax.X = b.X;
            if (b.Y > pMax.Y) pMax.Y = b.Y;
            if (b.Z > pMax.Z) pMax.Z = b.Z;
            Box box = new Box(pMin, pMax);
            return box;
        }

        public override Vector3 GetNormal(Vector3 point)
        {
            return normal;
        }

        public void Translate(Vector3 translation)
        {
            a = a + translation;
            b = b + translation;
            c = c + translation;

            u = b - a;
            v = c - a;

            normal = -Vector3.Normalize(Vector3.Cross(u, v));
        }

    }
}
