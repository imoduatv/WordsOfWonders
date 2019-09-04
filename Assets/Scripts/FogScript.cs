using System.Collections;
using UnityEngine;

public class FogScript : MonoBehaviour
{
	private float speed;

	private float endPoint;

	private Vector3 pos;

	private RectTransform rt;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void Move(Vector3 initalPos, float speed, float endPoint)
	{
		this.speed = speed;
		this.endPoint = endPoint;
		rt = base.transform.GetComponent<RectTransform>();
		rt.anchoredPosition = initalPos;
		StartCoroutine(MoveThread());
	}

	private IEnumerator MoveThread()
	{
		while (true)
		{
			pos = rt.anchoredPosition;
			if (Mathf.Abs(pos.x) >= Mathf.Abs(endPoint))
			{
				pos.x = 0f - endPoint;
			}
			pos.x += speed * Time.deltaTime;
			rt.anchoredPosition = pos;
			yield return null;
		}
	}
}
