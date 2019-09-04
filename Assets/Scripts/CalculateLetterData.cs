using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalculateLetterData : MonoBehaviour
{
	public static string alphabeth;

	public static float maxSize;

	public static Dictionary<string, float> letterData;

	public static Dictionary<string, LetterOffset> letterOffsets;

	public GameObject letterPrefab;

	public Transform holder;

	private void Start()
	{
		Movements.instance.executeWithDelay((Movements.Execute)init, 0.1f);
	}

	private void init()
	{
		maxSize = 0f;
		letterData = new Dictionary<string, float>();
		LanguageSelectScript.SetAlphabet();
		calculateData();
		parseLetterOffsets();
	}

	private void parseLetterOffsets()
	{
		TextAsset textAsset = Resources.Load<TextAsset>("LangFiles/arabicoffsets");
		JsonData jsonData = JsonMapper.ToObject(textAsset.text);
		letterOffsets = new Dictionary<string, LetterOffset>();
		for (int i = 0; i < jsonData.Count; i++)
		{
			LetterOffset letterOffset = new LetterOffset();
			letterOffset.letter = jsonData[i]["Letter"].ToString();
			Vector2 zero = Vector2.zero;
			zero.x = float.Parse(jsonData[i]["CellOffsetX"].ToString());
			zero.y = float.Parse(jsonData[i]["CellOffsetY"].ToString());
			zero.y *= -1f;
			letterOffset.cellOffset = zero;
			Vector2 zero2 = Vector2.zero;
			zero2.x = float.Parse(jsonData[i]["LetterOffsetX"].ToString());
			zero2.y = float.Parse(jsonData[i]["LetterOffsetY"].ToString());
			zero2.y *= -1f;
			letterOffset.letterOffset = zero2;
			letterOffset.calculateOffsets();
			letterOffsets.Add(letterOffset.letter, letterOffset);
		}
	}

	private void calculateData()
	{
		StartCoroutine(calculating());
	}

	private IEnumerator calculating()
	{
		alphabeth += "ÉÈ";
		for (int i = 0; i < alphabeth.Length; i++)
		{
			string text = alphabeth[i].ToString();
			GameObject gameObject = UnityEngine.Object.Instantiate(letterPrefab);
			gameObject.transform.SetParent(holder);
			gameObject.transform.ResetTransform();
			gameObject.GetComponent<Text>().text = text;
		}
		yield return null;
		yield return null;
		yield return null;
		for (int j = 0; j < alphabeth.Length; j++)
		{
			Transform child = holder.GetChild(j);
			string key = child.GetComponent<Text>().text.ToString();
			Vector2 sizeDelta = child.GetComponent<RectTransform>().sizeDelta;
			float x = sizeDelta.x;
			maxSize = Mathf.Max(x, maxSize);
			try
			{
				if (!letterData.ContainsKey(key))
				{
					letterData.Add(key, x);
				}
			}
			catch (Exception)
			{
			}
		}
		yield return null;
		UnityEngine.Object.Destroy(holder.gameObject);
	}
}
