using System.Numerics;

namespace SyntheseImage
{
    public class Sphere
    {
        public Vector3 center;

        public float radius;

        public Couleur couleur;

        public Sphere(Vector3 _p, float _radius, Couleur _couleur)
        {
            center = _p;
            radius = _radius;
            couleur = _couleur;
        }
    }
}