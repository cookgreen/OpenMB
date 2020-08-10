using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMB.FileFormats;
using Mogre;

namespace OpenMB.Connector
{
	public static class MBOgreExpandsion
	{
		public static Vector3 ToVector3(this Point3F point3f)
		{
			return new Vector3(point3f.x, point3f.y, point3f.z);
		}

		public static Quaternion ToQuaternion(this Point4F point4f)
		{
			return new Quaternion(point4f.w, point4f.x, point4f.y, point4f.z);
		}
	}
}
