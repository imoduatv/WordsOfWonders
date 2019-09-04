using Fabric.Answers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class BoardController : MonoBehaviour
{
	public static int levelToOpen = -1;

	public static bool daily = false;

	public static BoardController instance;

	public static Level level;

	public static LevelSet set;

	public static Section sec;

	public static float cellSize = 100f;

	public static Vector2 offsetX;

	public static Vector2 offsetY;

	public static float scaleAmount;

	public static Color boardColor = Color.red;

	public static bool nextSet;

	public static bool nextSec;

	public static Word bonus;

	public static bool blur;

	public GameObject boardPrefab;

	public GameObject testSprite;

	public Material bwMaterial;

	public int[] info;

	public GameObject cellTemp;

	public GameObject startTemp;

	public GameObject[,] board;

	public GameObject background;

	public new GameObject name;

	public GameObject cointText;

	public GameObject starContainer;

	public Sprite dailyBG;

	private Color cellBGColor;

	private float width;

	private float height;

	private Transform scale;

	private int startCount;

	private void Awake()
	{
		instance = this;
		if (daily)
		{
			startCount = 0;
			starContainer.SetActive(value: true);
			name.SetActive(value: false);
			UnityEngine.Debug.Log("daily star " + PlayerPrefsManager.getDailyStar());
			GameAnimController.instance.starCount = PlayerPrefsManager.getDailyStar();
			GameAnimController.instance.updateStar();
			GoogleAnalyticsScript.instance.LogScreen("Daily Puzzle Game");
		}
		else
		{
			GoogleAnalyticsScript.instance.LogScreen("Game");
			starContainer.SetActive(value: false);
			name.SetActive(value: true);
		}
	}

	private void OnApplicationFocus(bool focus)
	{
		if (daily)
		{
			GoogleAnalyticsScript.instance.LogScreen("Daily Puzzle Game");
		}
		else
		{
			GoogleAnalyticsScript.instance.LogScreen("Game");
		}
	}

	private void handleStarFound()
	{
	}

	private void Start()
	{
	}

	public void createBoard()
	{
		logLevelStart();
		bonus = null;
		scale = base.transform.Find("Scale");
		Vector2 sizeDelta = cellTemp.GetComponent<RectTransform>().sizeDelta;
		cellSize = sizeDelta.x;
		float num = 1336f / (float)Screen.height;
		width = (float)Screen.width * num / cellSize;
		Vector2 sizeDelta2 = scale.parent.GetComponent<RectTransform>().sizeDelta;
		height = sizeDelta2.y / cellSize;
		if (levelToOpen == PlayerPrefsManager.GetLevel())
		{
			levelToOpen = -1;
		}
		if (levelToOpen == -1)
		{
			info = FugoUtils.getLevelInfo();
		}
		else
		{
			info = FugoUtils.getLevelInfo(levelToOpen);
		}
		DateTime now = DateTime.Now;
		if (daily)
		{
			sec = Games.dailyPuzzles;
			set = sec.sets[now.Month - 1];
			level = set.levels[now.Day - 1];
		}
		else
		{
			sec = Games.sections[info[0]];
			set = sec.sets[info[1]];
			level = set.levels[info[2]];
		}
		ParticleAnimationManager.instance.PlayAnimation(set);
		Cell.cellColor = set.InGameTileColor;
		Cell.strokeColor = set.InGameSelectedLetterBGColor;
		nextSet = (set.levels.Count - 1 == info[2] && (levelToOpen == -1 || levelToOpen == PlayerPrefsManager.GetLevel()));
		nextSec = (sec.sets.Count - 1 == info[1] && (levelToOpen == -1 || levelToOpen == PlayerPrefsManager.GetLevel()) && nextSet);
		if (nextSet)
		{
			GameMenuController.instance.setGoldAmount(25);
		}
		if (nextSec)
		{
			GameMenuController.instance.setGoldAmount(100);
		}
		cellBGColor = FugoUtils.HexToColor(set.InGameSelectedLetterBGColor);
		board = new GameObject[level.width, level.height];
		background.SetActive(value: true);
		string path = "BGImages/" + set.bgImage;
		Sprite sprite = Resources.Load<Sprite>(path);
		background.GetComponent<Image>().sprite = sprite;
		background.transform.Find("Blur").gameObject.SetActive(value: false);
		if (PlayerPrefsManager.IsBlueMode())
		{
			background.SetActive(value: false);
		}
		if (levelToOpen != -1)
		{
			int[] levelInfo = FugoUtils.getLevelInfo();
			int[] levelInfo2 = FugoUtils.getLevelInfo(levelToOpen);
			UnityEngine.Debug.Log(levelInfo[1]);
			UnityEngine.Debug.Log(levelInfo2[1]);
			blur = (levelInfo[1] == levelInfo2[1]);
		}
		else
		{
			blur = true;
		}
		if (blur)
		{
			background.transform.Find("Blur").gameObject.SetActive(value: true);
		}
		else
		{
			background.transform.Find("Blur").gameObject.SetActive(value: true);
			FugoUtils.ChangeAlpha(background.transform.Find("Blur").GetComponent<Image>(), 0f);
			BlurControl.instance.enableBlur();
		}
		if (daily)
		{
			background.transform.Find("Blur").gameObject.SetActive(value: false);
		}
		name.GetComponent<Text>().text = set.SetFullName + " ● " + (info[2] + 1);
		name.GetComponent<Text>().color = FugoUtils.HexToColor(set.InGameHeaderColor);
		float a = width / (float)level.width;
		float b = height / (float)level.height;
		scaleAmount = Mathf.Min(a, b);
		scaleAmount *= 0.9f;
		cellSize *= scaleAmount;
		offsetX = Vector2.up * ((float)level.height - height) * cellSize * 0.5f * scaleAmount;
		offsetY = Vector2.left * ((float)level.width - width) * cellSize * 0.5f * scaleAmount;
		offsetX = Vector2.left * cellSize * level.width * 0.5f + Vector2.right * cellSize * 0.5f;
		offsetY = Vector2.up * cellSize * level.height * 0.5f + Vector2.down * cellSize * 0.5f;
		for (int i = 0; i < level.width; i++)
		{
			for (int j = 0; j < level.height; j++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(cellTemp);
				gameObject.transform.SetParent(scale);
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.SetAsFirstSibling();
				gameObject.SetActive(value: false);
				RectTransform component = gameObject.GetComponent<RectTransform>();
				component.anchoredPosition = (Vector3.right * i + Vector3.down * j) * cellSize;
				component.anchoredPosition += offsetX + offsetY;
				board[i, j] = gameObject;
				gameObject.transform.localScale = Vector3.one;
				gameObject.transform.Find("BG").localScale = Vector3.zero;
				gameObject.transform.Find("BG/Text").GetComponent<Text>().color = FugoUtils.HexToColor(set.InGameHintColor);
				component.sizeDelta = Vector2.one * cellSize;
				Cell component2 = gameObject.GetComponent<Cell>();
				component2.x = i;
				component2.y = j;
			}
		}
		GameObject[,] array = board;
		int length = array.GetLength(0);
		int length2 = array.GetLength(1);
		for (int k = 0; k < length; k++)
		{
			for (int l = 0; l < length2; l++)
			{
				GameObject gameObject2 = array[k, l];
				if (gameObject2.activeSelf)
				{
				}
			}
		}
		bool flag = false;
		foreach (Word word in level.words)
		{
			if (word.bonus)
			{
				flag = true;
				bonus = word;
			}
			try
			{
				placeWord(word);
			}
			catch (Exception)
			{
			}
		}
		if (flag)
		{
			Movements.instance.executeWithDelay((Movements.Execute)TutorialController.instance.openBonus, 0.5f);
		}
		byte[] bytes = Encoding.Default.GetBytes(SystemInfo.deviceUniqueIdentifier);
		int seed = PlayerPrefsManager.GetLevel() + BitConverter.ToInt32(bytes, 0);
		UnityEngine.Random.InitState(seed);
		UnityEngine.Random.InitState(DateTime.Now.Millisecond);
		Movements.instance.executeWithDelay((Movements.Execute)WheelController.instance.newGame, 0.55f);
		MapController.instance.setColors();
		if (daily && PlayerPrefsManager.IsItFirstDaily())
		{
			Movements.instance.executeWithDelay((Movements.Execute)TutorialController.instance.openDaily, 1.5f);
		}
		if (levelToOpen == -1)
		{
			if (PlayerPrefsManager.GetLevel() == 1)
			{
				WheelController.running = false;
				UnityEngine.Debug.Log("tutorial1");
				Movements.instance.executeWithDelay((Movements.Execute)TutorialController.instance.openFirst, 1.5f);
			}
			if (PlayerPrefsManager.GetLevel() == 3)
			{
				WheelController.running = false;
				UnityEngine.Debug.Log("tutorial2");
				Movements.instance.executeWithDelay((Movements.Execute)TutorialController.instance.openSecond, 1.5f);
			}
			if (PlayerPrefsManager.GetLevel() == 5)
			{
				WheelController.running = false;
				UnityEngine.Debug.Log("tutorial3");
				Movements.instance.executeWithDelay((Movements.Execute)TutorialController.instance.openThird, 1.5f);
			}
		}
	}

	private void logWords()
	{
		foreach (Word word in level.words)
		{
			UnityEngine.Debug.Log(word.word + "  " + word.X + "  " + word.Y + "  " + word.bonus);
		}
		string[] otherWords = level.otherWords;
		foreach (string message in otherWords)
		{
			UnityEngine.Debug.Log(message);
		}
	}

	public static GameObject getLevel(int level)
	{
		GameObject original = Resources.Load<GameObject>("Prefabs/BoardTemplate");
		GameObject original2 = Resources.Load<GameObject>("Prefabs/DebugCell");
		int[] levelInfo = FugoUtils.getLevelInfo(level);
		Section section = Games.sections[levelInfo[0]];
		LevelSet levelSet = section.sets[levelInfo[1]];
		Level level2 = levelSet.levels[levelInfo[2]];
		float num = FugoUtils.boardHeight;
		float num2 = num * FugoUtils.getBoardRatio();
		GameObject gameObject = UnityEngine.Object.Instantiate(original);
		Transform transform = gameObject.transform.Find("Board");
		float val = 7.5f / (float)level2.width;
		float val2 = num / 100f / (float)level2.height;
		float num3 = Math.Min(val, val2);
		float num4 = 100f * num3;
		float val3 = num2 / (float)level2.width;
		float val4 = num / (float)level2.height;
		num4 = Math.Min(val3, val4);
		float num5 = num2 - (float)level2.width * num4;
		float num6 = num - (float)level2.height * num4;
		UnityEngine.Debug.Log(num + "  " + level2.height + "   " + cellSize);
		num5 *= 0.5f;
		num6 *= 0.5f;
		RectTransform component = transform.GetComponent<RectTransform>();
		UnityEngine.Debug.Log("board size  " + component.sizeDelta);
		Vector2 sizeDelta = component.sizeDelta;
		sizeDelta.y = FugoUtils.adBoardHeight;
		component.sizeDelta = sizeDelta;
		Vector2 anchoredPosition = component.anchoredPosition;
		anchoredPosition.y = -100f;
		component.anchoredPosition = anchoredPosition;
		List<Word> words = level2.words;
		foreach (Word item in words)
		{
			int length = item.length;
			int x = item.X;
			int y = item.Y;
			for (int i = 0; i < length; i++)
			{
				GameObject gameObject2 = UnityEngine.Object.Instantiate(original2);
				gameObject2.GetComponent<Image>().color = FugoUtils.HexToColor(levelSet.InGameSelectedLetterBGColor);
				gameObject2.transform.Find("Text").GetComponent<Text>().color = FugoUtils.HexToColor(levelSet.InGameSelectedLetterColor);
				gameObject2.transform.Find("Text").GetComponent<Text>().text = item.word[i].ToString();
				RectTransform component2 = gameObject2.GetComponent<RectTransform>();
				gameObject2.transform.SetParent(transform.transform);
				component2.anchorMin = Vector2.up;
				component2.anchorMax = Vector2.up;
				component2.pivot = Vector2.up;
				component2.sizeDelta = Vector2.one * num4;
				component2.anchoredPosition = Vector2.right * x * num4 + Vector2.down * y * num4;
				component2.anchoredPosition += Vector2.down * num6 + Vector2.right * num5;
				if (item.orientation == Orientation.HORIZONTAL)
				{
					component2.anchoredPosition += Vector2.right * i * num4;
				}
				else
				{
					component2.anchoredPosition += Vector2.down * i * num4;
				}
				gameObject2.transform.localScale = Vector3.one;
			}
		}
		return gameObject;
	}

	private int[] getLevelInfo(int level)
	{
		throw new NotImplementedException();
	}

	private void openBonus()
	{
		TutorialController.instance.openBonus();
	}

	private IEnumerator loadImage(Image img)
	{
		float startTime = Time.realtimeSinceStartup;
		TextAsset textasset = Resources.Load("image") as TextAsset;
		string data = textasset.text;
		byte[] dataImage = Convert.FromBase64String(data);
		Texture2D mytexture = new Texture2D(1, 1);
		if (!mytexture.LoadImage(dataImage))
		{
			UnityEngine.Debug.Log("Failed loading image data!");
		}
		else
		{
			UnityEngine.Debug.Log("LoadImage - Still sane here - size: " + mytexture.width + "x" + mytexture.height);
		}
		yield return null;
		Sprite test = Sprite.Create(mytexture, new Rect(0f, 0f, mytexture.width, mytexture.height), new Vector2(0.5f, 0.5f), 100f);
		float endTime = Time.realtimeSinceStartup;
		UnityEngine.Debug.Log("loading time " + (endTime - startTime).ToString());
		BlurControl.instance.disableBlur();
		Movements.instance.fadeImage(background, test, 0.5f);
		yield return null;
	}

	private void placeWord(Word word)
	{
		for (int i = 0; i < word.length; i++)
		{
			if (levelToOpen != -1)
			{
				word.bonus = false;
			}
			GameObject gameObject = (word.orientation != 0) ? board[word.X, word.Y + i] : board[word.X + i, word.Y];
			if (word.bonus)
			{
				gameObject.transform.Find("BG/Coin").gameObject.SetActive(value: true);
				UnityEngine.Debug.Log("enable coin");
			}
			word.letters[i] = gameObject;
			gameObject.transform.Find("BG/Text").GetComponent<Text>().text = word.word[i].ToString();
			gameObject.SetActive(value: true);
		}
	}

	public void startInitAnims()
	{
		try
		{
			for (int i = 0; i < level.width; i++)
			{
				for (int j = 0; j < level.height; j++)
				{
					GameObject gameObject = board[i, j];
					gameObject.transform.localScale = Vector3.one;
					gameObject.transform.Find("BG").localScale = Vector3.zero;
					Movements.instance.scale(gameObject.transform.Find("BG").gameObject, Vector3.zero, Vector3.one, 0.3f, (float)(i + j) * 0.01f);
				}
			}
			GameObject gameObject2 = WheelController.instance.gameObject;
			Movements.instance.scale(gameObject2, Vector3.zero, Vector3.one * 1.3f, 0.3f);
			Movements.instance.scale(gameObject2, Vector3.one * 1.3f, Vector3.one * 1.21f, 0.2f, 0.3f);
		}
		catch (Exception)
		{
			UnityEngine.Debug.Log("retry");
			Movements.instance.executeWithDelay((Movements.Execute)startInitAnims, 0.5f);
		}
	}

	public void onHintClicked()
	{
		GameMenuController.instance.updateHintPrice();
		if (PlayerPrefsManager.GetCoin() >= PlayerPrefsManager.GetHintPrice())
		{
			bool flag = false;
			GameObject[,] array = board;
			int length = array.GetLength(0);
			int length2 = array.GetLength(1);
			for (int i = 0; i < length; i++)
			{
				for (int j = 0; j < length2; j++)
				{
					GameObject gameObject = array[i, j];
					if (!(gameObject == null) && gameObject.activeSelf && !gameObject.transform.Find("BG/Text").gameObject.activeSelf && gameObject.transform.Find("BG").GetChild(0).name != "DailyStar(Clone)")
					{
						flag = true;
					}
				}
			}
			bool flag2 = false;
			GameObject[,] array2 = board;
			int length3 = array2.GetLength(0);
			int length4 = array2.GetLength(1);
			for (int k = 0; k < length3; k++)
			{
				for (int l = 0; l < length4; l++)
				{
					GameObject gameObject2 = array2[k, l];
					if (!(gameObject2 == null) && !(gameObject2.transform.Find("BG").GetChild(0).name == "DailyStar(Clone)"))
					{
						flag2 = true;
					}
				}
			}
			if (flag2 && flag)
			{
				getHint();
			}
		}
		else
		{
			GameMenuController.instance.onShopClicked();
		}
	}

	public void getHint()
	{
		int num = UnityEngine.Random.Range(0, level.width);
		int num2 = UnityEngine.Random.Range(0, level.height);
		GameObject gameObject = board[num, num2];
		if (gameObject == null)
		{
			getHint();
		}
		else if (gameObject.activeSelf && !gameObject.transform.Find("BG/Text").gameObject.activeSelf && gameObject.transform.Find("BG").GetChild(0).name != "DailyStar(Clone)")
		{
			GameObject gameObject2 = gameObject.transform.Find("BG/Text").gameObject;
			gameObject.transform.Find("BG/Text").gameObject.SetActive(value: true);
			gameObject2.GetComponent<Text>().color = FugoUtils.HexToColor(set.InGameHintColor);
			Movements.instance.move(gameObject2, GameAnimController.instance.hint.transform.Find("Hint/Image").transform.position, gameObject2.transform.position, 0.3f);
			Movements.instance.scale(gameObject2, 2f, 0.25f);
			Movements.instance.scale(gameObject2, Vector3.one * 2f, Vector3.one, 0.25f, 0.25f);
			gameObject.transform.Find("BG/Coin").gameObject.SetActive(value: false);
			PlayerPrefsManager.SetCoin(PlayerPrefsManager.GetCoin() - PlayerPrefsManager.GetHintPrice());
			cointText.GetComponent<Text>().text = PlayerPrefsManager.GetCoin().ToString();
			SoundManager.instance.PlayHint();
			TutorialController.freeHint = false;
			if (!daily)
			{
				PlayerPrefsManager.UseProHint();
			}
			if (daily)
			{
				PlayerPrefsManager.AddHintDaily(num, num2);
			}
			else
			{
				PlayerPrefsManager.AddHint(num, num2);
			}
			GameMenuController.instance.updateHintPrice();
		}
		else
		{
			getHint();
		}
	}

	public void setStar()
	{
		if (!daily)
		{
			return;
		}
		int num = UnityEngine.Random.Range(0, level.width);
		int num2 = UnityEngine.Random.Range(0, level.height);
		for (int i = num; i < board.GetLength(0) + num; i++)
		{
			for (int j = num2; j < board.GetLength(1) + num2; j++)
			{
				int x = i % board.GetLength(0);
				int y = j % board.GetLength(1);
				GameObject gameObject = board[i % board.GetLength(0), j % board.GetLength(1)];
				if (!(gameObject == null) && !gameObject.transform.Find("BG/Text").gameObject.activeSelf && gameObject.activeSelf)
				{
					Cell component = gameObject.GetComponent<Cell>();
					component.starred = true;
					GameObject gameObject2 = UnityEngine.Object.Instantiate(startTemp);
					gameObject2.transform.SetParent(gameObject.transform.Find("BG"));
					gameObject2.transform.SetAsFirstSibling();
					gameObject2.transform.localPosition = Vector3.zero;
					gameObject2.transform.localScale = Vector3.zero;
					gameObject2.transform.GetChild(0).gameObject.SetActive(value: false);
					GameAnimController.dailyStar = gameObject2;
					Movements.instance.scale(gameObject2, Vector3.zero, Vector3.one * scaleAmount, 0.3f);
					PlayerPrefsManager.setStarPosition(x, y);
					return;
				}
			}
		}
	}

	public void spawnStar(int[] pos)
	{
		if (daily)
		{
			spawnStar(pos[0], pos[1]);
		}
	}

	public void spawnStar(int x, int y)
	{
		GameObject gameObject = board[x, y];
		if (!gameObject.transform.Find("BG/Text").gameObject.activeSelf && gameObject.activeSelf)
		{
			Cell component = gameObject.GetComponent<Cell>();
			component.starred = true;
			GameObject gameObject2 = UnityEngine.Object.Instantiate(startTemp);
			gameObject2.transform.SetParent(gameObject.transform.Find("BG"));
			gameObject2.transform.SetAsFirstSibling();
			gameObject2.transform.localPosition = Vector3.zero;
			gameObject2.transform.localScale = Vector3.zero;
			gameObject2.transform.GetChild(0).gameObject.SetActive(value: false);
			GameAnimController.dailyStar = gameObject2;
			Movements.instance.scale(gameObject2, Vector3.zero, Vector3.one * scaleAmount, 0.3f);
			PlayerPrefsManager.setStarPosition(x, y);
		}
	}

	private void logLevelStart()
	{
		string text = (levelToOpen != -1) ? ("Level_" + levelToOpen.ToString()) : ("Level_" + PlayerPrefsManager.GetLevel().ToString());
		if (daily)
		{
			text = "Daily";
		}
		Answers.LogLevelStart(text.ToString());
	}

	public void logLevelEnd()
	{
		string text = (levelToOpen != -1) ? ("Level_" + levelToOpen.ToString()) : ("Level_" + PlayerPrefsManager.GetLevel().ToString());
		if (daily)
		{
			text = "Daily";
		}
		Answers.LogLevelEnd(text.ToString());
	}
}
