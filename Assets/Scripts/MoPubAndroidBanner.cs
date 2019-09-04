using UnityEngine;

public class MoPubAndroidBanner
{
	private readonly AndroidJavaObject _bannerPlugin;

	public MoPubAndroidBanner(string adUnitId)
	{
		_bannerPlugin = new AndroidJavaObject("com.mopub.unity.MoPubBannerUnityPlugin", adUnitId);
	}

	public void CreateBanner(MoPubBase.AdPosition position)
	{
		_bannerPlugin.Call("createBanner", (int)position);
	}

	public void ShowBanner(bool shouldShow)
	{
		_bannerPlugin.Call("hideBanner", !shouldShow);
	}

	public void RefreshBanner(string keywords, string userDataKeywords = "")
	{
		_bannerPlugin.Call("refreshBanner", keywords, userDataKeywords);
	}

	public void DestroyBanner()
	{
		_bannerPlugin.Call("destroyBanner");
	}

	public void SetAutorefresh(bool enabled)
	{
		_bannerPlugin.Call("setAutorefreshEnabled", enabled);
	}

	public void ForceRefresh()
	{
		_bannerPlugin.Call("forceRefresh");
	}
}
