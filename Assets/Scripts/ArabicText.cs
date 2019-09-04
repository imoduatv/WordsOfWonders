using ArabicSupport;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ArabicText : MonoBehaviour
{
	public delegate void SendSignal();

	[Multiline]
	public string Text;

	public bool ShowTashkeel;

	public bool UseHinduNumbers;

	public static SendSignal FixAll;

	private Text txt;

	private string OldText;

	private int OldFontSize;

	private RectTransform rectTransform;

	private Vector2 OldDeltaSize;

	private bool OldEnabled;

	private List<RectTransform> OldRectTransformParents = new List<RectTransform>();

	private Vector2 OldScreenRect = new Vector2(Screen.width, Screen.height);

	public void Awake()
	{
		GetRectTransformParents(OldRectTransformParents);
	}

	public void Start()
	{
		txt = base.gameObject.GetComponent<Text>();
		rectTransform = GetComponent<RectTransform>();
		FixAll = (SendSignal)Delegate.Combine(FixAll, new SendSignal(FixArabicText));
	}

	public static void Fix()
	{
		Movements.instance.StartCoroutine(fixAllNextFrame());
	}

	public static IEnumerator fixAllNextFrame()
	{
		yield return null;
		if (FixAll != null)
		{
			FixAll();
		}
	}

	private void OnDestroy()
	{
		FixAll = (SendSignal)Delegate.Remove(FixAll, new SendSignal(FixArabicText));
	}

	private void GetRectTransformParents(List<RectTransform> rectTransforms)
	{
		rectTransforms.Clear();
		Transform parent = base.transform.parent;
		while (parent != null)
		{
			GameObject gameObject = parent.gameObject;
			RectTransform component = gameObject.GetComponent<RectTransform>();
			if ((bool)component)
			{
				rectTransforms.Add(component);
			}
			parent = parent.parent;
		}
	}

	private bool CheckRectTransformParentsIfChanged()
	{
		List<RectTransform> list = new List<RectTransform>();
		GetRectTransformParents(list);
		if (list.Count != OldRectTransformParents.Count)
		{
			return true;
		}
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i] != OldRectTransformParents[i])
			{
				return true;
			}
			if (!object.Equals(list[i], OldRectTransformParents[i]))
			{
				return true;
			}
		}
		return false;
	}

	public void Update()
	{
		FixTextForUI();
	}

	public void FixArabicText()
	{
		StartCoroutine(fixNextFrame());
	}

	public void ForceFix()
	{
		if (!(OldText == Text) || OldFontSize != txt.fontSize || !(OldDeltaSize == rectTransform.sizeDelta) || OldEnabled != txt.enabled || OldScreenRect.x != (float)Screen.width || OldScreenRect.y != (float)Screen.height || CheckRectTransformParentsIfChanged())
		{
			StartCoroutine(fixNextFrame());
		}
	}

	private IEnumerator fixNextFrame()
	{
		yield return null;
		FixTextForUI();
		OldText = Text;
		OldFontSize = txt.fontSize;
		OldDeltaSize = rectTransform.sizeDelta;
		OldEnabled = txt.enabled;
		OldScreenRect.x = Screen.width;
		OldScreenRect.y = Screen.height;
		GetRectTransformParents(OldRectTransformParents);
	}

	public void FixTextForUI()
	{
		if (!string.IsNullOrEmpty(Text))
		{
			string text = ArabicFixer.Fix(Text, ShowTashkeel, UseHinduNumbers);
			text = text.Replace("\r", string.Empty);
			string text2 = string.Empty;
			string[] array = text.Split('\n');
			txt.text = string.Empty;
			for (int i = 0; i < array.Length; i++)
			{
				string[] array2 = array[i].Split(' ');
				Array.Reverse(array2);
				txt.text = string.Join(" ", array2);
				Canvas.ForceUpdateCanvases();
				for (int j = 0; j < txt.cachedTextGenerator.lines.Count; j++)
				{
					UILineInfo uILineInfo = txt.cachedTextGenerator.lines[j];
					int startCharIdx = uILineInfo.startCharIdx;
					int num;
					if (j == txt.cachedTextGenerator.lines.Count - 1)
					{
						num = txt.text.Length;
					}
					else
					{
						UILineInfo uILineInfo2 = txt.cachedTextGenerator.lines[j + 1];
						num = uILineInfo2.startCharIdx;
					}
					int num2 = num;
					int length = num2 - startCharIdx;
					string[] array3 = txt.text.Substring(startCharIdx, length).Split(' ');
					Array.Reverse(array3);
					text2 = text2 + string.Join(" ", array3).Trim() + "\n";
				}
			}
			txt.text = text2.TrimEnd('\n');
		}
		else
		{
			txt.text = string.Empty;
		}
	}
}
