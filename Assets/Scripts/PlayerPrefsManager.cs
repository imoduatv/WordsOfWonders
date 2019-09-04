using System;
using System.Collections.Generic;
using UnityEngine;
using v2Gameplay;

public class PlayerPrefsManager : MonoBehaviour
{
	public static bool IsBonusWordTutorialShown()
	{
		return PlayerPrefs.GetInt("bonus_tutorial", 0) == 1;
	}

	public static void BonusTutotialShown()
	{
		PlayerPrefs.SetInt("bonus_tutorial", 1);
	}

	public static int GetSoundEffects()
	{
		return PlayerPrefs.GetInt("soundfx", 1);
	}

	public static void SetSoundEffects(int val)
	{
		PlayerPrefs.SetInt("soundfx", val);
	}

	public static int GetMusic()
	{
		return PlayerPrefs.GetInt("music", 0);
	}

	public static void SetMusic(int val)
	{
		PlayerPrefs.SetInt("music", val);
	}

	public static int GetLevel()
	{
		return PlayerPrefs.GetInt("level", 1);
	}

	public static void SetLevel(int val)
	{
		PlayerPrefs.SetInt("level", val);
		if (GetQuestEndTime() != string.Empty && DateTime.Compare(DateTime.Now, DateTime.Parse(GetQuestEndTime())) < 0 && val == GetQuestEndLevel())
		{
			SetQuestCompleted(1);
			NotificationSystem.DisableQuestLastTwoHoursNotifications();
		}
	}

	public static int GetLatestLevel()
	{
		return PlayerPrefs.GetInt("latest_level", -1);
	}

	public static void SetLatestLevel(int val)
	{
		PlayerPrefs.SetInt("latest_level", val);
	}

	public static bool isItFirst()
	{
		if (GetLatestLevel() != GetLevel())
		{
			return true;
		}
		return false;
	}

	public static void SetBrilliance(int val)
	{
		PlayerPrefs.SetInt("brilliance", val);
	}

	public static int GetBrilliance()
	{
		return PlayerPrefs.GetInt("brilliance", 0);
	}

	public static void DecreaseCoin(int amount)
	{
		SetCoin(GetCoin() - amount);
	}

	public static int GetCoin()
	{
		return PlayerPrefs.GetInt("coin", 250);
	}

	public static void SetCoin(int val)
	{
		PlayerPrefs.SetInt("coin", val);
	}

	public static void IncreaseCoin(int amount)
	{
		SetCoin(GetCoin() + amount);
	}

	public static void SetDaily(int val)
	{
		PlayerPrefs.SetInt("daily", val);
	}

	public static int GetDaily()
	{
		return PlayerPrefs.GetInt("daily");
	}

	public static int GetHintPrice()
	{
		if (TutorialController.freeHint)
		{
			return 0;
		}
		if (GetProHint() > 0 && !GameController.daily && GameController.levelToOpen == -1)
		{
			return 0;
		}
		DateTime t = DateTime.Parse(GetHintDiscountTime());
		if (DateTime.Compare(t, DateTime.Now) > 0)
		{
			return 50;
		}
		return 100;
	}

	public static int GetProHint()
	{
		return PlayerPrefs.GetInt("pro_hints", 0);
	}

	public static void ResetProHint()
	{
		if (GetPro())
		{
			PlayerPrefs.SetInt("pro_hints", 2);
		}
		else
		{
			PlayerPrefs.SetInt("pro_hints", 0);
		}
	}

	public static void UseProHint()
	{
		int proHint = GetProHint();
		PlayerPrefs.SetInt("pro_hints", Mathf.Clamp(proHint - 1, 0, 2));
	}

	public static string GetHintDiscountTime()
	{
		return PlayerPrefs.GetString("hintdiscount", DateTime.Now.AddHours(-1.0).ToString());
	}

	public static void SetHintDiscountTime(string val)
	{
		if (DateTime.Compare(DateTime.Parse(GetHintDiscountTime()), DateTime.Parse(val)) < 0)
		{
			PlayerPrefs.SetString("hintdiscount", val);
		}
	}

