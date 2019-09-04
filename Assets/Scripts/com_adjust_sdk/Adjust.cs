using System;
using System.Collections.Generic;
using UnityEngine;

namespace com.adjust.sdk
{
	public class Adjust : MonoBehaviour
	{
		private const string errorMsgEditor = "Adjust: SDK can not be used in Editor.";

		private const string errorMsgStart = "Adjust: SDK not started. Start it manually using the 'start' method.";

		private const string errorMsgPlatform = "Adjust: SDK can only be used in Android, iOS, Windows Phone 8.1, Windows Store or Universal Windows apps.";

		public bool startManually = true;

		public bool eventBuffering;

		public bool sendInBackground;

		public bool launchDeferredDeeplink = true;

		public string appToken = "{Your App Token}";

		public AdjustLogLevel logLevel = AdjustLogLevel.Info;

		public AdjustEnvironment environment;

		private void Awake()
		{
			if (!IsEditor())
			{
				UnityEngine.Object.DontDestroyOnLoad(base.transform.gameObject);
				if (!startManually)
				{
					AdjustConfig adjustConfig = new AdjustConfig(appToken, environment, logLevel == AdjustLogLevel.Suppress);
					adjustConfig.setLogLevel(logLevel);
					adjustConfig.setSendInBackground(sendInBackground);
					adjustConfig.setEventBufferingEnabled(eventBuffering);
					adjustConfig.setLaunchDeferredDeeplink(launchDeferredDeeplink);
					start(adjustConfig);
				}
			}
		}

		private void OnApplicationPause(bool pauseStatus)
		{
			if (!IsEditor())
			{
				if (pauseStatus)
				{
					AdjustAndroid.OnPause();
				}
				else
				{
					AdjustAndroid.OnResume();
				}
			}
		}

		public static void start(AdjustConfig adjustConfig)
		{
			if (!IsEditor())
			{
				if (adjustConfig == null)
				{
					UnityEngine.Debug.Log("Adjust: Missing config to start.");
				}
				else
				{
					AdjustAndroid.Start(adjustConfig);
				}
			}
		}

		public static void trackEvent(AdjustEvent adjustEvent)
		{
			if (!IsEditor())
			{
				if (adjustEvent == null)
				{
					UnityEngine.Debug.Log("Adjust: Missing event to track.");
				}
				else
				{
					AdjustAndroid.TrackEvent(adjustEvent);
				}
			}
		}

		public static void setEnabled(bool enabled)
		{
			if (!IsEditor())
			{
				AdjustAndroid.SetEnabled(enabled);
			}
		}

		public static bool isEnabled()
		{
			if (IsEditor())
			{
				return false;
			}
			return AdjustAndroid.IsEnabled();
		}

		public static void setOfflineMode(bool enabled)
		{
			if (!IsEditor())
			{
				AdjustAndroid.SetOfflineMode(enabled);
			}
		}

		public static void setDeviceToken(string deviceToken)
		{
			if (!IsEditor())
			{
				AdjustAndroid.SetDeviceToken(deviceToken);
			}
		}

		public static void gdprForgetMe()
		{
			AdjustAndroid.GdprForgetMe();
		}

		public static void appWillOpenUrl(string url)
		{
			if (!IsEditor())
			{
				AdjustAndroid.AppWillOpenUrl(url);
			}
		}

		public static void sendFirstPackages()
		{
			if (!IsEditor())
			{
				AdjustAndroid.SendFirstPackages();
			}
		}

		public static void addSessionPartnerParameter(string key, string value)
		{
			if (!IsEditor())
			{
				AdjustAndroid.AddSessionPartnerParameter(key, value);
			}
		}

		public static void addSessionCallbackParameter(string key, string value)
		{
			if (!IsEditor())
			{
				AdjustAndroid.AddSessionCallbackParameter(key, value);
			}
		}

		public static void removeSessionPartnerParameter(string key)
		{
			if (!IsEditor())
			{
				AdjustAndroid.RemoveSessionPartnerParameter(key);
			}
		}

		public static void removeSessionCallbackParameter(string key)
		{
			if (!IsEditor())
			{
				AdjustAndroid.RemoveSessionCallbackParameter(key);
			}
		}

		public static void resetSessionPartnerParameters()
		{
			if (!IsEditor())
			{
				AdjustAndroid.ResetSessionPartnerParameters();
			}
		}

		public static void resetSessionCallbackParameters()
		{
			if (!IsEditor())
			{
				AdjustAndroid.ResetSessionCallbackParameters();
			}
		}

		public static string getAdid()
		{
			if (IsEditor())
			{
				return string.Empty;
			}
			return AdjustAndroid.GetAdid();
		}

		public static AdjustAttribution getAttribution()
		{
			if (IsEditor())
			{
				return null;
			}
			return AdjustAndroid.GetAttribution();
		}

		public static string getWinAdid()
		{
			if (IsEditor())
			{
				return string.Empty;
			}
			UnityEngine.Debug.Log("Adjust: Error! Windows Advertising ID is not available on Android platform.");
			return string.Empty;
		}

		public static string getIdfa()
		{
			if (IsEditor())
			{
				return string.Empty;
			}
			UnityEngine.Debug.Log("Adjust: Error! IDFA is not available on Android platform.");
			return string.Empty;
		}

		public static string getSdkVersion()
		{
			if (IsEditor())
			{
				return string.Empty;
			}
			return AdjustAndroid.GetSdkVersion();
		}

		[Obsolete("This method is intended for testing purposes only. Do not use it.")]
		public static void setReferrer(string referrer)
		{
			if (!IsEditor())
			{
				AdjustAndroid.SetReferrer(referrer);
			}
		}

		public static void getGoogleAdId(Action<string> onDeviceIdsRead)
		{
			if (!IsEditor())
			{
				AdjustAndroid.GetGoogleAdId(onDeviceIdsRead);
			}
		}

		public static string getAmazonAdId()
		{
			if (IsEditor())
			{
				return string.Empty;
			}
			return AdjustAndroid.GetAmazonAdId();
		}

		private static bool IsEditor()
		{
			return false;
		}

		public static void SetTestOptions(Dictionary<string, string> testOptions)
		{
			if (!IsEditor())
			{
				AdjustAndroid.SetTestOptions(testOptions);
			}
		}
	}
}
