using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RequestManager : MonoBehaviour
{
	public delegate void FetchCallback(string text);

	public static RequestManager instance;

	private const string domain = "http://www.wordloop.net/wordsofwonders/";

	private const string domain2 = "http://89.19.7.45/wordsofwonders/";

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
			Object.DontDestroyOnLoad(this);
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public Coroutine CreateRequest(string php, FetchCallback callbackFunc, Dictionary<string, string> dict)
	{
		return StartCoroutine(FetchFromNet(php, callbackFunc, dict));
	}

	private IEnumerator FetchFromNet(string php, FetchCallback callbackFunc, Dictionary<string, string> dict)
	{
		string url = "http://www.wordloop.net/wordsofwonders/" + php + ".php?";
		if (php == "redeem")
		{
			url = "http://89.19.7.45/wordsofwonders/" + php + ".php?";
		}
		int index = 0;
		if (dict != null)
		{
			foreach (KeyValuePair<string, string> item in dict)
			{
				if (index != 0)
				{
					string text = url;
					url = text + "&" + item.Key + "=" + item.Value;
				}
				else
				{
					url = url + item.Key + "=" + item.Value;
				}
				index++;
			}
		}
		WWW www = new WWW(url);
		yield return www;
		if (string.IsNullOrEmpty(www.error))
		{
			if (callbackFunc != null && !string.IsNullOrEmpty(www.text))
			{
				callbackFunc(www.text);
			}
		}
		else
		{
			yield return new WaitForSeconds(3f);
			StartCoroutine(FetchFromNet(php, callbackFunc, dict));
			callbackFunc("error");
		}
	}

	public void LogRequest()
	{
		if (PlayerPrefsManager.GetFBID() != string.Empty)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("fbid", PlayerPrefsManager.GetFBID());
			dictionary.Add("lang", PlayerPrefsManager.GetLang());
			dictionary.Add("level", PlayerPrefsManager.GetLevel().ToString());
			dictionary.Add("coin", PlayerPrefsManager.GetCoin().ToString());
			dictionary.Add("gallery", PlayerPrefs.GetString("gallerystring"));
			dictionary.Add("paidadv", PlayerPrefs.GetString("paidadventures"));
			dictionary.Add("advlevels", PlayerPrefs.GetString("hiddenlevellevel"));
			CreateRequest("log2", null, dictionary);
		}
	}

	public void LogRequestAfterLogin()
	{
		if (PlayerPrefsManager.GetFBID() != string.Empty)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("fbid", PlayerPrefsManager.GetFBID());
			dictionary.Add("lang", PlayerPrefsManager.GetLang());
			dictionary.Add("level", PlayerPrefsManager.GetLevel().ToString());
			dictionary.Add("coin", PlayerPrefsManager.GetCoin().ToString());
			dictionary.Add("gallery", PlayerPrefs.GetString("gallerystring"));
			dictionary.Add("paidadv", PlayerPrefs.GetString("paidadventures"));
			dictionary.Add("advlevels", PlayerPrefs.GetString("hiddenlevellevel"));
			dictionary.Add("shouldlogadv", "0");
			CreateRequest("log2", null, dictionary);
		}
	}

	public void GetLogRequest()
	{
		if (PlayerPrefsManager.GetFBID() != string.Empty)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("fbid", PlayerPrefsManager.GetFBID());
			CreateRequest("getlog2", GetLogCallback, dictionary);
		}
	}

	private void GetLogCallback(string response)
	{
		JsonData jsonData = JsonMapper.ToObject(response);
		int result = 0;
		if (int.TryParse(jsonData["A"].ToString(), out result))
		{
			PlayerPrefsManager.SetHiddenMenu(result);
		}
		int result2 = 0;
		if (int.TryParse(jsonData["L"].ToString(), out result2) && PlayerPrefsManager.GetLevel() < result2)
		{
			PlayerPrefsManager.SetLevel(result2);
			PlayerPrefsManager.SetBrilliance(result2);
			SceneManager.LoadScene("Menu");
		}
		if (!(jsonData["Active"].ToString() == "1"))
		{
			return;
		}
		string text = jsonData["P"].ToString();
		string text2 = jsonData["AL"].ToString();
		if (text != string.Empty)
		{
			string[] array = text.Split(',');
			foreach (string adventurePaid in array)
			{
				PlayerPrefsManager.SetAdventurePaid(adventurePaid);
			}
		}
		if (text2 != string.Empty)
		{
			string[] array2 = text2.Split(',');
			foreach (string text3 in array2)
			{
				string[] array3 = text3.Split('|');
				PlayerPrefsManager.SetHiddenLevel(int.Parse(array3[0]), int.Parse(array3[1]));
			}
		}
	}
}
