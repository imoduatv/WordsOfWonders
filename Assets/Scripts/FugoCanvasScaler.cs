using UnityEngine;
using UnityEngine.UI;

public class FugoCanvasScaler : MonoBehaviour
{
	public Fit fit;

	private float ratio;

	private float adSize = 100f;

	private const float referenceRatio = 375f / 668f;

	private void Awake()
	{
		calculateSafeArea();
	}

	public void calculateSafeArea()
	{
		Rect rect = safeArea();
		Transform transform = GameObject.Find("Canvas").transform;
		float num = (float)Screen.width / (float)Screen.height;
		if (num < 375f / 668f)
		{
			fit = Fit.width;
			transform.GetComponent<CanvasScaler>().matchWidthOrHeight = 0f;
		}
		else
		{
			fit = Fit.height;
			transform.GetComponent<CanvasScaler>().matchWidthOrHeight = 1f;
		}
		RectTransform component = GetComponent<RectTransform>();
		component.anchorMin = Vector2.zero * 0.5f;
		component.anchorMax = Vector2.zero * 0.5f;
		component.pivot = Vector2.zero * 0.5f;
		float d;
		if (fit == Fit.height)
		{
			Vector2 referenceResolution = transform.GetComponent<CanvasScaler>().referenceResolution;
			d = referenceResolution.y / (float)Screen.height;
			component.sizeDelta = rect.size * d;
		}
		else
		{
			Vector2 referenceResolution2 = transform.GetComponent<CanvasScaler>().referenceResolution;
			d = referenceResolution2.x / (float)Screen.width;
			component.sizeDelta = rect.size * d;
		}
		component.anchoredPosition = new Vector2(rect.x, rect.y) * d;
		if (fit == Fit.height)
		{
			Vector2 sizeDelta = component.sizeDelta;
			float y = sizeDelta.y;
			Vector2 referenceResolution3 = transform.GetComponent<CanvasScaler>().referenceResolution;
			ratio = y / referenceResolution3.y;
		}
		else
		{
			Vector2 sizeDelta2 = component.sizeDelta;
			float x = sizeDelta2.x;
			Vector2 referenceResolution4 = transform.GetComponent<CanvasScaler>().referenceResolution;
			ratio = x / referenceResolution4.x;
		}
		for (int i = 0; i < base.transform.childCount; i++)
		{
		}
	}

	private void scaleUnit(RectTransform r)
	{
		ratio = 0.9f;
		r.transform.localScale *= ratio;
		Vector2 anchoredPosition = r.anchoredPosition;
		if (fit == Fit.height)
		{
		}
		anchoredPosition.y *= ratio;
		anchoredPosition.x *= ratio;
		r.anchoredPosition = anchoredPosition;
		Vector2 anchorMin = r.anchorMin;
		if (anchorMin.x == 0f)
		{
			Vector2 anchorMax = r.anchorMax;
			if (anchorMax.x == 1f)
			{
				Vector2 anchorMin2 = r.anchorMin;
				Vector2 anchorMax2 = r.anchorMax;
				anchorMin2.x = 0.5f;
				anchorMax2.x = 0.5f;
				r.anchorMin = anchorMin2;
				r.anchorMax = anchorMax2;
				Vector2 sizeDelta = r.sizeDelta;
				Vector2 sizeDelta2 = r.transform.parent.GetComponent<RectTransform>().sizeDelta;
				sizeDelta.x = sizeDelta2.x / ratio;
				r.sizeDelta = sizeDelta;
			}
		}
		Vector2 anchorMin3 = r.anchorMin;
		if (anchorMin3.y == 0f)
		{
			Vector2 anchorMax3 = r.anchorMax;
			if (anchorMax3.y == 1f)
			{
				Vector2 anchorMin4 = r.anchorMin;
				Vector2 anchorMax4 = r.anchorMax;
				anchorMin4.y = 0.5f;
				anchorMax4.y = 0.5f;
				r.anchorMin = anchorMin4;
				r.anchorMax = anchorMax4;
				Vector2 sizeDelta3 = r.sizeDelta;
				Vector2 sizeDelta4 = r.transform.parent.GetComponent<RectTransform>().sizeDelta;
				sizeDelta3.y = sizeDelta4.y / ratio;
				r.sizeDelta = sizeDelta3;
			}
		}
	}

	private Rect safeArea()
	{
		Rect safeArea = Screen.safeArea;
		if (PlayerPrefsManager.GetNoAd() == 0)
		{
			Vector2 size = safeArea.size;
			float y = size.y;
			float num = 1f;
			num = ((Screen.height > 720) ? (90f / (float)Screen.height) : (50f / (float)Screen.height));
			num *= 1.1f;
			y = ((PlayerPrefsManager.GetNoAd() != 0) ? (y * 0f) : (y * 0.15f));
			y *= 110f / 153f;
			Vector2 size2 = safeArea.size;
			size2.y -= y;
			safeArea.size = size2;
			safeArea.position += Vector2.up * y;
		}
		return safeArea;
	}
}
