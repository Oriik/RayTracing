using System;
using System.Numerics;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace SyntheseImage
{
    public class Image
    {
        // The output file after ray tracing operate


        public int height;
        public int width;
        public int[,] r;
        public int[,] g;
        public int[,] b;
        public string fileName;


        public Image(int _width, int _height, string _fileName)
        {
            height = _height;
            width = _width;
            fileName = _fileName + ".ppm";
            r = new int[width, height];
            g = new int[width, height];
            b = new int[width, height];

            for (int j = 0; j < height; j++)
                for (int i = 0; i < width; i++)
                {
                    r[i, j] = 240;
                    g[i, j] = 240;
                    b[i, j] = 240;
                }
        }

        public void drawARayon(Rayon rayon)
        {

            //BRESENHAM
            int x0 = (int)rayon.origine.X;
            int y0 = (int)rayon.origine.Y;
            Vector3 end = Vector3.Add(rayon.origine, rayon.direction);
            int x1 = (int)end.X;
            int y1 = (int)end.Y;
            int dx = Math.Abs(x1 - x0),
                sx = x0 < x1 ? 1 : -1;
            int dy = Math.Abs(y1 - y0),
                sy = y0 < y1 ? 1 : -1;
            int err = (dx > dy ? dx : -dy) / 2
                , e2;
            for (; ; )
            {
                SetPixel(x0, y0, 255, 255, 255);
                if (x0 == x1 && y0 == y1) break;
                e2 = err;
                if (e2 > -dx) { err -= dy; x0 += sx; }
                if (e2 < dy) { err += dx; y0 += sy; }
            }

        }



        public void SetPixel(int x, int y, float r, float g, float b)
        {
            this.r[x, y] = Clamp((int)(r * 255), 0, 255);
            this.g[x, y] = Clamp((int)(g * 255), 0, 255);
            this.b[x, y] = Clamp((int)(b * 255), 0, 255);
        }

        public void WritePPM(int i)
        {
            //Use a streamwriter to write the text part of the encoding.

            var writer = new StreamWriter(i + "_" + fileName);
            writer.Write("P3" + "\n");
            writer.Write(width + " " + height + "\n");
            writer.Write("255" + "\n");

            for (int y = 0; y < height; y++)
            {
                string line = "";
                for (int x = 0; x < width; x++)
                {
                    line += r[x, y] + " " + g[x, y] + " " + b[x, y] + " ";

                }
                writer.WriteLine(line);
            }
            writer.Close();

        }

        public Bitmap WriteBMP(int i)
        {
            // Create a Bitmap object from a file.
            Bitmap myBitmap = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color col = Color.FromArgb(r[x, y], g[x, y], b[x, y]);
                    myBitmap.SetPixel(x, y, col);
                }
            }

            return myBitmap;


        }

        private int Clamp(int v, int min, int max)
        {
            if (v < min) return min;
            if (v > max) return max;
            return v;
        }


    }
}