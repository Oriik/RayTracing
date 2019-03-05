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

        #region Variables
        private string m_fileName;
        private int m_height;
        private int m_width;
        private int[,] m_red;
        private int[,] m_green;
        private int[,] m_blue;
        #endregion

        public Image(int _width, int _height, string _fileName)
        {
            m_height = _height;
            m_width = _width;
            m_fileName = _fileName + ".ppm";
            m_red = new int[m_width, m_height];
            m_green = new int[m_width, m_height];
            m_blue = new int[m_width, m_height];

            for (int j = 0; j < m_height; j++)
                for (int i = 0; i < m_width; i++)
                {
                    m_red[i, j] = 240;
                    m_green[i, j] = 240;
                    m_blue[i, j] = 240;
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
            this.m_red[x, y] = Clamp((int)(r * 255), 0, 255);
            this.m_green[x, y] = Clamp((int)(g * 255), 0, 255);
            this.m_blue[x, y] = Clamp((int)(b * 255), 0, 255);
        }

        public void WritePPM()
        {
            //Use a streamwriter to write the text part of the encoding.

            var writer = new StreamWriter(m_fileName);
            writer.Write("P3" + "\n");
            writer.Write(m_width + " " + m_height + "\n");
            writer.Write("255" + "\n");

            for (int y = 0; y < m_height; y++)
            {
                string line = "";
                for (int x = 0; x < m_width; x++)
                {
                    line += m_red[x, y] + " " + m_green[x, y] + " " + m_blue[x, y] + " ";

                }
                writer.WriteLine(line);
            }
            writer.Close();

        }

        public Bitmap WriteBMP()
        {
            // Create a Bitmap object
            Bitmap myBitmap = new Bitmap(m_width, m_height, PixelFormat.Format24bppRgb);
            for (int y = 0; y < m_height; y++)
            {
                for (int x = 0; x < m_width; x++)
                {
                    Color col = Color.FromArgb(m_red[x, y], m_green[x, y], m_blue[x, y]);
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