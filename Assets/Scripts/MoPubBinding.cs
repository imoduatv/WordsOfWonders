using MoPubInternal.ThirdParty.MiniJSON;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

public class MoPubBinding
{
	public MoPubBase.Reward SelectedReward;

	private readonly string _adUnitId;

	public bool IsInterstitialReady => _moPubIsInterstitialReady(_adUnitId);

	public MoPubBinding(string adUnitId)
	{
		_adUnitId = adUnitId;
		SelectedReward = new MoPubBase.Reward
		{
			Label = string.Empty
		};
	}

	public void CreateBanner(MoPubBase.BannerType bannerType, MoPubBase.AdPosition position)
	{
		_moPubCreateBanner((int)bannerType, (int)position, _adUnitId);
	}

	public void DestroyBanner()
	{
		_moPubDestroyBanner(_adUnitId);
	}

	public void ShowBanner(bool shouldShow)
	{
		_moPubShowBanner(_adUnitId, shouldShow);
	}

	public void RefreshBanner(string keywords = "", string userDataKeywords = "")
	{
		_moPubRefreshBanner(_adUnitId, keywords, userDataKeywords);
	}

	public void SetAutorefresh(bool enabled)
	{
		_moPubSetAutorefreshEnabled(_adUnitId, enabled);
	}

	public void ForceRefresh()
	{
		_moPubForceRefresh(_adUnitId);
	}

	public void RequestInterstitialAd(string keywords = "", string userDataKeywords = "")
	{
		_moPubRequestInterstitialAd(_adUnitId, keywords, userDataKeywords);
	}

	public void ShowInterstitialAd()
	{
		_moPubShowInterstitialAd(_adUnitId);
	}

	public void DestroyInterstitialAd()
	{
		_moPubDestroyInterstitialAd(_adUnitId);
	}

	public void RequestRewardedVideo(List<MoPubBase.MediationSetting> mediationSettings = null, string keywords = null, string userDataKeywords = null, double latitude = 99999.0, double longitude = 99999.0, string customerId = null)
	{
		string json = (mediationSettings == null) ? null : Json.Serialize(mediationSettings);
		_moPubRequestRewardedVideo(_adUnitId, json, keywords, userDataKeywords, latitude, longitude, customerId);
	}

	public bool HasRewardedVideo()
	{
		return _mopubHasRewardedVideo(_adUnitId);
	}

	public List<MoPubBase.Reward> GetAvailableRewards()
	{
		int amount = 0;
		string text = _mopubGetAvailableRewards(_adUnitId) ?? string.Empty;
		IEnumerable<MoPubBase.Reward> source = (from rewardString in text.Split(',')
			select rewardString.Split(':') into rewardComponents
			where rewardComponents.Length == 2
			where int.TryParse(rewardComponents[1], out amount)
			select rewardComponents).Select(delegate(string[] rewardComponents)
		{
			MoPubBase.Reward result = default(MoPubBase.Reward);
			result.Label = rewardComponents[0];
			result.Amount = amount;
			return result;
		});
		return source.ToList();
	}

	public void ShowRewardedVideo(string customData)
	{
		_moPubShowRewardedVideo(_adUnitId, SelectedReward.Label, SelectedReward.Amount, customData);
	}

	[DllImport("__Internal")]
	private static extern void _moPubCreateBanner(int bannerType, int position, string adUnitId);

	[DllImport("__Internal")]
	private static extern void _moPubDestroyBanner(string adUnitId);

	[DllImport("__Internal")]
	private static extern void _moPubShowBanner(string adUnitId, bool shouldShow);

	[DllImport("__Internal")]
	private static extern void _moPubRefreshBanner(string adUnitId, string keywords, string userDataKeywords);

	[DllImport("__Internal")]
	private static extern void _moPubSetAutorefreshEnabled(string adUnitId, bool enabled);

	[DllImport("__Internal")]
	private static extern void _moPubForceRefresh(string adUnitId);

	[DllImport("__Internal")]
	private static extern void _moPubRequestInterstitialAd(string adUnitId, string keywords, string userDataKeywords);

	[DllImport("__Internal")]
	private static extern bool _moPubIsInterstitialReady(string adUnitId);

	[DllImport("__Internal")]
	private static extern void _moPubShowInterstitialAd(string adUnitId);

	[DllImport("__Internal")]
	private static extern void _moPubDestroyInterstitialAd(string adUnitId);

	[DllImport("__Internal")]
	private static extern void _moPubRequestRewardedVideo(string adUnitId, string json, string keywords, string userDataKeywords, double latitude, double longitude, string customerId);

	[DllImport("__Internal")]
	private static extern bool _mopubHasRewardedVideo(string adUnitId);

	[DllImport("__Internal")]
	private static extern string _mopubGetAvailableRewards(string adUnitId);

	[DllImport("__Internal")]
	private static extern void _moPubShowRewardedVideo(string adUnitId, string currencyName, int currencyAmount, string customData);
}
