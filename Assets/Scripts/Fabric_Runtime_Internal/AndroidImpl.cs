using UnityEngine;

namespace Fabric.Runtime.Internal
{
	internal class AndroidImpl : Impl
	{
		private static readonly AndroidJavaClass FabricInitializer = new AndroidJavaClass("io.fabric.unity.android.FabricInitializer");

		public override string Initialize()
		{
			return FabricInitializer.CallStatic<string>("JNI_InitializeFabric", new object[0]);
		}
	}
}
