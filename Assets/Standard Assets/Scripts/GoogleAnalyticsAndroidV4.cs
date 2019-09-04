using System;
using System.Collections.Generic;
using UnityEngine;

public class GoogleAnalyticsAndroidV4 : IDisposable
{
	private string trackingCode;

	private string appVersion;

	private string appName;

	private string bundleIdentifier;

	private int dispatchPeriod;

	private int sampleFrequency;

	private bool anonymizeIP;

	private bool adIdCollection;

	private bool dryRun;

	private int sessionTimeout;

	//private AndroidJavaObject tracker;
	//private AndroidJavaObject logger;
	//private AndroidJavaObject currentActivityObject;
	//private AndroidJavaObject googleAnalyticsSingleton;

	internal void InitializeTracker()
	{
		//UnityEngine.Debug.Log("Initializing Google Analytics Android Tracker.");
		//using (AndroidJavaObject androidJavaObject = new AndroidJavaClass("com.google.android.gms.analytics.GoogleAnalytics"))
		//{
		//	using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		//	{
		//		currentActivityObject = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
		//		googleAnalyticsSingleton = androidJavaObject.CallStatic<AndroidJavaObject>("getInstance", new object[1]
		//		{
		//			currentActivityObject
		//		});
		//		tracker = googleAnalyticsSingleton.Call<AndroidJavaObject>("newTracker", new object[1]
		//		{
		//			trackingCode
		//		});
		//		googleAnalyticsSingleton.Call("setLocalDispatchPeriod", dispatchPeriod);
		//		googleAnalyticsSingleton.Call("setDryRun", dryRun);
		//		tracker.Call("setSampleRate", (double)sampleFrequency);
		//		tracker.Call("setAppName", appName);
		//		tracker.Call("setAppId", bundleIdentifier);
		//		tracker.Call("setAppVersion", appVersion);
		//		tracker.Call("setAnonymizeIp", anonymizeIP);
		//		tracker.Call("enableAdvertisingIdCollection", adIdCollection);
		//	}
		//}
	}

	internal void SetTrackerVal(Field fieldName, object value)
	{
		object[] args = new object[2]
		{
			fieldName.ToString(),
			value
		};
		//tracker.Call("set", args);
	}

	private void SetSessionOnBuilder(AndroidJavaObject hitBuilder)
	{
	}

	internal void StartSession()
	{
	}

	internal void StopSession()
	{
	}

	public void SetOptOut(bool optOut)
	{
		//googleAnalyticsSingleton.Call("setAppOptOut", optOut);
	}

	internal void LogScreen(AppViewHitBuilder builder)
	{
		//tracker.Call("setScreenName", builder.GetScreenName());
		//AndroidJavaObject androidJavaObject = new AndroidJavaObject("com.google.android.gms.analytics.HitBuilders$ScreenViewBuilder");
		//object[] args = new object[1]
		//{
		//	androidJavaObject.Call<AndroidJavaObject>("build", new object[0])
		//};
		//tracker.Call("send", args);
	}

	internal void LogEvent(EventHitBuilder builder)
	{
		//AndroidJavaObject androidJavaObject = new AndroidJavaObject("com.google.android.gms.analytics.HitBuilders$EventBuilder");
		//androidJavaObject.Call<AndroidJavaObject>("setCategory", new object[1]
		//{
		//	builder.GetEventCategory()
		//});
		//androidJavaObject.Call<AndroidJavaObject>("setAction", new object[1]
		//{
		//	builder.GetEventAction()
		//});
		//androidJavaObject.Call<AndroidJavaObject>("setLabel", new object[1]
		//{
		//	builder.GetEventLabel()
		//});
		//androidJavaObject.Call<AndroidJavaObject>("setValue", new object[1]
		//{
		//	builder.GetEventValue()
		//});
		//foreach (KeyValuePair<int, string> customDimension in builder.GetCustomDimensions())
		//{
		//	androidJavaObject.Call<AndroidJavaObject>("setCustomDimension", new object[2]
		//	{
		//		customDimension.Key,
		//		customDimension.Value
		//	});
		//}
		//foreach (KeyValuePair<int, float> customMetric in builder.GetCustomMetrics())
		//{
		//	androidJavaObject.Call<AndroidJavaObject>("setCustomMetric", new object[2]
		//	{
		//		customMetric.Key,
		//		customMetric.Value
		//	});
		//}
		//object[] args = new object[1]
		//{
		//	androidJavaObject.Call<AndroidJavaObject>("build", new object[0])
		//};
		//tracker.Call("send", args);
	}

	internal void LogTransaction(TransactionHitBuilder builder)
	{
		//AndroidJavaObject androidJavaObject = new AndroidJavaObject("com.google.android.gms.analytics.HitBuilders$TransactionBuilder");
		//androidJavaObject.Call<AndroidJavaObject>("setTransactionId", new object[1]
		//{
		//	builder.GetTransactionID()
		//});
		//androidJavaObject.Call<AndroidJavaObject>("setAffiliation", new object[1]
		//{
		//	builder.GetAffiliation()
		//});
		//androidJavaObject.Call<AndroidJavaObject>("setRevenue", new object[1]
		//{
		//	builder.GetRevenue()
		//});
		//androidJavaObject.Call<AndroidJavaObject>("setTax", new object[1]
		//{
		//	builder.GetTax()
		//});
		//androidJavaObject.Call<AndroidJavaObject>("setShipping", new object[1]
		//{
		//	builder.GetShipping()
		//});
		//androidJavaObject.Call<AndroidJavaObject>("setCurrencyCode", new object[1]
		//{
		//	builder.GetCurrencyCode()
		//});
		//object[] args = new object[1]
		//{
		//	androidJavaObject.Call<AndroidJavaObject>("build", new object[0])
		//};
		//tracker.Call("send", args);
	}

