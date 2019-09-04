using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FortuneWheel : MonoBehaviour
{
	public static FortuneWheel instance;

	public GameObject slicePrefab;

	public GameObject animCoinPrefab;

	public GameObject confetti;

	public GameObject popup;

	public Transform sliceHolder;

	public Transform animation;

	public Transform coinHolder;

	public Transform menuShop;

	public Transform spinButton;

	public Transform hintPopup;

	public Transform trialAnim;

	public AnimationCurve ac;

	public Text coinText;

	private int[] percentages = new int[12]
	{
		30,
		30,
		30,
		30,
		30,
		30,
		30,
		30,
		30,
		30,
		30,
		30
	};

	private int[] prizes = new int[12]
	{
		10,
		-1,
		10,
		25,
		100,
		40,
		10,
		50,
		10,
		30,
		25,
		250
	};

	private string[] colors = new string[12]
	{
		"3BCEDC",
		"8842ED",
		"F3671C",
		"F79942",
		"6184DB",
		"76DC22",
		"3BCEDC",
		"F3671C",
		"3BCEDC",
		"DFBA0E",
		"F79942",
		"513EEE"
	};

	private bool animating;

	private bool firstTimeOfDay;

	private bool rewardedReady;

	private bool closing;

	private string lastPlayed = string.Empty;

	private float timer = 1f;

	private Vector2 firstVector;

	private Vector2 secondVector;

	private void Awake()
	{
		firstVector = default(Vector3);
		secondVector = default(Vector3);
		if (!PlayerPrefsManager.GetPro())
		{
			prizes[4] = 0;
		}
	}

	private void Start()
	{
		instance = this;
		CheckDailyGift();
		CreateSlices();
	}

	private void Update()
	{
		if (!rewardedReady && !firstTimeOfDay && !animating)
		{
			timer -= Time.deltaTime;
			if (timer < 0f)
			{
				timer = 1f;
				if (FugoAdManager.instance.isRewardedReady())
				{
					rewardedReady = true;
					SetSpinButton();
				}
			}
		}
		if (!firstTimeOfDay || animating || !popup.activeSelf)
		{
			return;
		}
		if (Input.GetMouseButtonDown(0))
		{
			firstVector = UnityEngine.Input.mousePosition - Camera.main.WorldToScreenPoint(sliceHolder.position);
		}
		if (!Input.GetMouseButtonUp(0))
		{
			return;
		}
		secondVector = UnityEngine.Input.mousePosition - Camera.main.WorldToScreenPoint(sliceHolder.position);
		if (firstVector.magnitude < 360f * (float)Screen.width / 1080f)
		{
			if (Vector2.SignedAngle(firstVector, secondVector) < 0f)
			{
				StartCoroutine(HandSpin(toLeft: false));
			}
			else
			{
				StartCoroutine(HandSpin(toLeft: true));
			}
		}
	}

	public void CreateSlices()
	{
		IEnumerator enumerator = sliceHolder.GetEnumerator();
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
		float num = 0f;
		for (int i = 0; i < percentages.Length; i++)
		{
			Transform transform2 = UnityEngine.Object.Instantiate(slicePrefab, sliceHolder).transform;
			transform2.localScale = Vector3.one;
			transform2.name = i.ToString();
			transform2.GetComponent<Image>().fillAmount = (float)percentages[i] / 360f;
			transform2.localRotation = Quaternion.Euler(0f, 0f, 360f - num);
			transform2.GetComponent<Image>().color = FugoUtils.HexToColor(colors[i]);
			num += (float)percentages[i];
			transform2.Find("PrizeHolder").localRotation = Quaternion.Euler(0f, 0f, (float)(-percentages[i]) / 2f);
			transform2.Find("PrizeHolder").localPosition = new Vector3(Mathf.Sin((float)percentages[i] / 2f * ((float)Math.PI / 180f)), Mathf.Cos((float)percentages[i] / 2f * ((float)Math.PI / 180f)), 0f) * 105f;
			transform2.Find("PrizeHolder/AmountText").GetComponent<Text>().text = Mathf.Abs(prizes[i]).ToString();
			if (prizes[i] > 0)
			{
				transform2.Find("PrizeHolder/CoinHolder").gameObject.SetActive(value: true);
				transform2.Find("PrizeHolder/HintHolder").gameObject.SetActive(value: false);
				transform2.Find("PrizeHolder/GiftHolder").gameObject.SetActive(value: false);
			}
			else if (prizes[i] < 0)
			{
				transform2.Find("PrizeHolder/CoinHolder").gameObject.SetActive(value: false);
				transform2.Find("PrizeHolder/AmountText").gameObject.SetActive(value: false);
				transform2.Find("PrizeHolder/HintHolder").gameObject.SetActive(value: true);
				transform2.Find("PrizeHolder/GiftHolder").gameObject.SetActive(value: false);
				transform2.Find("PrizeHolder/HintHolder").localPosition = new Vector3(0f, 67f, 0f);
			}
			else
			{
				transform2.Find("PrizeHolder/GiftHolder").gameObject.SetActive(value: true);
				transform2.Find("PrizeHolder/CoinHolder").gameObject.SetActive(value: true);
				transform2.Find("PrizeHolder/AmountText").gameObject.SetActive(value: true);
				transform2.Find("PrizeHolder/AmountText").GetComponent<Text>().text = "100";
				transform2.Find("PrizeHolder/HintHolder").gameObject.SetActive(value: false);
				transform2.Find("PrizeHolder/AmountText").localPosition = new Vector3(0f, 16f, 0f);
				transform2.Find("PrizeHolder/AmountText").GetComponent<Text>().resizeTextForBestFit = false;
				transform2.Find("PrizeHolder/AmountText").GetComponent<Text>().fontSize = 30;
				transform2.Find("PrizeHolder/CoinHolder").localPosition = new Vector3(0f, -24f, 0f);
				transform2.Find("PrizeHolder/CoinHolder").localScale = Vector3.one * 0.8f;
				transform2.Find("Pattern").gameObject.SetActive(value: true);
				transform2.GetComponent<Image>().color = FugoUtils.HexToColor("3C00FF");
			}
			ArabicController.MakeArabicMenu(transform2);
		}
		AnimateBG();
		SetCoinText();
		SetSpinButton();
	}

	public void Spin()
	{
		StartCoroutine(SpinThread());
	}

	public void SetCoinText()
	{
		coinText.text = PlayerPrefsManager.GetCoin().ToString();
		ArabicController.MakeArabicMenu(coinText.transform);
	}

	private IEnumerator HandSpin(bool toLeft)
	{
		yield return null;
		StartCoroutine(SpinThread(toLeft));
	}

	private IEnumerator SpinThread(bool toLeft = false)
	{
		animating = true;
		float time = UnityEngine.Random.Range(6f, 9f);
		StartCoroutine(Rotator(time, toLeft));
		yield return new WaitForSeconds(time);
		yield return null;
		Vector3 eulerAngles = sliceHolder.localRotation.eulerAngles;
		int prize = WhichPrize(eulerAngles.z);
		int repeatcount = 3;
		float delay = 0.15f;
		if (prizes[prize] > 0)
		{
			PlayerPrefsManager.SetCoin(PlayerPrefsManager.GetCoin() + prizes[prize]);
			if (PlayerPrefsManager.GetPro())
			{
				PlayerPrefsManager.SetCoin(PlayerPrefsManager.GetCoin() + prizes[prize]);
			}
			StartCoroutine(Flicker(sliceHolder.GetChild(prize).Find("PrizeHolder/CoinHolder"), delay, repeatcount));
			StartCoroutine(Flicker(sliceHolder.GetChild(prize).Find("PrizeHolder/AmountText"), delay, repeatcount));
			yield return new WaitForSeconds((float)repeatcount * delay * 2f);
			Vector3 pos2 = sliceHolder.GetChild(prize).Find("PrizeHolder/CoinHolder").position;
			int coinAmount2 = GetCoinAmount(prizes[prize]);
			StartCoroutine(CreateAnimCoinsRealPos(sliceHolder.parent.parent, pos2, coinAmount2, coinHolder));
			yield return new WaitForSeconds(1.5f + 0.1f * (float)coinAmount2);
		}
		else if (prizes[prize] < 0)
		{
			PlayerPrefsManager.SetHintDiscountTime(DateTime.Now.AddHours(Mathf.Abs(prizes[prize])).ToString());
			StartCoroutine(Flicker(sliceHolder.GetChild(prize).Find("PrizeHolder/HintHolder"), delay, repeatcount));
			StartCoroutine(Flicker(sliceHolder.GetChild(prize).Find("PrizeHolder/AmountText"), delay, repeatcount));
			yield return new WaitForSeconds((float)repeatcount * delay);
			hintPopup.localScale = Vector3.zero;
			hintPopup.gameObject.SetActive(value: true);
			StartCoroutine(FugoUtils.Scaler(Vector3.one, 0.3f, hintPopup));
			yield return new WaitForSeconds(3f);
			StartCoroutine(FugoUtils.Scaler(Vector3.zero, 0.3f, hintPopup));
			yield return new WaitForSeconds(0.3f);
			hintPopup.gameObject.SetActive(value: false);
		}
		else
		{
			PlayerPrefsManager.SetCoin(PlayerPrefsManager.GetCoin() + 100);
			StartCoroutine(Flicker(sliceHolder.GetChild(prize).Find("PrizeHolder/CoinHolder"), delay, repeatcount));
			StartCoroutine(Flicker(sliceHolder.GetChild(prize).Find("PrizeHolder/AmountText"), delay, repeatcount));
			yield return new WaitForSeconds((float)repeatcount * delay * 2f);
			Vector3 pos = sliceHolder.GetChild(prize).Find("PrizeHolder/CoinHolder").position;
			int coinAmount = GetCoinAmount(100);
			StartCoroutine(CreateAnimCoinsRealPos(sliceHolder.parent.parent, pos, coinAmount, coinHolder));
			StartCoroutine(ConfettiThread());
			yield return new WaitForSeconds(1.5f + 0.1f * (float)coinAmount);
			SoundManager.instance.FortuneWheel();
			MenuController.instance.OpenTrialProPopup();
			yield return new WaitForSeconds(0.3f);
		}
		animating = false;
		firstTimeOfDay = false;
		rewardedReady = false;
		SetSpinButton();
	}

	private IEnumerator Rotator(float time, bool reverse = false)
	{
		float spinCountPerSecond = UnityEngine.Random.Range(1f, 2f);
		Vector3 eulerAngles = sliceHolder.localRotation.eulerAngles;
		float oldPos = 0f - eulerAngles.z;
		if (reverse)
		{
			Vector3 eulerAngles2 = sliceHolder.localRotation.eulerAngles;
			oldPos = eulerAngles2.z;
		}
		Mathf.Abs(oldPos);
		float rotateAngle = Mathf.Abs(oldPos);
		float rotation = time * spinCountPerSecond * 360f;
		float finalAngle2 = 360f - (rotation - Mathf.Floor(time * spinCountPerSecond) * 360f);
		float num = finalAngle2;
		Vector3 eulerAngles3 = sliceHolder.localRotation.eulerAngles;
		finalAngle2 = num + eulerAngles3.z;
		if (finalAngle2 > 360f)
		{
			finalAngle2 -= 360f;
		}
		int rand = UnityEngine.Random.Range(0, 100);
		if (prizes[WhichPrize(finalAngle2)] == 250 && rand > 60)
		{
			rotation = UnityEngine.Random.Range(6f, 9f) * UnityEngine.Random.Range(1f, 2f) * 360f;
		}
		if (prizes[WhichPrize(finalAngle2)] == 100 && rand > 70)
		{
			rotation = UnityEngine.Random.Range(6f, 9f) * UnityEngine.Random.Range(1f, 2f) * 360f;
		}
		if (prizes[WhichPrize(finalAngle2)] == 0 && rand > 30)
		{
			rotation = UnityEngine.Random.Range(6f, 9f) * UnityEngine.Random.Range(1f, 2f) * 360f;
		}
		for (float t = 0f; t <= 1f; t += Time.deltaTime / time)
		{
			float tt = ac.Evaluate(t);
			float newPos = 0f - Mathf.Lerp(oldPos, oldPos + rotation, tt);
			if (reverse)
			{
				newPos = Mathf.Lerp(oldPos, oldPos + rotation, tt);
			}
			sliceHolder.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 1f) * newPos);
			if (Mathf.Abs(Mathf.Abs(newPos) - rotateAngle) >= 60f)
			{
				rotateAngle = Mathf.Abs(newPos);
				SoundManager.instance.WheelRotate();
			}
			yield return null;
		}
	}

	private void AnimateBG()
	{
		StartCoroutine(BGAnimThread());
		StartCoroutine(BGAnimThread2());
	}

	private IEnumerator BGAnimThread()
	{
		while (true)
		{
			StartCoroutine(FugoUtils.Rotator(360f, 8f, animation));
			StartCoroutine(FugoUtils.Rotator(360f, 8f, trialAnim));
			yield return new WaitForSeconds(8f);
		}
	}

	private IEnumerator BGAnimThread2()
	{
		float delay = 1f;
		while (true)
		{
			StartCoroutine(FugoUtils.Scaler(0.9f * Vector3.one, delay, animation));
			yield return new WaitForSeconds(delay);
			StartCoroutine(FugoUtils.Scaler(1f * Vector3.one, delay, animation));
			yield return new WaitForSeconds(delay);
		}
	}

	private int WhichPrize(float angle)
	{
		float num = 0f;
		for (int i = 0; i < percentages.Length; i++)
		{
			num += (float)percentages[i];
			if (num > angle)
			{
				return i;
			}
		}
		return -1;
	}

	public void WatchButtonOnClick()
	{
		if (!animating)
		{
			SoundManager.instance.Click();
			if (firstTimeOfDay)
			{
				Spin();
			}
			else
			{
				FugoAdManager.instance.ShowRewarded("wheel");
			}
		}
	}

	public void CloseButtonOnClick()
	{
		if (!animating)
		{
			closing = true;
			MenuController.instance.CloseWheelPopup(0.3f);
		}
	}

	public void CreateAnimCoins(Transform parent, Transform destination = null)
	{
		if (destination == null)
		{
			destination = menuShop;
		}
		SoundManager.instance.CoinGained();
		for (int i = 0; i < 5; i++)
		{
			Transform transform = UnityEngine.Object.Instantiate(animCoinPrefab, parent).transform;
			transform.localPosition = new Vector3(UnityEngine.Random.Range(-50f, 50f), UnityEngine.Random.Range(-50f, 50f), 0f);
			transform.localScale = Vector3.one;
			StartCoroutine(AnimCoinThread(transform, destination));
			UnityEngine.Object.Destroy(transform.gameObject, 2f);
		}
		StartCoroutine(UpdateCoins());
	}

	private IEnumerator CreateAnimCoinsRealPos(Transform parent, Vector3 pos, int count, Transform destination = null)
	{
		if (destination == null)
		{
			destination = menuShop;
		}
		SoundManager.instance.CoinGained();
		StartCoroutine(UpdateCoins());
		for (int i = 0; i < count; i++)
		{
			Transform t = UnityEngine.Object.Instantiate(animCoinPrefab, parent).transform;
			t.position = pos;
			t.localScale = Vector3.one;
			StartCoroutine(AnimCoinThread(t, destination));
			UnityEngine.Object.Destroy(t.gameObject, 2f);
			yield return new WaitForSeconds(0.1f);
		}
	}

	private IEnumerator CoinSoundThread()
	{
		yield return new WaitForSeconds(1f);
		SoundManager.instance.CoinIncrease();
	}

	private IEnumerator AnimCoinThread(Transform t, Transform destination)
	{
		StartCoroutine(CoinSoundThread());
		StartCoroutine(FugoUtils.CurveMover(destination.position, 1f, t));
		yield return new WaitForSeconds(0.8f);
		StartCoroutine(FugoUtils.FadeImage(0f, 0.2f, t.GetComponent<Image>()));
		yield return new WaitForSeconds(1f);
		UnityEngine.Object.Destroy(t.gameObject);
	}

	private IEnumerator Flicker(Transform t, float delay, int count)
	{
		for (int i = 0; i < count; i++)
		{
			StartCoroutine(FugoUtils.Scaler(Vector3.one * 1.2f, delay, t));
			yield return new WaitForSeconds(delay);
			StartCoroutine(FugoUtils.Scaler(Vector3.one * 1f, delay, t));
			yield return new WaitForSeconds(delay);
		}
	}

	private IEnumerator UpdateCoins()
	{
		yield return new WaitForSeconds(1f);
		MenuController.instance.IncreaseCoinAnim(0f, withsound: false);
	}

	public void FadeAllComponents(float aValue, float aTime, Transform go)
	{
		Text[] allComponents = go.GetAllComponents<Text>();
		Image[] allComponents2 = go.GetAllComponents<Image>();
		Text[] array = allComponents;
		foreach (Text go2 in array)
		{
			StartCoroutine(FugoUtils.FadeText(aValue, aTime, go2));
		}
		Image[] array2 = allComponents2;
		foreach (Image go3 in array2)
		{
			StartCoroutine(FugoUtils.FadeImage(aValue, aTime, go3));
		}
	}

	public void CheckDailyGift()
	{
		int day = DateTime.Now.Day;
		if (PlayerPrefsManager.GetDaily() == 0)
		{
			PlayerPrefsManager.SetDaily(day);
		}
		else if (PlayerPrefsManager.GetDaily() != day)
		{
			PlayerPrefsManager.SetDaily(day);
			firstTimeOfDay = true;
			StartCoroutine(OpenDelayedThread());
		}
	}

	private IEnumerator OpenDelayedThread()
	{
		yield return new WaitForSeconds(0.3f);
		MenuController.instance.OpenWheelPopup();
	}

	private void SetSpinButton()
	{
		if (firstTimeOfDay)
		{
			spinButton.Find("Ad").gameObject.SetActive(value: false);
			spinButton.Find("Text").localPosition = Vector3.zero;
			Vector2 sizeDelta = spinButton.Find("Text").GetComponent<RectTransform>().sizeDelta;
			sizeDelta.x = 256f;
			spinButton.Find("Text").GetComponent<RectTransform>().sizeDelta = sizeDelta;
			spinButton.GetComponent<Button>().interactable = true;
			return;
		}
		spinButton.Find("Ad").gameObject.SetActive(value: true);
		spinButton.Find("Text").localPosition = new Vector3(-47f, 0f, 0f);
		Vector2 sizeDelta2 = spinButton.Find("Text").GetComponent<RectTransform>().sizeDelta;
		sizeDelta2.x = 170f;
		spinButton.Find("Text").GetComponent<RectTransform>().sizeDelta = sizeDelta2;
		if (FugoAdManager.instance.isRewardedReady())
		{
			FadeAllComponents(1f, 0.001f, spinButton);
			spinButton.GetComponent<Button>().interactable = true;
		}
		else
		{
			FadeAllComponents(0.5f, 0.001f, spinButton);
			spinButton.GetComponent<Button>().interactable = false;
		}
	}

	public bool IsAnimating()
	{
		return animating;
	}

	public bool IsFirstTime()
	{
		return firstTimeOfDay;
	}

	private int GetCoinAmount(int val)
	{
		int result = 1;
		if (val >= 200)
		{
			result = 12;
		}
		else if (val >= 100)
		{
			result = 8;
		}
		else if (val >= 40)
		{
			result = 4;
		}
		else if (val >= 25)
		{
			result = 2;
		}
		return result;
	}

	private IEnumerator ConfettiThread()
	{
		confetti.SetActive(value: true);
		confetti.GetComponent<ParticleSystem>().Play();
		yield return new WaitForSeconds(10f);
		confetti.SetActive(value: false);
	}
}
