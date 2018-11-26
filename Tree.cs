using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SyntheseImage
{
    public class Tree : Shape
    {
        public bool leaf;
        public Shape shape;
        public Box box;
        public Tree tree1, tree2;

        public Tree(Shape s)
        {
            leaf = true;
            shape = s;            
        }
        public Tree(Box b, Tree t1, Tree t2)
        {
            box = b;
            tree1 = t1;
            tree2 = t2;
            leaf = false;
        }

        public override Box GetBoundingBox()
        {
            if (leaf) return shape.GetBoundingBox();
            else return box;
        }

        public override Vector3 GetNormal(Vector3 point)
        {
            return Vector3.Zero;
        }

        public override float RayIntersect(Rayon ray, out Shape returnShape)
        {
            if (GetBoundingBox().RayIntersect(ray))
            {
                if (leaf)
                {
                    return  this.shape.RayIntersect(ray, out returnShape);
                }
                else
                {
                    Shape t1Shape, t2Shape;
                    float t1 = tree1.RayIntersect(ray, out t1Shape);
                    float t2 = tree2.RayIntersect(ray, out t2Shape);
                    if (t1 == -1.0f)
                    {
                        if (t2 == -1.0f)
                        {
                            returnShape = null;
                            return -1.0f;
                        }
                        else
                        {
                            returnShape = t2Shape;
                            return t2;
                        }
                    }
                    else
                    {
                        if (t2 == -1.0f)
                        {
                            returnShape = t1Shape;
                            return t1;
                        }
                        else
                        {
                            if (t1 < t2)
                            {
                                returnShape = t1Shape;
                                return t1;
                            }
                            else
                            {
                                returnShape = t2Shape;
                                return t2;
                            }
                        }
                    }

                }
            }
            else
            {
                returnShape = null;
                return -1.0f;
            }
        }
    }
}
