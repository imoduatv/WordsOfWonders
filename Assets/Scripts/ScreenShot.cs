using UnityEngine;

public class ScreenShot : MonoBehaviour
{
	public static ScreenShot instance;

	private void Start()
	{
		if (instance == null)
		{
			instance = this;
			Object.DontDestroyOnLoad(base.transform.gameObject);
		}
		else
		{
			UnityEngine.Object.Destroy(base.transform.gameObject);
		}
	}

	private void Update()
	{
		if (UnityEngine.Input.GetKeyUp(KeyCode.H))
		{
			ScreenCapture.CaptureScreenshot(Screen.width + "x" + Screen.height + " - " + Time.time.ToString() + ".jpg");
		}
	}
}
