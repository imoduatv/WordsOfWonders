using System;
using System.Collections.Generic;
using UnityEngine;

namespace v2Gameplay
{
	public class SaveLoad : MonoBehaviour
	{
		public static SaveLoad instance;

		private List<Word> foundWords;

		private Level game;

		private void Awake()
		{
			instance = this;
		}

		private void Start()
		{
			WordController wordController = WordController.instance;
			wordController.onWordFound = (WordController.OnWordFound)Delegate.Combine(wordController.onWordFound, new WordController.OnWordFound(handleWordFound));
			GameController gameController = GameController.instance;
			gameController.onNewGame = (GameController.SendGame)Delegate.Combine(gameController.onNewGame, new GameController.SendGame(handleNewGame));
			GameController gameController2 = GameController.instance;
			gameController2.onGameEnd = (GameController.SendGame)Delegate.Combine(gameController2.onGameEnd, new GameController.SendGame(handleGameEnd));
			GameController gameController3 = GameController.instance;
			gameController3.onTournamentEnd = (GameController.SendGame)Delegate.Combine(gameController3.onTournamentEnd, new GameController.SendGame(handleAdventureEnd));
			GameController gameController4 = GameController.instance;
			gameController4.onNextLevel = (GameController.SendGame)Delegate.Combine(gameController4.onNextLevel, new GameController.SendGame(handleNextLevel));
		}

		private void handleNewGame(Level g)
		{
			game = g;
			foundWords = new List<Word>();
		}

		private void handleGameEnd(Level g)
		{
			if (ExtraWordController.instance.rewardable())
			{
				endGameClear();
				Movements.instance.executeWithDelay(endGameClear);
			}
		}

		private void endGameClear()
		{
			ClearHints(game);
			clearSavedWords(game);
			clearExtraWords(game);
		}

		private void handleNextLevel(Level g)
		{
			clearSavedWords(g);
			ClearHints(g);
		}

		private void handleAdventureEnd(Level g)
		{
			ClearHints(g);
			clearSavedWords(g);
			clearExtraWords(g);
		}

		private void handleWordFound(Word word)
		{
			if (ExtraWordController.instance.rewardable())
			{
				foundWords.Add(word);
				if (ExtraWordController.instance.rewardable())
				{
					save();
				}
			}
		}

		public void loadGame()
		{
			if (ExtraWordController.instance.rewardable())
			{
				List<string> list = GetFoundWords(game);
				foreach (string item in list)
				{
					foreach (Word word in game.words)
					{
						if (item == word.word)
						{
							word.setFound();
							word.enableWord();
							foundWords.Add(word);
						}
					}
				}
			}
		}

		private void save()
		{
			SetFoundWords(foundWords, game);
		}

		public void clearSavedWords(Level g)
		{
			foundWords = new List<Word>();
			SetFoundWords(foundWords, g);
		}

		public void clearExtraWords(Level g)
		{
			GameMode mode = g.mode;
			ResetExtraWordCount(g);
			game.foundExtras = new List<string>();
			SetFoundExtras(game.foundExtras, g);
		}

		public static void SetExtraWordCount(int value, Level g)
		{
			GameMode mode = g.mode;
			string text = "extra_word";
			if (mode == GameMode.NORMAL)
			{
				text = "extra_word";
			}
			if (mode == GameMode.DAILY)
			{
				text = "extra_word_daily";
			}
			if (mode == GameMode.ADVENTURE)
			{
				text = "extra_word_adventure";
			}
			if (g.mode == GameMode.ADVENTURE)
			{
				text += g.setId.ToString();
			}
			UnityEngine.Debug.Log("extra count key  " + text);
			PlayerPrefs.SetInt(text, value);
		}

		public static int GetExtraWordCount(Level g)
		{
			GameMode mode = g.mode;
			string text = "extra_word";
			if (mode == GameMode.NORMAL)
			{
				text = "extra_word";
			}
			if (mode == GameMode.DAILY)
			{
				text = "extra_word_daily";
			}
			if (mode == GameMode.ADVENTURE)
			{
				text = "extra_word_adventure";
			}
			if (g.mode == GameMode.ADVENTURE)
			{
				text += g.setId.ToString();
			}
			return PlayerPrefs.GetInt(text, 0);
		}

		public static bool IncreaseExtraWordCount(Level g)
		{
			GameMode mode = g.mode;
			int extraWordCount = GetExtraWordCount(g);
			extraWordCount++;
			SetExtraWordCount(extraWordCount, g);
			return false;
		}

		public static void ResetExtraWordCount(Level g)
		{
			SetExtraWordCount(0, g);
		}

		public static void SetFoundWords(List<Word> words, Level g)
		{
			GameMode mode = g.mode;
			string text = string.Empty;
			string text2 = "found_words";
			if (mode == GameMode.NORMAL)
			{
				text2 = "found_words";
			}
			if (mode == GameMode.DAILY)
			{
				text2 = "found_words_daily";
			}
			if (mode == GameMode.ADVENTURE)
			{
				text2 = "found_words_adventure";
			}
			if (g.mode == GameMode.ADVENTURE)
			{
				text2 += g.setId.ToString();
			}
			if (words == null)
			{
				PlayerPrefs.DeleteKey(text2);
				return;
			}
			foreach (Word word in words)
			{
				text = text + word.word + "|";
			}
			PlayerPrefs.SetString(text2, text);
		}

