using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SunScript : MonoBehaviour
{
	public Image shine;

	public Text text;

	public static SunScript instance;

	private void Awake()
	{
		instance = this;
	}

	private void OnEnable()
	{
		StopAllCoroutines();
		base.transform.localScale = Vector3.zero;
		StartCoroutine(FugoUtils.Scaler(Vector3.one, 0.3f, base.transform));
		FugoUtils.ChangeAlpha(shine, 0f);
		StartCoroutine(SunShineEffect(0.3f));
	}

	private IEnumerator SunShineEffect(float delay)
	{
		yield return new WaitForSeconds(delay);
		float time2 = UnityEngine.Random.Range(2f, 4f);
		float alpha2 = UnityEngine.Random.Range(0.7f, 1f);
		StartCoroutine(FugoUtils.FadeImage(alpha2, time2, shine));
		float wait = UnityEngine.Random.Range(0f, 0.5f);
		yield return new WaitForSeconds(time2 + wait);
		time2 = UnityEngine.Random.Range(2f, 4f);
		alpha2 = UnityEngine.Random.Range(0.3f, 0.5f);
		StartCoroutine(FugoUtils.FadeImage(alpha2, time2, shine));
		yield return new WaitForSeconds(time2 + wait);
		StartCoroutine(SunShineEffect(0f));
	}

	public void SetBrillianceText()
	{
		text.text = PlayerPrefsManager.GetBrilliance().ToString() + "\n<color=#88d5ff><size=25>" + LanguageScript.ExpeditionText + "</size></color>";
		if (PlayerPrefsManager.GetLang() == "Arabic" || PlayerPrefsManager.GetLang() == "Hebrew")
		{
			text.text = PlayerPrefsManager.GetBrilliance().ToString() + "\n\n" + LanguageScript.ExpeditionText;
		}
	}
}
