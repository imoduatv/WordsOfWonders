using System.Collections.Generic;
using UnityEngine;

public class Adventure
{
	public static int xBorder;

	public static int yBorder;

	public int curPos;

	public int Height;

	public int Width;

	public float scale;

	public Vector2 offset;

	public Section section;

	public LevelSet set;

	public Level[] Levels;

	public Adventure(Section sec, LevelSet s, List<Level> lvls)
	{
		Width = -1;
		section = sec;
		set = s;
		Levels = new Level[lvls.Count];
		for (int i = 0; i < lvls.Count; i++)
		{
			Levels[i] = lvls[i];
			Levels[i].adventure = this;
			Levels[i].mode = GameMode.ADVENTURE;
			Levels[i].part = AdventurePart.MID;
		}
		Levels[0].part = AdventurePart.START;
		Levels[Levels.Length - 1].part = AdventurePart.END;
	}

	private void setAdventure()
	{
		Level[] levels = Levels;
		foreach (Level level in levels)
		{
			UnityEngine.Debug.Log("ITEM NULL " + (level == null));
			level.adventure = this;
		}
	}
}
