using UnityEngine;
using UnityEngine.UI;

public class ArabicController : MonoBehaviour
{
	public static ArabicController instance;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		if (!UISwapper.flipGame)
		{
		}
	}

	public static void MakeArabic(Transform taget)
	{
		if (!(PlayerPrefsManager.GetLang() != SystemLanguage.Arabic.ToString()) && !(PlayerPrefsManager.GetLang() == SystemLanguage.Hebrew.ToString()))
		{
			Text[] allComponents = taget.GetAllComponents<Text>();
			Text[] array = allComponents;
			foreach (Text text in array)
			{
				string text2 = text.text;
				ArabicText arabicText = null;
				arabicText = ((!(text.GetComponent<ArabicText>() == null)) ? text.GetComponent<ArabicText>() : text.transform.gameObject.AddComponent<ArabicText>());
				arabicText.Text = text2;
			}
		}
	}

	public static void MakeArabicMenu(Transform taget)
	{
		if (PlayerPrefsManager.GetLang() != SystemLanguage.Arabic.ToString() && PlayerPrefsManager.GetLang() != SystemLanguage.Hebrew.ToString())
		{
			return;
		}
		if (taget.GetComponent<Text>() != null)
		{
			string text = taget.GetComponent<Text>().text;
			ArabicText arabicText = null;
			if (taget.GetComponent<ArabicText>() != null)
			{
				UnityEngine.Object.Destroy(taget.GetComponent<ArabicText>());
			}
			arabicText = taget.transform.gameObject.AddComponent<ArabicText>();
			arabicText.Text = text;
		}
		Text[] allComponents = taget.GetAllComponents<Text>();
		Text[] array = allComponents;
		foreach (Text text2 in array)
		{
			if (!(text2.name == "RewardCoinText"))
			{
				string text3 = text2.text;
				ArabicText arabicText2 = null;
				if (text2.GetComponent<ArabicText>() != null)
				{
					UnityEngine.Object.Destroy(text2.GetComponent<ArabicText>());
				}
				arabicText2 = text2.transform.gameObject.AddComponent<ArabicText>();
				arabicText2.Text = text3;
			}
		}
	}

	public static void MakeForceArabic(Transform target)
	{
		Text[] allComponents = target.GetAllComponents<Text>();
		Text[] array = allComponents;
		foreach (Text text in array)
		{
			string text2 = text.text;
			ArabicText arabicText = null;
			if (text.GetComponent<ArabicText>() != null)
			{
				UnityEngine.Object.Destroy(text.GetComponent<ArabicText>());
			}
			arabicText = text.transform.gameObject.AddComponent<ArabicText>();
			arabicText.Text = text2;
		}
	}
}
