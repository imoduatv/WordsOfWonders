using System;
using UnityEngine;
using UnityEngine.UI;

namespace v2Gameplay
{
	public class DailyController : MonoBehaviour
	{
		public delegate void NoParam();

		public static DailyController instance;

		public NoParam onStarFound;

		public Text starCount;

		public GameObject star;

		public GameObject starHolder;

		private Level game;

		private int count;

		private void Awake()
		{
			instance = this;
		}

		private void Start()
		{
			GameController gameController = GameController.instance;
			gameController.onNewGame = (GameController.SendGame)Delegate.Combine(gameController.onNewGame, new GameController.SendGame(handleNewGame));
			GameController gameController2 = GameController.instance;
			gameController2.onGameEnd = (GameController.SendGame)Delegate.Combine(gameController2.onGameEnd, new GameController.SendGame(handleEndGame));
		}

		private void updateCount()
		{
			starCount.text = count.ToString();
		}

		private void fadeIn()
		{
			starHolder.transform.position += Vector3.up * 2f;
			starHolder.SetActive(value: true);
		}

		private void handleNewGame(Level g)
		{
			game = g;
			if (game.mode != GameMode.DAILY)
			{
				GameController gameController = GameController.instance;
				gameController.onNewGame = (GameController.SendGame)Delegate.Remove(gameController.onNewGame, new GameController.SendGame(handleNewGame));
				GameController gameController2 = GameController.instance;
				gameController2.onGameEnd = (GameController.SendGame)Delegate.Remove(gameController2.onGameEnd, new GameController.SendGame(handleEndGame));
				UnityEngine.Object.Destroy(base.gameObject);
			}
			else
			{
				count = PlayerPrefsManager.getDailyStar();
				updateCount();
				fadeIn();
			}
		}

		private void handleEndGame(Level g)
		{
			GameController.daily = false;
			GameController.levelToOpen = -1;
			PlayerPrefsManager.SetEarnedStar(PlayerPrefsManager.getDailyStar());
			PlayerPrefsManager.deleteStarPos();
			PlayerPrefsManager.deleteDailyStar();
		}

		public void starFound(GameObject s)
		{
			s.transform.GetChild(0).gameObject.SetActive(value: true);
			count++;
			PlayerPrefsManager.setDailyStar(count);
			updateCount();
			Movements.instance.move(s, s.transform.position, star.transform.position, 0.5f);
			UnityEngine.Object.Destroy(s, 0.55f);
			if (onStarFound != null)
			{
				onStarFound();
			}
		}
	}
}
