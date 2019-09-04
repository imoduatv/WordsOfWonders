using System;
using UnityEngine;

namespace v2Gameplay
{
	public class NotificationController : MonoBehaviour
	{
		private Level game;

		private void Start()
		{
			GameController instance = GameController.instance;
			instance.onNewGame = (GameController.SendGame)Delegate.Combine(instance.onNewGame, new GameController.SendGame(handleNewGame));
			GameController instance2 = GameController.instance;
			instance2.onGameEnd = (GameController.SendGame)Delegate.Combine(instance2.onGameEnd, new GameController.SendGame(handleGameEnd));
			GameController instance3 = GameController.instance;
			instance3.onTournamentEnd = (GameController.SendGame)Delegate.Combine(instance3.onTournamentEnd, new GameController.SendGame(handleGameEnd));
			WordController instance4 = WordController.instance;
			instance4.onWordFound = (WordController.OnWordFound)Delegate.Combine(instance4.onWordFound, new WordController.OnWordFound(handleWordFound));
		}

		private void handleWordFound(Word word)
		{
			Word shortestWord = getShortestWord();
			if (shortestWord != null)
			{
				NotificationSystem.CreateHintNotification(word.word);
			}
		}

		private void handleNewGame(Level g)
		{
			game = g;
		}

		private void handleGameEnd(Level g)
		{
			NotificationSystem.DisableHintNotifications();
		}

		private Word getShortestWord()
		{
			for (int i = 0; i < game.words.Count; i++)
			{
				int index = game.words.Count - i - 1;
				if (!game.words[index].found)
				{
					return game.words[index];
				}
			}
			return null;
		}
	}
}
