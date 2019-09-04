using System;
using System.Collections.Generic;
using UnityEngine;

public class MoPubAndroid : MoPubBase
{
	public enum LocationAwareness
	{
		TRUNCATED,
		DISABLED,
		NORMAL
	}

	public static class PartnerApi
	{
		public static Uri CurrentConsentPrivacyPolicyUrl => MoPubBase.UrlFromString(PluginClass.CallStatic<string>("getCurrentPrivacyPolicyLink", new object[1]
		{
			MoPubBase.ConsentLanguageCode
		}));

		public static Uri CurrentVendorListUrl => MoPubBase.UrlFromString(PluginClass.CallStatic<string>("getCurrentVendorListLink", new object[1]
		{
			MoPubBase.ConsentLanguageCode
		}));

		public static string CurrentConsentIabVendorListFormat => PluginClass.CallStatic<string>("getCurrentVendorListIabFormat", new object[0]);

		public static string CurrentConsentPrivacyPolicyVersion => PluginClass.CallStatic<string>("getCurrentPrivacyPolicyVersion", new object[0]);

		public static string CurrentConsentVendorListVersion => PluginClass.CallStatic<string>("getCurrentVendorListVersion", new object[0]);

		public static string PreviouslyConsentedIabVendorListFormat => PluginClass.CallStatic<string>("getConsentedVendorListIabFormat", new object[0]);

		public static string PreviouslyConsentedPrivacyPolicyVersion => PluginClass.CallStatic<string>("getConsentedPrivacyPolicyVersion", new object[0]);

		public static string PreviouslyConsentedVendorListVersion => PluginClass.CallStatic<string>("getConsentedVendorListVersion", new object[0]);

		public static void GrantConsent()
		{
			PluginClass.CallStatic("grantConsent");
		}

		public static void RevokeConsent()
		{
			PluginClass.CallStatic("revokeConsent");
		}
	}

	private static readonly AndroidJavaClass PluginClass;

	private static readonly Dictionary<string, MoPubAndroidBanner> BannerPluginsDict;

	private static readonly Dictionary<string, MoPubAndroidInterstitial> InterstitialPluginsDict;

	private static readonly Dictionary<string, MoPubAndroidRewardedVideo> RewardedVideoPluginsDict;

	public static bool IsSdkInitialized => PluginClass.CallStatic<bool>("isSdkInitialized", new object[0]);

	public static bool AdvancedBiddingEnabled
	{
		get
		{
			return PluginClass.CallStatic<bool>("isAdvancedBiddingEnabled", new object[0]);
		}
		set
		{
			PluginClass.CallStatic("setAdvancedBiddingEnabled", value);
		}
	}

	public static bool CanCollectPersonalInfo => PluginClass.CallStatic<bool>("canCollectPersonalInfo", new object[0]);

	public static Consent.Status CurrentConsentStatus => Consent.FromString(PluginClass.CallStatic<string>("getPersonalInfoConsentState", new object[0]));

	public static bool ShouldShowConsentDialog => PluginClass.CallStatic<bool>("shouldShowConsentDialog", new object[0]);

	public static bool IsConsentDialogLoaded => PluginClass.CallStatic<bool>("isConsentDialogLoaded", new object[0]);

	public static bool? IsGdprApplicable
	{
		get
		{
			int num = PluginClass.CallStatic<int>("gdprApplies", new object[0]);
			return (num == 0) ? null : ((num <= 0) ? new bool?(false) : new bool?(true));
		}
	}

	static MoPubAndroid()
	{
		PluginClass = new AndroidJavaClass("com.mopub.unity.MoPubUnityPlugin");
		BannerPluginsDict = new Dictionary<string, MoPubAndroidBanner>();
		InterstitialPluginsDict = new Dictionary<string, MoPubAndroidInterstitial>();
		RewardedVideoPluginsDict = new Dictionary<string, MoPubAndroidRewardedVideo>();
		MoPubBase.InitManager();
	}

	public static void InitializeSdk(string anyAdUnitId)
	{
		MoPubBase.ValidateAdUnitForSdkInit(anyAdUnitId);
		SdkConfiguration sdkConfiguration = default(SdkConfiguration);
		sdkConfiguration.AdUnitId = anyAdUnitId;
		InitializeSdk(sdkConfiguration);
	}

