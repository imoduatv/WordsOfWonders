using System;
using System.Collections.Generic;
using UnityEngine;

namespace com.adjust.sdk
{
	public class AdjustAndroid
	{
		private class AttributionChangeListener : AndroidJavaProxy
		{
			private Action<AdjustAttribution> callback;

			public AttributionChangeListener(Action<AdjustAttribution> pCallback)
				: base("com.adjust.sdk.OnAttributionChangedListener")
			{
				callback = pCallback;
			}

			public void onAttributionChanged(AndroidJavaObject attribution)
			{
				if (callback != null)
				{
					AdjustAttribution adjustAttribution = new AdjustAttribution();
					adjustAttribution.trackerName = ((!(attribution.Get<string>(AdjustUtils.KeyTrackerName) == string.Empty)) ? attribution.Get<string>(AdjustUtils.KeyTrackerName) : null);
					adjustAttribution.trackerToken = ((!(attribution.Get<string>(AdjustUtils.KeyTrackerToken) == string.Empty)) ? attribution.Get<string>(AdjustUtils.KeyTrackerToken) : null);
					adjustAttribution.network = ((!(attribution.Get<string>(AdjustUtils.KeyNetwork) == string.Empty)) ? attribution.Get<string>(AdjustUtils.KeyNetwork) : null);
					adjustAttribution.campaign = ((!(attribution.Get<string>(AdjustUtils.KeyCampaign) == string.Empty)) ? attribution.Get<string>(AdjustUtils.KeyCampaign) : null);
					adjustAttribution.adgroup = ((!(attribution.Get<string>(AdjustUtils.KeyAdgroup) == string.Empty)) ? attribution.Get<string>(AdjustUtils.KeyAdgroup) : null);
					adjustAttribution.creative = ((!(attribution.Get<string>(AdjustUtils.KeyCreative) == string.Empty)) ? attribution.Get<string>(AdjustUtils.KeyCreative) : null);
					adjustAttribution.clickLabel = ((!(attribution.Get<string>(AdjustUtils.KeyClickLabel) == string.Empty)) ? attribution.Get<string>(AdjustUtils.KeyClickLabel) : null);
					adjustAttribution.adid = ((!(attribution.Get<string>(AdjustUtils.KeyAdid) == string.Empty)) ? attribution.Get<string>(AdjustUtils.KeyAdid) : null);
					callback(adjustAttribution);
				}
			}
		}

		private class DeferredDeeplinkListener : AndroidJavaProxy
		{
			private Action<string> callback;

			public DeferredDeeplinkListener(Action<string> pCallback)
				: base("com.adjust.sdk.OnDeeplinkResponseListener")
			{
				callback = pCallback;
			}

			public bool launchReceivedDeeplink(AndroidJavaObject deeplink)
			{
				if (callback == null)
				{
					return launchDeferredDeeplink;
				}
				string obj = deeplink.Call<string>("toString", new object[0]);
				callback(obj);
				return launchDeferredDeeplink;
			}
		}

		private class EventTrackingSucceededListener : AndroidJavaProxy
		{
			private Action<AdjustEventSuccess> callback;

			public EventTrackingSucceededListener(Action<AdjustEventSuccess> pCallback)
				: base("com.adjust.sdk.OnEventTrackingSucceededListener")
			{
				callback = pCallback;
			}

			public void onFinishedEventTrackingSucceeded(AndroidJavaObject eventSuccessData)
			{
				if (callback != null && eventSuccessData != null)
				{
					AdjustEventSuccess adjustEventSuccess = new AdjustEventSuccess();
					adjustEventSuccess.Adid = ((!(eventSuccessData.Get<string>(AdjustUtils.KeyAdid) == string.Empty)) ? eventSuccessData.Get<string>(AdjustUtils.KeyAdid) : null);
					adjustEventSuccess.Message = ((!(eventSuccessData.Get<string>(AdjustUtils.KeyMessage) == string.Empty)) ? eventSuccessData.Get<string>(AdjustUtils.KeyMessage) : null);
					adjustEventSuccess.Timestamp = ((!(eventSuccessData.Get<string>(AdjustUtils.KeyTimestamp) == string.Empty)) ? eventSuccessData.Get<string>(AdjustUtils.KeyTimestamp) : null);
					adjustEventSuccess.EventToken = ((!(eventSuccessData.Get<string>(AdjustUtils.KeyEventToken) == string.Empty)) ? eventSuccessData.Get<string>(AdjustUtils.KeyEventToken) : null);
					adjustEventSuccess.CallbackId = ((!(eventSuccessData.Get<string>(AdjustUtils.KeyCallbackId) == string.Empty)) ? eventSuccessData.Get<string>(AdjustUtils.KeyCallbackId) : null);
					try
					{
						AndroidJavaObject androidJavaObject = eventSuccessData.Get<AndroidJavaObject>(AdjustUtils.KeyJsonResponse);
						string jsonResponseString = androidJavaObject.Call<string>("toString", new object[0]);
						adjustEventSuccess.BuildJsonResponseFromString(jsonResponseString);
					}
					catch (Exception)
					{
					}
					callback(adjustEventSuccess);
				}
			}
		}

