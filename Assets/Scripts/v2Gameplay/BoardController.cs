using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace v2Gameplay
{
	public class BoardController : MonoBehaviour
	{
		public static BoardController instance;

		public static BoardAnimations animations;

		public static float cellSize;

		public static float letterScale;

		public Transform cellHolder;

		public GameObject cellPrefab;

		public GameObject hintPrefab;

		public Cell[,] board;

		private Level game;

		private Vector2 offsetX;

		private Vector2 offsetY;

		private bool inited;

		private void Awake()
		{
			inited = false;
			instance = this;
			cellSize = 100f;
		}

		private void Start()
		{
			Application.targetFrameRate = 60;
			QualitySettings.vSyncCount = 0;
			GameController gameController = GameController.instance;
			gameController.onNewGame = (GameController.SendGame)Delegate.Combine(gameController.onNewGame, new GameController.SendGame(handleNewGame));
		}

		private void handleStarFound()
		{
			getStarCell();
		}

		private void handleNewGame(Level g)
		{
			if (board != null)
			{
				animations.halfFade(board);
			}
			if (!inited && g.adventure != null)
			{
				Level[] levels = g.adventure.Levels;
				foreach (Level level in levels)
				{
					UnityEngine.Debug.Log("adventure part");
					if (g == level)
					{
						break;
					}
					game = level;
					calculateCellSize();
					calculatePositions();
					createBoard(game);
					if (board != null)
					{
						animations.halfFade(board);
					}
					Cell[,] array = board;
					int length = array.GetLength(0);
					int length2 = array.GetLength(1);
					for (int j = 0; j < length; j++)
					{
						for (int k = 0; k < length2; k++)
						{
							Cell cell = array[j, k];
							if (cell != null)
							{
								cell.openCell();
							}
							try
							{
								cell.transform.Find("BG").localScale = Vector3.one;
							}
							catch (Exception)
							{
							}
						}
					}
				}
			}
			game = g;
			printBoard();
			calculateCellSize();
			calculatePositions();
			createBoard(game);
			loadHints();
			SaveLoad.instance.loadGame();
			try
			{
				initDailyStar();
			}
			catch (Exception)
			{
				PlayerPrefsManager.deleteStarPos();
				initDailyStar();
			}
			if (game.mode == GameMode.DAILY)
			{
				DailyController dailyController = DailyController.instance;
				dailyController.onStarFound = (DailyController.NoParam)Delegate.Combine(dailyController.onStarFound, new DailyController.NoParam(handleStarFound));
			}
			float num = 0.5f;
			if (game.mode == GameMode.ADVENTURE)
			{
				if (!inited)
				{
					Movements.instance.executeWithDelay((Movements.Execute)AnimController.instance.initGameAnims, 0f);
					num = 1.5f;
				}
				if (game.part != AdventurePart.START)
				{
					Movements.instance.executeWithDelay((Movements.Execute)animWrapper, num);
					Movements.instance.scale(cellHolder.parent.gameObject, cellHolder.parent.localScale, game.scale, num);
				}
				else
				{
					animations.initAnim(board);
					cellHolder.parent.localScale = game.scale;
				}
			}
			else
			{
				animations.initAnim(board);
				cellHolder.parent.localScale = game.scale;
				Movements.instance.executeWithDelay((Movements.Execute)AnimController.instance.initGameAnims, 0f);
			}
			inited = true;
		}

		private void animWrapper()
		{
			animations.initAnim(board);
		}

		private void calculateCellSize()
		{
			float num = (float)game.width / (float)game.height;
			Vector2 boardSize = getBoardSize();
			float x = boardSize.x;
			float y = boardSize.y;
			float num2 = x / y;
			if (game.adventure != null)
			{
				game.adventure.scale = Mathf.Min(y / (float)game.adventure.Height, x / (float)game.adventure.Width) / cellSize;
			}
			if (num < num2)
			{
				game.cellSize = y / (float)game.height;
			}
			else
			{
				game.cellSize = x / (float)game.width;
			}
			if (game.mode == GameMode.ADVENTURE)
			{
				game.scale = Vector2.one * game.cellSize / cellSize;
			}
			else
			{
				game.scale = Vector3.one;
				cellSize = game.cellSize;
			}
			letterScale = 11f / 925f * game.cellSize * 1.05f;
		}

		private Vector2 getBoardSize()
		{
			Vector2 sizeDelta = cellHolder.parent.parent.GetComponent<RectTransform>().sizeDelta;
			RectTransform component = cellHolder.parent.GetComponent<RectTransform>();
			Vector2 anchorMax = component.anchorMax;
			float y = anchorMax.y;
			Vector2 anchorMin = component.anchorMin;
			float num = y - anchorMin.y;
			Vector2 result = default(Vector2);
			result.x = sizeDelta.x * 0.95f;
			result.y = sizeDelta.y * num;
			return result;
		}

		private void calculatePositions()
		{
			RectTransform component = cellHolder.GetComponent<RectTransform>();
			if (game.mode == GameMode.ADVENTURE)
			{
				Vector2 a = Vector2.left * cellSize * game.width;
				Vector2 a2 = Vector2.up * cellSize * game.height;
				a *= 0.5f;
				a2 *= 0.5f;
				if (game.adventure != null)
				{
					game.adventure.offset = Vector2.up * game.adventure.Height * 0.5f + Vector2.left * game.adventure.Width * 0.5f;
					game.adventure.offset += Vector2.down * Adventure.yBorder;
					game.adventure.offset *= cellSize;
				}
				offsetX = Vector2.right * game.offsetX * cellSize;
				offsetY = Vector2.down * game.offsetY * cellSize;
				game.position = a + a2 - offsetY - offsetX;
				game.offset = offsetX + offsetY;
			}
			else
			{
				Vector2 a3 = Vector2.left * cellSize * game.width;
				Vector2 a4 = Vector2.up * cellSize * game.height;
				a3 *= 0.5f;
				a4 *= 0.5f;
				game.position = a3 + a4;
			}
			if (game.part == AdventurePart.START || game.part == AdventurePart.NONE)
			{
				component.anchoredPosition = game.position;
				return;
			}
			Vector3 localPosition = component.transform.localPosition;
			component.anchoredPosition = game.position;
			Vector3 localPosition2 = component.transform.localPosition;
			component.transform.localPosition = localPosition;
			float animTime = 0.5f;
			if (!inited)
			{
				animTime = 1.5f;
			}
			Movements.instance.move(cellHolder.gameObject, localPosition, localPosition2, animTime, local: true);
		}

		private bool checkForCollapse(GameObject temp)
		{
			for (int i = 0; i < cellHolder.childCount; i++)
			{
				GameObject gameObject = cellHolder.GetChild(i).gameObject;
				if (gameObject.activeSelf && !(gameObject == temp) && (temp.transform.localPosition - gameObject.transform.localPosition).magnitude <= 0.01f)
				{
					return true;
				}
			}
			return false;
		}

		private void createBoard(Level lvl)
		{
			board = new Cell[lvl.width, lvl.height];
			for (int i = 0; i < lvl.width; i++)
			{
				for (int j = 0; j < lvl.height; j++)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate(cellPrefab);
					board[i, j] = gameObject.GetComponent<Cell>();
					board[i, j].x = i;
					board[i, j].y = j;
					gameObject.transform.SetParent(cellHolder);
					gameObject.transform.SetAsLastSibling();
					gameObject.transform.localScale = Vector3.one;
					gameObject.transform.localPosition = Vector3.zero;
					if (UISwapper.flipGame)
					{
						gameObject.transform.Find("BG/Text").transform.localScale = new Vector3(-1f, 1f, 1f);
					}
					gameObject.SetActive(value: false);
					Color tileColor = GameController.TileColor;
					gameObject.transform.Find("BG").GetComponent<Image>().color = tileColor;
					gameObject.transform.Find("BG/Stroke").GetComponent<Image>().color = GameController.TileColor;
				}
			}
			foreach (Word word in lvl.words)
			{
				word.found = false;
				addWord(word);
			}
			for (int k = 0; k < board.GetLength(0); k++)
			{
				for (int l = 0; l < board.GetLength(1); l++)
				{
					if (!board[k, l].gameObject.activeSelf)
					{
						UnityEngine.Object.Destroy(board[k, l].gameObject);
						board[k, l] = null;
						board[k, l] = null;
					}
				}
			}
			for (int m = 0; m < board.GetLength(0); m++)
			{
				for (int n = 0; n < board.GetLength(1); n++)
				{
					if (board[m, n] != null)
					{
						GameObject gameObject2 = board[m, n].gameObject;
						if (checkForCollapse(gameObject2))
						{
							Cell component = gameObject2.GetComponent<Cell>();
							component.setFound();
							component.enableLetter();
						}
					}
				}
			}
			if (UISwapper.flipGame)
			{
				foreach (Word word2 in game.words)
				{
				}
				for (int num = 0; num < game.otherWords.Length; num++)
				{
				}
			}
		}

		private void loadHints()
		{
			if (ExtraWordController.instance.rewardable())
			{
				int[,] hints = SaveLoad.GetHints(game);
				for (int i = 0; i < hints.GetLength(0); i++)
				{
					int num = hints[i, 0];
					int num2 = hints[i, 1];
					try
					{
						if (board[num, num2] != null)
						{
							board[num, num2].enableHint(GameController.InGameLetterColor.a);
						}
					}
					catch (Exception)
					{
					}
				}
			}
		}

		private void addWord(Word word)
		{
			word.cells = new Cell[word.length];
			int x = word.X;
			int y = word.Y;
			int num = 0;
			int num2 = 0;
			if (word.orientation == Orientation.VERTICAL)
			{
				num2 = 1;
			}
			else
			{
				num = 1;
			}
			for (int i = 0; i < word.length; i++)
			{
				int num3 = word.X + num * i;
				int num4 = word.Y + num2 * i;
				word.letters[i] = board[num3, num4].gameObject;
				word.cells[i] = board[num3, num4];
				word.cells[i].setText(word.word[i].ToString());
				word.cells[i].rect.sizeDelta = Vector2.one * cellSize;
				word.cells[i].rect.anchoredPosition = Vector2.right * num3 * cellSize + Vector2.down * num4 * cellSize + offsetX + offsetY;
				word.cells[i].gameObject.SetActive(value: true);
			}
		}

		private void printBoard()
		{
			foreach (Word word in game.words)
			{
			}
			string[] otherWords = game.otherWords;
			foreach (string text in otherWords)
			{
			}
		}

		public void getHint()
		{
			Cell hintCell = getHintCell();
			if (hintCell != null)
			{
				hintCell.hinted = true;
				hintAnim(hintCell);
				SaveLoad.AddHint(hintCell.x, hintCell.y, game);
				Movements.instance.executeWithDelay((Movements.Execute)hintCell.enableHint, 0.2f);
				Movements.instance.executeWithDelay((Movements.Execute)hintCell.hintAnim, 0.2f);
				PlayerPrefsManager.SetCoin(PlayerPrefsManager.GetCoin() - PlayerPrefsManager.GetHintPrice());
				if (!TutorialController.freeHint)
				{
					PlayerPrefsManager.UseProHint();
				}
				TutorialController.freeHint = false;
				GameMenuController.instance.updateHintPrice();
				GameMenuController.instance.updateCoin(animating: false);
			}
		}

		public void getHint(Cell H, Letter l)
		{
			if (H != null)
			{
				H.hinted = true;
				hintAnim(H, l);
				SaveLoad.AddHint(H.x, H.y, game);
				Movements.instance.executeWithDelay((Movements.Execute)H.enableHint, 0.3f);
				Movements.instance.executeWithDelay((Movements.Execute)H.hintAnim, 0.3f);
				GameMenuController.instance.updateHintPrice();
				GameMenuController.instance.updateCoin(animating: false);
			}
		}

		private Cell getHintCell()
		{
			int num = UnityEngine.Random.Range(0, board.GetLength(0));
			int num2 = UnityEngine.Random.Range(0, board.GetLength(1));
			for (int i = 0; i < board.GetLength(0); i++)
			{
				for (int j = 0; j < board.GetLength(1); j++)
				{
					int num3 = (num + i) % board.GetLength(0);
					int num4 = (num2 + j) % board.GetLength(1);
					Cell cell = board[num3, num4];
					if (cell != null && cell.isItHintable())
					{
						return cell;
					}
				}
			}
			return null;
		}

		public void hintAnim(Cell cell)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(hintPrefab);
			gameObject.transform.SetParent(cell.transform);
			cell.transform.SetAsLastSibling();
			gameObject.transform.localScale = Vector3.one;
			gameObject.GetComponent<Text>().text = cell.letter;
			gameObject.GetComponent<Text>().color = GameController.InGameLetterColor;
			Vector3 one = Vector3.one;
			if (UISwapper.flipGame)
			{
				one.x *= -1f;
			}
			gameObject.transform.position = ObjectHolder.instance.hintButton.transform.position;
			if (UISwapper.flipGame)
			{
				StartCoroutine(changePivot(gameObject.GetComponent<RectTransform>(), 0.3f));
			}
			Movements.instance.move(gameObject, ObjectHolder.instance.hintButton.transform.Find("Hint/Image").transform.position, cell.transform.position, 0.2f);
			Movements.instance.scale(gameObject, letterScale * one, letterScale * one * 1.1f, 1f);
			Movements.instance.scale(gameObject, letterScale * 1.1f * one, one, 0.1f, 0.1f);
			UnityEngine.Object.Destroy(gameObject, 0.21f);
		}

		public void hintAnim(Cell cell, Letter letter)
		{
			float num = 0.2f;
			GameObject gameObject = UnityEngine.Object.Instantiate(hintPrefab);
			gameObject.transform.SetParent(cell.transform.Find("BG"));
			RectTransform component = gameObject.GetComponent<RectTransform>();
			component.pivot = Vector2.one * 0.5f;
			component.anchorMin = Vector2.one * 0.5f;
			component.anchorMax = Vector2.one * 0.5f;
			cell.transform.SetAsLastSibling();
			gameObject.transform.localScale = Vector3.one;
			gameObject.GetComponent<Text>().text = cell.letter;
			gameObject.GetComponent<Text>().color = GameController.InGameLetterColor;
			Vector3 one = Vector3.one;
			if (UISwapper.flipGame)
			{
				one.x *= -1f;
			}
			gameObject.transform.position = letter.transform.position;
			if (UISwapper.flipGame)
			{
				StartCoroutine(changePivot(gameObject.GetComponent<RectTransform>(), num));
			}
			Movements.instance.move(gameObject, gameObject.transform.localPosition, Vector3.zero, num, local: true);
			Movements.instance.scale(gameObject, 1f * one, letterScale * one, num);
			gameObject.GetComponent<Text>().color = GameController.WheelStrokeColor;
			Movements.instance.lerpColorTo(gameObject, GameController.InGameLetterColor, num * 1f);
			UnityEngine.Object.Destroy(gameObject, num + 0.05f);
		}

		private IEnumerator changePivot(RectTransform rect, float time)
		{
			float t = 0f;
			while (t <= time)
			{
				float par = t / time;
				try
				{
					rect.pivot = Vector2.Lerp(Vector2.zero, Vector2.one, par);
				}
				catch
				{
				}
				t += Time.deltaTime;
				yield return null;
			}
			try
			{
				rect.pivot = Vector2.one;
			}
			catch
			{
			}
		}

		public List<Cell> getCellsWith(Letter letter)
		{
			List<Cell> list = new List<Cell>();
			Cell[,] array = board;
			int length = array.GetLength(0);
			int length2 = array.GetLength(1);
			for (int i = 0; i < length; i++)
			{
				for (int j = 0; j < length2; j++)
				{
					Cell cell = array[i, j];
					if (cell != null && cell.letter == letter.l && cell.isItHintable())
					{
						list.Add(cell);
					}
				}
			}
			return list;
		}

		private void initDailyStar()
		{
			if (game.mode == GameMode.DAILY)
			{
				int[] starPos = PlayerPrefsManager.getStarPos();
				if (starPos[0] == -1)
				{
					UnityEngine.Debug.Log("new star");
					getStarCell();
				}
				else
				{
					GameAnimController.dailyStar = board[starPos[0], starPos[1]].setStarred();
					UnityEngine.Debug.Log("load star");
				}
			}
		}

		private Cell getStarCell()
		{
			UnityEngine.Debug.Log("get hint");
			PlayerPrefsManager.deleteStarPosition();
			int num = UnityEngine.Random.Range(0, board.GetLength(0));
			int num2 = UnityEngine.Random.Range(0, board.GetLength(1));
			for (int i = 0; i < board.GetLength(0); i++)
			{
				for (int j = 0; j < board.GetLength(1); j++)
				{
					int num3 = (num + i) % board.GetLength(0);
					int num4 = (num2 + j) % board.GetLength(1);
					Cell cell = board[num3, num4];
					if (cell != null && cell.isItStarrable())
					{
						GameAnimController.dailyStar = cell.setStarred();
						return null;
					}
				}
			}
			return null;
		}

		public void finishGame()
		{
			foreach (Word word in game.words)
			{
				word.setFound();
			}
			GameController.instance.checkLevelEnd();
		}
	}
}
