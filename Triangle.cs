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
            if (c.X < pMin.X) pMin.X = c.X;
            if (c.Y < pMin.Y) pMin.Y = c.Y;
            if (c.Z < pMin.Z) pMin.Z = c.Z;

            pMax = a;
            if (b.X > pMax.X) pMax.X = b.X;
            if (b.Y > pMax.Y) pMax.Y = b.Y;
            if (b.Z > pMax.Z) pMax.Z = b.Z;
            if (c.X > pMax.X) pMax.X = c.X;
            if (c.Y > pMax.Y) pMax.Y = c.Y;
            if (c.Z > pMax.Z) pMax.Z = c.Z;
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

        public override float RayIntersect(Rayon ray, out Shape returnShape)
        {
            returnShape = null;
            Vector3 h = Vector3.Cross(ray.direction, this.v);
            float a = Vector3.Dot(this.u, h);
            if (a > -float.Epsilon && a < float.Epsilon) { return -1.0f; }
            float f = 1.0f / a;
            Vector3 s = ray.origine - this.a;
            float u = f * (Vector3.Dot(s, h));
            if (u < 0 || u > 1.0) { return -1.0f; }
            Vector3 q = Vector3.Cross(s, this.u);
            float v = f * Vector3.Dot(ray.direction, q);
            if (v < 0 || u + v > 1) { return -1.0f; }
            float t = f * Vector3.Dot(this.v, q);

            if (t > float.Epsilon)
            {
                returnShape = this;
                return t;
            }
            else { return -1.0f; }
        }

    }
}
