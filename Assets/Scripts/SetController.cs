using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using v2Gameplay;

public class SetController : MonoBehaviour
{
	public static SetController instance;

	public Transform levelHolder;

	public Transform levelsBG;

	public Text title;

	public Image ribbon;

	public Image bg;

	public GameObject letterPrefab;

	public GameObject levelPrefab;

	public Sprite currentLevelBGSprite;

	private float animationTime = 0.3f;

	private Coroutine anim;

	private Coroutine anim2;

	private Coroutine anim3;

	private void Awake()
	{
		instance = this;
	}

	private void CreateEmptyLevels(int levelCount)
	{
		float num = -282.5f;
		float num2 = 280.5f;
		float num3 = 188.3f;
		float num4 = 188f;
		if (levelCount > 16)
		{
			num4 = 160f;
		}
		if (levelCount > 20)
		{
			num4 = 140f;
		}
		for (int i = 0; i < levelCount; i++)
		{
			Transform transform = UnityEngine.Object.Instantiate(levelPrefab, levelHolder).transform;
			transform.localScale = Vector3.zero;
			transform.Find("TextHolder/NumberText").GetComponent<Text>().text = (i + 1).ToString();
			transform.localPosition = new Vector3(num + num3 * (float)(i % 4), num2 - num4 * (float)(i / 4), 0f);
			transform.gameObject.SetActive(value: false);
		}
	}

	public void CreateLevels(LevelSet set, bool isBlur = false)
	{
		if (isBlur)
		{
			bg.sprite = Resources.Load<Sprite>("BGImages/" + set.bgImage);
		}
		else
		{
			bg.sprite = Resources.Load<Sprite>("BGImages/" + set.bgImage);
		}
		CreateEmptyLevels(set.levels.Count);
		title.text = set.SetFullName;
		title.color = FugoUtils.HexToColor(set.TitleColor);
		ribbon.color = FugoUtils.HexToColor(set.RibbonColor);
		FugoUtils.ChangeAlpha(ribbon, 0f);
		FugoUtils.ChangeAlpha(title, 0f);
		FugoUtils.ChangeAlpha(levelsBG.GetComponent<Image>(), 0f);
		FugoUtils.ChangeAlpha(bg, 0f);
		Vector2 sizeDelta = levelsBG.GetComponent<RectTransform>().sizeDelta;
		sizeDelta.y = GetSizeOfBG(set.levels.Count);
		levelsBG.GetComponent<RectTransform>().sizeDelta = sizeDelta;
		int num = set.FirstLevel;
		for (int i = 0; i < set.levels.Count; i++)
		{
			Transform child = levelHolder.GetChild(i);
			child.name = num.ToString();
			if (num == PlayerPrefsManager.GetLevel())
			{
				SetLevelProperties(child, set, "current");
				PlaceLetters(child.Find("TextHolder"), set.levels[i].letters);
			}
			else if (num > PlayerPrefsManager.GetLevel())
			{
				SetLevelProperties(child, set, "locked");
			}
			else if (num < PlayerPrefsManager.GetLevel())
			{
				SetLevelProperties(child, set, "completed");
				PlaceLetters(child.Find("TextHolder"), set.levels[i].letters);
			}
			child.gameObject.SetActive(value: true);
			num++;
		}
		levelHolder.parent.parent.gameObject.SetActive(value: true);
		StartCoroutine(AnimateSetScreen(0.001f));
		ArabicController.MakeArabicMenu(levelHolder.parent.parent);
	}

	private void SetLevelProperties(Transform t, LevelSet set, string status)
	{
		t.GetComponent<Button>().onClick.AddListener(delegate
		{
			SetOnClick(t);
		});
		if (status == "current")
		{
			t.Find("BGHolder/Circle").GetComponent<Image>().color = FugoUtils.HexToColor(set.SelectedLevelBGColor);
			t.Find("BGHolder/Circle").GetComponent<Image>().sprite = currentLevelBGSprite;
			t.Find("BGHolder/Frame").GetComponent<Image>().color = FugoUtils.HexToColor(set.SelectedLevelBGColor);
			t.Find("BGHolder/Frame").gameObject.SetActive(value: true);
			t.Find("TextHolder/NumberText").GetComponent<Text>().color = FugoUtils.HexToColor(set.SelectedLevelNumberColor);
			t.GetComponent<SetScript>().scale = Vector3.one * 1.1f;
			anim = StartCoroutine(AnimateCurrentLevel(t, 1f));
			t.GetComponent<Button>().interactable = true;
		}
		else if (status == "locked")
		{
			t.GetComponent<Button>().interactable = false;
			t.Find("BGHolder/Circle").GetComponent<Image>().color = FugoUtils.HexToColor(set.NotCompletedLevelBGColor);
			t.Find("TextHolder/NumberText").GetComponent<Text>().color = FugoUtils.HexToColor(set.NotCompletedLevelNumberColor);
			t.GetComponent<SetScript>().scale = Vector3.one;
		}
		else if (status == "completed")
		{
			t.Find("BGHolder/Circle").GetComponent<Image>().color = FugoUtils.HexToColor(set.CompletedLevelBGColor);
			t.Find("TextHolder/NumberText").GetComponent<Text>().color = FugoUtils.HexToColor(set.CompletedLevelNumberColor);
			t.GetComponent<SetScript>().scale = Vector3.one;
			t.GetComponent<Button>().interactable = true;
		}
	}

