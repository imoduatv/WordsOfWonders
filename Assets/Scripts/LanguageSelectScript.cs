using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LanguageSelectScript : MonoBehaviour
{
	public Text title;

	public Text button;

	public Transform buttonHolder;

	public Transform langHolder;

	public GameObject languagePrefab;

	public static List<Language> languages;

	public static string alphabet;

	private void Awake()
	{
		ParseLangData();
	}

	private void Update()
	{
	}

	public void Init()
	{
		if (langHolder.childCount == 0)
		{
			CreateLangButtons();
		}
		if (PlayerPrefsManager.GetLang() == string.Empty)
		{
			if (SystemLanguageInList(Application.systemLanguage.ToString()))
			{
				PlayerPrefsManager.SetLang(Application.systemLanguage.ToString());
			}
			else
			{
				PlayerPrefsManager.SetLang(SystemLanguage.English.ToString());
			}
		}
		LanguageScript.ParseStrings();
		if (PlayerPrefsManager.GetLang() == "Hebrew")
		{
			UnityEngine.Object.Destroy(title.GetComponent<ArabicText>());
			UnityEngine.Object.Destroy(button.GetComponent<ArabicText>());
		}
		title.text = LanguageScript.SelectLanguageText;
		button.text = LanguageScript.PlayTextFixed;
		IEnumerator enumerator = buttonHolder.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform transform = (Transform)enumerator.Current;
				transform.Find("Frame").gameObject.SetActive(value: false);
				transform.Find("Text").GetComponent<Text>().color = Color.white;
				if (transform.name.ToLower() == PlayerPrefsManager.GetLang().ToLower())
				{
					transform.Find("Frame").gameObject.SetActive(value: true);
					transform.Find("Text").GetComponent<Text>().color = FugoUtils.HexToColor("09FF15");
					transform.Find("Frame").GetComponent<Image>().color = FugoUtils.HexToColor("09FF15");
				}
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
	}

	public void LangButtonOnClick(Transform t)
	{
		PlayerPrefsManager.SetLang(t.name);
		Init();
		t.Find("Frame").gameObject.SetActive(value: true);
	}

	public void PlayButtonOnClick()
	{
		Games.sections = null;
		PlayerPrefsManager.SetEarnedStar(-1);
		SceneManager.LoadScene("Menu");
	}

	public void CreateLangButtons()
	{
		IEnumerator enumerator = langHolder.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform transform = (Transform)enumerator.Current;
				UnityEngine.Object.Destroy(transform.gameObject);
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		int num = 0;
		float num2 = -207f;
		float num3 = 207f;
		float num4 = 245f;
		float num5 = -80f;
		foreach (Language language in languages)
		{
			Transform t = UnityEngine.Object.Instantiate(languagePrefab, langHolder).transform;
			t.localScale = Vector3.one;
			Vector3 localPosition = new Vector3(num2 + (float)(num % 3) * num3, num4 + (float)(num / 3) * num5, 0f);
			t.localPosition = localPosition;
			t.name = language.name;
			t.Find("Text").GetComponent<Text>().text = language.localname;
			t.GetComponent<Button>().onClick.AddListener(delegate
			{
				LangButtonOnClick(t);
			});
			num++;
			if (language.id == "AR" || language.id == "HE")
			{
				ArabicController.MakeForceArabic(t);
			}
		}
	}

	public static string GetLocalLanguage(string langname)
	{
		foreach (Language language in languages)
		{
			if (language.name.ToLower() == langname.ToLower())
			{
				return language.localname;
			}
		}
		return string.Empty;
	}

	public static string GetGlobalLanguage(string langname)
	{
		foreach (Language language in languages)
		{
			if (language.localname.ToLower() == langname.ToLower())
			{
				return language.name;
			}
		}
		return string.Empty;
	}

	public static void ParseLangData()
	{
		if (languages == null)
		{
			JsonData jsonData = JsonMapper.ToObject(Resources.Load<TextAsset>("LangFiles/alllangs").text);
			languages = new List<Language>();
			for (int i = 0; i < jsonData.Count; i++)
			{
				Language language = new Language();
				language.id = jsonData[i]["id"].ToString();
				language.name = jsonData[i]["Name"].ToString();
				language.localname = jsonData[i]["LocalName"].ToString();
				language.letters = jsonData[i]["Alphabet"].ToString();
				languages.Add(language);
			}
		}
	}

	private bool SystemLanguageInList(string syslang)
	{
		foreach (Language language in languages)
		{
			if (language.name.ToLower() == syslang.ToLower())
			{
				return true;
			}
		}
		return false;
	}

	public static void SetAlphabet()
	{
		Language language = null;
		foreach (Language language2 in languages)
		{
			if (language2.name == PlayerPrefsManager.GetLang())
			{
				alphabet = language2.letters;
				CalculateLetterData.alphabeth = language2.letters;
			}
		}
	}
}
