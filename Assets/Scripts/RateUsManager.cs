using UnityEngine;

public class RateUsManager : MonoBehaviour
{
	public static RateUsManager instance;

	private void Start()
	{
		instance = this;
	}

	private void Update()
	{
	}

	public void RateUsPopup()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			if (PlayerPrefsManager.GetRateUs() != -1)
			{
				GetComponent<PopupView>().OpenRatePopup();
			}
		}
		else
		{
			iOSReviewRequest.Request();
		}
	}
}
