using System;
using UnityEngine;
using UnityEngine.UI;
using v2Gameplay;

public class MapController : MonoBehaviour
{
	public static MapController instance;

	public GameObject pointTemp;

	public GameObject start;

	public GameObject end;

	public GameObject pointContainer;

	public GameObject player;

	public Text levelName;

	public GameObject mazeLine;

	public GameObject[] lines;

	public GameObject[] points;

	public GameObject gem;

	public Sprite mazeMap;

	public int pointIndex;

	private float angle;

	private float angleRange;

	private float distance;

	private Vector3[] positions;

	private float animTime;

	private GameObject ribbon;

	private Level game;

	private int current;

	private int total;

	private bool animated;

	private void Awake()
	{
		if (GameController.daily)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void Start()
	{
		GameController gameController = GameController.instance;
		gameController.onNewGame = (GameController.SendGame)Delegate.Combine(gameController.onNewGame, new GameController.SendGame(handleNewGame));
		animated = false;
		base.gameObject.SetActive(value: false);
	}

	private void initAdventure()
	{
		instance = this;
		pointIndex = 0;
		GetComponent<Image>().sprite = mazeMap;
		gem.SetActive(value: true);
		base.transform.Find("Start").gameObject.SetActive(value: false);
		base.transform.Find("End").gameObject.SetActive(value: false);
		positions = new Vector3[32];
		calculatePositions();
		base.gameObject.SetActive(value: false);
		GameObject gameObject = UnityEngine.Object.Instantiate(mazeLine);
		gameObject.transform.SetParent(base.transform);
		gameObject.transform.SetSiblingIndex(1);
		gameObject.transform.localScale = Vector3.one;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.name = "Lines";
		points = new GameObject[gameObject.transform.childCount];
		for (int i = 0; i < gameObject.transform.childCount; i++)
		{
			points[i] = gameObject.transform.GetChild(i).gameObject;
			points[i].SetActive(value: false);
		}
		points[0].SetActive(value: true);
		enablePartial(float.Parse(GameController.adventureGameId) / (float)GameController.adventureCount);
	}

	public void init()
	{
		animTime = 0.1f;
		instance = this;
		pointIndex = 0;
		angle = 0f;
		angleRange = 1.2f;
		ribbon = base.transform.Find("Ribbon").gameObject;
		positions = new Vector3[32];
		calculatePositions();
		base.gameObject.SetActive(value: false);
		int levelNumber = PlayerPrefsManager.GetLevel();
		if (GameController.levelToOpen != -1)
		{
			levelNumber = GameController.levelToOpen;
		}
		GameObject gameObject = UnityEngine.Object.Instantiate(lines[FugoUtils.getLevelInfo(levelNumber)[1] % lines.Length]);
		gameObject.transform.SetParent(base.transform);
		gameObject.transform.SetSiblingIndex(1);
		gameObject.transform.localScale = Vector3.one;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.name = "Lines";
		points = new GameObject[gameObject.transform.childCount];
		for (int i = 0; i < gameObject.transform.childCount; i++)
		{
			points[i] = gameObject.transform.GetChild(i).gameObject;
			points[i].SetActive(value: false);
			points[i].name = "point " + i;
		}
		points[0].SetActive(value: true);
		Vector3 localScale = gameObject.transform.localScale;
		if (localScale.x < 0f)
		{
		}
		setColors();
		calculatePartial();
	}

	private void handleNewGame(Level g)
	{
		if (!(instance != null))
		{
			game = g;
			if (game.mode == GameMode.ADVENTURE)
			{
				initAdventure();
			}
			else
			{
				init();
			}
		}
	}

	public void calculatePositions()
	{
		Vector3 position = start.transform.position;
		Vector2 sizeDelta = start.GetComponent<RectTransform>().sizeDelta;
		Vector3 vector = position + sizeDelta.x * 0.5f * Vector3.right;
		Vector3 position2 = end.transform.position;
		Vector2 sizeDelta2 = end.GetComponent<RectTransform>().sizeDelta;
		Vector3 b = position2 + sizeDelta2.x * 0.5f * Vector3.left;
		distance = (vector - b).magnitude / 20f;
		if (UISwapper.flipGame)
		{
			ArabicText arabicText = levelName.gameObject.AddComponent<ArabicText>();
			if (game.mode == GameMode.ADVENTURE)
			{
				arabicText.Text = game.set.SetFullName + " " + GameController.adventureGameId + "/" + GameController.adventureCount;
			}
			else
			{
				arabicText.Text = game.set.SetFullName + " " + (GameController.instance.getLevelNumber() + 1).ToString() + "/" + game.set.levels.Count;
			}
		}
		else if (game.mode == GameMode.ADVENTURE)
		{
			levelName.text = game.set.SetFullName + " " + GameController.adventureGameId + "/" + GameController.adventureCount;
		}
		else
		{
			levelName.text = game.set.SetFullName + " " + (GameController.instance.getLevelNumber() + 1).ToString() + "/" + game.set.levels.Count;
		}
		positions[0] = vector;
		for (int i = 1; i < positions.Length; i++)
		{
			positions[i] = Vector3.Lerp(vector, b, (float)i / (float)positions.Length);
			positions[i].y += 60f * Mathf.Cos(positions[i].x / 30f);
		}
	}

	public void setColors()
	{
		start.GetComponent<Image>().color = GameController.FirstSetColor;
		end.GetComponent<Image>().color = GameController.FirstSetColor;
		if (UISwapper.flipGame)
		{
			start.transform.Find("Text").GetComponent<Text>().lineSpacing = -1f;
		}
		start.transform.Find("Text").GetComponent<Text>().text = GameController.set.SetName;
	}

	public void enablePoint()
	{
		pointIndex++;
		try
		{
			GameObject gameObject = points[pointIndex];
			gameObject.SetActive(value: true);
			Movements.instance.move(player, player.transform.position, points[pointIndex + 1].transform.position, animTime);
			SoundManager.instance.RoadPoint();
		}
		catch (Exception)
		{
		}
	}

	public void enablePlayer()
	{
		try
		{
			player.transform.localPosition = points[pointIndex].transform.localPosition;
		}
		catch (Exception)
		{
		}
		player.SetActive(value: true);
	}

	public int nextLevel()
	{
		UnityEngine.Debug.Log("*****next level*****");
		int num = (int)((float)points.Length / (float)GameController.set.levels.Count);
		if (game.mode == GameMode.ADVENTURE)
		{
			num = (int)((float)points.Length / (float)GameController.adventureCount);
		}
		float num2 = 0f;
		for (int i = 0; i < num; i++)
		{
			num2 = (float)i * 0.12f;
			Movements.instance.executeWithDelay((Movements.Execute)instance.enablePoint, num2);
		}
		checkRibbon();
		Movements.instance.executeWithDelay((Movements.Execute)spawnBlink, 2.5f);
		return num;
	}

	public void spawnBlink()
	{
		float num = 1f;
		GameObject gameObject = UnityEngine.Object.Instantiate(player);
		UnityEngine.Object.Destroy(gameObject, num + 0.2f);
		gameObject.transform.SetAsFirstSibling();
		gameObject.transform.SetParent(player.transform);
		gameObject.transform.position = player.transform.position;
		gameObject.transform.localScale = player.transform.localScale;
		Movements.instance.scale(gameObject, Vector3.one, Vector3.one * 2f, num);
		Movements.instance.startFadeOut(gameObject, num, 0f);
		Movements.instance.executeWithDelay((Movements.Execute)spawnBlink, 2.5f);
	}

	private void calculatePartial()
	{
		float num = GameController.set.levels.Count;
		float num2 = GameController.instance.getLevelNumber();
		enablePartial(num2 / num);
	}

	public void enablePartial(float percent)
	{
		int num = 0;
		for (int i = 0; (float)i < (float)points.Length * percent; i++)
		{
			points[i].SetActive(value: true);
			pointIndex = i;
			player.transform.position = points[i].transform.position;
			num++;
		}
		enablePlayer();
	}

	public void enablePartial()
	{
		pointIndex = 0;
		points[0].SetActive(value: true);
		player.transform.position = points[0].transform.position;
		enablePartial((float)BoardController.instance.info[2] / (float)BoardController.set.levels.Count);
	}

	public void disablePlayer()
	{
		base.transform.Find("PointContainer/Player").gameObject.SetActive(value: false);
	}

	public void hidePlayer()
	{
		player.SetActive(value: false);
	}

	public void checkRibbon()
	{
		int[] levelInfo = FugoUtils.getLevelInfo();
		if (GameController.endType == EndType.Level || GameController.endType == EndType.Hidden)
		{
			return;
		}
		Movements.instance.executeWithDelay((Movements.Execute)SoundManager.instance.playPop2, 0.5f);
		ribbon.SetActive(value: true);
		GameObject gameObject = ribbon.transform.Find("LevelName").gameObject;
		if (UISwapper.flipGame)
		{
			ArabicText arabicText = gameObject.GetComponent<ArabicText>();
			if (arabicText == null)
			{
				arabicText = gameObject.AddComponent<ArabicText>();
			}
			arabicText.Text = AutoLanguage.dict["SetCompleteText"].Replace("%@%", GameController.set.SetFullName.ToUpper());
		}
		else
		{
			ribbon.transform.Find("LevelName").GetComponent<Text>().text = AutoLanguage.dict["SetCompleteText"].Replace("%@%", GameController.set.SetFullName.ToUpper());
		}
		ribbon.GetComponent<Image>().color = FugoUtils.HexToColor(GameController.section.FirstSetColor);
		Movements.instance.scale(ribbon, Vector3.zero, Vector3.one, 0.5f);
	}
}
