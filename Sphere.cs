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
    }
}