using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DailyPuzzleController : MonoBehaviour
{
	public Transform panel;

	public Transform rewardHolder;

	public Transform startext;

	public Transform gallery;

	public Transform galleryPanel;

	public Transform shopButton;

	public Transform starHolder;

	public Transform starDestination;

	public Transform popup;

	public Transform giftPopup;

	public Transform collectButton;

	public Transform rewardAmountText;

	public Transform coin;

	public Transform gift;

	public Transform galleryList;

	public Transform imageHolder;

	public Transform imagePanel;

	public Transform galleryButton;

	public Transform shadow;

	public Transform contButton;

	public Text month;

	public Text country;

	public Text keyText;

	public Text dayText;

	public Text gainedStarText;

	public Text congratzText;

	public Image bwImg;

	public Image normalImg;

	public Image bar;

	public GameObject rewardPrefab;

	public GameObject starPrefab;

	public GameObject nextButton;

	public GameObject popupButton;

	public GameObject particles;

	public Sprite[] boxesClosed;

	public Sprite[] boxesOpened;

	public Material grayscale;

	private string[] months;

	private int[] prizePercentages = new int[4]
	{
		25,
		50,
		75,
		90
	};

	private int[] prizeCoinAmounts = new int[4]
	{
		60,
		120,
		180,
		240
	};

	public int totalKeys = 50;

	private int[] boxes;

	private int nextGift;

	private void Start()
	{
		if (PlayerPrefsManager.GetDailyPuzzleDay() != DateTime.Now.Day)
		{
			PlayerPrefsManager.SetDailyPuzzleNewLabel(DateTime.Now.Day);
			PlayerPrefsManager.SetDailyPuzzleDay(DateTime.Now.Day);
			PlayerPrefsManager.SetEarnedStar(-1);
			PlayerPrefsManager.ResetContinueDaily();
		}
		CloseGallery(0.001f);
		ClosePopup(0.001f);
		gallery.parent.localScale = Vector3.one * SectionController.ratio;
		imageHolder.localScale = Vector3.one * SectionController.ratio;
		imageHolder.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
		imagePanel.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
		imagePanel.gameObject.SetActive(value: false);
	}

	private void Update()
	{
		if (bar.fillAmount > 0.075f && PlayerPrefsManager.GetDailyProcess() < totalKeys)
		{
			keyText.gameObject.SetActive(value: true);
		}
		else if (bar.fillAmount <= 0.075f)
		{
			keyText.gameObject.SetActive(value: false);
		}
		float num = 700f * normalImg.fillAmount + 5f;
		if (num < 20f)
		{
			num -= 2f;
		}
		shadow.GetComponent<RectTransform>().sizeDelta = new Vector2(num, 424f);
	}

	private void Init()
	{
		months = LanguageScript.MonthNames.Split(',');
		gainedStarText.text = PlayerPrefsManager.GetEarnedStar().ToString();
		starHolder.transform.localScale = Vector3.one;
		if (PlayerPrefsManager.GetEarnedStar() == 0)
		{
			starHolder.gameObject.SetActive(value: false);
			nextButton.SetActive(value: false);
			popupButton.SetActive(value: true);
		}
		else
		{
			nextButton.SetActive(value: true);
			popupButton.SetActive(value: false);
		}
		panel.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
		int num = DateTime.Now.Month;
		num--;
		try
		{
			month.text = months[num];
		}
		catch
		{
		}
		if (PlayerPrefsManager.GetLang() == "Arabic" || PlayerPrefsManager.GetLang() == "Hebrew")
		{
			UnityEngine.Object.Destroy(country.GetComponent<ArabicText>());
		}
		country.text = Games.sections[num].Title;
		Image image = bwImg;
		Sprite sectionBGImage = SectionController.instance.GetSectionBGImage(Games.sections[num].BGName);
		normalImg.sprite = sectionBGImage;
		image.sprite = sectionBGImage;
		float curPerc = (float)PlayerPrefsManager.GetDailyProcess() / (float)totalKeys;
		CreateGifts(curPerc, num);
		if (PlayerPrefsManager.GetGalleryString().Length == 0)
		{
			galleryButton.gameObject.SetActive(value: false);
		}
		ArabicController.MakeArabicMenu(month.transform);
	}

	private void SetProcessBars()
	{
		float num = (float)PlayerPrefsManager.GetDailyProcess() / (float)totalKeys;
		if (num > 1f)
		{
			num = 1f;
		}
		if (PlayerPrefsManager.GetDailyProcess() == 0)
		{
			keyText.gameObject.SetActive(value: false);
			num = 0f;
		}
		else if (num < 0.075f)
		{
			keyText.gameObject.SetActive(value: false);
		}
		else
		{
			keyText.gameObject.SetActive(value: true);
			Transform transform = keyText.transform;
			Vector2 sizeDelta = bar.GetComponent<RectTransform>().sizeDelta;
			transform.localPosition = new Vector3(-372f + sizeDelta.x * num, 0f, 0f);
		}
		normalImg.fillAmount = num;
		bar.fillAmount = num;
		keyText.text = PlayerPrefsManager.GetDailyProcess().ToString();
		if (PlayerPrefsManager.GetDailyProcess() >= totalKeys)
		{
			keyText.gameObject.SetActive(value: false);
		}
	}

	private void CreateGifts(float curPerc, int curMonth)
	{
		IEnumerator enumerator = rewardHolder.GetEnumerator();
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
		Vector2 sizeDelta = bar.transform.GetComponent<RectTransform>().sizeDelta;
		float x = sizeDelta.x;
		boxes = new int[prizePercentages.Length];
		int num = 0;
		int[] array = prizePercentages;
		foreach (int num2 in array)
		{
			Transform transform2 = UnityEngine.Object.Instantiate(rewardPrefab, rewardHolder).transform;
			transform2.localScale = Vector3.one;
			transform2.localPosition = new Vector3(x * ((float)num2 / 100f), 0f, 0f);
			boxes[num] = num;
			if (curPerc >= (float)num2 / 100f)
			{
				transform2.Find("Circle/Box").GetComponent<Image>().sprite = boxesOpened[num];
				transform2.Find("Circle/Box").GetComponent<Image>().material = grayscale;
				nextGift++;
			}
			else
			{
				transform2.Find("Circle/Box").GetComponent<Image>().sprite = boxesClosed[num];
			}
			num++;
		}
	}

	public void OpeningAnim()
	{
		Init();
		StartCoroutine(OpeningThread());
	}

	private IEnumerator OpeningThread()
	{
		SetGallery();
		SetProcessBars();
		yield return new WaitForSeconds(0.3f);
		Image[] images = panel.GetComponentsInChildren<Image>();
		Text[] texts = panel.GetComponentsInChildren<Text>();
		if (PlayerPrefsManager.GetLang() != "Arabic" && PlayerPrefsManager.GetLang() != "Hebrew")
		{
			StartCoroutine(FugoUtils.Scaler(Vector3.one, 0.3f, shopButton));
		}
		else
		{
			StartCoroutine(FugoUtils.Scaler(new Vector3(-1f, 1f, 1f), 0.3f, shopButton));
		}
		StartCoroutine(FugoUtils.Scaler(Vector3.one, 0.3f, shopButton.Find("ShopButton")));
		Image[] array = images;
		foreach (Image image in array)
		{
			if (!image.name.ToLower().Contains("button") && !image.name.ToLower().Contains("panel") && !(image.name.ToLower() == "imageholder"))
			{
				FugoUtils.ChangeAlpha(image, 0f);
				if (image.name == "ImageBW")
				{
					StartCoroutine(FugoUtils.FadeImage(0.5f, 0.3f, image));
				}
				else if (image.name.ToLower() == "bgframe")
				{
					StartCoroutine(FugoUtils.FadeImage(0.1f, 0.3f, image));
				}
				else
				{
					StartCoroutine(FugoUtils.FadeImage(1f, 0.3f, image));
				}
			}
		}
		Text[] array2 = texts;
		foreach (Text text in array2)
		{
			FugoUtils.ChangeAlpha(text, 0f);
			StartCoroutine(FugoUtils.FadeText(1f, 0.3f, text));
		}
		panel.gameObject.SetActive(value: true);
		if (PlayerPrefsManager.GetEarnedStar() > 0)
		{
			StartCoroutine(AnimateStarThread());
		}
	}

	public void BackButtonClicked()
	{
		SoundManager.instance.Click();
		StartCoroutine(ClosingThread());
	}

	private IEnumerator ClosingThread()
	{
		Image[] images = panel.GetComponentsInChildren<Image>();
		Text[] texts = panel.GetComponentsInChildren<Text>();
		StartCoroutine(FugoUtils.Scaler(Vector3.zero, 0.3f, shopButton));
		Image[] array = images;
		foreach (Image image in array)
		{
			if (!image.name.ToLower().Contains("button") && !image.name.ToLower().Contains("panel"))
			{
				StartCoroutine(FugoUtils.FadeImage(0f, 0.3f, image));
			}
		}
		Text[] array2 = texts;
		foreach (Text go in array2)
		{
			StartCoroutine(FugoUtils.FadeText(0f, 0.3f, go));
		}
		yield return new WaitForSeconds(0.3f);
		panel.gameObject.SetActive(value: false);
		MenuController.instance.AnimateMenu();
	}

	public void OpenGallery()
	{
		if (PlayerPrefsManager.GetGalleryString().Length > 0)
		{
			SoundManager.instance.Click();
			StartCoroutine(OpenGalleryThread());
		}
	}

	private IEnumerator OpenGalleryThread()
	{
		gallery.localScale = Vector3.zero;
		FugoUtils.ChangeAlpha(gallery.parent.parent.GetComponent<Image>(), 0f);
		galleryPanel.gameObject.SetActive(value: true);
		gallery.gameObject.SetActive(value: true);
		StartCoroutine(FugoUtils.Scaler(Vector3.one, 0.3f, gallery));
		StartCoroutine(FugoUtils.FadeImage(0.5f, 0.3f, galleryPanel.GetComponent<Image>()));
		yield return null;
	}

	public void CloseGallery(float time)
	{
		StartCoroutine(CloseGalleryThread(time));
		ClosePopup(time);
	}

	private IEnumerator CloseGalleryThread(float time)
	{
		if ((double)time > 0.01)
		{
			SoundManager.instance.Click();
		}
		StartCoroutine(FugoUtils.Scaler(Vector3.zero, time, gallery));
		StartCoroutine(FugoUtils.FadeImage(0f, time, galleryPanel.GetComponent<Image>()));
		yield return new WaitForSeconds(time);
		galleryPanel.gameObject.SetActive(value: false);
		gallery.gameObject.SetActive(value: false);
	}

	public void OpenPopup()
	{
		SoundManager.instance.Click();
		StartCoroutine(OpenPopupThread());
	}

	private IEnumerator OpenPopupThread()
	{
		popup.gameObject.SetActive(value: true);
		popup.transform.localScale = Vector3.one;
		popup.GetComponent<Animator>().enabled = true;
		popup.GetComponent<Animator>().Play("PopupOpen");
		FugoUtils.ChangeAlpha(galleryPanel.GetComponent<Image>(), 0f);
		StartCoroutine(FugoUtils.FadeImage(0.5f, 0.3f, galleryPanel.GetComponent<Image>()));
		galleryPanel.gameObject.SetActive(value: true);
		yield return null;
	}

	public void ClosePopup(float time)
	{
		StartCoroutine(ClosePopupThread(time));
	}

	private IEnumerator ClosePopupThread(float time)
	{
		popup.GetComponent<Animator>().enabled = false;
		if ((double)time > 0.01)
		{
			SoundManager.instance.Click();
		}
		StartCoroutine(FugoUtils.Scaler(Vector3.zero, time, popup));
		StartCoroutine(FugoUtils.FadeImage(0f, time, galleryPanel.GetComponent<Image>()));
		yield return new WaitForSeconds(time);
		galleryPanel.gameObject.SetActive(value: false);
		popup.gameObject.SetActive(value: false);
	}

	private IEnumerator AnimateStarThread()
	{
		contButton.GetComponent<Button>().interactable = false;
		SoundManager.instance.DailyReward();
		yield return new WaitForSeconds(0.3f);
		gainedStarText.text = PlayerPrefsManager.GetEarnedStar().ToString();
		int tStar = PlayerPrefsManager.GetDailyProcess();
		int remainingStar = PlayerPrefsManager.GetEarnedStar() - 1;
		for (int i = 0; i < PlayerPrefsManager.GetEarnedStar(); i++)
		{
			Transform star = UnityEngine.Object.Instantiate(starPrefab, starHolder).transform;
			star.localScale = Vector3.one;
			star.localPosition = Vector3.zero;
			if (tStar + i >= totalKeys)
			{
				StartCoroutine(FugoUtils.CurveMover(shopButton.position, 1.5f, star));
			}
			else
			{
				StartCoroutine(FugoUtils.CurveMover(starDestination.position, 1.5f, star));
			}
			UnityEngine.Object.Destroy(star.gameObject, 1.5f);
			StartCoroutine(DelaySetTexts(1.5f, remainingStar));
			remainingStar--;
			yield return new WaitForSeconds(0.2f);
		}
		yield return new WaitForSeconds(1.5f);
		StartCoroutine(FugoUtils.Scaler(Vector3.zero, 0.3f, starHolder));
		PlayerPrefsManager.SetEarnedStar(0);
		MenuController.fromDaily = false;
		float perc = (float)PlayerPrefsManager.GetDailyProcess() / (float)totalKeys;
		if (nextGift + 1 < prizePercentages.Length && perc >= (float)prizePercentages[nextGift + 1] / 100f)
		{
			rewardHolder.GetChild(nextGift + 1).Find("Circle/Box").GetComponent<Image>()
				.sprite = boxesOpened[nextGift + 1];
				prizeCoinAmounts[nextGift] += prizeCoinAmounts[nextGift + 1];
			}
			if (nextGift < prizePercentages.Length && perc >= (float)prizePercentages[nextGift] / 100f)
			{
				StartCoroutine(GiveGift());
				rewardHolder.GetChild(nextGift).Find("Circle/Box").GetComponent<Image>()
					.sprite = boxesOpened[nextGift];
				}
				if (PlayerPrefsManager.GetDailyProcess() == totalKeys)
				{
					PlayerPrefsManager.SetGalleryString(DateTime.Now.Month.ToString());
					SetGallery();
					StartCoroutine(MonthFinishedThread());
					keyText.gameObject.SetActive(value: false);
				}
				yield return new WaitForSeconds(1f);
				contButton.GetComponent<Button>().interactable = true;
			}

			private IEnumerator DelaySetTexts(float time, int remainingStar)
			{
				yield return new WaitForSeconds(time);
				if (PlayerPrefsManager.GetDailyProcess() < totalKeys)
				{
					PlayerPrefsManager.SetDailyProcess(PlayerPrefsManager.GetDailyProcess() + 1);
				}
				float perc = (float)PlayerPrefsManager.GetDailyProcess() / (float)totalKeys;
				StartCoroutine(FugoUtils.FillImage(perc, 0.2f, normalImg));
				StartCoroutine(FugoUtils.FillImage(perc, 0.2f, bar));
				Vector2 sizeDelta = bar.GetComponent<RectTransform>().sizeDelta;
				StartCoroutine(FugoUtils.Mover(new Vector3(-370f + sizeDelta.x * perc, 0f, 0f), 0.2f, keyText.transform));
				keyText.text = PlayerPrefsManager.GetDailyProcess().ToString();
				gainedStarText.text = remainingStar.ToString();
				if (PlayerPrefsManager.GetDailyProcess() >= totalKeys)
				{
					PlayerPrefsManager.SetCoin(PlayerPrefsManager.GetCoin() + 5);
				}
				SoundManager.instance.CoinIncrease();
				yield return new WaitForSeconds(0.2f);
				MenuController.instance.IncreaseCoinAnim(0f);
			}

			private IEnumerator GiveGift()
			{
				rewardAmountText.GetComponent<Text>().text = "+" + prizeCoinAmounts[nextGift].ToString();
				rewardAmountText.GetComponent<Text>().color = FugoUtils.HexToColor("FFFFFF00");
				rewardAmountText.Find("CoinImage").GetComponent<Image>().color = FugoUtils.HexToColor("FFFFFF00");
				congratzText.GetComponent<Text>().color = FugoUtils.HexToColor("FFFFFF00");
				collectButton.transform.localScale = Vector3.zero;
				gift = UnityEngine.Object.Instantiate(DailyGiftSystem.instance.giftPrefab, giftPopup).transform;
				gift.localScale = Vector3.one;
				gift.Find("Top").GetComponent<Image>().sprite = DailyGiftSystem.instance.tops[boxes[nextGift]];
				gift.Find("Bottom").GetComponent<Image>().sprite = DailyGiftSystem.instance.bottoms[boxes[nextGift]];
				giftPopup.gameObject.SetActive(value: true);
				giftPopup.GetComponent<Animator>().enabled = true;
				giftPopup.GetComponent<Animator>().Play("PopupOpen");
				yield return new WaitForSeconds(0.5f);
				particles.SetActive(value: true);
				yield return new WaitForSeconds(0.1f);
				Vector3 topPos = gift.Find("Top").transform.localPosition;
				topPos.y += 40f;
				StartCoroutine(FugoUtils.Mover(topPos, 0.2f, gift.Find("Top")));
				yield return new WaitForSeconds(0.2f);
				coin = DailyGiftSystem.instance.CreateCoin(giftPopup, prizeCoinAmounts[nextGift]);
				coin.SetAsFirstSibling();
				SoundManager.instance.DailyReward();
				Vector3 coinpos = coin.localPosition;
				coinpos.y += 100f;
				StartCoroutine(FugoUtils.FadeImage(1f, 0.3f, coin.Find("Shine").GetComponent<Image>()));
				StartCoroutine(FugoUtils.FadeImage(1f, 0.3f, coin.Find("Coin").GetComponent<Image>()));
				StartCoroutine(FugoUtils.Mover(coinpos, 0.3f, coin));
				StartCoroutine(Rotator(coin.Find("Shine")));
				yield return new WaitForSeconds(0.3f);
				StartCoroutine(FugoUtils.FadeImage(0f, 0.2f, gift.Find("Top").GetComponent<Image>()));
				StartCoroutine(FugoUtils.FadeImage(0f, 0.2f, gift.Find("Bottom").GetComponent<Image>()));
				StartCoroutine(FugoUtils.FadeText(1f, 0.3f, congratzText));
				StartCoroutine(FugoUtils.FadeText(1f, 0.3f, rewardAmountText.GetComponent<Text>()));
				StartCoroutine(FugoUtils.FadeImage(1f, 0.3f, rewardAmountText.Find("CoinImage").GetComponent<Image>()));
				StartCoroutine(FugoUtils.Scaler(Vector3.one, 0.3f, coin));
				StartCoroutine(FugoUtils.Mover(Vector3.zero, 0.3f, coin));
				yield return new WaitForSeconds(0.3f);
				collectButton.transform.localScale = Vector3.zero;
				StartCoroutine(FugoUtils.Scaler(Vector3.one, 0.3f, collectButton));
				yield return new WaitForSeconds(3f);
				particles.SetActive(value: false);
			}

			private IEnumerator Rotator(Transform t)
			{
				StartCoroutine(FugoUtils.Rotator(360f, 20f, t));
				yield return new WaitForSeconds(20f);
				StartCoroutine(Rotator(t));
			}

			public void CollectButtonOnClick()
			{
				PlayerPrefsManager.SetCoin(PlayerPrefsManager.GetCoin() + prizeCoinAmounts[nextGift]);
				FortuneWheel.instance.CreateAnimCoins(giftPopup);
				StartCoroutine(EndThread());
			}

			private IEnumerator EndThread()
			{
				yield return new WaitForSeconds(1f);
				MenuController.instance.IncreaseCoinAnim(0f);
				yield return new WaitForSeconds(0.4f);
				giftPopup.GetComponent<Animator>().enabled = false;
				StartCoroutine(FugoUtils.Scaler(Vector3.zero, 0.3f, giftPopup));
				yield return new WaitForSeconds(0.3f);
				UnityEngine.Object.Destroy(coin.gameObject);
				UnityEngine.Object.Destroy(gift.gameObject);
				StopAllCoroutines();
				rewardAmountText.gameObject.SetActive(value: false);
			}

			public void GalleryPicOnClick(Transform t)
			{
				UnityEngine.Debug.Log(t.name);
				SoundManager.instance.Click();
				imageHolder.GetComponent<Image>().sprite = t.GetComponent<Image>().sprite;
				StartCoroutine(FugoUtils.FadeImage(1f, 0.3f, imageHolder.GetComponent<Image>()));
				imagePanel.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
				imagePanel.Find("PreviewCountryText").GetComponent<Text>().color = new Color(1f, 1f, 1f, 0f);
				imagePanel.Find("PreviewCountryText").GetComponent<Text>().text = Games.sections[int.Parse(t.name)].Title;
				StartCoroutine(FugoUtils.FadeText(1f, 0.3f, imagePanel.Find("PreviewCountryText").GetComponent<Text>()));
				StartCoroutine(FugoUtils.FadeImage(1f, 0.3f, imagePanel.GetComponent<Image>()));
				imagePanel.gameObject.SetActive(value: true);
			}

			private void SetGallery()
			{
				dayText.text = DateTime.Now.Day.ToString();
				if (PlayerPrefsManager.GetGalleryString().Length > 0)
				{
					for (int i = 0; i < PlayerPrefsManager.GetGalleryString().Length; i++)
					{
						int index = int.Parse(PlayerPrefsManager.GetGalleryString()[i]) - 1;
						GameObject image = galleryList.GetChild(i / 3).GetChild(i % 3).GetChild(0)
							.gameObject;
							image.transform.parent.GetComponent<Button>().onClick.RemoveAllListeners();
							image.transform.parent.GetComponent<Button>().onClick.AddListener(delegate
							{
								GalleryPicOnClick(image.transform);
							});
							image.name = index.ToString();
							image.GetComponent<Image>().sprite = SectionController.instance.GetSectionBGImage(Games.sections[index].BGName);
							image.transform.Find("Text").GetComponent<Text>().text = Games.sections[index].Title;
							image.transform.parent.gameObject.SetActive(value: true);
							galleryList.GetChild(i / 3).gameObject.SetActive(value: true);
						}
					}
				}

				public void CloseImagePreview()
				{
					SoundManager.instance.Click();
					StartCoroutine(CloseImagePreviewThread());
				}

				private IEnumerator CloseImagePreviewThread()
				{
					StartCoroutine(FugoUtils.FadeImage(0f, 0.3f, imagePanel.GetComponent<Image>()));
					StartCoroutine(FugoUtils.FadeImage(0f, 0.3f, imageHolder.GetComponent<Image>()));
					StartCoroutine(FugoUtils.FadeText(0f, 0.3f, imagePanel.Find("PreviewCountryText").GetComponent<Text>()));
					yield return new WaitForSeconds(0.3f);
					imagePanel.gameObject.SetActive(value: false);
				}

				private IEnumerator MonthFinishedThread()
				{
					Transform image = UnityEngine.Object.Instantiate(normalImg.gameObject, panel).transform;
					image.localScale = Vector3.one;
					image.position = normalImg.transform.position;
					image.GetComponent<Image>().fillAmount = 1f;
					UnityEngine.Object.Destroy(image.gameObject, 2f);
					galleryButton.gameObject.SetActive(value: true);
					StartCoroutine(FugoUtils.Scaler(Vector3.zero, 0.5f, image));
					StartCoroutine(FugoUtils.CurveMoverLeft(galleryButton.position, 0.5f, image));
					yield return new WaitForSeconds(0.5f);
					SoundManager.instance.DailyPuzzleCollected();
					galleryButton.GetComponent<Animator>().enabled = false;
					StartCoroutine(FugoUtils.Scaler(Vector3.one * 1.2f, 0.2f, galleryButton));
					yield return new WaitForSeconds(0.2f);
					StartCoroutine(FugoUtils.Scaler(Vector3.one, 0.2f, galleryButton));
					yield return new WaitForSeconds(0.2f);
					galleryButton.GetComponent<Animator>().enabled = true;
				}
			}
