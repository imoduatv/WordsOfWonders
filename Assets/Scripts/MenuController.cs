using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using v2Gameplay;

public class MenuController : MonoBehaviour
{
	public static MenuController instance;

	public static bool fromGame;

	public static bool fromGameToSections;

	public static bool fromDaily;

	public static string rewardedWatched = string.Empty;

	public Transform settingsPopup;

	public Transform soundButton;

	public Transform musicButton;

	public Transform dailyGiftPopup;

	public Transform shopPopup;

	public Transform rewardedPopup;

	public Transform hiddenMenu;

	public Transform languagePopup;

	public Transform setTutorialPopup;

	public Transform questPopup;

	public Transform proPopup;

	public Transform redeemPopup;

	public Transform hiddenLevelPopup;

	public Transform wheelPopup;

	public Transform trialProPopup;

	public Transform sun;

	public Transform startButton;

	public Transform dailyButton;

	public Transform shopButton;

	public Transform levelsButton;

	public Transform animBG;

	public Transform settingsButton;

	public Transform facebookButton;

	public Transform logo;

	public Transform bgRotator;

	public Transform questButton;

	public GameObject sectionPanel;

	public GameObject googleAnalyticsPrefab;

	public GameObject admanagerPrefab;

	public GameObject dailypuzzlestarholder;

	public GameObject reqManager;

	public GameObject hiddenMenuButton;

	public GameObject proBadge;

	public GameObject exploreHeader;

	public Image fadePanel;

	public Sprite[] sliderImages;

	public Text coinText;

	public Text wheelCoinText;

	public Text versionText;

	public Text langText;

	public Text errorText;

	public Text errorText2;

	public Material grayscale;

	public Animator supportPopupAnimation;

	private bool shouldSetTexts = true;

	private void Awake()
	{
		try
		{
			instance = this;
			errorText2.text = "Error: 100";
			SetIfArabic();
			errorText2.text = "Error: 101";
			LanguageSelectScript.ParseLangData();
			errorText2.text = "Error: 102";
			if (PlayerPrefsManager.GetHiddenMenu())
			{
				hiddenMenuButton.SetActive(value: true);
			}
			else
			{
				hiddenMenuButton.SetActive(value: false);
			}
			errorText2.text = "Error: 103";
			errorText2.text = "Error: 104";
			errorText2.text = "Error: 105";
			if (GoogleAnalyticsScript.instance == null)
			{
				if (PlayerPrefsManager.GetGARandom() == -1)
				{
					PlayerPrefsManager.SetGARandom(UnityEngine.Random.Range(0, 10000));
				}
				UnityEngine.Object.Instantiate(googleAnalyticsPrefab).name = "GoogleAnalytics";
			}
			errorText2.text = "Error: 106";
			if (RequestManager.instance == null)
			{
				UnityEngine.Object.Instantiate(reqManager).name = "RequestManager";
				RequestManager.instance.GetLogRequest();
			}
			errorText2.text = "Error: 107";
			errorText2.text = "Error: 108";
			errorText2.text = "Error: 109";
			if (PlayerPrefsManager.GetLang() == string.Empty)
			{
				OpenLanguagePopup(flag: false);
				shouldSetTexts = false;
			}
			else
			{
				LanguageScript.ParseStrings();
				errorText2.text = "Error: 110";
				if (Games.sections == null)
				{
					Games.ParseGameData(string.Empty);
				}
				errorText2.text = "Error: 111";
				errorText2.text = "Error: 112";
				startButton.transform.localScale = Vector3.zero;
				questButton.transform.localScale = Vector3.zero;
				levelsButton.transform.localScale = Vector3.zero;
				dailyButton.transform.localScale = Vector3.zero;
				errorText2.text = "Error: 113";
				SettingsCloseButtonClicked(0.001f);
				errorText2.text = "Error: 114";
				CloseDailyGiftPopup(0.001f);
				errorText2.text = "Error: 115";
				ShopCloseButtonClicked(0.001f);
				errorText2.text = "Error: 116";
				CloseRewardedPopup(0.001f);
				errorText2.text = "Error: 117";
				CloseHiddenMenu(0.001f);
				errorText2.text = "Error: 118";
				CloseSetTutorialPopup(0.001f);
				CloseQuestPopup(0.001f);
				CloseHiddenLevelPopup(0.001f);
				CloseWheelPopup(0.001f);
				errorText2.text = "Error: 119";
				QualitySettings.vSyncCount = 0;
				Application.targetFrameRate = 60;
				errorText2.text = "Error: 120";
				if (fromGameToSections)
				{
					sectionPanel.SetActive(value: true);
				}
				errorText2.text = "Error: 121";
				SetVersionText();
				Input.multiTouchEnabled = false;
				errorText2.text = "Error: 122";
				Screen.sleepTimeout = -1;
				errorText2.text = "Error: 123";
				if (SectionController.setCompleted || SectionController.hiddenSetCompleted != -1)
				{
					sectionPanel.SetActive(value: true);
				}
				errorText2.text = "Error: 124";
				errorText2.text = "Error: 125";
				if (PlayerPrefsManager.IsFirstRun() == 0)
				{
					PlayerPrefsManager.SetFirstRun();
					GameController.levelToOpen = -1;
					StartCoroutine(GoToGameScreenDelayed());
				}
				errorText2.text = "Error: 126";
				if (FugoAdManager.instance == null)
				{
					UnityEngine.Object.Instantiate(admanagerPrefab).name = "AdManager";
					errorText.text = "Error: 127";
					FugoAdManager.instance.coinHolder = animBG;
					errorText.text = "Error: 128";
					NotificationSystem.CreateDailyNotification();
					errorText.text = "Error: 129";
				}
			}
		}
		catch (Exception exception)
		{
			errorText2.gameObject.SetActive(value: true);
			UnityEngine.Debug.LogException(exception);
		}
	}

