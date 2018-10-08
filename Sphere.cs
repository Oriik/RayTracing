using System.Numerics;

namespace SyntheseImage
{
    public class Sphere
    {
        public Vector3 center;

        public float radius;

        public Material material;

        public Sphere(Vector3 _p, float _radius, Material _material)
        {
            center = _p;
            radius = _radius;
            material = _material;
        }
    }
}