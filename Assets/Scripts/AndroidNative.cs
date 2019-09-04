using System;
using UnityEngine;

public class AndroidNative
{
	public static void CallStatic(string methodName, params object[] args)
	{
		try
		{
			string className = "com.tag.nativepopup.PopupManager";
			AndroidJavaObject bridge = new AndroidJavaObject(className);
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
			@static.Call("runOnUiThread", (AndroidJavaRunnable)delegate
			{
				bridge.CallStatic(methodName, args);
			});
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogWarning(ex.Message);
		}
	}

	public static void showRateUsPopUP(string title, string message, string rate, string remind, string declined)
	{
		CallStatic("ShowRatePopup", title, message, rate, remind, declined);
	}

	public static void showDialog(string title, string message, string yes, string no)
	{
		CallStatic("ShowDialogPopup", title, message, yes, no);
	}

	public static void showMessage(string title, string message, string ok)
	{
		CallStatic("ShowMessagePopup", title, message, ok);
	}

	public static void RedirectToAppStoreRatingPage(string appLink)
	{
		CallStatic("OpenAppRatingPage", appLink);
	}

	public static void RedirectToWebPage(string urlString)
	{
		CallStatic("OpenWebPage", urlString);
	}
}
