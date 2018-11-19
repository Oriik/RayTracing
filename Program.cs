using System;
using System.Collections;
using System.Numerics;

namespace SyntheseImage
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Working ...");

            Camera camera = new Camera(new Vector3(0, 0, 0), 1280, 720, new Vector3(0, 0, 1), 1000);
            Light light = new Light(new Vector3(600, 200, 200), new Vector3(1000000, 1000000, 1000000));
            Scene scene = new Scene(camera, light);

            Sphere leftWall = new Sphere(new Vector3((float)-1e5 - 100, 360, 500), (float)1e5,
                new Material(Materials.Mirror, Material.White));
            Sphere rightWall = new Sphere(new Vector3((float)1e5 + 1380, 360, 500), (float)1e5,
                new Material(Materials.Difuse, Material.Blue));
            Sphere topWall = new Sphere(new Vector3(640, (float)-1e5 - 100, 500), (float)1e5,
                new Material(Materials.Difuse, Material.Pink));
            Sphere bottomWall = new Sphere(new Vector3(640, (float)1e5 + 820, 500), (float)1e5,
                new Material(Materials.Difuse, Material.Red));
            Sphere backWall = new Sphere(new Vector3(640, 360, (float)1e5 + 1100), (float)1e5,
                new Material(Materials.Difuse, Material.Green));
            Sphere frontWall = new Sphere(new Vector3(640, 360, (float)-1e5 - 100), (float)1e5,
                new Material(Materials.Difuse, Material.Green));

            //Shape[] shapes = { frontWall, backWall, rightWall, leftWall, bottomWall, topWall,
            //    t1, t2,t3,t4,t5,t6,t7,t8,t9,t10,t11,t12,
            //tt1,tt2,tt3,tt4,tt5,tt6,tt7,tt8,tt9,tt10,tt11,tt12};
            Shape[] shapes = { frontWall, backWall, rightWall, leftWall, bottomWall, topWall};
            scene.shapes.AddRange(shapes);

            string[] lines = System.IO.File.ReadAllLines(@"C:\Users\guquiniou\Documents\GitHub\SyntheseImage\trex.stl");

            char[] paramSplit = new char[1] { ' ' };
            Vector3 translation = new Vector3(800, 300, 400);
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains("outer loop"))
                {

                    string[] t = lines[i + 1].Split(paramSplit);

                    Vector3 a = new Vector3(float.Parse(t[1].Replace('.', ',')), float.Parse(t[3].Replace('.', ',')), float.Parse(t[2].Replace('.', ',')));
                    t = lines[i + 2].Split(paramSplit);
                    Vector3 b = new Vector3(float.Parse(t[1].Replace('.', ',')), float.Parse(t[3].Replace('.', ',')), float.Parse(t[2].Replace('.', ',')));
                    t = lines[i + 3].Split(paramSplit);
                    Vector3 c = new Vector3(float.Parse(t[1].Replace('.', ',')), float.Parse(t[3].Replace('.', ',')), float.Parse(t[2].Replace('.', ',')));
                    Triangle temp = new Triangle(a, b, c, new Material(Materials.Difuse, Material.Blue));
                    temp.Translate(translation);
                    Console.WriteLine(temp.a);
                    scene.shapes.Add(temp);
                    i += 3;

                }
            }


           
            Image img3D = scene.DrawImg(1);
            img3D.WritePPM();

            //  System.Diagnostics.Process.Start(@"C:\Users\Guill\Documents\Gamagora\SyntheseImage\SyntheseImage\bin\Debug\" + img3D.fileName); //HOME
             System.Diagnostics.Process.Start(@"C:\Users\guquiniou\Documents\GitHub\SyntheseImage\bin\Release\" + img3D.fileName); // FAC
            //Console.ReadLine();



        }
    }
}