		private class EventTrackingFailedListener : AndroidJavaProxy
		{
			private Action<AdjustEventFailure> callback;

			public EventTrackingFailedListener(Action<AdjustEventFailure> pCallback)
				: base("com.adjust.sdk.OnEventTrackingFailedListener")
			{
				callback = pCallback;
			}

			public void onFinishedEventTrackingFailed(AndroidJavaObject eventFailureData)
			{
				if (callback != null && eventFailureData != null)
				{
					AdjustEventFailure adjustEventFailure = new AdjustEventFailure();
					adjustEventFailure.Adid = ((!(eventFailureData.Get<string>(AdjustUtils.KeyAdid) == string.Empty)) ? eventFailureData.Get<string>(AdjustUtils.KeyAdid) : null);
					adjustEventFailure.Message = ((!(eventFailureData.Get<string>(AdjustUtils.KeyMessage) == string.Empty)) ? eventFailureData.Get<string>(AdjustUtils.KeyMessage) : null);
					adjustEventFailure.WillRetry = eventFailureData.Get<bool>(AdjustUtils.KeyWillRetry);
					adjustEventFailure.Timestamp = ((!(eventFailureData.Get<string>(AdjustUtils.KeyTimestamp) == string.Empty)) ? eventFailureData.Get<string>(AdjustUtils.KeyTimestamp) : null);
					adjustEventFailure.EventToken = ((!(eventFailureData.Get<string>(AdjustUtils.KeyEventToken) == string.Empty)) ? eventFailureData.Get<string>(AdjustUtils.KeyEventToken) : null);
					adjustEventFailure.CallbackId = ((!(eventFailureData.Get<string>(AdjustUtils.KeyCallbackId) == string.Empty)) ? eventFailureData.Get<string>(AdjustUtils.KeyCallbackId) : null);
					try
					{
						AndroidJavaObject androidJavaObject = eventFailureData.Get<AndroidJavaObject>(AdjustUtils.KeyJsonResponse);
						string jsonResponseString = androidJavaObject.Call<string>("toString", new object[0]);
						adjustEventFailure.BuildJsonResponseFromString(jsonResponseString);
					}
					catch (Exception)
					{
					}
					callback(adjustEventFailure);
				}
			}
		}

		private class SessionTrackingSucceededListener : AndroidJavaProxy
		{
			private Action<AdjustSessionSuccess> callback;

			public SessionTrackingSucceededListener(Action<AdjustSessionSuccess> pCallback)
				: base("com.adjust.sdk.OnSessionTrackingSucceededListener")
			{
				callback = pCallback;
			}

			public void onFinishedSessionTrackingSucceeded(AndroidJavaObject sessionSuccessData)
			{
				if (callback != null && sessionSuccessData != null)
				{
					AdjustSessionSuccess adjustSessionSuccess = new AdjustSessionSuccess();
					adjustSessionSuccess.Adid = ((!(sessionSuccessData.Get<string>(AdjustUtils.KeyAdid) == string.Empty)) ? sessionSuccessData.Get<string>(AdjustUtils.KeyAdid) : null);
					adjustSessionSuccess.Message = ((!(sessionSuccessData.Get<string>(AdjustUtils.KeyMessage) == string.Empty)) ? sessionSuccessData.Get<string>(AdjustUtils.KeyMessage) : null);
					adjustSessionSuccess.Timestamp = ((!(sessionSuccessData.Get<string>(AdjustUtils.KeyTimestamp) == string.Empty)) ? sessionSuccessData.Get<string>(AdjustUtils.KeyTimestamp) : null);
					try
					{
						AndroidJavaObject androidJavaObject = sessionSuccessData.Get<AndroidJavaObject>(AdjustUtils.KeyJsonResponse);
						string jsonResponseString = androidJavaObject.Call<string>("toString", new object[0]);
						adjustSessionSuccess.BuildJsonResponseFromString(jsonResponseString);
					}
					catch (Exception)
					{
					}
					callback(adjustSessionSuccess);
				}
			}
		}

