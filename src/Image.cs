using System.Drawing;
using System.Drawing.Imaging;

namespace SyntheseImage
{
    public class Image
    {
        // The output file after ray tracing operate

        #region Variables
        private int m_height;
        private int m_width;
        private int[,] m_red;
        private int[,] m_green;
        private int[,] m_blue;
        #endregion

        public Image(int _width, int _height)
        {
            m_height = _height;
            m_width = _width;          
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

        public void SetPixel(int x, int y, float r, float g, float b)
        {
            this.m_red[x, y] = Clamp((int)(r * 255), 0, 255);
            this.m_green[x, y] = Clamp((int)(g * 255), 0, 255);
            this.m_blue[x, y] = Clamp((int)(b * 255), 0, 255);
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