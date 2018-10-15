﻿using System;
using System.Numerics;

namespace SyntheseImage
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Working ...");

            Camera camera = new Camera(new Vector3(0, 0, 0), 1280, 720, new Vector3(0, 0, 1), 1000);
            Light light = new Light(new Vector3(600, 400, 500), new Vector3(1000000, 1000000, 1000000));
            Scene scene = new Scene(camera, light);

            Sphere s1 = new Sphere(new Vector3(1050, 500, 800), 100,
                 new Material(Materials.Mirror, new Vector3(1f, 1f, 1f)));
            Sphere s2 = new Sphere(new Vector3(200, 200, 300), 100,
                new Material(Materials.Difuse, new Vector3(0.35f, 0.8f, 1f)));
            Sphere s3 = new Sphere(new Vector3(1000, 100, 650), 100,
                new Material(Materials.Difuse, new Vector3(0.45f, 0.75f, 0.3f)));

            

            Sphere leftWall = new Sphere(new Vector3((float)-1e5-100, 360, 500), (float)1e5,
                new Material(Materials.Difuse, new Vector3(0.8f, 0.8f, 0.2f))); //JAUNE
            Sphere rightWall = new Sphere(new Vector3((float)1e5 + 1380, 360, 500), (float)1e5,
                new Material(Materials.Difuse, new Vector3(0.2f, 0.8f, 0.8f))); //BLEU
            Sphere topWall = new Sphere(new Vector3(640, (float)-1e5-100, 500), (float)1e5, 
                new Material(Materials.Difuse, new Vector3(0.8f, 0.2f, 0.8f))); //ROSE
            Sphere bottomWall = new Sphere(new Vector3(640, (float)1e5+820 , 500), (float)1e5, 
                new Material(Materials.Difuse, new Vector3(0.8f, 0.2f, 0.2f))); //ROUGE
            Sphere backWall = new Sphere(new Vector3(640, 360, (float)1e5  +1100), (float)1e5,
                new Material(Materials.Difuse, new Vector3(0.2f, 1f, 0.2f))); //BLANC
            Sphere frontWall = new Sphere(new Vector3(640, 360, (float)-1e5 -100), (float)1e5, 
                new Material(Materials.Difuse, new Vector3(0.8f, 0.8f, 0.8f))); // BLANC

            Sphere[] spheres = { frontWall, backWall, rightWall, leftWall, bottomWall, topWall, s1, s2, s3};
            scene.spheres.AddRange(spheres);
            

            //scene.spheres.Add(new Sphere(new Vector3(1000, 1000, 500), 100, new Couleur(new Vector3(1, 0.8f, 0.2f))));



            Image img3D = scene.DrawImg();
            img3D.WritePPM();
           // System.Diagnostics.Process.Start(@"C:\Users\Guill\Documents\Gamagora\SyntheseImage\SyntheseImage\bin\Debug\" + img3D.fileName); //HOME
            System.Diagnostics.Process.Start(@"C:\Users\guquiniou\Documents\GitHub\SyntheseImage\bin\Release\" + img3D.fileName); // FAC
            //Console.ReadLine();



        }
    }
}
