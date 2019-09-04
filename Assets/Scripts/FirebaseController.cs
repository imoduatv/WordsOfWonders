using Firebase.Analytics;
using Firebase.RemoteConfig;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class FirebaseController : MonoBehaviour
{
	private static bool firebaseReady = false;

	private static int[] eventLevels = new int[7]
	{
		10,
		25,
		50,
		100,
		250,
		500,
		1000
	};

	private void Awake()
	{
		SetDefaults();
		FetchDataAsync();
	}

	private static void SetDefaults()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("menu_background", "1");
		dictionary.Add("daily_double", "1");
		dictionary.Add("redeem_status", "0");
		dictionary.Add("interstitial_firstlevel", "10");
		dictionary.Add("rewarded_status", "1");
		dictionary.Add("interstitial_interval", "120");
		dictionary.Add("level_test", "2");
		dictionary.Add("play_text_color", "2");
		dictionary.Add("menu_theme", "0");
		Dictionary<string, object> defaults = dictionary;
		FirebaseRemoteConfig.SetDefaults(defaults);
	}

	private static void FetchDataAsync()
	{
		Task task2 = FirebaseRemoteConfig.FetchAsync(TimeSpan.Zero);
		task2.ContinueWith(delegate
		{
			ConfigInfo info = FirebaseRemoteConfig.Info;
			switch (info.LastFetchStatus)
			{
			case LastFetchStatus.Success:
				FirebaseRemoteConfig.ActivateFetched();
				UnityEngine.Debug.Log("Fetch is completed.");
				break;
			case LastFetchStatus.Failure:
				switch (info.LastFetchFailureReason)
				{
				}
				break;
			}
		});
	}

	public static string FetchRemoteValue(string key)
	{
		try
		{
			return FirebaseRemoteConfig.GetValue(key).StringValue;
		}
		catch
		{
			return string.Empty;
		}
	}

	public static void SendLevelLog()
	{
		if (!firebaseReady)
		{
			return;
		}
		int num = 0;
		int[] array = eventLevels;
		foreach (int num2 in array)
		{
			if (PlayerPrefsManager.GetLevel() >= num2)
			{
				num = num2;
			}
		}
		if (num > 0 && PlayerPrefsManager.GetFirebaseLogLevel() < num)
		{
			UnityEngine.Debug.Log("log sent: " + num.ToString());
			try
			{
				FirebaseAnalytics.LogEvent("level" + num.ToString() + "_reached", string.Empty, 0.0);
				PlayerPrefsManager.SetFirebaseLogLevel(num);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.Log(ex.Message);
			}
		}
	}

	public static void SendLastWord(int level, string word)
	{
	}

	public static void LogEvent(string name, string param, string val)
	{
		try
		{
			FirebaseAnalytics.LogEvent(name, param, val);
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.Log(ex.Message);
		}
	}
}
