using System;
using System.Collections;
using UnityEngine;
using v2Gameplay;

public class Sphere : MonoBehaviour
{
	public static Sphere instance;

	private float speed = -6f;

	private void Start()
	{
		instance = this;
		if (GameController.instance != null)
		{
			GameController gameController = GameController.instance;
			gameController.onNewGame = (GameController.SendGame)Delegate.Combine(gameController.onNewGame, new GameController.SendGame(handleNewGame));
		}
	}

	private void Update()
	{
		base.transform.Rotate(0f, speed * Time.deltaTime, 0f);
	}

	public void spin()
	{
		StartCoroutine(goFaster());
	}

	private void handleNewGame(Level g)
	{
		if (g.mode != 0)
		{
			for (int i = 0; i < base.transform.parent.childCount; i++)
			{
				GameObject gameObject = base.transform.parent.GetChild(i).gameObject;
				gameObject.SetActive(gameObject.name == "BigStarHolder");
			}
		}
	}

	private IEnumerator goFaster()
	{
		float animTime = 0.1f;
		for (float t = 0f; t <= animTime; t += Time.deltaTime)
		{
			float par = t / animTime;
			speed = par * -94f;
			speed -= 6f;
			yield return null;
		}
		StartCoroutine(goSlower());
	}

	private IEnumerator goSlower()
	{
		float animTime = 5.1f;
		for (float t = 0f; t <= animTime; t += Time.deltaTime)
		{
			float par2 = t / animTime;
			par2 = 1f - par2;
			speed = par2 * -94f;
			speed -= 6f;
			yield return null;
		}
		speed = -6f;
	}
}
