using System;
using UnityEngine;

public class DebugHand : MonoBehaviour
{
	public static DebugHand instance;

	private bool active;

	private float offsetRatio;

	private Vector3 lastTouchPosition;

	private void Awake()
	{
		active = false;
		instance = this;
		offsetRatio = 0f;
	}

	private void Start()
	{
		if (PlayerPrefsManager.GetTrackerColor() != string.Empty || true)
		{
			UnityEngine.Debug.Log("video hand");
			try
			{
				active = true;
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.Log("hand crash  " + ex.ToString());
			}
		}
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			lastTouchPosition = UnityEngine.Input.mousePosition;
		}
		if (Input.GetMouseButton(0))
		{
			offsetRatio += 20f * Time.deltaTime;
			lastTouchPosition = UnityEngine.Input.mousePosition;
		}
		else
		{
			offsetRatio -= 20f * Time.deltaTime;
		}
		try
		{
			followTouch();
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.Log("touch crash  " + ex.ToString());
		}
		offsetRatio = Mathf.Clamp01(offsetRatio);
	}

	private void followTouch()
	{
		if (active)
		{
			Vector3 position = lastTouchPosition;
			Vector3 vector = Camera.main.ScreenToWorldPoint(position);
			vector.z = 0f;
			vector = Vector3.Lerp(base.transform.parent.position, vector, offsetRatio);
			base.transform.position = vector;
		}
	}
}
