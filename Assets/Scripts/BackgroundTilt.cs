using UnityEngine;

public class BackgroundTilt : MonoBehaviour
{
	public float speed;

	private RectTransform rect;

	private float range;

	private void Start()
	{
		Vector2 sizeDelta = base.transform.parent.GetComponent<RectTransform>().sizeDelta;
		range = 1135.6f - sizeDelta.x;
		range *= 0.5f;
		rect = GetComponent<RectTransform>();
	}

	private void Update()
	{
		tilt();
	}

	private void tilt()
	{
		if (SystemInfo.supportsGyroscope)
		{
			Vector2 vector = rect.anchoredPosition;
			Vector2 a = vector;
			Vector2 right = Vector2.right;
			Vector3 rotationRate = Input.gyro.rotationRate;
			vector = a - right * rotationRate.y * speed * Time.deltaTime;
			vector.x = Mathf.Clamp(vector.x, 0f - range, range);
			rect.anchoredPosition = vector;
		}
	}
}
