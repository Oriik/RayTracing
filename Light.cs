using System.Numerics;

namespace SyntheseImage
{
    public class Light
    {
        public Vector3 origine;
        public Vector3 power;


        public Light(Vector3 _o, Vector3 _power)
        {
            origine = _o;
            power = _power;
        }
    }
}