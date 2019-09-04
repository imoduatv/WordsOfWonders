using System;
using UnityEngine;
using UnityEngine.UI;

namespace v2Gameplay
{
	public class WheelController : MonoBehaviour
	{
		public delegate void NoParam();

		public static WheelController instance;

		public NoParam onWheelCreated;

		public GameObject letterPrefab;

		public Transform letterContainer;

		private const float maxCellRatio = 15f / 46f;

		private const float minCellRatio = 0.238410592f;

		private const float angleTg = -0.0146127269f;

		private const float plus = 0.369925141f;

		private static float range = 140f;

		private static float letterSize = 100f;

		private Level game;

		private Vector3[] positions;

		private GameObject[] letters;

		private void Awake()
		{
			instance = this;
		}

		private void Start()
		{
			GameController gameController = GameController.instance;
			gameController.onNewGame = (GameController.SendGame)Delegate.Combine(gameController.onNewGame, new GameController.SendGame(handleNewGame));
			if (PlayerPrefsManager.GetLevel() != 1)
			{
				destroyCanvas();
			}
		}

		public void destroyCanvas()
		{
			UnityEngine.Object.Destroy(letterContainer.parent.GetComponent<CanvasScaler>());
			UnityEngine.Object.Destroy(letterContainer.parent.GetComponent<GraphicRaycaster>());
			UnityEngine.Object.Destroy(letterContainer.parent.GetComponent<Canvas>());
		}

		public GameObject[] getLetters()
		{
			return letters;
		}

		public void shuffle()
		{
			SoundManager.instance.Shuffle();
			for (int i = 0; i < letters.Length; i++)
			{
				int num = UnityEngine.Random.Range(0, game.letters.Length);
				int num2 = UnityEngine.Random.Range(0, game.letters.Length);
				GameObject gameObject = letters[num];
				letters[num] = letters[num2];
				letters[num2] = gameObject;
			}
			for (int j = 0; j < letters.Length; j++)
			{
				GameObject gameObject2 = letters[j];
				Movements.instance.move(gameObject2, gameObject2.transform.localPosition, positions[j], 0.2f, local: true);
			}
		}

		private void handleNewGame(Level g)
		{
			UnityEngine.Debug.Log("wheel controller handle new game");
			game = g;
			Color wheelColor = GameController.WheelColor;
			letterContainer.parent.GetComponent<Image>().color = wheelColor;
			bool flag = game.part != AdventurePart.START && game.part != AdventurePart.NONE;
			if (flag)
			{
				clearAnim();
			}
			if (!flag)
			{
				createWheel();
			}
			else
			{
				Movements.instance.executeWithDelay((Movements.Execute)createWheel, 1f);
			}
		}

		private void clearWheel()
		{
			for (int i = 0; i < letterContainer.childCount; i++)
			{
				UnityEngine.Object.Destroy(letterContainer.GetChild(i).gameObject);
			}
		}

		private void createWheel()
		{
			float num = (float)Math.PI * 2f / (float)game.letters.Length;
			positions = new Vector3[game.letters.Length];
			letters = new GameObject[game.letters.Length];
			float wheelSize = getWheelSize();
			calculateLetterScale();
			range = (wheelSize - letterSize * 1.2f) * 0.5f;
			for (int i = 0; i < game.letters.Length; i++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(letterPrefab);
				gameObject.GetComponent<RectTransform>().sizeDelta = Vector2.one * letterSize;
				positions[i].x = range * Mathf.Sin(num * (float)i);
				positions[i].y = range * Mathf.Cos(num * (float)i);
				positions[i].y += 6f;
				positions[i].z = 0f;
				gameObject.transform.SetParent(letterContainer);
				gameObject.transform.localScale = Vector3.one;
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localPosition = positions[i];
				gameObject.GetComponent<RectTransform>().anchoredPosition = positions[i];
				gameObject.GetComponent<RectTransform>().sizeDelta = letterSize * Vector2.one;
				gameObject.GetComponent<Letter>().setLetter(game.letters[i].ToString());
				gameObject.transform.Find("Text").GetComponent<Text>().color = GameController.InGameLetterColor;
				letters[i] = gameObject;
			}
			float time = initAnim();
			Movements.instance.executeWithDelay((Movements.Execute)onCreated, time);
		}

		private void onCreated()
		{
			if (onWheelCreated != null)
			{
				onWheelCreated();
			}
		}

		private float getWheelSize()
		{
			Vector2 anchorMax = letterContainer.parent.GetComponent<RectTransform>().anchorMax;
			float y = anchorMax.y;
			Vector2 sizeDelta = letterContainer.parent.parent.GetComponent<RectTransform>().sizeDelta;
			float y2 = sizeDelta.y;
			return y2 * y;
		}

		private void clearAnim()
		{
			if (letters != null)
			{
				for (int i = 0; i < letters.Length; i++)
				{
					GameObject gameObject = letters[i].gameObject;
					float num = (float)i * 0.07f + 0.1f;
					Movements.instance.move(gameObject, gameObject.transform.localPosition, Vector3.zero, 0.2f, num, local: true);
					Movements.instance.scale(gameObject, Vector3.one, Vector3.zero, 0.2f, num);
					UnityEngine.Object.Destroy(gameObject, num + 0.21f);
				}
			}
		}

		private float initAnim()
		{
			float result = 0f;
			for (int i = 0; i < letters.Length; i++)
			{
				GameObject gameObject = letters[i].gameObject;
				float num = (float)i * 0.07f + 0.1f;
				Movements.instance.move(gameObject, Vector3.zero, gameObject.transform.localPosition, 0.2f, num, local: true);
				Movements.instance.scale(gameObject, Vector3.zero, Vector3.one, 0.2f, num);
				gameObject.transform.localScale = Vector3.zero;
				gameObject.transform.localPosition = Vector3.zero;
				result = num + 0.2f;
			}
			return result;
		}

		private float calculateLetterScale()
		{
			float num = 1f;
			num = 1f;
			float wheelSize = getWheelSize();
			float num2 = -0.0146127269f * (float)game.letters.Length + 0.369925141f;
			letterSize = wheelSize * num2 * 0.95f;
			return num;
		}
	}
}
