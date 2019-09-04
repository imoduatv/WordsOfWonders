using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelEditorScript : MonoBehaviour
{
	public Transform parent;

	public GameObject gamePrefab;

	public float canvassize;

	private int startIndex;

	private int endIndex;

	private void Start()
	{
		startIndex = 1;
		endIndex = 20;
		Games.ParseGameData(string.Empty, isLevelEditor: true);
		Vector2 sizeDelta = GameObject.Find("Canvas").GetComponent<RectTransform>().sizeDelta;
		canvassize = sizeDelta.x;
		CreateGames();
	}

	private void Update()
	{
	}

	private void CreateGames()
	{
		IEnumerator enumerator = parent.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform transform = (Transform)enumerator.Current;
				UnityEngine.Object.Destroy(transform.gameObject);
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		int num = 1;
		foreach (Section section in Games.sections)
		{
			foreach (LevelSet set in section.sets)
			{
				int num2 = 0;
				foreach (Level level in set.levels)
				{
					num2++;
					if (num < startIndex)
					{
						num++;
					}
					else
					{
						if (num > endIndex)
						{
							parent.parent.parent.GetComponent<ScrollRect>().normalizedPosition = new Vector2(0f, 1f);
							return;
						}
						Transform transform2 = UnityEngine.Object.Instantiate(gamePrefab, parent).transform;
						transform2.localScale = Vector3.one;
						Vector2 sizeDelta = transform2.GetComponent<RectTransform>().sizeDelta;
						sizeDelta.x = canvassize;
						transform2.GetComponent<RectTransform>().sizeDelta = sizeDelta;
						Transform transform3 = BoardController.getLevel(num).transform;
						transform3.SetParent(transform2);
						transform3.localScale = Vector3.one;
						Vector2 sizeDelta2 = transform3.Find("Board").GetComponent<RectTransform>().sizeDelta;
						float num3 = sizeDelta2.y + 150f;
						transform2.GetComponent<LayoutElement>().minHeight = num3;
						transform3.GetComponent<RectTransform>().offsetMin = new Vector2(0f, 0f);
						transform3.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);
						sizeDelta = transform3.GetComponent<RectTransform>().sizeDelta;
						transform3.Find("Board").GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
						sizeDelta.y = num3;
						transform3.GetComponent<RectTransform>().sizeDelta = sizeDelta;
						transform3.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
						transform2.Find("Title").GetComponent<Text>().text = "Section: " + section.Title + " Set: " + set.SetFullName + " Level: " + num2.ToString() + " LevelID: " + num.ToString();
						transform2.Find("Words").GetComponent<Text>().text = ReturnAllWords(level.words);
						transform3.SetAsFirstSibling();
						num++;
					}
				}
			}
		}
	}

	private string ReturnAllWords(List<Word> list)
	{
		string text = string.Empty;
		foreach (Word item in list)
		{
			text = text + item.word + ", ";
		}
		return text;
	}

	public void NextButton()
	{
		startIndex += 20;
		endIndex += 20;
		CreateGames();
	}

	public void BackButton()
	{
		startIndex -= 20;
		endIndex -= 20;
		if (startIndex < 1)
		{
			startIndex = 1;
		}
		if (endIndex < 20)
		{
			endIndex = 20;
		}
		CreateGames();
	}

	public void QuitButton()
	{
		SceneManager.LoadScene("Menu");
	}
}
