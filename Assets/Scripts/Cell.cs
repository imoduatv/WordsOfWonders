using System;
using UnityEngine;
using UnityEngine.UI;
using v2Gameplay;

public class Cell : MonoBehaviour
{
	public static string cellColor;

	public static string strokeColor;

	public int x;

	public int y;

	public bool active;

	public bool starred;

	public bool found;

	public bool hinted;

	public bool coined;

	public string letter;

	public RectTransform rect;

	public GameObject coinPrefab;

	public Image bg;

	private void Awake()
	{
		active = false;
		starred = false;
		hinted = false;
		found = false;
		coined = false;
		rect = GetComponent<RectTransform>();
	}

	public void setFound()
	{
		if (starred)
		{
			DailyController.instance.starFound(base.transform.Find("BG/star").gameObject);
		}
		found = true;
		starred = false;
		hinted = false;
	}

	public bool isItStarrable()
	{
		return !found && !hinted && !starred;
	}

	public GameObject setStarred()
	{
		starred = true;
		GameObject gameObject = UnityEngine.Object.Instantiate(ObjectHolder.instance.dailyStarPrefab);
		gameObject.name = "star";
		gameObject.transform.SetParent(base.transform.Find("BG"));
		gameObject.transform.ResetTransform();
		gameObject.transform.localScale = Vector3.zero;
		PlayerPrefsManager.setStarPosition(x, y);
		Movements.instance.scale(gameObject, 0.8f, 0.3f);
		return gameObject;
	}

	public bool isItHintable()
	{
		return !found && !hinted && !starred;
	}

	private void Start()
	{
	}

	public void openCell()
	{
		GameObject gameObject = base.transform.Find("BG").gameObject;
		GameObject gameObject2 = base.transform.Find("BG/Text").gameObject;
		gameObject.SetActive(value: true);
		gameObject2.SetActive(value: true);
		bg.color = GameController.SelectedLetterBGColor;
		gameObject2.GetComponent<Text>().color = GameController.InGameSelectedLetterColor;
		FugoUtils.ChangeAlpha(bg, 1f);
	}

	public void onlyStroke()
	{
		float num = 0.2f;
		float time = 0.15f;
		Transform transform = PreviewController.instance.wordContainer.parent.parent.transform;
		Vector3 position = transform.position;
		float num2 = position.y;
		Vector3 position2 = base.transform.position;
		float num3 = num2 - position2.y;
		if (num3 > 0f)
		{
			float num4 = num3;
			Vector2 sizeDelta = rect.sizeDelta;
			float num5 = num4 / sizeDelta.x;
			num *= (float)Math.Pow(0.090000003576278687, num5 * 100f);
		}
		GameObject gameObject = base.transform.Find("BG/Stroke").gameObject;
		GameObject gameObject2 = base.transform.Find("BG").gameObject;
		GameObject gameObject3 = base.transform.Find("BG/Text").gameObject;
		gameObject.GetComponent<Image>().color = GameController.SelectedLetterBGColor;
		FugoUtils.ChangeAlpha(gameObject.GetComponent<Image>(), 0f);
		gameObject.SetActive(value: true);
		Movements.instance.startFadeIn(gameObject2, time, num);
		Movements.instance.startFadeIn(gameObject3, time, num);
	}

	public void enableStroke()
	{
		float num = 0.15f;
		GameObject gameObject = base.transform.Find("BG/Stroke").gameObject;
		gameObject.GetComponent<Image>().color = Color.Lerp(GameController.SelectedLetterBGColor, Color.black, 0.2f);
		FugoUtils.ChangeAlpha(gameObject.GetComponent<Image>(), 0f);
		gameObject.gameObject.SetActive(value: true);
		SoundManager.instance.LetterPlace();
		Movements.instance.startFadeIn(gameObject, num, 1f);
		Movements.instance.scale(base.gameObject, Vector3.one, Vector3.one * 1.5f, num);
		Movements.instance.scale(base.gameObject, Vector3.one * 1.5f, Vector3.one, num, num);
	}

