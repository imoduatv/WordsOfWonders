using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace v2Gameplay
{
	public class LineController : MonoBehaviour
	{
		public Transform lineHolder;

		public GameObject linePrefab;

		private List<GameObject> lines;

		private bool limitReached;

		private Level game;

		private void Start()
		{
			lines = new List<GameObject>();
			GameController instance = GameController.instance;
			instance.onNewGame = (GameController.SendGame)Delegate.Combine(instance.onNewGame, new GameController.SendGame(handleNewGame));
			Writer instance2 = Writer.instance;
			instance2.onLetterAdd = (Writer.SendLetter)Delegate.Combine(instance2.onLetterAdd, new Writer.SendLetter(handleAddLetter));
			Writer instance3 = Writer.instance;
			instance3.onLetterRemoved = (Writer.SendCouple)Delegate.Combine(instance3.onLetterRemoved, new Writer.SendCouple(handleRemoveLetter));
			Writer instance4 = Writer.instance;
			instance4.onReset = (Writer.SendLetter)Delegate.Combine(instance4.onReset, new Writer.SendLetter(handleClear));
		}

		private void handleNewGame(Level g)
		{
			game = g;
		}

		private void handleAddLetter(Letter letter)
		{
			addLine(letter);
		}

		private void handleRemoveLetter(Letter letter, Letter older)
		{
			removeLine(letter, older);
		}

		private void handleClear(Letter letter)
		{
			clear();
		}

		private void addLine(Letter letter)
		{
			checkLimit();
			if (lines.Count > 0)
			{
				Line component = lines[lines.Count - 1].GetComponent<Line>();
				if (component != null)
				{
					component.disable();
					component.calculateTouch(letter.transform.position);
				}
			}
			GameObject gameObject = UnityEngine.Object.Instantiate(linePrefab);
			gameObject.transform.SetParent(lineHolder);
			gameObject.transform.position = letter.transform.position;
			gameObject.transform.localScale = Vector3.one;
			try
			{
				gameObject.GetComponent<Image>().color = GameController.SelectedLetterBGColor;
				gameObject.transform.GetChild(0).GetComponent<Image>().color = Color.Lerp(GameController.SelectedLetterBGColor, Color.black, 0.2f);
				gameObject.transform.GetChild(1).GetComponent<Image>().color = Color.Lerp(GameController.SelectedLetterBGColor, Color.black, 0.2f);
			}
			catch (Exception)
			{
			}
			if (limitReached)
			{
				gameObject.SetActive(value: false);
			}
			lines.Add(gameObject);
		}

		private void clear()
		{
			foreach (GameObject line in lines)
			{
				UnityEngine.Object.Destroy(line.gameObject);
			}
			lines.Clear();
			limitReached = false;
		}

		private void removeLine(Letter letter, Letter older)
		{
			if (lines.Count >= 2)
			{
				bool flag = lines.Count == game.letters.Length - 1;
				GameObject gameObject = lines[lines.Count - 1];
				GameObject gameObject2 = lines[lines.Count - 2];
				lines.Remove(gameObject.gameObject);
				UnityEngine.Object.Destroy(gameObject);
				lines.Remove(gameObject2);
				UnityEngine.Object.Destroy(gameObject2);
			}
			addLine(older);
		}

		private void checkLimit()
		{
			if (lines.Count == game.letters.Length - 1)
			{
				limitReached = true;
			}
			else
			{
				limitReached = false;
			}
		}
	}
}
