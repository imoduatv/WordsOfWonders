using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HiddenMenuScript : MonoBehaviour
{
	public GameObject adsOn;

	public GameObject adsOff;

	public GameObject blurOn;

	public GameObject blurOff;

	public GameObject blueOn;

	public GameObject blueOff;

	public GameObject proOn;

	public GameObject proOff;

	public InputField gold;

	public InputField level;

	public InputField daily;

	public InputField color;

	public InputField bg;

	public InputField trackerColor;

	private void OnEnable()
	{
		SetAdStatus();
		SetBlurStatus();
		SetBlueStatus();
	}

	private void Update()
	{
	}

	private void SetAdStatus()
	{
		if (PlayerPrefsManager.GetNoAd() == 1)
		{
			adsOn.SetActive(value: false);
			adsOff.SetActive(value: true);
		}
		else
		{
			adsOn.SetActive(value: true);
			adsOff.SetActive(value: false);
		}
		if (PlayerPrefsManager.GetPro())
		{
			proOn.SetActive(value: true);
			proOff.SetActive(value: false);
		}
		else
		{
			proOn.SetActive(value: false);
			proOff.SetActive(value: true);
		}
	}

	private void SetBlurStatus()
	{
		if (PlayerPrefsManager.IsBlurOn())
		{
			blurOn.SetActive(value: true);
			blurOff.SetActive(value: false);
		}
		else
		{
			blurOn.SetActive(value: false);
			blurOff.SetActive(value: true);
		}
	}

	private void SetBlueStatus()
	{
		if (PlayerPrefsManager.IsBlueMode())
		{
			blueOn.SetActive(value: true);
			blueOff.SetActive(value: false);
		}
		else
		{
			blueOn.SetActive(value: false);
			blueOff.SetActive(value: true);
		}
	}

	public void SetNoAdMode()
	{
		if (PlayerPrefsManager.GetNoAd() == 1)
		{
			PlayerPrefsManager.SetNoAd(0);
			FugoAdManager.instance.ShowBanner();
		}
		else
		{
			PlayerPrefsManager.SetNoAd(1);
			FugoAdManager.instance.HideBanner();
		}
		SetAdStatus();
		MenuItemArranger.instance.Arrange();
	}

	public void SetProMode()
	{
		if (PlayerPrefsManager.GetPro())
		{
			PlayerPrefsManager.SetPro(0);
			FugoAdManager.instance.ShowBanner();
		}
		else
		{
			PlayerPrefsManager.SetPro(1);
			FugoAdManager.instance.HideBanner();
		}
		SetAdStatus();
	}

	public void SetBlurMode()
	{
		if (PlayerPrefsManager.IsBlurOn())
		{
			PlayerPrefsManager.SetBlur(0);
		}
		else
		{
			PlayerPrefsManager.SetBlur(1);
		}
		SetBlurStatus();
	}

	public void SetBlueMode()
	{
		if (PlayerPrefsManager.IsBlueMode())
		{
			PlayerPrefsManager.SetBlueMode(0);
		}
		else
		{
			PlayerPrefsManager.SetBlueMode(1);
		}
		SetBlueStatus();
	}

	public void DoneButtonOnClick()
	{
		if (gold.text != string.Empty)
		{
			PlayerPrefsManager.SetCoin(int.Parse(gold.text));
		}
		if (level.text != string.Empty)
		{
			int a = int.Parse(level.text);
			PlayerPrefsManager.SetLevel(Mathf.Min(a, PlayerPrefsManager.CountLevels() + 1));
		}
		if (daily.text != string.Empty)
		{
			PlayerPrefsManager.SetDailyProcess(int.Parse(daily.text));
		}
		if (bg.text != string.Empty)
		{
			PlayerPrefsManager.SetConstantBG(bg.text);
		}
		if (color.text != string.Empty)
		{
			PlayerPrefsManager.SetInGameLetterColor(color.text);
		}
		if (trackerColor.text != string.Empty)
		{
			PlayerPrefsManager.SetTrackerColor(trackerColor.text);
		}
		SceneManager.LoadScene("Menu");
	}

	public void ResetAllButton()
	{
		string fBID = PlayerPrefsManager.GetFBID();
		PlayerPrefs.DeleteAll();
		PlayerPrefsManager.SetFBID(fBID);
		if (level.text != string.Empty)
		{
			int a = int.Parse(level.text);
			PlayerPrefsManager.SetLevel(Mathf.Min(a, PlayerPrefsManager.CountLevels() + 1));
		}
		SceneManager.LoadScene("Menu");
	}

	public void ResetDailyGift()
	{
		PlayerPrefsManager.ResetContinueDaily();
		PlayerPrefsManager.SetDaily(-1);
		PlayerPrefsManager.SetDailyPuzzleDay(-1);
		SceneManager.LoadScene("Menu");
	}

	public void LangButtonClick(Transform t)
	{
		if (t.name.ToLower().Contains("en"))
		{
			PlayerPrefsManager.SetLang("English");
		}
		else if (t.name.ToLower().Contains("tr"))
		{
			PlayerPrefsManager.SetLang("Turkish");
		}
		Games.sections = null;
		SceneManager.LoadScene("Menu");
	}

	public void LevelEditorButtonOnClick()
	{
		SceneManager.LoadScene("LevelEditor");
	}

	public void Quest()
	{
		MenuController.instance.CloseHiddenMenu(0.001f);
		QuestController.instance.GiveQuest();
	}
}
