using UnityEngine;

public class PopUp
{
	private bool isPopupNeeded;

	private string popupText;

	public void onGUI()
	{
		if (isPopupNeeded)
		{
			GUILayout.Window(0, new Rect(Screen.width / 2 - 130, Screen.height / 2 - 65, 300f, 150f), showGUI, string.Empty);
		}
	}

	public void showPopup(string text)
	{
		popupText = text;
		isPopupNeeded = true;
	}

	private void showGUI(int windowID)
	{
		GUILayout.Label(popupText);
		if (GUILayout.Button("OK"))
		{
			isPopupNeeded = false;
		}
	}
}
