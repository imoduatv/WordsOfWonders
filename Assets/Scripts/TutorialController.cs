using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using v2Gameplay;

public class TutorialController : MonoBehaviour
{
	public static TutorialController instance;

	public static bool freeHint;

	public GameObject hintbeacon;

	public GameObject shufflebeacon;

	public GameObject globalArrow;

	public GameObject tutorialContainer;

	public GameObject wheel;

	public GameObject board;

	public GameObject mid;

	public GameObject bot;

	public GameObject fade;

	public GameObject shop;

	public GameObject end;

	public bool waitingHint;

	public Transform megaHintButton;

	private Level game;

	private Transform tutorial;

	private bool running;

	private void Awake()
	{
		waitingHint = false;
		instance = this;
	}

	private void Start()
	{
		GameController gameController = GameController.instance;
		gameController.onNewGame = (GameController.SendGame)Delegate.Combine(gameController.onNewGame, new GameController.SendGame(handleNewGame));
		GameController gameController2 = GameController.instance;
		gameController2.onGameEnd = (GameController.SendGame)Delegate.Combine(gameController2.onGameEnd, new GameController.SendGame(handleEndGame));
		GameController gameController3 = GameController.instance;
		gameController3.onGameEnd = (GameController.SendGame)Delegate.Combine(gameController3.onGameEnd, new GameController.SendGame(handleEndAdventure));
	}

	private void handleNewGame(Level g)
	{
		running = true;
		game = g;
	}

	private void handleEndGame(Level g)
	{
		running = false;
		closeAll();
	}

	private void handleEndAdventure(Level g)
	{
		running = false;
	}

	private void handleWordFound(Word w)
	{
		closeAll();
	}

	public void openFirst()
	{
		if (!PlayerPrefsManager.GetTutorialDone(1))
		{
			WordController wordController = WordController.instance;
			wordController.onWordFound = (WordController.OnWordFound)Delegate.Combine(wordController.onWordFound, new WordController.OnWordFound(handleWordFound));
			PlayerPrefsManager.SetTutorialDone(1);
			TutorialHand.instance.init(game);
			TutorialHand.instance.transform.GetComponent<TrailRenderer>().enabled = true;
			tutorial = tutorialContainer.transform.Find("Tutorial1");
			setTargetText(game.words[0]);
			enableFade();
			board.transform.SetAsLastSibling();
			wheel.transform.SetAsLastSibling();
			Canvas component = wheel.GetComponent<Canvas>();
			component.sortingOrder = 2;
			tutorialContainer.transform.SetAsLastSibling();
			tutorial.localScale = Vector3.zero;
			tutorial.gameObject.SetActive(value: true);
			Movements.instance.scale(tutorial.gameObject, 1f, 0.3f);
			shop.transform.SetAsLastSibling();
		}
	}

	public void openSecond()
	{
		if (!PlayerPrefsManager.GetTutorialDone(2))
		{
			PlayerPrefsManager.SetTutorialDone(2);
			WheelController.running = false;
			tutorial = tutorialContainer.transform.Find("Tutorial2");
			enableFade();
			mid.transform.SetAsLastSibling();
			Movements.instance.executeWithDelay((Movements.Execute)moveArrow1, 0.5f);
			tutorial.localScale = Vector3.zero;
			tutorial.gameObject.SetActive(value: true);
			Movements.instance.scale(tutorial.gameObject, 1f, 0.3f);
			shop.transform.SetAsLastSibling();
		}
	}

	public void openThird()
	{
		if (!PlayerPrefsManager.GetTutorialDone(3))
		{
			PlayerPrefsManager.SetTutorialDone(3);
			waitingHint = true;
			WheelController.running = false;
			freeHint = true;
			tutorial = tutorialContainer.transform.Find("Tutorial3");
			GameObject gameObject = tutorialContainer.transform.Find("Tutorial3/Right").gameObject;
			gameObject.transform.position = hintbeacon.transform.position;
			Movements.instance.executeWithDelay((Movements.Execute)moveArrow2, 1.35f);
			enableFade();
			mid.transform.SetAsLastSibling();
			tutorial.localScale = Vector3.zero;
			tutorial.gameObject.SetActive(value: true);
			Movements.instance.scale(tutorial.gameObject, 1f, 0.3f);
			shop.transform.SetAsLastSibling();
			GameMenuController.instance.updateHintPrice();
		}
	}