	public static void InitializeSdk(SdkConfiguration sdkConfiguration)
	{
		MoPubBase.ValidateAdUnitForSdkInit(sdkConfiguration.AdUnitId);
		PluginClass.CallStatic("initializeSdk", sdkConfiguration.AdUnitId, sdkConfiguration.AdvancedBiddersString, sdkConfiguration.MediationSettingsJson, sdkConfiguration.NetworksToInitString);
	}

	public static void LoadBannerPluginsForAdUnits(string[] bannerAdUnitIds)
	{
		foreach (string text in bannerAdUnitIds)
		{
			BannerPluginsDict.Add(text, new MoPubAndroidBanner(text));
		}
		UnityEngine.Debug.Log(bannerAdUnitIds.Length + " banner AdUnits loaded for plugins:\n" + string.Join(", ", bannerAdUnitIds));
	}

	public static void LoadInterstitialPluginsForAdUnits(string[] interstitialAdUnitIds)
	{
		foreach (string text in interstitialAdUnitIds)
		{
			InterstitialPluginsDict.Add(text, new MoPubAndroidInterstitial(text));
		}
		UnityEngine.Debug.Log(interstitialAdUnitIds.Length + " interstitial AdUnits loaded for plugins:\n" + string.Join(", ", interstitialAdUnitIds));
	}

	public static void LoadRewardedVideoPluginsForAdUnits(string[] rewardedVideoAdUnitIds)
	{
		foreach (string text in rewardedVideoAdUnitIds)
		{
			RewardedVideoPluginsDict.Add(text, new MoPubAndroidRewardedVideo(text));
		}
		UnityEngine.Debug.Log(rewardedVideoAdUnitIds.Length + " rewarded video AdUnits loaded for plugins:\n" + string.Join(", ", rewardedVideoAdUnitIds));
	}

	public static void EnableLocationSupport(bool shouldUseLocation)
	{
		PluginClass.CallStatic("setLocationAwareness", LocationAwareness.NORMAL.ToString());
	}

	public static void ReportApplicationOpen(string iTunesAppId = null)
	{
		PluginClass.CallStatic("reportApplicationOpen");
	}

	protected static string GetSdkName()
	{
		return "Android SDK v" + PluginClass.CallStatic<string>("getSDKVersion", new object[0]);
	}

	public static void AddFacebookTestDeviceId(string hashedDeviceId)
	{
		PluginClass.CallStatic("addFacebookTestDeviceId", hashedDeviceId);
	}

	public static void CreateBanner(string adUnitId, AdPosition position)
	{
		if (BannerPluginsDict.TryGetValue(adUnitId, out MoPubAndroidBanner value))
		{
			value.CreateBanner(position);
		}
		else
		{
			MoPubBase.ReportAdUnitNotFound(adUnitId);
		}
	}

	public static void ShowBanner(string adUnitId, bool shouldShow)
	{
		if (BannerPluginsDict.TryGetValue(adUnitId, out MoPubAndroidBanner value))
		{
			value.ShowBanner(shouldShow);
		}
		else
		{
			MoPubBase.ReportAdUnitNotFound(adUnitId);
		}
	}

	public static void RefreshBanner(string adUnitId, string keywords, string userDataKeywords = "")
	{
		if (BannerPluginsDict.TryGetValue(adUnitId, out MoPubAndroidBanner value))
		{
			value.RefreshBanner(keywords, userDataKeywords);
		}
		else
		{
			MoPubBase.ReportAdUnitNotFound(adUnitId);
		}
	}

	public void SetAutorefresh(string adUnitId, bool enabled)
	{
		if (BannerPluginsDict.TryGetValue(adUnitId, out MoPubAndroidBanner value))
		{
			value.SetAutorefresh(enabled);
		}
		else
		{
			MoPubBase.ReportAdUnitNotFound(adUnitId);
		}
	}

	public void ForceRefresh(string adUnitId)
	{
		if (BannerPluginsDict.TryGetValue(adUnitId, out MoPubAndroidBanner value))
		{
			value.ForceRefresh();
		}
		else
		{
			MoPubBase.ReportAdUnitNotFound(adUnitId);
		}
	}

