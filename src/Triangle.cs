using System.Numerics;

namespace SyntheseImage
{
    public class Triangle : Shape
    {
        #region Variables
        private Vector3 m_a, m_b, m_c;
        private Vector3 m_normal;
        private Vector3 m_u, m_v;       
        #endregion

        public Triangle(Vector3 _a, Vector3 _b, Vector3 _c, Material _material)
        {
            material = _material;
            m_a = _a;
            m_b = _b;
            m_c = _c;

            m_u = m_b - m_a;
            m_v = m_c - m_a;

            m_normal = -Vector3.Normalize(Vector3.Cross(m_u, m_v));
        }

        public override Box GetBoundingBox()
        {
            Vector3 pMin, pMax;
            pMin = m_a;
            if (m_b.X < pMin.X) pMin.X = m_b.X;
            if (m_b.Y < pMin.Y) pMin.Y = m_b.Y;
            if (m_b.Z < pMin.Z) pMin.Z = m_b.Z;
            if (m_c.X < pMin.X) pMin.X = m_c.X;
            if (m_c.Y < pMin.Y) pMin.Y = m_c.Y;
            if (m_c.Z < pMin.Z) pMin.Z = m_c.Z;

            pMax = m_a;
            if (m_b.X > pMax.X) pMax.X = m_b.X;
            if (m_b.Y > pMax.Y) pMax.Y = m_b.Y;
            if (m_b.Z > pMax.Z) pMax.Z = m_b.Z;
            if (m_c.X > pMax.X) pMax.X = m_c.X;
            if (m_c.Y > pMax.Y) pMax.Y = m_c.Y;
            if (m_c.Z > pMax.Z) pMax.Z = m_c.Z;
            Box box = new Box(pMin, pMax);
            return box;
        }

        public override Vector3 GetNormal(Vector3 point)
        {
            return m_normal;
        }

        public void Translate(Vector3 translation)
        {
            m_a = m_a + translation;
            m_b = m_b + translation;
            m_c = m_c + translation;

            m_u = m_b - m_a;
            m_v = m_c - m_a;

            m_normal = -Vector3.Normalize(Vector3.Cross(m_u, m_v));
        }

        public override float RayIntersect(Ray ray, out Shape returnShape)
        {
            returnShape = null;
            Vector3 h = Vector3.Cross(ray.direction, this.m_v);
            float a = Vector3.Dot(this.m_u, h);
            if (a > -float.Epsilon && a < float.Epsilon) { return -1.0f; }
            float f = 1.0f / a;
            Vector3 s = ray.origin - this.m_a;
            float u = f * (Vector3.Dot(s, h));
            if (u < 0 || u > 1.0) { return -1.0f; }
            Vector3 q = Vector3.Cross(s, this.m_u);
            float v = f * Vector3.Dot(ray.direction, q);
            if (v < 0 || u + v > 1) { return -1.0f; }
            float t = f * Vector3.Dot(this.m_v, q);

            if (t > float.Epsilon)
            {
                returnShape = this;
                return t;
            }
            else { return -1.0f; }
        }

    }
}
