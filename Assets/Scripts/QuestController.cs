using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class QuestController : MonoBehaviour
{
	public static QuestController instance;

	public Transform questPopup;

	public Transform questButton;

	private int giftAmount;

	private int givenHours;

	private float timer;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		if (Games.sections == null || Games.sections.Count == 0)
		{
			return;
		}
		DateTime now = DateTime.Now;
		if (now.Day % 2 == 1)
		{
			now = now.AddDays(1.0);
			if (now.Day == 1)
			{
				now = now.AddDays(1.0);
			}
		}
		else
		{
			now = now.AddDays(2.0);
		}
		DateTime d = new DateTime(now.Year, now.Month, now.Day, 21, 0, 0);
		NotificationSystem.CreateQuestReadyNotification((int)(d - DateTime.Now).TotalSeconds);
	}

	private void Update()
	{
		if (PlayerPrefsManager.GetQuestEndTime() != string.Empty)
		{
			timer += Time.deltaTime;
			if (timer > 60f)
			{
				SetQuestButton();
			}
		}
		if (!Input.GetKeyDown(KeyCode.A))
		{
		}
	}

	public void GiveQuest()
	{
		PlayerPrefsManager.SetLastQuestTime(DateTime.Now.ToString("yy-MM-dd"));
		StopAllCoroutines();
		questButton.localScale = Vector3.one;
		int firstLevelOfNextSet = FugoUtils.GetFirstLevelOfNextSet();
		int num = firstLevelOfNextSet - PlayerPrefsManager.GetLevel();
		int num2 = FugoUtils.getLevelInfo()[0];
		int[] array = new int[5]
		{
			30,
			40,
			50,
			60,
			0
		};
		int[] array2 = new int[5]
		{
			40,
			60,
			0,
			0,
			0
		};
		PlayerPrefsManager.SetQuestEndLevel(firstLevelOfNextSet);
		if (num <= 6)
		{
			FugoUtils.ShuffleArray(array);
			giftAmount = array[0];
			PlayerPrefsManager.SetQuestRewardAmount(75);
			if (num2 > 13)
			{
				givenHours = 3;
			}
			else if (num2 > 7)
			{
				givenHours = 2;
			}
			else
			{
				givenHours = 1;
			}
		}
		else
		{
			FugoUtils.ShuffleArray(array2);
			giftAmount = array2[0];
			PlayerPrefsManager.SetQuestRewardAmount(100);
			if (num2 > 13)
			{
				givenHours = 3;
			}
			else if (num2 > 7)
			{
				givenHours = 2;
			}
			else
			{
				givenHours = 1;
			}
		}
		OpenQuestPopup("offer");
	}

	public void OpenQuestPopup(string type)
	{
		DisableAll();
		int index = FugoUtils.getLevelInfo(PlayerPrefsManager.GetQuestEndLevel() - 1)[0];
		int index2 = FugoUtils.getLevelInfo(PlayerPrefsManager.GetQuestEndLevel() - 1)[1];
		string setFullName = Games.sections[index].sets[index2].SetFullName;
		if (type.ToLower() == "offer")
		{
			string newValue = "<color=#FFC687FF>" + setFullName + "</color>";
			if (PlayerPrefsManager.GetLang() == "Arabic" || PlayerPrefsManager.GetLang() == "Hebrew")
			{
				newValue = setFullName;
			}
			if (givenHours > 1)
			{
				questPopup.Find("QuestSection/QuestText").GetComponent<Text>().text = LanguageScript.AdventureQuestText.Split('/')[1].Replace("%@", givenHours.ToString()).Replace("@%", newValue);
			}
			else
			{
				questPopup.Find("QuestSection/QuestText").GetComponent<Text>().text = LanguageScript.AdventureQuestText.Split('/')[0].Replace("%@", givenHours.ToString()).Replace("@%", newValue);
			}
			questPopup.Find("QuestSection").gameObject.SetActive(value: true);
			ArabicController.MakeArabicMenu(questPopup.Find("QuestSection/QuestText"));
		}
		else if (type.ToLower() == "gift")
		{
			questPopup.Find("GiftSection").gameObject.SetActive(value: true);
		}
		else if (type.ToLower() == "info")
		{
			string text = "<color=#FFC687FF>" + setFullName + "</color>";
			if (PlayerPrefsManager.GetLang() == "Arabic" || PlayerPrefsManager.GetLang() == "Hebrew")
			{
				text = setFullName;
			}
			questPopup.Find("InfoSection/QuestSection").GetComponent<Text>().text = text;
			questPopup.Find("InfoSection/RemainingLevel").GetComponent<Text>().text = LanguageScript.AdventureRemainingText.Replace("%@", (PlayerPrefsManager.GetQuestEndLevel() - PlayerPrefsManager.GetLevel()).ToString());
			TimeSpan ts = DateTime.Parse(PlayerPrefsManager.GetQuestEndTime()) - DateTime.Now;
			questPopup.Find("InfoSection/RemainingTime").GetComponent<Text>().text = LanguageScript.AdventureRemainingTimeText.Replace("%@", FugoUtils.DateFormatterLong(LanguageScript.DayHourText, ts));
			questPopup.Find("InfoSection").gameObject.SetActive(value: true);
			ArabicController.MakeArabicMenu(questPopup.Find("InfoSection/QuestSection"));
			ArabicController.MakeArabicMenu(questPopup.Find("InfoSection/RemainingLevel"));
			ArabicController.MakeArabicMenu(questPopup.Find("InfoSection/RemainingTime"));
		}
		else if (type.ToLower() == "reward")
		{
			questPopup.Find("RewardSection/GemRewardHolder/AmountText").GetComponent<Text>().text = PlayerPrefsManager.GetQuestRewardAmount().ToString();
			ArabicController.MakeArabicMenu(questPopup.Find("RewardSection/GemRewardHolder/AmountText"));
			questPopup.Find("RewardSection").gameObject.SetActive(value: true);
		}
		else if (type.ToLower() == "fail")
		{
			questPopup.Find("FailSection").gameObject.SetActive(value: true);
		}
		MenuController.instance.OpenQuestPopup();
	}

	private void DisableAll()
	{
		IEnumerator enumerator = questPopup.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform transform = (Transform)enumerator.Current;
				if (transform.name != "Icon")
				{
					transform.gameObject.SetActive(value: false);
				}
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

	public void AcceptOffer()
	{
		SoundManager.instance.Click();
		if (giftAmount == 0)
		{
			questPopup.Find("GiftSection/HintDiscountText").GetComponent<Text>().text = LanguageScript.AdventureHintDiscountText.Replace("%@", "50");
			questPopup.Find("GiftSection/HintDiscountText").gameObject.SetActive(value: true);
			ArabicController.MakeArabicMenu(questPopup.Find("GiftSection/HintDiscountText"));
			PlayerPrefsManager.SetHintDiscountTime(DateTime.Now.AddDays(givenHours).ToString());
		}
		else
		{
			questPopup.Find("GiftSection/GemRewardHolder/AmountText").GetComponent<Text>().text = giftAmount.ToString();
			ArabicController.MakeArabicMenu(questPopup.Find("GiftSection/GemRewardHolder/AmountText"));
			questPopup.Find("GiftSection/GemRewardHolder").gameObject.SetActive(value: true);
			PlayerPrefsManager.SetCoin(PlayerPrefsManager.GetCoin() + giftAmount);
		}
		StartCoroutine(AcceptThread());
		DateTime dateTime = DateTime.Now.AddDays(givenHours);
		PlayerPrefsManager.SetQuestEndTime(dateTime.ToString());
		NotificationSystem.CreateQuestLastTwoHoursNotification((int)(dateTime.AddHours(-2.0) - DateTime.Now).TotalSeconds);
		NotificationSystem.DisableQuestReadyNotifications();
		SetQuestButton();
	}

	private IEnumerator AcceptThread()
	{
		yield return new WaitForSeconds(0.3f);
		DisableAll();
		questPopup.Find("GiftSection").gameObject.SetActive(value: true);
	}

	public void DeclineOffer()
	{
		ResetQuest();
		SoundManager.instance.Click();
		MenuController.instance.CloseQuestPopup(0.3f);
		NotificationSystem.DisableQuestReadyNotifications();
	}

	public void ClaimRewardButtonOnClick()
	{
		SoundManager.instance.Click();
		PlayerPrefsManager.SetCoin(PlayerPrefsManager.GetCoin() + PlayerPrefsManager.GetQuestRewardAmount());
		PlayerPrefsManager.SetQuestRewardAmount(0);
		StartCoroutine(ClaimRewardThread());
	}

	private IEnumerator ClaimRewardThread()
	{
		FortuneWheel.instance.CreateAnimCoins(questPopup);
		ResetQuest();
		yield return new WaitForSeconds(1f);
		MenuController.instance.IncreaseCoinAnim(0f);
		MenuController.instance.CloseQuestPopup(0.3f);
	}

	public void ClaimGiftButtonOnClick()
	{
		SoundManager.instance.Click();
		StartCoroutine(ClaimGiftThread());
	}

	private IEnumerator ClaimGiftThread()
	{
		if (giftAmount != 0)
		{
			FortuneWheel.instance.CreateAnimCoins(questPopup);
			yield return new WaitForSeconds(1f);
			MenuController.instance.IncreaseCoinAnim(0f);
		}
		MenuController.instance.CloseQuestPopup(0.3f);
		yield return new WaitForSeconds(0.3f);
	}

	public void QuestFailed()
	{
		SoundManager.instance.Click();
		ResetQuest();
		MenuController.instance.CloseQuestPopup(0.3f);
	}

	public void ResetQuest()
	{
		PlayerPrefsManager.SetQuestRewardAmount(0);
		PlayerPrefsManager.SetQuestEndLevel(0);
		PlayerPrefsManager.SetQuestEndTime(string.Empty);
		PlayerPrefsManager.SetQuestCompleted(0);
		SetQuestButton();
	}

	public void SetQuestButton()
	{
		NewQuestButtonBG(condition: false);
		if (PlayerPrefsManager.GetLevel() >= PlayerPrefsManager.CountLevels() - 1)
		{
			questButton.gameObject.SetActive(value: false);
			return;
		}
		questButton.GetComponent<Button>().onClick.RemoveAllListeners();
		if (PlayerPrefsManager.GetQuestEndTime() == string.Empty)
		{
			if (DateTime.Now.ToString("yy-MM-dd") != PlayerPrefsManager.GetLastQuestTime() && DateTime.Now.Day % 2 == 0 && PlayerPrefsManager.GetLevel() > 20)
			{
				if (!MenuController.fromDaily)
				{
					questButton.gameObject.SetActive(value: true);
				}
				questButton.Find("Frame/Text").gameObject.SetActive(value: false);
				questButton.Find("Frame/Cross").gameObject.SetActive(value: false);
				questButton.Find("Frame/Check").gameObject.SetActive(value: false);
				questButton.GetComponent<Button>().onClick.AddListener(delegate
				{
					GiveQuest();
				});
				StartCoroutine(AnimateQuestButton());
			}
			else
			{
				questButton.gameObject.SetActive(value: false);
			}
		}
		else
		{
			if (!MenuController.fromDaily)
			{
				questButton.gameObject.SetActive(value: true);
			}
			if (DateTime.Compare(DateTime.Now, DateTime.Parse(PlayerPrefsManager.GetQuestEndTime())) < 0 && PlayerPrefsManager.GetLevel() < PlayerPrefsManager.GetQuestEndLevel())
			{
				TimeSpan ts = DateTime.Parse(PlayerPrefsManager.GetQuestEndTime()) - DateTime.Now;
				questButton.Find("Frame/Text").GetComponent<Text>().text = FugoUtils.DateFormatter(LanguageScript.DHText, ts);
				if (ThemeManager.theme == 1)
				{
					questButton.Find("Frame/Text").GetComponent<Text>().text = FugoUtils.DateFormatterNewTheme(null, ts);
				}
				ArabicController.MakeArabicMenu(questButton.Find("Frame/Text"));
				questButton.Find("Frame/Text").gameObject.SetActive(value: true);
				questButton.Find("Frame/Cross").gameObject.SetActive(value: false);
				questButton.Find("Frame/Check").gameObject.SetActive(value: false);
				questButton.GetComponent<Button>().onClick.AddListener(delegate
				{
					OpenQuestPopup("info");
				});
			}
			else
			{
				NewQuestButtonBG(condition: true);
				if (PlayerPrefsManager.GetQuestCompleted() == 0)
				{
					questButton.Find("Frame/Text").gameObject.SetActive(value: false);
					questButton.Find("Frame/Cross").gameObject.SetActive(value: true);
					questButton.Find("Frame/Check").gameObject.SetActive(value: false);
					questButton.GetComponent<Button>().onClick.AddListener(delegate
					{
						OpenQuestPopup("fail");
					});
					StartCoroutine(AnimateQuestButton());
				}
				else
				{
					questButton.Find("Frame/Text").gameObject.SetActive(value: false);
					questButton.Find("Frame/Cross").gameObject.SetActive(value: false);
					questButton.Find("Frame/Check").gameObject.SetActive(value: true);
					questButton.GetComponent<Button>().onClick.AddListener(delegate
					{
						OpenQuestPopup("reward");
					});
					StartCoroutine(AnimateQuestButton());
				}
			}
		}
		if (questButton.Find("Frame/BG") != null)
		{
			questButton.Find("Frame/BG").gameObject.SetActive(questButton.Find("Frame/Text").gameObject.activeSelf);
		}
		ArabicController.MakeArabicMenu(questButton.Find("Frame/Text").parent);
	}

	private void NewQuestButtonBG(bool condition)
	{
		if (questButton.Find("Frame/CrossCheckBG") != null)
		{
			questButton.Find("Frame/CrossCheckBG").gameObject.SetActive(condition);
		}
	}

	private IEnumerator AnimateQuestButton()
	{
		if (ThemeManager.theme != 1)
		{
			yield return new WaitForSeconds(3f);
			float time = 0.5f;
			for (int i = 0; i < 3; i++)
			{
				StartCoroutine(FugoUtils.Scaler(Vector3.one * 1.1f, time / 3f, questButton));
				yield return new WaitForSeconds(time / 3f);
				StartCoroutine(FugoUtils.Scaler(Vector3.one * 1f, time, questButton));
				yield return new WaitForSeconds(time);
			}
			yield return new WaitForSeconds(time * 5f);
			StartCoroutine(AnimateQuestButton());
		}
	}
}
