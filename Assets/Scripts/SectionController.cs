using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using v2Gameplay;

public class SectionController : MonoBehaviour
{
	public static SectionController instance;

	public static bool setCompleted;

	public static bool isTutorial;

	public static int hiddenSetCompleted = -1;

	public GameObject setPrefab;

	public GameObject sectionPrefab;

	public GameObject panel;

	public GameObject particle;

	public GameObject dotPrefab;

	public GameObject linePrefab;

	public GameObject hiddenSetPrefab;

	public GameObject hiddenDotPrefab;

	public Transform list;

	public Sprite currentSetSprite;

	public Sprite normalSetSprite;

	public Transform gallery;

	public Transform hiddenPanel;

	public Sprite[] sectionBGs;

	public static float canvasWidth;

	public static float canvasHeight;

	public static float ratio;

	private Transform currentSet;

	private Transform nextSet;

	private Transform currentSection;

	private int curLevel;

	private bool listMoved;

	private GameObject blinkPin;

	private Coroutine listMoveCoroutine;

	private Transform selectedHiddenLevel;

	private bool arabicFlag;

	private void Awake()
	{
		if (hiddenSetCompleted != -1)
		{
			setCompleted = false;
		}
		listMoved = false;
		particle.SetActive(value: false);
		curLevel = PlayerPrefsManager.GetLevel();
		if (setCompleted)
		{
			curLevel--;
		}
		instance = this;
	}

	private void Start()
	{
		RectTransform component = GameObject.Find("Canvas").GetComponent<RectTransform>();
		Vector2 sizeDelta = component.sizeDelta;
		canvasWidth = sizeDelta.x;
		Vector2 sizeDelta2 = component.sizeDelta;
		canvasHeight = sizeDelta2.y;
		ratio = canvasWidth / canvasHeight / 0.5625f;
		if (Games.sections != null && Games.sections.Count != 0)
		{
			CreateSections();
		}
	}

