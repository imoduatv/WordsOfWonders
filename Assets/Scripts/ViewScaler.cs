using UnityEngine;

public class ViewScaler : MonoBehaviour
{
	public static float height;

	public float referenceHeight;

	public GameObject[] views;

	public float[] heights;

	private float lastHeight;

	private RectTransform parent;

	private void Start()
	{
	}

	public void scale()
	{
		parent = GetComponent<RectTransform>();
		lastHeight = 0f;
		for (int i = 0; i < views.Length; i++)
		{
			RectTransform component = views[i].GetComponent<RectTransform>();
			component.anchorMax = new Vector2(1f, 1f);
			component.anchorMin = new Vector2(0f, 1f);
			component.pivot = new Vector2(0.5f, 1f);
			float num = heights[i] / referenceHeight;
			component.anchoredPosition = Vector2.down * lastHeight;
			lastHeight += num * height;
			Vector2 sizeDelta = component.sizeDelta;
			sizeDelta.y = num * height;
			component.sizeDelta = sizeDelta;
		}
		RectTransform component2 = views[views.Length - 1].GetComponent<RectTransform>();
		component2.anchorMax = new Vector2(1f, 0f);
		component2.anchorMin = new Vector2(0f, 0f);
		component2.pivot = new Vector2(0.5f, 0f);
		float num2 = heights[views.Length - 1] / referenceHeight;
		component2.anchoredPosition = Vector2.zero;
		lastHeight += num2 * height;
		Vector2 sizeDelta2 = component2.sizeDelta;
		sizeDelta2.y = num2 * height;
		component2.sizeDelta = sizeDelta2;
	}
}
