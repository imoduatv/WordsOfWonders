using PartaGames.Android;
using System;
using UnityEngine;

public class NotificationSystem : MonoBehaviour
{
	public static void CreateNewNotification(int secondsLater, string msg, int id)
	{
		RegisterForNotifications();
		LocalNotificationUnity.SendNotification("parta_default_channel_id", Application.productName, msg, DateTime.Now.AddSeconds(1.0 * (double)secondsLater), id);
	}

	public static void RegisterForNotifications()
	{
		LocalNotificationUnity.CreateChannel("parta_default_channel_id", "Notification", "WOW Notification", NotificationImportance.HIGH, enableLights: true, enableVibration: true);
	}

	public static void DisableDailyNotifications()
	{
		DisableNotification(123);
		DisableNotification(1234);
		DisableNotification(1235);
		DisableNotification(1236);
		DisableNotification(1237);
		DisableNotification(1238);
		DisableNotification(1239);
		DisableNotification(1240);
		DisableNotification(1241);
		DisableNotification(1242);
		DisableNotification(1243);
		DisableNotification(1244);
		DisableNotification(1245);
	}

	public static void CreateDailyNotification()
	{
		DisableDailyNotifications();
		CreateNewNotification(86400, LanguageScript.DailyGiftNotification, 1234);
		CreateNewNotification(172800, LanguageScript.DailyGiftNotification, 1235);
		CreateNewNotification(259200, LanguageScript.DailyGiftNotification, 1245);
		CreateNewNotification(345600, LanguageScript.DailyGiftNotification, 123);
		CreateNewNotification(432000, LanguageScript.DailyGiftNotification, 1236);
		CreateNewNotification(518400, LanguageScript.DailyGiftNotification, 1237);
		CreateNewNotification(604800, LanguageScript.DailyGiftNotification, 1238);
		CreateNewNotification(691200, LanguageScript.DailyGiftNotification, 1239);
		CreateNewNotification(777600, LanguageScript.DailyGiftNotification, 1240);
		CreateNewNotification(864000, LanguageScript.DailyGiftNotification, 1241);
		CreateNewNotification(1209600, LanguageScript.DailyGiftNotification, 1242);
		CreateNewNotification(1814400, LanguageScript.DailyGiftNotification, 1243);
		CreateNewNotification(2419200, LanguageScript.DailyGiftNotification, 1244);
	}

	public static void CreateQuestLastTwoHoursNotification(int seconds)
	{
		DisableQuestLastTwoHoursNotifications();
		CreateNewNotification(seconds, LanguageScript.AdventureNotifictionText2, 321);
	}

	public static void DisableQuestLastTwoHoursNotifications()
	{
		DisableNotification(321);
	}

	public static void CreateQuestReadyNotification(int seconds)
	{
		DisableQuestReadyNotifications();
		CreateNewNotification(seconds, LanguageScript.AdventureNotificationText, 4321);
	}

	public static void DisableQuestReadyNotifications()
	{
		DisableNotification(4321);
	}

	public static void CreateHintNotification(string word)
	{
		DisableHintNotification();
		CreateNewNotification(172800, LanguageScript.HintNotificationText.Replace("%@", word), 1111);
		CreateNewNotification(345600, LanguageScript.HintNotificationText.Replace("%@", word), 1112);
	}

	public static void DisableHintNotification()
	{
		DisableNotification(1111);
		DisableNotification(1112);
		DisableNotification(1235);
		DisableNotification(123);
	}

	public static void DisableHintNotifications()
	{
		DisableNotification(1111);
		DisableNotification(1112);
		CreateNewNotification(172800, LanguageScript.DailyGiftNotification, 1235);
		CreateNewNotification(345600, LanguageScript.DailyGiftNotification, 123);
	}

	public static void DisableNotification(int id)
	{
		LocalNotificationUnity.CancelNotification(id);
	}

	public static void DisableIosNotification(string id)
	{
	}
}
