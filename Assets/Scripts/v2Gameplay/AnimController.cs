using System;
using UnityEngine;

namespace v2Gameplay
{
	public class AnimController : MonoBehaviour
	{
		public static AnimController instance;

		public static float waitGemTime;

		private void Awake()
		{
			instance = this;
		}

		private void Start()
		{
			GameController gameController = GameController.instance;
			gameController.onGameEnd = (GameController.SendGame)Delegate.Combine(gameController.onGameEnd, new GameController.SendGame(handleGameEnd));
			GameController gameController2 = GameController.instance;
			gameController2.onTournamentEnd = (GameController.SendGame)Delegate.Combine(gameController2.onTournamentEnd, new GameController.SendGame(handleTournementEnd));
		}

		private void handleGameEnd(Level game)
		{
			if (game.mode == GameMode.NORMAL)
			{
				endGame();
				UnityEngine.Debug.Log("END GAME");
			}
			else if (game.mode == GameMode.DAILY)
			{
				endDaily();
			}
			ObjectHolder.instance.endGame.transform.SetAsLastSibling();
		}

		private void handleTournementEnd(Level game)
		{
			endTournament(game);
		}

		public void initGameAnims()
		{
			showButtons();
			expandWheel();
			Vector2 left = Vector2.left;
		}

		public void endGameAnims()
		{
			hideButtons(GameController.endType == EndType.Level);
			Movements.instance.executeWithDelay((Movements.Execute)shrinkWheel, 0.25f);
		}

		public void endGame()
		{
			ObjectHolder.instance.endGame.transform.SetAsLastSibling();
			Movements.instance.executeWithDelay((Movements.Execute)endGameAnims, 0.6f);
			Movements.instance.executeWithDelay((Movements.ExecuteFloat)BoardController.animations.fadeBoard, 0.8f);
			Movements.instance.executeWithDelay((Movements.Execute)BoardController.animations.dropBoard, 0.8f);
			Movements.instance.executeWithDelay((Movements.Execute)popEndItems, 2f);
		}

		public void endDaily()
		{
			Movements.instance.executeWithDelay((Movements.Execute)endGameAnims, 1f);
			Movements.instance.executeWithDelay((Movements.ExecuteFloat)BoardController.animations.fadeBoard, 0.8f);
			MenuController.fromDaily = true;
			Movements.instance.executeWithDelay((Movements.Execute)GameMenuController.instance.openMenu, 2f);
		}

		public void popEndItems()
		{
			if (GameController.game.mode == GameMode.ADVENTURE)
			{
				Movements.instance.executeWithDelay((Movements.Execute)EndGameController.instance.playAnim, 2f);
			}
			else
			{
				EndGameController.instance.playAnim();
			}
		}

		public void endTournament(Level game)
		{
			Movements.instance.executeWithDelay((Movements.Execute)endGameAnims, 1f);
			BoardController.animations.fadeBoard(game);
		}

