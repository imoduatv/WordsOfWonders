using System;
using UnityEngine;

public class Word
{
	public bool bonus;

	public bool stroke;

	public bool found;

	public string word;

	public Orientation orientation;

	public int X;

	public int Y;

	public int length;

	public Vector3[] positions;

	public GameObject[] letters;

	public Cell[] cells;

	public Word()
	{
		stroke = true;
		bonus = false;
		found = false;
	}

	public void setFound()
	{
		found = true;
		Cell[] array = cells;
		foreach (Cell cell in array)
		{
			cell.setFound();
		}
	}

	public bool isFound()
	{
		Cell[] array = cells;
		foreach (Cell cell in array)
		{
			if (!cell.found)
			{
				found = false;
			}
		}
		return found;
	}

	public void checkFound()
	{
		GameObject[] array = letters;
		foreach (GameObject gameObject in array)
		{
			if (!gameObject.GetComponent<Cell>().found)
			{
				found = false;
				return;
			}
		}
		found = true;
	}

	public bool checkWordDone()
	{
		GameObject[] array = letters;
		foreach (GameObject gameObject in array)
		{
			if (!gameObject.GetComponent<Cell>().found)
			{
				return false;
			}
		}
		return true;
	}

	public void enableStroke()
	{
		if (stroke)
		{
			for (int i = 0; i < letters.Length; i++)
			{
				Cell component = letters[i].GetComponent<Cell>();
				Movements.instance.executeWithDelay((Movements.Execute)component.enableStroke, 0.05f * (float)i);
				Movements.instance.executeWithDelay((Movements.Execute)SoundManager.instance.LetterPlace, 0.1f * (float)i);
			}
		}
	}

	public void checkBonus()
	{
		if (!bonus)
		{
			GameObject[] array = letters;
			foreach (GameObject gameObject in array)
			{
				try
				{
					GameObject gameObject2 = gameObject.transform.Find("BG/Coin").gameObject;
					if (gameObject2.activeSelf)
					{
						Movements.instance.scale(gameObject2, 0f, 0.15f, active: false);
					}
				}
				catch (Exception)
				{
				}
			}
		}
	}

	public void checkStar()
	{
	}

	public void enableWord()
	{
		bonus = false;
		found = true;
		GameObject[] array = letters;
		foreach (GameObject gameObject in array)
		{
			Cell component = gameObject.GetComponent<Cell>();
			component.enableLetter();
		}
	}
}
