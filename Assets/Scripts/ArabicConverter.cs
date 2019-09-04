using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArabicConverter : MonoBehaviour
{
	public TextAsset resource;

	private List<string> input;

	private List<string> output;

	public Text text;

	public ArabicText arab;

	private void Start()
	{
		input = new List<string>();
		output = new List<string>();
		StartCoroutine(convert());
	}

	private IEnumerator convert()
	{
		yield return null;
		yield return null;
		yield return null;
		string[] arr = resource.text.Split('\n');
		string[] array = arr;
		foreach (string item2 in array)
		{
			input.Add(item2);
		}
		string o = string.Empty;
		yield return null;
		yield return null;
		foreach (string item in input)
		{
			yield return null;
			GetComponent<ArabicText>().Text = item;
			yield return null;
			yield return null;
			o = o + text.text + "\n";
		}
		UnityEngine.Debug.Log(o);
	}
}
