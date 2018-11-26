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
            Light light = new Light(new Vector3(600, 200, 500), new Vector3(1000000, 1000000, 1000000));
            Scene scene = new Scene(camera, light);

            Sphere leftWall = new Sphere(new Vector3((float)-1e5 - 100, 360, 500), (float)1e5,
                new Material(Materials.Difuse, Material.Green));
            Sphere rightWall = new Sphere(new Vector3((float)1e5 + 1380, 360, 500), (float)1e5,
                new Material(Materials.Difuse, Material.Blue));
            Sphere topWall = new Sphere(new Vector3(640, (float)-1e5 - 100, 500), (float)1e5,
                new Material(Materials.Difuse, Material.Pink));
            Sphere bottomWall = new Sphere(new Vector3(640, (float)1e5 + 820, 500), (float)1e5,
                new Material(Materials.Difuse, Material.Red));
            Sphere backWall = new Sphere(new Vector3(640, 360, (float)1e5 + 1100), (float)1e5,
                new Material(Materials.Difuse, Material.Yellow));
            Sphere frontWall = new Sphere(new Vector3(640, 360, (float)-1e5 - 100), (float)1e5,
                new Material(Materials.Difuse, Material.Red));


            Shape[] walls = { frontWall, backWall, rightWall, leftWall, bottomWall, topWall };
            scene.walls.AddRange(walls);

            // string[] lines = System.IO.File.ReadAllLines(@"C:\Users\Guill\Documents\Gamagora\SyntheseImage\SyntheseImage\trex.stl"); //HOME
            string[] lines = System.IO.File.ReadAllLines(@" C:\Users\guquiniou\Documents\GitHub\SyntheseImage\test.stl"); //FAC
           

            char[] paramSplit = new char[1] { ' ' };
            Vector3 translation = new Vector3(350, 300, 400);
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
                    Triangle temp = new Triangle(a, b, c, new Material(Materials.Difuse, Material.Green));
                    temp.Translate(translation);

                    scene.shapes.Add(temp);
                    i += 3;

                }
            }

            Sphere s = new Sphere(new Vector3(200, 400, 200), 50, new Material(Materials.Mirror, Material.White));
            scene.shapes.Add(s);

           

           Image img3D = scene.DrawImg(10);
           img3D.WritePPM();

            //System.Diagnostics.Process.Start(@"C:\Users\Guill\Documents\Gamagora\SyntheseImage\SyntheseImage\bin\Debug\" + img3D.fileName); //HOME
            System.Diagnostics.Process.Start(@"C:\Users\guquiniou\Documents\GitHub\SyntheseImage\bin\Release\" + img3D.fileName); // FAC

            //Console.ReadLine();


        }
    }
}