	private void Start()
	{
		try
		{
			if (PlayerPrefsManager.GetFBID() != string.Empty)
			{
				facebookButton.parent.gameObject.SetActive(value: false);
			}
			string empty = string.Empty;
			empty = FirebaseController.FetchRemoteValue("menu_theme");
			if (empty == "1")
			{
				ThemeManager.instance.ChangeTheme(1);
			}
			else
			{
				ThemeManager.instance.ChangeTheme(0);
			}
			if (fromDaily)
			{
				DailyPuzzleButtonClicked();
				AfterGameFadeIn(GameController.set);
			}
			errorText.text = "Error: 200";
			SoundManager.instance.PlayMusic(1f);
			errorText.text = "Error: 205";
			SetTexts();
			errorText.text = "Error: 206";
			if (PlayerPrefsManager.GetLang() != string.Empty && shouldSetTexts)
			{
				TextController.instance.SetTexts(string.Empty);
			}
			errorText.text = "Error: 207";
			errorText.text = "Error: 208";
			SetSliderImages(soundButton, PlayerPrefsManager.GetSoundEffects());
			errorText.text = "Error: 209";
			SetSliderImages(musicButton, PlayerPrefsManager.GetMusic());
			errorText.text = "Error: 210";
			if (!SectionController.setCompleted && !fromGameToSections && !fromDaily)
			{
				errorText.text = "Error: 211";
				AnimateMenu();
				errorText.text = "Error: 212";
			}
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogException(exception);
			errorText.gameObject.SetActive(value: true);
		}
	}

