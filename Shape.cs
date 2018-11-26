using System.Numerics;

namespace SyntheseImage
{
    public abstract class Shape
    {
        public Material material;

        public abstract Vector3 GetNormal(Vector3 point);

        public abstract Box GetBoundingBox();

        public abstract float RayIntersect(Rayon ray, out Shape returnShape);
    }
}