	public static void DestroyBanner(string adUnitId)
	{
		if (BannerPluginsDict.TryGetValue(adUnitId, out MoPubAndroidBanner value))
		{
			value.DestroyBanner();
		}
		else
		{
			MoPubBase.ReportAdUnitNotFound(adUnitId);
		}
	}

	public static void RequestInterstitialAd(string adUnitId, string keywords = "", string userDataKeywords = "")
	{
		if (InterstitialPluginsDict.TryGetValue(adUnitId, out MoPubAndroidInterstitial value))
		{
			value.RequestInterstitialAd(keywords, userDataKeywords);
		}
		else
		{
			MoPubBase.ReportAdUnitNotFound(adUnitId);
		}
	}

	public static void ShowInterstitialAd(string adUnitId)
	{
		if (InterstitialPluginsDict.TryGetValue(adUnitId, out MoPubAndroidInterstitial value))
		{
			value.ShowInterstitialAd();
		}
		else
		{
			MoPubBase.ReportAdUnitNotFound(adUnitId);
		}
	}

	public bool IsInterstialReady(string adUnitId)
	{
		if (InterstitialPluginsDict.TryGetValue(adUnitId, out MoPubAndroidInterstitial value))
		{
			return value.IsInterstitialReady;
		}
		MoPubBase.ReportAdUnitNotFound(adUnitId);
		return false;
	}

	public void DestroyInterstitialAd(string adUnitId)
	{
		if (InterstitialPluginsDict.TryGetValue(adUnitId, out MoPubAndroidInterstitial value))
		{
			value.DestroyInterstitialAd();
		}
		else
		{
			MoPubBase.ReportAdUnitNotFound(adUnitId);
		}
	}

	public static void RequestRewardedVideo(string adUnitId, List<MediationSetting> mediationSettings = null, string keywords = null, string userDataKeywords = null, double latitude = 99999.0, double longitude = 99999.0, string customerId = null)
	{
		if (RewardedVideoPluginsDict.TryGetValue(adUnitId, out MoPubAndroidRewardedVideo value))
		{
			value.RequestRewardedVideo(mediationSettings, keywords, userDataKeywords, latitude, longitude, customerId);
		}
		else
		{
			MoPubBase.ReportAdUnitNotFound(adUnitId);
		}
	}

	public static void ShowRewardedVideo(string adUnitId, string customData = null)
	{
		if (RewardedVideoPluginsDict.TryGetValue(adUnitId, out MoPubAndroidRewardedVideo value))
		{
			value.ShowRewardedVideo(customData);
		}
		else
		{
			MoPubBase.ReportAdUnitNotFound(adUnitId);
		}
	}

	public static bool HasRewardedVideo(string adUnitId)
	{
		if (RewardedVideoPluginsDict.TryGetValue(adUnitId, out MoPubAndroidRewardedVideo value))
		{
			return value.HasRewardedVideo();
		}
		MoPubBase.ReportAdUnitNotFound(adUnitId);
		return false;
	}

	public static List<Reward> GetAvailableRewards(string adUnitId)
	{
		if (RewardedVideoPluginsDict.TryGetValue(adUnitId, out MoPubAndroidRewardedVideo value))
		{
			return value.GetAvailableRewards();
		}
		MoPubBase.ReportAdUnitNotFound(adUnitId);
		return null;
	}

	public static void SelectReward(string adUnitId, Reward selectedReward)
	{
		if (RewardedVideoPluginsDict.TryGetValue(adUnitId, out MoPubAndroidRewardedVideo value))
		{
			value.SelectReward(selectedReward);
		}
		else
		{
			MoPubBase.ReportAdUnitNotFound(adUnitId);
		}
	}

	public static void LoadConsentDialog()
	{
		PluginClass.CallStatic("loadConsentDialog");
	}

	public static void ShowConsentDialog()
	{
		PluginClass.CallStatic("showConsentDialog");
	}

	public static void ForceGdprApplicable()
	{
		PluginClass.CallStatic("forceGdprApplies");
	}
}
