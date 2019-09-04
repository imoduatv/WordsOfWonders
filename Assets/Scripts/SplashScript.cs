using UnityEngine;

public class SplashScript : MonoBehaviour
{
	public GameObject splash;

	public static bool splashShown = true;

	public static float splashTime = 1.5f;

	private float timer;

	private void Awake()
	{
		if (!splashShown)
		{
			timer = splashTime;
			splash.SetActive(value: true);
		}
	}

	private void Update()
	{
		if (!splashShown)
		{
			timer -= Time.deltaTime;
			if (timer <= 0f)
			{
				splashShown = true;
				splash.SetActive(value: false);
			}
		}
	}
}