	public void CreateSections()
	{
		IEnumerator enumerator = list.GetEnumerator();
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
		int num = 1;
		int num2 = 0;
		bool flag = false;
		Transform transform2 = null;
		Transform transform3 = null;
		for (int i = 0; i < Games.sections.Count; i++)
		{
			Transform transform4 = UnityEngine.Object.Instantiate(sectionPrefab, list).transform;
			transform4.name = Games.sections[i].SectionID;
			SetSectionProperties(Games.sections[i], transform4);
			if (i == Games.sections.Count - 1 && currentSection == null)
			{
				currentSection = transform4;
				num2 -= FugoUtils.GetLevelCount(Games.sections[i - 1]);
			}
			if (num > curLevel)
			{
				if (currentSection == null)
				{
					currentSection = transform3;
					num2 -= FugoUtils.GetLevelCount(Games.sections[i - 1]);
				}
				SetSectionStatus(Games.sections[i], transform4, "locked");
				int levelCount = FugoUtils.GetLevelCount(Games.sections[i]);
				transform4.Find("LevelCounter").GetComponent<Text>().text = "0/" + levelCount.ToString();
			}
			else
			{
				int levelCount2 = FugoUtils.GetLevelCount(Games.sections[i]);
				transform4.Find("LevelCounter").GetComponent<Text>().text = levelCount2.ToString() + "/" + levelCount2.ToString();
				num2 += FugoUtils.GetLevelCount(Games.sections[i]);
			}
			int count = Games.sections[i].sets.Count;
			int num3 = -1 * (count / 2);
			for (int j = 0; j < count; j++)
			{
				Transform set = UnityEngine.Object.Instantiate(setPrefab, transform4.transform.Find("SetHolder")).transform;
				SetScript component = set.GetComponent<SetScript>();
				component.set = Games.sections[i].sets[j];
				component.section = Games.sections[i];
				component.setColor = FugoUtils.HexToColor(Games.sections[i].sets[j].SetColor);
				set.Find("BGHolder/Circle").GetComponent<Image>().color = FugoUtils.HexToColor(Games.sections[i].sets[j].SetColor);
				set.Find("BGHolder/Frame").GetComponent<Image>().color = FugoUtils.HexToColor(Games.sections[i].sets[j].SetColor);
				set.Find("Magnifier").GetComponent<Image>().color = FugoUtils.HexToColor(Games.sections[i].KeyColor);
				set.Find("Footsteps").GetComponent<Image>().color = FugoUtils.HexToColor(Games.sections[i].sets[j].SetColor);
				Text component2 = set.Find("NameText").GetComponent<Text>();
				component2.color = FugoUtils.HexToColor(Games.sections[i].SetTitleColor);
				component2.text = Games.sections[i].sets[j].SetName;
				if (PlayerPrefsManager.GetLang() == "Hebrew")
				{
					component2.lineSpacing = -1f;
				}
				set.localScale = Vector3.one;
				float num4 = 220f;
				if (count == 3)
				{
					num4 = 110f;
				}
				set.localPosition = new Vector3(150f, num4 - (float)j * 110f, 0f);
				Transform transform5 = UnityEngine.Object.Instantiate(dotPrefab, transform4.transform.Find("DotHolder")).transform;
				transform5.localScale = Vector3.one;
				Vector3 pos = Games.sections[i].sets[j].pos;
				transform5.localPosition = pos * ratio;
				SetScript component3 = set.GetComponent<SetScript>();
				component3.dot = transform5;
				component3.line = DrawLine(transform4.transform.Find("LineHolder"), set.localPosition, transform5);
				transform5.GetChild(1).GetComponent<Image>().color = FugoUtils.HexToColor(Games.sections[i].sets[j].SetColor);
				Text component4 = transform5.Find("Dot/WhiteCircle/Text").GetComponent<Text>();
				component4.text = Games.sections[i].sets[j].SetName.Substring(0, 1);
				Color color = FugoUtils.HexToColor(Games.sections[i].sets[j].SetColor);
				color.a = 0.5f;
				component4.color = color;
				set.GetComponent<SetScript>().line.GetComponent<Image>().color = FugoUtils.HexToColor(Games.sections[i].sets[j].SetColor);
				Games.sections[i].sets[j].FirstLevel = num;
				num3++;
				if (num > curLevel)
				{
					LockSet(set, FugoUtils.HexToColor(Games.sections[i].KeyColor));
					if (nextSet == null)
					{
						nextSet = set;
					}
					if (transform2 != null && !flag)
					{
						flag = true;
						CurrentSet(transform2);
						currentSet = transform2;
					}
				}
				else if (i == Games.sections.Count - 1 && j == count - 1)
				{
					flag = true;
					CurrentSet(set);
					currentSet = set;
				}
				else
				{
					set.GetComponent<Button>().onClick.AddListener(delegate
					{
						CompletedSetOnClickFunc(set.GetComponent<SetScript>().set);
					});
				}
				num += Games.sections[i].sets[j].levels.Count;
				transform2 = set;
			}
			if (Games.sections[i].hiddensets.Count > 0)
			{
				int count2 = Games.sections[i].hiddensets.Count;
				for (int k = 0; k < count2; k++)
				{
					Transform hidden = UnityEngine.Object.Instantiate(hiddenSetPrefab, transform4.transform.Find("HiddenSetHolder")).transform;
					hidden.localScale = Vector3.one;
					hidden.localPosition = new Vector3(-135f, 220f - (float)k * 110f, 0f);
					hidden.GetComponent<SetScript>().set = Games.sections[i].hiddensets[k];
					hidden.GetComponent<SetScript>().set.section = Games.sections[i];
					hidden.Find("NotPaid/PriceText").GetComponent<Text>().text = Games.sections[i].hiddensets[k].Price.ToString();
					Transform transform6 = UnityEngine.Object.Instantiate(hiddenDotPrefab, transform4.transform.Find("HiddenDotHolder")).transform;
					transform6.localScale = Vector3.one;
					Vector3 pos2 = Games.sections[i].hiddensets[k].pos;
					transform6.localPosition = pos2 * ratio;
					transform6.name = Games.sections[i].hiddensets[k].SetID;
					hidden.GetComponent<SetScript>().dot = transform6;
					hidden.SetParent(transform4.transform.Find("SetHolder"));
					Transform transform7 = DrawLine(transform4.transform.Find("HiddenLineHolder"), hidden.localPosition, transform6);
					hidden.SetParent(transform4.transform.Find("HiddenSetHolder"));
					transform7.name = Games.sections[i].hiddensets[k].SetID;
					hidden.GetComponent<SetScript>().line = transform7;
					transform6.GetChild(1).GetComponent<Image>().color = FugoUtils.HexToColor(Games.sections[i].hiddensets[k].SetColor);
					transform7.GetComponent<Image>().color = FugoUtils.HexToColor(Games.sections[i].hiddensets[k].SetColor);
					hidden.GetComponent<Button>().onClick.AddListener(delegate
					{
						HiddenLevelOnClick(hidden);
					});
				}
			}
			if (num <= curLevel)
			{
				SetSectionStatus(Games.sections[i], transform4, "completed");
			}
			transform3 = transform4;
		}
		IEnumerator enumerator2 = list.GetEnumerator();
		try
		{
			while (enumerator2.MoveNext())
			{
				Transform transform8 = (Transform)enumerator2.Current;
				IEnumerator enumerator3 = transform8.Find("HiddenSetHolder").GetEnumerator();
				try
				{
					while (enumerator3.MoveNext())
					{
						Transform hiddenSetProperties = (Transform)enumerator3.Current;
						SetHiddenSetProperties(hiddenSetProperties);
					}
				}
				finally
				{
					IDisposable disposable2;
					if ((disposable2 = (enumerator3 as IDisposable)) != null)
					{
						disposable2.Dispose();
					}
				}
			}
		}
		finally
		{
			IDisposable disposable3;
			if ((disposable3 = (enumerator2 as IDisposable)) != null)
			{
				disposable3.Dispose();
			}
		}
		Text component5 = currentSection.Find("LevelCounter").GetComponent<Text>();
		string text = component5.text;
		component5.text = (PlayerPrefsManager.GetLevel() - num2 - 1).ToString() + "/" + text.Split('/')[1];
		Image component6 = currentSection.Find("Cover").GetComponent<Image>();
		Color color2 = component6.color;
		color2.a = 146f / 255f;
		component6.color = color2;
		if (panel.activeSelf)
		{
			OpeningAnimation(0.0002f);
		}
		if (setCompleted)
		{
			if (PlayerPrefsManager.GetLevel() > PlayerPrefsManager.CountLevels())
			{
				setCompleted = false;
			}
			else
			{
				int num5 = FugoUtils.getLevelInfo(curLevel)[0];
				int num6 = FugoUtils.getLevelInfo(curLevel + 1)[1];
				if (num6 == 2)
				{
					RateUsManager.instance.RateUsPopup();
				}
				list.localPosition = new Vector3(0f, 600f * (float)num5, 0f);
				listMoved = true;
			}
		}
		if (hiddenSetCompleted != -1)
		{
			list.localPosition = new Vector3(0f, 600f * (float)FugoUtils.GetHiddenSetSectionIndex(hiddenSetCompleted), 0f);
			listMoved = true;
		}
		if (!listMoved)
		{
			int num7 = Games.sections.Count - 1;
			if (FugoUtils.getLevelInfo(curLevel) != null)
			{
				num7 = FugoUtils.getLevelInfo(curLevel)[0];
			}
			if (GameController.levelToOpen > 0)
			{
				num7 = FugoUtils.getLevelInfo(GameController.levelToOpen)[0];
			}
			float num8 = Mathf.Clamp(0.2f * (float)num7, 0.5f, 200f);
			if (setCompleted)
			{
				num8 = 0f;
			}
			list.localPosition = new Vector3(0f, 600f * (float)num7, 0f);
			if (PlayerPrefsManager.GetLastPlayedMode() == GameMode.ADVENTURE)
			{
				list.localPosition = new Vector3(0f, 600f * (float)FugoUtils.GetHiddenSetSectionIndex(int.Parse(PlayerPrefsManager.GetLastHiddenSet())), 0f);
			}
			listMoved = true;
		}
		StartCoroutine(MakeArabic());
	}

	private IEnumerator MakeArabic()
	{
		yield return new WaitForSeconds(2f);
	}

