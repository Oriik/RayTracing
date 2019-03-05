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
        #region Variables
        private bool m_leaf;
        private Shape m_shape;
        private Box m_box;
        private Tree m_tree1, m_tree2;
        #endregion

        public Tree(Shape s)
        {
            m_leaf = true;
            m_shape = s;            
        }
        public Tree(Box b, Tree t1, Tree t2)
        {
            m_box = b;
            m_tree1 = t1;
            m_tree2 = t2;
            m_leaf = false;
        }

        public override Box GetBoundingBox()
        {
            if (m_leaf) return m_shape.GetBoundingBox();
            else return m_box;
        }

        public override Vector3 GetNormal(Vector3 point)
        {
            return Vector3.Zero;
        }

        public override float RayIntersect(Rayon ray, out Shape returnShape)
        {
            if (GetBoundingBox().RayIntersect(ray))
            {
                if (m_leaf)
                {
                    return  this.m_shape.RayIntersect(ray, out returnShape);
                }
                else
                {
                    Shape t1Shape, t2Shape;
                    float t1 = m_tree1.RayIntersect(ray, out t1Shape);
                    float t2 = m_tree2.RayIntersect(ray, out t2Shape);
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
