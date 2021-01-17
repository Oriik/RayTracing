using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Numerics;
using System.Threading;

namespace SyntheseImage
{
    class Program
    {
        static void Main(string[] args)
        {            
            List<Bitmap> bitmaps = new List<Bitmap>();
            List<Bitmap> bitmaps2 = new List<Bitmap>();

            int rayByPixel = 1;
            int halfNbFrames = 2;
                        
            var thread = new Thread(() =>
            {
                for (int i = 0; i < halfNbFrames; i++)
                {
                    bitmaps.Add(CreateBitmap(i, halfNbFrames, rayByPixel));
                }
            });

            var thread2 = new Thread(() =>
            {
                for (int i = halfNbFrames; i < halfNbFrames*2; i++)
                {
                    bitmaps2.Add(CreateBitmap(i, halfNbFrames, rayByPixel));
                }
            });

            thread.Start();
            thread2.Start();
            thread.Join();
            thread2.Join();

            bitmaps.AddRange(bitmaps2);

            using (AnimatedGif.AnimatedGifCreator gif = AnimatedGif.AnimatedGif.Create("gif.gif", 100))
            {
                foreach (Bitmap bmpImage in bitmaps)
                {
                    gif.AddFrame(bmpImage);
                }
            }          
        }

        private static Scene GetScene(int frameNumber, int halfTotalFrames)
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

            scene.lights.Clear();
            Light light1 = new Light(new Vector3(900, 300, 200), new Vector3(500000, 500000, 500000));
            Light[] lights = { light1 };
            scene.lights.AddRange(lights);

            string[] lines = File.ReadAllLines(Directory.GetParent(Directory.GetParent(Environment.CurrentDirectory).FullName) + "/cube.stl"); //HOME

            scene.shapes.Clear();

            char[] paramSplit = new char[1] { ' ' };
            Vector3 translation;
            if (frameNumber <= halfTotalFrames)
            {
                translation = new Vector3(550 - frameNumber, 450 - frameNumber, 300 - frameNumber);
            }
            else
            {
                translation = new Vector3(550 - (2 * halfTotalFrames - frameNumber), 450 - (2 * halfTotalFrames - frameNumber), 300 - (2 * halfTotalFrames - frameNumber));
            }
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

            Sphere sphere = new Sphere(new Vector3(800, 250, 500), 200f, new Material(Materials.Mirror, Material.White));
            scene.shapes.Add(sphere);

            return scene;
        }

        private static Bitmap CreateBitmap(int frameNumber, int halfTotalFrames, int rayByPixel)
        {
            Console.WriteLine("Working on frame : " + frameNumber);

            Scene scene = GetScene(frameNumber, halfTotalFrames);

            Image img3D = scene.DrawImg(rayByPixel);     
            return img3D.WriteBMP();
        }                
    }
}

