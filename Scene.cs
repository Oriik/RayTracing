using System;
using System.Collections.Generic;
using System.Numerics;

namespace SyntheseImage
{
    public class Scene
    {
        public List<Sphere> spheres;
        public Camera cam;
        public Light light;


        public Scene(Camera _cam, Light _light)
        {
            cam = _cam;
            light = _light;
            spheres = new List<Sphere>();

        }

        public Image DrawImg()
        {
            Image img = new Image(cam.width, cam.height, "toto3D");

            for (int y = (int)cam.o.Y; y < cam.o.Y + cam.height; y++)
                for (int x = (int)cam.o.X; x < cam.o.X + cam.width; x++)                
                {
                    Vector3 pointOnCam = new Vector3(x, y, cam.o.Z);
                    Rayon rFromCam = new Rayon(pointOnCam, cam.GetFocusAngle(x, y));
                    float temp = float.MaxValue;
                    Sphere sphere = null;
                    foreach (Sphere s in spheres)
                    {
                        if (rFromCam.IntersectASphere(s) != -1 && rFromCam.IntersectASphere(s) < temp)
                        {
                            temp = rFromCam.IntersectASphere(s);
                            sphere = s;
                        }
                    }
                    //Si on a rencontré une sphère
                    if (temp != float.MaxValue)
                    {
                        Vector3 pointOnSphere = Vector3.Add(pointOnCam, Vector3.Multiply(temp, cam.d));

                        //On décale i un tout petit peu vers l'extérieur de la sphère pour être sur de pas être dans la sphère.
                        //On calcule le vecteur centreSphere->pointSphere et on le normalise
                        Vector3 directionTemp = Vector3.Normalize(Vector3.Subtract(pointOnSphere, sphere.center));  

                        pointOnSphere = Vector3.Add(pointOnSphere, Vector3.Multiply(directionTemp,0.1f));

                        //On créé un rayon de la sphère jusqu'à la lumière
                        Rayon r2 = new Rayon(pointOnSphere, Vector3.Subtract(light.origine, pointOnSphere));
                        bool seeTheLight = true;
                        foreach (Sphere s in spheres)
                        {
                            //Si on croise une sphère avant d'arriver à la lumière
                            if (r2.IntersectASphere(s) != -1 && Vector3.Distance(Vector3.Multiply(r2.IntersectASphere(s), r2.d), r2.o) < Vector3.Distance(light.origine, r2.o))
                            {
                                seeTheLight = false;
                                break;
                            }

                        }
                        float dis = Vector3.Distance(pointOnSphere, light.origine);
                        Vector3 a = Vector3.Multiply(light.power, 1 / (dis * dis));
                        float cos = Vector3.Dot(Vector3.Normalize(r2.d), directionTemp);
                        Vector3 b = Vector3.Multiply(sphere.couleur.rgb, (float)(cos / Math.PI));
                        Vector3 c = Vector3.Multiply(a, b);
                        if (seeTheLight)
                        {


                            img.SetPixel(x, y, c.X, c.Y, c.Z);
                        }
                        else
                        {
                            img.SetPixel(x, y, 0,0,0);
                        }
                    }
                }

            return img;
        }

    }
}