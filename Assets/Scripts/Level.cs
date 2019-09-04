using LitJson;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Level
{
	public int sectionID;

	public int setId;

	public Vector2 position;

	public Vector3 scale;

	public Vector2 offset;

	public float cellSize;

	public GameMode mode;

	public string letters;

	public string id;

	public string gameID;

	public List<Word> words;

	public List<string> foundExtras;

	public int width;

	public int height;

	public string[] otherWords;

	public AdventurePart part;

	public Adventure adventure;

	public int offsetX;

	public int offsetY;

	public LevelSet set;

	public Level()
	{
		words = new List<Word>();
		foundExtras = new List<string>();
	}

	public Level(JsonData data)
	{
		words = new List<Word>();
		foundExtras = new List<string>();
		letters = data["Letters"].ToString();
		id = data["LevelID"].ToString();
		height = int.Parse(data["Row"].ToString());
		width = int.Parse(data["Column"].ToString());
		gameID = string.Empty;
		if (data.Keys.Contains("GameID"))
		{
			gameID = data["GameID"].ToString();
		}
		string[] array = data["Words"].ToString().Split('|');
		try
		{
			otherWords = data["OtherWords"].ToString().Split(',');
		}
		catch (Exception)
		{
		}
		try
		{
			sectionID = int.Parse(data["SectionID"].ToString());
			setId = int.Parse(data["SetID"].ToString());
			offsetX = int.Parse(data["ColumnDifference"].ToString());
			offsetY = int.Parse(data["RowDifference"].ToString());
		}
		catch (Exception)
		{
		}
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
			words.Add(word);
		}
	}

	public Level clone()
	{
		return (Level)MemberwiseClone();
	}
}
