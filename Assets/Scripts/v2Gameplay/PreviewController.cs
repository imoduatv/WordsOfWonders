using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace v2Gameplay
{
	public class PreviewController : MonoBehaviour
	{
		public static PreviewController instance;

		public ArabicText wordText;

		public Transform wordContainer;

		public RectTransform background;

		public RectTransform wordContainerRect;

		public Image bg;

		public GameObject textAnim;

		public GameObject letterPrefab;

		private void Awake()
		{
			instance = this;
		}

		private void Start()
		{
			Writer writer = Writer.instance;
			writer.onLetterAdd = (Writer.SendLetter)Delegate.Combine(writer.onLetterAdd, new Writer.SendLetter(handleLetterAdd));
			Writer writer2 = Writer.instance;
			writer2.onLetterRemoved = (Writer.SendCouple)Delegate.Combine(writer2.onLetterRemoved, new Writer.SendCouple(handleLetterRemove));
			WordController wordController = WordController.instance;
			wordController.onWordFound = (WordController.OnWordFound)Delegate.Combine(wordController.onWordFound, new WordController.OnWordFound(handleWordFound));
			WordController wordController2 = WordController.instance;
			wordController2.onSameFound = (WordController.OnWordFound)Delegate.Combine(wordController2.onSameFound, new WordController.OnWordFound(handleSameFound));
			WordController wordController3 = WordController.instance;
			wordController3.onExtraFound = (WordController.OnExtraFound)Delegate.Combine(wordController3.onExtraFound, new WordController.OnExtraFound(handleExtraFound));
			WordController wordController4 = WordController.instance;
			wordController4.onExtraFoundAgain = (WordController.NoParam)Delegate.Combine(wordController4.onExtraFoundAgain, new WordController.NoParam(handleExtraAgain));
			WordController wordController5 = WordController.instance;
			wordController5.onNotFound = (WordController.NoParam)Delegate.Combine(wordController5.onNotFound, new WordController.NoParam(handleNotFound));
			GameController gameController = GameController.instance;
			gameController.onNewGame = (GameController.SendGame)Delegate.Combine(gameController.onNewGame, new GameController.SendGame(handleNewGame));
		}

		private void handleNewGame(Level game)
		{
			bg.color = GameController.SelectedLetterBGColor;
			if (PlayerPrefsManager.GetLang() == "Arabic")
			{
				wordContainer.SetAsFirstSibling();
				wordText.transform.SetAsLastSibling();
				return;
			}
			wordContainer.SetAsLastSibling();
			if (wordText != null)
			{
				wordText.transform.SetAsFirstSibling();
				wordText.gameObject.SetActive(value: false);
				UnityEngine.Object.Destroy(wordText.gameObject);
			}
		}

		private void handleLetterAdd(Letter letter)
		{
			addToPreview(letter);
		}

		private void handleLetterRemove(Letter newLetter, Letter lastLetter)
		{
			removeFromPreview();
		}

		private void handleWordFound(Word word)
		{
			float num = 0.05f;
			GameObject[] array = new GameObject[wordContainer.childCount];
			for (int i = 0; i < wordContainer.childCount; i++)
			{
				array[i] = wordContainer.GetChild(i).gameObject;
			}
			float num2 = 0f;
			float num3 = (float)array.Length * num;
			for (int j = 0; j < array.Length; j++)
			{
				GameObject gameObject = array[j];
				int num4 = j;
				if (UISwapper.flipGame)
				{
					num4 = word.cells.Length - j - 1;
				}
				GameObject gameObject2 = word.cells[num4].gameObject;
				gameObject2.transform.SetAsLastSibling();
				Piece component = gameObject.GetComponent<Piece>();
				component.gameObject.SetActive(value: true);
				component.start = gameObject.transform.position;
				component.end = gameObject2.transform.position;
				gameObject.transform.SetParent(gameObject2.transform);
				gameObject.transform.SetAsLastSibling();
				num2 = (float)j * num;
				if (UISwapper.flipGame)
				{
					num2 = num3 - num2;
				}
				else if (j != 0)
				{
				}
				component.startMove(0.28f + num2);
				Movements.instance.executeWithDelay((Movements.Execute)component.transform.parent.GetComponent<Cell>().placeLetter, num2 + 0.2f);
				component.transform.parent.GetComponent<Cell>().found = true;
				int num5 = array.Length - 1 - j;
				word.cells[num5].transform.SetAsLastSibling();
			}
			for (int k = 0; k < array.Length; k++)
			{
				int num6 = word.cells.Length - k - 1;
				if (UISwapper.flipGame)
				{
					num6 = k;
				}
				GameObject gameObject3 = word.cells[num6].gameObject;
				gameObject3.transform.SetAsLastSibling();
			}
			Movements.instance.startFadeOut(background.gameObject, num2, 0f);
			if (PlayerPrefsManager.GetLang() == "Arabic")
			{
				wordText.gameObject.SetActive(value: false);
			}
		}

		private void handleSameFound(Word word)
		{
			Cell[] cells = word.cells;
			foreach (Cell cell in cells)
			{
				Movements.instance.tilt(cell.transform.gameObject, 2, 0.3f);
				Movements.instance.startFlash(cell.transform.Find("BG").GetComponent<Image>(), GameController.SelectedLetterBGColor, Color.white, 0.3f);
			}
			StartCoroutine(notFoundanim());
		}

		private void handleExtraFound(string word)
		{
			spawnExtra(word);
			if (!PlayerPrefsManager.GetFirstExtra())
			{
				Movements.instance.executeWithDelay((Movements.Execute)TutorialController.instance.openExtra, 0.3f);
				GameAnimController.instance.enableExtra();
				PlayerPrefsManager.OnFirstExtraFound();
			}
		}

		private void handleExtraAgain()
		{
			StartCoroutine(notFoundanim());
			Movements.instance.tilt(ObjectHolder.instance.starButton, 3, 0.3f);
		}

		private void handleNotFound()
		{
			StartCoroutine(notFoundanim());
		}

		public void clearPrev()
		{
			clear();
			calBg();
		}

		public void spawnExtra(string newExtra)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(textAnim);
			gameObject.GetComponent<Text>().text = newExtra;
			gameObject.transform.SetParent(wordContainer.parent);
			gameObject.transform.position = wordContainer.transform.position;
			ExtraWord component = gameObject.GetComponent<ExtraWord>();
			component.transform.localScale = Vector3.one;
			component.start = gameObject.transform.position;
			component.end = ObjectHolder.instance.starButton.transform.position;
			component.startMove();
			clear();
		}

		private GameObject copyContainer()
		{
			return UnityEngine.Object.Instantiate(wordContainer.parent.gameObject, wordContainer.parent.parent);
		}

		private IEnumerator notFoundanim()
		{
			GameObject copy = copyContainer();
			clear();
			calBg();
			GameObject nullObject = UnityEngine.Object.Instantiate(letterPrefab);
			nullObject.GetComponent<Text>().text = string.Empty;
			yield return null;
			nullObject.transform.SetParent(copy.transform.Find("WordContainer"));
			yield return null;
			Movements.instance.tilt(copy, 2, 0.35f);
			UnityEngine.Object.Destroy(copy, 0.31f);
		}

		private void clear()
		{
			clr();
		}

		private void clr()
		{
			for (int i = 0; i < wordContainer.childCount; i++)
			{
				UnityEngine.Object.Destroy(wordContainer.GetChild(i).gameObject);
			}
			if (PlayerPrefsManager.GetLang() == "Arabic")
			{
				wordText.Text = " ";
				wordText.GetComponent<Text>().text = string.Empty;
			}
			calBg();
		}

		private void addToPreview(Letter letter)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(letterPrefab);
			gameObject.transform.SetParent(wordContainer);
			gameObject.GetComponent<Text>().color = Color.white;
			gameObject.SetText(letter.l);
			gameObject.transform.localScale = Vector3.one;
			gameObject.transform.localPosition = Vector3.zero;
			if (PlayerPrefsManager.GetLang() == "Arabic")
			{
				gameObject.transform.SetAsFirstSibling();
				gameObject.SetActive(value: false);
				wordText.gameObject.SetActive(value: true);
			}
			if (PlayerPrefsManager.GetLang() == SystemLanguage.Hebrew.ToString())
			{
				gameObject.transform.SetAsFirstSibling();
			}
			updateText(removed: false);
			calculateBackground();
		}

		private void removeFromPreview()
		{
			GameObject obj = (!UISwapper.flipGame) ? wordContainer.GetChild(wordContainer.childCount - 1).gameObject : wordContainer.GetChild(0).gameObject;
			calculateBackground(obj);
			UnityEngine.Object.Destroy(obj);
			updateText(removed: true);
		}

		private void clearPreview()
		{
			for (int i = 0; i < wordContainer.childCount; i++)
			{
				UnityEngine.Object.Destroy(wordContainer.GetChild(i).gameObject);
			}
			hideBackground();
			if (UISwapper.flipGame)
			{
				wordText.Text = " ";
				wordText.GetComponent<Text>().text = string.Empty;
			}
		}

		private void calBg()
		{
			calculateBackground(0f);
		}

		private void calculateBackground(GameObject obj)
		{
			bg.color = GameController.SelectedLetterBGColor;
			float num = 0f;
			for (int i = 0; i < wordContainer.childCount; i++)
			{
				if (wordContainer.GetChild(i).gameObject != obj)
				{
					float num2 = num;
					Vector2 sizeDelta = wordContainer.GetChild(i).GetComponent<RectTransform>().sizeDelta;
					num = num2 + sizeDelta.x;
				}
			}
			Vector2 sizeDelta2 = wordContainerRect.sizeDelta;
			sizeDelta2.y = 80f;
			sizeDelta2.x = num;
			if (sizeDelta2.x < 20f)
			{
				sizeDelta2.x = 0f;
			}
			else
			{
				sizeDelta2.x += 60f;
			}
			background.sizeDelta = sizeDelta2;
			background.anchoredPosition = wordContainerRect.anchoredPosition + Vector2.down * 6f;
		}

		private void calculateBackground()
		{
			bg.color = GameController.SelectedLetterBGColor;
			float num = 0f;
			for (int i = 0; i < wordContainer.childCount; i++)
			{
				float num2 = num;
				Vector2 sizeDelta = wordContainer.GetChild(i).GetComponent<RectTransform>().sizeDelta;
				num = num2 + sizeDelta.x;
			}
			Vector2 sizeDelta2 = wordContainerRect.sizeDelta;
			sizeDelta2.y = 80f;
			sizeDelta2.x = num;
			if (sizeDelta2.x < 20f)
			{
				sizeDelta2.x = 0f;
			}
			else
			{
				sizeDelta2.x += 60f;
			}
			background.sizeDelta = sizeDelta2;
			background.anchoredPosition = wordContainerRect.anchoredPosition + Vector2.down * 6f;
		}

		private void calculateBackground(float s)
		{
			bg.color = GameController.SelectedLetterBGColor;
			Vector2 sizeDelta = wordContainerRect.sizeDelta;
			sizeDelta.x = s;
			background.sizeDelta = sizeDelta;
			background.anchoredPosition = wordContainerRect.anchoredPosition + Vector2.down * 6f;
		}

		private void hideBackground()
		{
			background.sizeDelta = Vector2.zero;
		}

		private void updateText(bool removed)
		{
			if (PlayerPrefsManager.GetLang() == "Arabic")
			{
				wordText.Text = getWord(removed);
				wordText.FixArabicText();
			}
		}

		private string getWord(bool removed)
		{
			string text = string.Empty;
			int num = wordContainer.childCount;
			if (removed)
			{
				num--;
			}
			for (int i = 0; i < num; i++)
			{
				int index = i;
				if (UISwapper.flipGame)
				{
					index = wordContainer.childCount - i - 1;
				}
				text += wordContainer.GetChild(index).GetComponent<Text>().text;
			}
			return text;
		}
	}
}
