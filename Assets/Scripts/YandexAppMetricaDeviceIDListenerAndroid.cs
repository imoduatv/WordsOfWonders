using System;
using UnityEngine;

public class YandexAppMetricaDeviceIDListenerAndroid : AndroidJavaProxy
{
	private readonly Action<string, YandexAppMetricaRequestDeviceIDError?> action;

	public YandexAppMetricaDeviceIDListenerAndroid(Action<string, YandexAppMetricaRequestDeviceIDError?> action)
		: base("com.yandex.metrica.AppMetricaDeviceIDListener")
	{
		this.action = action;
	}

	public void onLoaded(string deviceID)
	{
		action(deviceID, null);
	}

	public void onError(AndroidJavaObject reason)
	{
		action(null, ErrorFromAndroidReason(reason));
	}

	private YandexAppMetricaRequestDeviceIDError? ErrorFromAndroidReason(AndroidJavaObject reason)
	{
		if (reason == null)
		{
			return null;
		}
		try
		{
			string value = reason.Call<string>("toString", new object[0]);
			object obj = Enum.Parse(typeof(YandexAppMetricaRequestDeviceIDError), value);
			return (YandexAppMetricaRequestDeviceIDError?)obj;
		}
		catch (ArgumentException)
		{
			return YandexAppMetricaRequestDeviceIDError.UNKNOWN;
		}
	}
}
