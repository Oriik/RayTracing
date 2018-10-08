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
        private Random random;


        public Scene(Camera _cam, Light _light)
        {
            cam = _cam;
            light = _light;
            spheres = new List<Sphere>();
            random = new Random();

        }

        public Image DrawImg()
        {
            Image img = new Image(cam.width, cam.height, "toto3D");

            for (int y = 0; y < cam.height; y++)
                for (int x = 0; x < cam.width; x++)
                {
                    Vector3 pointOnCam = new Vector3(cam.o.X + x, cam.o.Y + y, cam.o.Z);
                     Vector3 pixelColor = new Vector3(0,0,0);
                     for (int i =0; i < 50; i++)
                     {
                         Rayon rFromCam = new Rayon(pointOnCam, cam.GetFocusAngle(x, y));
                        pixelColor = Vector3.Add(pixelColor,SendRayon(rFromCam));
                     }
                     pixelColor = Vector3.Divide(pixelColor, 50);
                   
                    img.SetPixel(x, y, pixelColor.X, pixelColor.Y, pixelColor.Z);

                }

            return img;
        }

        private Vector3 SendRayon(Rayon rFromCam, int cpt = 0)
        {

            ResFindSphere res = SearchSphereHit(rFromCam);

            //Si on a rencontré une sphère
            if (res.coeff != float.MaxValue)
            {
                cpt++;
                Vector3 pointOnSphere = rFromCam.GetPointAt(res.coeff);
                //On décale i un tout petit peu vers l'extérieur de la sphère pour être sur de pas être dans la sphère.
                //On calcule le vecteur centreSphere->pointSphere et on le normalise
                Vector3 directionTemp = Vector3.Normalize(Vector3.Subtract(pointOnSphere, res.sphere.center));

                Vector3 pointOnSphereDecal = Vector3.Add(pointOnSphere, Vector3.Multiply(directionTemp, 0.1f));

                Vector3 indirectLight = new Vector3(0, 0, 0);

                Vector3 l = Vector3.Subtract(light.origine, pointOnSphereDecal);
                float dist = l.Length();
                l = Vector3.Normalize(l);
                Vector3 n = Vector3.Subtract(pointOnSphereDecal, res.sphere.center);
                n = Vector3.Normalize(n);
                Vector3 lightEmmited = Vector3.Divide(Vector3.Multiply(res.sphere.material.albedo, Math.Max(Math.Min(Math.Abs(Vector3.Dot(n, l)), 1.0f), 0.0f)), (float)Math.PI);

                if (cpt < 10)
                {
                    Vector3 newDir;
                    float coef;

                    if (res.sphere.material.mat == Materials.Mirror)
                    {
                        coef = 1.0f;
                        Vector3 normal = Vector3.Subtract(pointOnSphereDecal, res.sphere.center);
                        normal = Vector3.Normalize(normal);
                        newDir = Vector3.Add(Vector3.Multiply(2 * -Vector3.Dot(rFromCam.d, normal), normal), rFromCam.d);
                    }
                    else
                    {
                        //On génère un rebond aléatoire
                        coef = 4* 3.14f;
                        double r1 = random.NextDouble();
                        double r2 = random.NextDouble();
                        float x = (float)(pointOnSphereDecal.X + 2 * Math.Cos(2 * Math.PI * r1)*Math.Sqrt(r2*(1-r2)));
                        float y = (float)(pointOnSphereDecal.Y + 2 * Math.Sin(2 * Math.PI * r1) * Math.Sqrt(r2 * (1 - r2)));
                        float z = (float)(pointOnSphereDecal.Z + (1 - 2 * r2));

                        newDir = Vector3.Subtract(new Vector3(x, y, z), pointOnSphereDecal); 
                        if(Vector3.Dot(newDir, Vector3.Subtract(pointOnSphereDecal, res.sphere.center)) < 0)
                        {
                            newDir = Vector3.Negate(newDir);
                        }
                        
                    }

                    indirectLight = lightEmmited *coef * IndirectLightning(pointOnSphereDecal, newDir, res, cpt);

                }

                if (res.sphere.material.mat != Materials.Mirror)
                {
                   

                    Vector3 directLight = DirectLightning(pointOnSphereDecal, res, dist);                    

                    return Vector3.Multiply(lightEmmited, Vector3.Add(directLight, indirectLight));
                }
                else
                {
                    return indirectLight;
                }
            }

            return new Vector3(240, 240, 240); //On ne voit rien, on retourne la couleur du fond de l'image
        }

        private Vector3 DirectLightning(Vector3 point, ResFindSphere res, float dist)
        {
            //On créé un rayon de la sphère jusqu'à la lumière
            Rayon r2 = new Rayon(point, Vector3.Subtract(light.origine, point));
            bool seeTheLight = true;
            foreach (Sphere s in spheres)
            {
                float coeff = r2.IntersectASphere(s);
                //Si on croise une sphère avant d'arriver à la lumière
                if (coeff != -1 && coeff < 1)
                {
                    seeTheLight = false;
                    break;
                }

            }

            if (seeTheLight)
            {

                Vector3 powerReceived = Vector3.Multiply(light.power, 1 / (dist * dist));

                return powerReceived;  //On voit une sphère et elle est éclairé, on retourne sa couleur
            }
            else
            {
                return new Vector3(0, 0, 0); //On voit une sphère mais elle n'est pas éclairé, on renvoie du noir
            }
        }

        private Vector3 IndirectLightning(Vector3 point, Vector3 dir, ResFindSphere res, int cpt)
        {

            Rayon mirrorRayon = new Rayon(point, dir);
            return Vector3.Multiply(res.sphere.material.albedo, SendRayon(mirrorRayon, cpt));
        }
        private ResFindSphere SearchSphereHit(Rayon rayon)
        {
            ResFindSphere res = new ResFindSphere();
            res.coeff = float.MaxValue;

            foreach (Sphere s in spheres)
            {
                float temp = rayon.IntersectASphere(s);
                if (temp != -1 && temp < res.coeff)
                {
                    res.coeff = rayon.IntersectASphere(s);
                    res.sphere = s;
                }
            }
            return res;
        }
        private struct ResFindSphere
        {
            public float coeff;
            public Sphere sphere;
        }
    }
}