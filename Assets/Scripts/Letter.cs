using System;
using UnityEngine;
using UnityEngine.UI;
using v2Gameplay;

public class Letter : MonoBehaviour
{
	public string l;

	public bool used;

	public float angle;

	private Coroutine touchThread;

	private bool touching;

	private float scale;

	private float range;

	private bool hint;

	private void Start()
	{
		touching = false;
		used = false;
		scale = 1336f / (float)Screen.height;
		hint = false;
		TouchController instance = TouchController.instance;
		instance.onCalculate = (TouchController.TouchAction)Delegate.Combine(instance.onCalculate, new TouchController.TouchAction(check));
	}

	private void OnDestroy()
	{
		TouchController instance = TouchController.instance;
		instance.onCalculate = (TouchController.TouchAction)Delegate.Remove(instance.onCalculate, new TouchController.TouchAction(check));
	}

	public void calculateAngle()
	{
		Vector3 vector = base.transform.position - base.transform.parent.position;
		vector.Normalize();
		angle = Mathf.Atan2(vector.y, 0f - vector.x);
		angle = 180f * angle / (float)Math.PI + 90f;
		if (angle < 0f)
		{
			angle += 360f;
		}
	}

	public void setLetter(string letter)
	{
		if (letter.Length > 1)
		{
		}
		l = letter;
		base.transform.Find("Text").GetComponent<Text>().text = l;
		Vector2 sizeDelta = GetComponent<RectTransform>().sizeDelta;
		range = sizeDelta.x * 0.5f * 0.9f;
		if (PlayerPrefsManager.GetLang() == "Arabic")
		{
			Vector2 sizeDelta2 = base.transform.GetComponent<RectTransform>().sizeDelta;
			float d = sizeDelta2.x / 100f;
			RectTransform component = base.transform.Find("Text").GetComponent<RectTransform>();
			LetterOffset letterOffset = CalculateLetterData.letterOffsets[letter];
			component.offsetMax = letterOffset.letterOffset * d;
			component.offsetMin = letterOffset.letterOffset * d;
			if (Application.platform == RuntimePlatform.Android)
			{
				component.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
			}
		}
	}

	private void check(Vector3 touch)
	{
		touch.z = 100f;
		Vector3 b = Camera.main.WorldToScreenPoint(base.transform.position);
		float num = (touch - b).magnitude * scale;
		if (!(num > range) && num < range)
		{
			bool narrow = num < range * 0.7f;
			if (MegaHintController.waiting)
			{
				MegaHintController.instance.letterClicked(this);
			}
			else
			{
				Writer.instance.letterTouch(this, narrow);
			}
		}
	}

	public void selected(bool s)
	{
		GameObject gameObject = base.transform.Find("Selected").gameObject;
		base.transform.Find("Selected").gameObject.SetActive(s);
		if (s)
		{
			VibrationManager.Haptic(HapticTypes.LightImpact);
			base.transform.Find("Text").GetComponent<Text>().color = GameController.InGameSelectedLetterColor;
		}
		else
		{
			base.transform.Find("Text").GetComponent<Text>().color = GameController.InGameLetterColor;
		}
		base.transform.Find("Selected").GetComponent<Image>().color = GameController.SelectedLetterBGColor;
	}

	public void openMegaHint()
	{
		if (!hint)
		{
			float num = 0.2f;
			hint = true;
			GameObject gameObject = base.transform.Find("Text").gameObject;
			Movements.instance.lerpColorTo(gameObject, GameController.WheelStrokeColor, num);
			Movements.instance.scale(gameObject, Vector3.one, Vector3.one * 1.3f, num * 0.5f);
			Movements.instance.scale(gameObject, Vector3.one * 1.3f, Vector3.one, num * 0.5f, num * 0.5f);
		}
	}

	public void closeMegaHint()
	{
		hint = false;
		base.transform.Find("Text").GetComponent<Text>().color = GameController.InGameLetterColor;
	}
}