	private Transform DrawLine(Transform parent, Vector3 startPos, Transform dot)
	{
		Transform parent2 = dot.parent;
		Transform transform = UnityEngine.Object.Instantiate(linePrefab, parent).transform;
		dot.SetParent(parent);
		float x = startPos.x;
		Vector3 localPosition = dot.localPosition;
		float num = x - localPosition.x;
		float y = startPos.y;
		Vector3 localPosition2 = dot.localPosition;
		float num2 = y - localPosition2.y;
		dot.SetParent(parent2);
		float x2 = Mathf.Sqrt(num * num + num2 * num2);
		Vector2 sizeDelta = transform.GetComponent<RectTransform>().sizeDelta;
		sizeDelta.x = x2;
		transform.GetComponent<RectTransform>().sizeDelta = sizeDelta;
		transform.localPosition = startPos;
		float num3 = Mathf.Atan2(num2, num);
		transform.eulerAngles = new Vector3(0f, 0f, 180f + 57.29578f * num3);
		return transform;
	}

	private void LockSet(Transform t, Color c)
	{
		t.Find("Key").gameObject.SetActive(value: true);
		t.Find("Magnifier").gameObject.SetActive(value: false);
		t.Find("Key").GetComponent<Image>().color = c;
		t.Find("NameText").gameObject.SetActive(value: false);
		t.GetComponent<SetScript>().line.gameObject.SetActive(value: false);
		t.GetComponent<SetScript>().dot.gameObject.SetActive(value: false);
		t.localScale = Vector3.one;
	}

	private void SetSectionProperties(Section s, Transform t)
	{
		Vector2 sizeDelta = t.GetComponent<RectTransform>().sizeDelta;
		sizeDelta.x = canvasWidth;
		t.GetComponent<RectTransform>().sizeDelta = sizeDelta;
		t.localScale = Vector3.one;
		t.Find("Cover").GetComponent<Image>().color = FugoUtils.HexToColor(s.BGCoverColorCurrent);
		Vector2 sizeDelta2 = t.Find("TitleBG").GetComponent<RectTransform>().sizeDelta;
		sizeDelta2.x = 75f * Mathf.Clamp(ratio, 0f, 1f);
		t.Find("TitleBG").GetComponent<RectTransform>().sizeDelta = sizeDelta2;
		t.Find("CompleteBG").GetComponent<RectTransform>().sizeDelta = sizeDelta2;
		t.Find("TitleBG/Title").GetComponent<Text>().color = FugoUtils.HexToColor(s.TitleColor);
		t.Find("LevelCounter").GetComponent<Text>().color = FugoUtils.HexToColor(s.TextColor);
		t.Find("CompleteBG/Title").GetComponent<Text>().color = FugoUtils.HexToColor(s.TextColor);
		t.Find("CompleteBG/Title").GetComponent<Text>().text = LanguageScript.CompletedText;
		t.Find("TitleBG/Title").GetComponent<Text>().text = s.Title;
		t.Find("Map").GetComponent<Image>().sprite = Resources.Load<Sprite>("Maps/" + s.MapName);
		t.Find("Map").GetComponent<Image>().color = FugoUtils.HexToColor(s.MapColor);
		t.Find("BG").GetComponent<Image>().sprite = GetSectionBGImage(s.BGName);
		t.Find("TitleBG").GetComponent<Image>().color = FugoUtils.HexToColor(s.TitleBGColor);
		Vector2 sizeDelta3 = t.Find("Map").GetComponent<RectTransform>().sizeDelta;
		sizeDelta3 *= ratio;
		t.Find("Map").GetComponent<RectTransform>().sizeDelta = sizeDelta3;
		t.Find("SetHolder").transform.localScale = Mathf.Clamp(ratio, 0f, 1f) * Vector3.one;
		t.Find("HiddenSetHolder").transform.localScale = Mathf.Clamp(ratio, 0f, 1f) * Vector3.one;
		t.Find("LineHolder").transform.localScale = Mathf.Clamp(ratio, 0f, 1f) * Vector3.one;
		t.Find("HiddenLineHolder").transform.localScale = Mathf.Clamp(ratio, 0f, 1f) * Vector3.one;
	}

	private void SetSectionStatus(Section s, Transform t, string status)
	{
		if (status == "completed")
		{
			t.Find("TitleBG/Title").GetComponent<Text>().color = FugoUtils.HexToColor(s.TextColor);
			t.Find("CompleteBG").gameObject.SetActive(value: true);
			FugoUtils.ChangeAlpha(t.Find("CompleteBG").GetComponent<Image>(), 0f);
			FugoUtils.ChangeAlpha(t.Find("CompleteBG/Title").GetComponent<Text>(), 0f);
			FugoUtils.ChangeAlpha(t.Find("CompleteBG/Image").GetComponent<Image>(), 0f);
			t.Find("LevelCounter").gameObject.SetActive(value: false);
		}
		else if (status == "locked")
		{
			t.Find("Cover").GetComponent<Image>().color = FugoUtils.HexToColor(s.BGCoverColor);
			t.Find("Map").GetComponent<Image>().color = FugoUtils.HexToColor(s.MapLockedColor);
			t.Find("CompleteBG").gameObject.SetActive(value: false);
			t.Find("SetHolder").gameObject.SetActive(value: false);
			t.Find("TitleBG/Title").GetComponent<Text>().text = LanguageScript.UnexploredText;
			t.Find("LevelCounter").gameObject.SetActive(value: false);
			t.Find("DotHolder").gameObject.SetActive(value: false);
			t.Find("LineHolder").gameObject.SetActive(value: false);
			t.Find("Map/Qmark").gameObject.SetActive(value: true);
			t.Find("HiddenSetHolder").gameObject.SetActive(value: false);
			t.Find("HiddenDotHolder").gameObject.SetActive(value: false);
			t.Find("HiddenLineHolder").gameObject.SetActive(value: false);
		}
	}

	private IEnumerator UnlockSection(Transform t, Section s)
	{
		float time = 1f;
		StartCoroutine(FugoUtils.FadeImage(0f, time, t.Find("Map/Qmark").GetComponent<Image>()));
		StartCoroutine(FugoUtils.FadeImage(0f, time, t.Find("Cover").GetComponent<Image>()));
		StartCoroutine(FugoUtils.FadeText(0f, time, t.Find("TitleBG/Title").GetComponent<Text>()));
		StartCoroutine(FugoUtils.Blend(FugoUtils.HexToColor(s.MapColor), time, t.Find("Map").GetComponent<Image>()));
		IEnumerator enumerator = t.Find("SetHolder").GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform transform = (Transform)enumerator.Current;
				transform.localScale = Vector3.zero;
				StartCoroutine(FugoUtils.Scaler(Vector3.one, time, transform));
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
		t.Find("SetHolder").gameObject.SetActive(value: true);
		yield return new WaitForSeconds(time);
		t.Find("TitleBG/Title").GetComponent<Text>().text = s.Title;
		StartCoroutine(FugoUtils.FadeText(1f, time, t.Find("TitleBG/Title").GetComponent<Text>()));
	}

