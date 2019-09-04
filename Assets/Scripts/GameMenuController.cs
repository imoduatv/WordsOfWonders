using UnityEngine;
using UnityEngine.UI;
using v2Gameplay;

public class GameMenuController : MonoBehaviour
{
	public static GameMenuController instance;

	public static bool megaHintOpening;

	public GameObject coin;

	public GameObject nameText;

	public GameObject hintButton;

	public GameObject collect;

	public GameObject container;

	public GameObject contiune;

	public GameObject coinPrefab;

	public Sprite coinSprite;

	public Sprite crownSprite;

	public Text megaHintPrice;

	public GameObject handPrefab;

	public Button finishGameButton;

	private AsyncOperation op;

	private int gold;

	private void Awake()
	{
		SoundManager.instance.StopMusic();
		gold = 0;
		instance = this;
		SplashScript.splashShown = true;
		if (PlayerPrefsManager.GetHiddenMenu())
		{
			finishGameButton.gameObject.SetActive(value: true);
			finishGameButton.transform.localScale = Vector3.one;
		}
	}

	private void Start()
	{
		if (PlayerPrefsManager.IsBlueMode())
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(handPrefab);
			gameObject.transform.SetParent(GameObject.Find("Canvas/Enviroment").transform);
			gameObject.transform.ResetTransform();
			Color color = new Color(0f, 0f, 0f, 0f);
			GameObject.Find("Canvas/Background").GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
			Camera.main.backgroundColor = Color.blue;
		}
		updateCoin(animating: false);
		updateHintPrice();
		SoundManager.instance.GameStart();
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		if (pauseStatus)
		{
			PlayerPrefsManager.IncreaseCoin(gold);
			gold = 0;
		}
	}

	public void openMain()
	{
		if (BoardController.levelToOpen != 0)
		{
		}
		PlayerPrefsManager.SetLatestLevel(PlayerPrefsManager.GetLevel());
		SoundManager.instance.Click();
		MenuController.fromGame = true;
		FugoUtils.openScene("Menu");
	}

	public void openSections()
	{
		SoundManager.instance.Click();
		MenuController.fromGameToSections = true;
		FugoUtils.openScene("Menu");
	}

	public void nextLevel()
	{
		SoundManager.instance.Click();
		if (FugoAdManager.instance.InterstitialReady())
		{
			if (PlayerPrefsManager.GetTutorialDone(5))
			{
				FugoAdManager.instance.interstitialClosed = prepareNextLevel;
			}
			else
			{
				FugoAdManager.instance.interstitialClosed = TutorialController.instance.openShop;
			}
			FugoAdManager.instance.ShowInterstitial();
		}
		else
		{
			prepareNextLevel();
		}
	}

	private void prepareNextLevel()
	{
		if (GameController.game.mode == GameMode.ADVENTURE)
		{
			if (GameController.adventureGameId == (GameController.adventureCount + 1).ToString())
			{
				PlayerPrefsManager.SetLastPlayedMode(GameMode.NORMAL);
				Movements.instance.executeWithDelay((Movements.Execute)openSections, 0.7f);
			}
			else
			{
				Movements.instance.executeWithDelay((Movements.Execute)openNext, 0.7f);
			}
		}
		else
		{
			Movements.instance.executeWithDelay((Movements.Execute)openNext, 0.7f);
		}
		GameAnimController.instance.hideMapAndStuff();
		collect.transform.Find("Frame").gameObject.SetActive(value: false);
		contiune.transform.Find("Frame").gameObject.SetActive(value: false);
	}

	public void onBackClicked()
	{
		float num = 1f;
		if (GameController.game.mode == GameMode.ADVENTURE)
		{
			v2Gameplay.BoardController.animations.fadeAdventureBoard();
		}
		else
		{
			num = v2Gameplay.BoardController.animations.fadeBoard();
		}
		AnimController.instance.endGameAnims();
		Movements.instance.executeWithDelay((Movements.Execute)openMenu, num + 0.1f);
		HiddenLevelController.instance.endAnim();
	}

	public void openMenu()
	{
		FugoUtils.openScene("Menu");
	}

	private void openNext()
	{
		if (GameController.endType != 0)
		{
			SectionController.setCompleted = true;
			FugoUtils.openScene("Menu");
		}
		else
		{
			FugoUtils.openScene("GameNew");
		}
	}

	public void updateCoin(bool animating)
	{
		coin.GetComponent<Text>().text = PlayerPrefsManager.GetCoin().ToString();
		if (animating)
		{
			Vector3 vector = (!UISwapper.flipGame) ? Vector3.one : new Vector3(-1f, 1f, 1f);
			Movements.instance.scale(coin, vector, vector * 1.1f, 0.1f);
			Movements.instance.scale(coin, vector * 1.1f, vector, 0.1f);
		}
	}

	public void onHintClicked()
	{
        if (PlayerPrefsManager.GetCoin() >= PlayerPrefsManager.GetHintPrice())
        {
            v2Gameplay.BoardController.instance.getHint();
    }
		else
		{
			onShopClicked();
}
	}

	public void onMegaHintClicked()
	{
		if (PlayerPrefsManager.GetCoin() >= MegaHintController.GetHintPrice())
		{
			MegaHintController.instance.openLetterSelect();
			if (megaHintOpening)
			{
				PlayerPrefsManager.DecreaseCoin(MegaHintController.GetHintPrice());
			}
		}
		else
		{
			onShopClicked();
		}
		updateCoin(animating: false);
		updateHintPrice();
		megaHintOpening = false;
	}

	public void updateHintPrice()
	{
		megaHintPrice.text = MegaHintController.GetHintPrice().ToString();
		int hintPrice = PlayerPrefsManager.GetHintPrice();
		if (hintPrice == 0 && !TutorialController.freeHint)
		{
			hintButton.transform.Find("Hint/Text").gameObject.SetActive(value: true);
			hintButton.transform.Find("Hint/coin").gameObject.SetActive(value: true);
			hintButton.transform.Find("Hint/coin").GetComponent<Image>().sprite = crownSprite;
			hintButton.transform.Find("Hint/Text").GetComponent<Text>().text = "x" + PlayerPrefsManager.GetProHint();
		}
		else if (hintPrice == 0)
		{
			hintButton.transform.Find("Hint/Text").gameObject.SetActive(value: false);
			hintButton.transform.Find("Hint/coin").gameObject.SetActive(value: false);
		}
		else
		{
			hintButton.transform.Find("Hint/Text").gameObject.SetActive(value: true);
			hintButton.transform.Find("Hint/coin").gameObject.SetActive(value: true);
			hintButton.transform.Find("Hint/Text").GetComponent<Text>().text = PlayerPrefsManager.GetHintPrice().ToString();
			hintButton.transform.Find("Hint/coin").GetComponent<Image>().sprite = coinSprite;
		}
	}

	public void onShopClicked()
	{
		SoundManager.instance.Click();
		ExtraWordController.instance.closeExtraWords();
		Movements.instance.executeWithDelay((Movements.Execute)SoundManager.instance.SlideIn, 0.1f);
		GameAnimController.instance.openShopAnimation();
		BackButtonController.instance.onBackClicked = onShopCloseClicked;
		closeProPopup();
		ObjectHolder.instance.starButton.transform.Find("Plus").GetComponent<Button>().interactable = false;
	}

	public void onShopCloseClicked()
	{
		Movements.instance.executeWithDelay((Movements.Execute)SoundManager.instance.SlideOut, 0.1f);
		ObjectHolder.instance.starButton.transform.Find("Plus").GetComponent<Button>().interactable = true;
		SoundManager.instance.Click();
		GameAnimController.instance.closeShopAnimation();
	}

	public void collectClicked()
	{
		SoundManager.instance.Click();
		collect.GetComponent<Button>().interactable = false;
		prepareCollect();
	}

	private void prepareCollect()
	{
		int num = gold / 5;
		num--;
		for (int i = 0; i < 5; i++)
		{
			PlayerPrefsManager.IncreaseCoin(num);
			Movements.instance.executeWithDelay((Movements.Execute)spawnCoin, (float)i * 0.1f);
		}
		Movements.instance.scale(collect.transform.parent.parent.gameObject, Vector3.one, Vector3.zero, 0.3f, 0.5f);
		Movements.instance.executeWithDelay((Movements.Execute)GameAnimController.instance.hideShop, 1.2f);
		Movements.instance.executeWithDelay((Movements.Execute)GameAnimController.instance.hideMapAndStuff, 1.5f);
		Movements.instance.executeWithDelay((Movements.Execute)GameAnimController.instance.openNextPage, 2.2f);
		collect.transform.Find("Frame").gameObject.SetActive(value: false);
		contiune.transform.Find("Frame").gameObject.SetActive(value: false);
	}

	public void spawnCoin()
	{
		Transform transform = collect.transform.Find("Text/Coin/Mid");
		Transform transform2 = UnityEngine.Object.Instantiate(coinPrefab).transform;
		transform2.SetParent(container.transform);
		transform2.SetAsLastSibling();
		transform2.position = transform.position;
		transform2.localScale = Vector3.one;
		transform2.GetComponent<CoinAnim>().start = transform2.position;
		transform2.GetComponent<CoinAnim>().end = GameAnimController.instance.store.transform.Find("ShopButton").position;
		transform2.GetComponent<CoinAnim>().startParabolicMove();
	}

	public void setGoldAmount(int val)
	{
		gold = val;
		if (UISwapper.flipGame)
		{
			UnityEngine.Debug.Log("set gold arabic " + val);
			ArabicText arabicText = collect.transform.Find("Text").gameObject.AddComponent<ArabicText>();
			arabicText.Text = AutoLanguage.dict["CollectText"] + " " + val;
		}
		else
		{
			collect.transform.Find("Text").GetComponent<Text>().text = AutoLanguage.dict["CollectText"] + " " + val;
		}
	}

	public void nextPageClicked()
	{
		GameAnimController.instance.closeDescription();
		Movements.instance.executeWithDelay((Movements.Execute)openNext, 0.15f);
	}

	public void openProPopup()
	{
		GameObject.Find("Canvas").transform.Find("Enviroment/ShopHolder/ProPopup/ProButton/PriceText").GetComponent<Text>().text = ShopScript.itemPrices[6] + " / " + LanguageScript.WeeklyText;
		GameObject.Find("Canvas").transform.Find("Enviroment/ShopHolder/ProPopup/ProButton/Text").GetComponent<Text>().text = LanguageScript.BecomeProText;
		GameObject.Find("Canvas").transform.Find("Enviroment/ShopHolder/ProPopup/DescriptionText").GetComponent<Text>().text = LanguageScript.ProContentText.Replace("%@", ShopScript.itemPrices[6]);
		GameObject.Find("Canvas").transform.Find("Enviroment/ShopHolder/ProPopup/BG/HintText").GetComponent<Text>().text = LanguageScript.ProFreeHintText;
		GameObject.Find("Canvas").transform.Find("Enviroment/ShopHolder/ProPopup/BG/NoAdText").GetComponent<Text>().text = LanguageScript.ProNoAdText;
		GameObject.Find("Canvas").transform.Find("Enviroment/ShopHolder/ProPopup/BG/GiftText").GetComponent<Text>().text = LanguageScript.ProDoubleDailyText;
		if (PlayerPrefsManager.GetPro())
		{
			GameObject.Find("Canvas").transform.Find("Enviroment/ShopHolder/ProPopup/ProButton/PriceText").GetComponent<Text>().text = LanguageScript.AlreadyPurchasedText;
			GameObject.Find("Canvas").transform.Find("Enviroment/ShopHolder/ProPopup/ProButton/PriceText").GetComponent<Text>().color = Color.white;
			GameObject.Find("Canvas").transform.Find("Enviroment/ShopHolder/ProPopup/ProButton").GetComponent<Button>().interactable = false;
			GameObject.Find("Canvas").transform.Find("Enviroment/ShopHolder/ProPopup/DescriptionText").GetComponent<Text>().text = LanguageScript.ProContentText.Replace("%@", ShopScript.itemPrices[6]);
		}
		UnityEngine.Debug.Log("game pro");
		GameObject.Find("Canvas").transform.Find("Enviroment/ShopHolder/ProPopup").gameObject.SetActive(value: true);
		GameObject.Find("Canvas").transform.Find("Enviroment/ShopHolder/ShopPopup").gameObject.SetActive(value: false);
		if (UISwapper.flipGame)
		{
			GameObject.Find("Canvas").transform.Find("Enviroment/ShopHolder/ProPopup/BG/HintText").transform.localScale = new Vector3(1f, 1f, 1f);
			GameObject.Find("Canvas").transform.Find("Enviroment/ShopHolder/ProPopup/BG/NoAdText").transform.localScale = new Vector3(1f, 1f, 1f);
			GameObject.Find("Canvas").transform.Find("Enviroment/ShopHolder/ProPopup/BG/GiftText").transform.localScale = new Vector3(1f, 1f, 1f);
			GameObject.Find("Canvas").transform.Find("Enviroment/ShopHolder/ProPopup/BG").transform.localScale = new Vector3(-1f, 1f, 1f);
		}
		ObjectHolder.instance.starButton.transform.Find("Plus").GetComponent<Button>().interactable = false;
	}

	public void closeProPopup()
	{
		GameObject.Find("Canvas").transform.Find("Enviroment/ShopHolder/ProPopup").gameObject.SetActive(value: false);
		ObjectHolder.instance.starButton.transform.Find("Plus").GetComponent<Button>().interactable = true;
	}

	public void closeFade()
	{
		Movements.instance.startFadeOut(GameAnimController.instance.fade, 0.3f, 0f);
		GameAnimController.instance.fade.GetComponent<Image>().raycastTarget = false;
	}
}