	public static int GetNoAd()
	{
		if (GetPro())
		{
			return 1;
		}
		string redeemNoAdDate = GetRedeemNoAdDate();
		if (redeemNoAdDate != string.Empty)
		{
			DateTime t = DateTime.Parse(redeemNoAdDate);
			if (DateTime.Compare(DateTime.Now, t) < 0)
			{
				return 1;
			}
			SetRedeemNoAdDate(string.Empty);
		}
		return PlayerPrefs.GetInt("noad");
	}

	public static void SetNoAd(int val)
	{
		PlayerPrefs.SetInt("noad", val);
	}

	public static int GetDailyProcess()
	{
		if (PlayerPrefs.GetInt("dailymonth") == 0 || PlayerPrefs.GetInt("dailymonth") != DateTime.Now.Month)
		{
			PlayerPrefs.SetInt("dailymonth", DateTime.Now.Month);
			SetDailyProcess(0);
		}
		return PlayerPrefs.GetInt("dailyprocess");
	}

	public static void SetDailyProcess(int val)
	{
		PlayerPrefs.SetInt("dailyprocess", val);
	}

	public static int GetEarnedStar()
	{
		return PlayerPrefs.GetInt("earnedstar");
	}

	public static void SetEarnedStar(int val)
	{
		PlayerPrefs.SetInt("earnedstar", val);
	}

	public static int GetDailyPuzzleDay()
	{
		return PlayerPrefs.GetInt("dailypuzzleday");
	}

	public static void SetDailyPuzzleDay(int val)
	{
		PlayerPrefs.SetInt("dailypuzzleday", val);
	}

	public static string[] GetGalleryString()
	{
		if (PlayerPrefs.GetString("gallerystring") == string.Empty)
		{
			return new string[0];
		}
		return PlayerPrefs.GetString("gallerystring").Split(',');
	}

	public static void SetGalleryString(string val)
	{
		string @string = PlayerPrefs.GetString("gallerystring");
		if (@string == string.Empty)
		{
			PlayerPrefs.SetString("gallerystring", val);
		}
		else if (Array.IndexOf(GetGalleryString(), val) < 0)
		{
			PlayerPrefs.SetString("gallerystring", @string + "," + val);
		}
	}

	public static void SetExtraWordCount(int value, Level game)
	{
		GameMode mode = game.mode;
		string text = string.Empty;
		if (mode == GameMode.NORMAL)
		{
			text = "extra_word";
		}
		if (mode == GameMode.DAILY)
		{
			text = "extra_word_daily";
		}
		if (mode == GameMode.ADVENTURE)
		{
			text = "extra_word_adventure" + game.setId;
		}
		UnityEngine.Debug.Log("extra count " + text);
		PlayerPrefs.SetInt(text, value);
	}

	public static void SetExtraWordCount(int value)
	{
		PlayerPrefs.SetInt("extra_word", value);
	}

	public static int GetExtraWordCount(Level game)
	{
		GameMode mode = game.mode;
		string text = string.Empty;
		if (mode == GameMode.NORMAL)
		{
			text = "extra_word";
		}
		if (mode == GameMode.DAILY)
		{
			text = "extra_word_daily";
		}
		if (mode == GameMode.ADVENTURE)
		{
			text = "extra_word_adventure" + game.setId;
		}
		UnityEngine.Debug.Log("extra count " + text);
		return PlayerPrefs.GetInt(text, 0);
	}

	public static void SetExtraWordCountDaily(int value)
	{
		PlayerPrefs.SetInt("extra_word_daily", value);
	}

	public static int GetExtraWordCountDaily()
	{
		return PlayerPrefs.GetInt("extra_word_daily", 0);
	}

	public static bool IncreaseExtraWordCountDaily()
	{
		int extraWordCountDaily = GetExtraWordCountDaily();
		extraWordCountDaily++;
		SetExtraWordCountDaily(extraWordCountDaily);
		return extraWordCountDaily >= FugoUtils.extraPerCoin;
	}

