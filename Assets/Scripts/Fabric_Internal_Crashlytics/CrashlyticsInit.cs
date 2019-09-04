using Fabric.Crashlytics;
using Fabric.Internal.Runtime;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Fabric.Internal.Crashlytics
{
	public class CrashlyticsInit : MonoBehaviour
	{
		private static readonly string kitName = "Crashlytics";

		private static CrashlyticsInit instance;

		[CompilerGenerated]
		private static UnhandledExceptionEventHandler _003C_003Ef__mg_0024cache0;

		[CompilerGenerated]
		private static Application.LogCallback _003C_003Ef__mg_0024cache1;

		private void Awake()
		{
			if (instance == null)
			{
				AwakeOnce();
				instance = this;
				UnityEngine.Object.DontDestroyOnLoad(this);
			}
			else if (instance != this)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}

		private void AwakeOnce()
		{
			RegisterExceptionHandlers();
		}

		private static void RegisterExceptionHandlers()
		{
			if (IsSDKInitialized())
			{
				Utils.Log(kitName, "Registering exception handlers");
				AppDomain.CurrentDomain.UnhandledException += HandleException;
				Application.logMessageReceived += HandleLog;
			}
			else
			{
				Utils.Log(kitName, "Did not register exception handlers: Crashlytics SDK was not initialized");
			}
		}

		private static bool IsSDKInitialized()
		{
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.crashlytics.android.Crashlytics");
			AndroidJavaObject androidJavaObject = null;
			try
			{
				androidJavaObject = androidJavaClass.CallStatic<AndroidJavaObject>("getInstance", new object[0]);
			}
			catch
			{
				androidJavaObject = null;
			}
			return androidJavaObject != null;
		}

		private static void HandleException(object sender, UnhandledExceptionEventArgs eArgs)
		{
			Exception ex = (Exception)eArgs.ExceptionObject;
			HandleLog(ex.Message.ToString(), ex.StackTrace.ToString(), LogType.Exception);
		}

		private static void HandleLog(string message, string stackTraceString, LogType type)
		{
			if (type == LogType.Exception)
			{
				Utils.Log(kitName, "Recording exception: " + message);
				Utils.Log(kitName, "Exception stack trace: " + stackTraceString);
				string[] messageParts = getMessageParts(message);
				Fabric.Crashlytics.Crashlytics.RecordCustomException(messageParts[0], messageParts[1], stackTraceString);
			}
		}

		private static string[] getMessageParts(string message)
		{
			char[] separator = new char[1]
			{
				':'
			};
			string[] array = message.Split(separator, 2, StringSplitOptions.None);
			string[] array2 = array;
			foreach (string text in array2)
			{
				text.Trim();
			}
			if (array.Length == 2)
			{
				return array;
			}
			return new string[2]
			{
				"Exception",
				message
			};
		}
	}
}