	public void spawnCoin()
	{
		if (base.transform.Find("BG/Coin").gameObject.activeSelf)
		{
			SoundManager.instance.CoinGained();
			base.transform.Find("BG/Coin").gameObject.SetActive(value: false);
			float num = 20f;
			GameObject gameObject = UnityEngine.Object.Instantiate(coinPrefab);
			gameObject.transform.SetParent(base.transform.parent);
			gameObject.transform.localScale = Vector3.zero;
			gameObject.transform.position = base.transform.position;
			gameObject.GetComponent<CoinAnim>().startMove();
		}
	}

	public void placeLetter()
	{
		found = true;
		base.transform.Find("BG/Text").gameObject.SetActive(value: true);
		base.transform.Find("BG/Stroke").gameObject.SetActive(value: false);
		base.transform.Find("BG/Text").GetComponent<Text>().color = GameController.InGameSelectedLetterColor;
		FugoUtils.ChangeAlpha(base.transform.Find("BG/Text").GetComponent<Text>(), 1f);
		Movements.instance.lerpColorTo(base.transform.Find("BG").gameObject, GameController.SelectedLetterBGColor, 0.2f);
		Movements.instance.lerpColorTo(base.transform.Find("BG/Text").gameObject, GameController.InGameSelectedLetterColor, 0.2f);
		base.transform.Find("BG/Text").gameObject.SetActive(value: true);
		spawnCoin();
		checkStar();
	}

	public void enableHint()
	{
		enableHint(1f);
	}

	public void enableHint(float alpha)
	{
		alpha = GameController.TileColor.a;
		hinted = true;
		coined = false;
		base.transform.Find("BG/Text").gameObject.SetActive(value: true);
		base.transform.Find("BG/Coin").gameObject.SetActive(value: false);
		Color inGameLetterColor = GameController.InGameLetterColor;
		base.transform.Find("BG/Text").GetComponent<Text>().color = inGameLetterColor;
	}

	public void hintAnim()
	{
		SoundManager.instance.PlayHint();
		float num = 0.1f;
		Color inGameLetterColor = GameController.InGameLetterColor;
		Text component = base.transform.Find("BG/Text").GetComponent<Text>();
		Image component2 = base.transform.Find("BG").GetComponent<Image>();
		Image component3 = base.transform.Find("BG/Stroke").GetComponent<Image>();
		component2.color = Color.white;
		inGameLetterColor.a = 0f;
		component.color = inGameLetterColor;
		component3.color = GameController.SelectedLetterBGColor;
		component3.gameObject.SetActive(value: true);
		Movements.instance.startFadeIn(component.gameObject, num, GameController.InGameLetterColor.a);
		Movements.instance.lerpColorTo(component2.gameObject, GameController.TileColor, num);
		Movements.instance.startFadeOut(component3.gameObject, num, 0f);
		component2.transform.localScale = Vector3.one * 1.2f;
		Movements.instance.scale(component2.gameObject, 1f, num);
		ParticleSystem component4 = base.transform.Find("BG/Particle").GetComponent<ParticleSystem>();
		component4.Play();
	}

	public void enableCoin()
	{
		GameObject gameObject = base.transform.Find("BG/Coin").gameObject;
		if (gameObject.activeSelf)
		{
			Movements.instance.scale(gameObject, Vector3.zero, Vector3.one, 0.2f);
			UnityEngine.Debug.Log("coin enabled");
		}
	}

	public void disableCoin()
	{
		GameObject gameObject = base.transform.Find("BG/Coin").gameObject;
		if (gameObject.activeSelf)
		{
			Movements.instance.scale(gameObject, Vector3.one, Vector3.zero, 0.2f);
			Movements.instance.executeWithDelay((Movements.Execute)closeCoin, 0.21f);
		}
	}

	private void closeCoin()
	{
		base.transform.Find("BG/Coin").gameObject.SetActive(value: false);
	}

