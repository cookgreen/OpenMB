using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Utilities
{
    public class Helper
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
        public static T Clamp<T>(T val, T minval, T maxval) where T : IComparable
        {
            T temp;
            if (val.CompareTo(maxval) < 0)
            {
                temp = val;
            }
            else
            {
                temp = maxval;
            }
            if (temp.CompareTo(minval) > 0)
            {
                return temp;
            }
            else
            {
                return minval;
            }
        }

        public static uint GetStringHash(string str)
        {
            uint seed = 131;
            uint hash = 0;
            uint i = 0;

            for (i = 0; i < str.Length; i++)
            {
                hash = (hash * seed) + ((byte)str[(int)i]);
            }

            return hash;
        }
    }
}