		private class SessionTrackingFailedListener : AndroidJavaProxy
		{
			private Action<AdjustSessionFailure> callback;

			public SessionTrackingFailedListener(Action<AdjustSessionFailure> pCallback)
				: base("com.adjust.sdk.OnSessionTrackingFailedListener")
			{
				callback = pCallback;
			}

			public void onFinishedSessionTrackingFailed(AndroidJavaObject sessionFailureData)
			{
				if (callback != null && sessionFailureData != null)
				{
					AdjustSessionFailure adjustSessionFailure = new AdjustSessionFailure();
					adjustSessionFailure.Adid = ((!(sessionFailureData.Get<string>(AdjustUtils.KeyAdid) == string.Empty)) ? sessionFailureData.Get<string>(AdjustUtils.KeyAdid) : null);
					adjustSessionFailure.Message = ((!(sessionFailureData.Get<string>(AdjustUtils.KeyMessage) == string.Empty)) ? sessionFailureData.Get<string>(AdjustUtils.KeyMessage) : null);
					adjustSessionFailure.WillRetry = sessionFailureData.Get<bool>(AdjustUtils.KeyWillRetry);
					adjustSessionFailure.Timestamp = ((!(sessionFailureData.Get<string>(AdjustUtils.KeyTimestamp) == string.Empty)) ? sessionFailureData.Get<string>(AdjustUtils.KeyTimestamp) : null);
					try
					{
						AndroidJavaObject androidJavaObject = sessionFailureData.Get<AndroidJavaObject>(AdjustUtils.KeyJsonResponse);
						string jsonResponseString = androidJavaObject.Call<string>("toString", new object[0]);
						adjustSessionFailure.BuildJsonResponseFromString(jsonResponseString);
					}
					catch (Exception)
					{
					}
					callback(adjustSessionFailure);
				}
			}
		}

		private class DeviceIdsReadListener : AndroidJavaProxy
		{
			private Action<string> onPlayAdIdReadCallback;

			public DeviceIdsReadListener(Action<string> pCallback)
				: base("com.adjust.sdk.OnDeviceIdsRead")
			{
				onPlayAdIdReadCallback = pCallback;
			}

			public void onGoogleAdIdRead(string playAdId)
			{
				if (onPlayAdIdReadCallback != null)
				{
					onPlayAdIdReadCallback(playAdId);
				}
			}

			public void onGoogleAdIdRead(AndroidJavaObject ajoAdId)
			{
				if (ajoAdId == null)
				{
					string playAdId = null;
					onGoogleAdIdRead(playAdId);
				}
				else
				{
					onGoogleAdIdRead(ajoAdId.Call<string>("toString", new object[0]));
				}
			}
		}

		private const string sdkPrefix = "unity4.17.0";

		private static bool launchDeferredDeeplink = true;

		private static AndroidJavaClass ajcAdjust = new AndroidJavaClass("com.adjust.sdk.Adjust");

		private static AndroidJavaObject ajoCurrentActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");

		private static DeferredDeeplinkListener onDeferredDeeplinkListener;

		private static AttributionChangeListener onAttributionChangedListener;

		private static EventTrackingFailedListener onEventTrackingFailedListener;

		private static EventTrackingSucceededListener onEventTrackingSucceededListener;

		private static SessionTrackingFailedListener onSessionTrackingFailedListener;

		private static SessionTrackingSucceededListener onSessionTrackingSucceededListener;