	public void openExtra()
	{
		if (!PlayerPrefsManager.GetTutorialDone(4))
		{
			PlayerPrefsManager.SetTutorialDone(4);
			tutorial = tutorialContainer.transform.Find("TutorialExtra");
			GameObject gameObject = bot.transform.Find("PlusButtonContainer/Arrow").gameObject;
			Movements.instance.startFadeIn(gameObject, 0.3f, 1f);
			tiltArrow();
			enableFade();
			bot.transform.SetAsLastSibling();
			board.transform.SetAsFirstSibling();
			tutorial.localScale = Vector3.zero;
			tutorial.gameObject.SetActive(value: true);
			Movements.instance.scale(tutorial.gameObject, 1f, 0.3f);
			shop.transform.SetAsLastSibling();
		}
	}

	public void openShop()
	{
		if (!PlayerPrefsManager.GetTutorialDone(5))
		{
			PlayerPrefsManager.SetTutorialDone(5);
			GameAnimController.instance.star.transform.Find("Plus").GetComponent<Button>().interactable = false;
			GameAnimController.instance.store.transform.Find("ShopButton").GetComponent<Button>().interactable = false;
			tutorial = tutorialContainer.transform.Find("TutorialShop");
			enableFade();
			bot.transform.SetAsLastSibling();
			tutorial.localScale = Vector3.zero;
			tutorial.gameObject.SetActive(value: true);
			Movements.instance.scale(tutorial.gameObject, 1f, 0.3f);
			shop.transform.SetAsLastSibling();
			end.transform.SetAsFirstSibling();
			tutorialContainer.transform.SetAsLastSibling();
		}
	}

	private void openMegaButton()
	{
	}

	public void openMega()
	{
		if (!PlayerPrefsManager.GetTutorialDone(8))
		{
			PlayerPrefsManager.SetTutorialDone(8);
			tutorial = tutorialContainer.transform.Find("TutorialMega");
			enableFade();
			tutorial.localScale = Vector3.zero;
			tutorial.gameObject.SetActive(value: true);
			Vector3 vector = Vector3.one;
			if (UISwapper.flipGame)
			{
				vector = new Vector3(-1f, 1f, 1f);
			}
			Movements.instance.scale(tutorial.gameObject, Vector3.zero, vector, 0.5f);
			fade.transform.SetAsLastSibling();
			tutorialContainer.transform.SetAsLastSibling();
			StartCoroutine(closeWait());
		}
	}

	private IEnumerator closeWait()
	{
		bool flag = true;
		float cooldown = 2f;
		while (flag)
		{
			if (Input.GetMouseButtonDown(0) && cooldown < 0f)
			{
				closeAll();
				flag = false;
			}
			cooldown -= Time.deltaTime;
			yield return null;
		}
	}

	public void openBonus()
	{
	}

	public void openDaily()
	{
		if (!PlayerPrefsManager.GetTutorialDone(7))
		{
			PlayerPrefsManager.SetTutorialDone(7);
			GameObject gameObject = GameAnimController.dailyStar.transform.parent.parent.gameObject;
			gameObject.transform.SetAsLastSibling();
			bool flag = false;
			string newValue = string.Empty;
			foreach (Word word in GameController.game.words)
			{
				GameObject[] letters = word.letters;
				foreach (GameObject y in letters)
				{
					flag = (gameObject == y);
					if (flag)
					{
						newValue = word.word;
						break;
					}
				}
				if (flag)
				{
					break;
				}
			}
			tutorial = tutorialContainer.transform.Find("TutorialDaily");
			string text = AutoLanguage.dict["TutorialDaily"].Replace("%@", newValue);
			if (PlayerPrefsManager.GetLang() == SystemLanguage.Hebrew.ToString())
			{
			}
			board.transform.SetAsLastSibling();
			GameObject gameObject2 = board.transform.Find("Scale/Arrow").gameObject;
			gameObject2.transform.localScale = Vector3.one * BoardController.scaleAmount;
			gameObject2.transform.localScale = Vector3.one * 0.5f;
			gameObject2.transform.position = GameAnimController.dailyStar.transform.position;
			gameObject2.GetComponent<RectTransform>().anchoredPosition += Vector2.down * BoardController.cellSize;
			Movements.instance.startFadeIn(gameObject2, 0.3f, 1f);
			tiltArrow();
			enableFade();
			bot.transform.SetAsFirstSibling();
			tutorial.localScale = Vector3.zero;
			tutorial.gameObject.SetActive(value: true);
			Movements.instance.scale(tutorial.gameObject, 1f, 0.3f);
			PlayerPrefsManager.FirstDailyDone();
			shop.transform.SetAsFirstSibling();
			tutorial.transform.Find("Description").GetComponent<Text>().SetText(AutoLanguage.dict["TutorialDaily"].Replace("%@", newValue));
			gameObject2.transform.SetAsLastSibling();
			tutorialContainer.transform.SetAsLastSibling();
		}
	}

