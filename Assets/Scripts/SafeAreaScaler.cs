using UnityEngine;

public class SafeAreaScaler : MonoBehaviour
{
	public static SafeAreaScaler instance;

	public static float scale;

	private static float adSize;

	public float height;

	public GameObject[] scales;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		calculateSafeArea();
		scale = 1f;
		float num = (float)Screen.width / (float)Screen.height;
		float num2 = 375f / 668f;
		GetComponent<ViewScaler>().scale();
		if (num < num2)
		{
			scale = num / num2;
			scaleothers();
		}
	}

	private void Update()
	{
	}

	public void calculateSafeArea()
	{
		RectTransform component = GetComponent<RectTransform>();
		Rect safeArea = Screen.safeArea;
		Vector2 size = safeArea.size;
		float y = size.y;
		float num = 1f;
		num = ((Screen.height > 720) ? (90f / (float)Screen.height) : (50f / (float)Screen.height));
		num *= 1.1f;
		y = ((PlayerPrefsManager.GetNoAd() != 0) ? (y * 0f) : (y * 0.1f));
		if (PlayerPrefsManager.GetNoAd() == 0)
		{
			safeArea.size -= new Vector2(0f, y);
			if (!FugoAdManager.isBannerAtTop)
			{
				safeArea.position += new Vector2(0f, y);
			}
		}
		component.sizeDelta = safeArea.size;
		component.anchoredPosition = safeArea.position;
		Vector2 size2 = safeArea.size;
		float num2 = size2.y * (1336f / (float)Screen.height);
		Vector2 position = safeArea.position;
		float num3 = position.y * (1336f / (float)Screen.height);
		float num4 = Screen.height;
		Vector2 size3 = safeArea.size;
		float num5 = num4 - size3.y;
		Vector2 position2 = safeArea.position;
		float num6 = num5 - position2.y;
		num6 *= 1336f / (float)Screen.height;
		component.offsetMin = new Vector2(0f, num3);
		component.offsetMax = new Vector2(0f, 0f - num6);
		ViewScaler.height = 1336f - num3 - num6;
	}

	public void scaleothers()
	{
		scale = 1f;
		float num = (float)Screen.width / (float)Screen.height;
		float num2 = 375f / 668f;
		if (num < num2)
		{
			scale = num / num2;
		}
		GameObject[] array = scales;
		foreach (GameObject gameObject in array)
		{
			gameObject.transform.localScale = Vector3.one * scale;
		}
	}
}
