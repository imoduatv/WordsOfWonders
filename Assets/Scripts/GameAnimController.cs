using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using v2Gameplay;

public class GameAnimController : MonoBehaviour
{
	public static GameObject dailyStar;

	public static GameAnimController instance;

	public static float initTime = 0.4f;

	public GameObject coinPrefab;

	public GameObject ribbonPrefab;

	public GameObject enviroment;

	public GameObject shuffle;

	public GameObject back;

	public GameObject star;

	public GameObject sections;

	public new GameObject name;

	public GameObject extrawords;

	public GameObject fade;

	public GameObject fadeExtra;

	public GameObject hint;

	public GameObject store;

	public GameObject dailyStartText;

	public GameObject nextPage;

	public GameObject startHolder;

	public GameObject end;

	public GameObject shopPopUp;

	public GameObject backbutton;

	public GameObject setbutton;

	public GameObject shufflebutton;

	public GameObject levelname;

	public GameObject sphere;

	public int starCount;

	public bool extra;

	private Level level;

	private Coroutine extraScale;

	private Coroutine extraFade;

	private GameObject startCirle;

	private GameObject endCircle;

	private GameObject randomRibbon;

	private string[] ribbonWords = new string[7]
	{
		"GREAT",
		"WONDERFULL",
		"EXCELENT",
		"NICE",
		"SUPER",
		"WOW",
		"AMAZING"
	};

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		startCirle = end.transform.Find("LogoNStuff/Progress/Start").gameObject;
		endCircle = end.transform.Find("LogoNStuff/Progress/End").gameObject;
		instance = this;
		extra = false;
	}

	private void getGame(Level l)
	{
		level = l;
	}

	public void toMenu()
	{
		GameObject[,] board = BoardController.instance.board;
		int num = board.GetLength(0) + board.GetLength(1);
		float delay = 0f;
		float num2 = 0.35f / (float)num;
		GameObject[,] array = board;
		int length = array.GetLength(0);
		int length2 = array.GetLength(1);
		for (int i = 0; i < length; i++)
		{
			for (int j = 0; j < length2; j++)
			{
				GameObject gameObject = array[i, j];
				if (!(gameObject == null))
				{
					Cell component = gameObject.GetComponent<Cell>();
					GameObject gameObject2 = gameObject.transform.Find("BG").gameObject;
					GameObject gameObject3 = gameObject.transform.Find("BG/Text").gameObject;
					GameObject gameObject4 = gameObject.transform.Find("BG/Stroke").gameObject;
					GameObject gameObject5 = gameObject.transform.Find("BG/Coin").gameObject;
					delay = (float)(num - (component.x + component.y)) * num2;
					Movements.instance.startFadeOut(gameObject2.GetComponent<Image>(), 0.15f, 0f, delay);
					Movements.instance.startFadeOut(gameObject4.GetComponent<Image>(), 0.15f, 0f, delay);
					Movements.instance.startFadeOut(gameObject5.GetComponent<Image>(), 0.15f, 0f, delay);
					Movements.instance.startFadeOut(gameObject3.GetComponent<Text>(), 0.15f, 0f, delay);
				}
			}
		}
		Movements.instance.startFadeOut(startHolder.transform.Find("StarBG").GetComponent<Image>(), 0.15f, 0f, delay);
		Movements.instance.startFadeOut(startHolder.transform.Find("StarBG/StarCountText").GetComponent<Text>(), 0.15f, 0f, delay);
		Movements.instance.startFadeOut(startHolder.transform.Find("Frame").GetComponent<Image>(), 0.15f, 0f, delay);
		Movements.instance.startFadeOut(startHolder.transform.Find("Frame/Star").GetComponent<Image>(), 0.15f, 0f, delay);
		Movements.instance.startFadeOut(dailyStar, (float)num * num2 + 0.17f, 0f);
		Movements.instance.startFadeOut(name, (float)num * num2 + 0.17f, 0f);
		Movements.instance.scale(WheelController.instance.gameObject, 0f, (float)num * num2 + 0.17f);
		clearButtons();
		Movements.instance.executeWithDelay((Movements.Execute)GameMenuController.instance.openMain, (float)num * num2 + 0.17f);
	}

	private void clearButtons()
	{
		Movements.instance.move(shuffle, shuffle.transform.position, shuffle.transform.position + Vector3.left * 3f, 0.5f);
		Movements.instance.move(back, back.transform.position, back.transform.position + Vector3.left * 3f, 0.5f);
		Movements.instance.move(star, star.transform.position, star.transform.position + Vector3.left * 3f, 0.5f);
		Movements.instance.move(sections, sections.transform.position, sections.transform.position + Vector3.right * 3f, 0.5f);
		Movements.instance.move(store, store.transform.position, store.transform.position + Vector3.right * 3f, 0.5f);
		Movements.instance.move(hint, hint.transform.position, hint.transform.position + Vector3.right * 3f, 0.5f);
	}

	public void startInitAnims()
	{
		FugoUtils.ChangeAlpha(hint.transform.Find("Hint/icon").GetComponent<Image>(), 0.5f);
		for (int i = 0; i < ribbonWords.Length; i++)
		{
			ribbonWords[i] = AutoLanguage.dict["Ribbon" + (i + 1)];
		}
		shuffle.SetActive(value: true);
		back.SetActive(value: true);
		hint.SetActive(value: true);
		store.SetActive(value: true);
		sections.SetActive(value: true);
		if (PlayerPrefsManager.GetLevel() >= 3)
		{
			Movements.instance.move(shuffle, shuffle.transform.position + Vector3.left * 1f, shuffle.transform.position, initTime);
		}
		Movements.instance.move(back, back.transform.position + Vector3.left * 1f, back.transform.position, initTime);
		if (PlayerPrefsManager.GetFirstExtra())
		{
			enableExtra();
		}
		Movements.instance.move(sections, sections.transform.position + Vector3.right * 1f, sections.transform.position, initTime);
		if (PlayerPrefsManager.GetLevel() >= 5)
		{
			Movements.instance.move(hint, hint.transform.position + Vector3.right * 1f, hint.transform.position, initTime);
		}
		if (PlayerPrefsManager.GetLevel() == 1)
		{
			store.SetActive(value: false);
		}
		else
		{
			Movements.instance.move(store, store.transform.position + Vector3.right * 3f, store.transform.position, initTime);
		}
		Movements.instance.startFadeIn(name, initTime, 1f);
		GameObject gameObject = end.transform.Find("LogoNStuff/Progress").gameObject;
		end.transform.Find("LogoNStuff/SunHolder/BrillianceText").GetComponent<Text>().text = PlayerPrefsManager.GetBrilliance().ToString() + "\n<color=#71b1e0><size=25>" + LanguageScript.ExpeditionText + "</size></color>";
		end.transform.Find("LogoNStuff/SunHolder/Core").GetComponent<Image>().color = Color.black;
		UnityEngine.Debug.Log("init anim done");
	}

	public void enableCoins()
	{
		for (int i = 0; i < level.width; i++)
		{
			for (int j = 0; j < level.width; j++)
			{
				try
				{
					Cell component = BoardController.instance.board[i, j].GetComponent<Cell>();
					if (BoardController.levelToOpen == -1)
					{
						Movements.instance.executeWithDelay((Movements.Execute)component.enableCoin, (float)(i + j) * 0.01f);
					}
				}
				catch (Exception)
				{
				}
			}
		}
		int[] starPos = PlayerPrefsManager.getStarPos();
		if (starPos[0] != -1)
		{
			BoardController.instance.spawnStar(starPos);
		}
		else
		{
			BoardController.instance.setStar();
		}
	}

	public void toggleExtraWords()
	{
		if (!WheelController.running)
		{
		}
		SoundManager.instance.Click();
		float num = 0.2f;
		extra = !extra;
		WheelController.running = !extra;
		fadeExtra.GetComponent<Image>().raycastTarget = extra;
		if (extraScale != null)
		{
			StopCoroutine(extraScale);
		}
		if (extraFade != null)
		{
			StopCoroutine(extraFade);
		}
		if (extra)
		{
			Movements.instance.executeWithDelay((Movements.Execute)SoundManager.instance.SlideIn, 0.1f);
			extraScale = Movements.instance.scale(extrawords, extrawords.transform.localScale, Vector3.one, num);
			extraFade = Movements.instance.startFadeIn(fadeExtra, num, 0.3f);
		}
		else
		{
			Movements.instance.executeWithDelay((Movements.Execute)SoundManager.instance.SlideOut, 0.1f);
			extraScale = Movements.instance.scale(extrawords, extrawords.transform.localScale, Vector3.zero, num);
			extraFade = Movements.instance.startFadeOut(fadeExtra, num, 0f);
		}
	}

	public void endGame()
	{
		if (!WheelController.running)
		{
		}
		sphere.SetActive(value: true);
		star.transform.Find("Plus").GetComponent<Button>().enabled = false;
		end.transform.SetAsLastSibling();
		shopPopUp.transform.parent.SetAsLastSibling();
		WheelController.instance.wordContiner.SetActive(value: false);
		end.transform.Find("LogoNStuff/Progress/ProgressText").GetComponent<Text>().text = BoardController.set.SetFullName + " " + (BoardController.instance.info[2] + 1) + "/" + BoardController.set.levels.Count;
		end.transform.Find("LogoNStuff/Progress/Start/Text").GetComponent<Text>().text = BoardController.set.SetName;
		hideButtons();
		end.transform.Find("NiceText").GetComponent<Image>().color = FugoUtils.HexToColor(BoardController.set.InGameRibbonColor);
		end.transform.Find("NiceText/Text").GetComponent<Text>().text = ribbonWords[UnityEngine.Random.Range(0, ribbonWords.Length - 1)];
		UnityEngine.Debug.Log("is it daily? " + BoardController.daily);
		if (BoardController.daily)
		{
			PlayerPrefsManager.ResetContinueDaily();
		}
		else if (BoardController.levelToOpen == -1)
		{
			PlayerPrefsManager.ResetContinue();
		}
	}

	public void hideButtons()
	{
		float num = 0.5f;
		Movements.instance.move(backbutton, backbutton.transform.position, backbutton.transform.position + Vector3.left * 3f, num);
		Movements.instance.move(shufflebutton, shufflebutton.transform.position, shufflebutton.transform.position + Vector3.left * 3f, num);
		Movements.instance.move(star, star.transform.position, star.transform.position + Vector3.left * 3f, num);
		Movements.instance.move(setbutton, setbutton.transform.position, setbutton.transform.position + Vector3.right * 3f, num);
		if (!BoardController.nextSet)
		{
			Movements.instance.move(store, store.transform.position, store.transform.position + Vector3.right * 3f, num);
		}
		Movements.instance.move(dailyStartText.transform.parent.parent.gameObject, dailyStartText.transform.parent.parent.transform.position, dailyStartText.transform.parent.parent.transform.position + Vector3.up * 3f, num);
		Movements.instance.move(hint, hint.transform.position, hint.transform.position + Vector3.right * 3f, num);
		Movements.instance.startFadeOut(levelname, num, 0f);
		if (BoardController.daily)
		{
			clearBoard();
		}
		else
		{
			Movements.instance.executeWithDelay((Movements.Execute)niceTexts, 0.3f);
		}
	}

	public void niceTexts()
	{
		SoundManager.instance.playRibbon();
		GameObject gameObject = end.transform.Find("NiceText").gameObject;
		GameObject gameObject2 = end.transform.Find("NiceText/Text").gameObject;
		if (PlayerPrefsManager.GetLang() == SystemLanguage.Hebrew.ToString() || PlayerPrefsManager.GetLang() == SystemLanguage.Arabic.ToString())
		{
			ArabicText arabicText = gameObject2.GetComponent<ArabicText>();
			if (arabicText == null)
			{
				arabicText = gameObject2.gameObject.AddComponent<ArabicText>();
			}
			arabicText.Text = AutoLanguage.dict["Ribbon" + UnityEngine.Random.Range(1, 7)];
		}
		else
		{
			gameObject2.GetComponent<Text>().text = AutoLanguage.dict["Ribbon" + UnityEngine.Random.Range(1, 7)];
		}
		gameObject.GetComponent<Image>().color = GameController.RibbonColor;
		float num = 0.3f;
		Movements.instance.startFadeIn(gameObject, num, 1f);
		Movements.instance.startFadeIn(gameObject2, num, 1f);
		Movements.instance.scale(gameObject, Vector3.one * 0.5f, Vector3.one * 1f, num);
		Movements.instance.executeWithDelay((Movements.Execute)fadeNiceText, num + 0.5f);
	}

	public void openMenu()
	{
		FugoUtils.openScene("Menu");
	}

	public void clearBoard()
	{
		GameObject gameObject = WheelController.instance.gameObject;
		float num = 0.2f;
		int width = BoardController.level.width;
		int height = BoardController.level.height;
		float num2 = 0f;
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				GameObject gameObject2 = BoardController.instance.board[i, j];
				if (!(gameObject2 == null) && gameObject2.activeSelf)
				{
					checkDuplicates(gameObject2.transform);
					Vector3 position = gameObject2.transform.position;
					Vector3 vector = position + Vector3.down * 1f;
					Image component = gameObject2.transform.Find("BG").GetComponent<Image>();
					Text text = null;
					try
					{
						text = gameObject2.transform.Find("Piece(Clone)").GetComponent<Text>();
					}
					catch (Exception)
					{
					}
					Text component2 = gameObject2.transform.Find("BG/Text").GetComponent<Text>();
					Image component3 = gameObject2.transform.Find("BG/Stroke").GetComponent<Image>();
					Image component4 = gameObject2.transform.Find("BG/Coin").GetComponent<Image>();
					num2 = (float)(width + height - i - j) * 0.08f;
					Movements.instance.move(gameObject2, position, vector, num, num2);
					Movements.instance.startFadeOut(component, num, 0f, num2);
					if (text != null)
					{
						Movements.instance.startFadeOut(text, num, 0f, num2);
					}
					Movements.instance.startFadeOut(component3, num, 0f, num2);
					Movements.instance.startFadeOut(component4, num, 0f, num2);
					Movements.instance.startFadeOut(component2, num, 0f, num2);
				}
			}
		}
		num *= 2f;
		Movements.instance.scale(gameObject, Vector3.one, Vector3.zero, num);
		Movements.instance.startFadeOut(gameObject, num, 0f);
		for (int k = 0; k < gameObject.transform.Find("LetterContainer").childCount; k++)
		{
			GameObject gameObject3 = gameObject.transform.Find("LetterContainer").GetChild(k).gameObject;
			Movements.instance.startFadeOut(gameObject3.transform.Find("Text").gameObject, num, 0f);
		}
		if (BoardController.daily)
		{
			MenuController.fromDaily = true;
			PlayerPrefsManager.SetEarnedStar(instance.starCount);
			Movements.instance.executeWithDelay((Movements.Execute)openMenu, num2 + num + 0.6f);
		}
		num2 *= 1.2f;
		Movements.instance.executeWithDelay((Movements.Execute)disableBoard, num2 + num + 0.9f);
	}

	private void disableBoard()
	{
		WheelController.instance.gameObject.SetActive(value: false);
	}

	private void fadeNiceText()
	{
		float num = 0.2f;
		GameObject gameObject = end.transform.Find("NiceText").gameObject;
		GameObject gameObject2 = end.transform.Find("NiceText/Text").gameObject;
		Movements.instance.move(gameObject, gameObject.transform.position, gameObject.transform.position + Vector3.down * 2f, num);
		Movements.instance.startFadeOut(gameObject, num, 0f);
		Movements.instance.startFadeOut(gameObject2, num, 0f);
	}

	private void enableSun()
	{
		GameObject gameObject = end.transform.Find("LogoNStuff").gameObject;
		gameObject.SetActive(value: true);
		GameObject gameObject2 = gameObject.transform.Find("Logo").gameObject;
		GameObject gameObject3 = gameObject.transform.Find("SunHolder").gameObject;
		GameObject gameObject4 = gameObject.transform.Find("Progress").gameObject;
		gameObject4.transform.localScale = Vector3.zero;
		gameObject4.SetActive(value: true);
		float num = 0.4f;
		float num2 = 0.1f;
		float delay = num;
		Movements.instance.scale(gameObject2, Vector3.zero, Vector3.one * 1.2f * SafeAreaScaler.scale, num);
		Movements.instance.scale(gameObject2, Vector3.one * 1.2f * SafeAreaScaler.scale, Vector3.one * SafeAreaScaler.scale, num2, delay);
		delay = num + num2;
		Movements.instance.scale(gameObject3, Vector3.zero, Vector3.one * 1.2f * SafeAreaScaler.scale, num, delay);
		delay += num;
		Movements.instance.executeWithDelay((Movements.Execute)SoundManager.instance.playPop1, delay);
		Movements.instance.scale(gameObject3, Vector3.one * 1.2f * SafeAreaScaler.scale, Vector3.one * SafeAreaScaler.scale, num2, delay);
		delay += num2;
		Movements.instance.scale(gameObject4, Vector3.zero, Vector3.one * 1.2f * SafeAreaScaler.scale, num, delay);
		delay += num;
		Movements.instance.scale(gameObject4, Vector3.one * 1.2f * SafeAreaScaler.scale, Vector3.one * SafeAreaScaler.scale, num2, delay);
		delay += num2;
		Movements.instance.executeWithDelay((Movements.Execute)expandSun, delay);
	}

	private void expandSun()
	{
		GameObject gameObject = end.transform.Find("LogoNStuff/SunHolder").gameObject;
		GameObject gameObject2 = gameObject.transform.Find("BrillianceText").gameObject;
		float num = 0.6f;
		float num2 = 0.4f;
		float num3 = num;
		SoundManager.instance.scoreCount();
		Movements.instance.scale(gameObject, Vector3.one * SafeAreaScaler.scale, Vector3.one * 1.2f * SafeAreaScaler.scale, num);
		Movements.instance.scale(gameObject2, 0f, 0.1f);
		Movements.instance.executeWithDelay((Movements.Execute)setVoyageText, 0.1f);
		Movements.instance.scale(gameObject2, Vector3.zero, Vector3.one, 0.1f, 0.1f);
		Sphere.instance.spin();
		Movements.instance.scale(gameObject, Vector3.one * 1.2f * SafeAreaScaler.scale, Vector3.one * SafeAreaScaler.scale, num2, num3);
		shine();
		num3 += num2;
		Movements.instance.executeWithDelay((Movements.Execute)walkOnMap, num3);
	}

	private void setVoyageText()
	{
		GameObject gameObject = end.transform.Find("LogoNStuff/SunHolder").gameObject;
		gameObject.transform.Find("BrillianceText").GetComponent<Text>().text = PlayerPrefsManager.GetBrilliance().ToString() + "\n<color=#71b1e0><size=25>" + LanguageScript.ExpeditionText + "</size></color>";
	}

	private void walkOnMap()
	{
		MapController.instance.calculatePositions();
		enableNext();
		for (int i = 0; i < 0; i++)
		{
			Transform transform = UnityEngine.Object.Instantiate(coinPrefab).transform;
			transform.position = extrawords.transform.position;
			transform.localScale = Vector3.one;
			CoinAnim component = transform.GetComponent<CoinAnim>();
			component.start = extrawords.transform.position;
			component.end = store.transform.position;
			component.startParabolicMove((float)i * 0.1f);
		}
		Movements.instance.executeWithDelay((Movements.Execute)MapController.instance.checkRibbon, 0.6f);
	}

	private void enableNext()
	{
		UnityEngine.Debug.Log("enable buttons");
		BoardController.daily = false;
		MapController.instance.spawnBlink();
		GameObject gameObject = end.transform.Find("ButtonHolder/Collect").gameObject;
		GameObject gameObject2 = end.transform.Find("ButtonHolder/Next").gameObject;
		GameObject gameObject3 = end.transform.Find("ButtonHolder").gameObject;
		if (BoardController.nextSet)
		{
			gameObject2.SetActive(value: false);
			gameObject.SetActive(value: true);
		}
		else
		{
			gameObject2.SetActive(value: true);
			gameObject.SetActive(value: false);
		}
		Movements.instance.scale(gameObject3, Vector3.zero, Vector3.one, 0.2f);
	}

	private void checkDuplicates(Transform parent)
	{
		int num = 0;
		for (int i = 0; i < parent.childCount; i++)
		{
			if (parent.GetChild(i).name == "Piece(Clone)")
			{
				num++;
				parent.GetChild(i).name = "a";
			}
		}
		try
		{
			GameObject gameObject = parent.Find("a").gameObject;
			gameObject.name = "Piece(Clone)";
			while (num > 1)
			{
				num--;
				GameObject gameObject2 = parent.Find("a").gameObject;
				UnityEngine.Object.Destroy(gameObject2);
			}
		}
		catch (Exception)
		{
		}
	}

	public void openGame()
	{
		PlayerPrefsManager.SetLevel(PlayerPrefsManager.GetLevel() + 1);
		RequestManager.instance.LogRequest();
		if (BoardController.levelToOpen != -1)
		{
			BoardController.levelToOpen++;
		}
		if (BoardController.nextSet)
		{
			SectionController.setCompleted = true;
			FugoUtils.openScene("Menu");
		}
		else
		{
			FugoUtils.openScene("Game");
		}
	}

	public void shine()
	{
		float time = 2.5f;
		Movements.instance.startFadeIn(end.transform.Find("LogoNStuff/SunHolder/Shine").gameObject, time, 1f);
		Movements.instance.executeWithDelay((Movements.Execute)dim, time);
	}

	public void dim()
	{
		float time = 2.5f;
		Movements.instance.startFadeOut(end.transform.Find("LogoNStuff/SunHolder/Shine").gameObject, time, 0.5f);
		Movements.instance.executeWithDelay((Movements.Execute)shine, time);
	}

	public void fadeOutAll()
	{
		end.transform.Find("LogoNStuff/SunHolder/Shine").gameObject.SetActive(value: false);
		float num = 0.1f;
		Movements.instance.scale(end.transform.Find("LogoNStuff/SunHolder").gameObject, 0f, num);
		Image[] componentsInChildren = enviroment.GetComponentsInChildren<Image>();
		Text[] componentsInChildren2 = enviroment.GetComponentsInChildren<Text>();
		Image[] array = componentsInChildren;
		foreach (Image image in array)
		{
			Movements.instance.startFadeOut(image.gameObject, num, 0f);
		}
		Text[] array2 = componentsInChildren2;
		foreach (Text text in array2)
		{
			Movements.instance.startFadeOut(text.gameObject, num, 0f);
		}
		Movements.instance.executeWithDelay((Movements.Execute)MapController.instance.hidePlayer, num * 0.9f);
	}

	private IEnumerator hintTilt()
	{
		yield return new WaitForSeconds(UnityEngine.Random.Range(5f, 10f));
		StartCoroutine(hintTilt());
	}

	public void tiltHint()
	{
		GameObject gameObject = hint.transform.Find("Hint/icon/Question").gameObject;
		float num = 0f;
		Movements.instance.lerpColorTo(gameObject, Color.black, 0.1f, num);
		num += 0.1f;
		Movements.instance.lerpColorTo(gameObject, Color.white, 0.1f, num);
		num += 0.1f;
		Movements.instance.lerpColorTo(gameObject, Color.black, 0.1f, num);
		num += 0.1f;
		Movements.instance.lerpColorTo(gameObject, Color.white, 0.1f, num);
		num += 0.1f;
		Movements.instance.lerpColorTo(gameObject, Color.black, 0.1f, num);
		num += 0.1f;
		Movements.instance.lerpColorTo(gameObject, Color.white, 0.1f, num);
		num += 0.1f;
		Movements.instance.lerpColorTo(gameObject, Color.black, 0.1f, num);
		num += 0.1f;
		Movements.instance.lerpColorTo(gameObject, Color.white, 0.1f, num);
		num += 0.1f;
		Movements.instance.lerpColorTo(gameObject, Color.black, 0.1f, num);
		num += 0.1f;
		Movements.instance.lerpColorTo(gameObject, Color.white, 0.1f, num);
		num += 0.1f;
		Movements.instance.lerpColorTo(gameObject, Color.black, 0.1f, num);
		num += 0.1f;
		Movements.instance.lerpColorTo(gameObject, Color.white, 0.1f, num);
		num += 0.1f;
		Movements.instance.lerpColorTo(gameObject, Color.black, 0.1f, num);
		num += 0.1f;
		Movements.instance.lerpColorTo(gameObject, Color.white, 0.1f, num);
		num += 0.1f;
		SoundManager.instance.HintJiggle();
	}

	public void openShopAnimation()
	{
		shopPopUp.SetActive(value: true);
		shopPopUp.GetComponent<Animator>().enabled = true;
		shopPopUp.GetComponent<Animator>().Play("PopupOpen");
		fade.SetActive(value: true);
		fade.GetComponent<Image>().raycastTarget = true;
		Movements.instance.startFadeIn(fade, 0.3f, 0.7f);
		shopPopUp.transform.parent.SetAsLastSibling();
	}

	public void closeShopAnimation()
	{
		fade.GetComponent<Image>().raycastTarget = false;
		shopPopUp.GetComponent<Animator>().enabled = false;
		Movements.instance.scale(shopPopUp, Vector3.one, Vector3.zero, 0.3f);
		Movements.instance.startFadeOut(fade, 0.3f, 0f);
	}

	public void collectStarAnim()
	{
		Transform transform = BoardController.instance.starContainer.transform;
		dailyStar.transform.GetChild(0).gameObject.SetActive(value: true);
		Movements.instance.rotate(dailyStar, 0f, 360f, 0.5f);
		Movements.instance.move(dailyStar, dailyStar.transform.position, transform.position, 0.5f);
		Movements.instance.scale(dailyStar, Vector3.one, Vector3.one * 0.2f, 0.5f);
		UnityEngine.Object.Destroy(dailyStar, 0.55f);
		dailyStar = null;
		starCount++;
		PlayerPrefsManager.setDailyStar(starCount);
		updateStar();
	}

	public void updateStar()
	{
		dailyStartText.GetComponent<Text>().text = starCount.ToString();
		Movements.instance.scale(dailyStartText, Vector3.one, Vector3.one * 1.1f, 0.1f);
		Movements.instance.scale(dailyStartText, Vector3.one * 1.1f, Vector3.one, 0.1f, 0.1f);
	}

	public void onExtraCoinFull()
	{
		int num = 0;
		if (BoardController.daily)
		{
		}
		for (int i = 0; i < num; i++)
		{
			Transform transform = UnityEngine.Object.Instantiate(coinPrefab).transform;
			transform.SetParent(instance.shopPopUp.transform.parent);
			transform.transform.SetAsLastSibling();
			transform.position = star.transform.Find("Plus").position;
			transform.localScale = Vector3.one;
			CoinAnim component = transform.GetComponent<CoinAnim>();
			component.start = star.transform.Find("Plus").position;
			component.end = store.transform.Find("ShopButton").position;
			component.amount = 2;
			transform.gameObject.SetActive(value: false);
			Movements.instance.executeWithDelay((Movements.Execute)component.startParabolicMove, (float)i * 0.1f);
		}
	}

	public void enableExtra()
	{
		star.SetActive(value: true);
		Movements.instance.move(star, star.transform.position + Vector3.left * 3f, star.transform.position, initTime);
	}

	public void hideShop()
	{
		Movements.instance.move(store, store.transform.position, store.transform.position + Vector3.right * 3f, 0.4f);
	}

	public void tiltExtra()
	{
		Movements.instance.tilt(star, 3, 0.3f, 2f);
	}

	public void popRibbon()
	{
		UnityEngine.Object.Destroy(randomRibbon);
		UnityEngine.Random.InitState(DateTime.UtcNow.Millisecond);
		randomRibbon = UnityEngine.Object.Instantiate(ribbonPrefab);
		randomRibbon.transform.Find("Text").GetComponent<Text>().text = ribbonWords[UnityEngine.Random.Range(0, ribbonWords.Length)];
		randomRibbon.GetComponent<Image>().color = FugoUtils.HexToColor(BoardController.set.InGameRibbonColor);
		UnityEngine.Debug.Log("in game ribbon color  " + BoardController.set.InGameRibbonColor);
		randomRibbon.transform.SetParent(end.transform.parent);
		randomRibbon.transform.localPosition = Vector3.zero;
		randomRibbon.transform.localScale = Vector3.zero;
		Movements.instance.scale(randomRibbon, 1f, 0.2f);
		Movements.instance.executeWithDelay((Movements.Execute)disableRibbon, 0.5f);
	}

	public void disableRibbon()
	{
		float num = 0.3f;
		Movements.instance.startFadeOut(randomRibbon, num, 0f);
		Movements.instance.startFadeOut(randomRibbon.transform.Find("Text").gameObject, num, 0f);
		Movements.instance.move(randomRibbon, randomRibbon.transform.position, randomRibbon.transform.position + Vector3.down * 3f, num);
	}

	public void hideMapAndStuff()
	{
		float animTime = 0.2f;
		GameObject gameObject = end.transform.Find("LogoNStuff").gameObject;
		GameObject gameObject2 = gameObject.transform.Find("Logo").gameObject;
		GameObject gameObject3 = gameObject.transform.Find("SunHolder").gameObject;
		GameObject gameObject4 = gameObject.transform.Find("Progress").gameObject;
		Movements.instance.scale(gameObject2, gameObject2.transform.localScale, Vector3.zero, animTime);
		Movements.instance.scale(gameObject3, gameObject3.transform.localScale, Vector3.zero, animTime, 0.1f);
		Movements.instance.scale(gameObject4, gameObject4.transform.localScale, Vector3.zero, animTime, 0.2f);
		Movements.instance.scale(ObjectHolder.instance.buttonHolder, ObjectHolder.instance.buttonHolder.transform.localScale, Vector3.zero, animTime, 0.3f);
	}

	public void openNextPage()
	{
		nextPage.SetActive(value: true);
		nextPage.transform.SetAsLastSibling();
		GameObject gameObject = nextPage.transform.Find("Description").gameObject;
		if (UISwapper.flipGame)
		{
			ArabicText arabicText = gameObject.transform.Find("TopText").gameObject.AddComponent<ArabicText>();
			arabicText.Text = GameController.set.TopText;
			arabicText = gameObject.transform.Find("BotText").gameObject.AddComponent<ArabicText>();
			arabicText.Text = GameController.set.BottomText;
		}
		else
		{
			gameObject.transform.Find("TopText").GetComponent<Text>().text = GameController.set.TopText;
			gameObject.transform.Find("BotText").GetComponent<Text>().text = GameController.set.BottomText;
		}
		gameObject.transform.position += Vector3.right * 5f;
		gameObject.SetActive(value: true);
		Movements.instance.move(gameObject, gameObject.transform.position, gameObject.transform.position + Vector3.left * 5f, 0.3f);
		SoundManager.instance.blurReveal();
	}

	public void closeDescription()
	{
		GameObject gameObject = nextPage.transform.Find("Description").gameObject;
		Movements.instance.move(gameObject, gameObject.transform.position, gameObject.transform.position + Vector3.right * 9f, 0.1f);
	}
}
