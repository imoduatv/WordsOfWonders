using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopScript : MonoBehaviour
{
	public static ShopScript instance;

	public GameObject shopItemPrefab;

	public Transform list;

	public Sprite[] shopImages;

	public Sprite[] badges;

	private string[] badgeTexts = new string[3]
	{
		"<size=27>Most</size>\nPopular",
		"<size=27>Best</size>\nValue",
		"New"
	};

	private string[] titles = new string[7]
	{
		"Remove Ads",
		"240",
		"720",
		"1340",
		"2940",
		"6240",
		"PRO"
	};

	private string[] descs = new string[7]
	{
		string.Empty,
		string.Empty,
		string.Empty,
		"11",
		"22",
		"30",
		string.Empty
	};

	private int[] order = new int[7]
	{
		0,
		6,
		1,
		2,
		3,
		4,
		5
	};

	public static string[] itemPrices = new string[8]
	{
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty
	};

	public GameObject menuPanel;

	private Purchaser purchaser;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		badgeTexts[0] = LanguageScript.MostPopularText;
		badgeTexts[1] = LanguageScript.BestValueText;
		badgeTexts[2] = LanguageScript.NewText;
		titles[0] = LanguageScript.RemoveAdsText;
		for (int i = 0; i < descs.Length; i++)
		{
			if (descs[i] != string.Empty)
			{
				descs[i] = LanguageScript.DiscountText.Replace("%@", descs[i]);
			}
		}
		descs[6] = LanguageScript.ProDescriptionText;
		purchaser = GameObject.Find("Purchaser").GetComponent<Purchaser>();
		CreateShopItems();
	}

	public void NoAdButton()
	{
		purchaser.BuyProduct(Purchaser.removeads);
	}

	public void FirstGoldButton()
	{
		purchaser.BuyProduct(Purchaser.coinpack1);
	}

	public void SecondGoldButton()
	{
		purchaser.BuyProduct(Purchaser.coinpack2);
	}

	public void ThirdGoldButton()
	{
		purchaser.BuyProduct(Purchaser.coinpack3);
	}

	public void FourthGoldButton()
	{
		purchaser.BuyProduct(Purchaser.coinpack4);
	}

	public void FifthGoldButton()
	{
		purchaser.BuyProduct(Purchaser.coinpack5);
	}

	public void SubscriptionButton()
	{
		UnityEngine.Debug.Log("pro button");
		if (SceneManager.GetActiveScene().name == "Menu")
		{
			MenuController.instance.OpenProPopup();
		}
		else
		{
			GameMenuController.instance.openProPopup();
		}
	}

	public void BuyProButton()
	{
		purchaser.BuyProduct(Purchaser.subscription);
	}

	public void BuyTrialProButton()
	{
		purchaser.BuyProduct(Purchaser.trialsubscription);
	}

	public void NoAdCallback()
	{
		PlayerPrefsManager.SetNoAd(1);
		HideAds();
		AfterPurchase();
	}

	public void FirstGoldPackCallback()
	{
		PlayerPrefsManager.SetCoin(PlayerPrefsManager.GetCoin() + 240);
		AfterPurchase();
	}

	public void SecondGoldPackCallback()
	{
		PlayerPrefsManager.SetCoin(PlayerPrefsManager.GetCoin() + 720);
		AfterPurchase();
	}

	public void ThirdGoldPackCallback()
	{
		PlayerPrefsManager.SetCoin(PlayerPrefsManager.GetCoin() + 1340);
		AfterPurchase();
	}

	public void FourthGoldPackCallback()
	{
		PlayerPrefsManager.SetCoin(PlayerPrefsManager.GetCoin() + 2940);
		AfterPurchase();
	}

	public void FifthGoldPackCallback()
	{
		PlayerPrefsManager.SetCoin(PlayerPrefsManager.GetCoin() + 6240);
		AfterPurchase();
	}

	public void SubscriptionCallback()
	{
		PlayerPrefsManager.SetPro(1);
		PlayerPrefsManager.ResetProHint();
		HideAds();
		AfterPurchase();
		if (SceneManager.GetActiveScene().name == "GameNew")
		{
			GameMenuController.instance.closeFade();
			GameMenuController.instance.updateHintPrice();
		}
	}

	public void PrivacyButtonClicked()
	{
		SoundManager.instance.Click();
		Application.OpenURL("http://fugo.com.tr/privacypolicy-wordsofwonders.html");
	}

	private void AfterPurchase()
	{
		SoundManager.instance.PurchaseComplete();
		if (SceneManager.GetActiveScene().name == "Menu")
		{
			MenuController.instance.SetTexts();
			MenuController.instance.IncreaseCoinAnim(0f);
			MenuItemArranger.instance.Arrange();
			MenuController.instance.SetTexts();
			MenuController.instance.CloseProPopup(0.3f, withSound: false);
		}
		if (SceneManager.GetActiveScene().name == "GameNew")
		{
			GameMenuController.instance.updateCoin(animating: true);
		}
	}

	public void HideAds()
	{
		FugoAdManager.instance.HideBanner();
		if (SceneManager.GetActiveScene().name == "GameNew")
		{
			SafeAreaScaler.instance.calculateSafeArea();
		}
	}

	public void ButtonCallback(string s)
	{
		SoundManager.instance.Click();
		UnityEngine.Debug.Log(s);
		if (PlayerPrefsManager.GetLang() == "Arabic" && s == "تﺎﻧﻼﻋﻹا ﺔﻟازإ")
		{
			NoAdButton();
		}
		if (PlayerPrefsManager.GetLang() == "Hebrew" && s == "תומוסרפ תרסה")
		{
			NoAdButton();
		}
		if (s == titles[0])
		{
			NoAdButton();
		}
		else if (s == titles[1])
		{
			FirstGoldButton();
		}
		else if (s == titles[2])
		{
			SecondGoldButton();
		}
		else if (s == titles[3])
		{
			ThirdGoldButton();
		}
		else if (s == titles[4])
		{
			FourthGoldButton();
		}
		else if (s == titles[5])
		{
			FifthGoldButton();
		}
		else if (s == titles[6])
		{
			SubscriptionButton();
		}
	}

	public void CreateShopItems()
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
		Vector3 zero = Vector3.zero;
		int[] array = order;
		foreach (int num in array)
		{
			Transform t = UnityEngine.Object.Instantiate(shopItemPrefab, list).transform;
			t.localPosition = zero;
			t.localScale = Vector3.one;
			t.Find("ShopImage").GetComponent<Image>().sprite = shopImages[num];
			t.Find("TitleText").GetComponent<Text>().text = titles[num];
			t.Find("DescText").GetComponent<Text>().text = descs[num];
			t.Find("BuyButton").GetComponent<Button>().onClick.AddListener(delegate
			{
				ButtonCallback(t.Find("TitleText").GetComponent<Text>().text);
			});
			t.Find("BuyButton/Text").GetComponent<Text>().text = itemPrices[num];
			if (num % 2 == 1)
			{
				t.GetComponent<Image>().color = FugoUtils.HexToColor("96969636");
			}
			if (num == 3)
			{
				t.Find("BadgeImage").GetComponent<Image>().sprite = badges[0];
				t.Find("BadgeImage/Text").GetComponent<Text>().text = badgeTexts[0];
				t.Find("BadgeImage").gameObject.SetActive(value: true);
				if (UISwapper.flipGame)
				{
					t.Find("BadgeImage").GetComponent<RectTransform>().anchoredPosition = Vector2.left * 70f + Vector2.up * 23.4f;
				}
			}
			if (num == 5)
			{
				t.Find("BadgeImage").GetComponent<Image>().sprite = badges[1];
				t.Find("BadgeImage/Text").GetComponent<Text>().text = badgeTexts[1];
				t.Find("BadgeImage").gameObject.SetActive(value: true);
				if (UISwapper.flipGame)
				{
					t.Find("BadgeImage").GetComponent<RectTransform>().anchoredPosition = Vector2.left * 70f + Vector2.up * 23.4f;
				}
			}
			if (num == 6)
			{
				t.Find("BadgeImage").GetComponent<Image>().sprite = badges[2];
				t.Find("BadgeImage/Text").GetComponent<Text>().text = badgeTexts[2];
				t.Find("BadgeImage").gameObject.SetActive(value: true);
				t.Find("BuyButton/Text").GetComponent<Text>().text = LanguageScript.BecomeProText;
				if (UISwapper.flipGame)
				{
					t.Find("BadgeImage").GetComponent<RectTransform>().anchoredPosition = Vector2.left * 70f + Vector2.up * 23.4f;
				}
			}
			zero.y -= 120f;
			if (PlayerPrefsManager.GetLang() == "Hebrew")
			{
				ArabicController.MakeArabicMenu(t);
			}
		}
		if (UISwapper.flipGame)
		{
			Text[] allComponents = list.GetAllComponents<Text>();
			Text[] array2 = allComponents;
			foreach (Text text in array2)
			{
				Vector3 localScale = text.transform.localScale;
				localScale.x *= -1f;
				text.transform.localScale = localScale;
				string text2 = text.text;
				ArabicText arabicText = text.gameObject.AddComponent<ArabicText>();
				arabicText.Text = text2;
			}
		}
		if (SceneManager.GetActiveScene().name == "Menu")
		{
			GameObject gameObject = GameObject.Find("Canvas");
			gameObject.transform.Find("PopupHolder/ProPopup/ProButton/PriceText").GetComponent<Text>().text = itemPrices[6] + " / " + LanguageScript.WeeklyText;
			gameObject.transform.Find("PopupHolder/ProPopup/DescriptionText").GetComponent<Text>().text = LanguageScript.ProContentText.Replace("%@", itemPrices[6]);
			gameObject.transform.Find("PopupHolder/TrialProPopup/ProButton/PriceText").GetComponent<Text>().text = LanguageScript.FreeTrialText.Replace("%@", itemPrices[7]);
			gameObject.transform.Find("PopupHolder/TrialProPopup/DescriptionText").GetComponent<Text>().text = LanguageScript.ProContentText.Replace("%@", itemPrices[7]);
			if (PlayerPrefsManager.GetPro())
			{
				gameObject.transform.Find("PopupHolder/ProPopup/ProButton/PriceText").GetComponent<Text>().text = LanguageScript.AlreadyPurchasedText;
				gameObject.transform.Find("PopupHolder/ProPopup/ProButton/PriceText").GetComponent<Text>().color = Color.white;
				gameObject.transform.Find("PopupHolder/ProPopup/ProButton").GetComponent<Button>().interactable = false;
				gameObject.transform.Find("PopupHolder/ProPopup/DescriptionText").GetComponent<Text>().text = LanguageScript.ProContentText.Replace("%@", itemPrices[6]);
				gameObject.transform.Find("PopupHolder/TrialProPopup/ProButton/PriceText").GetComponent<Text>().text = LanguageScript.AlreadyPurchasedText;
				gameObject.transform.Find("PopupHolder/TrialProPopup/ProButton/PriceText").GetComponent<Text>().color = Color.white;
				gameObject.transform.Find("PopupHolder/TrialProPopup/ProButton").GetComponent<Button>().interactable = false;
				gameObject.transform.Find("PopupHolder/TrialProPopup/DescriptionText").GetComponent<Text>().text = LanguageScript.ProContentText.Replace("%@", itemPrices[7]);
			}
		}
		else
		{
			GameObject.Find("Canvas").transform.Find("Enviroment/ShopHolder/ProPopup/ProButton/PriceText").GetComponent<Text>().SetText(itemPrices[6] + " / " + LanguageScript.WeeklyText);
			GameObject.Find("Canvas").transform.Find("Enviroment/ShopHolder/ProPopup/DescriptionText").GetComponent<Text>().SetText(LanguageScript.ProContentText.Replace("%@", itemPrices[6]));
			GameObject gameObject2 = GameObject.Find("Canvas").transform.Find("Enviroment/ShopHolder/ProPopup/DescriptionText").gameObject;
			gameObject2.GetComponent<Text>().SetText(LanguageScript.ProContentText.Replace("%@", itemPrices[6]));
			if (PlayerPrefsManager.GetPro())
			{
				GameObject.Find("Canvas").transform.Find("Enviroment/ShopHolder/ProPopup/ProButton/PriceText").GetComponent<Text>().SetText(LanguageScript.AlreadyPurchasedText);
				GameObject.Find("Canvas").transform.Find("Enviroment/ShopHolder/ProPopup/ProButton/PriceText").GetComponent<Text>().color = Color.white;
				GameObject.Find("Canvas").transform.Find("Enviroment/ShopHolder/ProPopup/ProButton").GetComponent<Button>().interactable = false;
				GameObject.Find("Canvas").transform.Find("Enviroment/ShopHolder/ProPopup/DescriptionText").GetComponent<Text>().SetText(LanguageScript.ProContentText.Replace("%@", itemPrices[6]));
				LanguageScript.ProContentText.Replace("%@", itemPrices[6]);
			}
			GameObject.Find("Canvas").transform.Find("Enviroment/ShopHolder/ProPopup/BG/HintText").GetComponent<Text>().SetText(LanguageScript.ProFreeHintText);
			GameObject.Find("Canvas").transform.Find("Enviroment/ShopHolder/ProPopup/BG/NoAdText").GetComponent<Text>().SetText(LanguageScript.ProNoAdText);
			GameObject.Find("Canvas").transform.Find("Enviroment/ShopHolder/ProPopup/BG/GiftText").GetComponent<Text>().SetText(LanguageScript.ProDoubleDailyText);
			GameObject.Find("Canvas").transform.Find("Enviroment/ShopHolder/ProPopup/Terms&Conditions").GetComponent<Text>().SetText(LanguageScript.PrivacyPolicyText);
		}
	}

	public void ShowRewardedButtonOnClick()
	{
		FugoAdManager.instance.ShowRewarded("25gem");
	}

	public void ShowWaiting()
	{
		if (SceneManager.GetActiveScene().name == "Menu")
		{
			menuPanel.SetActive(value: true);
		}
	}

	public void HideWaiting()
	{
		if (SceneManager.GetActiveScene().name == "Menu")
		{
			menuPanel.SetActive(value: false);
		}
		else
		{
			GameMenuController.instance.closeFade();
		}
	}
}
