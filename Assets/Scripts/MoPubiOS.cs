using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class MoPubiOS : MoPubBase
{
	public static class PartnerApi
	{
		public static Uri CurrentConsentPrivacyPolicyUrl
		{
			get
			{
				string url = _moPubCurrentConsentPrivacyPolicyUrl(MoPubBase.ConsentLanguageCode);
				return MoPubBase.UrlFromString(url);
			}
		}

		public static Uri CurrentVendorListUrl
		{
			get
			{
				string url = _moPubCurrentConsentVendorListUrl(MoPubBase.ConsentLanguageCode);
				return MoPubBase.UrlFromString(url);
			}
		}

		public static string CurrentConsentIabVendorListFormat => _moPubCurrentConsentIabVendorListFormat();

		public static string CurrentConsentPrivacyPolicyVersion => _moPubCurrentConsentPrivacyPolicyVersion();

		public static string CurrentConsentVendorListVersion => _moPubCurrentConsentVendorListVersion();

		public static string PreviouslyConsentedIabVendorListFormat => _moPubPreviouslyConsentedIabVendorListFormat();

		public static string PreviouslyConsentedPrivacyPolicyVersion => _moPubPreviouslyConsentedPrivacyPolicyVersion();

		public static string PreviouslyConsentedVendorListVersion => _moPubPreviouslyConsentedVendorListVersion();

		public static void GrantConsent()
		{
			_moPubGrantConsent();
		}

		public static void RevokeConsent()
		{
			_moPubRevokeConsent();
		}
	}

	private static readonly Dictionary<string, MoPubBinding> PluginsDict;

	public static bool IsSdkInitialized => _moPubIsSdkInitialized();

	public static bool AdvancedBiddingEnabled
	{
		get
		{
			return _moPubIsAdvancedBiddingEnabled();
		}
		set
		{
			_moPubSetAdvancedBiddingEnabled(value);
		}
	}

	public static LogLevel SdkLogLevel
	{
		get
		{
			return (LogLevel)_moPubGetLogLevel();
		}
		set
		{
			_moPubSetLogLevel((int)value);
		}
	}

	public static bool CanCollectPersonalInfo => _moPubCanCollectPersonalInfo();

	public static Consent.Status CurrentConsentStatus => (Consent.Status)_moPubCurrentConsentStatus();

	public static bool ShouldShowConsentDialog => _moPubShouldShowConsentDialog();

	public static bool IsConsentDialogLoaded => _moPubIsConsentDialogLoaded();

	public static bool? IsGdprApplicable
	{
		get
		{
			int num = _moPubIsGDPRApplicable();
			return (num == 0) ? null : ((num <= 0) ? new bool?(false) : new bool?(true));
		}
	}

	static MoPubiOS()
	{
		PluginsDict = new Dictionary<string, MoPubBinding>();
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
		_moPubInitializeSdk(sdkConfiguration.AdUnitId, sdkConfiguration.AdvancedBiddersString, sdkConfiguration.MediationSettingsJson, sdkConfiguration.NetworksToInitString);
	}

	public static void LoadBannerPluginsForAdUnits(string[] adUnitIds)
	{
		LoadPluginsForAdUnits(adUnitIds);
	}

	public static void LoadInterstitialPluginsForAdUnits(string[] adUnitIds)
	{
		LoadPluginsForAdUnits(adUnitIds);
	}

	public static void LoadRewardedVideoPluginsForAdUnits(string[] adUnitIds)
	{
		LoadPluginsForAdUnits(adUnitIds);
	}

	public static void EnableLocationSupport(bool shouldUseLocation)
	{
		_moPubEnableLocationSupport(shouldUseLocation: true);
	}

	public static void ReportApplicationOpen(string iTunesAppId = null)
	{
		_moPubReportApplicationOpen(iTunesAppId);
	}

	protected static string GetSdkName()
	{
		return "iOS SDK v" + _moPubGetSDKVersion();
	}

	private static void LoadPluginsForAdUnits(string[] adUnitIds)
	{
		foreach (string text in adUnitIds)
		{
			PluginsDict.Add(text, new MoPubBinding(text));
		}
		UnityEngine.Debug.Log(adUnitIds.Length + " AdUnits loaded for plugins:\n" + string.Join(", ", adUnitIds));
	}

	public static void ForceWKWebView(bool shouldForce)
	{
		_moPubForceWKWebView(shouldForce);
	}

	public static void CreateBanner(string adUnitId, AdPosition position, BannerType bannerType = BannerType.Size320x50)
	{
		if (PluginsDict.TryGetValue(adUnitId, out MoPubBinding value))
		{
			value.CreateBanner(bannerType, position);
		}
		else
		{
			MoPubBase.ReportAdUnitNotFound(adUnitId);
		}
	}

	public static void ShowBanner(string adUnitId, bool shouldShow)
	{
		if (PluginsDict.TryGetValue(adUnitId, out MoPubBinding value))
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
		if (PluginsDict.TryGetValue(adUnitId, out MoPubBinding value))
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
		if (PluginsDict.TryGetValue(adUnitId, out MoPubBinding value))
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
		if (PluginsDict.TryGetValue(adUnitId, out MoPubBinding value))
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
		if (PluginsDict.TryGetValue(adUnitId, out MoPubBinding value))
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
		if (PluginsDict.TryGetValue(adUnitId, out MoPubBinding value))
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
		if (PluginsDict.TryGetValue(adUnitId, out MoPubBinding value))
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
		if (PluginsDict.TryGetValue(adUnitId, out MoPubBinding value))
		{
			return value.IsInterstitialReady;
		}
		MoPubBase.ReportAdUnitNotFound(adUnitId);
		return false;
	}

	public void DestroyInterstitialAd(string adUnitId)
	{
		if (PluginsDict.TryGetValue(adUnitId, out MoPubBinding value))
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
		if (PluginsDict.TryGetValue(adUnitId, out MoPubBinding value))
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
		if (PluginsDict.TryGetValue(adUnitId, out MoPubBinding value))
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
		if (PluginsDict.TryGetValue(adUnitId, out MoPubBinding value))
		{
			return value.HasRewardedVideo();
		}
		MoPubBase.ReportAdUnitNotFound(adUnitId);
		return false;
	}

	public static List<Reward> GetAvailableRewards(string adUnitId)
	{
		if (!PluginsDict.TryGetValue(adUnitId, out MoPubBinding value))
		{
			MoPubBase.ReportAdUnitNotFound(adUnitId);
			return null;
		}
		List<Reward> availableRewards = value.GetAvailableRewards();
		UnityEngine.Debug.Log($"GetAvailableRewards found {availableRewards.Count} rewards for ad unit {adUnitId}");
		return availableRewards;
	}

	public static void SelectReward(string adUnitId, Reward selectedReward)
	{
		if (PluginsDict.TryGetValue(adUnitId, out MoPubBinding value))
		{
			value.SelectedReward = selectedReward;
		}
		else
		{
			MoPubBase.ReportAdUnitNotFound(adUnitId);
		}
	}

	public static void LoadConsentDialog()
	{
		_moPubLoadConsentDialog();
	}

	public static void ShowConsentDialog()
	{
		_moPubShowConsentDialog();
	}

	public static void ForceGdprApplicable()
	{
		_moPubForceGDPRApplicable();
	}

	[DllImport("__Internal")]
	private static extern void _moPubInitializeSdk(string adUnitId, string advancedBiddersString, string mediationSettingsJson, string networksToInitString);

	[DllImport("__Internal")]
	private static extern bool _moPubIsSdkInitialized();

	[DllImport("__Internal")]
	private static extern void _moPubSetAdvancedBiddingEnabled(bool advancedBiddingEnabled);

	[DllImport("__Internal")]
	private static extern bool _moPubIsAdvancedBiddingEnabled();

	[DllImport("__Internal")]
	private static extern string _moPubGetSDKVersion();

	[DllImport("__Internal")]
	private static extern void _moPubEnableLocationSupport(bool shouldUseLocation);

	[DllImport("__Internal")]
	private static extern int _moPubGetLogLevel();

	[DllImport("__Internal")]
	private static extern void _moPubSetLogLevel(int logLevel);

	[DllImport("__Internal")]
	private static extern void _moPubForceWKWebView(bool shouldForce);

	[DllImport("__Internal")]
	private static extern void _moPubReportApplicationOpen(string iTunesAppId);

	[DllImport("__Internal")]
	private static extern bool _moPubCanCollectPersonalInfo();

	[DllImport("__Internal")]
	private static extern int _moPubCurrentConsentStatus();

	[DllImport("__Internal")]
	private static extern int _moPubIsGDPRApplicable();

	[DllImport("__Internal")]
	private static extern int _moPubForceGDPRApplicable();

	[DllImport("__Internal")]
	private static extern bool _moPubShouldShowConsentDialog();

	[DllImport("__Internal")]
	private static extern bool _moPubIsConsentDialogLoaded();

	[DllImport("__Internal")]
	private static extern void _moPubLoadConsentDialog();

	[DllImport("__Internal")]
	private static extern void _moPubShowConsentDialog();

	[DllImport("__Internal")]
	private static extern string _moPubCurrentConsentPrivacyPolicyUrl(string isoLanguageCode = null);

	[DllImport("__Internal")]
	private static extern string _moPubCurrentConsentVendorListUrl(string isoLanguageCode = null);

	[DllImport("__Internal")]
	private static extern void _moPubGrantConsent();

	[DllImport("__Internal")]
	private static extern void _moPubRevokeConsent();

	[DllImport("__Internal")]
	private static extern string _moPubCurrentConsentIabVendorListFormat();

	[DllImport("__Internal")]
	private static extern string _moPubCurrentConsentPrivacyPolicyVersion();

	[DllImport("__Internal")]
	private static extern string _moPubCurrentConsentVendorListVersion();

	[DllImport("__Internal")]
	private static extern string _moPubPreviouslyConsentedIabVendorListFormat();

	[DllImport("__Internal")]
	private static extern string _moPubPreviouslyConsentedPrivacyPolicyVersion();

	[DllImport("__Internal")]
	private static extern string _moPubPreviouslyConsentedVendorListVersion();
}
