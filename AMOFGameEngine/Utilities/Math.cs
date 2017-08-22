using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Utilities
{
    public class Math
    {
        public static float Clamp(float val, float minval, float maxval)
        {
            return System.Math.Max(System.Math.Min(val, maxval), minval);
        }
        public static int Clamp(int val, int minval, int maxval)
        {
            return System.Math.Max(System.Math.Min(val, maxval), minval);
        }
        public static decimal Clamp(decimal val, decimal minval, decimal maxval)
        {
            return System.Math.Max(System.Math.Min(val, maxval), minval);
        }
        public static double Clamp(double val, double minval, double maxval)
        {
            return System.Math.Max(System.Math.Min(val, maxval), minval);
        }
        public static uint Clamp(uint val, uint minval, uint maxval)
        {
            return System.Math.Max(System.Math.Min(val, maxval), minval);
        }
    }
}