		public static void Start(AdjustConfig adjustConfig)
		{
			AndroidJavaObject androidJavaObject = (adjustConfig.environment != 0) ? new AndroidJavaClass("com.adjust.sdk.AdjustConfig").GetStatic<AndroidJavaObject>("ENVIRONMENT_PRODUCTION") : new AndroidJavaClass("com.adjust.sdk.AdjustConfig").GetStatic<AndroidJavaObject>("ENVIRONMENT_SANDBOX");
			bool? allowSuppressLogLevel = adjustConfig.allowSuppressLogLevel;
			AndroidJavaObject androidJavaObject2 = (!allowSuppressLogLevel.HasValue) ? new AndroidJavaObject("com.adjust.sdk.AdjustConfig", ajoCurrentActivity, adjustConfig.appToken, androidJavaObject) : new AndroidJavaObject("com.adjust.sdk.AdjustConfig", ajoCurrentActivity, adjustConfig.appToken, androidJavaObject, adjustConfig.allowSuppressLogLevel);
			launchDeferredDeeplink = adjustConfig.launchDeferredDeeplink;
			AdjustLogLevel? logLevel = adjustConfig.logLevel;
			if (logLevel.HasValue)
			{
				AndroidJavaObject androidJavaObject3 = (!adjustConfig.logLevel.Value.ToUppercaseString().Equals("SUPPRESS")) ? new AndroidJavaClass("com.adjust.sdk.LogLevel").GetStatic<AndroidJavaObject>(adjustConfig.logLevel.Value.ToUppercaseString()) : new AndroidJavaClass("com.adjust.sdk.LogLevel").GetStatic<AndroidJavaObject>("SUPRESS");
				if (androidJavaObject3 != null)
				{
					androidJavaObject2.Call("setLogLevel", androidJavaObject3);
				}
			}
			androidJavaObject2.Call("setSdkPrefix", "unity4.17.0");
			double? delayStart = adjustConfig.delayStart;
			if (delayStart.HasValue)
			{
				androidJavaObject2.Call("setDelayStart", adjustConfig.delayStart);
			}
			bool? eventBufferingEnabled = adjustConfig.eventBufferingEnabled;
			if (eventBufferingEnabled.HasValue)
			{
				AndroidJavaObject androidJavaObject4 = new AndroidJavaObject("java.lang.Boolean", adjustConfig.eventBufferingEnabled.Value);
				androidJavaObject2.Call("setEventBufferingEnabled", androidJavaObject4);
			}
			bool? sendInBackground = adjustConfig.sendInBackground;
			if (sendInBackground.HasValue)
			{
				androidJavaObject2.Call("setSendInBackground", adjustConfig.sendInBackground.Value);
			}
			if (adjustConfig.userAgent != null)
			{
				androidJavaObject2.Call("setUserAgent", adjustConfig.userAgent);
			}
			if (!string.IsNullOrEmpty(adjustConfig.processName))
			{
				androidJavaObject2.Call("setProcessName", adjustConfig.processName);
			}
			if (adjustConfig.defaultTracker != null)
			{
				androidJavaObject2.Call("setDefaultTracker", adjustConfig.defaultTracker);
			}
			if (IsAppSecretSet(adjustConfig))
			{
				androidJavaObject2.Call("setAppSecret", adjustConfig.secretId.Value, adjustConfig.info1.Value, adjustConfig.info2.Value, adjustConfig.info3.Value, adjustConfig.info4.Value);
			}
			if (adjustConfig.isDeviceKnown.HasValue)
			{
				androidJavaObject2.Call("setDeviceKnown", adjustConfig.isDeviceKnown.Value);
			}
			if (adjustConfig.readImei.HasValue)
			{
			}
			if (adjustConfig.attributionChangedDelegate != null)
			{
				onAttributionChangedListener = new AttributionChangeListener(adjustConfig.attributionChangedDelegate);
				androidJavaObject2.Call("setOnAttributionChangedListener", onAttributionChangedListener);
			}
			if (adjustConfig.eventSuccessDelegate != null)
			{
				onEventTrackingSucceededListener = new EventTrackingSucceededListener(adjustConfig.eventSuccessDelegate);
				androidJavaObject2.Call("setOnEventTrackingSucceededListener", onEventTrackingSucceededListener);
			}
			if (adjustConfig.eventFailureDelegate != null)
			{
				onEventTrackingFailedListener = new EventTrackingFailedListener(adjustConfig.eventFailureDelegate);
				androidJavaObject2.Call("setOnEventTrackingFailedListener", onEventTrackingFailedListener);
			}
			if (adjustConfig.sessionSuccessDelegate != null)
			{
				onSessionTrackingSucceededListener = new SessionTrackingSucceededListener(adjustConfig.sessionSuccessDelegate);
				androidJavaObject2.Call("setOnSessionTrackingSucceededListener", onSessionTrackingSucceededListener);
			}
			if (adjustConfig.sessionFailureDelegate != null)
			{
				onSessionTrackingFailedListener = new SessionTrackingFailedListener(adjustConfig.sessionFailureDelegate);
				androidJavaObject2.Call("setOnSessionTrackingFailedListener", onSessionTrackingFailedListener);
			}
			if (adjustConfig.deferredDeeplinkDelegate != null)
			{
				onDeferredDeeplinkListener = new DeferredDeeplinkListener(adjustConfig.deferredDeeplinkDelegate);
				androidJavaObject2.Call("setOnDeeplinkResponseListener", onDeferredDeeplinkListener);
			}
			ajcAdjust.CallStatic("onCreate", androidJavaObject2);
			ajcAdjust.CallStatic("onResume");
		}