	private void PlaceLetters(Transform holder, string word)
	{
		float num = 360f / (float)word.Length;
		float d = 42f;
		for (int i = 0; i < word.Length; i++)
		{
			Transform transform = UnityEngine.Object.Instantiate(letterPrefab, holder).transform;
			transform.localScale = Vector3.one;
			transform.GetComponent<Text>().text = word[i].ToString();
			Vector3 localPosition = new Vector3(Mathf.Cos((num * (float)(-i) + 90f) * ((float)Math.PI / 180f)), Mathf.Sin((num * (float)(-i) + 90f) * ((float)Math.PI / 180f)), 0f) * d;
			localPosition.y += 3f;
			transform.localPosition = localPosition;
		}
	}

	private float GetSizeOfBG(int count)
	{
		if (count > 20)
		{
			return 845.81f;
		}
		if (count > 16)
		{
			return 787.81f;
		}
		if (count > 12)
		{
			return 696.5f;
		}
		if (count > 8)
		{
			return 515.6f;
		}
		if (count > 4)
		{
			return 327f;
		}
		return 134f;
	}

	private IEnumerator AnimateCurrentLevel(Transform t, float delay)
	{
		yield return new WaitForSeconds(delay);
		if (t == null)
		{
			yield return null;
			yield break;
		}
		float time = 1f;
		anim2 = StartCoroutine(FugoUtils.Scaler(Vector3.one * 1f, time, t));
		yield return new WaitForSeconds(time);
		anim3 = StartCoroutine(FugoUtils.Scaler(Vector3.one * 1.1f, time, t));
		yield return new WaitForSeconds(time);
		StartCoroutine(AnimateCurrentLevel(t, 0f));
	}

	private IEnumerator AnimateSetScreen(float delay)
	{
		StartCoroutine(FugoUtils.FadeImage(1f, 0.3f, bg));
		yield return new WaitForSeconds(delay + 0.3f);
		SoundManager.instance.SlideIn();
		StartCoroutine(FugoUtils.FadeText(1f, animationTime, title));
		StartCoroutine(FugoUtils.FadeImage(1f, animationTime, levelsBG.GetComponent<Image>()));
		StartCoroutine(FugoUtils.FadeImage(1f, animationTime, ribbon));
		float levelDelay = 0.1f;
		IEnumerator enumerator = levelHolder.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform transform = (Transform)enumerator.Current;
				StartCoroutine(FugoUtils.Scaler(transform.GetComponent<SetScript>().scale, levelDelay, transform));
				levelDelay += 0.05f;
			}
		}
		finally
		{
			IDisposable disposable;
			IDisposable disposable2 = disposable = (enumerator as IDisposable);
			if (disposable != null)
			{
				disposable2.Dispose();
			}
		}
	}

	private IEnumerator AnimateSetScreenExit()
	{
		StartCoroutine(FugoUtils.FadeText(0f, animationTime, title));
		StartCoroutine(FugoUtils.FadeImage(0f, animationTime, levelsBG.GetComponent<Image>()));
		StartCoroutine(FugoUtils.FadeImage(0f, animationTime, ribbon));
		IEnumerator enumerator = levelHolder.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform go = (Transform)enumerator.Current;
				StartCoroutine(FugoUtils.Scaler(Vector3.zero, animationTime, go));
			}
		}
		finally
		{
			IDisposable disposable;
			IDisposable disposable2 = disposable = (enumerator as IDisposable);
			if (disposable != null)
			{
				disposable2.Dispose();
			}
		}
		yield return new WaitForSeconds(animationTime);
		StartCoroutine(FugoUtils.FadeImage(0f, 0.2f, bg));
		GetComponent<SectionController>().OpeningAnimation(0.18f);
		yield return new WaitForSeconds(0.2f);
		IEnumerator enumerator2 = levelHolder.GetEnumerator();
		try
		{
			while (enumerator2.MoveNext())
			{
				Transform transform = (Transform)enumerator2.Current;
				UnityEngine.Object.Destroy(transform.gameObject);
			}
		}
		finally
		{
			IDisposable disposable;
			IDisposable disposable3 = disposable = (enumerator2 as IDisposable);
			if (disposable != null)
			{
				disposable3.Dispose();
			}
		}
		if (anim != null)
		{
			StopCoroutine(anim);
		}
		levelHolder.parent.parent.gameObject.SetActive(value: false);
	}

	public void BackButtonClicked()
	{
		SoundManager.instance.Click();
		StopAllCoroutines();
		StartCoroutine(AnimateSetScreenExit());
	}

	public void SetOnClick(Transform t)
	{
		PlayerPrefsManager.SetLastPlayedMode(GameMode.NORMAL);
		SoundManager.instance.Click();
		GameController.levelToOpen = int.Parse(t.name);
		GameController.daily = false;
		MenuController.instance.GoToGameScreen();
		GameController.mode = GameMode.NORMAL;
	}
}