	internal void LogItem(ItemHitBuilder builder)
	{
		//AndroidJavaObject androidJavaObject = new AndroidJavaObject("com.google.android.gms.analytics.HitBuilders$ItemBuilder");
		//androidJavaObject.Call<AndroidJavaObject>("setTransactionId", new object[1]
		//{
		//	builder.GetTransactionID()
		//});
		//androidJavaObject.Call<AndroidJavaObject>("setName", new object[1]
		//{
		//	builder.GetName()
		//});
		//androidJavaObject.Call<AndroidJavaObject>("setSku", new object[1]
		//{
		//	builder.GetSKU()
		//});
		//androidJavaObject.Call<AndroidJavaObject>("setCategory", new object[1]
		//{
		//	builder.GetCategory()
		//});
		//androidJavaObject.Call<AndroidJavaObject>("setPrice", new object[1]
		//{
		//	builder.GetPrice()
		//});
		//androidJavaObject.Call<AndroidJavaObject>("setQuantity", new object[1]
		//{
		//	builder.GetQuantity()
		//});
		//androidJavaObject.Call<AndroidJavaObject>("setCurrencyCode", new object[1]
		//{
		//	builder.GetCurrencyCode()
		//});
		//object[] args = new object[1]
		//{
		//	androidJavaObject.Call<AndroidJavaObject>("build", new object[0])
		//};
		//tracker.Call("send", args);
	}

	public void LogException(ExceptionHitBuilder builder)
	{
		//AndroidJavaObject androidJavaObject = new AndroidJavaObject("com.google.android.gms.analytics.HitBuilders$ExceptionBuilder");
		//androidJavaObject.Call<AndroidJavaObject>("setDescription", new object[1]
		//{
		//	builder.GetExceptionDescription()
		//});
		//androidJavaObject.Call<AndroidJavaObject>("setFatal", new object[1]
		//{
		//	builder.IsFatal()
		//});
		//object[] args = new object[1]
		//{
		//	androidJavaObject.Call<AndroidJavaObject>("build", new object[0])
		//};
		//tracker.Call("send", args);
	}

	public void LogSocial(SocialHitBuilder builder)
	{
		//AndroidJavaObject androidJavaObject = new AndroidJavaObject("com.google.android.gms.analytics.HitBuilders$SocialBuilder");
		//androidJavaObject.Call<AndroidJavaObject>("setAction", new object[1]
		//{
		//	builder.GetSocialAction()
		//});
		//androidJavaObject.Call<AndroidJavaObject>("setNetwork", new object[1]
		//{
		//	builder.GetSocialNetwork()
		//});
		//androidJavaObject.Call<AndroidJavaObject>("setTarget", new object[1]
		//{
		//	builder.GetSocialTarget()
		//});
		//object[] args = new object[1]
		//{
		//	androidJavaObject.Call<AndroidJavaObject>("build", new object[0])
		//};
		//tracker.Call("send", args);
	}

	public void LogTiming(TimingHitBuilder builder)
	{
		//AndroidJavaObject androidJavaObject = new AndroidJavaObject("com.google.android.gms.analytics.HitBuilders$TimingBuilder");
		//androidJavaObject.Call<AndroidJavaObject>("setCategory", new object[1]
		//{
		//	builder.GetTimingCategory()
		//});
		//androidJavaObject.Call<AndroidJavaObject>("setLabel", new object[1]
		//{
		//	builder.GetTimingLabel()
		//});
		//androidJavaObject.Call<AndroidJavaObject>("setValue", new object[1]
		//{
		//	builder.GetTimingInterval()
		//});
		//androidJavaObject.Call<AndroidJavaObject>("setVariable", new object[1]
		//{
		//	builder.GetTimingName()
		//});
		//object[] args = new object[1]
		//{
		//	androidJavaObject.Call<AndroidJavaObject>("build", new object[0])
		//};
		//tracker.Call("send", args);
	}

	public void DispatchHits()
	{
	}

	public void SetSampleFrequency(int sampleFrequency)
	{
		this.sampleFrequency = sampleFrequency;
	}

	public void ClearUserIDOverride()
	{
		SetTrackerVal(Fields.USER_ID, null);
	}

	public void SetTrackingCode(string trackingCode)
	{
		this.trackingCode = trackingCode;
	}

	public void SetAppName(string appName)
	{
		this.appName = appName;
	}

	public void SetBundleIdentifier(string bundleIdentifier)
	{
		this.bundleIdentifier = bundleIdentifier;
	}

	public void SetAppVersion(string appVersion)
	{
		this.appVersion = appVersion;
	}

	public void SetDispatchPeriod(int dispatchPeriod)
	{
		this.dispatchPeriod = dispatchPeriod;
	}

	public void SetLogLevelValue(GoogleAnalyticsV4.DebugMode logLevel)
	{
	}

	public void SetAnonymizeIP(bool anonymizeIP)
	{
		this.anonymizeIP = anonymizeIP;
	}

	public void SetAdIdCollection(bool adIdCollection)
	{
		this.adIdCollection = adIdCollection;
	}

	public void SetDryRun(bool dryRun)
	{
		this.dryRun = dryRun;
	}

	public void Dispose()
	{
		//googleAnalyticsSingleton.Dispose();
		//tracker.Dispose();
	}
}
