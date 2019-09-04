using System;
using UnityEngine;

namespace v2Gameplay
{
	public class BoardAnimations : MonoBehaviour
	{
		private BoardController controller;

		private Level lvl;

		private RectTransform rect;

		private float startTime;

		private void Awake()
		{
			BoardController.animations = this;
			controller = GetComponent<BoardController>();
			startTime = Time.realtimeSinceStartup;
		}

		public void initAnim(Cell[,] cells)
		{
			int num = cells.GetLength(0) + cells.GetLength(1);
			float num2 = 0.3f / (float)num;
			float num3 = 0f;
			int length = cells.GetLength(0);
			int length2 = cells.GetLength(1);
			for (int i = 0; i < length; i++)
			{
				for (int j = 0; j < length2; j++)
				{
					Cell cell = cells[i, j];
					if (cell != null)
					{
						num3 = (float)(cell.x + cell.y) * num2;
						Movements.instance.executeWithDelay((Movements.Execute)cell.enableCell, num3);
					}
				}
			}
		}

		public void halfFade(Cell[,] cells)
		{
			float num = 1f;
			int length = cells.GetLength(0);
			int length2 = cells.GetLength(1);
			for (int i = 0; i < length; i++)
			{
				for (int j = 0; j < length2; j++)
				{
					Cell cell = cells[i, j];
					if (cell != null)
					{
						float time = num + (float)(cell.x + cell.y) * 0.05f;
						Movements.instance.executeWithDelay((Movements.Execute)cell.onlyStroke, time);
					}
				}
			}
		}

		public void fadeTournementBoard()
		{
		}

		public void fadeBoard(Level game)
		{
			rect = BoardController.instance.cellHolder.GetComponent<RectTransform>();
			RectTransform component = BoardController.instance.cellHolder.parent.GetComponent<RectTransform>();
			for (int i = 0; i < BoardController.instance.cellHolder.transform.childCount; i++)
			{
				GameObject gameObject = BoardController.instance.cellHolder.transform.GetChild(i).gameObject;
				if (!(gameObject.name == "Arrow"))
				{
					GameObject gameObject2 = gameObject.transform.Find("BG").gameObject;
					GameObject gameObject3 = gameObject.transform.Find("BG/Text").gameObject;
					Movements.instance.startFadeIn(gameObject2, 0.3f, 1f);
					Movements.instance.startFadeIn(gameObject3, 0.3f, 1f);
				}
			}
			float num = 0.5f;
			float num2 = 0f;
			Vector2 zero = Vector2.zero;
			Vector3 zero2 = Vector3.zero;
			zero2 = BoardController.instance.cellHolder.parent.localScale;
			if (game.mode == GameMode.ADVENTURE)
			{
				Movements.instance.scale(BoardController.instance.cellHolder.parent.gameObject, zero2, game.adventure.scale * Vector3.one, 0.5f, num2);
				Movements.instance.move(rect, rect.anchoredPosition, game.adventure.offset, 0.5f, num2);
				Movements.instance.executeWithDelay((Movements.ExecuteFloat)fadeAdventureBoard, num2 + 2.7f);
			}
			else
			{
				Movements.instance.executeWithDelay((Movements.ExecuteFloat)fadeBoard, num2 + 0.52f);
			}
			Movements.instance.executeWithDelay((Movements.Execute)AnimController.instance.popEndItems, num2 + 0.5f);
		}

		public float fadeAdventureBoard()
		{
			float result = 0f;
			Transform cellHolder = BoardController.instance.cellHolder;
			float num = 0.65f;
			float num2 = 0f;
			for (int i = 0; i < cellHolder.childCount; i++)
			{
				GameObject gameObject = cellHolder.GetChild(i).gameObject;
				if (num2 < gameObject.transform.localPosition.magnitude)
				{
					num2 = gameObject.transform.localPosition.magnitude;
				}
			}
			float num3 = num / num2;
			for (int j = 0; j < cellHolder.childCount; j++)
			{
				GameObject gameObject2 = cellHolder.GetChild(j).gameObject;
				try
				{
					float delay = num - gameObject2.transform.localPosition.magnitude * num3;
					gameObject2.GetComponent<Cell>().fadeOut(0f, 0.2f, delay);
					Movements.instance.move(gameObject2, gameObject2.transform.position, gameObject2.transform.position + Vector3.down * 0.5f, 0.2f, delay);
				}
				catch (Exception)
				{
					UnityEngine.Object.Destroy(gameObject2);
				}
			}
			Cell[,] board = BoardController.instance.board;
			return result;
		}

		public float fadeBoard()
		{
			float num = 0f;
			Transform cellHolder = BoardController.instance.cellHolder;
			float num2 = 0.3f;
			Cell[,] board = BoardController.instance.board;
			int length = board.GetLength(0);
			int length2 = board.GetLength(1);
			float num3 = num2 / (float)(length + length2);
			for (int i = 0; i < length; i++)
			{
				for (int j = 0; j < length2; j++)
				{
					float num4 = (float)(length + length2 - i - j) * num3;
					num = Mathf.Max(num4 + 0.2f, num);
					Cell cell = board[i, j];
					try
					{
						cell.fadeOut(0f, 0.2f, num4);
					}
					catch (Exception)
					{
					}
				}
			}
			return num;
		}

		public void dropBoard()
		{
			Transform cellHolder = BoardController.instance.cellHolder;
			float num = 0.3f;
			float num2 = 0f;
			for (int i = 0; i < cellHolder.childCount; i++)
			{
				GameObject gameObject = cellHolder.GetChild(i).gameObject;
				if (num2 < gameObject.transform.localPosition.magnitude)
				{
					num2 = gameObject.transform.localPosition.magnitude;
				}
			}
			Cell[,] board = BoardController.instance.board;
			int length = board.GetLength(0);
			int length2 = board.GetLength(1);
			float num3 = num / (float)(length + length2);
			for (int j = 0; j < length; j++)
			{
				for (int k = 0; k < length2; k++)
				{
					float delay = (float)(length + length2 - j - k) * num3;
					try
					{
						GameObject gameObject2 = board[j, k].gameObject;
						Movements.instance.move(gameObject2, gameObject2.transform.position, gameObject2.transform.position + Vector3.down * 1f, 0.2f, delay);
					}
					catch (Exception)
					{
					}
				}
			}
		}
	}
}
