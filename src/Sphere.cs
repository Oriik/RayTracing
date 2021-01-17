using System;
using System.Numerics;

namespace SyntheseImage
{
    public class Sphere : Shape
    {
        #region Variables
        private Vector3 m_center;
        private float m_radius;
        #endregion

        public Sphere(Vector3 _p, float _radius, Material _material)
        {
            m_center = _p;
            m_radius = _radius;
            material = _material;
        }

        public override Box GetBoundingBox()
        {
            Vector3 pMin, pMax;
            pMin = new Vector3(m_center.X - m_radius, m_center.Y - m_radius, m_center.Z - m_radius);
            pMax = new Vector3(m_center.X + m_radius, m_center.Y + m_radius, m_center.Z + m_radius);
            Box box = new Box(pMin, pMax);
            return box;
        }

        public override Vector3 GetNormal(Vector3 point)
        {
            return Vector3.Normalize(Vector3.Subtract(point, m_center));
        }

        public override float RayIntersect(Ray ray, out Shape returnShape)
        {
            returnShape = null;
            float A = Vector3.Dot(ray.direction, ray.direction);
            float B = 2 * (Vector3.Dot(ray.origin, ray.direction) - Vector3.Dot(m_center, ray.direction));
            float C = Vector3.Dot(Vector3.Subtract(m_center, ray.origin), Vector3.Subtract(m_center, ray.origin)) - (m_radius * m_radius);
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