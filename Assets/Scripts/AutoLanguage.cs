using LitJson;
using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AutoLanguage : MonoBehaviour
{
	public delegate void OnTextChanged();

	public static AutoLanguage instance;

	public OnTextChanged onTextChanged;

	public static Dictionary<string, string> dict;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
			LanguageScript.ParseStrings();
			if (Application.isPlaying)
			{
				UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			}
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public static void Inıt()
	{
		dict = new Dictionary<string, string>();
	}

	private static void addToDictionary(JsonData lang)
	{
		if (dict != null)
		{
			dict.Clear();
		}
		dict = new Dictionary<string, string>();
		for (int i = 0; i < lang.Count; i++)
		{
			string key = lang[i]["stringKey"].ToString();
			string original = lang[i]["stringValue"].ToString();
			try
			{
				dict.Add(key, CorretArabic(original));
			}
			catch (Exception)
			{
			}
		}
	}

	public static string CorretArabic(string original)
	{
		return original;
	}

	public static string StartWordzText(string lang)
	{
		return "KELİME AVINA BAŞLA";
	}

	public static string GetLanguageAbbr()
	{
		switch (Application.systemLanguage.ToString())
		{
		case "Afrikaans":
			return "AF";
		case "Arabic":
			return "AR";
		case "Bulgarian":
			return "BU";
		case "Catalan":
			return "CA";
		case "Czech":
			return "CS";
		case "Danish":
			return "DA";
		case "Dutch":
			return "NL";
		case "English":
			return "EN";
		case "Finnish":
			return "FI";
		case "French":
			return "FR";
		case "German":
			return "DE";
		case "Greek":
			return "GR";
		case "Hebrew":
			return "HE";
		case "Italian":
			return "IT";
		case "Indonesian":
			return "ID";
		case "Japanese":
			return "JA";
		case "Korean":
			return "KO";
		case "Lithuanian":
			return "LT";
		case "Norwegian":
			return "NO";
		case "Polish":
			return "PL";
		case "Portuguese":
			return "PT";
		case "Romanian":
			return "RO";
		case "Russian":
			return "RU";
		case "SerboCroatian":
			return "HR";
		case "Serbo-Croatian":
			return "HR";
		case "Croatian":
			return "HR";
		case "Slovak":
			return "SK";
		case "Slovenian":
			return "SL";
		case "Spanish":
			return "ES";
		case "Swedish":
			return "SE";
		case "Turkish":
			return "TR";
		case "Ukrainian":
			return "UK";
		case "Hungarian":
			return "HU";
		default:
			return "EN";
		}
	}
}