	public void checkStar()
	{
		if (starred)
		{
			Movements.instance.executeWithDelay((Movements.Execute)SoundManager.instance.StarCollected, 0.1f);
			GameAnimController.instance.collectStarAnim();
			if (WheelController.running)
			{
				Movements.instance.executeWithDelay((Movements.Execute)BoardController.instance.setStar, 0.2f);
				starred = false;
			}
		}
	}

	public void setCellColor(Color color)
	{
		base.transform.Find("BG").GetComponent<Image>().color = color;
	}

	public void setTextColor(Color color)
	{
		base.transform.Find("BG/Text").GetComponent<Text>().color = color;
	}

	public void setText(string l)
	{
		letter = l;
		base.transform.Find("BG/Text").GetComponent<Text>().text = l;
		GameObject gameObject = base.transform.Find("BG/Text").gameObject;
		Vector2 sizeDelta = base.transform.GetComponent<RectTransform>().sizeDelta;
		float num = sizeDelta.x / 100f;
		num = v2Gameplay.BoardController.cellSize / 100f;
		if (PlayerPrefsManager.GetLang() == "Arabic")
		{
			RectTransform component = gameObject.GetComponent<RectTransform>();
			LetterOffset letterOffset = CalculateLetterData.letterOffsets[l];
			component.offsetMax = letterOffset.cellOffset * num;
			component.offsetMin = letterOffset.cellOffset * num;
			if (Application.platform == RuntimePlatform.Android)
			{
				gameObject.transform.localScale = new Vector3(-0.9f, 0.9f, 0.9f);
			}
			UnityEngine.Debug.Log(component.offsetMin + "   " + component.offsetMax + "  " + letterOffset.cellOffset + "  " + num + "  " + l);
		}
	}

	public void enableHint(Color hintColor)
	{
		hinted = true;
		coined = false;
		base.transform.Find("BG/Text").gameObject.SetActive(value: true);
		base.transform.Find("BG/Text").GetComponent<Text>().color = hintColor;
	}

	public void enableLetter()
	{
		Color selectedLetterBGColor = GameController.SelectedLetterBGColor;
		setCellColor(selectedLetterBGColor);
		selectedLetterBGColor = GameController.InGameSelectedLetterColor;
		setTextColor(selectedLetterBGColor);
		active = true;
		base.transform.Find("BG/Text").gameObject.SetActive(value: true);
		base.transform.Find("BG/Coin").gameObject.SetActive(value: false);
		base.transform.Find("BG/Stroke").gameObject.SetActive(value: false);
	}

	public void enableCell()
	{
		float a = GameController.TileColor.a;
		active = true;
		float a2 = GameController.TileColor.a;
		if (found)
		{
			GameObject gameObject = base.transform.Find("BG/Text").gameObject;
			a2 = 1f;
			Text component = gameObject.GetComponent<Text>();
			Color color = component.color;
			color.a = 1f;
			if (!gameObject.activeSelf)
			{
				component.color = color;
			}
		}
		else if (hinted)
		{
			Text component2 = base.transform.Find("BG/Text").GetComponent<Text>();
			Color color2 = component2.color;
			color2.a = 1f;
			component2.color = color2;
		}
		Movements.instance.scale(bg.gameObject, 1f, 0.15f);
	}

	public void fadeOut(float alpha, float time, float delay)
	{
		Movements.instance.startFadeOut(base.transform.Find("BG").GetComponent<Image>(), time, alpha, delay);
		Movements.instance.startFadeOut(base.transform.Find("BG/Text").GetComponent<Text>(), time, alpha, delay);
		Movements.instance.startFadeOut(base.transform.Find("BG/Stroke").GetComponent<Image>(), time, alpha, delay);
		Movements.instance.startFadeOut(base.transform.Find("BG/Coin").GetComponent<Image>(), time, alpha, delay);
		if (starred)
		{
			Movements.instance.startFadeOut(base.transform.Find("BG/star").GetComponent<Image>(), time, alpha, delay);
		}
	}
}
