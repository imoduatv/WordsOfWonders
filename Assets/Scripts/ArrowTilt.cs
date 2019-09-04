using System;
using System.Collections;
using UnityEngine;

public class ArrowTilt : MonoBehaviour
{
	private void Start()
	{
		StartCoroutine(tilt());
	}

	private void Update()
	{
	}

	private IEnumerator tilt()
	{
		float animTime = 3f;
		float t = 0f;
		while (t < animTime)
		{
			float par = t / animTime;
			base.transform.localPosition += base.transform.right * Mathf.Cos(3f * par * 2f * (float)Math.PI) * 0.1f;
			t += Time.deltaTime;
			yield return null;
		}
		StartCoroutine(tilt());
	}
}
