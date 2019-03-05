using System;
using System.Numerics;

namespace SyntheseImage
{
    public class Rayon
    {
        #region Variables
        public Vector3 origine;
        public Vector3 direction;
        #endregion

        public Rayon(Vector3 _origine, Vector3 _direction)
        {
            origine = _origine;
            direction = _direction;

        }
      
        public Vector3 GetPointAt(float coeff)
        {
            return Vector3.Add(origine, Vector3.Multiply(coeff, direction));
        }
    }
}