	private void Update()
	{
		if (UnityEngine.Input.GetKeyDown(KeyCode.Escape) && Application.platform == RuntimePlatform.Android)
		{
			BackButtonPressed();
		}
		if (rewardedWatched != string.Empty)
		{
			if (rewardedWatched == "25gem")
			{
				FortuneWheel.instance.CreateAnimCoins(GameObject.Find("Canvas/PopupHolder").transform);
				IncreaseCoinAnim(0.5f);
				CloseRewardedPopup(0.3f);
			}
			else if (rewardedWatched == "wheel")
			{
				FortuneWheel.instance.Spin();
			}
			rewardedWatched = string.Empty;
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
		{
			QuestController.instance.GiveQuest();
		}
	}

	public void OpenDailyGiftPopup()
	{
		GetComponent<DailyGiftSystem>().CreateDailyGifts();
		dailyGiftPopup.gameObject.SetActive(value: true);
		fadePanel.gameObject.SetActive(value: true);
		StartCoroutine(FugoUtils.FadeImage(0.6f, 0.35f, fadePanel));
		dailyGiftPopup.GetComponent<Animator>().enabled = true;
		dailyGiftPopup.GetComponent<Animator>().Play("PopupOpen");
		SoundManager.instance.SlideIn();
	}

	public void CloseDailyGiftPopup(float time)
	{
		StartCoroutine(PopupCloseThread(time, dailyGiftPopup));
	}

	public void OpenRewardedPopup()
	{
		if (FugoAdManager.instance.isRewardedReady())
		{
			StartCoroutine(RewardedPopupThread());
		}
	}

	private IEnumerator RewardedPopupThread()
	{
		yield return new WaitForSeconds(0.3f);
		rewardedPopup.gameObject.SetActive(value: true);
		fadePanel.gameObject.SetActive(value: true);
		StartCoroutine(FugoUtils.FadeImage(0.6f, 0.35f, fadePanel));
		rewardedPopup.GetComponent<Animator>().enabled = true;
		rewardedPopup.GetComponent<Animator>().Play("PopupOpen");
	}

	public void CloseRewardedPopup(float time)
	{
		if (time > 0.01f)
		{
			SoundManager.instance.Click();
		}
		StartCoroutine(PopupCloseThread(time, rewardedPopup));
	}

	public void RewardedButtonOnClick()
	{
		FugoAdManager.instance.ShowRewarded("25gem");
	}

	public void OpenSetTutorialPopup()
	{
		setTutorialPopup.gameObject.SetActive(value: true);
		fadePanel.gameObject.SetActive(value: true);
		StartCoroutine(FugoUtils.FadeImage(0.6f, 0.35f, fadePanel));
		setTutorialPopup.GetComponent<Animator>().enabled = true;
		setTutorialPopup.GetComponent<Animator>().Play("PopupOpen");
		SoundManager.instance.SlideIn();
	}

	public void CloseSetTutorialPopup(float time)
	{
		SectionController.isTutorial = false;
		if (time > 0.01f)
		{
			SoundManager.instance.Click();
		}
		StartCoroutine(PopupCloseThread(time, setTutorialPopup));
	}

	public void OpenRedeemPopup()
	{
		StartCoroutine(RedeemPopupThread());
	}

	private IEnumerator RedeemPopupThread()
	{
		SettingsCloseButtonClicked(0.2f, fadePanelInteraction: false);
		yield return new WaitForSeconds(0.2f);
		redeemPopup.gameObject.SetActive(value: true);
		redeemPopup.GetComponent<Animator>().enabled = true;
		redeemPopup.GetComponent<Animator>().Play("PopupOpen");
	}

	public void CloseRedeemPopup()
	{
		SoundManager.instance.Click();
		StartCoroutine(PopupCloseThread(0.3f, redeemPopup));
	}

	public void OpenQuestPopup()
	{
		questPopup.gameObject.SetActive(value: true);
		fadePanel.gameObject.SetActive(value: true);
		StartCoroutine(FugoUtils.FadeImage(0.6f, 0.35f, fadePanel));
		questPopup.GetComponent<Animator>().enabled = true;
		questPopup.GetComponent<Animator>().Play("PopupOpen");
		SoundManager.instance.SlideIn();
	}

	public void CloseQuestPopup(float time)
	{
		if (time > 0.01f)
		{
			SoundManager.instance.Click();
		}
		StartCoroutine(PopupCloseThread(time, questPopup));
	}

	public void OpenWheelPopup()
	{
		wheelPopup.gameObject.SetActive(value: true);
		fadePanel.gameObject.SetActive(value: true);
		StartCoroutine(FugoUtils.FadeImage(0.8f, 0.35f, fadePanel));
		wheelPopup.GetComponent<Animator>().enabled = true;
		wheelPopup.GetComponent<Animator>().Play("PopupOpen");
		SoundManager.instance.SlideIn();
	}

	public void CloseWheelPopup(float time)
	{
		if (time > 0.01f)
		{
			SoundManager.instance.Click();
		}
		StartCoroutine(PopupCloseThread(time, wheelPopup));
	}

	public void OpenTrialProPopup()
	{
		trialProPopup.gameObject.SetActive(value: true);
		fadePanel.gameObject.SetActive(value: true);
		StartCoroutine(FugoUtils.FadeImage(0.8f, 0.35f, fadePanel));
		trialProPopup.GetComponent<Animator>().enabled = true;
		trialProPopup.GetComponent<Animator>().Play("PopupOpen");
		SoundManager.instance.SlideIn();
	}

	public void CloseTrialProPopup(float time)
	{
		if (time > 0.01f)
		{
			SoundManager.instance.Click();
		}
		StartCoroutine(PopupCloseThread(time, trialProPopup, fadePanelInteraction: false));
	}

	public void OpenHiddenLevelPopup(Transform t)
	{
		hiddenLevelPopup.gameObject.SetActive(value: true);
		hiddenLevelPopup.Find("OKButton/Text").GetComponent<Text>().text = t.GetComponent<SetScript>().set.Price.ToString();
		fadePanel.gameObject.SetActive(value: true);
		StartCoroutine(FugoUtils.FadeImage(0.6f, 0.35f, fadePanel));
		hiddenLevelPopup.GetComponent<Animator>().enabled = true;
		hiddenLevelPopup.GetComponent<Animator>().Play("PopupOpen");
		SoundManager.instance.SlideIn();
	}

	public void CloseHiddenLevelPopup(float time)
	{
		if (time > 0.01f)
		{
			SoundManager.instance.Click();
		}
		StartCoroutine(PopupCloseThread(time, hiddenLevelPopup));
	}

	public void OpenProPopup()
	{
		StartCoroutine(ProPopupThread());
	}

	private IEnumerator ProPopupThread()
	{
		ShopCloseButtonClicked(0.3f);
		yield return new WaitForSeconds(0.4f);
		proPopup.gameObject.SetActive(value: true);
		fadePanel.gameObject.SetActive(value: true);
		StartCoroutine(FugoUtils.FadeImage(0.6f, 0.35f, fadePanel));
		proPopup.GetComponent<Animator>().enabled = true;
		proPopup.GetComponent<Animator>().Play("PopupOpen");
		SoundManager.instance.SlideIn();
	}

	public void CloseProPopup(float time, bool withSound = true)
	{
		if (time > 0.01f && withSound)
		{
			SoundManager.instance.Click();
		}
		StartCoroutine(PopupCloseThread(time, proPopup));
	}

	public void CloseProPopupButton()
	{
		StartCoroutine(PopupCloseThread(0.3f, proPopup));
	}

	public void OpenLanguagePopup(bool flag = true)
	{
		GetComponent<LanguageSelectScript>().Init();
		if (flag)
		{
			SoundManager.instance.Click();
			SoundManager.instance.SlideIn();
		}
		StartCoroutine(LangPopupThread());
		try
		{
			TextController.instance.FixLangsTextSizes();
		}
		catch (Exception)
		{
		}
	}

	private IEnumerator LangPopupThread()
	{
		if (settingsPopup.gameObject.activeSelf)
		{
			SettingsCloseButtonClicked(0.3f);
			yield return new WaitForSeconds(0.3f);
		}
		languagePopup.gameObject.SetActive(value: true);
		fadePanel.gameObject.SetActive(value: true);
		StartCoroutine(FugoUtils.FadeImage(0.6f, 0.35f, fadePanel));
		languagePopup.GetComponent<Animator>().enabled = true;
		languagePopup.GetComponent<Animator>().Play("PopupOpen");
	}

	public void SettingsButtonClicked()
	{
		if (!SectionController.isTutorial)
		{
			SoundManager.instance.Click();
			settingsPopup.gameObject.SetActive(value: true);
			fadePanel.gameObject.SetActive(value: true);
			StartCoroutine(FugoUtils.FadeImage(0.6f, 0.35f, fadePanel));
			settingsPopup.GetComponent<Animator>().enabled = true;
			settingsPopup.GetComponent<Animator>().Play("PopupOpen");
			SoundManager.instance.SlideIn();
			supportPopupAnimation.GetComponent<Animator>().Play("MenuHideAnimation2");
			TextController.instance.FixSettingsTextSizes();
		}
	}

	public void SettingsBackButtonClicked()
	{
		supportPopupAnimation.GetComponent<Animator>().Play("MenuShowReverseAnimation2");
	}

	public void SettingsCloseButtonClicked(float time, bool fadePanelInteraction = true)
	{
		if (time > 0.01f)
		{
			SoundManager.instance.Click();
		}
		StartCoroutine(PopupCloseThread(time, settingsPopup, fadePanelInteraction));
	}

	public void SettingsCloseButtonClicked(float time)
	{
		if (time > 0.01f)
		{
			SoundManager.instance.Click();
		}
		StartCoroutine(PopupCloseThread(time, settingsPopup));
	}

	public void SoundButtonClicked()
	{
		if (PlayerPrefsManager.GetSoundEffects() == 1)
		{
			PlayerPrefsManager.SetSoundEffects(0);
		}
		else
		{
			PlayerPrefsManager.SetSoundEffects(1);
		}
		SetSliderImages(soundButton, PlayerPrefsManager.GetSoundEffects());
		SoundManager.instance.ArrangeMuted();
		SoundManager.instance.Click();
	}

	public void MusicButtonClicked()
	{
		if (PlayerPrefsManager.GetMusic() == 1)
		{
			PlayerPrefsManager.SetMusic(0);
		}
		else
		{
			PlayerPrefsManager.SetMusic(1);
		}
		SetSliderImages(musicButton, PlayerPrefsManager.GetMusic());
		SoundManager.instance.ArrangeMusic();
		SoundManager.instance.Click();
	}

	private void SetSliderImages(Transform slider, int val)
	{
		if (val == 0)
		{
			slider.Find("Frame").GetComponent<Image>().sprite = sliderImages[2];
			slider.Find("Image").GetComponent<Image>().sprite = sliderImages[0];
			slider.Find("OffText").gameObject.SetActive(value: true);
			slider.Find("OnText").gameObject.SetActive(value: false);
		}
		else
		{
			slider.Find("Frame").GetComponent<Image>().sprite = sliderImages[3];
			slider.Find("Image").GetComponent<Image>().sprite = sliderImages[1];
			slider.Find("OffText").gameObject.SetActive(value: false);
			slider.Find("OnText").gameObject.SetActive(value: true);
		}
	}

	public void SupportButtonOnClick()
	{
		SoundManager.instance.Click();
		string text = "wow@fugo.com.tr";
		string text2 = FugoUtils.MyEscapeURL("Words Of Wonders Support Mail");
		string str = "\n\n\n\n";
		str = str + SystemInfo.deviceModel + "\n";
		str = str + SystemInfo.operatingSystem + "\n";
		str = str + Application.systemLanguage + "\n";
		str = str + "v" + Application.version + "\n";
		string text3 = str;
		str = text3 + "L: " + PlayerPrefsManager.GetLevel() + "\n";
		str = str + "I: " + SystemInfo.deviceUniqueIdentifier + "\n";
		if (PlayerPrefsManager.GetFBID() != string.Empty)
		{
			str = str + "FB: " + PlayerPrefsManager.GetFBID() + "\n";
		}
		str = FugoUtils.MyEscapeURL(str);
		Application.OpenURL("mailto:" + text + "?subject=" + text2 + "&body=" + str);
	}

	public void RestorePurchaseButtonOnClick()
	{
		SoundManager.instance.Click();
		GameObject.Find("Purchaser").GetComponent<Purchaser>().RestorePurchases();
	}

	public void PrivacyButtonClicked()
	{
		SoundManager.instance.Click();
		Application.OpenURL("http://fugo.com.tr/privacypolicy-wordsofwonders.html");
	}

	public void PlayButtonClicked()
	{
		if (PlayerPrefsManager.GetLastPlayedMode() == GameMode.NORMAL)
		{
			if (PlayerPrefsManager.GetLevel() <= PlayerPrefsManager.CountLevels())
			{
				GameController.daily = false;
				GameController.mode = GameMode.NORMAL;
				StartCoroutine(PlayButtonThread());
			}
			return;
		}
		PlayerPrefsManager.SetLastPlayedMode(GameMode.ADVENTURE);
		GameController.daily = false;
		SoundManager.instance.Click();
		GameController.mode = GameMode.ADVENTURE;
		GameController.adventureSectionId = PlayerPrefsManager.GetLastHiddenSection();
		GameController.adventureSetId = PlayerPrefsManager.GetLastHiddenSet();
		GameController.adventureGameId = PlayerPrefsManager.GetHiddenGameID();
		GameController.slidePos = PlayerPrefsManager.GetHiddenPos();
		GameController.adventureCount = PlayerPrefsManager.GetHiddenGameCount();
		instance.GoToGameScreen();
	}

	public void modeButtonClicked()
	{
		GameController.daily = false;
		GameController.mode = GameMode.ADVENTURE;
		FugoUtils.openScene("GameNew");
	}

	private IEnumerator PlayButtonThread()
	{
		SoundManager.instance.Click();
		int[] levelIndex = new int[3];
		try
		{
			levelIndex = FugoUtils.getLevelInfo();
		}
		catch (Exception)
		{
		}
		GameController.levelToOpen = -1;
		GameController.daily = false;
		try
		{
			BeforeGameFadeOut(Games.sections[levelIndex[0]].sets[levelIndex[1]]);
		}
		catch (Exception)
		{
		}
		yield return new WaitForSeconds(0.2f);
		SceneManager.LoadScene("GameNew");
	}

	public void LevelsButtonClicked()
	{
		SoundManager.instance.Click();
		GetComponent<SectionController>().OpeningAnimation(0.001f);
		sectionPanel.SetActive(value: true);
	}

	public void DailyPuzzleButtonClicked()
	{
		PlayerPrefsManager.SetDailyPuzzleNewLabel(-1);
		if (!fromDaily)
		{
			SoundManager.instance.Click();
		}
		if (PlayerPrefsManager.GetEarnedStar() != -1)
		{
			ScaleDownMenuItems();
			GetComponent<DailyPuzzleController>().OpeningAnim();
		}
		else
		{
			GameController.daily = true;
			SceneManager.LoadScene("GameNew");
		}
	}

	public void ShopButtonClicked()
	{
		SoundManager.instance.Click();
		StartCoroutine(ShopOpeningThread());
	}

	private IEnumerator ShopOpeningThread()
	{
		SoundManager.instance.SlideIn();
		if (settingsPopup.gameObject.activeSelf)
		{
			SettingsCloseButtonClicked(0.3f);
			yield return new WaitForSeconds(0.3f);
		}
		shopPopup.gameObject.SetActive(value: true);
		fadePanel.gameObject.SetActive(value: true);
		StartCoroutine(FugoUtils.FadeImage(0.6f, 0.35f, fadePanel));
		shopPopup.GetComponent<Animator>().enabled = true;
		shopPopup.GetComponent<Animator>().Play("PopupOpen");
	}

	public void ShopCloseButtonClicked(float time)
	{
		StartCoroutine(PopupCloseThread(time, shopPopup));
		if (time > 0.01f)
		{
			SoundManager.instance.Click();
			if (proPopup.gameObject.activeSelf)
			{
				OpenRewardedPopup();
			}
		}
	}

	public void AnimateMenu()
	{
		StartCoroutine(MenuThread());
	}

	private IEnumerator MenuThread()
	{
		proBadge.transform.localScale = Vector3.zero;
		startButton.transform.localScale = Vector3.zero;
		dailyButton.transform.localScale = Vector3.zero;
		levelsButton.transform.localScale = Vector3.zero;
		shopButton.parent.transform.localScale = Vector3.zero;
		questButton.localScale = Vector3.zero;
		sun.localScale = Vector3.zero;
		sun.gameObject.SetActive(value: true);
		QuestController.instance.SetQuestButton();
		if (fromGame && GameController.set != null)
		{
			AfterGameFadeIn(GameController.set);
			yield return new WaitForSeconds(0.4f);
			fromGame = false;
		}
		if (!SplashScript.splashShown)
		{
			yield return new WaitForSeconds(SplashScript.splashTime);
		}
		SoundManager.instance.SlideIn();
		yield return new WaitForSeconds(0.1f);
		if (PlayerPrefsManager.GetDailyPuzzleNewLabel() > 0)
		{
			dailyButton.Find("NewLabel").gameObject.SetActive(value: true);
		}
		else
		{
			dailyButton.Find("NewLabel").gameObject.SetActive(value: false);
		}
		animBG.gameObject.SetActive(value: false);
		StartCoroutine(FugoUtils.Scaler(Vector3.one, 0.3f, startButton));
		StartCoroutine(FugoUtils.Scaler(Vector3.one, 0.3f, logo));
		StartCoroutine(FugoUtils.Scaler(Vector3.one, 0.3f, settingsButton));
		StartCoroutine(FugoUtils.Scaler(Vector3.one, 0.3f, facebookButton));
		StartCoroutine(FugoUtils.Scaler(Vector3.one, 0.3f, sun));
		StartCoroutine(FugoUtils.Scaler(Vector3.one, 0.3f, proBadge.transform));
		yield return new WaitForSeconds(0.1f);
		if (PlayerPrefsManager.GetLevel() >= 5)
		{
			StartCoroutine(FugoUtils.Scaler(Vector3.one, 0.3f, levelsButton));
		}
		yield return new WaitForSeconds(0.1f);
		if (PlayerPrefsManager.GetLevel() >= 13)
		{
			StartCoroutine(FugoUtils.Scaler(Vector3.one, 0.3f, dailyButton));
		}
		yield return new WaitForSeconds(0.1f);
		StartCoroutine(FugoUtils.Scaler(Vector3.one, 0.3f, questButton));
		if (PlayerPrefsManager.GetLang() != "Arabic" && PlayerPrefsManager.GetLang() != "Hebrew")
		{
			StartCoroutine(FugoUtils.Scaler(Vector3.one, 0.3f, shopButton.parent));
		}
		else
		{
			StartCoroutine(FugoUtils.Scaler(new Vector3(-1f, 1f, 1f), 0.3f, shopButton.parent));
		}
	}

	private IEnumerator PopupCloseThread(float time, Transform popup, bool fadePanelInteraction = true)
	{
		if (time > 0.01f)
		{
			SoundManager.instance.SlideOut();
		}
		popup.GetComponent<Animator>().enabled = false;
		if (fadePanelInteraction)
		{
			StartCoroutine(FugoUtils.FadeImage(0f, time, fadePanel));
		}
		StartCoroutine(FugoUtils.Scaler(Vector3.zero, time, popup));
		yield return new WaitForSeconds(time);
		if (fadePanelInteraction)
		{
			fadePanel.gameObject.SetActive(value: false);
		}
		popup.gameObject.SetActive(value: false);
	}

	public void SetTexts()
	{
		coinText.text = PlayerPrefsManager.GetCoin().ToString();
		proBadge.SetActive(PlayerPrefsManager.GetPro());
	}

	public void IncreaseCoinAnim(float delay, bool withsound = true)
	{
		StartCoroutine(IncreaseCoinAnimThread(delay, withsound));
	}

	public void GoToGameScreen()
	{
		SceneManager.LoadScene("GameNew");
	}

	private IEnumerator GoToGameScreenDelayed()
	{
		yield return new WaitForSeconds(0.1f);
		for (int i = 0; i < 5; i++)
		{
			yield return null;
		}
		GoToGameScreen();
	}

	private IEnumerator IncreaseCoinAnimThread(float delay, bool withSound = true)
	{
		yield return new WaitForSeconds(delay);
		int currentCoin = int.Parse(coinText.text);
		if (withSound)
		{
			StartCoroutine(IncreaseCoinSoundThread());
		}
		while (true)
		{
			currentCoin = ((PlayerPrefsManager.GetCoin() - currentCoin <= 50) ? (currentCoin + 5) : (currentCoin + 50));
			coinText.text = currentCoin.ToString();
			wheelCoinText.text = currentCoin.ToString();
			if (currentCoin >= PlayerPrefsManager.GetCoin())
			{
				break;
			}
			ArabicController.MakeArabicMenu(coinText.transform);
			ArabicController.MakeArabicMenu(wheelCoinText.transform);
			yield return new WaitForSeconds(0.03f);
		}
		coinText.text = PlayerPrefsManager.GetCoin().ToString();
		wheelCoinText.text = PlayerPrefsManager.GetCoin().ToString();
		StartCoroutine(FugoUtils.Scaler(Vector3.one * 1.2f, 0.1f, coinText.transform.parent));
		StartCoroutine(FugoUtils.Scaler(Vector3.one * 1.2f, 0.1f, wheelCoinText.transform));
		yield return new WaitForSeconds(0.1f);
		StartCoroutine(FugoUtils.Scaler(Vector3.one * 1f, 0.1f, coinText.transform.parent));
		StartCoroutine(FugoUtils.Scaler(Vector3.one * 1f, 0.1f, wheelCoinText.transform));
	}

	private IEnumerator IncreaseCoinSoundThread()
	{
		for (int i = 0; i < 3; i++)
		{
			SoundManager.instance.CoinIncrease();
			yield return new WaitForSeconds(0.2f);
		}
	}

	public void OpenHiddenMenu()
	{
		SoundManager.instance.Click();
		hiddenMenu.gameObject.SetActive(value: true);
		fadePanel.gameObject.SetActive(value: true);
		StartCoroutine(FugoUtils.FadeImage(0.6f, 0.35f, fadePanel));
		hiddenMenu.GetComponent<Animator>().enabled = true;
		hiddenMenu.GetComponent<Animator>().Play("PopupOpen");
	}

	public void CloseHiddenMenu(float time)
	{
		StartCoroutine(PopupCloseThread(time, hiddenMenu));
	}

	public void AfterGameFadeIn(LevelSet currentSet)
	{
		StartCoroutine(AfterGameFadeInThread(currentSet));
	}

	private IEnumerator AfterGameFadeInThread(LevelSet currentSet)
	{
		FugoUtils.ChangeAlpha(animBG.GetComponent<Image>(), 1f);
		animBG.gameObject.SetActive(value: true);
		string bgPath = GameController.set.bgImage;
		if (GameController.levelToOpen == -1)
		{
			animBG.GetComponent<Image>().sprite = Resources.Load<Sprite>("BGImages/" + bgPath);
			if (animBG.GetComponent<Image>().sprite == null)
			{
				animBG.GetComponent<Image>().sprite = Resources.Load<Sprite>("BGImages/" + bgPath);
			}
		}
		else
		{
			animBG.GetComponent<Image>().sprite = Resources.Load<Sprite>("BGImages/" + bgPath);
		}
		StartCoroutine(FugoUtils.FadeImage(0f, 0.5f, animBG.GetComponent<Image>()));
		yield return new WaitForSeconds(0.5f);
		animBG.gameObject.SetActive(value: false);
	}

	public void BeforeGameFadeOut(LevelSet currentSet)
	{
		if (GameController.levelToOpen == -1)
		{
			animBG.GetComponent<Image>().sprite = Resources.Load<Sprite>("BGImages/" + currentSet.bgImage);
			if (animBG.GetComponent<Image>().sprite == null)
			{
				animBG.GetComponent<Image>().sprite = Resources.Load<Sprite>("BGImages/" + GameController.set.bgImage);
			}
		}
		else
		{
			animBG.GetComponent<Image>().sprite = Resources.Load<Sprite>("BGImages/" + currentSet.bgImage);
		}
		FugoUtils.ChangeAlpha(animBG.GetComponent<Image>(), 0f);
		StartCoroutine(FugoUtils.FadeImage(1f, 0.1f, animBG.GetComponent<Image>()));
		animBG.gameObject.SetActive(value: true);
	}

	private void ScaleDownMenuItems()
	{
		if (fromDaily)
		{
			sun.localScale = Vector3.zero;
			sun.gameObject.SetActive(value: false);
			proBadge.transform.localScale = Vector3.zero;
			startButton.localScale = Vector3.zero;
			dailyButton.localScale = Vector3.zero;
			shopButton.localScale = Vector3.zero;
			levelsButton.localScale = Vector3.zero;
			settingsButton.localScale = Vector3.zero;
			facebookButton.localScale = Vector3.zero;
			logo.localScale = Vector3.zero;
			questButton.localScale = Vector3.zero;
			questButton.gameObject.SetActive(value: false);
		}
		else
		{
			SoundManager.instance.SlideOut();
			StartCoroutine(FugoUtils.Scaler(Vector3.zero, 0.3f, sun));
			StartCoroutine(FugoUtils.Scaler(Vector3.zero, 0.3f, startButton));
			StartCoroutine(FugoUtils.Scaler(Vector3.zero, 0.3f, dailyButton));
			StartCoroutine(FugoUtils.Scaler(Vector3.zero, 0.3f, shopButton.parent));
			StartCoroutine(FugoUtils.Scaler(Vector3.zero, 0.3f, levelsButton));
			StartCoroutine(FugoUtils.Scaler(Vector3.zero, 0.3f, settingsButton));
			StartCoroutine(FugoUtils.Scaler(Vector3.zero, 0.3f, facebookButton));
			StartCoroutine(FugoUtils.Scaler(Vector3.zero, 0.3f, logo));
			StartCoroutine(FugoUtils.Scaler(Vector3.zero, 0.3f, proBadge.transform));
			StartCoroutine(FugoUtils.Scaler(Vector3.zero, 0.3f, questButton));
			questButton.gameObject.SetActive(value: false);
		}
	}

	public void FadePanelOnClick()
	{
		if (!dailyGiftPopup.gameObject.activeSelf && !languagePopup.gameObject.activeSelf && !questPopup.gameObject.activeSelf && !redeemPopup.gameObject.activeSelf && !FortuneWheel.instance.IsAnimating() && !FortuneWheel.instance.IsFirstTime())
		{
			if (!shopPopup.gameObject.activeSelf && !rewardedPopup.gameObject.activeSelf)
			{
				SettingsCloseButtonClicked(0.3f);
				CloseRewardedPopup(0.3f);
				CloseHiddenMenu(0.3f);
				CloseSetTutorialPopup(0.3f);
				CloseProPopup(0.3f);
				CloseHiddenLevelPopup(0.3f);
				CloseWheelPopup(0.3f);
			}
			else if (shopPopup.gameObject.activeSelf && !rewardedPopup.gameObject.activeSelf)
			{
				ShopCloseButtonClicked(0.3f);
			}
		}
	}

	public void SetLangText()
	{
		if (langText != null && langText.GetComponent<CurvedText>() != null)
		{
			langText.GetComponent<Text>().text = LanguageSelectScript.GetLocalLanguage(PlayerPrefsManager.GetLang());
		}
	}

	private void OnApplicationFocus(bool focus)
	{
		if (focus)
		{
			RequestManager.instance.GetLogRequest();
			if (FortuneWheel.instance != null)
			{
				FortuneWheel.instance.CheckDailyGift();
			}
			if (GoogleAnalyticsScript.instance != null)
			{
				GoogleAnalyticsScript.instance.LogScreen("Menu");
			}
		}
	}

	public void SetVersionText()
	{
		versionText.text = LanguageScript.BuildText + " " + Application.version;
		if (PlayerPrefsManager.GetFBID() != string.Empty)
		{
			Text text = versionText;
			text.text = text.text + "\n" + PlayerPrefsManager.GetFBID();
			facebookButton.parent.gameObject.SetActive(value: false);
		}
		ArabicController.MakeArabicMenu(versionText.transform);
	}

	public void DisablePro()
	{
		string redeemProDate = PlayerPrefsManager.GetRedeemProDate();
		if (redeemProDate != string.Empty)
		{
			DateTime t = DateTime.Parse(redeemProDate);
			if (DateTime.Compare(DateTime.Now, t) < 0)
			{
				return;
			}
			PlayerPrefsManager.SetRedeemProDate(string.Empty);
		}
		PlayerPrefsManager.SetPro(0);
		SetTexts();
	}

	public void BackButtonPressed()
	{
		bool flag = false;
		IEnumerator enumerator = proPopup.parent.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform transform = (Transform)enumerator.Current;
				if (transform.gameObject.activeSelf)
				{
					flag = true;
				}
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		if (flag)
		{
			FadePanelOnClick();
		}
		else if (GameObject.Find("Canvas").transform.Find("GalleryPanel").gameObject.activeSelf)
		{
			SectionController.instance.GalleryImageOnClick();
		}
		else if (GameObject.Find("Canvas").transform.Find("SetPanel").gameObject.activeSelf)
		{
			SetController.instance.BackButtonClicked();
		}
		else if (sectionPanel.gameObject.activeSelf)
		{
			SectionController.instance.BackButtonOnClick();
		}
		else if (GameObject.Find("Canvas").transform.Find("DailyPuzzlePanel").gameObject.activeSelf)
		{
			GetComponent<DailyPuzzleController>().BackButtonClicked();
		}
		else
		{
			Application.Quit();
		}
	}

	private void SetIfArabic()
	{
		UISwapper.flipGame = false;
		if (PlayerPrefsManager.GetLang() == "Arabic")
		{
			UISwapper.flipGame = true;
		}
		if (PlayerPrefsManager.GetLang() == "Arabic" || PlayerPrefsManager.GetLang() == "Hebrew")
		{
			UnityEngine.Object.Destroy(supportPopupAnimation.GetComponent<Animator>());
			UnityEngine.Object.Destroy(supportPopupAnimation.GetComponent<RectMask2D>());
			UnityEngine.Object.Destroy(supportPopupAnimation.GetComponent<SettingsPopupController>());
			supportPopupAnimation.transform.Find("SupportButtonHolder/SupportButton").GetComponent<Button>().onClick.RemoveAllListeners();
			supportPopupAnimation.transform.Find("SupportButtonHolder/SupportButton").GetComponent<Button>().onClick.AddListener(delegate
			{
				SupportButtonOnClick();
			});
			supportPopupAnimation.transform.Find("RedeemHolder").gameObject.SetActive(value: false);
		}
	}
}
