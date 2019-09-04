using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using v2Gameplay;

public class HiddenSetController : MonoBehaviour
{
	public static HiddenSetController instance;

	public static bool isAnimating;

	public Transform levelHolder;

	public Transform levelsBG;

	public Transform panel;

	public Transform bigLevel;

	public Text title;

	public Image ribbon;

	public Image bg;

	public GameObject starPrefab;

	public GameObject levelPrefab;

	private float animationTime = 0.3f;

	private Transform currentSet;

	private LevelSet curSet;

	private int curLevelInGame = -1;

	public Sprite currentLevelBGSprite;

	private void Start()
	{
		instance = this;
	}

	private void Update()
	{
		if (UnityEngine.Input.GetKeyDown(KeyCode.O))
		{
			PlayerPrefsManager.SetHiddenLevel(int.Parse(curSet.SetID), 150);
		}
	}

	public void CreateLevels(LevelSet set)
	{
		curSet = set;
		title.text = set.SetFullName;
		title.color = FugoUtils.HexToColor(set.TitleColor);
		ribbon.color = FugoUtils.HexToColor(set.RibbonColor);
		bg.sprite = Resources.Load<Sprite>("BGImages/" + set.bgImage);
		FugoUtils.ChangeAlpha(ribbon, 0f);
		FugoUtils.ChangeAlpha(title, 0f);
		FugoUtils.ChangeAlpha(levelsBG.GetComponent<Image>(), 0f);
		FugoUtils.ChangeAlpha(bg, 0f);
		bigLevel.localScale = Vector3.zero;
		List<int> hiddenLevelsInSet = FugoUtils.GetHiddenLevelsInSet(set);
		CreateEmptyLevels(hiddenLevelsInSet);
		IEnumerator enumerator = levelHolder.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform level = (Transform)enumerator.Current;
				SetHiddenSetProperties(level, "locked", set);
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
		int hiddenLevel = PlayerPrefsManager.GetHiddenLevel(int.Parse(set.SetID));
		int num = 0;
		for (int i = 0; i < hiddenLevelsInSet.Count; i++)
		{
			num += hiddenLevelsInSet[i];
			if (num >= hiddenLevel)
			{
				currentSet = levelHolder.GetChild(i);
				break;
			}
		}
		int num2 = 0;
		int num3 = 1;
		IEnumerator enumerator2 = levelHolder.GetEnumerator();
		try
		{
			while (enumerator2.MoveNext())
			{
				Transform transform = (Transform)enumerator2.Current;
				if (transform == currentSet)
				{
					SetHiddenSetProperties(transform, "current", set);
					CreateEmptyBigStars(bigLevel, currentSet.Find("StarHolder").childCount);
					bigLevel.Find("LevelText").GetComponent<Text>().text = currentSet.Find("LevelText").GetComponent<Text>().text;
					StartCoroutine(AnimateCurrentLevel(transform, 0.5f));
					break;
				}
				num3 += hiddenLevelsInSet[num2];
				SetHiddenSetProperties(transform, "completed", set);
				num2++;
			}
		}
		finally
		{
			IDisposable disposable2;
			if ((disposable2 = (enumerator2 as IDisposable)) != null)
			{
				disposable2.Dispose();
			}
		}
		PaintAllChilds(bigLevel.Find("StarHolder"), FugoUtils.HexToColor(set.NotCompletedLevelBGColor));
		bigLevel.Find("LevelText").GetComponent<Text>().color = FugoUtils.HexToColor(set.SelectedLevelNumberColor);
		bigLevel.GetComponent<Image>().color = FugoUtils.HexToColor(set.SelectedLevelBGColor);
		curLevelInGame = hiddenLevel - num3 + 1;
		for (int j = 0; j < hiddenLevel - num3; j++)
		{
			if (bigLevel.Find("StarHolder").childCount > j)
			{
				bigLevel.Find("StarHolder").GetChild(j).GetComponent<Image>()
					.color = FugoUtils.HexToColor(set.CompletedLevelBGColor);
				}
			}
			panel.gameObject.SetActive(value: true);
			StartCoroutine(AnimateSetScreen());
			if (currentSet == null)
			{
				CreateEmptyBigStars(bigLevel, hiddenLevelsInSet[hiddenLevelsInSet.Count - 1]);
				bigLevel.Find("LevelText").GetComponent<Text>().text = hiddenLevelsInSet.Count.ToString();
				PaintAllChilds(bigLevel.Find("StarHolder"), FugoUtils.HexToColor(set.CompletedLevelBGColor));
			}
		}

