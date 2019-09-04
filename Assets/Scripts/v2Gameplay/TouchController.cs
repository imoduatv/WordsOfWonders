using System;
using System.Collections;
using UnityEngine;

namespace v2Gameplay
{
	public class TouchController : MonoBehaviour
	{
		public delegate void TouchAction(Vector3 t);

		public static TouchController instance;

		public TouchAction onCalculate;

		public TouchAction onUnTouch;

		private bool active;

		private void Awake()
		{
			instance = this;
			active = true;
		}

		private void Start()
		{
			GameController gameController = GameController.instance;
			gameController.onGameEnd = (GameController.SendGame)Delegate.Combine(gameController.onGameEnd, new GameController.SendGame(handleGameEnd));
			GameController gameController2 = GameController.instance;
			gameController2.onNewGame = (GameController.SendGame)Delegate.Combine(gameController2.onNewGame, new GameController.SendGame(handleNewGame));
			GameController gameController3 = GameController.instance;
			gameController3.onTournamentEnd = (GameController.SendGame)Delegate.Combine(gameController3.onTournamentEnd, new GameController.SendGame(handleGameEnd));
			WheelController wheelController = WheelController.instance;
			wheelController.onWheelCreated = (WheelController.NoParam)Delegate.Combine(wheelController.onWheelCreated, new WheelController.NoParam(handleWheelCreated));
			enable();
		}

		public void calculateTouch()
		{
			if (active)
			{
				Vector3 mousePosition = UnityEngine.Input.mousePosition;
				onCalculate(mousePosition);
			}
		}

		public void unTouch()
		{
			onUnTouch(UnityEngine.Input.mousePosition);
		}

		private void testTouch()
		{
		}

		public void enable()
		{
			UnityEngine.Debug.Log("enable touch");
			active = true;
		}

		public void disable()
		{
			UnityEngine.Debug.Log("disable touch");
			StartCoroutine(delayTouch());
			active = false;
		}

		private void handleWheelCreated()
		{
			enable();
		}

		private void handleNewGame(Level g)
		{
			if (g.part != AdventurePart.START && g.part != 0)
			{
				disable();
			}
		}

		private void handleGameEnd(Level g)
		{
			disable();
		}

		private IEnumerator delayTouch()
		{
			yield return null;
			if (onUnTouch != null)
			{
				onUnTouch(UnityEngine.Input.mousePosition);
			}
		}
	}
}