	public static void ResetExtraWordCountDaily()
	{
		SetExtraWordCountDaily(0);
	}

	public static int IsFirstRun()
	{
		return PlayerPrefs.GetInt("firstrun");
	}

	public static void SetFirstRun()
	{
		PlayerPrefs.SetInt("firstrun", 1);
	}

	public static void setFoundWords(List<Word> words)
	{
		string text = string.Empty;
		foreach (Word word in words)
		{
			text = text + word.word + "|";
		}
		PlayerPrefs.SetString("found_words", text);
	}

	public static void AddHint(int x, int y)
	{
		PlayerPrefs.SetString("hints", GetHint() + "|" + x + ":" + y);
	}

	private static string GetHint()
	{
		return PlayerPrefs.GetString("hints", string.Empty);
	}

	public static int[,] GetHints()
	{
		string hint = GetHint();
		string[] array = hint.Split('|');
		int[,] array2 = new int[array.Length - 1, 2];
		for (int i = 1; i < array.Length; i++)
		{
			string[] array3 = array[i].Split(':');
			array2[i - 1, 0] = int.Parse(array3[0]);
			array2[i - 1, 1] = int.Parse(array3[1]);
		}
		return array2;
	}

	public static void clearHints()
	{
		PlayerPrefs.DeleteKey("hints");
	}

	public static void AddHintDaily(int x, int y)
	{
		PlayerPrefs.SetString("daily_hints", GetHintDaily() + "|" + x + ":" + y);
	}

	private static string GetHintDaily()
	{
		return PlayerPrefs.GetString("daily_hints", string.Empty);
	}

	public static int[,] GetHintsDaily()
	{
		string hintDaily = GetHintDaily();
		string[] array = hintDaily.Split('|');
		int[,] array2 = new int[array.Length - 1, 2];
		for (int i = 1; i < array.Length; i++)
		{
			string[] array3 = array[i].Split(':');
			array2[i - 1, 0] = int.Parse(array3[0]);
			array2[i - 1, 1] = int.Parse(array3[1]);
		}
		return array2;
	}

	public static void clearHintsDaily()
	{
		PlayerPrefs.DeleteKey("daily_hints");
	}

	public static List<string> getFoundWords()
	{
		List<string> list = new List<string>();
		string @string = PlayerPrefs.GetString("found_words", string.Empty);
		string[] array = @string.Split('|');
		string[] array2 = array;
		foreach (string item in array2)
		{
			list.Add(item);
		}
		return list;
	}

	public static void setFoundExtras(List<string> words)
	{
		string text = string.Empty;
		foreach (string word in words)
		{
			text = text + word + "|";
		}
		PlayerPrefs.SetString("found_extra", text);
	}

	public static List<string> getFoundExtras()
	{
		List<string> list = new List<string>();
		string @string = PlayerPrefs.GetString("found_extra", string.Empty);
		string[] array = @string.Split('|');
		string[] array2 = array;
		foreach (string item in array2)
		{
			list.Add(item);
		}
		return list;
	}

	public static void setFoundWordsDaily(List<Word> words)
	{
		string text = string.Empty;
		foreach (Word word in words)
		{
			text = text + word.word + "|";
		}
		PlayerPrefs.SetString("found_words_daily", text);
	}

	public static List<string> getFoundWordsDaily()
	{
		List<string> list = new List<string>();
		string @string = PlayerPrefs.GetString("found_words_daily", string.Empty);
		string[] array = @string.Split('|');
		string[] array2 = array;
		foreach (string item in array2)
		{
			list.Add(item);
		}
		return list;
	}

	public static void setFoundExtrasDaily(List<string> words)
	{
		string text = string.Empty;
		foreach (string word in words)
		{
			text = text + word + "|";
		}
		PlayerPrefs.SetString("found_extra_daily", text);
	}

	public static List<string> getFoundExtrasDaily()
	{
		List<string> list = new List<string>();
		string @string = PlayerPrefs.GetString("found_extra_daily", string.Empty);
		string[] array = @string.Split('|');
		string[] array2 = array;
		foreach (string item in array2)
		{
			list.Add(item);
		}
		return list;
	}

