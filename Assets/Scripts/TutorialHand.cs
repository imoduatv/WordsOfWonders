using System;
using System.Collections.Generic;
using UnityEngine;
using v2Gameplay;

public class TutorialHand : MonoBehaviour
{
	public static TutorialHand instance;

	private List<Letter> letters;

	private List<Vector3> positions;

	private Coroutine repeat;

	private Level game;

	private List<Coroutine> moves;

	private void Awake()
	{
		instance = this;
	}

	public void init(Level g)
	{
		game = g;
		UnityEngine.Debug.Log("hand init");
		letters = new List<Letter>();
		Transform letterContainer = v2Gameplay.WheelController.instance.letterContainer;
		for (int i = 0; i < letterContainer.childCount; i++)
		{
			letters.Add(letterContainer.GetChild(i).GetComponent<Letter>());
			UnityEngine.Debug.Log("letter add " + letterContainer.GetChild(i).GetComponent<Letter>());
		}
		WordController wordController = WordController.instance;
		wordController.onWordFound = (WordController.OnWordFound)Delegate.Combine(wordController.onWordFound, new WordController.OnWordFound(handleWordFound));
	}

	private void handleWordFound(Word word)
	{
		foreach (Word word2 in game.words)
		{
			if (!word2.found)
			{
				findWord(word2);
				break;
			}
		}
	}

	public void startAnim(Word w)
	{
	}

	public void findWord(Word w)
	{
		stopAnim();
		positions = new List<Vector3>();
		List<Letter> list = new List<Letter>();
		foreach (Letter letter in letters)
		{
			list.Add(letter);
		}
		for (int i = 0; i < w.word.Length; i++)
		{
			foreach (Letter item in list)
			{
				if (item.l == w.word[i].ToString())
				{
					positions.Add(item.transform.position);
					list.Remove(item);
					break;
				}
			}
		}
		Movements.instance.startFadeIn(base.gameObject, 0.2f, 1f);
		animate();
	}

	public void stopAnim()
	{
		if (repeat != null)
		{
			StopCoroutine(repeat);
		}
		try
		{
			foreach (Coroutine move in moves)
			{
				StopCoroutine(move);
			}
		}
		catch (Exception)
		{
		}
	}

	private void animate()
	{
		base.transform.GetComponent<TrailRenderer>().Clear();
		moves = new List<Coroutine>();
		float num = 1f;
		float time = 0f;
		for (int i = 1; i < positions.Count; i++)
		{
			moves.Add(Movements.instance.move(base.gameObject, positions[i - 1], positions[i], num, (float)(i - 1) * num));
			time = (float)(i + 1) * num;
		}
		repeat = Movements.instance.executeWithDelay((Movements.Execute)animate, time);
	}

	public void disableHand()
	{
		Movements.instance.startFadeOut(base.gameObject, 0.3f, 0f);
		GetComponent<TrailRenderer>().enabled = false;
	}
}
