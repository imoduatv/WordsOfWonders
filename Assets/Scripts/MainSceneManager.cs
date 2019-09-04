using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneManager : MonoBehaviour
{
	private GameObject nullGameObject;

	private static bool isLocationTracking = true;

	private static bool isStatisticsSending = true;

	private PopUp popupWindow = new PopUp();

	private static int testCounter = 1;

	private static int eventCounter = 1;

	private void InitGUI()
	{
		GUI.skin.button.fontSize = 40;
		GUI.skin.textField.fontSize = 35;
		GUI.contentColor = Color.white;
		GUI.skin.label.fontSize = 40;
	}

	private void OnGUI()
	{
		InitGUI();
		popupWindow.onGUI();
		IYandexAppMetrica instance = AppMetrica.Instance;
		if (Button("Report Test"))
		{
			string text = "Test" + testCounter++;
			instance.ReportEvent(text);
			popupWindow.showPopup("Report: " + text);
		}
		if (Button("Send Event Immediately"))
		{
			string text2 = "Event" + eventCounter++;
			instance.ReportEvent(text2);
			instance.SendEventsBuffer();
			popupWindow.showPopup("Report: " + text2);
		}
		if (Button("Track Location Enabled: " + isLocationTracking))
		{
			isLocationTracking = !isLocationTracking;
			instance.SetLocationTracking(isLocationTracking);
		}
		if (Button("Send Statistics Enabled: " + isStatisticsSending))
		{
			isStatisticsSending = !isStatisticsSending;
			instance.SetStatisticsSending(isStatisticsSending);
		}
		if (Button("[CRASH] NullReference"))
		{
			nullGameObject.SendMessage(string.Empty);
		}
		if (Button("LOG AppMetrica DeviceID"))
		{
			instance.RequestAppMetricaDeviceID(delegate(string deviceId, YandexAppMetricaRequestDeviceIDError? error)
			{
				if (error.HasValue)
				{
					popupWindow.showPopup("Error: " + error);
				}
				else
				{
					popupWindow.showPopup("DeviceID: " + deviceId);
				}
			});
		}
		if (Button("LOG Library Version"))
		{
			popupWindow.showPopup("Version: " + instance.LibraryVersion);
		}
		if (Button("LOG Library API Level"))
		{
			popupWindow.showPopup("Level: " + instance.LibraryApiLevel);
		}
		if (Button("[SCENE] Load"))
		{
			SceneManager.LoadScene("AnotherScene");
		}
		if (Button("Exit"))
		{
			Application.Quit();
		}
	}

	private bool Button(string title)
	{
		return GUILayout.Button(title, GUILayout.Width(Screen.width), GUILayout.Height(Screen.height / 15));
	}
}
