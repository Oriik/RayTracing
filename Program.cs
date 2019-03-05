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
            List<Bitmap> bitmaps = new List<Bitmap>();


            for (int i = 1; i < 3; i++)
            {

                var thread = new Thread(() =>
                {
                    bitmaps.Add(createGif(1*i +i));
                });
                thread.Start();
                var thread2 = new Thread(() =>
                {
                    bitmaps.Add(createGif(2*i +i));
                });
                thread2.Start();
                var thread3 = new Thread(() =>
                {
                    bitmaps.Add(createGif(3*i +i));
                });
                thread3.Start();
                var thread4 = new Thread(() =>
                {
                    bitmaps.Add(createGif(4*i +i));
                });
                thread4.Start();
                var thread5 = new Thread(() =>
                {
                    bitmaps.Add(createGif(5*i +i));
                });
                thread5.Start();
                var thread6 = new Thread(() =>
                {
                    bitmaps.Add(createGif(6*i +i));
                });
                thread6.Start();
                var thread7 = new Thread(() =>
                {
                    bitmaps.Add(createGif(7*i +i));
                });
                thread7.Start();
                var thread8 = new Thread(() =>
                {
                    bitmaps.Add(createGif(8*i +i));
                });
                thread8.Start();

                thread.Join();
                thread2.Join();
                thread3.Join();
                thread4.Join();
                thread5.Join();
                thread6.Join();
                thread7.Join();
                thread8.Join();

            }



            GifBitmapEncoder gEnc = new GifBitmapEncoder();
            foreach (Bitmap bmpImage in bitmaps)
            {
                var bmp = bmpImage.GetHbitmap();
                var src = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    bmp,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
                gEnc.Frames.Add(BitmapFrame.Create(src));

            }

            using (var ms = new MemoryStream())
            {
                gEnc.Save(ms);
                var fileBytes = ms.ToArray();
                // This is the NETSCAPE2.0 Application Extension.
                var applicationExtension = new byte[] { 33, 255, 11, 78, 69, 84, 83, 67, 65, 80, 69, 50, 46, 48, 3, 1, 0, 0, 0 }; //gif looping
                var newBytes = new List<byte>();
                newBytes.AddRange(fileBytes.Take(13));
                newBytes.AddRange(applicationExtension);
                newBytes.AddRange(fileBytes.Skip(13));
                File.WriteAllBytes("gif.gif", newBytes.ToArray());
            }

            //System.Diagnostics.Process.Start(@"C:\Users\Guill\Documents\Gamagora\SyntheseImage\SyntheseImage\bin\Debug\" + img3D.fileName); //HOME
            //System.Diagnostics.Process.Start(@"C:\Users\guquiniou\Documents\GitHub\SyntheseImage\bin\Release\" + img3D.fileName); // FAC


        }

        private static Bitmap createGif(int frameNumber)
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

            // string[] lines = System.IO.File.ReadAllLines(@"C:\Users\Guill\Documents\Gamagora\SyntheseImage\SyntheseImage\test.stl"); //HOME
            string[] lines = System.IO.File.ReadAllLines(@" C:\Users\guquiniou\Documents\GitHub\SyntheseImage\test.stl"); //FAC

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


            Image img3D = scene.DrawImg(5);
            img3D.WritePPM(frameNumber);
            return img3D.WriteBMP();

        }
    }
}

