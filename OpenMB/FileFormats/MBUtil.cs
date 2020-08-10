using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace OpenMB.FileFormats
{
	public class MBUtil
	{
		public static string GetLastPathName(string path)
		{
			int index = path.LastIndexOf("\\");
			return path.Substring(index + 1, path.Length - index - 1);
		}

		public static float LoadFloat(BinaryReader reader)
		{
			return reader.ReadSingle();
		}

		public static int LoadInt32(BinaryReader reader)
		{
			return reader.ReadInt32();
		}

		public static uint LoadUInt32(BinaryReader reader)
		{
			return reader.ReadUInt32();
		}

		public static Point3F LoadPoint3F(BinaryReader reader)
		{
			Point3F vect = new Point3F();
			vect.x = LoadFloat(reader);
			vect.y = LoadFloat(reader);
			vect.z = LoadFloat(reader);
			return vect;
		}

		public static Point4F LoadPoint4F(BinaryReader reader)
		{
			Point4F vect = new Point4F();
			vect.w = LoadFloat(reader);
			vect.x = LoadFloat(reader);
			vect.y = LoadFloat(reader);
			vect.z = LoadFloat(reader);
			return vect;
		}

		public static PointF LoadPoint2F(BinaryReader reader)
		{
			PointF vect = new PointF();
			vect.X = LoadFloat(reader);
			vect.Y = LoadFloat(reader);
			return vect;
		}

		public static string LoadString(BinaryReader reader)
		{
			byte b = reader.ReadByte();
			reader.ReadBytes(3);
			byte[] bytes = reader.ReadBytes(b);
			string str = Encoding.UTF8.GetString(bytes);
			return str;
		}

		public static string LoadStringMaybe(BinaryReader reader, string ifnot)
		{
			byte b = reader.ReadByte();
			reader.ReadBytes(3);

			if (b < 99 && b > 0)
			{
				byte[] bytes = reader.ReadBytes(b);
				string str = Encoding.UTF8.GetString(bytes);
				return str;
			}
			else
			{
				reader.BaseStream.Seek(-4, SeekOrigin.Current);
				return ifnot;
			}

		}

		public static byte LoadByte(BinaryReader reader)
		{
			return reader.ReadByte();
		}

		public static bool LoadVector(BinaryReader reader, ref List<int> v)
		{
			uint k;
			k = LoadUInt32(reader);
			for (uint i = 0; i < k; i++)
				v[(int)i] = LoadInt32(reader);
			return true;
		}

		public static bool LoadVector(BinaryReader reader, ref List<Point3F> v)
		{
			uint k;
			k = LoadUInt32(reader);
			v = new List<Point3F>((int)k);
			for (uint i = 0; i < k; i++)
				v.Add(LoadPoint3F(reader));
			return true;
		}

		public static bool LoadVector<T>(BinaryReader reader, ref List<T> v)
		{
			uint k;
			k = LoadUInt32(reader);
			v = new List<T>((int)k);
			for (uint i = 0; i < k; i++)
			{
				//v.Add(0);
			}
			return true;
		}

		public static int TmpRigging2Rigging(ref List<TmpSkinning> v1, ref List<MBBrfSkinning> v2)
		{
			int max = 0;

			for (int i = 0; i < v1.Count; i++)
			{
				for (int j = 0; j < v1[i].pairs.Count; j++)
				{
					v2[v1[i].pairs[j].vindex].Add(v1[i].bindex, v1[i].pairs[j].vindex);
				}
			}

			return max;
		}

		public static void TmpBone2BrfFrame(List<TmpBone4> tmpBone4v, List<TmpCas3F> tmpCas3f, out List<MBBrfAnimationFrame> frames)
		{
			frames = new List<MBBrfAnimationFrame>();

			int MAXFRAMES = 0;

			for (int i = 0; i < tmpBone4v.Count; i++)
			{
				for (int j = 0; j < tmpBone4v[i].casList.Count; j++)
				{
					int fi = tmpBone4v[i].casList[j].findex + 1;
					if (fi > MAXFRAMES)
					{
						MAXFRAMES = fi;
					}
				}
			}
			for (int i = 0; i < tmpCas3f.Count; i++)
			{
				int fi = tmpCas3f[i].findex + 1;
				if (fi > MAXFRAMES)
				{
					MAXFRAMES = fi;
				}
			}

			List<int> present = new List<int>();
			for (int i = 0; i < MAXFRAMES; i++)
			{
				present[i] = 0;
			}

			int nf = 0;
			for (int i = 0; i < tmpBone4v.Count; i++)
			{
				int ndup = 1;

				for (int j = 0; j < tmpBone4v[i].casList.Count; j++)
				{
					int fi = tmpBone4v[i].casList[j].findex;
					if (j > 0)
					{
						if (fi == tmpBone4v[i].casList[j - 1].findex)
						{
							ndup++;
						}
						else
						{
							ndup = 1;
						}
					}
					if (present[fi] < ndup)
					{
						nf++;
						present[fi] = ndup;
					}
				}
			}
			{
				int ndup = 1;
				for (int j = 0; j < tmpCas3f.Count; j++)
				{
					int fi = tmpCas3f[j].findex;
					if (j > 0)
					{
						if (fi == tmpCas3f[j - 1].findex) ndup++; else ndup = 1;
					}
					if (present[fi] < ndup)
					{
						nf++;
						present[fi] = ndup;
					}
				}
			}
			for (int i = 0; i < nf; i++)
			{
				MBBrfAnimationFrame frame = new MBBrfAnimationFrame();
				frame.rot = new List<Point4F>();
				for (int j = 0; j < tmpBone4v.Count; j++)
				{
					frame.rot.Add(new Point4F());
				}
				frame.wasImplicit = new List<bool>();
				for (int j = 0; j < tmpBone4v.Count + 1; j++)
				{
					frame.wasImplicit.Add(false);
				}
				frames.Add(frame);
			}

			for (int bi = 0; bi < tmpBone4v.Count; bi++)
			{
				int j = 0, // pos in current vb
					m = 0; // pos in vf
				for (int fi = 0; fi < MAXFRAMES; fi++)
				{
					for (int dupl = 0; dupl < present[fi]; dupl++) // often zero times
					{
						int fi2;

						while ((fi2 = tmpBone4v[bi].casList[j].findex) < fi) j++;
						if (dupl > 0) { if (fi2 == fi) { j++; fi2 = tmpBone4v[bi].casList[j].findex; } }

						frames[m].index = fi;
						if (fi2 == fi)
						{
							frames[m].rot[bi] = tmpBone4v[bi].casList[j].rot;
							frames[m].wasImplicit[bi] = false;
						}
						else
						{
							// copy from prev
							frames[m].rot[bi] = frames[m - 1].rot[bi];
							frames[m].wasImplicit[bi] = true;
						}
						m++;
					}
				}
			}

			{
				// last round for translation
				int j = 0, // pos in current vb
					m = 0; // pos in vf
				for (int fi = 0; fi < MAXFRAMES; fi++)
				{
					for (int dupl = 0; dupl < present[fi]; dupl++) // often zero times
					{
						int fi2;
						while ((fi2 = tmpCas3f[j].findex) < fi) j++;
						if (dupl > 0) { if (fi2 == fi) { j++; fi2 = tmpCas3f[j].findex; } }

						frames[m].index = fi;
						if (fi2 == fi)
						{
							frames[m].tra = tmpCas3f[j].rot;
							frames[m].wasImplicit[tmpBone4v.Count] = false;
						}
						else
						{
							// copy from prev
							frames[m].tra = frames[m - 1].tra;
							frames[m].wasImplicit[tmpBone4v.Count] = true;
						}
						m++;
					}
				}
			}

		}
	}
}
