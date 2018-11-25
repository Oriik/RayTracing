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
    }
}