	public void tiltArrow()
	{
		GameObject gameObject = bot.transform.Find("PlusButtonContainer/Arrow").gameObject;
		GameObject gameObject2 = board.transform.Find("Scale/Arrow").gameObject;
		Movements.instance.tiltVertical(gameObject, 3, 2.5f, 5f);
		Movements.instance.tiltVertical(gameObject2, 3, 2.5f, 5f);
		Movements.instance.executeWithDelay((Movements.Execute)tiltArrow, 5f);
	}

	public void closeAll()
	{
		v2Gameplay.WheelController.instance.destroyCanvas();
		tutorialContainer.transform.Find("TutorialShop").gameObject.SetActive(value: false);
		GameAnimController.instance.star.transform.Find("Plus").GetComponent<Button>().interactable = true;
		GameAnimController.instance.store.transform.Find("ShopButton").GetComponent<Button>().interactable = true;
		SoundManager.instance.Click();
		shop.transform.SetAsLastSibling();
		if (!tutorialContainer.transform.Find("TutorialShop").gameObject.activeSelf)
		{
			WheelController.running = true;
		}
		disableFade();
		GameObject gameObject = bot.transform.Find("PlusButtonContainer/Arrow").gameObject;
		GameObject gameObject2 = board.transform.Find("Scale/Arrow").gameObject;
		tutorialContainer.transform.Find("Tutorial1").gameObject.SetActive(value: false);
		tutorialContainer.transform.Find("Tutorial2").gameObject.SetActive(value: false);
		tutorialContainer.transform.Find("Tutorial3").gameObject.SetActive(value: false);
		tutorialContainer.transform.Find("TutorialExtra").gameObject.SetActive(value: false);
		tutorialContainer.transform.Find("TutorialDaily").gameObject.SetActive(value: false);
		tutorialContainer.transform.Find("TutorialMega").gameObject.SetActive(value: false);
		Movements.instance.startFadeOut(gameObject, 0.3f, 0f);
		Movements.instance.startFadeOut(gameObject2, 0.3f, 0f);
		Movements.instance.startFadeOut(globalArrow, 0.3f, 0f);
		TutorialHand.instance.disableHand();
		if (running)
		{
			TouchController.instance.enable();
		}
	}

	private Word getNextWord()
	{
		for (int i = 0; i < game.words.Count; i++)
		{
			if (!game.words[i].found)
			{
				return game.words[i];
			}
		}
		return null;
	}

	public void setTargetText(Word w)
	{
		w = getNextWord();
		if (UISwapper.flipGame)
		{
			GameObject gameObject = tutorialContainer.transform.Find("Tutorial1/Description").gameObject;
			ArabicText arabicText = gameObject.GetComponent<ArabicText>();
			if (arabicText == null)
			{
				arabicText = gameObject.AddComponent<ArabicText>();
			}
			arabicText.Text = AutoLanguage.dict["Tutorial1"].Replace("%@", w.word);
		}
		else
		{
			tutorialContainer.transform.Find("Tutorial1/Description").GetComponent<Text>().text = AutoLanguage.dict["Tutorial1"].Replace("%@", w.word);
		}
		TutorialHand.instance.findWord(w);
	}

	public void enableFade()
	{
		Movements.instance.startFadeIn(fade, 0.3f, 0.8f);
		fade.GetComponent<Image>().raycastTarget = true;
	}

	public void disableFade()
	{
		Movements.instance.startFadeOut(fade, 0.3f, 0f);
		fade.GetComponent<Image>().raycastTarget = false;
	}

	public void moveArrow1()
	{
		GameObject gameObject = tutorialContainer.transform.Find("Tutorial2/Left").gameObject;
		gameObject.transform.position = shufflebeacon.transform.position + Vector3.right * 0.1f;
		Movements.instance.startFadeIn(gameObject, 0.2f, 1f);
		Movements.instance.reverseTilt(gameObject, 3, 2.5f, 4f);
		Movements.instance.executeWithDelay((Movements.Execute)moveArrow1, 5f);
	}

	public void moveArrow2()
	{
		GameObject gameObject = tutorialContainer.transform.Find("Tutorial3/Right").gameObject;
		gameObject.transform.position = hintbeacon.transform.position + Vector3.left * 0.15f;
		Movements.instance.startFadeIn(gameObject, 0.2f, 1f);
		Movements.instance.reverseTilt(gameObject, 3, 2.5f, 4f);
		Movements.instance.executeWithDelay((Movements.Execute)moveArrow2, 5f);
	}

	public void hintClicked()
	{
		if (waitingHint)
		{
			closeAll();
			waitingHint = false;
			Movements.instance.executeWithDelay((Movements.Execute)mid.transform.SetAsFirstSibling, 0.5f);
		}
	}
}
