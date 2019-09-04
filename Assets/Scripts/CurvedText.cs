using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class CurvedText : Text
{
	public float radius = 2f;

	public float wrapAngle = 10f;

	public float scaleFactor = 100f;

	private float _radius = -1f;

	private float _scaleFactor = -1f;

	private float _circumference = -1f;

	private float circumference
	{
		get
		{
			if (_radius != radius || _scaleFactor != scaleFactor)
			{
				_circumference = (float)Math.PI * 2f * radius * scaleFactor;
				_radius = radius;
				_scaleFactor = scaleFactor;
			}
			return _circumference;
		}
	}

	protected override void OnPopulateMesh(VertexHelper vh)
	{
		base.OnPopulateMesh(vh);
		List<UIVertex> list = new List<UIVertex>();
		vh.GetUIVertexStream(list);
		for (int i = 0; i < list.Count; i++)
		{
			UIVertex value = list[i];
			float num = value.position.x / circumference;
			Vector3 a = Quaternion.Euler(0f, 0f, (0f - num) * 360f) * Vector3.up;
			value.position = a * radius * scaleFactor + a * value.position.y;
			value.position += Vector3.down * radius * scaleFactor;
			list[i] = value;
		}
		vh.AddUIVertexTriangleStream(list);
	}

	private void Update()
	{
		if (radius <= 0f)
		{
			radius = 0.001f;
		}
		if (scaleFactor <= 0f)
		{
			scaleFactor = 0.001f;
		}
		RectTransform rectTransform = base.rectTransform;
		float x = circumference * wrapAngle / 360f;
		Vector2 sizeDelta = base.rectTransform.sizeDelta;
		rectTransform.sizeDelta = new Vector2(x, sizeDelta.y);
	}
}
