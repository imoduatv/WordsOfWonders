using System;
using UnityEngine;

public class NotificationManager : MonoBehaviour
{
	private static NotificationManager instance;

	public Action<string> OnNotificationClicked;

	public NotificationManager Instance => instance;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
			UnityEngine.Object.DontDestroyOnLoad(instance);
		}
		else
		{
			UnityEngine.Object.Destroy(this);
		}
	}

	private void OnEnable()
	{
		OnNotificationClicked = (Action<string>)Delegate.Combine(OnNotificationClicked, new Action<string>(NotificationClicked));
	}

	private void OnDisable()
	{
		OnNotificationClicked = (Action<string>)Delegate.Remove(OnNotificationClicked, new Action<string>(NotificationClicked));
	}

	public void OnNotificationClickedNative(string data)
	{
		if (OnNotificationClicked != null)
		{
			OnNotificationClicked(data);
		}
	}

	private void NotificationClicked(string data)
	{
		UnityEngine.Debug.Log("Notification with id " + data + " clicked");
	}
}
