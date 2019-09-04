using MoPubInternal.ThirdParty.MiniJSON;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoPubManager : MonoBehaviour
{
	public static MoPubManager Instance
	{
		get;
		private set;
	}

	public static event Action<string> OnSdkInitalizedEvent;

	public static event Action<string, float> OnAdLoadedEvent;

	public static event Action<string, string> OnAdFailedEvent;

	public static event Action<string> OnAdClickedEvent;

	public static event Action<string> OnAdExpandedEvent;

	public static event Action<string> OnAdCollapsedEvent;

	public static event Action<string> OnInterstitialLoadedEvent;

	public static event Action<string, string> OnInterstitialFailedEvent;

	public static event Action<string> OnInterstitialDismissedEvent;

	public static event Action<string> OnInterstitialExpiredEvent;

	public static event Action<string> OnInterstitialShownEvent;

	public static event Action<string> OnInterstitialClickedEvent;

	public static event Action<string> OnRewardedVideoLoadedEvent;

	public static event Action<string, string> OnRewardedVideoFailedEvent;

	public static event Action<string> OnRewardedVideoExpiredEvent;

	public static event Action<string> OnRewardedVideoShownEvent;

	public static event Action<string> OnRewardedVideoClickedEvent;

	public static event Action<string, string> OnRewardedVideoFailedToPlayEvent;

	public static event Action<string, string, float> OnRewardedVideoReceivedRewardEvent;

	public static event Action<string> OnRewardedVideoClosedEvent;

	public static event Action<string> OnRewardedVideoLeavingApplicationEvent;

	public static event Action<MoPubBase.Consent.Status, MoPubBase.Consent.Status, bool> OnConsentStatusChangedEvent;

	public static event Action OnConsentDialogLoadedEvent;

	public static event Action<string> OnConsentDialogFailedEvent;

	public static event Action OnConsentDialogShownEvent;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}
		else
		{
			UnityEngine.Object.Destroy(this);
		}
	}

	private void OnDestroy()
	{
		if (Instance == this)
		{
			Instance = null;
		}
	}

	private string[] DecodeArgs(string argsJson, int min)
	{
		bool flag = false;
		List<object> list = Json.Deserialize(argsJson) as List<object>;
		if (list == null)
		{
			UnityEngine.Debug.LogError("Invalid JSON data: " + argsJson);
			list = new List<object>();
			flag = true;
		}
		if (list.Count < min)
		{
			if (!flag)
			{
				UnityEngine.Debug.LogError("Missing one or more values: " + argsJson + " (expected " + min + ")");
			}
			while (list.Count < min)
			{
				list.Add(string.Empty);
			}
		}
		return (from v in list
			select v.ToString()).ToArray();
	}

	public void EmitSdkInitializedEvent(string argsJson)
	{
		string[] array = DecodeArgs(argsJson, 1);
		string obj = array[0];
		MoPubManager.OnSdkInitalizedEvent?.Invoke(obj);
	}

	public void EmitConsentStatusChangedEvent(string argsJson)
	{
		string[] array = DecodeArgs(argsJson, 3);
		MoPubBase.Consent.Status arg = MoPubBase.Consent.FromString(array[0]);
		MoPubBase.Consent.Status arg2 = MoPubBase.Consent.FromString(array[1]);
		bool arg3 = array[2].ToLower() == "true";
		MoPubManager.OnConsentStatusChangedEvent?.Invoke(arg, arg2, arg3);
	}

	public void EmitConsentDialogLoadedEvent()
	{
		MoPubManager.OnConsentDialogLoadedEvent?.Invoke();
	}

	public void EmitConsentDialogFailedEvent(string argsJson)
	{
		string[] array = DecodeArgs(argsJson, 1);
		string obj = array[0];
		MoPubManager.OnConsentDialogFailedEvent?.Invoke(obj);
	}

	public void EmitConsentDialogShownEvent()
	{
		MoPubManager.OnConsentDialogShownEvent?.Invoke();
	}

	public void EmitAdLoadedEvent(string argsJson)
	{
		string[] array = DecodeArgs(argsJson, 2);
		string arg = array[0];
		string s = array[1];
		MoPubManager.OnAdLoadedEvent?.Invoke(arg, float.Parse(s));
	}

	public void EmitAdFailedEvent(string argsJson)
	{
		string[] array = DecodeArgs(argsJson, 2);
		string arg = array[0];
		string arg2 = array[1];
		MoPubManager.OnAdFailedEvent?.Invoke(arg, arg2);
	}

	public void EmitAdClickedEvent(string argsJson)
	{
		string[] array = DecodeArgs(argsJson, 1);
		string obj = array[0];
		MoPubManager.OnAdClickedEvent?.Invoke(obj);
	}

	public void EmitAdExpandedEvent(string argsJson)
	{
		string[] array = DecodeArgs(argsJson, 1);
		string obj = array[0];
		MoPubManager.OnAdExpandedEvent?.Invoke(obj);
	}

	public void EmitAdCollapsedEvent(string argsJson)
	{
		string[] array = DecodeArgs(argsJson, 1);
		string obj = array[0];
		MoPubManager.OnAdCollapsedEvent?.Invoke(obj);
	}

	public void EmitInterstitialLoadedEvent(string argsJson)
	{
		string[] array = DecodeArgs(argsJson, 1);
		string obj = array[0];
		MoPubManager.OnInterstitialLoadedEvent?.Invoke(obj);
	}

	public void EmitInterstitialFailedEvent(string argsJson)
	{
		string[] array = DecodeArgs(argsJson, 2);
		string arg = array[0];
		string arg2 = array[1];
		MoPubManager.OnInterstitialFailedEvent?.Invoke(arg, arg2);
	}

	public void EmitInterstitialDismissedEvent(string argsJson)
	{
		string[] array = DecodeArgs(argsJson, 1);
		string obj = array[0];
		MoPubManager.OnInterstitialDismissedEvent?.Invoke(obj);
	}

	public void EmitInterstitialDidExpireEvent(string argsJson)
	{
		string[] array = DecodeArgs(argsJson, 1);
		string obj = array[0];
		MoPubManager.OnInterstitialExpiredEvent?.Invoke(obj);
	}

	public void EmitInterstitialShownEvent(string argsJson)
	{
		string[] array = DecodeArgs(argsJson, 1);
		string obj = array[0];
		MoPubManager.OnInterstitialShownEvent?.Invoke(obj);
	}

	public void EmitInterstitialClickedEvent(string argsJson)
	{
		string[] array = DecodeArgs(argsJson, 1);
		string obj = array[0];
		MoPubManager.OnInterstitialClickedEvent?.Invoke(obj);
	}

	public void EmitRewardedVideoLoadedEvent(string argsJson)
	{
		string[] array = DecodeArgs(argsJson, 1);
		string obj = array[0];
		MoPubManager.OnRewardedVideoLoadedEvent?.Invoke(obj);
	}

	public void EmitRewardedVideoFailedEvent(string argsJson)
	{
		string[] array = DecodeArgs(argsJson, 2);
		string arg = array[0];
		string arg2 = array[1];
		MoPubManager.OnRewardedVideoFailedEvent?.Invoke(arg, arg2);
	}

	public void EmitRewardedVideoExpiredEvent(string argsJson)
	{
		string[] array = DecodeArgs(argsJson, 1);
		string obj = array[0];
		MoPubManager.OnRewardedVideoExpiredEvent?.Invoke(obj);
	}

	public void EmitRewardedVideoShownEvent(string argsJson)
	{
		string[] array = DecodeArgs(argsJson, 1);
		string obj = array[0];
		MoPubManager.OnRewardedVideoShownEvent?.Invoke(obj);
	}

	public void EmitRewardedVideoClickedEvent(string argsJson)
	{
		string[] array = DecodeArgs(argsJson, 1);
		string obj = array[0];
		MoPubManager.OnRewardedVideoClickedEvent?.Invoke(obj);
	}

	public void EmitRewardedVideoFailedToPlayEvent(string argsJson)
	{
		string[] array = DecodeArgs(argsJson, 2);
		string arg = array[0];
		string arg2 = array[1];
		MoPubManager.OnRewardedVideoFailedToPlayEvent?.Invoke(arg, arg2);
	}

	public void EmitRewardedVideoReceivedRewardEvent(string argsJson)
	{
		string[] array = DecodeArgs(argsJson, 3);
		string arg = array[0];
		string arg2 = array[1];
		string s = array[2];
		MoPubManager.OnRewardedVideoReceivedRewardEvent?.Invoke(arg, arg2, float.Parse(s));
	}

	public void EmitRewardedVideoClosedEvent(string argsJson)
	{
		string[] array = DecodeArgs(argsJson, 1);
		string obj = array[0];
		MoPubManager.OnRewardedVideoClosedEvent?.Invoke(obj);
	}

	public void EmitRewardedVideoLeavingApplicationEvent(string argsJson)
	{
		string[] array = DecodeArgs(argsJson, 1);
		string obj = array[0];
		MoPubManager.OnRewardedVideoLeavingApplicationEvent?.Invoke(obj);
	}
}
