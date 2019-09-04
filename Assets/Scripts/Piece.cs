using UnityEngine;
using UnityEngine.UI;
using v2Gameplay;

public class Piece : MonoBehaviour
{
	public Vector3 start;

	public Vector3 end;

	private static float animTime = 0.4f;

	private Vector2 v1;

	private Vector2 v2;

	private Vector2 v3;

	public Vector3 startScale;

	public Vector3 endScale;

	private Vector3 direction;

	private Shadow shadow;

	private RectTransform rect;

	private void Start()
	{
		rect = GetComponent<RectTransform>();
		shadow = GetComponent<Shadow>();
		GetComponent<Text>().color = GameController.InGameSelectedLetterColor;
	}

	public void startMove()
	{
		startMove(0.28f);
	}

	public void startMove(float time)
	{
		startScale = base.transform.localScale;
		base.transform.position = start;
		base.transform.position = Vector3.Lerp(start, end, 0f);
		Movements.instance.move(moveFunc, time);
	}

	public void moveFunc(float par, MoveState state)
	{
		if (state == MoveState.START)
		{
			rect = GetComponent<RectTransform>();
			Transform parent = base.transform.parent;
			v1 = rect.anchorMax;
			v2 = rect.anchorMin;
			v3 = rect.pivot;
			parent.GetComponent<Cell>().enableCell();
			direction = end - start;
			base.gameObject.SetActive(value: true);
			startScale = base.transform.localScale;
			endScale = Vector3.one * (11f / 925f) * v2Gameplay.BoardController.cellSize;
			if (UISwapper.flipGame)
			{
				endScale.x *= -1f;
			}
			base.transform.position = start;
		}
		if (state == MoveState.MOVE)
		{
			par = Mathf.Pow(par, 0.66f);
			float d = Mathf.Lerp(0f, direction.x, Mathf.Pow(par, 0.5f));
			float d2 = Mathf.Lerp(0f, direction.y, Mathf.Pow(par, 2f));
			if (UISwapper.flipGame)
			{
				Vector2 b = new Vector2(0.5f, 0.5f);
				Vector2 b2 = new Vector2(0.5f, 0.5f);
				rect.anchorMin = Vector2.Lerp(v1, b, par);
				rect.anchorMax = Vector2.Lerp(v2, b, par);
				rect.pivot = Vector2.Lerp(v3, b2, par);
			}
			else
			{
				GetComponent<RectTransform>().pivot = Vector2.Lerp(v1, new Vector2(0f, 1f), par);
			}
			Vector3 b3 = Vector3.right * d + Vector3.up * d2;
			base.transform.position = start + b3;
			base.transform.localScale = Vector3.Lerp(startScale, endScale * 1.2f, Mathf.Pow(par, 1f)) * (1f + 3f * par * (1f - par));
			if (UISwapper.flipGame)
			{
				rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, Vector3.zero, par);
			}
		}
		if (state == MoveState.END)
		{
			Transform parent2 = base.transform.parent;
			base.transform.position = end;
			base.transform.localScale = endScale;
			shadow.enabled = false;
			SoundManager.instance.LetterPlace();
			base.gameObject.SetActive(value: false);
			UnityEngine.Object.Destroy(this, 0.01f);
		}
	}
}