		private void SetHiddenSetProperties(Transform level, string type, LevelSet set)
		{
			if (type == "locked")
			{
				level.Find("Locked").gameObject.SetActive(value: true);
				level.Find("StarHolder").gameObject.SetActive(value: false);
				level.Find("Locked").GetComponent<Image>().color = FugoUtils.HexToColor(set.CompletedLevelBGColor);
				level.Find("Locked/Image").GetComponent<Image>().color = FugoUtils.HexToColor(set.NotCompletedLevelNumberColor);
				level.GetComponent<Button>().onClick.RemoveAllListeners();
			}
			else if (type == "current")
			{
				level.Find("Locked").gameObject.SetActive(value: false);
				level.Find("StarHolder").gameObject.SetActive(value: false);
				level.GetComponent<Image>().color = FugoUtils.HexToColor(set.SelectedLevelBGColor);
				level.GetComponent<Image>().sprite = currentLevelBGSprite;
				level.Find("LevelText").GetComponent<Text>().color = FugoUtils.HexToColor(set.SelectedLevelNumberColor);
				level.GetComponent<Button>().onClick.AddListener(delegate
				{
					HiddenSetOnClick(level, set, isCurrent: true);
				});
				bigLevel.GetComponent<Button>().onClick.RemoveAllListeners();
				bigLevel.GetComponent<Button>().onClick.AddListener(delegate
				{
					HiddenSetOnClick(level, set, isCurrent: true);
				});
			}
			else if (type == "completed")
			{
				level.Find("StarHolder").gameObject.SetActive(value: true);
				level.Find("Locked").gameObject.SetActive(value: false);
				level.GetComponent<Image>().color = FugoUtils.HexToColor(set.CompletedLevelBGColor);
				level.Find("LevelText").GetComponent<Text>().color = FugoUtils.HexToColor(set.CompletedLevelNumberColor);
				PaintAllChilds(level.Find("StarHolder"), FugoUtils.HexToColor(set.CompletedLevelLetterColor));
				level.GetComponent<Button>().onClick.AddListener(delegate
				{
					HiddenSetOnClick(level, set);
				});
			}
		}

		private void CreateEmptyLevels(List<int> sets)
		{
			IEnumerator enumerator = levelHolder.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Transform transform = (Transform)enumerator.Current;
					UnityEngine.Object.Destroy(transform.gameObject);
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
			for (int i = 0; i < sets.Count; i++)
			{
				Transform transform2 = UnityEngine.Object.Instantiate(levelPrefab, levelHolder).transform;
				transform2.localScale = Vector3.zero;
				transform2.name = (i + 1).ToString();
				transform2.Find("LevelText").GetComponent<Text>().text = transform2.name;
				CreateEmptyStars(transform2, sets[i]);
			}
		}

		private void CreateEmptyStars(Transform t, int count)
		{
			float num = 36f;
			float d = 35f;
			for (int i = 0; i < count; i++)
			{
				Transform transform = UnityEngine.Object.Instantiate(starPrefab, t.Find("StarHolder")).transform;
				transform.localScale = Vector3.one;
				float num2 = (count % 2 != 1) ? ((float)(i - count / 2) + 0.5f) : ((float)(i - count / 2));
				Vector3 localPosition = new Vector3(Mathf.Cos((num * num2 - 90f) * ((float)Math.PI / 180f)), Mathf.Sin((num * num2 - 90f) * ((float)Math.PI / 180f)), 0f) * d;
				localPosition.y += 3f;
				transform.localPosition = localPosition;
			}
		}