		public void showButtons()
		{
			float d = 1f;
			float animTime = 0.2f;
			ObjectHolder.instance.shuffleButton.transform.position += Vector3.left * d;
			ObjectHolder.instance.backButton.transform.position += Vector3.left * d;
			ObjectHolder.instance.sectionsButton.transform.position += Vector3.right * d;
			ObjectHolder.instance.shopButton.transform.position += Vector3.right * d;
			ObjectHolder.instance.hintButton.transform.position += Vector3.right * d;
			ObjectHolder.instance.shuffleButton.SetActive(value: true);
			ObjectHolder.instance.backButton.SetActive(value: true);
			if (PlayerPrefsManager.GetFirstExtra())
			{
				ObjectHolder.instance.starButton.transform.position += Vector3.left * d;
				ObjectHolder.instance.starButton.SetActive(value: true);
				Movements.instance.move(ObjectHolder.instance.starButton, ObjectHolder.instance.starButton.transform.position, ObjectHolder.instance.starButton.transform.position + Vector3.right * d, animTime);
			}
			ObjectHolder.instance.sectionsButton.SetActive(value: true);
			ObjectHolder.instance.shopButton.SetActive(value: true);
			ObjectHolder.instance.hintButton.SetActive(value: true);
			Movements.instance.startFadeIn(ObjectHolder.instance.name, 0.5f, 1f);
			Movements.instance.move(ObjectHolder.instance.starHolder, ObjectHolder.instance.starHolder.transform.position, ObjectHolder.instance.starHolder.transform.position + Vector3.down * 2f, 0.5f);
			Movements.instance.move(ObjectHolder.instance.shuffleButton, ObjectHolder.instance.shuffleButton.transform.position, ObjectHolder.instance.shuffleButton.transform.position + Vector3.right * d, animTime);
			Movements.instance.move(ObjectHolder.instance.backButton, ObjectHolder.instance.backButton.transform.position, ObjectHolder.instance.backButton.transform.position + Vector3.right * d, animTime);
			Movements.instance.move(ObjectHolder.instance.sectionsButton, ObjectHolder.instance.sectionsButton.transform.position, ObjectHolder.instance.sectionsButton.transform.position + Vector3.left * d, animTime);
			Movements.instance.move(ObjectHolder.instance.hintButton, ObjectHolder.instance.hintButton.transform.position, ObjectHolder.instance.hintButton.transform.position + Vector3.left * d, animTime);
			if (PlayerPrefsManager.GetLevel() != 1)
			{
				Movements.instance.move(ObjectHolder.instance.shopButton, ObjectHolder.instance.shopButton.transform.position, ObjectHolder.instance.shopButton.transform.position + Vector3.left * d, animTime);
			}
		}

		public void hideButtons()
		{
			hideButtons(shop: true);
		}

		public void hideButtons(bool shop)
		{
			float d = 1.3f;
			float animTime = 0.2f;
			Movements.instance.move(ObjectHolder.instance.starHolder, ObjectHolder.instance.starHolder.transform.position, ObjectHolder.instance.starHolder.transform.position + Vector3.up * 2f, animTime);
			Movements.instance.move(ObjectHolder.instance.shuffleButton, ObjectHolder.instance.shuffleButton.transform.position, ObjectHolder.instance.shuffleButton.transform.position + Vector3.left * d, animTime);
			Movements.instance.move(ObjectHolder.instance.backButton, ObjectHolder.instance.backButton.transform.position, ObjectHolder.instance.backButton.transform.position + Vector3.left * d, animTime);
			Movements.instance.move(ObjectHolder.instance.starButton, ObjectHolder.instance.starButton.transform.position, ObjectHolder.instance.starButton.transform.position + Vector3.left * d, animTime, waitGemTime);
			Movements.instance.move(ObjectHolder.instance.sectionsButton, ObjectHolder.instance.sectionsButton.transform.position, ObjectHolder.instance.sectionsButton.transform.position + Vector3.right * d, animTime);
			if (shop)
			{
				Movements.instance.move(ObjectHolder.instance.shopButton, ObjectHolder.instance.shopButton.transform.position, ObjectHolder.instance.shopButton.transform.position + Vector3.right * d, animTime, waitGemTime);
			}
			Movements.instance.move(ObjectHolder.instance.hintButton, ObjectHolder.instance.hintButton.transform.position, ObjectHolder.instance.hintButton.transform.position + Vector3.right * d, animTime);
			Movements.instance.startFadeIn(ObjectHolder.instance.name, 0.5f, 0f);
			waitGemTime = 0f;
		}

		public void expandWheel()
		{
			Movements.instance.scale(ObjectHolder.instance.wheel, 1.2f, 0.3f);
			Movements.instance.scale(ObjectHolder.instance.wheel, 1.2f * Vector3.one, Vector3.one, 0.1f, 0.3f);
		}

		public void shrinkWheel()
		{
			Movements.instance.scale(ObjectHolder.instance.wheel, 0f, 0.3f);
		}
	}
}