	public static void ResetContinue()
	{
		PlayerPrefs.DeleteKey("found_words");
		PlayerPrefs.DeleteKey("found_extra");
		clearHints();
	}

	public static void ResetContinueDaily()
	{
		deleteDailyStar();
		deleteStarPos();
		PlayerPrefs.SetString("found_words_daily", string.Empty);
		PlayerPrefs.SetString("found_extra_daily", string.Empty);
		clearHintsDaily();
		ResetExtraWordCountDaily();
	}

	public static bool GetFirstExtra()
	{
		return PlayerPrefs.GetInt("first_extra", 0) == 1;
	}

	public static void OnFirstExtraFound()
	{
		PlayerPrefs.SetInt("first_extra", 1);
	}

	public static string GetLang()
	{
		return PlayerPrefs.GetString("language");
	}

	public static void SetLang(string val)
	{
		PlayerPrefs.SetString("language", val);
	}

	public static bool IsItFirstDaily()
	{
		return 1 == PlayerPrefs.GetInt("first_daily", 1);
	}

	public static void FirstDailyDone()
	{
		PlayerPrefs.SetInt("first_daily", 0);
	}

	public static int CountLevels()
	{
		int num = 0;
		foreach (Section section in Games.sections)
		{
			foreach (LevelSet set in section.sets)
			{
				num += set.levels.Count;
			}
		}
		return num;
	}

	public static bool firstInterstitial()
	{
		return PlayerPrefs.GetInt("first_interstitial", 0) == 1;
	}

	public static void interstitialShown()
	{
		PlayerPrefs.SetInt("first_interstitial", 1);
	}

	public static int getDailyStar()
	{
		return PlayerPrefs.GetInt("daily_star", 0);
	}

	public static void setDailyStar(int val)
	{
		PlayerPrefs.SetInt("daily_star", val);
	}

	public static void deleteDailyStar()
	{
		PlayerPrefs.DeleteKey("daily_star");
	}

	public static void deleteStarPosition()
	{
		PlayerPrefs.DeleteKey("star_pos");
	}

	public static void setStarPosition(int x, int y)
	{
		PlayerPrefs.SetString("star_pos", x + "|" + y);
	}

	public static int[] getStarPos()
	{
		int[] array = new int[2];
		string[] array2 = PlayerPrefs.GetString("star_pos", "-1|-1").Split('|');
		array[0] = int.Parse(array2[0]);
		array[1] = int.Parse(array2[1]);
		return array;
	}

	public static void deleteStarPos()
	{
		PlayerPrefs.DeleteKey("star_pos");
	}

	public static void SetRateUs(int value)
	{
		PlayerPrefs.SetInt("rateus", value);
	}

	public static int GetRateUs()
	{
		return PlayerPrefs.GetInt("rateus");
	}

	public static bool IsBlurOn()
	{
		if (PlayerPrefs.GetInt("blurmode") == 1)
		{
			return true;
		}
		return false;
	}

	public static void SetBlur(int val)
	{
		PlayerPrefs.SetInt("blurmode", val);
	}

	public static string GetConstantBG()
	{
		return PlayerPrefs.GetString("constantbg");
	}

	public static void SetConstantBG(string val)
	{
		PlayerPrefs.SetString("constantbg", val);
	}

	public static string GetInGameLetterColor()
	{
		return PlayerPrefs.GetString("ingamelettercolor", string.Empty);
	}

	public static void SetInGameLetterColor(string val)
	{
		PlayerPrefs.SetString("ingamelettercolor", val);
	}

	public static bool IsBlueMode()
	{
		if (PlayerPrefs.GetInt("bluemode") == 1)
		{
			return true;
		}
		return false;
	}

	public static void SetBlueMode(int val)
	{
		PlayerPrefs.SetInt("bluemode", val);
	}

	public static void SetHiddenMenu(int val)
	{
		PlayerPrefs.SetInt("hiddenmenu", val);
	}