	private void CurrentSet(Transform t)
	{
		t.Find("BGHolder/Frame").gameObject.SetActive(value: true);
		Color color = t.Find("BGHolder/Circle").GetComponent<Image>().color;
		t.Find("BGHolder/Circle").GetComponent<Image>().color = Color.white;
		t.Find("BGHolder/Circle").GetComponent<Image>().sprite = currentSetSprite;
		t.Find("BGHolder/Frame").GetComponent<Image>().color = color;
		t.Find("Footsteps").gameObject.SetActive(value: true);
		t.Find("Magnifier").gameObject.SetActive(value: false);
		t.Find("NameText").GetComponent<Text>().color = color;
		Button component = t.GetComponent<Button>();
		component.onClick.RemoveAllListeners();
		component.onClick.AddListener(delegate
		{
			SetOnClickFunc(t.GetComponent<SetScript>().set);
		});
		t.localScale = Vector3.zero;
		t.GetComponent<SetScript>().dot.GetComponent<Button>().onClick.RemoveAllListeners();
		t.GetComponent<SetScript>().dot.GetComponent<Button>().onClick.AddListener(delegate
		{
			CurrentSetDotOnClick();
		});
	}

	private void CurrentSetDotOnClick()
	{
		PlayerPrefsManager.SetLastPlayedMode(GameMode.NORMAL);
		MenuController.instance.PlayButtonClicked();
	}

	private IEnumerator AnimateCurrentSet(Transform t)
	{
		float time = 1f;
		StartCoroutine(FugoUtils.Scaler(Vector3.one * 1.1f, time, t));
		yield return new WaitForSeconds(time);
		StartCoroutine(FugoUtils.Scaler(Vector3.one * 1f, time, t));
		yield return new WaitForSeconds(time);
		StartCoroutine(AnimateCurrentSet(t));
	}

	private void SetOnClickFunc(LevelSet set)
	{
		SoundManager.instance.Click();
		SetController.instance.CreateLevels(set, isBlur: true);
	}

	private void GallerySetOnClickFunc(LevelSet set)
	{
		SoundManager.instance.Click();
		SetController.instance.CreateLevels(set);
		CloseGallery(0.3f, destroyAnim: false);
	}

	private void GalleryHiddenSetOnClickFunc(LevelSet set)
	{
		SoundManager.instance.Click();
		HiddenSetController.instance.CreateLevels(set);
		CloseGallery(0.3f, destroyAnim: false);
	}

	private void CompletedSetOnClickFunc(LevelSet set, bool isHidden = false)
	{
		SoundManager.instance.Click();
		gallery.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("BGImages/" + set.bgImage);
		gallery.Find("TextBG/TopText").GetComponent<Text>().text = set.TopText;
		gallery.Find("TextBG/BotText").GetComponent<Text>().text = set.BottomText;
		Image[] componentsInChildren = gallery.GetComponentsInChildren<Image>();
		Text[] componentsInChildren2 = gallery.GetComponentsInChildren<Text>();
		Image[] array = componentsInChildren;
		foreach (Image image in array)
		{
			if (!image.name.ToLower().Contains("button"))
			{
				FugoUtils.ChangeAlpha(image, 0f);
				StartCoroutine(FugoUtils.FadeImage(1f, 0.3f, image));
			}
		}
		Text[] array2 = componentsInChildren2;
		foreach (Text text in array2)
		{
			FugoUtils.ChangeAlpha(text, 0f);
			StartCoroutine(FugoUtils.FadeText(1f, 0.3f, text));
		}
		Button component = gallery.Find("SetSceneButton").GetComponent<Button>();
		component.onClick.RemoveAllListeners();
		if (isHidden)
		{
			component.onClick.AddListener(delegate
			{
				GalleryHiddenSetOnClickFunc(set);
			});
		}
		else
		{
			component.onClick.AddListener(delegate
			{
				GallerySetOnClickFunc(set);
			});
		}
		ArabicController.MakeArabicMenu(gallery);
		gallery.gameObject.SetActive(value: true);
	}

	public void GalleryImageOnClick()
	{
		CloseGallery(0.001f, destroyAnim: true);
	}

	private void CloseGallery(float delay, bool destroyAnim)
	{
		if (destroyAnim)
		{
		}
		SoundManager.instance.Click();
		if (delay < 0.1f)
		{
			SoundManager.instance.SlideIn();
		}
		StartCoroutine(CloseGalleryThread(delay));
	}

	private IEnumerator CloseGalleryThread(float delay)
	{
		yield return new WaitForSeconds(delay);
		Image[] images = gallery.GetComponentsInChildren<Image>();
		Text[] texts = gallery.GetComponentsInChildren<Text>();
		Image[] array = images;
		foreach (Image go in array)
		{
			StartCoroutine(FugoUtils.FadeImage(0f, 0.3f, go));
		}
		Text[] array2 = texts;
		foreach (Text go2 in array2)
		{
			StartCoroutine(FugoUtils.FadeText(0f, 0.3f, go2));
		}
		yield return new WaitForSeconds(0.3f);
		gallery.gameObject.SetActive(value: false);
	}

	public void BackButtonOnClick()
	{
		SoundManager.instance.Click();
		panel.SetActive(value: false);
		MenuController.instance.AnimateMenu();
	}

	public void OpeningAnimation(float delay)
	{
		StopAllCoroutines();
		StartCoroutine(OpeningAnimThread(delay));
	}

