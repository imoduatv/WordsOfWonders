using UnityEngine;

public class VibrationManager : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	public static void Haptic(HapticTypes type)
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer && false)
		{
			MMVibrationManager.Haptic(type);
		}
	}
}
