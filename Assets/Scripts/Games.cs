using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Games : MonoBehaviour
{
	public static List<Section> sections;

	public static Section dailyPuzzles;

	public static Adventure adventure;

	private void Awake()
	{
	}

	private void Start()
	{
	}

	private IEnumerator FetchFromNet()
	{
		string url = "http://www.wordloop.net/wordsofwonders/gamedatafilenew.php?lang=" + PlayerPrefsManager.GetLang() + "&file=false";
		WWW www = new WWW(url);
		yield return www;
		if (string.IsNullOrEmpty(www.error))
		{
			UnityEngine.Debug.Log("fetch from net");
			ParseGameData(www.text);
			SectionController.instance.CreateSections();
		}
	}

	public static void ParseGameData(string str, bool isLevelEditor = false)
	{
		JsonData jsonData = null;
		jsonData = JsonMapper.ToObject(Resources.Load<TextAsset>("gamedata" + PlayerPrefsManager.GetLang().ToLower() + FugoUtils.getGameIndex()).text);
		JsonData data = jsonData["Sections"];
		sections = new List<Section>();
		ParseData(data, sections);
		string empty = string.Empty;
		if (empty != string.Empty)
		{
			List<Section> list = new List<Section>();
			try
			{
				JsonData jsonData2 = JsonMapper.ToObject(Resources.Load<TextAsset>("gamedata" + PlayerPrefsManager.GetLang().ToLower() + "ab" + empty).text);
				ParseData(jsonData2["Sections"], list);
				for (int i = 0; i < list.Count; i++)
				{
					for (int j = 0; j < list[i].sets.Count; j++)
					{
						for (int k = 0; k < list[i].sets[j].levels.Count; k++)
						{
							sections[i].sets[j].levels[k] = list[i].sets[j].levels[k];
						}
					}
				}
			}
			catch
			{
				UnityEngine.Debug.Log("there is no ab levels");
			}
		}
		JsonData jsonData3 = jsonData["DailyLevels"];
		dailyPuzzles = new Section();
		for (int l = 0; l < jsonData3.Count; l++)
		{
			LevelSet levelSet = new LevelSet(jsonData3[l]["SetID"].ToString(), jsonData3[l]["SectionID"].ToString(), jsonData3[l]["SetName"].ToString(), jsonData3[l]["SetFullName"].ToString(), jsonData3[l]["SetColor"].ToString(), jsonData3[l]["TitleColor"].ToString(), jsonData3[l]["RibbonColor"].ToString(), jsonData3[l]["CompletedLevelBGColor"].ToString(), jsonData3[l]["CompletedLevelLetterColor"].ToString(), jsonData3[l]["CompletedLevelNumberColor"].ToString(), jsonData3[l]["NotCompletedLevelBGColor"].ToString(), jsonData3[l]["NotCompletedLevelLetterColor"].ToString(), jsonData3[l]["NotCompletedLevelNumberColor"].ToString(), jsonData3[l]["SelectedLevelBGColor"].ToString(), jsonData3[l]["SelectedLevelLetterColor"].ToString(), jsonData3[l]["SelectedLevelNumberColor"].ToString(), jsonData3[l]["InGameLetterColor"].ToString(), jsonData3[l]["InGameCircleColor"].ToString(), jsonData3[l]["InGameSelectedLetterColor"].ToString(), jsonData3[l]["InGameSelectedLetterBGColor"].ToString(), jsonData3[l]["InGameHeaderColor"].ToString(), jsonData3[l]["InGameHintColor"].ToString(), jsonData3[l]["InGameRibbonColor"].ToString(), jsonData3[l]["InGameTileColor"].ToString(), jsonData3[l]["BackgroundImage"].ToString(), int.Parse(jsonData3[l]["XCoordinate"].ToString()), int.Parse(jsonData3[l]["YCoordinate"].ToString()), jsonData3[l]["TopText"].ToString(), jsonData3[l]["BottomText"].ToString());
			JsonData jsonData4 = jsonData3[l]["Levels"];
			for (int m = 0; m < jsonData4.Count; m++)
			{
				Level level = new Level();
				level.letters = jsonData4[m]["Letters"].ToString();
				level.id = jsonData4[m]["LevelID"].ToString();
				level.height = int.Parse(jsonData4[m]["Row"].ToString());
				level.width = int.Parse(jsonData4[m]["Column"].ToString());
				try
				{
					level.otherWords = jsonData4[m]["OtherWords"].ToString().Split(',');
				}
				catch (Exception)
				{
				}
				string[] array = jsonData4[m]["Words"].ToString().Split('|');
				string[] array2 = array;
				foreach (string text in array2)
				{
					string[] array3 = text.Split(',');
					Word word = new Word();
					word.Y = int.Parse(array3[0]);
					word.X = int.Parse(array3[1]);
					word.word = array3[2];
					word.length = word.word.Length;
					word.positions = new Vector3[word.length];
					word.letters = new GameObject[word.length];
					if (array3[3] == "H" || array3[3] == "BH")
					{
						word.orientation = Orientation.HORIZONTAL;
						if (array3[3] == "BH")
						{
							word.bonus = true;
						}
					}
					else if (array3[3] == "V" || array3[3] == "BV")
					{
						word.orientation = Orientation.VERTICAL;
						if (array3[3] == "BV")
						{
							word.bonus = true;
						}
					}
					level.words.Add(word);
				}
				levelSet.levels.Add(level);
			}
			dailyPuzzles.sets.Add(levelSet);
		}
		Resources.UnloadUnusedAssets();
	}

	private static void ParseData(JsonData data, List<Section> sectionList)
	{
		for (int i = 0; i < data.Count; i++)
		{
			Section section = new Section(data[i]["SectionID"].ToString(), data[i]["Title"].ToString(), data[i]["TitleColor"].ToString(), data[i]["FirstSetColor"].ToString(), data[i]["LastSetColor"].ToString(), data[i]["KeyColor"].ToString(), data[i]["BGCoverColor"].ToString(), data[i]["BGCoverColorCurrent"].ToString(), data[i]["TextColor"].ToString(), data[i]["SetTitleColor"].ToString(), data[i]["BGName"].ToString(), data[i]["MapName"].ToString(), data[i]["MapColor"].ToString(), data[i]["MapLockedColor"].ToString(), data[i]["TitleBGColor"].ToString());
			JsonData jsonData = data[i]["Sets"];
			for (int j = 0; j < jsonData.Count; j++)
			{
				LevelSet levelSet = new LevelSet(jsonData[j]["SetID"].ToString(), jsonData[j]["SectionID"].ToString(), jsonData[j]["SetName"].ToString(), jsonData[j]["SetFullName"].ToString(), jsonData[j]["SetColor"].ToString(), jsonData[j]["TitleColor"].ToString(), jsonData[j]["RibbonColor"].ToString(), jsonData[j]["CompletedLevelBGColor"].ToString(), jsonData[j]["CompletedLevelLetterColor"].ToString(), jsonData[j]["CompletedLevelNumberColor"].ToString(), jsonData[j]["NotCompletedLevelBGColor"].ToString(), jsonData[j]["NotCompletedLevelLetterColor"].ToString(), jsonData[j]["NotCompletedLevelNumberColor"].ToString(), jsonData[j]["SelectedLevelBGColor"].ToString(), jsonData[j]["SelectedLevelLetterColor"].ToString(), jsonData[j]["SelectedLevelNumberColor"].ToString(), jsonData[j]["InGameLetterColor"].ToString(), jsonData[j]["InGameCircleColor"].ToString(), jsonData[j]["InGameSelectedLetterColor"].ToString(), jsonData[j]["InGameSelectedLetterBGColor"].ToString(), jsonData[j]["InGameHeaderColor"].ToString(), jsonData[j]["InGameHintColor"].ToString(), jsonData[j]["InGameRibbonColor"].ToString(), jsonData[j]["InGameTileColor"].ToString(), jsonData[j]["BackgroundImage"].ToString(), int.Parse(jsonData[j]["XCoordinate"].ToString()), int.Parse(jsonData[j]["YCoordinate"].ToString()), jsonData[j]["TopText"].ToString(), jsonData[j]["BottomText"].ToString());
				JsonData jsonData2 = jsonData[j]["Levels"];
				for (int k = 0; k < jsonData2.Count; k++)
				{
					Level level = new Level();
					level.letters = jsonData2[k]["Letters"].ToString();
					level.id = jsonData2[k]["LevelID"].ToString();
					level.height = int.Parse(jsonData2[k]["Row"].ToString());
					level.width = int.Parse(jsonData2[k]["Column"].ToString());
					try
					{
						level.otherWords = jsonData2[k]["OtherWords"].ToString().Split(',');
					}
					catch (Exception)
					{
					}
					string[] array = jsonData2[k]["Words"].ToString().Split('|');
					string[] array2 = array;
					foreach (string text in array2)
					{
						string[] array3 = text.Split(',');
						Word word = new Word();
						word.Y = int.Parse(array3[0]);
						word.X = int.Parse(array3[1]);
						word.word = array3[2];
						word.length = word.word.Length;
						word.positions = new Vector3[word.length];
						word.letters = new GameObject[word.length];
						if (array3[3] == "H" || array3[3] == "BH")
						{
							word.orientation = Orientation.HORIZONTAL;
							if (array3[3] == "BH")
							{
								word.bonus = true;
							}
						}
						else if (array3[3] == "V" || array3[3] == "BV")
						{
							word.orientation = Orientation.VERTICAL;
							if (array3[3] == "BV")
							{
								word.bonus = true;
							}
						}
						level.words.Add(word);
					}
					levelSet.levels.Add(level);
				}
				section.sets.Add(levelSet);
			}
			if (data[i].Keys.Contains("HiddenSets"))
			{
				jsonData = data[i]["HiddenSets"];
				for (int m = 0; m < jsonData.Count; m++)
				{
					LevelSet levelSet2 = new LevelSet(jsonData[m]["SetID"].ToString(), jsonData[m]["SectionID"].ToString(), jsonData[m]["SetName"].ToString(), jsonData[m]["SetFullName"].ToString(), jsonData[m]["SetColor"].ToString(), jsonData[m]["TitleColor"].ToString(), jsonData[m]["RibbonColor"].ToString(), jsonData[m]["CompletedLevelBGColor"].ToString(), jsonData[m]["CompletedLevelLetterColor"].ToString(), jsonData[m]["CompletedLevelNumberColor"].ToString(), jsonData[m]["NotCompletedLevelBGColor"].ToString(), jsonData[m]["NotCompletedLevelLetterColor"].ToString(), jsonData[m]["NotCompletedLevelNumberColor"].ToString(), jsonData[m]["SelectedLevelBGColor"].ToString(), jsonData[m]["SelectedLevelLetterColor"].ToString(), jsonData[m]["SelectedLevelNumberColor"].ToString(), jsonData[m]["InGameLetterColor"].ToString(), jsonData[m]["InGameCircleColor"].ToString(), jsonData[m]["InGameSelectedLetterColor"].ToString(), jsonData[m]["InGameSelectedLetterBGColor"].ToString(), jsonData[m]["InGameHeaderColor"].ToString(), jsonData[m]["InGameHintColor"].ToString(), jsonData[m]["InGameRibbonColor"].ToString(), jsonData[m]["InGameTileColor"].ToString(), jsonData[m]["BackgroundImage"].ToString(), int.Parse(jsonData[m]["XCoordinate"].ToString()), int.Parse(jsonData[m]["YCoordinate"].ToString()), jsonData[m]["TopText"].ToString(), jsonData[m]["BottomText"].ToString(), jsonData[m]["Price"].ToString());
					JsonData jsonData3 = jsonData[m]["Levels"];
					for (int n = 0; n < jsonData3.Count; n++)
					{
						Level level2 = new Level();
						level2.letters = jsonData3[n]["Letters"].ToString();
						level2.id = jsonData3[n]["LevelID"].ToString();
						level2.height = int.Parse(jsonData3[n]["Row"].ToString());
						level2.width = int.Parse(jsonData3[n]["Column"].ToString());
						level2.offsetX = int.Parse(jsonData3[n]["ColumnDifference"].ToString());
						level2.offsetY = int.Parse(jsonData3[n]["RowDifference"].ToString());
						level2.gameID = string.Empty;
						if (jsonData3[n].Keys.Contains("GameID"))
						{
							level2.gameID = jsonData3[n]["GameID"].ToString();
						}
						try
						{
							level2.otherWords = jsonData3[n]["OtherWords"].ToString().Split(',');
						}
						catch (Exception)
						{
						}
						string[] array4 = jsonData3[n]["Words"].ToString().Split('|');
						string[] array5 = array4;
						foreach (string text2 in array5)
						{
							string[] array6 = text2.Split(',');
							Word word2 = new Word();
							word2.Y = int.Parse(array6[0]);
							word2.X = int.Parse(array6[1]);
							word2.word = array6[2];
							word2.length = word2.word.Length;
							word2.positions = new Vector3[word2.length];
							word2.letters = new GameObject[word2.length];
							if (array6[3] == "H" || array6[3] == "BH")
							{
								word2.orientation = Orientation.HORIZONTAL;
								if (array6[3] == "BH")
								{
									word2.bonus = true;
								}
							}
							else if (array6[3] == "V" || array6[3] == "BV")
							{
								word2.orientation = Orientation.VERTICAL;
								if (array6[3] == "BV")
								{
									word2.bonus = true;
								}
							}
							level2.words.Add(word2);
						}
						levelSet2.levels.Add(level2);
					}
					section.hiddensets.Add(levelSet2);
				}
			}
			if (section.sets[0].levels.Count > 0)
			{
				sectionList.Add(section);
			}
		}
	}

	public static Section GetSection(string id)
	{
		foreach (Section section in sections)
		{
			if (section.SectionID == id)
			{
				return section;
			}
		}
		return null;
	}
}
