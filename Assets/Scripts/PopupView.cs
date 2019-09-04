using UnityEngine;

public class PopupView : MonoBehaviour
{
	private void OnEnable()
	{
		AndroidRateUsPopUp.onRateUSPopupComplete += OnRateUSPopupComplete;
	}

	private void OnDisable()
	{
		AndroidRateUsPopUp.onRateUSPopupComplete -= OnRateUSPopupComplete;
	}

	private void OnRateUSPopupComplete(MessageState state)
	{
		switch (state)
		{
		case MessageState.RATED:
			PlayerPrefsManager.SetRateUs(-1);
			break;
		case MessageState.DECLINED:
			PlayerPrefsManager.SetRateUs(-1);
			break;
		}
	}

	public void OpenRatePopup()
	{
		string appLink = "market://details?id=" + Application.identifier;
		NativeRateUS nativeRateUS = new NativeRateUS(LanguageScript.RateUsHeaderText, LanguageScript.RateUsContextText);
		nativeRateUS.SetAppLink(appLink);
		nativeRateUS.InitRateUS();
	}
}
