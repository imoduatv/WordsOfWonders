using System.Collections.Generic;
using UnityEngine;

public class LevelSet
{
	public Section section;

	public Vector3 pos;

	public int FirstLevel;

	public List<Level> levels;

	public string SetID
	{
		get;
		set;
	}

	public string SectionID
	{
		get;
		set;
	}

	public string SetName
	{
		get;
		set;
	}

	public string SetFullName
	{
		get;
		set;
	}

	public string SetColor
	{
		get;
		set;
	}

	public string TitleColor
	{
		get;
		set;
	}

	public string RibbonColor
	{
		get;
		set;
	}

	public string CompletedLevelBGColor
	{
		get;
		set;
	}

	public string CompletedLevelLetterColor
	{
		get;
		set;
	}

	public string CompletedLevelNumberColor
	{
		get;
		set;
	}

	public string NotCompletedLevelBGColor
	{
		get;
		set;
	}

	public string NotCompletedLevelLetterColor
	{
		get;
		set;
	}

	public string NotCompletedLevelNumberColor
	{
		get;
		set;
	}

	public string SelectedLevelBGColor
	{
		get;
		set;
	}

	public string SelectedLevelLetterColor
	{
		get;
		set;
	}

	public string SelectedLevelNumberColor
	{
		get;
		set;
	}

	public string InGameLetterColor
	{
		get;
		set;
	}

	public string InGameCircleColor
	{
		get;
		set;
	}

	public string InGameSelectedLetterColor
	{
		get;
		set;
	}

	public string InGameSelectedLetterBGColor
	{
		get;
		set;
	}

	public string InGameTileColor
	{
		get;
		set;
	}

	public string InGameHeaderColor
	{
		get;
		set;
	}

	public string InGameHintColor
	{
		get;
		set;
	}

	public string InGameRibbonColor
	{
		get;
		set;
	}

	public string bgImage
	{
		get;
		set;
	}

	public string TopText
	{
		get;
		set;
	}

	public string BottomText
	{
		get;
		set;
	}

	public int Price
	{
		get;
		set;
	}

	public LevelSet()
	{
		levels = new List<Level>();
	}

	public LevelSet(string id, string sectionid, string name, string fullname, string setcolor, string titlecolor, string ribboncolor, string clevelbg, string clevelletter, string clevelnumber, string nclevelbg, string nclevelletter, string nclevelnumber, string slevelbg, string slevelletter, string slevelnumber, string ingameletter, string ingamecircle, string ingameselectedletter, string ingameselectedletterbg, string ingameheadercolor, string ingamehintcolor, string ingameribboncolor, string tilecolor, string bg, float xcoor, float ycoor, string toptext, string bottext, string price = "0")
	{
		SetID = id;
		SectionID = sectionid;
		SetName = name;
		SetFullName = fullname;
		SetColor = setcolor;
		TitleColor = titlecolor;
		RibbonColor = ribboncolor;
		CompletedLevelBGColor = clevelbg;
		CompletedLevelLetterColor = clevelletter;
		CompletedLevelNumberColor = clevelnumber;
		NotCompletedLevelBGColor = nclevelbg;
		NotCompletedLevelLetterColor = nclevelletter;
		NotCompletedLevelNumberColor = nclevelnumber;
		SelectedLevelBGColor = slevelbg;
		SelectedLevelLetterColor = slevelletter;
		SelectedLevelNumberColor = slevelnumber;
		InGameLetterColor = ingameletter;
		InGameCircleColor = ingamecircle;
		InGameSelectedLetterColor = ingameselectedletter;
		InGameSelectedLetterBGColor = ingameselectedletterbg;
		InGameHeaderColor = ingameheadercolor;
		InGameHintColor = ingamehintcolor;
		InGameRibbonColor = ingameribboncolor;
		InGameTileColor = tilecolor;
		bgImage = bg;
		TopText = toptext;
		BottomText = bottext;
		Price = int.Parse(price);
		pos = new Vector3(xcoor, ycoor, 0f);
		levels = new List<Level>();
	}

	public List<Level> getLevelsWithID(string id)
	{
		List<Level> list = new List<Level>();
		foreach (Level level in levels)
		{
			if (level.gameID == id)
			{
				list.Add(level);
			}
		}
		return list;
	}
}
