using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Line : MonoBehaviour
{
	public bool active;

	public static List<Line> lines;

	private static float thickness = 20f;

	private static float scale = 1f;

	private RectTransform rect;

	private void Awake()
	{
		active = true;
		rect = GetComponent<RectTransform>();
		scale = 1336f / (float)Screen.height;
		Vector2 sizeDelta = rect.sizeDelta;
		sizeDelta.y = thickness;
		rect.sizeDelta = sizeDelta;
		GetComponent<Image>().color = WheelController.gameColor;
		Transform transform = base.transform;
		Vector3 position = base.transform.position;
		float x = position.x;
		Vector3 position2 = base.transform.position;
		transform.position = new Vector3(x, position2.y, 0f);
	}

	private void Update()
	{
		if (active)
		{
			calculateTouch();
		}
	}

	private void calculateTouch()
	{
		Vector3 mousePosition = UnityEngine.Input.mousePosition;
		mousePosition.z = 100f;
		Vector3 b = Camera.main.WorldToScreenPoint(base.transform.position);
		Vector3 a = mousePosition - b;
		a *= scale;
		Vector2 sizeDelta = rect.sizeDelta;
		float d = Mathf.Atan2(a.y, a.x);
		sizeDelta.x = a.magnitude * 0.85f;
		sizeDelta.y = thickness;
		rect.sizeDelta = sizeDelta;
		rect.localRotation = Quaternion.Euler(Vector3.forward * d * 180f / (float)Math.PI);
	}

	public void calculateTouch(Vector3 touch)
	{
		touch = Camera.main.WorldToScreenPoint(touch);
		Vector3 b = Camera.main.WorldToScreenPoint(base.transform.position);
		b.z = 0f;
		Vector3 a = touch - b;
		a *= scale;
		Vector2 sizeDelta = rect.sizeDelta;
		float d = Mathf.Atan2(a.y, a.x);
		sizeDelta.x = a.magnitude - WheelController.letterSize * 0.4f;
		sizeDelta.y = thickness;
		rect.sizeDelta = sizeDelta;
		rect.localRotation = Quaternion.Euler(Vector3.forward * d * 180f / (float)Math.PI);
	}

	public void disable()
	{
		active = false;
		UnityEngine.Object.Destroy(this);
	}
}
