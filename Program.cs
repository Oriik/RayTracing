using System;
using System.Numerics;

namespace SyntheseImage
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Working ...");

            Camera camera = new Camera(new Vector3(0, 0, 0), 1280, 720, new Vector3(0, 0, 1), 1000);
            Light light = new Light(new Vector3(400, 200, 200), new Vector3(1000000, 1000000, 1000000));
            Scene scene = new Scene(camera, light);

            Sphere s1 = new Sphere(new Vector3(1050, 500, 800), 100,
                 new Material(Materials.Mirror, Material.White));

            //FACE
            Triangle t1 = new Triangle(new Vector3(300,500,500), new Vector3(400,500,500), new Vector3(300,600,500),
                  new Material(Materials.Difuse, Material.Red));
            Triangle t2 = new Triangle(new Vector3(300, 600, 500), new Vector3(400, 500, 500), new Vector3(400, 600, 500),
                  new Material(Materials.Difuse, Material.Red));
            //HAUT
            Triangle t3 = new Triangle(new Vector3(300, 500, 500), new Vector3(320, 500, 600), new Vector3(420, 500, 600),
                new Material(Materials.Difuse, Material.Blue));
            Triangle t4 = new Triangle(new Vector3(300, 500, 500), new Vector3(420, 500, 600), new Vector3(400, 500, 500),
                 new Material(Materials.Difuse, Material.Blue));
            //DROITE
            Triangle t5 = new Triangle(new Vector3(400, 500, 500), new Vector3(420, 500, 600), new Vector3(400, 600, 500),
                 new Material(Materials.Mirror, Material.White));
            Triangle t6 = new Triangle(new Vector3(400, 600, 500), new Vector3(420, 500, 600), new Vector3(420, 600, 600),
                new Material(Materials.Mirror, Material.White));
            //GAUCHE
            Triangle t7 = new Triangle(new Vector3(320, 600, 600), new Vector3(320, 500, 600), new Vector3(300, 600, 500),
                 new Material(Materials.Difuse, Material.Yellow));
            Triangle t8 = new Triangle(new Vector3(300, 600, 500), new Vector3(320, 500, 600), new Vector3(300, 500, 500),
                  new Material(Materials.Difuse, Material.Yellow));
            //BAS
            Triangle t9 = new Triangle(new Vector3(300, 600, 500), new Vector3(400, 600, 500), new Vector3(320, 600, 600),
                  new Material(Materials.Difuse, Material.Blue));
            Triangle t10 = new Triangle(new Vector3(320, 600, 600), new Vector3(400, 600, 500), new Vector3(420, 600, 600),
                 new Material(Materials.Difuse, Material.Blue));
            //FOND
            Triangle t11 = new Triangle(new Vector3(320, 500, 600), new Vector3(420, 500, 600), new Vector3(320, 600, 600),
                 new Material(Materials.Difuse, Material.Red));
            Triangle t12 = new Triangle(new Vector3(320, 600, 600), new Vector3(420, 500, 600), new Vector3(420, 600, 600),
                  new Material(Materials.Difuse, Material.Red));

            //FACE
            Triangle tt1 = new Triangle(new Vector3(800, 300, 300), new Vector3(900, 300, 300), new Vector3(800, 400, 300),
                  new Material(Materials.Difuse, Material.Red));
            Triangle tt2 = new Triangle(new Vector3(800, 400, 300), new Vector3(900, 300, 300), new Vector3(900, 400, 300),
                  new Material(Materials.Difuse, Material.Red));
            //HAUT
            Triangle tt3 = new Triangle(new Vector3(800, 300, 300), new Vector3(780, 300, 400), new Vector3(880, 300, 400),
                new Material(Materials.Difuse, Material.Blue));
            Triangle tt4 = new Triangle(new Vector3(800, 300, 300), new Vector3(880, 300, 400), new Vector3(900, 300, 300),
                 new Material(Materials.Difuse, Material.Blue));
            //DROITE
            Triangle tt5 = new Triangle(new Vector3(900, 300, 300), new Vector3(880, 300, 400), new Vector3(900, 400, 300),
                 new Material(Materials.Difuse, Material.Yellow));
            Triangle tt6 = new Triangle(new Vector3(900, 400, 300), new Vector3(880, 300, 400), new Vector3(880, 400, 400),
                new Material(Materials.Difuse, Material.Yellow));
            //GAUCHE
            Triangle tt7 = new Triangle(new Vector3(780, 400, 400), new Vector3(780, 300, 400), new Vector3(800, 400, 300),
                 new Material(Materials.Difuse, Material.Yellow));
            Triangle tt8 = new Triangle(new Vector3(800, 400, 300), new Vector3(780, 300, 400), new Vector3(800, 300, 300),
                  new Material(Materials.Difuse, Material.Yellow));
            //BAS
            Triangle tt9 = new Triangle(new Vector3(800, 400, 300), new Vector3(900, 400, 300), new Vector3(780, 400, 400),
                  new Material(Materials.Difuse, Material.Blue));
            Triangle tt10 = new Triangle(new Vector3(780, 400, 400), new Vector3(900, 400, 300), new Vector3(880, 400, 400),
                 new Material(Materials.Difuse, Material.Blue));
            //FOND
            Triangle tt11 = new Triangle(new Vector3(780, 300, 400), new Vector3(880, 300, 400), new Vector3(780, 400, 400),
                 new Material(Materials.Difuse, Material.Red));
            Triangle tt12 = new Triangle(new Vector3(780, 400, 400), new Vector3(880, 300, 400), new Vector3(880, 400, 400),
                  new Material(Materials.Difuse, Material.Red));


            Sphere leftWall = new Sphere(new Vector3((float)-1e5-100, 360, 500), (float)1e5,
                new Material(Materials.Mirror,Material.White)); 
            Sphere rightWall = new Sphere(new Vector3((float)1e5 + 1380, 360, 500), (float)1e5,
                new Material(Materials.Difuse, Material.Blue)); 
            Sphere topWall = new Sphere(new Vector3(640, (float)-1e5-100, 500), (float)1e5, 
                new Material(Materials.Difuse, Material.Pink)); 
            Sphere bottomWall = new Sphere(new Vector3(640, (float)1e5+820 , 500), (float)1e5, 
                new Material(Materials.Difuse, Material.Red)); 
            Sphere backWall = new Sphere(new Vector3(640, 360, (float)1e5  +1100), (float)1e5,
                new Material(Materials.Difuse,Material.Green)); 
            Sphere frontWall = new Sphere(new Vector3(640, 360, (float)-1e5 -100), (float)1e5, 
                new Material(Materials.Difuse, Material.Green)); 

            Shape[] shapes = { frontWall, backWall, rightWall, leftWall, bottomWall, topWall,
                t1, t2,t3,t4,t5,t6,t7,t8,t9,t10,t11,t12,
            tt1,tt2,tt3,tt4,tt5,tt6,tt7,tt8,tt9,tt10,tt11,tt12};
            scene.shapes.AddRange(shapes);
                        
            Image img3D = scene.DrawImg(50);
            img3D.WritePPM();

             System.Diagnostics.Process.Start(@"C:\Users\Guill\Documents\Gamagora\SyntheseImage\SyntheseImage\bin\Debug\" + img3D.fileName); //HOME
            // System.Diagnostics.Process.Start(@"C:\Users\guquiniou\Documents\GitHub\SyntheseImage\bin\Release\" + img3D.fileName); // FAC
             //Console.ReadLine();



        }
    }
}