		public static void TrackEvent(AdjustEvent adjustEvent)
		{
			AndroidJavaObject androidJavaObject = new AndroidJavaObject("com.adjust.sdk.AdjustEvent", adjustEvent.eventToken);
			double? revenue = adjustEvent.revenue;
			if (revenue.HasValue)
			{
				AndroidJavaObject androidJavaObject2 = androidJavaObject;
				object[] array = new object[2];
				double? revenue2 = adjustEvent.revenue;
				array[0] = revenue2.Value;
				array[1] = adjustEvent.currency;
				androidJavaObject2.Call("setRevenue", array);
			}
			if (adjustEvent.callbackList != null)
			{
				for (int i = 0; i < adjustEvent.callbackList.Count; i += 2)
				{
					string text = adjustEvent.callbackList[i];
					string text2 = adjustEvent.callbackList[i + 1];
					androidJavaObject.Call("addCallbackParameter", text, text2);
				}
			}
			if (adjustEvent.partnerList != null)
			{
				for (int j = 0; j < adjustEvent.partnerList.Count; j += 2)
				{
					string text3 = adjustEvent.partnerList[j];
					string text4 = adjustEvent.partnerList[j + 1];
					androidJavaObject.Call("addPartnerParameter", text3, text4);
				}
			}
			if (adjustEvent.transactionId != null)
			{
				androidJavaObject.Call("setOrderId", adjustEvent.transactionId);
			}
			if (adjustEvent.callbackId != null)
			{
				androidJavaObject.Call("setCallbackId", adjustEvent.callbackId);
			}
			ajcAdjust.CallStatic("trackEvent", androidJavaObject);
		}

		public static bool IsEnabled()
		{
			return ajcAdjust.CallStatic<bool>("isEnabled", new object[0]);
		}

		public static void SetEnabled(bool enabled)
		{
			ajcAdjust.CallStatic("setEnabled", enabled);
		}

		public static void SetOfflineMode(bool enabled)
		{
			ajcAdjust.CallStatic("setOfflineMode", enabled);
		}

		public static void SendFirstPackages()
		{
			ajcAdjust.CallStatic("sendFirstPackages");
		}

		public static void SetDeviceToken(string deviceToken)
		{
			ajcAdjust.CallStatic("setPushToken", deviceToken, ajoCurrentActivity);
		}

		public static string GetAdid()
		{
			return ajcAdjust.CallStatic<string>("getAdid", new object[0]);
		}

		public static void GdprForgetMe()
		{
			ajcAdjust.CallStatic("gdprForgetMe", ajoCurrentActivity);
		}

		public static AdjustAttribution GetAttribution()
		{
			try
			{
				AndroidJavaObject androidJavaObject = ajcAdjust.CallStatic<AndroidJavaObject>("getAttribution", new object[0]);
				if (androidJavaObject == null)
				{
					return null;
				}
				AdjustAttribution adjustAttribution = new AdjustAttribution();
				adjustAttribution.trackerName = ((!(androidJavaObject.Get<string>(AdjustUtils.KeyTrackerName) == string.Empty)) ? androidJavaObject.Get<string>(AdjustUtils.KeyTrackerName) : null);
				adjustAttribution.trackerToken = ((!(androidJavaObject.Get<string>(AdjustUtils.KeyTrackerToken) == string.Empty)) ? androidJavaObject.Get<string>(AdjustUtils.KeyTrackerToken) : null);
				adjustAttribution.network = ((!(androidJavaObject.Get<string>(AdjustUtils.KeyNetwork) == string.Empty)) ? androidJavaObject.Get<string>(AdjustUtils.KeyNetwork) : null);
				adjustAttribution.campaign = ((!(androidJavaObject.Get<string>(AdjustUtils.KeyCampaign) == string.Empty)) ? androidJavaObject.Get<string>(AdjustUtils.KeyCampaign) : null);
				adjustAttribution.adgroup = ((!(androidJavaObject.Get<string>(AdjustUtils.KeyAdgroup) == string.Empty)) ? androidJavaObject.Get<string>(AdjustUtils.KeyAdgroup) : null);
				adjustAttribution.creative = ((!(androidJavaObject.Get<string>(AdjustUtils.KeyCreative) == string.Empty)) ? androidJavaObject.Get<string>(AdjustUtils.KeyCreative) : null);
				adjustAttribution.clickLabel = ((!(androidJavaObject.Get<string>(AdjustUtils.KeyClickLabel) == string.Empty)) ? androidJavaObject.Get<string>(AdjustUtils.KeyClickLabel) : null);
				adjustAttribution.adid = ((!(androidJavaObject.Get<string>(AdjustUtils.KeyAdid) == string.Empty)) ? androidJavaObject.Get<string>(AdjustUtils.KeyAdid) : null);
				return adjustAttribution;
			}
			catch (Exception)
			{
			}
			return null;
		}

