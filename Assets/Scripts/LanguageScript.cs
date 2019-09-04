using LitJson;
using System;
using System.Reflection;
using UnityEngine;

public class LanguageScript : MonoBehaviour
{
	public static string ExpeditionText;

	public static string PlayText;

	public static string ExploreText;

	public static string DailyPuzzleText;

	public static string SettingsText;

	public static string SoundText;

	public static string NotificationText;

	public static string ONText;

	public static string OFFText;

	public static string ShopText;

	public static string SupportText;

	public static string RestorePurchasesText;

	public static string BuildText;

	public static string RemoveAdsText;

	public static string MostPopularText;

	public static string BestValueText;

	public static string DiscountText;

	public static string WatchText;

	public static string FreeCoinText;

	public static string DailyRewardTitleText;

	public static string DailyRewardContentText;

	public static string DailyGiftHalfPriceHint;

	public static string CongratulationsText;

	public static string MonthNames;

	public static string NextText;

	public static string ComeLaterText;

	public static string OKText;

	public static string CompletedText;

	public static string UnexploredText;

	public static string CollectText;

	public static string DailyPuzzleGalleryHeader;

	public static string SelectLanguageText;

	public static string LanguageText;

	public static string YesText;

	public static string NoText;

	public static string LaterText;

	public static string RateUsHeaderText;

	public static string RateUsContextText;

	public static string DailyGiftNotification;

	public static string DailyPuzzleNotification;

	public static string SetCompletedTutorial;

	public static string NewText;

	public static string LoginText;

	public static string LogoutText;

	public static string AdventureTitleText;

	public static string AdventureQuestText;

	public static string AdventureGiftText;

	public static string AdventureRemainingText;

	public static string AdventureRemainingTimeText;

	public static string DayHourText;

	public static string DHText;

	public static string AdventureRewardText;

	public static string SorryText;

	public static string AdventureFailText;

	public static string AdventureHintDiscountText;

	public static string AdventureNotificationText;

	public static string AdventureNotifictionText2;

	public static string MusicText;

	public static string BecomeProText;

	public static string WeeklyText;

	public static string ProNoAdText;

	public static string ProDoubleDailyText;

	public static string ProFreeHintText;

	public static string ProContentText;

	public static string PrivacyPolicyText;

	public static string ProDescriptionText;

	public static string AlreadyPurchasedText;

	public static string RedeemHeader;

	public static string RedeemInstruction;

	public static string RedeemPlaceholder;

	public static string RedeemErrorAlreadyUsed;

	public static string RedeemErrorInvalid;

	public static string RedeemSucceedCoin;

	public static string RedeemSucceedLevel;

	public static string GeneralErrorText;

	public static string RedeemNoAdText;

	public static string HiddenLevelHeaderText;

	public static string HiddenLevelContentText;

	public static string PlayTextFixed;

	public static string WheelSpinText;

	public static string WheelNoThanksText;

	public static string WheelHintText;

	public static string HintNotificationText;

	public static string ProOneTimeOfferText;

	public static string FreeTrialText;

	public static string SpecialOfferText;

	public static void ParseStrings()
	{
		try
		{
			AutoLanguage.Inıt();
		}
		catch (Exception)
		{
		}
		JsonData jsonData = new JsonData();
		try
		{
			string path = "LangFiles/" + PlayerPrefsManager.GetLang().ToLower();
			TextAsset textAsset = Resources.Load<TextAsset>(path);
			jsonData = JsonMapper.ToObject(textAsset.text);
		}
		catch (Exception)
		{
		}
		for (int i = 0; i < jsonData.Count; i++)
		{
			try
			{
				Type typeFromHandle = typeof(LanguageScript);
				FieldInfo field = typeFromHandle.GetField(jsonData[i]["Key"].ToString().Trim());
				field.SetValue(null, jsonData[i]["Value"].ToString());
			}
			catch (Exception)
			{
			}
			try
			{
				AutoLanguage.dict.Add(jsonData[i]["Key"].ToString().Trim(), AutoLanguage.CorretArabic(jsonData[i]["Value"].ToString()));
			}
			catch (Exception)
			{
			}
		}
		try
		{
			AutoLanguage.instance.onTextChanged();
		}
		catch (Exception)
		{
		}
	}
}