		public static List<string> GetFoundWords(Level g)
		{
			GameMode mode = g.mode;
			string text = "found_words";
			if (mode == GameMode.NORMAL)
			{
				text = "found_words";
			}
			if (mode == GameMode.DAILY)
			{
				text = "found_words_daily";
			}
			if (mode == GameMode.ADVENTURE)
			{
				text = "found_words_adventure";
			}
			if (g.mode == GameMode.ADVENTURE)
			{
				text += g.setId.ToString();
			}
			UnityEngine.Debug.Log(text);
			List<string> list = new List<string>();
			string @string = PlayerPrefs.GetString(text, string.Empty);
			string[] array = @string.Split('|');
			string[] array2 = array;
			foreach (string item in array2)
			{
				list.Add(item);
			}
			return list;
		}

		public static void SetFoundExtras(List<string> words, Level g)
		{
			GameMode mode = g.mode;
			string text = string.Empty;
			string text2 = "found_extra";
			if (mode == GameMode.NORMAL)
			{
				text2 = "found_extra";
			}
			if (mode == GameMode.DAILY)
			{
				text2 = "found_extra_daily";
			}
			if (mode == GameMode.ADVENTURE)
			{
				text2 = "found_extra_adventure";
			}
			if (g.mode == GameMode.ADVENTURE)
			{
				text2 += g.setId.ToString();
			}
			foreach (string word in words)
			{
				text = text + word + "|";
			}
			PlayerPrefs.SetString(text2, text);
		}

		public static List<string> GetFoundExtras(Level g)
		{
			GameMode mode = g.mode;
			string text = "found_extra";
			if (mode == GameMode.NORMAL)
			{
				text = "found_extra";
			}
			if (mode == GameMode.DAILY)
			{
				text = "found_extra_daily";
			}
			if (mode == GameMode.ADVENTURE)
			{
				text = "found_extra_adventure";
			}
			if (g.mode == GameMode.ADVENTURE)
			{
				text += g.setId.ToString();
			}
			List<string> list = new List<string>();
			string @string = PlayerPrefs.GetString(text, string.Empty);
			string[] array = @string.Split('|');
			string[] array2 = array;
			foreach (string item in array2)
			{
				list.Add(item);
			}
			return list;
		}

		public static void AddHint(int x, int y, Level g)
		{
			if (ExtraWordController.instance.rewardable())
			{
				GameMode mode = g.mode;
				string text = "hints";
				if (mode == GameMode.NORMAL)
				{
					text = "hints";
				}
				if (mode == GameMode.DAILY)
				{
					text = "daily_hints";
				}
				if (mode == GameMode.ADVENTURE)
				{
					text = "adventure_hints";
				}
				if (g.mode == GameMode.ADVENTURE)
				{
					text += g.setId.ToString();
				}
				PlayerPrefs.SetString(text, GetHint(g) + "|" + x + ":" + y);
			}
		}

		private static string GetHint(Level g)
		{
			GameMode mode = g.mode;
			string text = "hints";
			if (mode == GameMode.NORMAL)
			{
				text = "hints";
			}
			if (mode == GameMode.DAILY)
			{
				text = "daily_hints";
			}
			if (mode == GameMode.ADVENTURE)
			{
				text = "adventure_hints";
			}
			if (g.mode == GameMode.ADVENTURE)
			{
				text += g.setId.ToString();
			}
			return PlayerPrefs.GetString(text, string.Empty);
		}

		public static int[,] GetHints(Level g)
		{
			string hint = GetHint(g);
			string[] array = hint.Split('|');
			int[,] array2 = new int[array.Length - 1, 2];
			for (int i = 1; i < array.Length; i++)
			{
				string[] array3 = array[i].Split(':');
				array2[i - 1, 0] = int.Parse(array3[0]);
				array2[i - 1, 1] = int.Parse(array3[1]);
			}
			return array2;
		}

		public static void ClearHints(Level g)
		{
			GameMode mode = g.mode;
			string text = "hints";
			if (mode == GameMode.NORMAL)
			{
				text = "hints";
			}
			if (mode == GameMode.DAILY)
			{
				text = "daily_hints";
			}
			if (mode == GameMode.ADVENTURE)
			{
				text = "adventure_hints";
			}
			if (g.mode == GameMode.ADVENTURE)
			{
				text += g.setId.ToString();
			}
			PlayerPrefs.DeleteKey(text);
		}

		private void migratePreferences()
		{
		}

		private void movePreferences(string A, string B)
		{
			string @string = PlayerPrefs.GetString(A, string.Empty);
			if (@string != string.Empty)
			{
				PlayerPrefs.SetString(B, @string);
			}
		}
	}
}
