using UnityEngine;

public static class MMVibrationManager
{
	public static long LightDuration = 20L;

	public static long MediumDuration = 40L;

	public static long HeavyDuration = 80L;

	public static int LightAmplitude = 40;

	public static int MediumAmplitude = 120;

	public static int HeavyAmplitude = 255;

	private static int _sdkVersion = -1;

	private static long[] _successPattern = new long[4]
	{
		0L,
		LightDuration,
		LightDuration,
		HeavyDuration
	};

	private static int[] _successPatternAmplitude = new int[4]
	{
		0,
		LightAmplitude,
		0,
		HeavyAmplitude
	};

	private static long[] _warningPattern = new long[4]
	{
		0L,
		HeavyDuration,
		LightDuration,
		MediumDuration
	};

	private static int[] _warningPatternAmplitude = new int[4]
	{
		0,
		HeavyAmplitude,
		0,
		MediumAmplitude
	};

	private static long[] _failurePattern = new long[8]
	{
		0L,
		MediumDuration,
		LightDuration,
		MediumDuration,
		LightDuration,
		HeavyDuration,
		LightDuration,
		LightDuration
	};

	private static int[] _failurePatternAmplitude = new int[8]
	{
		0,
		MediumAmplitude,
		0,
		MediumAmplitude,
		0,
		HeavyAmplitude,
		0,
		LightAmplitude
	};

	private static AndroidJavaClass UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

	private static AndroidJavaObject CurrentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

	private static AndroidJavaObject AndroidVibrator = CurrentActivity.Call<AndroidJavaObject>("getSystemService", new object[1]
	{
		"vibrator"
	});

	private static AndroidJavaClass VibrationEffectClass;

	private static AndroidJavaObject VibrationEffect;

	private static int DefaultAmplitude;

	private static bool iOSHapticsInitialized = false;

	public static bool Android()
	{
		return true;
	}

	public static bool iOS()
	{
		return false;
	}

	public static void Vibrate()
	{
		if (Android())
		{
			AndroidVibrate(MediumDuration);
		}
		else if (iOS())
		{
			iOSTriggerHaptics(HapticTypes.MediumImpact);
		}
	}

	public static void Haptic(HapticTypes type)
	{
		if (Android())
		{
			switch (type)
			{
			case HapticTypes.Selection:
				AndroidVibrate(LightDuration, LightAmplitude);
				break;
			case HapticTypes.Success:
				AndroidVibrate(_successPattern, _successPatternAmplitude, -1);
				break;
			case HapticTypes.Warning:
				AndroidVibrate(_warningPattern, _warningPatternAmplitude, -1);
				break;
			case HapticTypes.Failure:
				AndroidVibrate(_failurePattern, _failurePatternAmplitude, -1);
				break;
			case HapticTypes.LightImpact:
				AndroidVibrate(LightDuration, LightAmplitude);
				break;
			case HapticTypes.MediumImpact:
				AndroidVibrate(MediumDuration, MediumAmplitude);
				break;
			case HapticTypes.HeavyImpact:
				AndroidVibrate(HeavyDuration, HeavyAmplitude);
				break;
			}
		}
		else if (iOS())
		{
			iOSTriggerHaptics(type);
		}
	}

	public static void AndroidVibrate(long milliseconds)
	{
		if (Android())
		{
			AndroidVibrator.Call("vibrate", milliseconds);
		}
	}

	public static void AndroidVibrate(long milliseconds, int amplitude)
	{
		if (Android())
		{
			if (AndroidSDKVersion() < 26)
			{
				AndroidVibrate(milliseconds);
				return;
			}
			VibrationEffectClassInitialization();
			VibrationEffect = VibrationEffectClass.CallStatic<AndroidJavaObject>("createOneShot", new object[2]
			{
				milliseconds,
				amplitude
			});
			AndroidVibrator.Call("vibrate", VibrationEffect);
		}
	}

	public static void AndroidVibrate(long[] pattern, int repeat)
	{
		if (Android())
		{
			if (AndroidSDKVersion() < 26)
			{
				AndroidVibrator.Call("vibrate", pattern, repeat);
				return;
			}
			VibrationEffectClassInitialization();
			VibrationEffect = VibrationEffectClass.CallStatic<AndroidJavaObject>("createWaveform", new object[2]
			{
				pattern,
				repeat
			});
			AndroidVibrator.Call("vibrate", VibrationEffect);
		}
	}

	public static void AndroidVibrate(long[] pattern, int[] amplitudes, int repeat)
	{
		if (Android())
		{
			if (AndroidSDKVersion() < 26)
			{
				AndroidVibrator.Call("vibrate", pattern, repeat);
				return;
			}
			VibrationEffectClassInitialization();
			VibrationEffect = VibrationEffectClass.CallStatic<AndroidJavaObject>("createWaveform", new object[3]
			{
				pattern,
				amplitudes,
				repeat
			});
			AndroidVibrator.Call("vibrate", VibrationEffect);
		}
	}

	public static void AndroidCancelVibrations()
	{
		if (Android())
		{
			AndroidVibrator.Call("cancel");
		}
	}

	private static void VibrationEffectClassInitialization()
	{
		if (VibrationEffectClass == null)
		{
			VibrationEffectClass = new AndroidJavaClass("android.os.VibrationEffect");
		}
	}

	public static int AndroidSDKVersion()
	{
		if (_sdkVersion == -1)
		{
			return _sdkVersion = int.Parse(SystemInfo.operatingSystem.Substring(SystemInfo.operatingSystem.IndexOf("-") + 1, 3));
		}
		return _sdkVersion;
	}

	private static void InstantiateFeedbackGenerators()
	{
	}

	private static void ReleaseFeedbackGenerators()
	{
	}

	private static void SelectionHaptic()
	{
	}

	private static void SuccessHaptic()
	{
	}

	private static void WarningHaptic()
	{
	}

	private static void FailureHaptic()
	{
	}

	private static void LightImpactHaptic()
	{
	}

	private static void MediumImpactHaptic()
	{
	}

	private static void HeavyImpactHaptic()
	{
	}

	public static void iOSInitializeHaptics()
	{
		if (iOS())
		{
			InstantiateFeedbackGenerators();
			iOSHapticsInitialized = true;
		}
	}

	public static void iOSReleaseHaptics()
	{
		if (iOS())
		{
			ReleaseFeedbackGenerators();
		}
	}

	public static bool HapticsSupported()
	{
		return false;
	}

	public static void iOSTriggerHaptics(HapticTypes type)
	{
		if (!iOS())
		{
			return;
		}
		if (!iOSHapticsInitialized)
		{
			iOSInitializeHaptics();
		}
		if (HapticsSupported())
		{
			switch (type)
			{
			case HapticTypes.Selection:
				SelectionHaptic();
				break;
			case HapticTypes.Success:
				SuccessHaptic();
				break;
			case HapticTypes.Warning:
				WarningHaptic();
				break;
			case HapticTypes.Failure:
				FailureHaptic();
				break;
			case HapticTypes.LightImpact:
				LightImpactHaptic();
				break;
			case HapticTypes.MediumImpact:
				MediumImpactHaptic();
				break;
			case HapticTypes.HeavyImpact:
				HeavyImpactHaptic();
				break;
			}
		}
	}

	public static string iOSSDKVersion()
	{
		return null;
	}
}
