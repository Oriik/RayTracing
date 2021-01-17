using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace SyntheseImage
{
    public class Scene
    {
        #region Variables
        public List<Shape> shapes;
        public List<Shape> walls;
        public List<Light> lights;

        private List<Shape> m_fastStruct;
        private Camera m_cam;
        private Tree m_tree;
        private Random m_random;

        private struct ResFindShape
        {
            public float coeff;
            public Shape shape;
        }
        #endregion

        public Scene(Camera _cam)
        {
            m_cam = _cam;
            lights = new List<Light>();
            shapes = new List<Shape>();
            walls = new List<Shape>();
            m_random = new Random();
        }

        public Image DrawImg(int rayPerPixels)
        {
            Image img = new Image(m_cam.width, m_cam.height);

            m_tree = CreateTree(shapes);

            m_fastStruct = new List<Shape>(walls){ m_tree };

            for (int x = 0; x < m_cam.width; x++)
            {
                for (int y = 0; y < m_cam.height; y++)
                {
                    Vector3 pointOnCam = new Vector3(m_cam.origin.X + x, m_cam.origin.Y + y, m_cam.origin.Z);
                    Vector3 pixelColor = new Vector3(0, 0, 0);
                    for (int i = 0; i < rayPerPixels; i++)
                    {
                        pointOnCam = Camera.AntiAliasing(pointOnCam, m_random);
                        Ray rFromCam = new Ray(pointOnCam, m_cam.GetFocusAngle(x, y));
                        pixelColor = Vector3.Add(pixelColor, SendRay(rFromCam));
                    }

                    pixelColor = Vector3.Divide(pixelColor, rayPerPixels);
                    img.SetPixel(x, y, pixelColor.X, pixelColor.Y, pixelColor.Z);
                }
            }

            return img;
        }


        private Vector3 SendRay(Ray rFromCam, int cpt = 0)
        {
            ResFindShape res = SearchShapeHit(rFromCam);

            //If we hit a shape
            if (res.coeff != float.MaxValue && res.shape != null)
            {
                cpt++;
                Vector3 pointOnShape = rFromCam.GetPointAt(res.coeff);

                // Move hit point slightly on the outside to be sure we're not inside the shape
                Vector3 normalOnPointOnShape = res.shape.GetNormal(pointOnShape);
                Vector3 pointOnShapeDecal = Vector3.Add(pointOnShape, Vector3.Multiply(normalOnPointOnShape, 0.5f));

                Vector3 indirectLight = new Vector3(0, 0, 0);

                if (cpt < 10) // Bounce
                {
                    Vector3 newDir;

                    if (res.shape.material.mat == Materials.Mirror)
                    {
                        //Reflexion direction
                        newDir = Vector3.Add(
                            Vector3.Multiply(2 * -Vector3.Dot(rFromCam.direction, normalOnPointOnShape), normalOnPointOnShape)
                            , rFromCam.direction);

                        indirectLight = res.shape.material.albedo * IndirectLightning(pointOnShapeDecal, newDir, res, cpt);
                    }
                    else if (res.shape.material.mat == Materials.Glass && m_random.Next(2) == 0)
                    {
                        newDir = Vector3.Add(
                           Vector3.Multiply(2 * -Vector3.Dot(rFromCam.direction, normalOnPointOnShape), normalOnPointOnShape)
                           , rFromCam.direction);

                        indirectLight = res.shape.material.albedo * IndirectLightning(pointOnShapeDecal, newDir, res, cpt);
                    }
                    else
                    {
                        newDir = RandomBounce(pointOnShapeDecal);
                        indirectLight = IndirectLightning(pointOnShapeDecal, newDir, res, cpt);
                    }
                }

                if (res.shape.material.mat != Materials.Mirror)
                {
                    Vector3 temp = Vector3.Zero;
                    foreach (Light light in lights)
                    {
                        Vector3 l = Vector3.Subtract(light.origine, pointOnShapeDecal);
                        float dist = l.Length();
                        l = Vector3.Normalize(l);

                        Vector3 lightEmmited = Vector3.Divide(
                            Vector3.Multiply(res.shape.material.albedo, Math.Max(Math.Min(Vector3.Dot(normalOnPointOnShape, l), 1.0f), 0.0f))
                            , (float)Math.PI);
                        Vector3 directLight = DirectLightning(pointOnShapeDecal, res, dist, light);

                        temp += Vector3.Add(Vector3.Multiply(lightEmmited, directLight), indirectLight);

                    }
                    return temp;
                }
                else
                {
                    return indirectLight;
                }
            }

            return new Vector3(0.1f, 0.1f, 0.25f); //Nothing is hit, nothing to return, so return atmosphere value  --ATMOSPHERE
        }

        private Vector3 DirectLightning(Vector3 point, ResFindShape res, float dist, Light light)
        {
            //Create a ray from the shape to the light
            Ray r2 = new Ray(point, Vector3.Subtract(light.origine, point));
            bool seeTheLight = true;
            foreach (Shape s in m_fastStruct)
            {
                float coeff = s.RayIntersect(r2, out res.shape);

                //If we hit something before the light
                if (coeff != -1 && coeff < 1)
                {
                    seeTheLight = false;
                    break;
                }
            }

            if (seeTheLight)
            {
                Vector3 powerReceived = Vector3.Multiply(light.power, 1 / (dist * dist));
                return powerReceived; 
            }
            else
            {
                return new Vector3(0, 0, 0);
            }
        }

        private Vector3 IndirectLightning(Vector3 point, Vector3 dir, ResFindShape res, int cpt)
        {
            Ray mirrorRayon = new Ray(point, dir);
            return Vector3.Multiply(res.shape.material.albedo, SendRay(mirrorRayon, cpt));
        }

        private Vector3 RandomBounce(Vector3 point)
        {
            double r1 = m_random.NextDouble();
            double r2 = m_random.NextDouble();
          
            float X = (float)(Math.Cos(2 * Math.PI * r1) * Math.Sqrt(1 - r2));
            float Y = (float)(Math.Sin(2 * Math.PI * r1) * Math.Sqrt(1 - r2));
            float Z = (float)Math.Sqrt(r2);

            Vector3 randomVector = new Vector3((float)m_random.NextDouble(), (float)m_random.NextDouble(), (float)m_random.NextDouble());

            Vector3 xBase = Vector3.Normalize(Vector3.Cross(point, randomVector));
            Vector3 yBase = Vector3.Cross(xBase, point);
            Vector3 zBase = point;

            return Vector3.Add(Vector3.Add(X * xBase, Y * yBase), Z * zBase);
        }

        private ResFindShape SearchShapeHit(Ray rayon)
        {
            ResFindShape res = new ResFindShape();
            res.coeff = float.MaxValue;

            foreach (Shape s in m_fastStruct)
            {
                Shape tempShape;
                float temp = s.RayIntersect(rayon, out tempShape);

                if (temp != -1 && temp < res.coeff)
                {
                    res.coeff = temp;
                    res.shape = tempShape;
                }
            }
            return res;
        }

        private Tree CreateTree(List<Shape> elements)
        {
            if (elements.Count == 1) return new Tree(elements[0]);
            else
            {
                Box b = elements[0].GetBoundingBox();
                for (int i = 1; i < elements.Count; i++)
                {
                    b = b.Fusion(elements[i].GetBoundingBox());
                }
                elements = elements.OrderBy(s => (s.GetBoundingBox().pMin.X + s.GetBoundingBox().pMin.Y + s.GetBoundingBox().pMin.Z)).ToList();

                List<Shape> leftElements = elements.GetRange(0, elements.Count / 2);
                List<Shape> rightElements;
                if (elements.Count % 2 == 0)
                {
                    rightElements = elements.GetRange(elements.Count / 2, elements.Count / 2);
                }
                else
                {
                    rightElements = elements.GetRange(elements.Count / 2, (elements.Count / 2) + 1);
                }


                return new Tree(b, CreateTree(leftElements), CreateTree(rightElements));
            }
        }

    }
}