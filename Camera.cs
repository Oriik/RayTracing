using System.Numerics;

namespace SyntheseImage
{
    public class Camera
    {
        public Vector3 o;
        public int height, width;
        public Vector3 d;
        public Vector3 focus;
        private float distanceFocus;

        public Camera(Vector3 _origine, int _width, int _height, Vector3 _direction, float _distanceFocus)
        {
            o = _origine;
            height = _height;
            width = _width;
            d = Vector3.Normalize(_direction);
            distanceFocus = _distanceFocus;
            focus = new Vector3(o.X + (width / 2), o.Y + (height / 2), o.Z);
            focus = Vector3.Add(focus, Vector3.Multiply(Vector3.Negate(d), distanceFocus));
        }

        public Vector3 GetFocusAngle(float x, float y)
        {
            Vector3 res = Vector3.Subtract(new Vector3(x, y, o.Z), focus);
            return Vector3.Normalize(res);
        }
    }
}