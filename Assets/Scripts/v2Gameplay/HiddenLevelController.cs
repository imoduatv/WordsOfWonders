using System;
using UnityEngine;
using UnityEngine.UI;

namespace v2Gameplay
{
	public class HiddenLevelController : MonoBehaviour
	{
		public static HiddenLevelController instance;

		public Transform starHolder;

		public GameObject starPrefab;

		private static Color disabledColor = FugoUtils.Color(222, 222, 222);

		private static Color enabledColor = FugoUtils.Color(254, 216, 53);

		private Level game;

		private GameObject[] stars;

		private void Awake()
		{
			instance = this;
		}

		private void Start()
		{
			GameController gameController = GameController.instance;
			gameController.onNewGame = (GameController.SendGame)Delegate.Combine(gameController.onNewGame, new GameController.SendGame(handleNewGame));
			GameController gameController2 = GameController.instance;
			gameController2.onTournamentEnd = (GameController.SendGame)Delegate.Combine(gameController2.onTournamentEnd, new GameController.SendGame(handleAdventureEnd));
		}

		public void handleNewGame(Level g)
		{
			if (g.mode != GameMode.ADVENTURE)
			{
				GameController gameController = GameController.instance;
				gameController.onNewGame = (GameController.SendGame)Delegate.Remove(gameController.onNewGame, new GameController.SendGame(handleNewGame));
				GameController gameController2 = GameController.instance;
				gameController2.onTournamentEnd = (GameController.SendGame)Delegate.Remove(gameController2.onTournamentEnd, new GameController.SendGame(handleAdventureEnd));
				UnityEngine.Object.Destroy(starHolder.gameObject);
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			if (game == null)
			{
				game = g;
				disabledColor = Color.white;
				initStars();
			}
			game = g;
			updateStars();
		}

		public void handleAdventureEnd(Level g)
		{
			maxStars();
			try
			{
				endAnim();
			}
			catch (Exception)
			{
			}
		}

		private void initStars()
		{
			stars = new GameObject[game.adventure.Levels.Length];
			for (int i = 0; i < stars.Length; i++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(starPrefab);
				stars[i] = gameObject;
				gameObject.transform.SetParent(starHolder);
				gameObject.transform.ResetTransform();
				gameObject.transform.GetChild(0).GetComponent<Image>().color = disabledColor;
			}
		}

		private void updateStars()
		{
			for (int i = 0; i < game.adventure.curPos; i++)
			{
				stars[i].transform.GetChild(0).GetComponent<Image>().color = enabledColor;
			}
		}

		private void maxStars()
		{
			for (int i = 0; i < stars.Length; i++)
			{
				stars[i].transform.GetChild(0).GetComponent<Image>().color = enabledColor;
			}
		}

		private void startAnim()
		{
		}

		public void endAnim()
		{
			if (game != null)
			{
				for (int i = 0; i < stars.Length; i++)
				{
					GameObject gameObject = stars[i].gameObject;
					Movements.instance.move(gameObject, gameObject.transform.position, gameObject.transform.position + Vector3.up * 2f, 0.5f + (float)i * 0.3f, 0.5f);
				}
			}
		}
	}
}
