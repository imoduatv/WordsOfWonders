using UnityEngine;

public class LineScript : MonoBehaviour
{
	private LineRenderer line;

	private void Awake()
	{
		line = GetComponent<LineRenderer>();
	}

	private void Update()
	{
		Vector2 v = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
		line.material.SetTextureOffset("_MainTex", new Vector2(Time.timeSinceLevelLoad * 4f, 0f));
		line.material.SetTextureScale("_MainTex", new Vector2(v.magnitude, 1f));
		line.SetPosition(0, v);
	}
}