	public static bool GetHiddenMenu()
	{
		if (PlayerPrefs.GetInt("hiddenmenu") == 99)
		{
			return true;
		}
		return false;
	}

	public static string GetFBID()
	{
		return PlayerPrefs.GetString("facebookid");
	}

	public static void SetFBID(string id)
	{
		PlayerPrefs.SetString("facebookid", id);
	}

	public static void SetDailyPuzzleNewLabel(int val)
	{
		PlayerPrefs.SetInt("dpnewlabel", val);
	}

	public static int GetDailyPuzzleNewLabel()
	{
		return PlayerPrefs.GetInt("dpnewlabel");
	}

	public static void SetTrackerColor(string s)
	{
		PlayerPrefs.SetString("trackercolor", s);
	}

	public static string GetTrackerColor()
	{
		return PlayerPrefs.GetString("trackercolor", string.Empty);
	}

	public static void SetQuestEndTime(string time)
	{
		PlayerPrefs.SetString("questendtime", time);
	}

	public static string GetQuestEndTime()
	{
		return PlayerPrefs.GetString("questendtime");
	}

	public static void SetQuestEndLevel(int level)
	{
		PlayerPrefs.SetInt("questendlevel", level);
	}

	public static int GetQuestEndLevel()
	{
		return PlayerPrefs.GetInt("questendlevel");
	}

	public static void SetQuestCompleted(int val)
	{
		PlayerPrefs.SetInt("questcompleted", val);
	}

	public static int GetQuestCompleted()
	{
		return PlayerPrefs.GetInt("questcompleted", 0);
	}

	public static void SetQuestRewardAmount(int val)
	{
		PlayerPrefs.SetInt("questreward", val);
	}

	public static int GetQuestRewardAmount()
	{
		return PlayerPrefs.GetInt("questreward");
	}

	public static string GetLastQuestTime()
	{
		return PlayerPrefs.GetString("lastquesttime");
	}

	public static void SetLastQuestTime(string val)
	{
		PlayerPrefs.SetString("lastquesttime", val);
	}

	public static void SetPro(int val)
	{
		PlayerPrefs.SetInt("pro", val);
		if (val == 0)
		{
			ResetProHint();
		}
	}

	public static bool GetPro()
	{
		if (PlayerPrefs.GetInt("pro") == 0)
		{
			return false;
		}
		return true;
	}

	public static void SetFirebaseLogLevel(int val)
	{
		PlayerPrefs.SetInt("firebaseloglevel", val);
	}

	public static int GetFirebaseLogLevel()
	{
		return PlayerPrefs.GetInt("firebaseloglevel");
	}

	public static void SetRedeemNoAdDate(string val)
	{
		PlayerPrefs.SetString("redeemnoaddate", val);
	}

	public static string GetRedeemNoAdDate()
	{
		return PlayerPrefs.GetString("redeemnoaddate");
	}

	public static void SetRedeemProDate(string val)
	{
		PlayerPrefs.SetString("redeemprodate", val);
	}

	public static string GetRedeemProDate()
	{
		return PlayerPrefs.GetString("redeemprodate");
	}

	public static int GetGARandom()
	{
		return PlayerPrefs.GetInt("garandom", -1);
	}

	public static void SetGARandom(int val)
	{
		PlayerPrefs.SetInt("garandom", val);
	}

	public static int GetAdventurePosition(int setID)
	{
		return PlayerPrefs.GetInt("adventure_position" + setID, 0);
	}

	public static void SetAdventurePosition(int pos, int setID)
	{
		PlayerPrefs.SetInt("adventure_position" + setID, pos);
	}

	public static bool IsAdventurePaid(string num)
	{
		string[] array = PlayerPrefs.GetString("paidadventures").Split(',');
		string[] array2 = array;
		foreach (string a in array2)
		{
			if (a == num)
			{
				return true;
			}
		}
		return false;
	}

