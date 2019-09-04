using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace v2Gameplay
{
	public class WordController : MonoBehaviour
	{
		public delegate void OnWordFound(Word w);

		public delegate void OnExtraFound(string w);

		public delegate void NoParam();

		public static WordController instance;

		public OnWordFound onWordFound;

		public OnWordFound onSameFound;

		public OnExtraFound onExtraFound;

		public NoParam onNotFound;

		public NoParam onExtraFoundAgain;

		private Level game;

		private Coroutine stroke;

		private void Awake()
		{
			instance = this;
		}

		private void Start()
		{
			GameController gameController = GameController.instance;
			gameController.onNewGame = (GameController.SendGame)Delegate.Combine(gameController.onNewGame, new GameController.SendGame(setGame));
		}

		private IEnumerator checkStroke()
		{
			int count = 0;
			Word last = null;
			foreach (Word word in game.words)
			{
				if (!word.found)
				{
					count++;
					last = word;
				}
			}
			if (count == 1)
			{
				yield return new WaitForSeconds(2f);
				for (int i = 0; i < last.cells.Length; i++)
				{
					Cell @object = last.cells[i];
					Movements.instance.executeWithDelay((Movements.Execute)@object.enableStroke, (float)i * 0.1f);
				}
			}
		}

		public void setGame(Level g)
		{
			g.foundExtras = new List<string>();
			if (game != null && game.foundExtras != null)
			{
				foreach (string foundExtra in game.foundExtras)
				{
					if (!g.foundExtras.Contains(foundExtra))
					{
						g.foundExtras.Add(foundExtra);
					}
				}
			}
			game = g;
			foreach (string foundExtra2 in game.foundExtras)
			{
			}
		}

		public void checkWord(string word)
		{
			UnityEngine.Debug.Log(word);
			string empty = string.Empty;
			if (UISwapper.flipGame)
			{
				empty = word.Reverse();
			}
			for (int i = 0; i < game.otherWords.Length; i++)
			{
				if (game.otherWords[i] == word)
				{
					if (game.foundExtras.Contains(word))
					{
						extraFoundAgain(word);
					}
					else
					{
						extraFound(word);
					}
					return;
				}
			}
			if (PlayerPrefsManager.GetLang() == SystemLanguage.Hebrew.ToString())
			{
			}
			foreach (Word word2 in game.words)
			{
				if (word2.orientation == Orientation.VERTICAL)
				{
					if (word2.word == word)
					{
						if (word2.found)
						{
							sameFound(word2);
						}
						else
						{
							wordFound(word2);
						}
						return;
					}
				}
				else if (word2.word == word)
				{
					if (word2.found)
					{
						sameFound(word2);
					}
					else
					{
						wordFound(word2);
					}
					return;
				}
			}
			notFound(word);
		}

		private void wordFound(Word word)
		{
			word.setFound();
			foreach (Word word2 in game.words)
			{
				word2.checkFound();
			}
			if (stroke != null)
			{
				StopCoroutine(stroke);
			}
			stroke = StartCoroutine(checkStroke());
			SoundManager.instance.WordFound();
			if (onWordFound != null)
			{
				onWordFound(word);
			}
		}

		private void sameFound(Word word)
		{
			if (onSameFound != null)
			{
				onSameFound(word);
			}
			SoundManager.instance.WordSame();
		}

		private void extraFound(string word)
		{
			if (onExtraFound != null)
			{
				onExtraFound(word);
			}
			if (!game.foundExtras.Contains(word))
			{
				game.foundExtras.Add(word);
			}
			SoundManager.instance.WordExtra();
		}

		private void extraFoundAgain(string word)
		{
			if (onExtraFoundAgain != null)
			{
				onExtraFoundAgain();
			}
			SoundManager.instance.WordSame();
		}

		private void notFound(string word)
		{
			if (onNotFound != null)
			{
				onNotFound();
			}
			SoundManager.instance.WordInvalid();
		}
	}
}
