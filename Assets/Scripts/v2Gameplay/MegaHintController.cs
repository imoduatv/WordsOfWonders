using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace v2Gameplay
{
	public class MegaHintController : MonoBehaviour
	{
		public static MegaHintController instance;

		public static bool waiting;

		public static int firstLevel = 15;

		public GameObject hintButton;

		public Image border;

		public Image logo;

		private const float animTime = 0.5f;

		private GameObject[] letters;

		private Dictionary<Letter, List<Cell>> cells;

		private Level game;

		private float waitTime;

		private void Awake()
		{
			instance = this;
		}

		private void Start()
		{
			border.color = GameController.WheelStrokeColor;
			GameController gameController = GameController.instance;
			gameController.onNewGame = (GameController.SendGame)Delegate.Combine(gameController.onNewGame, new GameController.SendGame(handleNewGame));
			WheelController wheelController = WheelController.instance;
			wheelController.onWheelCreated = (WheelController.NoParam)Delegate.Combine(wheelController.onWheelCreated, new WheelController.NoParam(handleWheelCreated));
		}

		private void handleNewGame(Level g)
		{
			game = g;
			letters = WheelController.instance.getLetters();
		}

		private void handleWheelCreated()
		{
			Movements.instance.executeWithDelay(init);
		}

		private void init()
		{
			letters = WheelController.instance.getLetters();
			GameObject[] array = letters;
			foreach (GameObject gameObject in array)
			{
				gameObject.GetComponent<Letter>().calculateAngle();
			}
			waiting = false;
			if (getMegaHint())
			{
				Movements.instance.executeWithDelay(openLetterSelect);
			}
		}

		public void letterClicked(Letter l)
		{
			if (cells[l].Count != 0)
			{
				TouchController.instance.disable();
				StartCoroutine(sendHints(l));
				Movements.instance.executeWithDelay((Movements.Execute)closeLetterSelect, waitTime);
			}
		}

		private IEnumerator sendHints(Letter l)
		{
			waitTime = (float)cells[l].Count * 0.2f;
			foreach (Cell item in cells[l])
			{
				UnityEngine.Debug.Log("cell");
				BoardController.instance.getHint(item, l);
				yield return new WaitForSeconds(0.15f);
			}
		}

		public void openLetterSelect()
		{
			logo.color = GameController.WheelStrokeColor;
			cells = new Dictionary<Letter, List<Cell>>();
			GameObject[] array = letters;
			foreach (GameObject gameObject in array)
			{
				Letter component = gameObject.GetComponent<Letter>();
				component.calculateAngle();
				cells.Add(component, BoardController.instance.getCellsWith(component));
			}
			bool flag = false;
			GameObject[] array2 = letters;
			foreach (GameObject gameObject2 in array2)
			{
				if (cells[gameObject2.GetComponent<Letter>()].Count != 0)
				{
					flag = true;
				}
			}
			if (!flag)
			{
				GameMenuController.megaHintOpening = false;
				return;
			}
			waiting = true;
			SoundManager.instance.playMegaHint();
			TouchController.instance.disable();
			FadeController.instance.fadeIn();
			ObjectHolder.instance.wheel.transform.SetAsLastSibling();
			ObjectHolder.instance.board.transform.SetAsLastSibling();
			StartCoroutine(open());
			setMegaHint(enabled: true);
			GameMenuController.megaHintOpening = true;
		}

		private IEnumerator open()
		{
			float t = 0f;
			while (t <= 0.5f)
			{
				float par = t / 0.5f;
				border.fillAmount = par;
				logo.transform.localScale = Vector3.one * par;
				float angle = par * 360f;
				GameObject[] array = letters;
				foreach (GameObject gameObject in array)
				{
					Letter component = gameObject.GetComponent<Letter>();
					if (component.angle < angle && cells[component].Count != 0)
					{
						component.openMegaHint();
					}
				}
				t += Time.deltaTime;
				yield return null;
			}
			border.fillAmount = 1f;
			TouchController.instance.enable();
			logo.transform.localScale = Vector3.one;
		}

		public void closeLetterSelect()
		{
			TouchController.instance.disable();
			FadeController.instance.fadeOut();
			waiting = false;
			StartCoroutine(close());
			setMegaHint(enabled: false);
		}

		private IEnumerator close()
		{
			float t = 0f;
			while (t <= 0.5f)
			{
				float par2 = t / 0.5f;
				par2 = 1f - par2;
				border.fillAmount = par2;
				logo.transform.localScale = Vector3.one * par2;
				float angle = par2 * 360f;
				GameObject[] array = letters;
				foreach (GameObject gameObject in array)
				{
					Letter component = gameObject.GetComponent<Letter>();
					if (component.angle >= angle)
					{
						component.closeMegaHint();
					}
				}
				t += Time.deltaTime;
				yield return null;
			}
			border.fillAmount = 0f;
			GameObject[] array2 = letters;
			foreach (GameObject gameObject2 in array2)
			{
				Letter component2 = gameObject2.GetComponent<Letter>();
				component2.closeMegaHint();
			}
			logo.transform.localScale = Vector3.zero;
			TouchController.instance.enable();
		}

		public static int GetHintPrice()
		{
			DateTime t = DateTime.Parse(PlayerPrefsManager.GetHintDiscountTime());
			if (DateTime.Compare(t, DateTime.Now) > 0)
			{
				return 175;
			}
			return 350;
		}

		public void checkMegaHint()
		{
			if (PlayerPrefsManager.GetLevel() >= firstLevel)
			{
				if (PlayerPrefsManager.MegaHintShown())
				{
					hintButton.transform.localScale = Vector3.one;
					return;
				}
				TouchController.instance.disable();
				Movements.instance.scale(hintButton, Vector3.zero, Vector3.one * 1.1f, 0.3f, 1f);
				Movements.instance.scale(hintButton, Vector3.one * 1.1f, Vector3.one, 0.1f, 1.3f);
				Movements.instance.executeWithDelay((Movements.Execute)TutorialController.instance.openMega, 1.8f);
			}
		}

		public bool getMegaHint()
		{
			string key = "mega_hint" + game.mode + game.setId + game.id;
			return PlayerPrefs.GetInt(key, 0) == 1;
		}

		public void setMegaHint(bool enabled)
		{
			int value = 0;
			if (enabled)
			{
				value = 1;
			}
			string key = "mega_hint" + game.mode + game.setId + game.id;
			PlayerPrefs.SetInt(key, value);
		}
	}
}