	private IEnumerator OpeningAnimThread(float delay)
	{
		IEnumerator enumerator = list.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform transform = (Transform)enumerator.Current;
				Transform transform2 = transform.Find("TitleBG/Title");
				Transform transform3 = transform.Find("LevelCounter");
				FugoUtils.ChangeAlpha(transform2.GetComponent<Text>(), 0f);
				FugoUtils.ChangeAlpha(transform3.GetComponent<Text>(), 0f);
				StartCoroutine(DelayAnimThread(transform2, delay, "fadetext"));
				StartCoroutine(DelayAnimThread(transform3, delay, "fadetext"));
				if (transform.Find("CompleteBG").gameObject.activeSelf)
				{
					SectionCompletedAnim(transform, 0.3f);
				}
				if (delay > 0.001f)
				{
					SoundManager.instance.SlideIn();
				}
				float num = 0.05f;
				IEnumerator enumerator2 = transform.Find("SetHolder").GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						Transform transform4 = (Transform)enumerator2.Current;
						if (transform4.name.Contains("Set"))
						{
							transform4.localScale = Vector3.zero;
							StartCoroutine(DelayAnimThread(transform4, num, "scale"));
							num += 0.05f;
						}
					}
				}
				finally
				{
					IDisposable disposable;
					if ((disposable = (enumerator2 as IDisposable)) != null)
					{
						disposable.Dispose();
					}
				}
				num = 0.05f;
				IEnumerator enumerator3 = transform.Find("LineHolder").GetEnumerator();
				try
				{
					while (enumerator3.MoveNext())
					{
						Transform transform5 = (Transform)enumerator3.Current;
						FugoUtils.ChangeAlpha(transform5.GetComponent<Image>(), 0f);
						StartCoroutine(DelayAnimThread(transform5, num, "fadeimg"));
						num += 0.05f;
					}
				}
				finally
				{
					IDisposable disposable2;
					if ((disposable2 = (enumerator3 as IDisposable)) != null)
					{
						disposable2.Dispose();
					}
				}
				num = 0.05f;
				IEnumerator enumerator4 = transform.Find("DotHolder").GetEnumerator();
				try
				{
					while (enumerator4.MoveNext())
					{
						Transform transform6 = (Transform)enumerator4.Current;
						FugoUtils.ChangeAlpha(transform6.GetChild(1).GetComponent<Image>(), 0f);
						StartCoroutine(DelayAnimThread(transform6.GetChild(1), num, "fadeimg"));
						FugoUtils.ChangeAlpha(transform6.GetChild(0).GetComponent<Image>(), 0f);
						StartCoroutine(DelayAnimThread(transform6.GetChild(0), num, "fadeimg"));
						num += 0.05f;
					}
				}
				finally
				{
					IDisposable disposable3;
					if ((disposable3 = (enumerator4 as IDisposable)) != null)
					{
						disposable3.Dispose();
					}
				}
			}
		}
		finally
		{
			IDisposable disposable4;
			IDisposable disposable5 = disposable4 = (enumerator as IDisposable);
			if (disposable4 != null)
			{
				disposable5.Dispose();
			}
		}
		if (setCompleted || MenuController.fromGameToSections)
		{
			MenuController.fromGameToSections = false;
			MenuController.instance.AfterGameFadeIn(currentSet.GetComponent<SetScript>().set);
			yield return new WaitForSeconds(0.3f);
		}
		yield return new WaitForSeconds(0.25f);
		if (!setCompleted)
		{
			yield return new WaitForSeconds(0.25f);
			StartCoroutine(AnimateCurrentSet(currentSet));
			StartCoroutine(CurrentPlace(currentSet.GetComponent<SetScript>().dot));
			StartCoroutine(CurrentPlaceShadow(currentSet.GetComponent<SetScript>().dot));
		}
		else if (setCompleted)
		{
			if (PlayerPrefsManager.GetLevel() < 10)
			{
				isTutorial = true;
			}
			list.parent.parent.GetComponent<ScrollRect>().vertical = false;
			if (nextSet.parent != currentSet.parent)
			{
				int nextSection = FugoUtils.getLevelInfo()[0];
				StartCoroutine(UnlockSection(nextSet.parent.parent, Games.sections[nextSection]));
				yield return new WaitForSeconds(1.1f);
			}
			SoundManager.instance.SetUnlocked();
			float speed = 0.3f;
			particle.transform.position = currentSet.transform.position;
			particle.GetComponent<ParticleSystem>().startColor = currentSet.GetComponent<SetScript>().setColor;
			Vector3 curPos = currentSet.localPosition;
			curPos.z = -100f;
			currentSet.localPosition = curPos;
			particle.SetActive(value: true);
			yield return new WaitForSeconds(0.6f);
			StartCoroutine(FugoUtils.Scaler(Vector3.one, speed, currentSet));
			currentSet.Find("BGHolder/Circle").GetComponent<Image>().sprite = normalSetSprite;
			StartCoroutine(FugoUtils.Blend(currentSet.GetComponent<SetScript>().setColor, speed, currentSet.Find("BGHolder/Circle").GetComponent<Image>()));
			StartCoroutine(FugoUtils.FadeImage(0f, speed, currentSet.Find("Footsteps").GetComponent<Image>()));
			StartCoroutine(FugoUtils.Blend(Color.white, speed, currentSet.Find("NameText").GetComponent<Text>()));
			StartCoroutine(FugoUtils.MoverWorldPos(nextSet.position, speed, currentSet.Find("BGHolder/Frame")));
			yield return new WaitForSeconds(speed);
			FugoUtils.ChangeAlpha(currentSet.Find("Magnifier").GetComponent<Image>(), 0f);
			currentSet.Find("Magnifier").gameObject.SetActive(value: true);
			StartCoroutine(FugoUtils.FadeImage(1f, speed, currentSet.Find("Magnifier").GetComponent<Image>()));
			nextSet.Find("BGHolder/Frame").gameObject.SetActive(value: true);
			currentSet.Find("BGHolder/Frame").gameObject.SetActive(value: false);
			StartCoroutine(FugoUtils.Scaler(Vector3.one * 1.1f, speed, nextSet));
			nextSet.Find("BGHolder/Circle").GetComponent<Image>().sprite = currentSetSprite;
			StartCoroutine(FugoUtils.Blend(Color.white, speed, nextSet.Find("BGHolder/Circle").GetComponent<Image>()));
			StartCoroutine(FugoUtils.FadeImage(0f, speed, nextSet.Find("Key").GetComponent<Image>()));
			yield return new WaitForSeconds(speed);
			nextSet.Find("NameText").gameObject.SetActive(value: true);
			StartCoroutine(FugoUtils.Blend(nextSet.GetComponent<SetScript>().setColor, speed, nextSet.Find("NameText").GetComponent<Text>()));
			FugoUtils.ChangeAlpha(nextSet.Find("Footsteps").GetComponent<Image>(), 0f);
			nextSet.Find("Footsteps").gameObject.SetActive(value: true);
			StartCoroutine(FugoUtils.FadeImage(1f, speed, nextSet.Find("Footsteps").GetComponent<Image>()));
			yield return new WaitForSeconds(speed);
			StartCoroutine(AnimateCurrentSet(nextSet));
			setCompleted = false;
			curLevel++;
			bool touchActivated = true;
			if (nextSet.parent != currentSet.parent)
			{
				SectionCompletedAnim(currentSet.parent.parent, 3f);
				touchActivated = false;
			}
			Button currentSetButton = currentSet.GetComponent<Button>();
			currentSetButton.onClick.RemoveAllListeners();
			currentSetButton.onClick.AddListener(delegate
			{
				CompletedSetOnClickFunc(currentSet.GetComponent<SetScript>().set);
			});
			Button nextSetButton = nextSet.GetComponent<Button>();
			nextSetButton.onClick.RemoveAllListeners();
			nextSetButton.onClick.AddListener(delegate
			{
				MenuController.instance.PlayButtonClicked();
			});
			SetScript nextSetScript = nextSet.GetComponent<SetScript>();
			nextSetScript.dot.GetComponent<Button>().onClick.RemoveAllListeners();
			nextSetScript.dot.GetComponent<Button>().onClick.AddListener(delegate
			{
				MenuController.instance.PlayButtonClicked();
			});
			FugoUtils.ChangeAlpha(nextSetScript.dot.GetChild(1).GetComponent<Image>(), 0f);
			FugoUtils.ChangeAlpha(nextSetScript.dot.GetChild(0).GetComponent<Image>(), 0f);
			nextSet.GetComponent<SetScript>().dot.parent.gameObject.SetActive(value: true);
			StartCoroutine(FugoUtils.FadeImage(1f, 0.3f, nextSet.GetComponent<SetScript>().dot.GetChild(1).GetComponent<Image>()));
			StartCoroutine(FugoUtils.FadeImage(1f, 0.3f, nextSet.GetComponent<SetScript>().dot.GetChild(0).GetComponent<Image>()));
			nextSet.GetComponent<SetScript>().dot.gameObject.SetActive(value: true);
			yield return new WaitForSeconds(0.3f);
			StartCoroutine(CurrentPlace(nextSet.GetComponent<SetScript>().dot));
			StartCoroutine(CurrentPlaceShadow(nextSet.GetComponent<SetScript>().dot));
			list.parent.GetComponent<RectMask2D>().enabled = false;
			list.parent.GetComponent<RectMask2D>().enabled = true;
			if (touchActivated)
			{
				list.parent.parent.GetComponent<ScrollRect>().vertical = true;
			}
			if (PlayerPrefsManager.GetLevel() < 10)
			{
				MenuController.instance.OpenSetTutorialPopup();
			}
			yield return new WaitForSeconds(0.3f);
			FugoUtils.ChangeAlpha(nextSet.GetComponent<SetScript>().line.GetComponent<Image>(), 0f);
			nextSet.GetComponent<SetScript>().line.parent.gameObject.SetActive(value: true);
			StartCoroutine(FugoUtils.FadeImage(1f, 0.7f, nextSet.GetComponent<SetScript>().line.GetComponent<Image>()));
			nextSet.GetComponent<SetScript>().line.gameObject.SetActive(value: true);
			particle.SetActive(value: false);
		}
		if (hiddenSetCompleted != -1)
		{
			list.parent.parent.GetComponent<ScrollRect>().vertical = false;
			int completedHiddenSet = hiddenSetCompleted;
			hiddenSetCompleted = -1;
			Transform completedSet = null;
			IEnumerator enumerator5 = list.GetEnumerator();
			try
			{
				while (enumerator5.MoveNext())
				{
					Transform transform7 = (Transform)enumerator5.Current;
					IEnumerator enumerator6 = transform7.Find("HiddenSetHolder").GetEnumerator();
					try
					{
						while (enumerator6.MoveNext())
						{
							Transform transform8 = (Transform)enumerator6.Current;
							if (transform8.GetComponent<SetScript>().set.SetID == completedHiddenSet.ToString())
							{
								completedSet = transform8;
								break;
							}
						}
					}
					finally
					{
						IDisposable disposable6;
						if ((disposable6 = (enumerator6 as IDisposable)) != null)
						{
							disposable6.Dispose();
						}
					}
					if (completedSet != null)
					{
						break;
					}
				}
			}
			finally
			{
				IDisposable disposable4;
				IDisposable disposable7 = disposable4 = (enumerator5 as IDisposable);
				if (disposable4 != null)
				{
					disposable7.Dispose();
				}
			}
			particle.transform.position = completedSet.transform.position;
			particle.GetComponent<ParticleSystem>().startColor = FugoUtils.HexToColor(completedSet.GetComponent<SetScript>().set.SetColor);
			particle.GetComponent<ParticleSystemRenderer>().sortingOrder = 2;
			particle.SetActive(value: true);
			yield return new WaitForSeconds(0.6f);
			list.parent.GetComponent<RectMask2D>().enabled = false;
			yield return null;
			list.parent.GetComponent<RectMask2D>().enabled = true;
			list.parent.parent.GetComponent<ScrollRect>().vertical = true;
			yield return new WaitForSeconds(5f);
			particle.SetActive(value: false);
		}
	}

	private IEnumerator ClosingAnimThread()
	{
		IEnumerator enumerator = list.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform transform = (Transform)enumerator.Current;
				StartCoroutine(FugoUtils.FadeText(0f, 0.3f, transform.Find("TitleBG/Title").GetComponent<Text>()));
				IEnumerator enumerator2 = transform.Find("SetHolder").GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						Transform transform2 = (Transform)enumerator2.Current;
						if (transform2.name.Contains("Set"))
						{
							transform2.Find("BGHolder").localScale = Vector3.zero;
							transform2.Find("NameText").localScale = Vector3.zero;
							transform2.Find("Key").localScale = Vector3.zero;
							StartCoroutine(FugoUtils.Scaler(Vector3.zero, 0.3f, transform2.Find("BGHolder")));
							StartCoroutine(FugoUtils.Scaler(Vector3.zero, 0.3f, transform2.Find("NameText")));
							StartCoroutine(FugoUtils.Scaler(Vector3.zero, 0.3f, transform2.Find("Key")));
						}
					}
				}
				finally
				{
					IDisposable disposable;
					if ((disposable = (enumerator2 as IDisposable)) != null)
					{
						disposable.Dispose();
					}
				}
			}
		}
		finally
		{
			IDisposable disposable2;
			IDisposable disposable3 = disposable2 = (enumerator as IDisposable);
			if (disposable2 != null)
			{
				disposable3.Dispose();
			}
		}
		yield return null;
	}

	private IEnumerator DelayAnimThread(Transform t, float delay, string op)
	{
		yield return new WaitForSeconds(delay);
		if (op == "scale")
		{
			if (t == currentSet)
			{
				StartCoroutine(FugoUtils.Scaler(Vector3.one * 1.1f, 0.3f, t));
			}
			else
			{
				StartCoroutine(FugoUtils.Scaler(Vector3.one, 0.3f, t));
			}
		}
		else if (op == "fadetext")
		{
			StartCoroutine(FugoUtils.FadeText(1f, 0.3f, t.GetComponent<Text>()));
		}
		else if (op == "fadeimg")
		{
			StartCoroutine(FugoUtils.FadeImage(1f, 0.3f, t.GetComponent<Image>()));
		}
	}

	private void SectionCompletedAnim(Transform t, float time)
	{
		FugoUtils.ChangeAlpha(t.Find("CompleteBG").GetComponent<Image>(), 0f);
		FugoUtils.ChangeAlpha(t.Find("CompleteBG/Title").GetComponent<Text>(), 0f);
		FugoUtils.ChangeAlpha(t.Find("CompleteBG/Image").GetComponent<Image>(), 0f);
		t.Find("LevelCounter").gameObject.SetActive(value: false);
		t.Find("CompleteBG").gameObject.SetActive(value: true);
		StartCoroutine(FugoUtils.FadeImage(0.7f, time, t.Find("CompleteBG").GetComponent<Image>()));
		StartCoroutine(FugoUtils.FadeText(1f, time, t.Find("CompleteBG/Title").GetComponent<Text>()));
		StartCoroutine(FugoUtils.FadeImage(1f, time, t.Find("CompleteBG/Image").GetComponent<Image>()));
		if (time > 1f)
		{
			StartCoroutine(AnimateHiddenLevel(t, "sectioncompleted"));
		}
	}

	private IEnumerator CurrentPlace(Transform t)
	{
		Transform dot = t.Find("Dot");
		float time = 0.2f;
		Vector3 initPos = dot.localPosition;
		Vector3 movePos = initPos;
		movePos.y = initPos.y + 10f;
		StartCoroutine(FugoUtils.Mover(movePos, time, dot));
		yield return new WaitForSeconds(time);
		movePos.y = initPos.y;
		StartCoroutine(FugoUtils.Mover(movePos, time, dot));
		yield return new WaitForSeconds(time);
		movePos.y = initPos.y + 5f;
		StartCoroutine(FugoUtils.Mover(movePos, time * 0.6f, dot));
		yield return new WaitForSeconds(time * 0.6f);
		movePos.y = initPos.y;
		StartCoroutine(FugoUtils.Mover(movePos, time * 0.6f, dot));
		yield return new WaitForSeconds(time * 0.6f);
		movePos.y = initPos.y + 3f;
		StartCoroutine(FugoUtils.Mover(movePos, time * 0.3f, dot));
		yield return new WaitForSeconds(time * 0.3f);
		movePos.y = initPos.y;
		StartCoroutine(FugoUtils.Mover(movePos, time * 0.3f, dot));
		yield return new WaitForSeconds(time * 0.3f);
		dot.localPosition = initPos;
		yield return new WaitForSeconds(time * 5f);
		StartCoroutine(CurrentPlace(t));
	}

	private IEnumerator CurrentPlaceShadow(Transform t)
	{
		Transform dot = t.Find("Shadow");
		float time = 0.2f;
		Vector3 initPos = dot.localPosition;
		Vector3 movePos = initPos;
		movePos.y = initPos.y + 10f;
		movePos.x = initPos.x + 10f;
		StartCoroutine(FugoUtils.Mover(movePos, time, dot));
		yield return new WaitForSeconds(time);
		movePos.y = initPos.y;
		movePos.x = initPos.x;
		StartCoroutine(FugoUtils.Mover(movePos, time, dot));
		yield return new WaitForSeconds(time);
		movePos.y = initPos.y + 5f;
		movePos.x = initPos.x + 5f;
		StartCoroutine(FugoUtils.Mover(movePos, time * 0.6f, dot));
		yield return new WaitForSeconds(time * 0.6f);
		movePos.y = initPos.y;
		movePos.x = initPos.x;
		StartCoroutine(FugoUtils.Mover(movePos, time * 0.6f, dot));
		yield return new WaitForSeconds(time * 0.6f);
		movePos.y = initPos.y + 3f;
		movePos.x = initPos.x + 3f;
		StartCoroutine(FugoUtils.Mover(movePos, time * 0.3f, dot));
		yield return new WaitForSeconds(time * 0.3f);
		movePos.y = initPos.y;
		movePos.x = initPos.x;
		StartCoroutine(FugoUtils.Mover(movePos, time * 0.3f, dot));
		yield return new WaitForSeconds(time * 0.3f);
		dot.localPosition = initPos;
		yield return new WaitForSeconds(time * 5f);
		StartCoroutine(CurrentPlaceShadow(t));
	}

	public Sprite GetSectionBGImage(string name)
	{
		Sprite[] array = sectionBGs;
		foreach (Sprite sprite in array)
		{
			if (sprite != null && name == sprite.name)
			{
				return sprite;
			}
		}
		return null;
	}

	private IEnumerator AnimateHiddenLevel(Transform section, string type)
	{
		yield return new WaitForSeconds(1f);
		if (type == "sectioncompleted")
		{
			IEnumerator enumerator = section.Find("HiddenSetHolder").GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Transform t = (Transform)enumerator.Current;
					StartCoroutine(HiddenLevelUnlockAnimation(t));
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
		yield return new WaitForSeconds(1f);
	}

	private IEnumerator HiddenLevelUnlockAnimation(Transform t)
	{
		list.parent.parent.GetComponent<ScrollRect>().vertical = false;
		Transform animBG2 = t.Find("BGHolder");
		animBG2 = UnityEngine.Object.Instantiate(animBG2, t);
		animBG2.SetAsLastSibling();
		Image circle = animBG2.Find("Circle").GetComponent<Image>();
		FugoUtils.ChangeAlpha(circle, 0f);
		particle.transform.position = t.transform.position;
		particle.GetComponent<ParticleSystem>().startColor = t.Find("BGHolder/Circle").GetComponent<Image>().color;
		particle.GetComponent<ParticleSystemRenderer>().sortingOrder = 1;
		Vector3 curPos = t.localPosition;
		curPos.z = -100f;
		t.localPosition = curPos;
		particle.SetActive(value: true);
		float animDelay = 0.3f;
		UnityEngine.Object.Destroy(animBG2.gameObject, 3f * animDelay);
		StartCoroutine(FugoUtils.FadeImage(1f, animDelay, circle));
		yield return new WaitForSeconds(animDelay);
		t.Find("Locked").gameObject.SetActive(value: false);
		t.Find("NotPaid").gameObject.SetActive(value: true);
		StartCoroutine(FugoUtils.FadeImage(0f, animDelay, circle));
		t.GetComponent<Button>().onClick.RemoveAllListeners();
		t.GetComponent<Button>().onClick.AddListener(delegate
		{
			HiddenLevelOnClick(t);
		});
		yield return new WaitForSeconds(1f);
		list.parent.GetComponent<RectMask2D>().enabled = false;
		yield return null;
		list.parent.GetComponent<RectMask2D>().enabled = true;
		list.parent.parent.GetComponent<ScrollRect>().vertical = true;
	}

	public void HiddenLevelOnClick(Transform t)
	{
		if (HiddenSetController.isAnimating)
		{
			return;
		}
		selectedHiddenLevel = t;
		SoundManager.instance.Click();
		if (PlayerPrefsManager.IsAdventurePaid(t.GetComponent<SetScript>().set.SetID))
		{
			if (FugoUtils.IsAdventureCompleted(t.GetComponent<SetScript>().set.SetID))
			{
				CompletedSetOnClickFunc(t.GetComponent<SetScript>().set, isHidden: true);
			}
			else
			{
				HiddenSetController.instance.CreateLevels(t.GetComponent<SetScript>().set);
			}
		}
		else
		{
			MenuController.instance.OpenHiddenLevelPopup(t);
		}
	}

	public void HiddenLevelPurchaseButtonOnClick()
	{
		SoundManager.instance.Click();
		if (selectedHiddenLevel != null)
		{
			int price = selectedHiddenLevel.GetComponent<SetScript>().set.Price;
			if (PlayerPrefsManager.GetCoin() >= price)
			{
				PlayerPrefsManager.SetCoin(PlayerPrefsManager.GetCoin() - price);
				PlayerPrefsManager.SetAdventurePaid(selectedHiddenLevel.GetComponent<SetScript>().set.SetID);
				PlayerPrefsManager.SetHiddenLevel(int.Parse(selectedHiddenLevel.GetComponent<SetScript>().set.SetID), 1);
				SetHiddenSetStatus(selectedHiddenLevel, onPurchase: true);
				MenuController.instance.SetTexts();
				SoundManager.instance.PurchaseComplete();
				MenuController.instance.CloseHiddenLevelPopup(0.3f);
				FirebaseController.LogEvent("hidden_unlock_" + selectedHiddenLevel.GetComponent<SetScript>().set.SetID, string.Empty, string.Empty);
				RequestManager.instance.LogRequest();
			}
			else
			{
				StartCoroutine(NotEnoughCoinThread());
			}
		}
	}

	private IEnumerator NotEnoughCoinThread()
	{
		MenuController.instance.CloseHiddenLevelPopup(0.3f);
		yield return new WaitForSeconds(0.3f);
		MenuController.instance.ShopButtonClicked();
	}

	public void SetHiddenSetStatus(Transform go, bool onPurchase = false)
	{
		string setID = go.GetComponent<SetScript>().set.SetID;
		if (go.parent.parent == currentSection && PlayerPrefsManager.GetLevel() <= PlayerPrefsManager.CountLevels() && !onPurchase)
		{
			go.GetComponent<SetScript>().dot.gameObject.SetActive(value: false);
			go.GetComponent<SetScript>().line.gameObject.SetActive(value: false);
			go.Find("Paid").gameObject.SetActive(value: false);
			go.Find("NotPaid").gameObject.SetActive(value: false);
			go.Find("Locked").gameObject.SetActive(value: true);
			go.Find("BGHolder/Frame").gameObject.SetActive(value: false);
			go.GetComponent<Button>().onClick.RemoveAllListeners();
		}
		else if (PlayerPrefsManager.IsAdventurePaid(setID))
		{
			go.GetComponent<SetScript>().dot.gameObject.SetActive(value: true);
			go.GetComponent<SetScript>().line.gameObject.SetActive(value: true);
			go.Find("BGHolder/Frame").gameObject.SetActive(value: true);
			go.Find("Paid").gameObject.SetActive(value: true);
			go.Find("NotPaid").gameObject.SetActive(value: false);
			go.Find("Locked").gameObject.SetActive(value: false);
			go.Find("BGHolder/Frame").GetComponent<Image>().color = FugoUtils.HexToColor("6bbd40ff");
			go.Find("BGHolder/Frame").gameObject.SetActive(value: true);
			go.Find("Paid/Text").GetComponent<Text>().text = go.GetComponent<SetScript>().set.SetName;
			go.Find("Paid/Image").GetComponent<Image>().color = FugoUtils.HexToColor(go.GetComponent<SetScript>().set.section.KeyColor);
		}
		else
		{
			go.GetComponent<SetScript>().dot.gameObject.SetActive(value: false);
			go.GetComponent<SetScript>().line.gameObject.SetActive(value: false);
			go.Find("Paid").gameObject.SetActive(value: false);
			go.Find("NotPaid").gameObject.SetActive(value: true);
			go.Find("Locked").gameObject.SetActive(value: false);
			go.Find("BGHolder/Frame").gameObject.SetActive(value: true);
			go.Find("NotPaid/Text").GetComponent<Text>().text = LanguageScript.HiddenLevelHeaderText;
		}
	}

	public void SetHiddenSetProperties(Transform go)
	{
		LevelSet set = go.GetComponent<SetScript>().set;
		go.Find("BGHolder/Circle").GetComponent<Image>().color = FugoUtils.HexToColor(set.SetColor);
		go.Find("BGHolder/Frame").GetComponent<Image>().color = FugoUtils.ChangeBrightness(FugoUtils.HexToColor(set.SetColor), 1.2f);
		go.Find("Locked/Image").GetComponent<Image>().color = FugoUtils.ChangeBrightness(FugoUtils.HexToColor(set.SetColor), 1.2f);
		SetHiddenSetStatus(go);
	}
}
