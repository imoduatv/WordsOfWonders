using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextController : MonoBehaviour
{
	public delegate void TextDelegate();

	public static TextController instance;

	public Canvas canvas;

	public Text playButton;

	public Text exploreButton;

	public Text dailyPuzzleButton;

	public Text sectionHeader;

	public Text brillianceText;

	public Text settingsTitle;

	public Text soundText;

	public Text shopButtonText;

	public Text supportButtonText;

	public Text restorePurchaseText;

	public Text fbtext;

	public Text musicText;

	public Text redeemText;

	public Text privacyText;

	public Text dailyPlayButton;

	public Text dailyNextButton;

	public Text dailyPopup;

	public Text dailyPopupHeader;

	public Text prizePopupHeader;

	public Text prizePopupCongratz;

	public Text prizePopupCollect;

	public Text dailyGalleryHeader;

	public Text dailyPuzzleOKText;

	public Text newText;

	public Text shopTitle;

	public Text rewardedPopupButton;

	public Text rewardedPopupText;

	public Text dailyRewardTitle;

	public Text dailyRewardInfo;

	public Text dailyRewardCollect;

	public Text dailyRewardCongratz;

	public Text dailyRewardHalfPriceHint;

	public Text setCompletedTutorialText;

	public Text questGiftHeaderText;

	public Text questGiftContentText;

	public Text questGiftCollectText;

	public Text questHeaderText;

	public Text questYesText;

	public Text questNoText;

	public Text questInfoHeaderText;

	public Text questInfoOKText;

	public Text questRewardHeaderText;

	public Text questRewardContentText;

	public Text questRewardOKText;

	public Text questFailHeaderText;

	public Text questFailContentText;

	public Text questFailOKText;

	public Text proHintText;

	public Text proNoAdText;

	public Text proGiftText;

	public Text becomeProText;

	public Text proTermsText;

	public Text redeemExplanationText;

	public Text redeemPlaceholderText;

	public Text redeemOKText;

	public Text redeemCongratzText;

	public Text redeemCollectText;

	public Text adventureHeder;

	public Text adventureContent;

	public Text wheelSpinText;

	public Text wheelNoThanksText;

	public Text wheelHintPopupText;

	public Text trialProSpecialOfferText;

	public Text trialProOneTimeText;

	public Text trialProHintText;

	public Text trialProNoadText;

	public Text trialProGiftText;

	public Text trialBecomeProText;

	public Text trialProTermsText;

	public Font regular;

	public Font light;

	public Font semi;

	public Transform langHolder;

	public TextDelegate SetTextProperties;

	private bool textSizesFixed;

	private bool textSizesFixedSettings;

	private bool textSizesFixedLanguage;

	private void Awake()
	{
		instance = this;
	}

	private void Update()
	{
		if (!Input.GetKeyDown(KeyCode.R))
		{
		}
	}

	public void SetTexts(string type = "")
	{
		if (type == "facebook")
		{
			if (PlayerPrefsManager.GetFBID() == string.Empty)
			{
				fbtext.text = LanguageScript.LoginText;
			}
			else
			{
				fbtext.text = LanguageScript.LogoutText;
			}
			ArabicController.MakeArabicMenu(fbtext.transform.parent);
			return;
		}
		playButton.text = LanguageScript.PlayText;
		exploreButton.text = LanguageScript.ExploreText;
		dailyPuzzleButton.text = LanguageScript.DailyPuzzleText;
		settingsTitle.text = LanguageScript.SettingsText;
		soundText.text = LanguageScript.SoundText;
		shopButtonText.text = LanguageScript.LanguageText;
		restorePurchaseText.text = LanguageScript.RestorePurchasesText;
		redeemText.text = LanguageScript.RedeemHeader;
		dailyPlayButton.text = LanguageScript.PlayText;
		dailyNextButton.text = LanguageScript.NextText;
		dailyPopup.text = LanguageScript.ComeLaterText;
		dailyPopupHeader.text = LanguageScript.DailyPuzzleText;
		shopTitle.text = LanguageScript.ShopText;
		rewardedPopupText.text = LanguageScript.FreeCoinText;
		rewardedPopupButton.text = LanguageScript.WatchText;
		prizePopupHeader.text = LanguageScript.DailyPuzzleText;
		prizePopupCongratz.text = LanguageScript.CongratulationsText;
		prizePopupCollect.text = LanguageScript.CollectText;
		dailyGalleryHeader.text = LanguageScript.DailyPuzzleGalleryHeader;
		supportButtonText.text = LanguageScript.SupportText;
		dailyRewardInfo.text = LanguageScript.DailyRewardContentText;
		dailyRewardTitle.text = LanguageScript.DailyRewardTitleText;
		dailyRewardCollect.text = LanguageScript.CollectText;
		dailyRewardCongratz.text = LanguageScript.CongratulationsText;
		if (PlayerPrefsManager.GetPro())
		{
			LanguageScript.DailyGiftHalfPriceHint = LanguageScript.DailyGiftHalfPriceHint.Replace("1", "2");
		}
		dailyRewardHalfPriceHint.text = LanguageScript.DailyGiftHalfPriceHint;
		dailyPuzzleOKText.text = LanguageScript.OKText;
		setCompletedTutorialText.text = LanguageScript.SetCompletedTutorial;
		newText.text = LanguageScript.NewText;
		if (PlayerPrefsManager.GetFBID() == string.Empty)
		{
			fbtext.text = LanguageScript.LoginText;
		}
		else
		{
			fbtext.text = LanguageScript.LogoutText;
		}
		musicText.text = LanguageScript.MusicText;
		questGiftHeaderText.text = LanguageScript.CongratulationsText;
		questGiftContentText.text = LanguageScript.AdventureGiftText;
		questGiftCollectText.text = LanguageScript.CollectText;
		questHeaderText.text = LanguageScript.AdventureTitleText;
		questYesText.text = LanguageScript.YesText;
		questNoText.text = LanguageScript.NoText;
		questInfoHeaderText.text = LanguageScript.AdventureTitleText;
		questInfoOKText.text = LanguageScript.OKText;
		questRewardHeaderText.text = LanguageScript.CongratulationsText;
		questRewardContentText.text = LanguageScript.AdventureRewardText;
		questRewardOKText.text = LanguageScript.CollectText;
		questFailHeaderText.text = LanguageScript.SorryText;
		questFailContentText.text = LanguageScript.AdventureFailText;
		questFailOKText.text = LanguageScript.OKText;
		proHintText.text = LanguageScript.ProFreeHintText;
		proNoAdText.text = LanguageScript.ProNoAdText;
		proGiftText.text = LanguageScript.ProDoubleDailyText;
		becomeProText.text = LanguageScript.BecomeProText;
		proTermsText.text = LanguageScript.PrivacyPolicyText;
		redeemOKText.text = LanguageScript.OKText;
		redeemCollectText.text = LanguageScript.CollectText;
		redeemCongratzText.text = LanguageScript.CongratulationsText;
		redeemExplanationText.text = LanguageScript.RedeemInstruction;
		redeemPlaceholderText.text = LanguageScript.RedeemPlaceholder;
		adventureHeder.text = LanguageScript.HiddenLevelHeaderText;
		adventureContent.text = LanguageScript.HiddenLevelContentText;
		if (ThemeManager.theme == 0)
		{
			try
			{
				SunScript.instance.SetBrillianceText();
			}
			catch (Exception)
			{
			}
		}
		SetBrillianceText();
		MenuController.instance.SetLangText();
		wheelSpinText.text = LanguageScript.WheelSpinText;
		wheelNoThanksText.text = LanguageScript.WheelNoThanksText;
		wheelHintPopupText.text = LanguageScript.DailyGiftHalfPriceHint;
		trialProHintText.text = LanguageScript.ProFreeHintText;
		trialProNoadText.text = LanguageScript.ProNoAdText;
		trialProGiftText.text = LanguageScript.ProDoubleDailyText;
		trialBecomeProText.text = LanguageScript.BecomeProText;
		trialProTermsText.text = LanguageScript.PrivacyPolicyText;
		trialProOneTimeText.text = LanguageScript.ProOneTimeOfferText;
		trialProSpecialOfferText.text = LanguageScript.SpecialOfferText;
		sectionHeader.text = LanguageScript.ExploreText;
		ArabicController.MakeArabicMenu(GameObject.Find("Canvas").transform);
		if (PlayerPrefsManager.GetLang() == "Arabic")
		{
			shopTitle.transform.localScale = new Vector3(-1f, 1f, 1f);
			shopTitle.transform.parent.localScale = new Vector3(-1f, 1f, 1f);
		}
		else
		{
			shopTitle.transform.localScale = new Vector3(1f, 1f, 1f);
			shopTitle.transform.parent.localScale = new Vector3(1f, 1f, 1f);
		}
		SetTextProperties();
		if (!textSizesFixed && ThemeManager.theme == 1)
		{
			textSizesFixed = true;
			List<Text> list = new List<Text>();
			list.Add(playButton);
			list.Add(exploreButton);
			list.Add(dailyPuzzleButton);
			StartCoroutine(FugoUtils.SetSameTextSize(list, canvas));
		}
	}

	public void FixSettingsTextSizes()
	{
		if (ThemeManager.theme == 1)
		{
			StartCoroutine(FixTextSizes());
		}
	}

	private IEnumerator FixTextSizes()
	{
		if (!textSizesFixedSettings)
		{
			textSizesFixedSettings = true;
			yield return null;
			StartCoroutine(FugoUtils.SetSameTextSize(new List<Text>
			{
				shopButtonText,
				supportButtonText,
				restorePurchaseText,
				fbtext,
				privacyText
			}, canvas));
		}
	}

	public void FixLangsTextSizes()
	{
		if (ThemeManager.theme == 1)
		{
			StartCoroutine(FixLangsTextSizesThread());
		}
	}

	private IEnumerator FixLangsTextSizesThread()
	{
		if (!textSizesFixedLanguage)
		{
			textSizesFixedLanguage = true;
			yield return null;
			List<Text> sameSizeTexts = new List<Text>();
			IEnumerator enumerator = langHolder.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Transform transform = (Transform)enumerator.Current;
					sameSizeTexts.Add(transform.GetComponent<Text>());
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
			StartCoroutine(FugoUtils.SetSameTextSize(sameSizeTexts, canvas));
		}
	}

	public void SetBrillianceText()
	{
		brillianceText.text = PlayerPrefsManager.GetBrilliance().ToString() + "\n<color=#88d5ff><size=25>" + LanguageScript.ExpeditionText + "</size></color>";
		if (PlayerPrefsManager.GetLang() == "Arabic" || PlayerPrefsManager.GetLang() == "Hebrew")
		{
			brillianceText.text = PlayerPrefsManager.GetBrilliance().ToString() + "\n\n" + LanguageScript.ExpeditionText;
		}
	}
}
