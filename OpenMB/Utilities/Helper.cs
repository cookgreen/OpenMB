using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Mogre;
using System.Runtime.InteropServices;

namespace OpenMB.Utilities
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

        public static MemoryStream DataPtrToStream(DataStreamPtr ptr)
        {
            if (ptr.Size() != 0)
            {
                byte[] buffer = new byte[ptr.Size()];
                unsafe
                {
                    fixed (byte* bufferPtr = &buffer[0])
                    {
                        ptr.Read(bufferPtr, (uint)buffer.Length);
                    }
                }
                MemoryStream memoryStream = new MemoryStream(buffer);
                return memoryStream;
            }
            return null;
        }

        public static DataStreamPtr StreamToDataPtr(MemoryStream stream)
        {
            if (stream.Length != 0)
            {
                BinaryReader br = new BinaryReader(stream);
                byte[] pBuffer = br.ReadBytes((int)br.BaseStream.Length);
                stream.Close();
                unsafe
                {
                    GCHandle handle = GCHandle.Alloc(pBuffer, GCHandleType.Pinned);
                    void* pUnsafeBuffer = (void*)handle.AddrOfPinnedObject();
                    MemoryDataStream memorydataStream = new MemoryDataStream(pUnsafeBuffer, (uint)pBuffer.Length);
                    DataStreamPtr ptr = new DataStreamPtr(memorydataStream);
                    handle.Free();
                    return ptr;
                }
            }
            return null;
        }

        public static ColourValue HexToRgb(string hexstr)
        {
            ColourValue cv = new ColourValue();
            if (hexstr.StartsWith("0x") && hexstr.Length == 8)
            {
                cv = new ColourValue(
                 (Convert.ToInt32(hexstr[2].ToString(), 16) * 16 + Convert.ToInt32(hexstr[3].ToString(), 16)) / 255,
                 (Convert.ToInt32(hexstr[4].ToString(), 16) * 16 + Convert.ToInt32(hexstr[5].ToString(), 16)) / 255,
                 (Convert.ToInt32(hexstr[6].ToString(), 16) * 16 + Convert.ToInt32(hexstr[7].ToString(), 16)) / 255
                 );
                return cv;
            }
            return cv;
        }

        public static string ConvertUintToString(uint text)
        {
            char[] chars = System.Text.Encoding.Default.GetChars(BitConverter.GetBytes(text));
            StringBuilder sb = new StringBuilder();
            foreach (char c in chars)
            {
                if (c != '\0')
                    sb.Append(c);
            }
            string str = sb.ToString();
            return str;
        }
    }
}
