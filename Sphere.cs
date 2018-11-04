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

        public override Vector3 GetNormal(Vector3 point)
        {
            return Vector3.Normalize(Vector3.Subtract(point, center));
        }
    }
}