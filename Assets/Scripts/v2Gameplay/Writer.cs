using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace v2Gameplay
{
	public class Writer : MonoBehaviour
	{
		public delegate void SendLetter(Letter l);

		public delegate void SendCouple(Letter last, Letter older);

		public static Writer instance;

		public SendLetter onLetterAdd;

		public SendCouple onLetterRemoved;

		public SendLetter onReset;

		public Text wordText;

		public Transform wordContainer;

		public RectTransform background;

		public RectTransform wordContainerRect;

		public GameObject letterPrefab;

		private Level game;

		private string word;

		private List<Letter> letters;

		private void Awake()
		{
			instance = this;
		}

		private void Start()
		{
			TouchController touchController = TouchController.instance;
			touchController.onUnTouch = (TouchController.TouchAction)Delegate.Combine(touchController.onUnTouch, new TouchController.TouchAction(unTouch));
			GameController gameController = GameController.instance;
			gameController.onNewGame = (GameController.SendGame)Delegate.Combine(gameController.onNewGame, new GameController.SendGame(handleNewGame));
		}

		public void letterTouch(Letter l, bool narrow)
		{
			int count = letters.Count;
			if (count == 0)
			{
				addLetter(l);
			}
			else if (count == 1)
			{
				addLetter(l);
			}
			else
			{
				if (count <= 1)
				{
					return;
				}
				if (letters[letters.Count - 2] == l)
				{
					if (narrow)
					{
						removeLetter(letters[letters.Count - 1]);
					}
				}
				else
				{
					addLetter(l);
				}
			}
		}

		private void updateText()
		{
		}

		private void handleNewGame(Level g)
		{
			game = g;
			word = string.Empty;
			letters = new List<Letter>();
			updateText();
		}

		private void unTouch(Vector3 touch)
		{
			if (!(word == string.Empty) && game != null)
			{
				checkWord();
				removeAllLetters();
				SoundManager.instance.ResetSelectIndex();
			}
		}

		private void checkWord()
		{
			WordController.instance.checkWord(word);
		}

		private void removeAllLetters()
		{
			foreach (Letter letter in letters)
			{
				letter.used = false;
				letter.selected(s: false);
			}
			letters.Clear();
			word = string.Empty;
			updateText();
			if (onReset != null)
			{
				onReset(null);
			}
		}

		private void addLetter(Letter l)
		{
			if (!l.used)
			{
				l.used = true;
				letters.Add(l);
				l.selected(s: true);
				word = getWord();
				updateText();
				SoundManager.instance.SelectLetter();
				if (onLetterAdd != null)
				{
					onLetterAdd(l);
				}
			}
		}

		private void removeLetter(Letter l)
		{
			l.used = false;
			letters.Remove(l);
			l.selected(s: false);
			word = getWord();
			updateText();
			SoundManager.instance.DeselectLetter();
			if (onLetterRemoved != null)
			{
				onLetterRemoved(l, letters[letters.Count - 1]);
			}
		}

		private string getWord()
		{
			string text = string.Empty;
			foreach (Letter letter in letters)
			{
				text += letter.l;
			}
			return text;
		}

		private bool doesContain(Letter l)
		{
			foreach (Letter letter in letters)
			{
				if (letter == l)
				{
					return true;
				}
			}
			return false;
		}
	}
}
