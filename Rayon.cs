using System;
using System.Numerics;

namespace SyntheseImage
{
    public class Rayon
    {

        public Vector3 origine;
        public Vector3 direction;

        public Rayon(Vector3 _origine, Vector3 _direction)
        {
            origine = _origine;
            direction = _direction;

        }

        public bool IntersectABox(Box box)
        {
            float tmin = (box.pMin.X - origine.X) / direction.X;
            float tmax = (box.pMax.X - origine.X) / direction.X;

            if (tmin > tmax)
            {
                float temp = tmin;
                tmin = tmax;
                tmax = temp;
            }

            float tymin = (box.pMin.Y - origine.Y) / direction.Y;
            float tymax = (box.pMax.Y - origine.Y) / direction.Y;

            if (tymin > tymax)
            {
                float temp = tymin;
                tymin = tymax;
                tymax = temp;                
            }

            if ((tmin > tymax) || (tymin > tmax))
                return false;

            if (tymin > tmin)
                tmin = tymin;

            if (tymax < tmax)
                tmax = tymax;

            float tzmin = (box.pMin.Z - origine.Z) / direction.Z;
            float tzmax = (box.pMax.Z - origine.Z) / direction.Z;

            if (tzmin > tzmax)
            {
                float temp = tzmin;
                tzmin = tzmax;
                tzmax = temp;
            }

            if ((tmin > tzmax) || (tzmin > tmax))
                return false;

            if (tzmin > tmin)
                tmin = tzmin;

            if (tzmax < tmax)
                tmax = tzmax;

            return true;


        }
        public float IntersectAShape(Shape shape, out Shape shape1)
        {
            shape1 = null;
            if (shape is Sphere)
            {
                return IntersectASphere((Sphere)shape, out shape1);
            }
            if (shape is Triangle)
            {
                return IntersectATriangle((Triangle)shape, out shape1);
            }
            if (shape is Tree)
            {
                return IntersectATree((Tree)shape, out shape1);
            }
            return -1.0f;
        }

        private float IntersectATree(Tree tree, out Shape shape)
        {
            if (IntersectABox(tree.GetBoundingBox()))
            {
                if (tree.leaf)
                {
                    return IntersectAShape(tree.shape, out shape);
                }
                else
                {
                    float t1 = IntersectAShape(tree.tree1, out shape);
                    float t2 = IntersectAShape(tree.tree2, out shape);
                    if (t1 == -1.0f)
                    {
                        if (t2 == -1.0f)
                        {
                            shape = null;
                            return -1.0f;
                        }
                        else
                        {
                            return IntersectAShape(tree.tree2, out shape);
                        }
                    }
                    else
                    {
                        if (t2 == -1.0f)
                        {
                            return IntersectAShape(tree.tree1, out shape);
                        }
                        else
                        {
                            if (t1 < t2) return IntersectAShape(tree.tree1, out shape);
                            else return IntersectAShape(tree.tree2, out shape);
                        }
                    }

                }
            }
            else
            {
                shape = null;
                return -1.0f;
            }
        }

        private float IntersectASphere(Sphere sphere, out Shape shape1)
        //RETURN -1 IF NO INTERSECT
        {
            shape1 = null;
            float A = Vector3.Dot(direction, direction);
            float B = 2 * (Vector3.Dot(origine, direction) - Vector3.Dot(sphere.center, direction));
            float C = Vector3.Dot(Vector3.Subtract(sphere.center, origine), Vector3.Subtract(sphere.center, origine)) - (sphere.radius * sphere.radius);
            float D = B * B - 4 * A * C;
            if (D < 0)
                return -1.0f;
            else
            {
                float i1 = ((-B) + (float)Math.Sqrt(D)) / (2 * A);
                float i2 = ((-B) - (float)Math.Sqrt(D)) / (2 * A);
                if (i2 > 0)
                {
                    shape1 = sphere;
                    return i2;
                }

                else if (i1 > 0)
                {
                    shape1 = sphere;
                    return i1;
                }

                else
                    return -1.0f;
            }


        }

        private float IntersectATriangle(Triangle triangle, out Shape shape1) //ALGO MOLLER TRUMBORE
                                                                              //RETURN -1 IF NO INTERSECT
        {
            shape1 = null;
            Vector3 h = Vector3.Cross(direction, triangle.v);
            float a = Vector3.Dot(triangle.u, h);
            if (a > -float.Epsilon && a < float.Epsilon) { return -1.0f; }
            float f = 1.0f / a;
            Vector3 s = origine - triangle.a;
            float u = f * (Vector3.Dot(s, h));
            if (u < 0 || u > 1.0) { return -1.0f; }
            Vector3 q = Vector3.Cross(s, triangle.u);
            float v = f * Vector3.Dot(direction, q);
            if (v < 0 || u + v > 1) { return -1.0f; }
            float t = f * Vector3.Dot(triangle.v, q);

            if (t > float.Epsilon)
            {
                shape1 = triangle;
                return t;
            }
            else { return -1.0f; }
        }

        public Vector3 GetPointAt(float coeff)
        {
            return Vector3.Add(origine, Vector3.Multiply(coeff, direction));
        }
    }
}