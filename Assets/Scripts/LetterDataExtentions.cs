using System;
using UnityEngine;
using UnityEngine.UI;

public static class LetterDataExtentions
{
	public static void SetText(this GameObject obj, string s)
	{
		UnityEngine.Object.Destroy(obj.GetComponent<ContentSizeFitter>());
		obj.GetComponent<Text>().text = s;
		Vector2 sizeDelta = obj.GetComponent<RectTransform>().sizeDelta;
		try
		{
			sizeDelta.x = CalculateLetterData.letterData[s];
		}
		catch (Exception)
		{
			sizeDelta.x = CalculateLetterData.maxSize;
		}
		obj.GetComponent<RectTransform>().sizeDelta = sizeDelta;
	}
}
