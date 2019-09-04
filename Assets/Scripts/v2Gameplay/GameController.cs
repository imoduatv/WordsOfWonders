using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace v2Gameplay
{
	public class GameController : MonoBehaviour
	{
		public delegate void SendGame(Level game);

		public static GameController instance;

		public static int levelToOpen = -1;

		public static GameMode mode;

		public static string adventureSectionId;

		public static string adventureSetId;

		public static string adventureGameId;

		public static int adventureCount;

		public static bool daily;

		public static EndType endType;

		public static int slidePos;

		public static Section section;

		public static LevelSet set;

		public static Level game;

		public static Color InGameSelectedLetterColor;

		public static Color InGameLetterColor;

		public static Color SelectedLetterBGColor;

		public static Color TileColor;

		public static Color WheelColor;

		public static Color FirstSetColor;

		public static Color RibbonColor;

		public static Color HeaderColor;

		public static Color WheelStrokeColor;

		public SendGame onNewGame;

		public SendGame onGameEnd;

		public SendGame onTournamentEnd;

		public SendGame onNextLevel;

		public Image background;

		public new Text name;

		private float startTime;

		private void Awake()
		{
			if (PlayerPrefsManager.GetLevel() == levelToOpen)
			{
				levelToOpen = -1;
			}
			instance = this;
			tempStart();
			GoogleAnalyticsScript.instance.LogScreen("Game");
		}

		private void Start()
		{
			WordController wordController = WordController.instance;
			wordController.onWordFound = (WordController.OnWordFound)Delegate.Combine(wordController.onWordFound, new WordController.OnWordFound(handleWordFound));
			WordController wordController2 = WordController.instance;
			wordController2.onNotFound = (WordController.NoParam)Delegate.Combine(wordController2.onNotFound, new WordController.NoParam(handleNotFound));
			StartCoroutine(delay());
			setName();
		}

		private void OnApplicationFocus(bool focus)
		{
			if (Application.platform != 0)
			{
				if (daily)
				{
					GoogleAnalyticsScript.instance.LogScreen("Daily Puzzle Game");
				}
				else
				{
					GoogleAnalyticsScript.instance.LogScreen("Game");
				}
			}
		}

		public void newGame()
		{
			newGame(game);
		}

		public void newGame(Level game)
		{
			setColors();
		}

		private void setColors()
		{
			InGameSelectedLetterColor = FugoUtils.HexToColor(set.InGameSelectedLetterColor);
			SelectedLetterBGColor = FugoUtils.HexToColor(set.InGameSelectedLetterBGColor);
			InGameLetterColor = FugoUtils.HexToColor(set.InGameLetterColor);
			if (section.FirstSetColor != null)
			{
				FirstSetColor = FugoUtils.HexToColor(section.FirstSetColor);
			}
			TileColor = FugoUtils.HexToColor(set.InGameTileColor);
			WheelColor = FugoUtils.HexToColor(set.InGameCircleColor);
			HeaderColor = FugoUtils.HexToColor(set.InGameHeaderColor);
			RibbonColor = FugoUtils.HexToColor(set.InGameRibbonColor);
			WheelStrokeColor = FugoUtils.Color(253, 181, 47);
			if (PlayerPrefsManager.IsBlueMode() && PlayerPrefsManager.GetInGameLetterColor() != string.Empty)
			{
				SelectedLetterBGColor = FugoUtils.HexToColor(PlayerPrefsManager.GetInGameLetterColor());
			}
		}

		public void checkLevelEnd()
		{
			bool flag = true;
			foreach (Word word in game.words)
			{
				if (!word.isFound())
				{
					flag = false;
				}
			}
			if (!flag)
			{
				return;
			}
			if (game.mode == GameMode.ADVENTURE)
			{
				if (game.part == AdventurePart.END)
				{
					tournamentEnd();
				}
				else
				{
					Games.adventure.curPos++;
					PlayerPrefsManager.SetAdventurePosition(Games.adventure.curPos, game.setId);
					game = Games.adventure.Levels[Games.adventure.curPos];
					game.setId = int.Parse(adventureSetId);
					if (onNextLevel != null)
					{
						onNextLevel(game);
					}
					newGame();
					TouchController.instance.disable();
					StartCoroutine(delay());
				}
				PlayerPrefsManager.SetHiddenLevel(int.Parse(set.SetID), PlayerPrefsManager.GetHiddenLevel(int.Parse(set.SetID)) + 1);
			}
			else
			{
				levelEnd();
			}
			RequestManager.instance.LogRequest();
		}

		public int getLevelNumber()
		{
			try
			{
				if (levelToOpen == -1)
				{
					return FugoUtils.getLevelInfo(PlayerPrefsManager.GetLevel() % (PlayerPrefsManager.CountLevels() + 2))[2];
				}
				return FugoUtils.getLevelInfo(levelToOpen)[2];
			}
			catch (Exception)
			{
				return 1;
			}
		}

		public int getSetNumber()
		{
			if (levelToOpen == -1)
			{
				return FugoUtils.getLevelInfo(PlayerPrefsManager.GetLevel())[1];
			}
			return FugoUtils.getLevelInfo(levelToOpen)[1];
		}

		public int getSectionNumber()
		{
			if (levelToOpen == -1)
			{
				return FugoUtils.getLevelInfo(PlayerPrefsManager.GetLevel())[0];
			}
			return FugoUtils.getLevelInfo(levelToOpen)[0];
		}

		public EndType getEndType()
		{
			UnityEngine.Debug.Log("get end type  " + game.mode);
			if (game.mode == GameMode.NORMAL)
			{
				if (levelToOpen != -1)
				{
					return EndType.Level;
				}
				int levelNumber = getLevelNumber();
				int sectionNumber = getSectionNumber();
				int setNumber = getSetNumber();
				if (levelNumber + 1 == set.levels.Count)
				{
					if (setNumber + 1 == section.sets.Count)
					{
						return EndType.Section;
					}
					return EndType.Set;
				}
				return EndType.Level;
			}
			if (game.mode == GameMode.ADVENTURE)
			{
				saveHidden();
				if (PlayerPrefsManager.GetHiddenLevel(game.setId) == set.levels.Count)
				{
					SectionController.hiddenSetCompleted = int.Parse(set.SetID);
					return EndType.Hidden;
				}
				return EndType.Level;
			}
			return EndType.Level;
		}

		private void tournamentEnd()
		{
			endType = getEndType();
			Games.adventure.curPos = 0;
			slidePos = 1;
			adventureGameId = (int.Parse(adventureGameId) + 1).ToString();
			if (onTournamentEnd != null)
			{
				onTournamentEnd(game);
			}
		}

		public void saveHidden()
		{
			UnityEngine.Debug.Log("hidden saved");
			PlayerPrefsManager.SetAdventurePosition(0, game.setId);
			PlayerPrefsManager.SetLastHiddenSet(game.setId.ToString());
			PlayerPrefsManager.SetLastHiddenSection(adventureSectionId);
			PlayerPrefsManager.SetHiddenPos(game.adventure.curPos + 1);
			PlayerPrefsManager.SetHiddenGameID(adventureGameId);
			PlayerPrefsManager.SetHiddenCount(adventureCount);
		}

		private void nextPart()
		{
		}

		private void levelEnd()
		{
			endType = getEndType();
			UnityEngine.Debug.Log(endType);
			if (game.mode != GameMode.ADVENTURE)
			{
				SoundManager.instance.GameWin();
			}
			if (game.mode == GameMode.NORMAL)
			{
				if (levelToOpen == -1)
				{
					PlayerPrefsManager.SetLevel(PlayerPrefsManager.GetLevel() + 1);
					PlayerPrefsManager.SetBrilliance(PlayerPrefsManager.GetBrilliance() + 1);
					PlayerPrefsManager.ResetProHint();
					FirebaseController.SendLevelLog();
				}
				else
				{
					levelToOpen++;
				}
			}
			Movements.instance.executeWithDelay((Movements.Execute)GameAnimController.instance.niceTexts, 0.7f);
			if (onGameEnd != null)
			{
				onGameEnd(game);
			}
		}

		private void handleNotFound()
		{
			if (game.part == AdventurePart.NONE)
			{
				checkLevelEnd();
				return;
			}
			Movements.instance.executeWithDelay((Movements.Execute)checkLevelEnd, 0.5f);
			PreviewController.instance.clearPrev();
		}

		private void handleWordFound(Word word)
		{
			if (game.part == AdventurePart.NONE)
			{
				checkLevelEnd();
				return;
			}
			Movements.instance.executeWithDelay((Movements.Execute)checkLevelEnd, 0.5f);
			PreviewController.instance.clearPrev();
		}

		private void testLevelInfo()
		{
			for (int i = 1; i < 300; i++)
			{
				levelToOpen = i;
				UnityEngine.Debug.Log("section: " + getSectionNumber() + "  set: " + getSetNumber() + "  level: " + getLevelNumber());
			}
			levelToOpen = -1;
		}

		private void openMenu()
		{
			FugoUtils.openScene("Menu");
		}

		public void handleGameParse()
		{
			StartCoroutine(delay());
		}

		private IEnumerator delay()
		{
			startTime = Time.realtimeSinceStartup;
			yield return null;
			UnityEngine.Debug.Log("Anim Delay  " + (Time.realtimeSinceStartup - startTime));
			if (game.mode == GameMode.DAILY)
			{
				Movements.instance.executeWithDelay((Movements.Execute)TutorialController.instance.openDaily, 0.5f);
			}
			if (levelToOpen == -1)
			{
				int index = PlayerPrefsManager.GetLevel();
				if (game.mode == GameMode.NORMAL)
				{
					if (index == 1)
					{
						Movements.instance.executeWithDelay((Movements.Execute)TutorialController.instance.openFirst, 1.8f);
					}
					if (index == 3)
					{
						Movements.instance.executeWithDelay((Movements.Execute)TutorialController.instance.openSecond, 0.1f);
					}
					if (index == 5)
					{
						Movements.instance.executeWithDelay((Movements.Execute)TutorialController.instance.openThird, 0.1f);
					}
				}
			}
			else
			{
				int levelToOpen2 = levelToOpen;
			}
			MegaHintController.instance.checkMegaHint();
			if (game.mode == GameMode.ADVENTURE)
			{
				saveHidden();
			}
			if (onNewGame != null)
			{
				onNewGame(game);
			}
			Movements.instance.executeWithDelay((Movements.Execute)checkLevelEnd, 1.5f);
			yield return null;
		}

		private void loadGame(int id)
		{
			int[] levelInfo = FugoUtils.getLevelInfo(id);
		}

		private void tempStart()
		{
			int num = 0;
			if (daily)
			{
				DateTime now = DateTime.Now;
				section = Games.dailyPuzzles;
				set = section.sets[now.Month - 1];
				game = set.levels[now.Day - 1];
				game.mode = GameMode.DAILY;
			}
			else
			{
				num = ((levelToOpen != -1) ? levelToOpen : PlayerPrefsManager.GetLevel());
				int[] levelInfo = FugoUtils.getLevelInfo(num);
				if (mode == GameMode.ADVENTURE)
				{
					section = Games.GetSection(adventureSectionId);
					set = section.getHiddenSet(adventureSetId);
					UnityEngine.Debug.Log(adventureGameId + "   " + adventureSetId + "   " + adventureSectionId);
					Adventure adventure = Games.adventure = new Adventure(section, set, set.getLevelsWithID(adventureGameId));
					if (slidePos == 0)
					{
						Games.adventure.curPos = 0;
					}
					else
					{
						Games.adventure.curPos = slidePos - 1;
					}
					game = Games.adventure.Levels[Games.adventure.curPos];
					game.setId = int.Parse(adventureSetId);
					game.mode = GameMode.ADVENTURE;
					UnityEngine.Debug.Log(game.part);
					if (game.adventure.Width == -1)
					{
						calculateSize();
					}
				}
				else
				{
					section = Games.sections[levelInfo[0]];
					set = section.sets[levelInfo[1]];
					game = set.levels[levelInfo[2]];
				}
				UnityEngine.Debug.Log("game name " + UISwapper.flipGame);
				name.gameObject.SetActive(value: true);
			}
			string path = "BGImages/" + set.bgImage;
			game.set = set;
			background.sprite = Resources.Load<Sprite>(path);
			background.gameObject.SetActive(value: true);
			newGame(game);
			if (UISwapper.flipGame)
			{
				StartCoroutine(ArabicText.fixAllNextFrame());
			}
			name.color = HeaderColor;
			Color headerColor = HeaderColor;
			headerColor.a = 0f;
			name.color = headerColor;
		}

		private void setName()
		{
			int levelNumber = (levelToOpen != -1) ? levelToOpen : PlayerPrefsManager.GetLevel();
			int[] levelInfo = FugoUtils.getLevelInfo(levelNumber);
			if (UISwapper.flipGame)
			{
				ArabicText arabicText = name.gameObject.AddComponent<ArabicText>();
				if (game.mode == GameMode.ADVENTURE)
				{
					arabicText.Text = set.SetFullName + " ● " + adventureGameId;
				}
				else
				{
					arabicText.Text = set.SetFullName + " ● " + (levelInfo[2] + 1);
				}
			}
			else
			{
				try
				{
					if (game.mode == GameMode.ADVENTURE)
					{
						name.text = (set.SetFullName + " ● " + adventureGameId).AddSpaces();
					}
					else
					{
						name.text = (set.SetFullName + " ● " + (levelInfo[2] + 1)).AddSpaces();
					}
				}
				catch (Exception)
				{
				}
			}
		}

		private void calculateSize()
		{
			int num = -999;
			int num2 = -999;
			int num3 = 999;
			int num4 = 999;
			Level[] levels = game.adventure.Levels;
			foreach (Level level in levels)
			{
				int a = level.width + level.offsetX;
				int offsetX = level.offsetX;
				num = Mathf.Max(a, num);
				num3 = Mathf.Min(offsetX, num3);
				a = level.height + level.offsetY;
				offsetX = level.offsetY;
				num2 = Mathf.Max(a, num2);
				num4 = Mathf.Min(offsetX, num4);
			}
			game.adventure.Width = num - num3;
			game.adventure.Height = num2 - num4;
			Adventure.yBorder = -num4;
			UnityEngine.Debug.Log("GAME BORDER  " + num4);
		}
	}
}
