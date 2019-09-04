using Firebase.Analytics;
using Firebase.RemoteConfig;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FugoAdManager : MonoBehaviour
{
	public delegate void NoParam();

	public static FugoAdManager instance;

	public static int firstLevel = 13;

	public static float interstitialTime = 300f;

	public static float lastInterstitialTime = -9999f;

	public static bool rewardedEnabled;

	public static bool isBannerAtTop;

	public NoParam interstitialClosed;

	public Transform coinHolder;

	private static int emptyInterstitialCount;

	private float adTimer;

	private string appKey;

	private string AdID;

	private bool isRewarded;

	private string rewardedType = string.Empty;

	public static bool rewardedCompleted;

	public static bool rewardedOpened;

	public static bool interstitialAvailable;

	private MoPubBase.Consent.Status _currentConsentStatus;

	private bool _canCollectPersonalInfo;

	private bool _shouldShowConsentDialog;

	private bool? _isGdprApplicable = false;

	private readonly string[] _bannerAdUnits = new string[1]
	{
		"389ea3c655244a0daa3ae0c5e7409a21"
	};

	private readonly string[] _interstitialAdUnits = new string[1]
	{
		"2e56984d2cd2476699657e7279d9d51f"
	};

	private readonly string[] _rewardedVideoAdUnits = new string[1]
	{
		"bb275bfb3f1b4d46b11d24a420f7746d"
	};

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			string a = string.Empty;
			string text = string.Empty;
			string text2 = string.Empty;
			try
			{
				a = FirebaseRemoteConfig.GetValue("rewarded_status").StringValue;
				text = FirebaseRemoteConfig.GetValue("interstitial_interval").StringValue;
				text2 = FirebaseRemoteConfig.GetValue("interstitial_firstlevel").StringValue;
			}
			catch (Exception)
			{
			}
			if (a == "1")
			{
				rewardedEnabled = true;
			}
			else
			{
				rewardedEnabled = false;
			}
			if (text != string.Empty)
			{
				float.TryParse(text, out interstitialTime);
			}
			else
			{
				interstitialTime = 300f;
			}
			if (text2 != string.Empty)
			{
				int.TryParse(text2, out firstLevel);
				firstLevel--;
			}
			else
			{
				firstLevel = 13;
			}
			adTimer = interstitialTime;
			lastInterstitialTime = 99999f;
			Init();
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void Init()
	{
		MoPubManager.OnSdkInitalizedEvent += OnSdkInitializedEvent;
		MoPubManager.OnConsentStatusChangedEvent += OnConsentStatusChangedEvent;
		MoPubManager.OnConsentDialogLoadedEvent += OnConsentDialogLoadedEvent;
		MoPubManager.OnConsentDialogFailedEvent += OnConsentDialogFailedEvent;
		MoPubManager.OnConsentDialogShownEvent += OnConsentDialogShownEvent;
		MoPubManager.OnAdLoadedEvent += OnAdLoadedEvent;
		MoPubManager.OnAdFailedEvent += OnAdFailedEvent;
		MoPubManager.OnAdClickedEvent += OnAdClickedEvent;
		MoPubManager.OnAdExpandedEvent += OnAdExpandedEvent;
		MoPubManager.OnAdCollapsedEvent += OnAdCollapsedEvent;
		MoPubManager.OnInterstitialLoadedEvent += OnInterstitialLoadedEvent;
		MoPubManager.OnInterstitialFailedEvent += OnInterstitialFailedEvent;
		MoPubManager.OnInterstitialShownEvent += OnInterstitialShownEvent;
		MoPubManager.OnInterstitialClickedEvent += OnInterstitialClickedEvent;
		MoPubManager.OnInterstitialDismissedEvent += OnInterstitialDismissedEvent;
		MoPubManager.OnInterstitialExpiredEvent += OnInterstitialExpiredEvent;
		MoPubManager.OnRewardedVideoLoadedEvent += OnRewardedVideoLoadedEvent;
		MoPubManager.OnRewardedVideoFailedEvent += OnRewardedVideoFailedEvent;
		MoPubManager.OnRewardedVideoExpiredEvent += OnRewardedVideoExpiredEvent;
		MoPubManager.OnRewardedVideoShownEvent += OnRewardedVideoShownEvent;
		MoPubManager.OnRewardedVideoClickedEvent += OnRewardedVideoClickedEvent;
		MoPubManager.OnRewardedVideoFailedToPlayEvent += OnRewardedVideoFailedToPlayEvent;
		MoPubManager.OnRewardedVideoReceivedRewardEvent += OnRewardedVideoReceivedRewardEvent;
		MoPubManager.OnRewardedVideoClosedEvent += OnRewardedVideoClosedEvent;
		MoPubManager.OnRewardedVideoLeavingApplicationEvent += OnRewardedVideoLeavingApplicationEvent;
	}

	private void Start()
	{
		MoPubBase.SdkConfiguration sdkConfiguration = default(MoPubBase.SdkConfiguration);
		sdkConfiguration.AdUnitId = _interstitialAdUnits[0];
		sdkConfiguration.NetworksToInit = new MoPubBase.RewardedNetwork[9]
		{
			MoPubBase.RewardedNetwork.AdColony,
			MoPubBase.RewardedNetwork.Facebook,
			MoPubBase.RewardedNetwork.Unity,
			MoPubBase.RewardedNetwork.AdMob,
			MoPubBase.RewardedNetwork.AppLovin,
			MoPubBase.RewardedNetwork.Chartboost,
			MoPubBase.RewardedNetwork.IronSource,
			MoPubBase.RewardedNetwork.Tapjoy,
			MoPubBase.RewardedNetwork.Vungle
		};
		sdkConfiguration.AdvancedBidders = new MoPubBase.AdvancedBidder[1]
		{
			MoPubBase.AdvancedBidder.Facebook
		};
		MoPubBase.SdkConfiguration sdkConfiguration2 = sdkConfiguration;
		MoPubAndroid.InitializeSdk(sdkConfiguration2);
		MoPubAndroid.LoadBannerPluginsForAdUnits(_bannerAdUnits);
		MoPubAndroid.LoadInterstitialPluginsForAdUnits(_interstitialAdUnits);
		MoPubAndroid.LoadRewardedVideoPluginsForAdUnits(_rewardedVideoAdUnits);
	}

	private void Update()
	{
		adTimer += Time.deltaTime;
	}

	private void SdkInitialized()
	{
		MoPubAndroid.PartnerApi.GrantConsent();
		UpdateConsentValues();
		RequestBanner();
		RequestRewarded();
		RequestInterstitial();
	}

	private void OnSdkInitializedEvent(string adUnitId)
	{
		UnityEngine.Debug.Log("OnSdkInitializedEvent: " + adUnitId);
		SdkInitialized();
	}

	private void OnConsentStatusChangedEvent(MoPubBase.Consent.Status oldStatus, MoPubBase.Consent.Status newStatus, bool canCollectPersonalInfo)
	{
		UnityEngine.Debug.Log("OnConsetStatusChangedEvent: old=" + oldStatus + " new=" + newStatus + " personalInfoOk=" + canCollectPersonalInfo);
		ConsentStatusChanged(newStatus, canCollectPersonalInfo);
	}

	private void OnConsentDialogLoadedEvent()
	{
		UnityEngine.Debug.Log("OnConsentDialogLoadedEvent: dialog loaded");
	}

	private void OnConsentDialogFailedEvent(string err)
	{
		UnityEngine.Debug.Log("OnConsentDialogFailedEvent: " + err);
	}

	private void OnConsentDialogShownEvent()
	{
		UnityEngine.Debug.Log("OnConsentDialogShownEvent: dialog shown");
	}

	private void OnAdLoadedEvent(string adUnitId, float height)
	{
		UnityEngine.Debug.Log("OnAdLoadedEvent: " + adUnitId + " height: " + height);
	}

	private void OnAdFailedEvent(string adUnitId, string error)
	{
		UnityEngine.Debug.Log("OnAdFailedEvent: " + adUnitId);
		RequestBanner();
	}

	private void OnAdClickedEvent(string adUnitId)
	{
		UnityEngine.Debug.Log("OnAdClickedEvent: " + adUnitId);
	}

	private void OnAdExpandedEvent(string adUnitId)
	{
		UnityEngine.Debug.Log("OnAdExpandedEvent: " + adUnitId);
	}

	private void OnAdCollapsedEvent(string adUnitId)
	{
		UnityEngine.Debug.Log("OnAdCollapsedEvent: " + adUnitId);
		RequestBanner();
	}

	private void OnInterstitialLoadedEvent(string adUnitId)
	{
		interstitialAvailable = true;
		UnityEngine.Debug.Log("OnInterstitialLoadedEvent: " + adUnitId);
	}

	private void OnInterstitialFailedEvent(string adUnitId, string error)
	{
		interstitialAvailable = false;
		RequestInterstitial();
	}

	private void OnInterstitialShownEvent(string adUnitId)
	{
		GoogleAnalyticsScript.instance.AdjustEvent("ahltu6");
		interstitialAvailable = false;
		RequestInterstitial();
		MonoBehaviour.print("Interstitial opened");
	}

	private void OnInterstitialClickedEvent(string adUnitId)
	{
		GoogleAnalyticsScript.instance.AdjustEvent("sldrq8");
		MonoBehaviour.print("Interstitial clicked");
	}

	private void OnInterstitialDismissedEvent(string adUnitId)
	{
		invokeInterstitialClosed();
		PlayerPrefsManager.interstitialShown();
		if (!interstitialAvailable)
		{
			RequestInterstitial();
		}
		MonoBehaviour.print("Interstitial closed");
	}

	public void invokeInterstitialClosed()
	{
		if (interstitialClosed != null)
		{
			interstitialClosed();
		}
	}

	private void OnInterstitialExpiredEvent(string adUnitId)
	{
		UnityEngine.Debug.Log("OnInterstitialExpiredEvent: " + adUnitId);
		interstitialAvailable = false;
		RequestInterstitial();
	}

	private void OnRewardedVideoLoadedEvent(string adUnitId)
	{
		UnityEngine.Debug.Log("*************REWARDED VIDEO READY********** :");
	}

	private void OnRewardedVideoFailedEvent(string adUnitId, string error)
	{
		if (!isRewardedReady())
		{
			RequestRewarded();
		}
	}

	private void OnRewardedVideoExpiredEvent(string adUnitId)
	{
		if (!isRewardedReady())
		{
			RequestRewarded();
		}
		UnityEngine.Debug.Log("OnRewardedVideoExpiredEvent: " + adUnitId);
	}

	private void OnRewardedVideoShownEvent(string adUnitId)
	{
		rewardedOpened = true;
		if (!isRewardedReady())
		{
			RequestRewarded();
		}
		GoogleAnalyticsScript.instance.AdjustEvent("f6rztj");
		UnityEngine.Debug.Log("*************REWARDED VIDEO ENDED**********");
	}

	private void OnRewardedVideoClickedEvent(string adUnitId)
	{
		UnityEngine.Debug.Log("*************REWARDED VIDEO CLICKED**********");
		GoogleAnalyticsScript.instance.AdjustEvent("eoklxw");
	}

	private void OnRewardedVideoFailedToPlayEvent(string adUnitId, string error)
	{
		ShowRewarded(rewardedType);
		rewardedOpened = false;
	}

	private void OnRewardedVideoReceivedRewardEvent(string adUnitId, string label, float amount)
	{
		if (rewardedType == "25gem")
		{
			PlayerPrefsManager.SetCoin(PlayerPrefsManager.GetCoin() + 25);
		}
		if (SceneManager.GetActiveScene().name == "Menu")
		{
			MenuController.rewardedWatched = rewardedType;
		}
		if (!isRewardedReady())
		{
			RequestRewarded();
		}
		UnityEngine.Debug.Log("*************REWARDED VIDEO REWARDED**********");
	}

	private void OnRewardedVideoClosedEvent(string adUnitId)
	{
		rewardedOpened = false;
		if (!isRewardedReady())
		{
			RequestRewarded();
		}
		UnityEngine.Debug.Log("*************REWARDED VIDEO CLOSED**********");
	}

	private void OnRewardedVideoLeavingApplicationEvent(string adUnitId)
	{
		UnityEngine.Debug.Log("OnRewardedVideoLeavingApplicationEvent: " + adUnitId);
	}

	public bool isRewardedReady()
	{
		UnityEngine.Debug.Log("*******REWARDED READY: " + MoPubAndroid.HasRewardedVideo(_rewardedVideoAdUnits[0]));
		return MoPubAndroid.HasRewardedVideo(_rewardedVideoAdUnits[0]);
	}

	public void ShowRewarded(string type)
	{
		if (isRewardedReady())
		{
			UnityEngine.Debug.Log("************ SHOWING REWARDED*************");
			rewardedOpened = true;
			rewardedType = type;
			MoPubAndroid.ShowRewardedVideo(_rewardedVideoAdUnits[0]);
		}
		else
		{
			RequestRewarded();
			rewardedOpened = false;
			UnityEngine.Debug.Log("************ REWARDED NOT LOADED*************");
		}
	}

	public bool InterstitialReady()
	{
		if (PlayerPrefsManager.GetLevel() < firstLevel || adTimer < interstitialTime)
		{
			return false;
		}
		return interstitialAvailable && PlayerPrefsManager.GetNoAd() == 0;
	}

	public void ShowInterstitial()
	{
		UnityEngine.Debug.Log("SHOW ADS");
		if (PlayerPrefsManager.GetLevel() >= firstLevel && !(adTimer < interstitialTime) && PlayerPrefsManager.GetNoAd() == 0)
		{
			if (interstitialAvailable)
			{
				lastInterstitialTime = Time.realtimeSinceStartup;
				adTimer = 0f;
				MoPubAndroid.ShowInterstitialAd(_interstitialAdUnits[0]);
			}
			else
			{
				emptyInterstitialCount++;
				FirebaseAnalytics.LogEvent("Empty_Interstitial", "count", emptyInterstitialCount);
				RequestInterstitial();
			}
		}
	}

	public void RequestInterstitial()
	{
		if (PlayerPrefsManager.GetNoAd() != 1)
		{
			if (Application.internetReachability == NetworkReachability.NotReachable)
			{
				Invoke("RequestInterstitial", 1f);
			}
			else if (!interstitialAvailable)
			{
				MoPubAndroid.RequestInterstitialAd(_interstitialAdUnits[0], string.Empty, string.Empty);
			}
		}
	}

	private IEnumerator waitRequestInterstitial()
	{
		yield return new WaitForSeconds(5f);
		RequestInterstitial();
	}

	private IEnumerator waitRequestRewarded()
	{
		yield return new WaitForSeconds(5f);
		RequestRewarded();
	}

	public void RequestRewarded()
	{
		if (Application.internetReachability == NetworkReachability.NotReachable)
		{
			Invoke("RequestRewarded", 1f);
		}
		else if (!isRewardedReady())
		{
			MoPubAndroid.RequestRewardedVideo(_rewardedVideoAdUnits[0]);
		}
	}

	private void RequestBanner()
	{
		if (PlayerPrefsManager.GetNoAd() == 1)
		{
			HideBanner();
		}
		else
		{
			MoPubAndroid.CreateBanner(_bannerAdUnits[0], MoPubBase.AdPosition.BottomCenter);
		}
	}

	public void ShowBanner()
	{
		if (PlayerPrefsManager.GetNoAd() == 0)
		{
			if (isBannerAtTop)
			{
				MoPubAndroid.CreateBanner(_bannerAdUnits[0], MoPubBase.AdPosition.TopCenter);
				MoPubAndroid.ShowBanner(_bannerAdUnits[0], shouldShow: true);
			}
			else
			{
				MoPubAndroid.ShowBanner(_bannerAdUnits[0], shouldShow: true);
			}
		}
	}

	public void HideBanner()
	{
		MoPubAndroid.ShowBanner(_bannerAdUnits[0], shouldShow: false);
	}

	private void UpdateConsentValues()
	{
		_canCollectPersonalInfo = MoPubAndroid.CanCollectPersonalInfo;
		_currentConsentStatus = MoPubAndroid.CurrentConsentStatus;
		_shouldShowConsentDialog = MoPubAndroid.ShouldShowConsentDialog;
		_isGdprApplicable = MoPubAndroid.IsGdprApplicable;
	}

	public void ConsentStatusChanged(MoPubBase.Consent.Status newStatus, bool canCollectPersonalInfo)
	{
		_canCollectPersonalInfo = canCollectPersonalInfo;
		_currentConsentStatus = newStatus;
		_shouldShowConsentDialog = MoPubAndroid.ShouldShowConsentDialog;
	}
}
