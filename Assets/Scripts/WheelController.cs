using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WheelController : MonoBehaviour
{
	public static WheelController instance;

	public static string letters = "ASDFG";

	public static Level level;

	public static Color letterColor;

	public static Color selectedLetterColor;

	public static Color gameColor;

	public static Color selectedLetterBGColor;

	public static bool running;

	public static bool locked;

	public static float range = 140f;

	public static float letterScale;

	public static float letterSize;

	public GameObject letterTemp;

	public GameObject lineTemp;

	public GameObject pieceTemp;

	public GameObject wordContiner;

	public GameObject wordUI;

	public GameObject textAnim;

	public GameObject starButton;

	public GameObject extraWordContainer;

	public GameObject extraPrefab;

	public GameObject barHolder;

	public List<Word> words;

	public List<Word> foundWords;

	private static float hintTime = 40f;

	private float hintTimer;

	private Transform wordList;

	private RectTransform wordBG;

	private RectTransform listRect;

	private Line line;

	private Transform lineContainer;

	private Text wordText;

	private string word;

	private Vector3[] positions;

	private List<Letter> letterlist;

	private List<string> extras;

	private List<string> foundExtras;

	private string newExtra;

	private Transform letterContainer;

	private static string lastWord;

	private void Awake()
	{
		FugoAdManager.instance.ShowBanner();
		hintTimer = -5f;
		instance = this;
		lineContainer = base.transform.Find("LineContainer").transform;
		wordBG = wordContiner.transform.Find("WordBG").GetComponent<RectTransform>();
		wordList = wordContiner.transform.Find("Word");
		listRect = wordContiner.transform.Find("Word").GetComponent<RectTransform>();
		letterContainer = base.transform.Find("LetterContainer");
	}

	private void Update()
	{
		try
		{
		}
		catch (Exception)
		{
		}
		if (Input.GetMouseButtonUp(0) && running)
		{
			checkWord();
		}
		if (Input.GetMouseButton(0) || true)
		{
			calculateWordBackground();
		}
		hintTimer += Time.deltaTime;
		if (hintTimer > hintTime)
		{
			if (PlayerPrefsManager.GetLevel() >= 5)
			{
				GameAnimController.instance.tiltHint();
			}
			hintTimer = 0f;
		}
	}

	private void loadGame()
	{
		if (BoardController.levelToOpen != -1)
		{
			return;
		}
		List<string> list = null;
		int[,] array = null;
		if (!BoardController.daily)
		{
			if (BoardController.levelToOpen == -1)
			{
				list = PlayerPrefsManager.getFoundWords();
				foundExtras = PlayerPrefsManager.getFoundExtras();
				array = PlayerPrefsManager.GetHints();
			}
		}
		else
		{
			list = PlayerPrefsManager.getFoundWordsDaily();
			foundExtras = PlayerPrefsManager.getFoundExtrasDaily();
			array = PlayerPrefsManager.GetHintsDaily();
		}
		Color hintColor = FugoUtils.HexToColor(BoardController.set.InGameHintColor);
		for (int i = 0; i < array.GetLength(0); i++)
		{
			try
			{
				BoardController.instance.board[array[i, 0], array[i, 1]].GetComponent<Cell>().enableHint(hintColor);
			}
			catch (Exception)
			{
			}
		}
		foreach (string item in list)
		{
		}
		for (int j = 0; j < list.Count; j++)
		{
			for (int k = 0; k < words.Count; k++)
			{
				string text = list[j];
				Word word = words[k];
				if (!(text == string.Empty) && word.word == text)
				{
					word.enableWord();
					words.Remove(word);
					foundWords.Add(word);
					k--;
				}
			}
		}
		for (int l = 0; l < foundExtras.Count; l++)
		{
			for (int m = 0; m < extras.Count; m++)
			{
				string text2 = foundExtras[l];
				string text3 = extras[m];
				if (!(text2 == string.Empty) && text3 == text2)
				{
					extras.Remove(text3);
					m--;
					GameObject gameObject = UnityEngine.Object.Instantiate(extraPrefab);
					gameObject.transform.parent = extraWordContainer.transform;
					gameObject.transform.localScale = Vector3.one;
					gameObject.transform.Find("Word").GetComponent<Text>().text = text3;
				}
			}
		}
	}

	public void newGame()
	{
		updateExtraCount();
		locked = false;
		running = true;
		foundWords = new List<Word>();
		words = new List<Word>();
		extras = new List<string>();
		foundExtras = new List<string>();
		level = BoardController.level;
		foreach (Word word2 in level.words)
		{
			words.Add(word2);
		}
		string[] otherWords = level.otherWords;
		foreach (string item in otherWords)
		{
			extras.Add(item);
		}
		letterColor = FugoUtils.HexToColor(BoardController.set.InGameLetterColor);
		selectedLetterColor = FugoUtils.HexToColor(BoardController.set.InGameSelectedLetterColor);
		gameColor = FugoUtils.HexToColor(BoardController.set.TitleColor);
		selectedLetterBGColor = FugoUtils.HexToColor(BoardController.set.InGameSelectedLetterBGColor);
		wordText = wordUI.GetComponent<Text>();
		word = string.Empty;
		letterlist = new List<Letter>();
		float num = 360f / (float)level.letters.Length;
		positions = new Vector3[level.letters.Length];
		letterContainer.parent.localScale = Vector3.zero;
		letterScale = 1f;
		if (level.letters.Length > 4)
		{
			letterScale = 4.5f / (float)level.letters.Length;
			range = 140f + (1f - letterScale) * BoardController.cellSize * 0.75f;
			if (SafeAreaScaler.scale < 1f)
			{
				range *= SafeAreaScaler.scale * 1.2f;
				letterScale *= SafeAreaScaler.scale * 1.2f;
			}
		}
		else
		{
			letterScale = 1f;
			range = 140f;
			if (SafeAreaScaler.scale < 1f)
			{
				letterScale *= SafeAreaScaler.scale;
			}
		}
		string text = "preferences failed";
		try
		{
			text = PlayerPrefs.GetString("found_words", string.Empty);
		}
		catch (Exception)
		{
		}
		try
		{
			loadGame();
		}
		catch (Exception ex2)
		{
			UnityEngine.Debug.Log("load game fail:  " + ex2.ToString());
		}
		calculateRange();
		range *= 0.965f;
		for (int j = 0; j < level.letters.Length; j++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(letterTemp);
			gameObject.GetComponent<RectTransform>().sizeDelta = Vector2.one * letterSize;
			positions[j].x = range * Mathf.Sin(num * (float)j * (float)Math.PI / 180f);
			positions[j].y = range * Mathf.Cos(num * (float)j * (float)Math.PI / 180f);
			positions[j].y += 3f;
			positions[j].z = 0f;
			gameObject.transform.parent = letterContainer;
			gameObject.transform.localScale = Vector3.one;
			gameObject.transform.localPosition = positions[j];
			gameObject.GetComponent<Letter>().setLetter(level.letters[j].ToString());
			gameObject.transform.Find("Text").GetComponent<Text>().color = letterColor;
		}
		Color color = FugoUtils.HexToColor(BoardController.set.InGameCircleColor);
		GetComponent<Image>().color = color;
		if (PlayerPrefsManager.GetLevel() != 1)
		{
			UnityEngine.Object.Destroy(GetComponent<GraphicRaycaster>());
			UnityEngine.Object.Destroy(GetComponent<Canvas>());
		}
		try
		{
			killCells();
		}
		catch (Exception)
		{
		}
	}

	private void calculateRange()
	{
		Vector2 sizeDelta = GetComponent<RectTransform>().sizeDelta;
		range = sizeDelta.y * 0.5f;
		float num = (float)Math.PI * 2f / (float)level.letters.Length;
		float num2 = 1f + 1f / Mathf.Sin(num * 0.5f);
		letterSize = 2f * range / num2;
		float num3 = -4f / 75f * (float)level.letters.Length + 1.48f;
		letterSize /= num3;
		range -= letterSize * 0.5f;
		float num4 = 0.9f;
		range += letterSize * (1f - num4) * 0.35f;
		letterSize *= num4;
	}

	private void enableHand()
	{
		TutorialHand.instance.findWord(words[0]);
	}

	private void saveGame()
	{
		if (running)
		{
			if (BoardController.daily)
			{
				PlayerPrefsManager.setFoundWordsDaily(foundWords);
				PlayerPrefsManager.setFoundExtrasDaily(foundExtras);
			}
			else if (BoardController.levelToOpen == -1)
			{
				PlayerPrefsManager.setFoundWords(foundWords);
				PlayerPrefsManager.setFoundExtras(foundExtras);
			}
		}
	}

	public void checkWord()
	{
		if (word.Length == 0)
		{
			return;
		}
		try
		{
			if (running)
			{
				checkLevelEnd();
			}
		}
		catch (Exception)
		{
		}
		SoundManager.instance.ResetSelectIndex();
		bool flag = false;
		foreach (Word foundWord in foundWords)
		{
			if (foundWord.word == word)
			{
				flag = true;
				sameWord(foundWord);
				break;
			}
		}
		foreach (Word word2 in words)
		{
			if (word2.word == word)
			{
				flag = true;
				StartCoroutine(wordFound(word2));
				break;
			}
		}
		if (!flag)
		{
			if (foundExtras.Contains(word))
			{
				sameExtra();
			}
			bool flag2 = false;
			for (int i = 0; i < extras.Count; i++)
			{
				string a = extras[i];
				if (a == word)
				{
					extras.Remove(word);
					foundExtras.Add(word);
					flag2 = true;
					break;
				}
			}
			if (flag2)
			{
				extraFound(word);
				UnityEngine.Debug.Log("extra");
			}
			else
			{
				UnityEngine.Debug.Log("not found");
				notfound(word);
			}
		}
		word = string.Empty;
		for (int j = 0; j < lineContainer.childCount; j++)
		{
			UnityEngine.Object.Destroy(lineContainer.GetChild(j).gameObject);
		}
		foreach (Letter item in letterlist)
		{
			item.selected(s: false);
			item.used = false;
		}
		letterlist.Clear();
		try
		{
			if (running)
			{
				checkLevelEnd();
			}
		}
		catch (Exception)
		{
		}
		locked = true;
		Movements.instance.executeWithDelay((Movements.Execute)unlock, 0.6f);
		if (BoardController.levelToOpen == PlayerPrefsManager.GetLevel() || BoardController.levelToOpen == -1)
		{
			saveGame();
		}
	}

	private void unlock()
	{
		locked = false;
	}

	private void sameExtra()
	{
		GameAnimController.instance.tiltExtra();
	}

	private void extraFound(string w)
	{
		if (!(w == string.Empty))
		{
			disableCoins();
			SoundManager.instance.WordExtra();
			newExtra = w;
			if (PlayerPrefsManager.GetFirstExtra())
			{
				spawnExtra();
			}
			else
			{
				Movements.instance.executeWithDelay((Movements.Execute)TutorialController.instance.openExtra, 0.3f);
				GameAnimController.instance.enableExtra();
				Movements.instance.executeWithDelay((Movements.Execute)spawnExtra, GameAnimController.initTime);
				PlayerPrefsManager.OnFirstExtraFound();
			}
			Movements.instance.scale(wordContiner, Vector3.one * 0.9f, Vector3.one, 0.15f);
			Movements.instance.scale(wordContiner, Vector3.one, Vector3.one * 0.9f, 0.15f, 0.15f);
			for (int i = 0; i < wordList.childCount; i++)
			{
				GameObject gameObject = wordList.transform.GetChild(i).gameObject;
				UnityEngine.Object.Destroy(gameObject, 0.32f);
			}
			GameObject gameObject2 = UnityEngine.Object.Instantiate(extraPrefab);
			gameObject2.transform.parent = extraWordContainer.transform;
			gameObject2.transform.localScale = Vector3.one;
			gameObject2.transform.Find("Word").GetComponent<Text>().text = w;
		}
	}

	public void spawnExtra()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(textAnim);
		gameObject.GetComponent<Text>().text = newExtra;
		gameObject.GetComponent<Text>().color = FugoUtils.HexToColor(BoardController.set.InGameSelectedLetterColor);
		gameObject.transform.parent = base.transform;
		gameObject.transform.position = wordContiner.transform.position;
		ExtraWord component = gameObject.GetComponent<ExtraWord>();
		component.transform.localScale = Vector3.one;
		component.start = gameObject.transform.position;
		component.end = starButton.transform.position;
		component.startMove();
		checkExtraCoin();
	}

	public bool isItEnd()
	{
		GameObject[,] board = BoardController.instance.board;
		int length = board.GetLength(0);
		int length2 = board.GetLength(1);
		for (int i = 0; i < length; i++)
		{
			for (int j = 0; j < length2; j++)
			{
				GameObject gameObject = board[i, j];
				if (!(gameObject == null) && !gameObject.transform.GetComponent<Cell>().active)
				{
					return false;
				}
			}
		}
		return true;
	}

	public void checkLevelEnd()
	{
		if (isItEnd())
		{
			endGame();
		}
		else if (running)
		{
			checkAllWords();
			if (words.Count <= 1 && (words.Count != 1 || words[0].bonus))
			{
				endGame();
			}
		}
	}

	private static void endGame()
	{
		if (!running)
		{
			return;
		}
		if (BoardController.levelToOpen == -1)
		{
			GameAnimController.instance.onExtraCoinFull();
		}
		GameAnimController.instance.extrawords.SetActive(value: false);
		Movements.instance.executeWithDelay((Movements.Execute)GameAnimController.instance.endGame, 1f);
		if (BoardController.levelToOpen == -1 && !BoardController.daily)
		{
			PlayerPrefsManager.SetBrilliance(PlayerPrefsManager.GetBrilliance() + 1);
		}
		BoardController.instance.logLevelEnd();
		GameAnimController.instance.extra = true;
		GameAnimController.instance.toggleExtraWords();
		TutorialController.instance.closeAll();
		SoundManager.instance.GameWin();
		if (BoardController.daily)
		{
			UnityEngine.Debug.Log("daily end");
		}
		else if (BoardController.levelToOpen != -1)
		{
			BoardController.levelToOpen++;
		}
		else
		{
			int num = PlayerPrefsManager.GetLevel();
			PlayerPrefsManager.SetLatestLevel(PlayerPrefsManager.GetLevel());
			PlayerPrefsManager.SetLevel(PlayerPrefsManager.GetLevel() + 1);
			FirebaseController.SendLevelLog();
			if (BoardController.levelToOpen == -1)
			{
				PlayerPrefsManager.ResetProHint();
			}
			RequestManager.instance.LogRequest();
		}
		running = false;
	}

	private IEnumerator wordFound(Word w)
	{
		w.found = true;
		foreach (Word foundWord in foundWords)
		{
			if (!(foundWord.word == w.word))
			{
			}
		}
		if (foundWords.Contains(w))
		{
			sameWord(w);
			UnityEngine.Debug.Log("same");
		}
		else
		{
			SoundManager.instance.WordFound();
			lastWord = w.word;
			w.checkStar();
			hintTimer = -5f;
			foundWords.Add(w);
			words.Remove(w);
			try
			{
				TutorialController.instance.setTargetText(instance.words[0]);
			}
			catch (Exception)
			{
				TutorialHand.instance.disableHand();
			}
			Vector3 vector3 = (Vector3)(BoardController.offsetX + BoardController.offsetY);
			Movements.instance.scale(wordContiner, Vector3.one * 0.9f, Vector3.one, 0.1f);
			Movements.instance.scale(wordContiner, Vector3.one, Vector3.one * 0.9f, 0.1f, 0.1f);
			GameObject copyWord = UnityEngine.Object.Instantiate(wordContiner);
			copyWord.SetActive(value: false);
			yield return new WaitForSeconds(0f);
			copyWord.transform.SetParent(wordContiner.transform.parent);
			copyWord.transform.SetAsFirstSibling();
			int count = 0;
			copyWord.SetActive(value: true);
			copyWord.transform.position = wordContiner.transform.position;
			copyWord.transform.localScale = wordContiner.transform.localScale;
			copyWord.transform.SetAsFirstSibling();
			Transform copyList = copyWord.transform.Find("Word");
			List<Transform> cellList = new List<Transform>();
			for (int i = 0; i < w.length; i++)
			{
				GameObject gameObject = copyList.GetChild(0).gameObject;
				Piece component = gameObject.GetComponent<Piece>();
				component.start = gameObject.transform.position;
				Vector3 lossyScale = gameObject.transform.lossyScale;
				Vector3 position = gameObject.transform.position;
				gameObject.transform.parent = null;
				UnityEngine.Object.Destroy(gameObject.GetComponent<ContentSizeFitter>());
				RectTransform component2 = gameObject.GetComponent<RectTransform>();
				Vector2 sizeDeltum = component2.sizeDelta;
				Vector2 vector2 = component2.sizeDelta = Vector2.one * BoardController.cellSize;
				component2.pivot = new Vector2(0f, 1f);
				component2.anchorMax = new Vector2(0f, 1f);
				component2.anchorMin = new Vector2(0f, 1f);
				gameObject.transform.localScale = lossyScale;
				gameObject.transform.position = position;
				Movements.instance.startFadeIn(gameObject, 0.1f, 1f);
				gameObject.transform.SetParent(w.letters[i].transform);
				cellList.Add(gameObject.transform.parent);
				component.start = gameObject.transform.position;
				gameObject.SetActive(value: true);
				Transform child = wordContiner.transform.Find("Word").GetChild(i);
				gameObject.GetComponent<RectTransform>().sizeDelta = child.GetComponent<RectTransform>().sizeDelta;
				gameObject.transform.position = child.position;
				w.letters[i].transform.SetAsLastSibling();
				component.start = gameObject.transform.position;
				component.end = w.letters[i].transform.position;
				component.endScale = BoardController.scaleAmount * Vector3.one;
				float time = (float)i * 0.05f + 0.2f;
				Movements.instance.executeWithDelay((Movements.Execute)component.startMove, time);
				count = i;
			}
			for (int j = 0; j < cellList.Count; j++)
			{
				int index = cellList.Count - j - 1;
				cellList[index].SetAsLastSibling();
			}
			float fadeOutTime2 = (float)count * 0.05f + 0.2f;
			UnityEngine.Object.Destroy(copyWord, fadeOutTime2 + 0.1f);
			Movements.instance.executeWithDelay((Movements.Execute)disableCoins, fadeOutTime2 + 0.4f);
			Movements.instance.startFadeOut(wordContiner.transform.Find("WordBG/Left").gameObject, fadeOutTime2, 0f);
			Movements.instance.startFadeOut(wordContiner.transform.Find("WordBG/Right").gameObject, fadeOutTime2, 0f);
			Movements.instance.startFadeOut(wordContiner.transform.Find("WordBG/mid").gameObject, fadeOutTime2, 0f);
			Movements.instance.startFadeOut(copyWord.transform.Find("WordBG/Left").gameObject, fadeOutTime2, 0f);
			Movements.instance.startFadeOut(copyWord.transform.Find("WordBG/Right").gameObject, fadeOutTime2, 0f);
			Movements.instance.startFadeOut(copyWord.transform.Find("WordBG/mid").gameObject, fadeOutTime2, 0f);
			GameObject letterContiner = wordContiner.transform.Find("Word").gameObject;
			for (int k = 0; k < letterContiner.transform.childCount; k++)
			{
				Movements.instance.startFadeOut(letterContiner.transform.GetChild(k).gameObject, fadeOutTime2, 0f);
				UnityEngine.Object.Destroy(letterContiner.transform.GetChild(k).gameObject, fadeOutTime2);
			}
			fadeOutTime2 += 0.1f;
			UnityEngine.Object.Destroy(copyWord, fadeOutTime2 + 0.02f);
			if (words.Count == 1)
			{
				Movements.instance.executeWithDelay((Movements.Execute)words[0].enableStroke, 3f);
			}
		}
		calculateWordBackground();
		if (BoardController.levelToOpen == PlayerPrefsManager.GetLevel() || BoardController.levelToOpen == -1)
		{
			saveGame();
		}
	}

	private void sameWord(Word w)
	{
		disableCoins();
		SoundManager.instance.WordSame();
		GameObject[] array = w.letters;
		foreach (GameObject gameObject in array)
		{
			Movements.instance.tilt(gameObject.transform.gameObject, 2, 0.3f);
			Movements.instance.startFlash(gameObject.transform.Find("BG").GetComponent<Image>(), selectedLetterBGColor, Color.white, 0.3f);
		}
		Movements.instance.scale(wordContiner, Vector3.one * 0.9f, Vector3.one, 0.15f);
		Movements.instance.scale(wordContiner, Vector3.one, Vector3.one * 0.9f, 0.15f, 0.15f);
		for (int j = 0; j < wordList.childCount; j++)
		{
			GameObject gameObject2 = wordList.transform.GetChild(j).gameObject;
			UnityEngine.Object.Destroy(gameObject2, 0.32f);
		}
	}

	private void startPieceAnim(GameObject temp, GameObject l, float animTime, float delay)
	{
		temp.SetActive(value: false);
		temp.transform.SetAsLastSibling();
		Vector3 position = temp.transform.position;
		temp.transform.SetParent(base.transform.parent.Find("Board/Scale"), worldPositionStays: true);
		temp.transform.position = position;
	}

	public void notfound(string w)
	{
		if (w.Length == 1)
		{
			SoundManager.instance.DeselectLetter();
		}
		else
		{
			SoundManager.instance.WordInvalid();
		}
		Vector3 localPosition = wordContiner.transform.localPosition;
		localPosition.x = 0f;
		wordContiner.transform.localPosition = localPosition;
		Movements.instance.tilt(wordContiner, 2, 0.3f);
		for (int i = 0; i < wordList.childCount; i++)
		{
			UnityEngine.Object.Destroy(wordList.GetChild(i).gameObject, 0.2f);
		}
	}

	public void addLetter(Letter letter)
	{
		if (letter.used)
		{
			if (letterlist.Count > 1 && letterlist[letterlist.Count - 2] == letter)
			{
				removeLetter();
			}
			return;
		}
		SoundManager.instance.SelectLetter();
		if (letterlist.Count == 0)
		{
			instance.clearPreview();
		}
		letter.selected(s: true);
		letter.used = true;
		letterlist.Add(letter);
		word += letter.l;
		GameObject gameObject = UnityEngine.Object.Instantiate(pieceTemp);
		Text component = gameObject.GetComponent<Text>();
		component.text = letter.l;
		Vector2 sizeDelta = gameObject.GetComponent<RectTransform>().sizeDelta;
		sizeDelta.x = (float)FugoUtils.CalculateLengthOfMessage(component.text, component) * 4f;
		gameObject.GetComponent<RectTransform>().sizeDelta = sizeDelta;
		gameObject.transform.SetParent(wordList);
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		calculateWordBackground();
		if (line != null)
		{
			line.active = false;
			line.calculateTouch(letter.transform.position);
			UnityEngine.Object.Destroy(line);
		}
		if (letterlist.Count != level.letters.Length)
		{
			GameObject gameObject2 = UnityEngine.Object.Instantiate(lineTemp);
			gameObject2.transform.parent = lineContainer;
			gameObject2.transform.position = letter.transform.position;
			gameObject2.transform.localScale = Vector3.one;
			gameObject2.GetComponent<Image>().color = selectedLetterBGColor;
			line = gameObject2.GetComponent<Line>();
		}
	}

	private void removeLetter()
	{
		SoundManager.instance.DeselectLetter();
		if (letterlist.Count != level.letters.Length)
		{
			UnityEngine.Object.Destroy(lineContainer.GetChild(lineContainer.childCount - 1).gameObject);
			UnityEngine.Object.Destroy(lineContainer.GetChild(lineContainer.childCount - 2).gameObject);
		}
		else
		{
			UnityEngine.Object.Destroy(lineContainer.GetChild(lineContainer.childCount - 1).gameObject);
		}
		Letter letter = letterlist[letterlist.Count - 1];
		letter.used = false;
		letter.selected(s: false);
		letterlist.RemoveAt(letterlist.Count - 1);
		word = word.Substring(word.Length - 1);
		word = string.Empty;
		for (int i = 0; i < letterlist.Count; i++)
		{
			word += letterlist[i].l;
		}
		wordText.text = word;
		UnityEngine.Object.Destroy(wordList.GetChild(wordList.childCount - 1).gameObject);
		calculateWordBackground();
		GameObject gameObject = UnityEngine.Object.Instantiate(lineTemp);
		gameObject.transform.parent = lineContainer;
		gameObject.transform.position = letterlist[letterlist.Count - 1].transform.position;
		gameObject.transform.localScale = Vector3.one;
		gameObject.GetComponent<Image>().color = selectedLetterBGColor;
		line = gameObject.GetComponent<Line>();
	}

	private void calculateWordBackground()
	{
		Vector3 position = wordContiner.transform.position;
		position.x = 0f;
		wordContiner.transform.position = position;
		wordBG.sizeDelta = listRect.sizeDelta;
		wordBG.transform.Find("Left").GetComponent<Image>().color = selectedLetterBGColor;
		wordBG.transform.Find("Right").GetComponent<Image>().color = selectedLetterBGColor;
		wordBG.transform.Find("mid").GetComponent<Image>().color = selectedLetterBGColor;
		Vector2 sizeDelta = wordBG.sizeDelta;
		if (sizeDelta.x < 2f)
		{
			sizeDelta.x = -0.1f;
		}
		else
		{
			sizeDelta.x += 60f;
			sizeDelta.x = Mathf.Clamp(sizeDelta.x, 120f, 999999f);
		}
		wordBG.sizeDelta = sizeDelta;
		Vector3 localPosition = wordBG.transform.localPosition;
		localPosition.x = 0f;
		wordBG.transform.localPosition = localPosition;
	}

	public void shuffle()
	{
		SoundManager.instance.Shuffle();
		List<Vector3> list = new List<Vector3>();
		List<Vector3> list2 = new List<Vector3>();
		float num = (float)Math.PI * 2f / (float)level.letters.Length;
		for (int i = 0; i < letterContainer.childCount; i++)
		{
			Vector3 item = default(Vector3);
			item.x = range * Mathf.Sin(num * (float)i);
			item.y = range * Mathf.Cos(num * (float)i);
			item.z = 0f;
			list.Add(item);
		}
		while (list.Count != 0)
		{
			int index = UnityEngine.Random.Range(0, list.Count);
			list2.Add(list[index]);
			list.RemoveAt(index);
		}
		bool flag = true;
		for (int j = 0; j < list.Count; j++)
		{
			if (list[j] != list2[j])
			{
				flag = false;
				break;
			}
		}
		if (flag)
		{
		}
		for (int k = 0; k < list2.Count; k++)
		{
			GameObject gameObject = letterContainer.GetChild(k).gameObject;
			Movements.instance.move(gameObject, gameObject.transform.localPosition, list2[k], 0.2f, local: true);
		}
	}

	public void clearPreview()
	{
		GameObject gameObject = wordContiner.transform.Find("Word").gameObject;
		for (int i = 0; i < gameObject.transform.childCount; i++)
		{
			UnityEngine.Object.Destroy(gameObject.transform.GetChild(i).gameObject);
		}
		calculateWordBackground();
	}

	public void disableCoins()
	{
		foreach (Word word2 in words)
		{
			word2.checkBonus();
		}
	}

	private void checkExtraCoin()
	{
		if (BoardController.levelToOpen == -1 && BoardController.daily)
		{
			PlayerPrefsManager.IncreaseExtraWordCountDaily();
		}
		updateExtraCount();
	}

	private void updateExtraCount()
	{
		if (BoardController.levelToOpen == -1)
		{
			if (BoardController.daily)
			{
				GameAnimController.instance.star.transform.Find("Plus/Count").GetComponent<Text>().text = PlayerPrefsManager.GetExtraWordCountDaily().ToString();
			}
		}
		else
		{
			GameAnimController.instance.star.transform.Find("Plus/Count").GetComponent<Text>().text = string.Empty;
		}
	}

	private void checkAllWords()
	{
		try
		{
			foreach (Word word2 in words)
			{
				if (word2.checkWordDone())
				{
					foundWords.Add(word2);
					words.Remove(word2);
				}
			}
		}
		catch (Exception)
		{
		}
	}

	private void killCells()
	{
		GameObject[,] board = BoardController.instance.board;
		for (int i = 0; i < board.GetLength(0); i++)
		{
			for (int j = 0; j < board.GetLength(1); j++)
			{
				GameObject gameObject = board[i, j];
				if (!gameObject.activeSelf)
				{
					UnityEngine.Object.Destroy(gameObject);
					board[i, j] = null;
				}
			}
		}
	}
}
