using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RedeemController : MonoBehaviour
{
	public InputField input;

	public Text errorText;

	public Button collectButton;

	public Transform popup;

	public Transform privacy;

	public Transform fblogin;

	public Transform redeem;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void OKButtonClicked()
	{
		if (input.text.Length != 6)
		{
			StartCoroutine(ErrorText(LanguageScript.RedeemErrorInvalid));
		}
		else
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("code", input.text);
			dictionary.Add("level", PlayerPrefsManager.GetLevel().ToString());
			dictionary.Add("lang", PlayerPrefsManager.GetLang());
			RequestManager.instance.CreateRequest("redeem", CallbackFunction, dictionary);
			ShopScript.instance.ShowWaiting();
		}
		SoundManager.instance.Click();
	}

	public void CallbackFunction(string response)
	{
		ShopScript.instance.HideWaiting();
		collectButton.onClick.RemoveAllListeners();
		if (response == "101")
		{
			StartCoroutine(ErrorText(LanguageScript.RedeemErrorInvalid));
		}
		else if (response == "102")
		{
			StartCoroutine(ErrorText(LanguageScript.RedeemErrorAlreadyUsed));
		}
		else if (response.Contains("|"))
		{
			string[] array = response.Split('|');
			int result = 0;
			int result2 = 0;
			int.TryParse(array[0], out result2);
			int.TryParse(array[1], out result);
			if (result == 0 && result2 == 0)
			{
				StartCoroutine(ErrorText(LanguageScript.GeneralErrorText));
			}
			else if (result2 == -1)
			{
				popup.transform.Find("GiftSection/GiftText").GetComponent<Text>().text = LanguageScript.RedeemNoAdText.Replace("%@", result.ToString());
				collectButton.onClick.AddListener(delegate
				{
					CollectButtonOnClick("level");
				});
				ActivateGiftSection();
				PlayerPrefsManager.SetRedeemNoAdDate(DateTime.Now.AddDays(result).ToString());
				FugoAdManager.instance.HideBanner();
			}
			else if (result2 == -2 && result > 0)
			{
				popup.transform.Find("GiftSection/GiftText").GetComponent<Text>().text = "PRO";
				collectButton.onClick.AddListener(delegate
				{
					CollectButtonOnClick("level");
				});
				PlayerPrefsManager.SetRedeemProDate(DateTime.Now.AddDays(result).ToString());
				PlayerPrefsManager.SetPro(1);
				SceneManager.LoadScene("Menu");
			}
			else if (result != 0 && result2 == 0)
			{
				PlayerPrefsManager.SetLevel(result);
				PlayerPrefsManager.SetBrilliance(result);
				SunScript.instance.SetBrillianceText();
				SectionController.instance.CreateSections();
				popup.transform.Find("GiftSection/GiftText").GetComponent<Text>().text = LanguageScript.RedeemSucceedLevel;
				collectButton.onClick.AddListener(delegate
				{
					CollectButtonOnClick("level");
				});
				ActivateGiftSection();
			}
			else if (result == 0 && result2 != 0)
			{
				PlayerPrefsManager.SetCoin(PlayerPrefsManager.GetCoin() + result2);
				popup.transform.Find("GiftSection/GiftText").GetComponent<Text>().text = LanguageScript.RedeemSucceedCoin;
				popup.transform.Find("GiftSection/GemRewardHolder/AmountText").GetComponent<Text>().text = result2.ToString();
				popup.transform.Find("GiftSection/GemRewardHolder").gameObject.SetActive(value: true);
				collectButton.onClick.AddListener(delegate
				{
					CollectButtonOnClick("coin");
				});
				ActivateGiftSection();
			}
			else
			{
				StartCoroutine(ErrorText(LanguageScript.GeneralErrorText));
			}
		}
		input.text = string.Empty;
	}

	private IEnumerator ErrorText(string str)
	{
		errorText.text = str;
		FugoUtils.ChangeAlpha(errorText, 1f);
		errorText.gameObject.SetActive(value: true);
		yield return new WaitForSeconds(2f);
		StartCoroutine(FugoUtils.FadeText(0f, 0.3f, errorText));
		yield return new WaitForSeconds(0.3f);
		errorText.gameObject.SetActive(value: false);
	}

	public void InputFieldToUpper()
	{
		input.text = input.text.ToUpper();
	}

	public void OpenButtonClicked()
	{
		ActivateCodeSection();
		popup.transform.Find("GiftSection/GemRewardHolder").gameObject.SetActive(value: false);
		MenuController.instance.OpenRedeemPopup();
	}

	private void ActivateGiftSection()
	{
		popup.transform.Find("CodeSection").gameObject.SetActive(value: false);
		popup.transform.Find("GiftSection").gameObject.SetActive(value: true);
	}

	private void ActivateCodeSection()
	{
		popup.transform.Find("CodeSection").gameObject.SetActive(value: true);
		popup.transform.Find("GiftSection").gameObject.SetActive(value: false);
	}

	public void CollectButtonOnClick(string type)
	{
		if (type == "coin")
		{
			StartCoroutine(ClaimGiftThread());
		}
		else
		{
			MenuController.instance.CloseRedeemPopup();
		}
	}

	private IEnumerator ClaimGiftThread()
	{
		FortuneWheel.instance.CreateAnimCoins(popup.transform);
		yield return new WaitForSeconds(1f);
		MenuController.instance.IncreaseCoinAnim(0f);
		MenuController.instance.CloseRedeemPopup();
	}
}