		private void CreateEmptyBigStars(Transform t, int count)
		{
			float num = 36f;
			float d = 120f;
			IEnumerator enumerator = t.Find("StarHolder").GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Transform transform = (Transform)enumerator.Current;
					UnityEngine.Object.Destroy(transform.gameObject);
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
			for (int i = 0; i < count; i++)
			{
				Transform transform2 = UnityEngine.Object.Instantiate(starPrefab, t.Find("StarHolder")).transform;
				transform2.localScale = Vector3.one;
				transform2.GetComponent<RectTransform>().sizeDelta = Vector2.one * 60f;
				float num2 = (count % 2 != 1) ? ((float)(i - count / 2) + 0.5f) : ((float)(i - count / 2));
				Vector3 localPosition = new Vector3(Mathf.Cos((num * num2 - 90f) * ((float)Math.PI / 180f)), Mathf.Sin((num * num2 - 90f) * ((float)Math.PI / 180f)), 0f) * d;
				localPosition.y += 12f;
				transform2.localPosition = localPosition;
			}
		}

		private void ArrangeScrollPosition()
		{
			if (currentSet == null)
			{
				levelHolder.parent.parent.GetComponent<ScrollRect>().horizontalNormalizedPosition = 0f;
				return;
			}
			Vector3 position = bigLevel.transform.position;
			Vector3 position2 = currentSet.transform.position;
			Vector3 position3 = levelHolder.transform.position;
			position3.x += position.x - position2.x;
			levelHolder.transform.position = position3;
		}

		private IEnumerator AnimateSetScreen()
		{
			StartCoroutine(FugoUtils.FadeImage(1f, 0.3f, bg));
			yield return new WaitForSeconds(0.3f);
			SoundManager.instance.SlideIn();
			StartCoroutine(FugoUtils.FadeText(1f, animationTime, title));
			StartCoroutine(FugoUtils.FadeImage(1f, animationTime, levelsBG.GetComponent<Image>()));
			StartCoroutine(FugoUtils.FadeImage(1f, animationTime, ribbon));
			StartCoroutine(FugoUtils.Scaler(Vector3.one, animationTime, bigLevel));
			float levelDelay = 0.1f;
			IEnumerator enumerator = levelHolder.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Transform go = (Transform)enumerator.Current;
					StartCoroutine(FugoUtils.Scaler(Vector3.one, levelDelay, go));
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
			ArrangeScrollPosition();
		}

		public void BackButtonClicked()
		{
			SoundManager.instance.Click();
			StopAllCoroutines();
			StartCoroutine(AnimateSetScreenExit());
		}

		private IEnumerator AnimateSetScreenExit()
		{
			isAnimating = true;
			StartCoroutine(FugoUtils.FadeText(0f, animationTime, title));
			StartCoroutine(FugoUtils.FadeImage(0f, animationTime, levelsBG.GetComponent<Image>()));
			StartCoroutine(FugoUtils.FadeImage(0f, animationTime, ribbon));
			StartCoroutine(FugoUtils.Scaler(Vector3.zero, animationTime, bigLevel));
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
			panel.gameObject.SetActive(value: false);
			yield return new WaitForSeconds(0.5f);
			StopAllCoroutines();
			isAnimating = false;
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
			StartCoroutine(FugoUtils.Scaler(Vector3.one * 1f, time, t));
			yield return new WaitForSeconds(time);
			StartCoroutine(FugoUtils.Scaler(Vector3.one * 1.1f, time, t));
			yield return new WaitForSeconds(time);
			StartCoroutine(AnimateCurrentLevel(t, 0f));
		}

		private void PaintAllChilds(Transform parent, Color c)
		{
			IEnumerator enumerator = parent.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Transform transform = (Transform)enumerator.Current;
					transform.GetComponent<Image>().color = c;
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
		}

		public void HiddenSetOnClick(Transform t, LevelSet set, bool isCurrent = false)
		{
			PlayerPrefsManager.SetLastPlayedMode(GameMode.ADVENTURE);
			GameController.daily = false;
			SoundManager.instance.Click();
			GameController.mode = GameMode.ADVENTURE;
			GameController.adventureSectionId = set.SectionID;
			GameController.adventureSetId = set.SetID;
			GameController.adventureGameId = t.Find("LevelText").GetComponent<Text>().text;
			if (isCurrent)
			{
				GameController.slidePos = curLevelInGame;
			}
			else
			{
				GameController.slidePos = 0;
			}
			MenuController.instance.GoToGameScreen();
			GameController.adventureCount = levelHolder.childCount;
		}
	}
