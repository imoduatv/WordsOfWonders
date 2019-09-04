using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YMMJSONUtils;

public class YandexAppMetricaAndroid : BaseYandexAppMetrica
{
	private readonly AndroidJavaClass metricaClass = new AndroidJavaClass("com.yandex.metrica.YandexMetrica");

	public override int LibraryApiLevel => metricaClass.CallStatic<int>("getLibraryApiLevel", new object[0]);

	public override string LibraryVersion => metricaClass.CallStatic<string>("getLibraryVersion", new object[0]);

	public override void ActivateWithConfiguration(YandexAppMetricaConfig config)
	{
		base.ActivateWithConfiguration(config);
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
			metricaClass.CallStatic("activate", @static, config.ToAndroidAppMetricaConfig());
		}
	}

	public override void ResumeSession()
	{
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
			metricaClass.CallStatic("resumeSession", @static);
		}
	}

	public override void PauseSession()
	{
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
			metricaClass.CallStatic("pauseSession", @static);
		}
	}

	public override void ReportEvent(string message)
	{
		metricaClass.CallStatic("reportEvent", message);
	}

	public override void ReportEvent(string message, Dictionary<string, object> parameters)
	{
		metricaClass.CallStatic("reportEvent", message, JsonStringFromDictionary(parameters));
	}

	public override void ReportError(string condition, string stackTrace)
	{
		AndroidJavaObject androidJavaObject = new AndroidJavaObject("java.lang.Throwable", "\n" + stackTrace);
		metricaClass.CallStatic("reportError", condition, androidJavaObject);
	}

	public override void SetLocationTracking(bool enabled)
	{
		metricaClass.CallStatic("setLocationTracking", enabled);
	}

	public override void SetLocation(YandexAppMetricaConfig.Coordinates? coordinates)
	{
		if (coordinates.HasValue)
		{
			metricaClass.CallStatic("setLocation", coordinates.Value.ToAndroidLocation());
		}
		else
		{
			metricaClass.CallStatic("setLocation", null);
		}
	}

	public override void SetUserProfileID(string userProfileID)
	{
		metricaClass.CallStatic("setUserProfileID", userProfileID);
	}

	public override void ReportUserProfile(YandexAppMetricaUserProfile userProfile)
	{
		metricaClass.CallStatic("reportUserProfile", userProfile.ToAndroidUserProfile());
	}

	public override void ReportRevenue(YandexAppMetricaRevenue revenue)
	{
		metricaClass.CallStatic("reportRevenue", revenue.ToAndroidRevenue());
	}

	public override void SetStatisticsSending(bool enabled)
	{
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
			metricaClass.CallStatic("setStatisticsSending", @static, enabled);
		}
	}

	public override void SendEventsBuffer()
	{
		metricaClass.CallStatic("sendEventsBuffer");
	}

	public override void RequestAppMetricaDeviceID(Action<string, YandexAppMetricaRequestDeviceIDError?> action)
	{
		metricaClass.CallStatic("requestAppMetricaDeviceID", new YandexAppMetricaDeviceIDListenerAndroid(action));
	}

	private string JsonStringFromDictionary(IDictionary dictionary)
	{
		return (dictionary != null) ? JSONEncoder.Encode(dictionary) : null;
	}
}
