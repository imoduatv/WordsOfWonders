using MoPubInternal.ThirdParty.MiniJSON;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoPubAndroidRewardedVideo
{
	private static readonly AndroidJavaClass PluginClass = new AndroidJavaClass("com.mopub.unity.MoPubRewardedVideoUnityPlugin");

	private readonly AndroidJavaObject _plugin;

	private readonly Dictionary<MoPubBase.Reward, AndroidJavaObject> _rewardsDict = new Dictionary<MoPubBase.Reward, AndroidJavaObject>();

	public MoPubAndroidRewardedVideo(string adUnitId)
	{
		_plugin = new AndroidJavaObject("com.mopub.unity.MoPubRewardedVideoUnityPlugin", adUnitId);
	}

	public static void InitializeRewardedVideo()
	{
		PluginClass.CallStatic("initializeRewardedVideo");
	}

	public static void InitializeRewardedVideoWithSdkConfiguration(MoPubBase.SdkConfiguration sdkConfiguration)
	{
		PluginClass.CallStatic("initializeRewardedVideoWithSdkConfiguration", sdkConfiguration.AdUnitId, sdkConfiguration.AdvancedBiddersString, sdkConfiguration.MediationSettingsJson, sdkConfiguration.NetworksToInitString);
	}

	public static void InitializeRewardedVideoWithNetworks(IEnumerable<string> networks)
	{
		PluginClass.CallStatic("initializeRewardedVideoWithNetworks", string.Join(",", networks.ToArray()));
	}

	public void RequestRewardedVideo(List<MoPubBase.MediationSetting> mediationSettings = null, string keywords = null, string userDataKeywords = null, double latitude = 99999.0, double longitude = 99999.0, string customerId = null)
	{
		string text = (mediationSettings == null) ? null : Json.Serialize(mediationSettings);
		_plugin.Call("requestRewardedVideo", text, keywords, userDataKeywords, latitude, longitude, customerId);
	}

	public void ShowRewardedVideo(string customData)
	{
		_plugin.Call("showRewardedVideo", customData);
	}

	public bool HasRewardedVideo()
	{
		return _plugin.Call<bool>("hasRewardedVideo", new object[0]);
	}

	public List<MoPubBase.Reward> GetAvailableRewards()
	{
		_rewardsDict.Clear();
		using (AndroidJavaObject androidJavaObject = _plugin.Call<AndroidJavaObject>("getAvailableRewards", new object[0]))
		{
			AndroidJavaObject[] array = AndroidJNIHelper.ConvertFromJNIArray<AndroidJavaObject[]>(androidJavaObject.GetRawObject());
			if (array.Length <= 1)
			{
				return new List<MoPubBase.Reward>(_rewardsDict.Keys);
			}
			AndroidJavaObject[] array2 = array;
			foreach (AndroidJavaObject androidJavaObject2 in array2)
			{
				_rewardsDict.Add(new MoPubBase.Reward
				{
					Label = androidJavaObject2.Call<string>("getLabel", new object[0]),
					Amount = androidJavaObject2.Call<int>("getAmount", new object[0])
				}, androidJavaObject2);
			}
		}
		return new List<MoPubBase.Reward>(_rewardsDict.Keys);
	}

	public void SelectReward(MoPubBase.Reward selectedReward)
	{
		if (_rewardsDict.TryGetValue(selectedReward, out AndroidJavaObject value))
		{
			_plugin.Call("selectReward", value);
		}
		else
		{
			UnityEngine.Debug.LogWarning($"Selected reward {selectedReward} is not available.");
		}
	}
}
