using System;
using UnityEngine;

public class AndroidRateUsPopUp : MonoBehaviour
{
	public delegate void OnRateUSPopupComplete(MessageState state);

	public string title;

	public string message;

	public string rate;

	public string remind;

	public string declined;

	public string appLink;

	public static event OnRateUSPopupComplete onRateUSPopupComplete;

	private void RaiseOnOnRateUSPopupComplete(MessageState state)
	{
		if (AndroidRateUsPopUp.onRateUSPopupComplete != null)
		{
			AndroidRateUsPopUp.onRateUSPopupComplete(state);
		}
	}

	public static AndroidRateUsPopUp Create()
	{
		return Create("Like the Game?", "Rate US");
	}

	public static AndroidRateUsPopUp Create(string title, string message)
	{
		return Create(title, message, "Rate Now", "Ask me later", "No, thanks");
	}

	public static AndroidRateUsPopUp Create(string title, string message, string rate, string remind, string declined)
	{
		AndroidRateUsPopUp androidRateUsPopUp = new GameObject("AndroidRateUsPopUp").AddComponent<AndroidRateUsPopUp>();
		androidRateUsPopUp.title = title;
		androidRateUsPopUp.message = message;
		androidRateUsPopUp.rate = rate;
		androidRateUsPopUp.remind = remind;
		androidRateUsPopUp.declined = declined;
		androidRateUsPopUp.init();
		return androidRateUsPopUp;
	}

	public void init()
	{
		AndroidNative.showRateUsPopUP(title, message, rate, remind, declined);
	}

	public void OnRatePopUpCallBack(string buttonIndex)
	{
		switch (Convert.ToInt16(buttonIndex))
		{
		case 0:
			AndroidNative.RedirectToAppStoreRatingPage(appLink);
			RaiseOnOnRateUSPopupComplete(MessageState.RATED);
			break;
		case 1:
			RaiseOnOnRateUSPopupComplete(MessageState.REMIND);
			break;
		case 2:
			RaiseOnOnRateUSPopupComplete(MessageState.DECLINED);
			break;
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