		public static void AddSessionPartnerParameter(string key, string value)
		{
			if (ajcAdjust == null)
			{
				ajcAdjust = new AndroidJavaClass("com.adjust.sdk.Adjust");
			}
			ajcAdjust.CallStatic("addSessionPartnerParameter", key, value);
		}

		public static void AddSessionCallbackParameter(string key, string value)
		{
			if (ajcAdjust == null)
			{
				ajcAdjust = new AndroidJavaClass("com.adjust.sdk.Adjust");
			}
			ajcAdjust.CallStatic("addSessionCallbackParameter", key, value);
		}

		public static void RemoveSessionPartnerParameter(string key)
		{
			if (ajcAdjust == null)
			{
				ajcAdjust = new AndroidJavaClass("com.adjust.sdk.Adjust");
			}
			ajcAdjust.CallStatic("removeSessionPartnerParameter", key);
		}

		public static void RemoveSessionCallbackParameter(string key)
		{
			if (ajcAdjust == null)
			{
				ajcAdjust = new AndroidJavaClass("com.adjust.sdk.Adjust");
			}
			ajcAdjust.CallStatic("removeSessionCallbackParameter", key);
		}

		public static void ResetSessionPartnerParameters()
		{
			if (ajcAdjust == null)
			{
				ajcAdjust = new AndroidJavaClass("com.adjust.sdk.Adjust");
			}
			ajcAdjust.CallStatic("resetSessionPartnerParameters");
		}

		public static void ResetSessionCallbackParameters()
		{
			if (ajcAdjust == null)
			{
				ajcAdjust = new AndroidJavaClass("com.adjust.sdk.Adjust");
			}
			ajcAdjust.CallStatic("resetSessionCallbackParameters");
		}

		public static void AppWillOpenUrl(string url)
		{
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("android.net.Uri");
			AndroidJavaObject androidJavaObject = androidJavaClass.CallStatic<AndroidJavaObject>("parse", new object[1]
			{
				url
			});
			ajcAdjust.CallStatic("appWillOpenUrl", androidJavaObject, ajoCurrentActivity);
		}

		public static void OnPause()
		{
			ajcAdjust.CallStatic("onPause");
		}

		public static void OnResume()
		{
			ajcAdjust.CallStatic("onResume");
		}

		public static void SetReferrer(string referrer)
		{
			ajcAdjust.CallStatic("setReferrer", referrer, ajoCurrentActivity);
		}

		public static void GetGoogleAdId(Action<string> onDeviceIdsRead)
		{
			DeviceIdsReadListener deviceIdsReadListener = new DeviceIdsReadListener(onDeviceIdsRead);
			ajcAdjust.CallStatic("getGoogleAdId", ajoCurrentActivity, deviceIdsReadListener);
		}

		public static string GetAmazonAdId()
		{
			return ajcAdjust.CallStatic<string>("getAmazonAdId", new object[1]
			{
				ajoCurrentActivity
			});
		}

		public static string GetSdkVersion()
		{
			string str = ajcAdjust.CallStatic<string>("getSdkVersion", new object[0]);
			return "unity4.17.0@" + str;
		}

		public static void SetTestOptions(Dictionary<string, string> testOptions)
		{
			AndroidJavaObject androidJavaObject = AdjustUtils.TestOptionsMap2AndroidJavaObject(testOptions, ajoCurrentActivity);
			ajcAdjust.CallStatic("setTestOptions", androidJavaObject);
		}

		private static bool IsAppSecretSet(AdjustConfig adjustConfig)
		{
			return adjustConfig.secretId.HasValue && adjustConfig.info1.HasValue && adjustConfig.info2.HasValue && adjustConfig.info3.HasValue && adjustConfig.info4.HasValue;
		}
	}
}
