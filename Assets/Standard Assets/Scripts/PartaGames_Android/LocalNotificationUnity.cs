using System;
using UnityEngine;

namespace PartaGames.Android
{
	public class LocalNotificationUnity : MonoBehaviour
	{
		private static int DEFAULT_NOTIFICATION_ID = 1;

		private const string ANDROID_JAVA_CLASS_NAME = "com.partagames.unity.plugins.notification.NotificationUtil";

		public static void CreateChannel(string id, string title, string description)
		{
			DoCreateChannel(id, title, description);
		}

		public static void CreateChannel(string id, string title, string description, NotificationImportance importance, bool enableLights, bool enableVibration)
		{
			DoCreateChannel(id, title, description, importance, enableLights, enableVibration);
		}

		public static void CreateChannel(string id, string title, string description, NotificationImportance importance, bool enableLights, bool enableVibration, string soundFileName, NotificationSoundContentType contentType)
		{
			DoCreateChannel(id, title, description, importance, enableLights, enableVibration, soundFileName, contentType);
		}

		public static void DeleteChannel(string id)
		{
			DoDeleteChannel(id);
		}

		public static void CancelNotification()
		{
			DoCancelNotification(DEFAULT_NOTIFICATION_ID);
		}

		public static void CancelNotification(int notificationId)
		{
			DoCancelNotification(notificationId);
		}

		public static void SendNotification(string channelId, string title, string text)
		{
			DoSendNotification(channelId, title, text, DateTime.Now, null, null, DEFAULT_NOTIFICATION_ID);
		}

		public static void SendNotification(string channelId, string title, string text, string soundFileName)
		{
			DoSendNotification(channelId, title, text, DateTime.Now, soundFileName, null, DEFAULT_NOTIFICATION_ID);
		}

		public static void SendNotification(string channelId, string title, string text, string soundFileName, string bigPictureFileName)
		{
			DoSendNotification(channelId, title, text, DateTime.Now, soundFileName, bigPictureFileName, DEFAULT_NOTIFICATION_ID);
		}

		public static void SendNotification(string channelId, string title, string text, int notificationId)
		{
			DoSendNotification(channelId, title, text, DateTime.Now, null, null, notificationId);
		}

		public static void SendNotification(string channelId, string title, string text, DateTime fireDate)
		{
			SendNotification(channelId, title, text, fireDate, DEFAULT_NOTIFICATION_ID);
		}

		public static void SendNotification(string channelId, string title, string text, DateTime fireDate, string soundFileName)
		{
			SendNotification(channelId, title, text, fireDate, soundFileName, null, DEFAULT_NOTIFICATION_ID);
		}

		public static void SendNotification(string channelId, string title, string text, DateTime fireDate, string soundFileName, string bigPictureFileName)
		{
			SendNotification(channelId, title, text, fireDate, soundFileName, bigPictureFileName, DEFAULT_NOTIFICATION_ID);
		}

		public static void SendNotification(string channelId, string title, string text, DateTime fireDate, int notificationId)
		{
			DoSendNotification(channelId, title, text, fireDate, null, null, notificationId);
		}

		public static void SendNotification(string channelId, string title, string text, DateTime fireDate, string soundFileName, string bigPictureFileName, int notificationId)
		{
			DoSendNotification(channelId, title, text, fireDate, soundFileName, bigPictureFileName, notificationId);
		}

		private static void DoCreateChannel(string id, string title, string description, NotificationImportance importance = NotificationImportance.HIGH, bool enableLights = true, bool enableVibration = true, string soundFileName = null, NotificationSoundContentType contentType = NotificationSoundContentType.UNKNOWN)
		{
			if (Application.platform != RuntimePlatform.Android)
			{
				UnityEngine.Debug.LogWarning("Local notifications plugin can only be used when running on the Android platform");
				return;
			}
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
			AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("com.partagames.unity.plugins.notification.NotificationUtil");
			androidJavaClass2.CallStatic("createChannel", @static, id, title, description, (int)importance, enableLights, enableVibration, soundFileName, (int)contentType);
		}

		private static void DoDeleteChannel(string id)
		{
			if (Application.platform != RuntimePlatform.Android)
			{
				UnityEngine.Debug.LogWarning("Local notifications plugin can only be used when running on the Android platform");
				return;
			}
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
			AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("com.partagames.unity.plugins.notification.NotificationUtil");
			androidJavaClass2.CallStatic("deleteChannel", @static, id);
		}

		private static void DoCancelNotification(int notificationId)
		{
			if (Application.platform != RuntimePlatform.Android)
			{
				UnityEngine.Debug.LogWarning("Local notifications plugin can only be used when running on the Android platform");
				return;
			}
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
			AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("com.partagames.unity.plugins.notification.NotificationUtil");
			androidJavaClass2.CallStatic("cancelNotification", @static, notificationId);
		}

		private static void DoSendNotification(string channelId, string title, string text, DateTime fireDate, string soundFileName, string bigPictureFileName, int notificationId)
		{
			if (Application.platform != RuntimePlatform.Android)
			{
				UnityEngine.Debug.LogWarning("Local notifications plugin can only be used when running on the Android platform");
				return;
			}
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
			AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("com.partagames.unity.plugins.notification.NotificationUtil");
			long num = fireDate.Ticks / 10000 - DateTime.Now.Ticks / 10000;
			androidJavaClass2.CallStatic("sendNotification", @static, title, text, num, soundFileName, bigPictureFileName, notificationId, channelId);
		}
	}
}
