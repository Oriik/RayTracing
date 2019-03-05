using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace SyntheseImage
{
    class Program
    {
        static void Main(string[] args)
        {
            // createPPM("glass");
            List<Bitmap> bitmaps = new List<Bitmap>();
            List<Bitmap> bitmaps2 = new List<Bitmap>();


            var thread = new Thread(() =>
            {
                for (int i = 0; i < 5; i++)
                {
                    bitmaps.Add(createBitmap(i));
                }
            });

            var thread2 = new Thread(() =>
            {
                for (int i = 5; i < 10; i++)
                {
                    bitmaps2.Add(createBitmap(i));
                }
            });

            thread.Start();
            thread2.Start();
            thread.Join();
            thread2.Join();

            bitmaps.AddRange(bitmaps2);

            using (var gif = AnimatedGif.AnimatedGif.Create("gif.gif", 100))
            {
                foreach (Bitmap bmpImage in bitmaps)
                {
                    gif.AddFrame(bmpImage);
                }
            }

            //System.Diagnostics.Process.Start(@"C:\Users\Guill\Documents\Gamagora\SyntheseImage\SyntheseImage\bin\Debug\" + img3D.fileName); //HOME
            //System.Diagnostics.Process.Start(@"C:\Users\guquiniou\Documents\GitHub\SyntheseImage\bin\Release\" + img3D.fileName); // FAC


        }

        private static Bitmap createBitmap(int frameNumber)
        {

            Console.WriteLine("Start thread " + frameNumber);
            Camera camera = new Camera(new Vector3(0, 0, 0), 1280, 720, new Vector3(0, 0, 1), 1000);
            Scene scene = new Scene(camera);

            Sphere leftWall = new Sphere(new Vector3((float)-1e5 - 100, 360, 500), (float)1e5,
              new Material(Materials.Difuse, Material.Green));
            Sphere rightWall = new Sphere(new Vector3((float)1e5 + 1380, 360, 500), (float)1e5,
                new Material(Materials.Difuse, Material.Blue));
            Sphere topWall = new Sphere(new Vector3(640, (float)-1e5 - 100, 500), (float)1e5,
                new Material(Materials.Difuse, Material.Pink));
            Sphere bottomWall = new Sphere(new Vector3(640, (float)1e5 + 820, 500), (float)1e5,
                new Material(Materials.Difuse, Material.White));
            Sphere backWall = new Sphere(new Vector3(640, 360, (float)1e5 + 1100), (float)1e5,
                new Material(Materials.Difuse, Material.Yellow));
            Sphere frontWall = new Sphere(new Vector3(640, 360, (float)-1e5 - 100), (float)1e5,
                new Material(Materials.Difuse, Material.Red));


            Shape[] walls = { bottomWall, leftWall, rightWall, topWall, backWall, frontWall };
            scene.walls.AddRange(walls);
            List<Bitmap> bitmaps = new List<Bitmap>();

            scene.lights.Clear();
            Light light1 = new Light(new Vector3(800 + frameNumber * 5, 200 + frameNumber * 5, 800 + frameNumber * 5), new Vector3(500000, 500000, 500000));
            Light[] lights = { light1 };
            scene.lights.AddRange(lights);

            string[] lines = System.IO.File.ReadAllLines(@"C:\Users\Guill\Documents\Gamagora\SyntheseImage\SyntheseImage\test.stl"); //HOME
                                                                                                                                     // string[] lines = System.IO.File.ReadAllLines(@" C:\Users\guquiniou\Documents\GitHub\SyntheseImage\test.stl"); //FAC

            scene.shapes.Clear();

            char[] paramSplit = new char[1] { ' ' };
            Vector3 translation = new Vector3(350 - frameNumber * 2, 300 - frameNumber * 2, 400 - frameNumber * 2);
            for (int j = 0; j < lines.Length; j++)
            {
                if (lines[j].Contains("outer loop"))
                {
                    string[] t = lines[j + 1].Split(paramSplit);

                    Vector3 a = new Vector3(float.Parse(t[1].Replace('.', ',')), float.Parse(t[3].Replace('.', ',')), float.Parse(t[2].Replace('.', ',')));
                    t = lines[j + 2].Split(paramSplit);
                    Vector3 b = new Vector3(float.Parse(t[1].Replace('.', ',')), float.Parse(t[3].Replace('.', ',')), float.Parse(t[2].Replace('.', ',')));
                    t = lines[j + 3].Split(paramSplit);
                    Vector3 c = new Vector3(float.Parse(t[1].Replace('.', ',')), float.Parse(t[3].Replace('.', ',')), float.Parse(t[2].Replace('.', ',')));
                    Triangle temp = new Triangle(a, b, c, new Material(Materials.Difuse, Material.Red));
                    temp.Translate(translation);

                    scene.shapes.Add(temp);
                    j += 3;
                }
            }


            Image img3D = scene.DrawImg(5, frameNumber.ToString());
            img3D.WritePPM();
            return img3D.WriteBMP();

        }
        private static void createPPM(string filename)
        {
           
            Camera camera = new Camera(new Vector3(0, 0, 0), 1280, 720, new Vector3(0, 0, 1), 1000);
            Scene scene = new Scene(camera);

            Sphere leftWall = new Sphere(new Vector3((float)-1e5 - 100, 360, 500), (float)1e5,
              new Material(Materials.Difuse, Material.Green));
            Sphere rightWall = new Sphere(new Vector3((float)1e5 + 1380, 360, 500), (float)1e5,
                new Material(Materials.Difuse, Material.Blue));
            Sphere topWall = new Sphere(new Vector3(640, (float)-1e5 - 100, 500), (float)1e5,
                new Material(Materials.Difuse, Material.Pink));
            Sphere bottomWall = new Sphere(new Vector3(640, (float)1e5 + 820, 500), (float)1e5,
                new Material(Materials.Difuse, Material.White));
            Sphere backWall = new Sphere(new Vector3(640, 360, (float)1e5 + 1100), (float)1e5,
                new Material(Materials.Difuse, Material.Yellow));
            Sphere frontWall = new Sphere(new Vector3(640, 360, (float)-1e5 - 100), (float)1e5,
                new Material(Materials.Difuse, Material.Red));


            Shape[] walls = { bottomWall, leftWall, rightWall, topWall, backWall, frontWall };
            scene.walls.AddRange(walls);
            List<Bitmap> bitmaps = new List<Bitmap>();

            scene.lights.Clear();
            Light light1 = new Light(new Vector3(800 , 200 , 800 ), new Vector3(500000, 500000, 500000));
            Light[] lights = { light1 };
            scene.lights.AddRange(lights);

            string[] lines = System.IO.File.ReadAllLines(@"C:\Users\Guill\Documents\Gamagora\SyntheseImage\SyntheseImage\test.stl"); //HOME
                                                                                                                                     // string[] lines = System.IO.File.ReadAllLines(@" C:\Users\guquiniou\Documents\GitHub\SyntheseImage\test.stl"); //FAC

            scene.shapes.Clear();

            char[] paramSplit = new char[1] { ' ' };
            Vector3 translation = new Vector3(350 , 300 , 400);
            for (int j = 0; j < lines.Length; j++)
            {
                if (lines[j].Contains("outer loop"))
                {
                    string[] t = lines[j + 1].Split(paramSplit);

                    Vector3 a = new Vector3(float.Parse(t[1].Replace('.', ',')), float.Parse(t[3].Replace('.', ',')), float.Parse(t[2].Replace('.', ',')));
                    t = lines[j + 2].Split(paramSplit);
                    Vector3 b = new Vector3(float.Parse(t[1].Replace('.', ',')), float.Parse(t[3].Replace('.', ',')), float.Parse(t[2].Replace('.', ',')));
                    t = lines[j + 3].Split(paramSplit);
                    Vector3 c = new Vector3(float.Parse(t[1].Replace('.', ',')), float.Parse(t[3].Replace('.', ',')), float.Parse(t[2].Replace('.', ',')));
                    Triangle temp = new Triangle(a, b, c, new Material(Materials.Glass, Material.White));
                    temp.Translate(translation);

                    scene.shapes.Add(temp);
                    j += 3;
                }
            }

            Image img3D = scene.DrawImg(10, filename);
            img3D.WritePPM();

        }
    }
}

