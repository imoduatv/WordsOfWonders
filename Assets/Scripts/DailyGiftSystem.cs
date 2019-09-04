using Firebase.RemoteConfig;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DailyGiftSystem : MonoBehaviour
{
	public static DailyGiftSystem instance;

	public static bool rewardedDoubled;

	public Sprite[] tops;

	public Sprite[] bottoms;

	public Sprite[] rewards;

	public Transform[] places;

	public GameObject giftPrefab;

	public GameObject dailyCoinPrefab;

	public GameObject animCoinPrefab;

	public GameObject particles;

	public GameObject proBadge;

	public Transform rewardBG;

	public Transform collectButton;

	public Transform rewardAmountText;

	public Transform rewardHintText;

	public Transform shop;

	public Transform rewardedButton;

	public Text congratzText;

	private int[] indexes = new int[4]
	{
		0,
		1,
		2,
		3
	};

	private int[] gifts = new int[8]
	{
		25,
		30,
		40,
		50,
		75,
		100,
		0,
		0
	};

	private Transform coin;

	private int reward;

	private float timer;

	private bool giftClicked;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (rewardedDoubled)
		{
			rewardedDoubled = false;
			GiveGiftAndCloseDaily();
		}
		timer += Time.deltaTime;
		if (timer >= 1f)
		{
			timer = 0f;
			CheckRewardedButtonStatus();
		}
	}

	public void CreateDailyGifts()
	{
		giftClicked = false;
		rewardBG.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
		rewardBG.gameObject.SetActive(value: false);
		congratzText.GetComponent<Text>().color = FugoUtils.HexToColor("FFFFFF00");
		rewardAmountText.GetComponent<Text>().color = FugoUtils.HexToColor("FFFFFF00");
		rewardHintText.GetComponent<Text>().color = FugoUtils.HexToColor("FFFFFF00");
		rewardAmountText.Find("CoinImage").GetComponent<Image>().color = FugoUtils.HexToColor("FFFFFF00");
		proBadge.transform.Find("Text").GetComponent<Text>().color = FugoUtils.HexToColor("FFFFFF00");
		proBadge.transform.GetComponent<Image>().color = FugoUtils.HexToColor("FFFFFF00");
		proBadge.gameObject.SetActive(value: false);
		rewardedButton.gameObject.SetActive(value: false);
		Transform[] array = places;
		foreach (Transform transform in array)
		{
			if (transform.childCount > 0)
			{
				UnityEngine.Object.Destroy(transform.GetChild(0).gameObject);
			}
		}
		FugoUtils.ShuffleArray(indexes);
		FugoUtils.ShuffleArray(places);
		FugoUtils.ShuffleArray(gifts);
		reward = gifts[0];
		if (reward == 0)
		{
			DateTime dateTime = DateTime.Now.AddHours(1.0);
			if (PlayerPrefsManager.GetPro())
			{
				dateTime = dateTime.AddHours(1.0);
			}
			PlayerPrefsManager.SetHintDiscountTime(dateTime.ToString());
			rewardHintText.gameObject.SetActive(value: true);
		}
		else
		{
			rewardAmountText.GetComponent<Text>().text = "+" + reward;
			rewardAmountText.gameObject.SetActive(value: true);
		}
		int[] array2 = indexes;
		foreach (int num in array2)
		{
			int num2 = num;
			if (num == 3)
			{
				num2 = 2;
			}
			Transform go = UnityEngine.Object.Instantiate(giftPrefab, places[num2]).transform;
			go.localScale = Vector3.one;
			go.Find("Top").GetComponent<Image>().sprite = tops[num];
			go.Find("Bottom").GetComponent<Image>().sprite = bottoms[num];
			go.GetComponent<Button>().onClick.AddListener(delegate
			{
				GiftClicked(go);
			});
			StartCoroutine(BoxAnimThread(go));
		}
		ArrangeCollectButtons();
	}

	private void ArrangeCollectButtons()
	{
		string a = string.Empty;
		try
		{
			a = FirebaseRemoteConfig.GetValue("daily_double").StringValue;
		}
		catch
		{
		}
		if (a == "1" && reward != 0 && !PlayerPrefsManager.GetPro())
		{
			rewardedButton.gameObject.SetActive(value: true);
			Vector3 v = collectButton.GetComponent<RectTransform>().anchoredPosition;
			v.x = -140f;
			collectButton.GetComponent<RectTransform>().anchoredPosition = v;
			v.x = 140f;
			rewardedButton.GetComponent<RectTransform>().anchoredPosition = v;
		}
		else
		{
			rewardedButton.gameObject.SetActive(value: false);
			Vector3 v2 = collectButton.GetComponent<RectTransform>().anchoredPosition;
			v2.x = 0f;
			collectButton.GetComponent<RectTransform>().anchoredPosition = v2;
		}
		collectButton.GetComponent<Button>().interactable = true;
		rewardedButton.GetComponent<Button>().interactable = true;
	}

	private void CheckRewardedButtonStatus()
	{
		if (FugoAdManager.instance.isRewardedReady())
		{
			rewardedButton.GetComponent<Button>().interactable = true;
			rewardedButton.Find("Image").GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
			rewardedButton.Find("Video").GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
			rewardedButton.Find("Text").GetComponent<Text>().color = new Color(1f, 1f, 1f, 1f);
		}
		else
		{
			rewardedButton.GetComponent<Button>().interactable = false;
			rewardedButton.Find("Image").GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.5f);
			rewardedButton.Find("Video").GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.5f);
			rewardedButton.Find("Text").GetComponent<Text>().color = new Color(1f, 1f, 1f, 0.5f);
		}
	}

	public void GiftClicked(Transform t)
	{
		if (!giftClicked)
		{
			giftClicked = true;
			StopAllCoroutines();
			rewardBG.gameObject.SetActive(value: true);
			if (PlayerPrefsManager.GetPro())
			{
				proBadge.gameObject.SetActive(value: true);
				rewardAmountText.GetComponent<RectTransform>().anchoredPosition = new Vector2(-175f, -147f);
			}
			collectButton.gameObject.SetActive(value: true);
			collectButton.GetComponent<Animator>().enabled = false;
			collectButton.localScale = Vector3.zero;
			rewardedButton.GetComponent<Animator>().enabled = false;
			rewardedButton.localScale = Vector3.zero;
			t.SetParent(rewardBG);
			StartCoroutine(GiftThread(t));
		}
	}

	private IEnumerator GiftThread(Transform t)
	{
		StartCoroutine(FugoUtils.FadeImage(1f, 0.2f, rewardBG.GetComponent<Image>()));
		yield return new WaitForSeconds(0.2f);
		StartCoroutine(FugoUtils.Mover(Vector3.zero, 0.2f, t));
		yield return new WaitForSeconds(0.2f);
		StartCoroutine(FugoUtils.Scaler(Vector3.one * 1.2f, 0.2f, t));
		yield return new WaitForSeconds(0.1f);
		particles.SetActive(value: true);
		yield return new WaitForSeconds(0.1f);
		Vector3 topPos = t.Find("Top").transform.localPosition;
		topPos.y += 40f;
		StartCoroutine(FugoUtils.Mover(topPos, 0.2f, t.Find("Top")));
		yield return new WaitForSeconds(0.2f);
		coin = CreateCoin(rewardBG, reward);
		SoundManager.instance.DailyReward();
		Vector3 coinpos = coin.localPosition;
		coinpos.y += 100f;
		StartCoroutine(FugoUtils.FadeImage(1f, 0.3f, coin.Find("Shine").GetComponent<Image>()));
		StartCoroutine(FugoUtils.FadeImage(1f, 0.3f, coin.Find("Coin").GetComponent<Image>()));
		StartCoroutine(FugoUtils.Mover(coinpos, 0.3f, coin));
		StartCoroutine(Rotator(coin.Find("Shine")));
		yield return new WaitForSeconds(0.3f);
		StartCoroutine(FugoUtils.FadeImage(0f, 0.2f, t.Find("Top").GetComponent<Image>()));
		StartCoroutine(FugoUtils.FadeImage(0f, 0.2f, t.Find("Bottom").GetComponent<Image>()));
		StartCoroutine(FugoUtils.FadeText(1f, 0.3f, congratzText));
		StartCoroutine(FugoUtils.FadeText(1f, 0.3f, rewardAmountText.GetComponent<Text>()));
		StartCoroutine(FugoUtils.FadeText(1f, 0.3f, rewardHintText.GetComponent<Text>()));
		StartCoroutine(FugoUtils.FadeText(1f, 0.3f, proBadge.transform.Find("Text").GetComponent<Text>()));
		StartCoroutine(FugoUtils.FadeImage(1f, 0.3f, rewardAmountText.Find("CoinImage").GetComponent<Image>()));
		StartCoroutine(FugoUtils.FadeImage(1f, 0.3f, proBadge.GetComponent<Image>()));
		StartCoroutine(FugoUtils.Scaler(Vector3.one, 0.3f, coin));
		StartCoroutine(FugoUtils.Mover(Vector3.zero, 0.3f, coin));
		yield return new WaitForSeconds(0.3f);
		StartCoroutine(FugoUtils.Scaler(Vector3.one, 0.3f, collectButton));
		StartCoroutine(FugoUtils.Scaler(Vector3.one, 0.3f, rewardedButton));
		yield return new WaitForSeconds(0.3f);
		collectButton.GetComponent<Animator>().enabled = true;
		rewardedButton.GetComponent<Animator>().enabled = true;
		yield return new WaitForSeconds(3f);
		particles.SetActive(value: false);
	}

	public Transform CreateCoin(Transform parent, int rewardAmount)
	{
		Transform transform = UnityEngine.Object.Instantiate(dailyCoinPrefab, parent).transform;
		transform.localScale = Vector3.one * 0.5f;
		transform.localPosition = Vector3.zero;
		transform.Find("Shine").GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
		transform.Find("Coin").GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
		if (rewardAmount == 0)
		{
			transform.Find("Coin").GetComponent<Image>().sprite = rewards[1];
		}
		else
		{
			transform.Find("Coin").GetComponent<Image>().sprite = rewards[0];
		}
		return transform;
	}

	private IEnumerator Rotator(Transform t)
	{
		StartCoroutine(FugoUtils.Rotator(360f, 20f, t));
		yield return new WaitForSeconds(20f);
		StartCoroutine(Rotator(t));
	}

	public void CreateAnimCoins(Transform parent)
	{
		SoundManager.instance.CoinGained();
		for (int i = 0; i < 5; i++)
		{
			Transform transform = UnityEngine.Object.Instantiate(animCoinPrefab, parent.parent).transform;
			transform.localPosition = new Vector3(UnityEngine.Random.Range(-50f, 50f), UnityEngine.Random.Range(-50f, 50f), 0f);
			transform.localScale = Vector3.one;
			StartCoroutine(AnimCoinThread(transform));
			UnityEngine.Object.Destroy(transform.gameObject, 2f);
		}
	}

	private IEnumerator AnimCoinThread(Transform t)
	{
		yield return new WaitForSeconds(UnityEngine.Random.Range(0f, 0.1f));
		StartCoroutine(FugoUtils.CurveMover(shop.position, 1f, t));
		yield return new WaitForSeconds(0.8f);
		StartCoroutine(FugoUtils.FadeImage(0f, 0.2f, t.GetComponent<Image>()));
		yield return new WaitForSeconds(1f);
		UnityEngine.Object.Destroy(t.gameObject);
	}

	public void GiveGiftAndCloseDaily()
	{
		collectButton.GetComponent<Button>().interactable = false;
		rewardedButton.GetComponent<Button>().interactable = false;
		if (reward != 0)
		{
			CreateAnimCoins(rewardBG);
		}
		StartCoroutine(EndThread());
	}

	private IEnumerator EndThread()
	{
		yield return new WaitForEndOfFrame();
		if (reward != 0)
		{
			yield return new WaitForSeconds(1f);
		}
		if (reward != 0)
		{
			MenuController.instance.IncreaseCoinAnim(0f);
		}
		yield return new WaitForSeconds(0.4f);
		MenuController.instance.CloseDailyGiftPopup(0.3f);
		yield return new WaitForSeconds(0.3f);
		UnityEngine.Object.Destroy(coin.gameObject);
		StopAllCoroutines();
		rewardHintText.gameObject.SetActive(value: false);
		rewardAmountText.gameObject.SetActive(value: false);
	}

	public void CollectButtonOnClick()
	{
		if (PlayerPrefsManager.GetPro())
		{
			reward *= 2;
		}
		PlayerPrefsManager.SetCoin(PlayerPrefsManager.GetCoin() + reward);
		GiveGiftAndCloseDaily();
	}

	public void RewardedButtonOnClick()
	{
		if (reward != 0)
		{
			FugoAdManager.instance.ShowRewarded(string.Empty);
		}
	}

	private IEnumerator BoxAnimThread(Transform t)
	{
		float time = 1f;
		StartCoroutine(FugoUtils.Scaler(Vector3.one * 1.1f, time, t));
		yield return new WaitForSeconds(time);
		StartCoroutine(FugoUtils.Scaler(Vector3.one * 1f, time, t));
		yield return new WaitForSeconds(time);
		StartCoroutine(BoxAnimThread(t));
	}

	private IEnumerator OpenDelayedThread()
	{
		yield return new WaitForSeconds(1.5f);
		MenuController.instance.OpenDailyGiftPopup();
	}
}
