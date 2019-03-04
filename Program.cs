using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace SyntheseImage
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Working ...");

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
            for (int i = 0; i < 20; i++)
            {
                scene.lights.Clear();
                Light light1 = new Light(new Vector3(800 + i * 5, 200 + i * 5, 800 + i * 5), new Vector3(500000, 500000, 500000));
                Light[] lights = { light1 };
                scene.lights.AddRange(lights);

                string[] lines = System.IO.File.ReadAllLines(@"C:\Users\Guill\Documents\Gamagora\SyntheseImage\SyntheseImage\test.stl"); //HOME
                                                                                                                                         //string[] lines = System.IO.File.ReadAllLines(@" C:\Users\guquiniou\Documents\GitHub\SyntheseImage\test.stl"); //FAC

                scene.shapes.Clear();

                char[] paramSplit = new char[1] { ' ' };
                Vector3 translation = new Vector3(350 - i * 2, 300 - i * 2, 400 - i * 2);
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
                bitmaps.Add(img3D.WriteBMP(i));

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

            //using (FileStream fs = new FileStream("gif.gif", FileMode.Create))
            //{
            //    gEnc.Save(fs);
            //}
            using (var ms = new MemoryStream())
            {
                gEnc.Save(ms);
                var fileBytes = ms.ToArray();
                // This is the NETSCAPE2.0 Application Extension.
                var applicationExtension = new byte[] { 33, 255, 11, 78, 69, 84, 83, 67, 65, 80, 69, 50, 46, 48, 3, 1, 0, 0, 0 };
                var newBytes = new List<byte>();                
                newBytes.AddRange(fileBytes.Take(13));
                newBytes.AddRange(applicationExtension);
                newBytes.AddRange(fileBytes.Skip(13));
                File.WriteAllBytes("gif.gif", newBytes.ToArray());
            }

            //System.Diagnostics.Process.Start(@"C:\Users\Guill\Documents\Gamagora\SyntheseImage\SyntheseImage\bin\Debug\" + img3D.fileName); //HOME
            //System.Diagnostics.Process.Start(@"C:\Users\guquiniou\Documents\GitHub\SyntheseImage\bin\Release\" + img3D.fileName); // FAC

            //Console.ReadLine();                              

        }
    }
}

