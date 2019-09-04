using UnityEngine;

namespace Fabric.Internal.Runtime
{
	public static class Utils
	{
		public static void Log(string kit, string message)
		{
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("android.util.Log");
			androidJavaClass.CallStatic<int>("d", new object[2]
			{
				kit,
				message
			});
		}
	}
}
