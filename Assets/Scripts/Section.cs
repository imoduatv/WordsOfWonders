using System.Collections.Generic;

public class Section
{
	public List<LevelSet> sets;

	public List<LevelSet> hiddensets;

	public string SectionID
	{
		get;
		set;
	}

	public string Title
	{
		get;
		set;
	}

	public string TitleColor
	{
		get;
		set;
	}

	public string FirstSetColor
	{
		get;
		set;
	}

	public string LastSetColor
	{
		get;
		set;
	}

	public string KeyColor
	{
		get;
		set;
	}

	public string BGCoverColor
	{
		get;
		set;
	}

	public string BGCoverColorCurrent
	{
		get;
		set;
	}

	public string TextColor
	{
		get;
		set;
	}

	public string SetTitleColor
	{
		get;
		set;
	}

	public string BGName
	{
		get;
		set;
	}

	public string MapName
	{
		get;
		set;
	}

	public string MapColor
	{
		get;
		set;
	}

	public string MapLockedColor
	{
		get;
		set;
	}

	public string TitleBGColor
	{
		get;
		set;
	}

	public Section()
	{
		sets = new List<LevelSet>();
		hiddensets = new List<LevelSet>();
	}

	public Section(string id, string title, string titlecolor, string firstsetcolor, string lastsetcolor, string keycolor, string bgcovercolor, string bgcovercolorcurrent, string textcolor, string settitlecolor, string bgname, string map, string mapcolor, string maplockedcolor, string titlebgcolor)
	{
		sets = new List<LevelSet>();
		hiddensets = new List<LevelSet>();
		SectionID = id;
		Title = title;
		TitleColor = titlecolor;
		FirstSetColor = firstsetcolor;
		LastSetColor = lastsetcolor;
		KeyColor = keycolor;
		BGCoverColor = bgcovercolor;
		BGCoverColorCurrent = bgcovercolorcurrent;
		TextColor = textcolor;
		SetTitleColor = settitlecolor;
		BGName = bgname;
		MapName = map;
		MapColor = mapcolor;
		MapLockedColor = maplockedcolor;
		TitleBGColor = titlebgcolor;
	}

	public LevelSet getHiddenSet(string id)
	{
		foreach (LevelSet hiddenset in hiddensets)
		{
			if (hiddenset.SetID == id)
			{
				return hiddenset;
			}
		}
		return null;
	}
}
