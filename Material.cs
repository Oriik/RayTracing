using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SyntheseImage
{
    public enum Materials
    {
        Difuse, Mirror, Glass
    }


    public class Material
    {
        public static Vector3 Red = new Vector3(0.8f, 0.2f, 0.2f);
        public static Vector3 Yellow = new Vector3(0.8f, 0.8f, 0.2f);
        public static Vector3 Blue = new Vector3(0.2f, 0.8f, 0.8f);
        public static Vector3 Pink = new Vector3(0.8f, 0.2f, 0.8f);
        public static Vector3 Green = new Vector3(0.2f, 0.8f, 0.2f);
        public static Vector3 White = new Vector3(0.9f, 0.9f, 0.9f);

        public Materials mat;
        public Vector3 albedo;

        public Material(Materials _mat, Vector3 _albedo)
        {
            mat = _mat;
            albedo = _albedo;
        }
    }
}
