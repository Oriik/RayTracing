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

            for (int y = 0; y <  cam.height; y++)
                for (int x =0; x <  cam.width; x++)
                {
                    Vector3 pointOnCam = new Vector3(cam.o.X + x, cam.o.Y+y, cam.o.Z);
                    Rayon rFromCam = new Rayon(pointOnCam, cam.GetFocusAngle(x, y));
                    Vector3 pixelColor = SendRayon(rFromCam);
                    img.SetPixel(x, y, pixelColor.X, pixelColor.Y, pixelColor.Z);                   
                    
                }

            return img;
        }

        private Vector3 SendRayon(Rayon rFromCam)
        {
            
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
                Vector3 pointOnSphere = Vector3.Add(rFromCam.o, Vector3.Multiply(temp, cam.d));

                //On décale i un tout petit peu vers l'extérieur de la sphère pour être sur de pas être dans la sphère.
                //On calcule le vecteur centreSphere->pointSphere et on le normalise
                Vector3 directionTemp = Vector3.Normalize(Vector3.Subtract(pointOnSphere, sphere.center));

                pointOnSphere = Vector3.Add(pointOnSphere, Vector3.Multiply(directionTemp, 0.1f));

                //On créé un rayon de la sphère jusqu'à la lumière
                Rayon r2 = new Rayon(pointOnSphere, Vector3.Subtract(light.origine, pointOnSphere));
                bool seeTheLight = true;
                foreach (Sphere s in spheres)
                {
                    //Si on croise une sphère avant d'arriver à la lumière
                    if (r2.IntersectASphere(s) != -1 && r2.IntersectASphere(s) <= 1.0f) 
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
                    Vector3 l = Vector3.Subtract(light.origine, pointOnSphere);
                    float dist = l.Length();
                    l = Vector3.Normalize(l);
                    Vector3 n = Vector3.Subtract(pointOnSphere, sphere.center);
                    n = Vector3.Normalize(n);

                    Vector3 powerReceived = Vector3.Multiply(light.power, 1 / (dist * dist));
                    Vector3 lightEmmited = Vector3.Divide(Vector3.Multiply(sphere.couleur.rgb, Math.Max(Math.Min(Vector3.Dot(n, l), 1.0f), 0.0f)), (float)Math.PI);

                   // return Vector3.Multiply(powerReceived, lightEmmited);

                    return c; //On voit une sphère et elle est éclairé, on retourne sa couleur
                }
                else
                {
                    return new Vector3(0, 0, 0); //On voit une sphère mais elle n'est pas éclairé, on renvoie du noir
                }
            }

            return new Vector3(240, 240, 240); //On ne voit rien, on retourne la couleur du fond de l'image
        }
    }
}