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
        Difuse, Mirror, Light
    }

   
    public class Material
    {
        public Materials mat;
        public Vector3 albedo;

        public Material(Materials _mat, Vector3 _albedo)
        {
            mat = _mat;
            albedo = _albedo;
        }
    }
}
