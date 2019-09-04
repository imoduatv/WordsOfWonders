using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class IText : MonoBehaviour
{
	public FontType fontType;

	private Text text;

	private bool subscribed;

	private bool textSpaced;

	private void Start()
	{
		Subscribe();
	}

	private void OnEnable()
	{
		if (TextController.instance != null)
		{
			Subscribe();
		}
	}

	private void OnDestroy()
	{
		TextController instance = TextController.instance;
		instance.SetTextProperties = (TextController.TextDelegate)Delegate.Remove(instance.SetTextProperties, new TextController.TextDelegate(SetText));
		subscribed = false;
	}

	public void SetText()
	{
		if (ThemeManager.theme == 1)
		{
			if (text == null)
			{
				text = GetComponent<Text>();
			}
			if (fontType == FontType.LIGHT)
			{
				text.font = TextController.instance.light;
				StartCoroutine(AddSpaces());
			}
			else if (fontType == FontType.REGULAR)
			{
				text.font = TextController.instance.regular;
			}
			else if (fontType == FontType.SEMI)
			{
				text.font = TextController.instance.semi;
			}
			else if (fontType == FontType.LIGTHNOSPACE)
			{
				text.font = TextController.instance.light;
			}
		}
	}

	private void Subscribe()
	{
		if (!subscribed)
		{
			subscribed = false;
			textSpaced = false;
			subscribed = true;
			TextController instance = TextController.instance;
			instance.SetTextProperties = (TextController.TextDelegate)Delegate.Combine(instance.SetTextProperties, new TextController.TextDelegate(SetText));
			SetText();
		}
	}

	private IEnumerator AddSpaces()
	{
		yield return null;
		if (textSpaced)
		{
			yield break;
		}
		textSpaced = true;
		string text = GetComponent<Text>().text;
		string text2 = string.Empty;
		bool flag = false;
		string text3 = text;
		foreach (char c in text3)
		{
			switch (c)
			{
			case '<':
				flag = true;
				break;
			case '>':
				flag = false;
				break;
			}
			text2 += c;
			if (!flag && c != '>')
			{
				text2 += "\u00a0";
			}
		}
		GetComponent<Text>().text = text2;
		if (base.name == "SectionHeader")
		{
			UnityEngine.Debug.Log("spaced");
		}
	}
}
