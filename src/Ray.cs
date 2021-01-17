using System;
using System.Numerics;

namespace SyntheseImage
{
    public class Ray
    {
        #region Variables
        public Vector3 origin;
        public Vector3 direction;
        #endregion

        public Ray(Vector3 _origin, Vector3 _direction)
        {
            origin = _origin;
            direction = _direction;

        }
      
        public Vector3 GetPointAt(float coeff)
        {
            return Vector3.Add(origin, Vector3.Multiply(coeff, direction));
        }
    }
}