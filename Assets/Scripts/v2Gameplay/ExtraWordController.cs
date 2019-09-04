using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace v2Gameplay
{
	public class ExtraWordController : MonoBehaviour
	{
		public static ExtraWordController instance;

		public Transform wordContainer;

		public GameObject extrawords;

		public GameObject wordPrefab;

		public GameObject coinPrefab;

		public Image fadeExtra;

		public Text countText;

		private Level game;

		private bool extra;

		private int extraCount;

		private Coroutine extraScale;

		private Coroutine extraFade;

		private void Awake()
		{
			instance = this;
		}

		private void Start()
		{
			extra = false;
			WordController wordController = WordController.instance;
			wordController.onExtraFound = (WordController.OnExtraFound)Delegate.Combine(wordController.onExtraFound, new WordController.OnExtraFound(handleExtraFound));
			GameController gameController = GameController.instance;
			gameController.onNewGame = (GameController.SendGame)Delegate.Combine(gameController.onNewGame, new GameController.SendGame(handleNewGame));
			GameController gameController2 = GameController.instance;
			gameController2.onGameEnd = (GameController.SendGame)Delegate.Combine(gameController2.onGameEnd, new GameController.SendGame(handleGameEnd));
			GameController gameController3 = GameController.instance;
			gameController3.onTournamentEnd = (GameController.SendGame)Delegate.Combine(gameController3.onTournamentEnd, new GameController.SendGame(handleGameEnd));
		}

		private void handleNewGame(Level g)
		{
			if (PlayerPrefsManager.GetLang() == SystemLanguage.Hebrew.ToString())
			{
			}
			g.foundExtras = new List<string>();
			if (game != null && game.foundExtras != null)
			{
				foreach (string foundExtra in game.foundExtras)
				{
					if (!g.foundExtras.Contains(foundExtra))
					{
						g.foundExtras.Add(foundExtra);
					}
					UnityEngine.Debug.Log(foundExtra);
				}
			}
			game = g;
			if (rewardable())
			{
				Movements.instance.executeWithDelay((Movements.Execute)loadExtras, 0.2f);
			}
			else
			{
				countText.gameObject.SetActive(value: false);
				extraCount = 0;
			}
			updateCount();
			GameController gameController = GameController.instance;
			gameController.onNewGame = (GameController.SendGame)Delegate.Remove(gameController.onNewGame, new GameController.SendGame(handleNewGame));
		}

		private void handleGameEnd(Level g)
		{
			if (rewardable())
			{
				int extraWordCount = PlayerPrefsManager.GetExtraWordCount(game);
				AnimController.waitGemTime = 0.05f * (float)extraWordCount;
				for (int i = 0; i < extraWordCount; i++)
				{
					Movements.instance.executeWithDelay((Movements.Execute)spawnCoin, (float)i * 0.05f);
				}
				SaveLoad.ResetExtraWordCount(game);
			}
		}

		private void handleExtraFound(string word)
		{
			if (!PlayerPrefsManager.GetFirstExtra())
			{
				TutorialController.instance.openExtra();
				PlayerPrefsManager.OnFirstExtraFound();
			}
			if (!game.foundExtras.Contains(word))
			{
				game.foundExtras.Add(word);
			}
			addWord(word);
			if (rewardable())
			{
				SaveLoad.IncreaseExtraWordCount(game);
				SaveLoad.SetFoundExtras(game.foundExtras, game);
			}
			extraCount++;
			updateCount();
		}

		public void closeExtraWords()
		{
			if (extra)
			{
				toggleExtraWords();
			}
		}

		public void toggleExtraWords()
		{
			ObjectHolder.instance.botHolder.transform.SetAsLastSibling();
			SoundManager.instance.Click();
			float num = 0.2f;
			extra = !extra;
			fadeExtra.raycastTarget = extra;
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
				extraFade = Movements.instance.startFadeIn(fadeExtra.gameObject, num, 0.3f);
			}
			else
			{
				Movements.instance.executeWithDelay((Movements.Execute)SoundManager.instance.SlideOut, 0.1f);
				extraScale = Movements.instance.scale(extrawords, extrawords.transform.localScale, Vector3.zero, num);
				extraFade = Movements.instance.startFadeOut(fadeExtra.gameObject, num, 0f);
			}
		}

		private void loadExtras()
		{
			List<string> foundExtras = SaveLoad.GetFoundExtras(game);
			extraCount = 0;
			foreach (string item in foundExtras)
			{
				if (item != string.Empty)
				{
					addWord(item);
					UnityEngine.Debug.Log(item);
					if (!game.foundExtras.Contains(item))
					{
						game.foundExtras.Add(item);
					}
				}
			}
			extraCount = game.foundExtras.Count;
			updateCount();
			if (rewardable())
			{
				SaveLoad.SetFoundExtras(game.foundExtras, game);
			}
		}

		private void addWord(string word)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(wordPrefab);
			gameObject.transform.parent = wordContainer.transform;
			gameObject.transform.localScale = Vector3.one;
			if (PlayerPrefsManager.GetLang() == SystemLanguage.Hebrew.ToString())
			{
				word = word.Reverse();
			}
			if (PlayerPrefsManager.GetLang() == "Arabic")
			{
				ArabicText arabicText = gameObject.transform.Find("Word").gameObject.AddComponent<ArabicText>();
				arabicText.Text = word;
			}
			else if (PlayerPrefsManager.GetLang() == SystemLanguage.Hebrew.ToString())
			{
				UnityEngine.Debug.Log(word);
				gameObject.transform.Find("Word").GetComponent<Text>().text = word.Reverse();
			}
			else
			{
				gameObject.transform.Find("Word").GetComponent<Text>().text = word;
			}
		}

		private void updateCount()
		{
			string text = extraCount.ToString();
			countText.text = text;
		}

		private void spawnCoin()
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(coinPrefab);
			gameObject.transform.SetParent(ObjectHolder.instance.shopButton.transform);
			gameObject.transform.ResetTransform();
			CoinAnim component = gameObject.GetComponent<CoinAnim>();
			component.start = ObjectHolder.instance.starButton.transform.Find("Plus").position;
			component.end = ObjectHolder.instance.shopButton.transform.Find("ShopButton").position;
			component.startParabolicMove();
		}

		public bool rewardable()
		{
			if (game.mode == GameMode.NORMAL)
			{
				if (GameController.levelToOpen == -1)
				{
					return true;
				}
				return false;
			}
			if (game.mode == GameMode.DAILY)
			{
				return true;
			}
			if (game.mode == GameMode.ADVENTURE)
			{
				if (GameController.slidePos != 0)
				{
					return true;
				}
				return false;
			}
			return false;
		}
	}
}
