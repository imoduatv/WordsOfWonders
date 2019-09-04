using UnityEngine;

public class MoPubAndroidInterstitial
{
	private readonly AndroidJavaObject _interstitialPlugin;

	public bool IsInterstitialReady => _interstitialPlugin.Call<bool>("isReady", new object[0]);

	public MoPubAndroidInterstitial(string adUnitId)
	{
		_interstitialPlugin = new AndroidJavaObject("com.mopub.unity.MoPubInterstitialUnityPlugin", adUnitId);
	}

	public void RequestInterstitialAd(string keywords = "", string userDataKeywords = "")
	{
		_interstitialPlugin.Call("request", keywords, userDataKeywords);
	}

	public void ShowInterstitialAd()
	{
		_interstitialPlugin.Call("show");
	}

	public void DestroyInterstitialAd()
	{
		_interstitialPlugin.Call("destroy");
	}
}
