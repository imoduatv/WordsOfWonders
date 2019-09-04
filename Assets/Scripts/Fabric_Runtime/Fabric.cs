using Fabric.Runtime.Internal;
using System;
using System.Reflection;
using UnityEngine;

namespace Fabric.Runtime
{
	public class Fabric
	{
		private static readonly Impl impl;

		static Fabric()
		{
			impl = Impl.Make();
		}

		public static void Initialize()
		{
			string text = impl.Initialize();
			if (!string.IsNullOrEmpty(text))
			{
				string[] array = text.Split(',');
				string[] array2 = array;
				foreach (string kitMethod in array2)
				{
					Initialize(kitMethod);
				}
			}
		}

		internal static void Initialize(string kitMethod)
		{
			int num = kitMethod.LastIndexOf('.');
			string typeName = kitMethod.Substring(0, num);
			string name = kitMethod.Substring(num + 1);
			Type type = Type.GetType(typeName);
			if (type == null)
			{
				return;
			}
			MethodInfo method = type.GetMethod(name, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			if (method != null)
			{
				object obj = (!typeof(ScriptableObject).IsAssignableFrom(type)) ? Activator.CreateInstance(type) : ScriptableObject.CreateInstance(type);
				if (obj != null)
				{
					method.Invoke(obj, new object[0]);
				}
			}
		}
	}
}
