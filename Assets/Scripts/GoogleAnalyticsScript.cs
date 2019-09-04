using com.adjust.sdk;
using UnityEngine;

public class GoogleAnalyticsScript : MonoBehaviour
{
	public static GoogleAnalyticsScript instance;

	private const string trackingCode = "UA-125812104-1";

	private GoogleAnalyticsV4 ga;

	private bool shouldInit;

	private void Awake()
	{
		if (PlayerPrefsManager.GetGARandom() % 100 == 0)
		{
			shouldInit = true;
		}
		if (instance == null)
		{
			instance = this;
			Init();
		}
		else if (instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		Object.DontDestroyOnLoad(base.gameObject);
	}

	private void Init()
	{
		if (shouldInit)
		{
			ga = GetComponent<GoogleAnalyticsV4>();
			ga.androidTrackingCode = "UA-125812104-1";
			ga.IOSTrackingCode = "UA-125812104-1";
			ga.otherTrackingCode = "UA-125812104-1";
			ga.productName = Application.productName;
			ga.bundleIdentifier = Application.identifier;
			ga.bundleVersion = Application.version;
			ga.logLevel = GoogleAnalyticsV4.DebugMode.VERBOSE;
			ga.StartSession();
		}
	}

	public void LogScreen(string scene)
	{
		if (shouldInit)
		{
			ga.LogScreen(scene);
		}
	}

	public void LogEvent(string type, string action, string description, long value)
	{
		if (shouldInit)
		{
			ga.LogEvent(type, action, description, value);
			if (type == "Screen")
			{
				LogScreen(action);
			}
		}
	}

	public void AdjustEvent(string token)
	{
		AdjustEvent adjustEvent = new AdjustEvent(token);
		Adjust.trackEvent(adjustEvent);
	}

	public void AdjustPurcaseEvent(string tid, double value, string currency)
	{
		AdjustEvent adjustEvent = new AdjustEvent("fen452");
		adjustEvent.setRevenue(value, currency);
		adjustEvent.setTransactionId(tid);
		Adjust.trackEvent(adjustEvent);
	}
}
