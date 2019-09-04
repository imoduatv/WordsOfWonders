using Fabric.Crashlytics.Internal;
using System.Diagnostics;

namespace Fabric.Crashlytics
{
	public class Crashlytics
	{
		private static readonly Impl impl;

		static Crashlytics()
		{
			impl = Impl.Make();
		}

		public static void SetDebugMode(bool debugMode)
		{
			impl.SetDebugMode(debugMode);
		}

		public static void Crash()
		{
			impl.Crash();
		}

		public static void ThrowNonFatal()
		{
			impl.ThrowNonFatal();
		}

		public static void Log(string message)
		{
			impl.Log(message);
		}

		public static void SetKeyValue(string key, string value)
		{
			impl.SetKeyValue(key, value);
		}

		public static void SetUserIdentifier(string identifier)
		{
			impl.SetUserIdentifier(identifier);
		}

		public static void SetUserEmail(string email)
		{
			impl.SetUserEmail(email);
		}

		public static void SetUserName(string name)
		{
			impl.SetUserName(name);
		}

		public static void RecordCustomException(string name, string reason, StackTrace stackTrace)
		{
			impl.RecordCustomException(name, reason, stackTrace);
		}

		public static void RecordCustomException(string name, string reason, string stackTraceString)
		{
			impl.RecordCustomException(name, reason, stackTraceString);
		}
	}
}