	public static void SetAdventurePaid(string num)
	{
		if (PlayerPrefs.GetString("paidadventures") == string.Empty)
		{
			PlayerPrefs.SetString("paidadventures", num);
			return;
		}
		string[] array = PlayerPrefs.GetString("paidadventures").Split(',');
		bool flag = true;
		string[] array2 = array;
		foreach (string b in array2)
		{
			if (num == b)
			{
				flag = false;
			}
		}
		if (flag)
		{
			PlayerPrefs.SetString("paidadventures", PlayerPrefs.GetString("paidadventures") + "," + num);
		}
	}

	public static void SetHiddenLevel(int setid, int level)
	{
		string text = setid.ToString() + "|" + level.ToString();
		if (PlayerPrefs.GetString("hiddenlevellevel") == string.Empty)
		{
			PlayerPrefs.SetString("hiddenlevellevel", text);
			return;
		}
		if (GetHiddenLevel(setid) == -1)
		{
			PlayerPrefs.SetString("hiddenlevellevel", PlayerPrefs.GetString("hiddenlevellevel") + "," + text);
			return;
		}
		string[] array = PlayerPrefs.GetString("hiddenlevellevel").Split(',');
		for (int i = 0; i < array.Length; i++)
		{
			if (int.Parse(array[i].Split('|')[0]) == setid)
			{
				array[i] = text;
			}
		}
		PlayerPrefs.SetString("hiddenlevellevel", string.Join(",", array));
	}

	public static int GetHiddenLevel(int setid)
	{
		if (PlayerPrefs.GetString("hiddenlevellevel") == string.Empty)
		{
			return -1;
		}
		string[] array = PlayerPrefs.GetString("hiddenlevellevel").Split(',');
		for (int i = 0; i < array.Length; i++)
		{
			if (int.Parse(array[i].Split('|')[0]) == setid)
			{
				return int.Parse(array[i].Split('|')[1]);
			}
		}
		return -1;
	}

	public static bool GetTutorialDone(int id)
	{
		return PlayerPrefs.GetInt("tutorial_" + id.ToString(), 0) == 1;
	}

	public static void SetTutorialDone(int id)
	{
		PlayerPrefs.SetInt("tutorial_" + id.ToString(), 1);
	}

	public static bool MegaHintShown()
	{
		return GetTutorialDone(8);
	}

	public static GameMode GetLastPlayedMode()
	{
		string @string = PlayerPrefs.GetString("lastplayedmode");
		if (@string == GameMode.NORMAL.ToString())
		{
			return GameMode.NORMAL;
		}
		if (@string == GameMode.ADVENTURE.ToString())
		{
			return GameMode.ADVENTURE;
		}
		return GameMode.NORMAL;
	}

	public static void SetLastPlayedMode(GameMode val)
	{
		PlayerPrefs.SetString("lastplayedmode", val.ToString());
	}

	public static void SetLastHiddenSet(string id)
	{
		PlayerPrefs.SetString("last_hidden_id", id);
	}

	public static string GetLastHiddenSet()
	{
		return PlayerPrefs.GetString("last_hidden_id", string.Empty);
	}

	public static void SetLastHiddenSection(string id)
	{
		PlayerPrefs.SetString("last_hidden_sec_id", id);
	}

	public static string GetLastHiddenSection()
	{
		return PlayerPrefs.GetString("last_hidden_sec_id", string.Empty);
	}

	public static int GetHiddenPos()
	{
		return PlayerPrefs.GetInt("hidden_pos", -1);
	}

	public static void SetHiddenPos(int pos)
	{
		PlayerPrefs.SetInt("hidden_pos", pos);
	}

	public static int GetHiddenGameCount()
	{
		return PlayerPrefs.GetInt("hidden_count", 0);
	}

	public static void SetHiddenCount(int count)
	{
		PlayerPrefs.SetInt("hidden_count", count);
	}

	public static void SetHiddenGameID(string id)
	{
		PlayerPrefs.SetString("hidden_game_id", id);
	}

	public static string GetHiddenGameID()
	{
		return PlayerPrefs.GetString("hidden_game_id");
	}
}
