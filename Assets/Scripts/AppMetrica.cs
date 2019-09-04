using UnityEngine;

public class AppMetrica : MonoBehaviour
{
	[SerializeField]
	private string ApiKey;

	[SerializeField]
	private bool ExceptionsReporting = true;

	[SerializeField]
	private uint SessionTimeoutSec = 10u;

	[SerializeField]
	private bool LocationTracking = true;

	[SerializeField]
	private bool Logs = true;

	[SerializeField]
	private bool HandleFirstActivationAsUpdate;

	[SerializeField]
	private bool StatisticsSending = true;

	private static bool _isInitialized = false;

	private bool _actualPauseStatus;

	private static IYandexAppMetrica _metrica = null;

	private static object syncRoot = new Object();

	public static IYandexAppMetrica Instance
	{
		get
		{
			if (_metrica == null)
			{
				lock (syncRoot)
				{
					if (_metrica == null && Application.platform == RuntimePlatform.Android)
					{
						_metrica = new YandexAppMetricaAndroid();
					}
					if (_metrica == null)
					{
						_metrica = new YandexAppMetricaDummy();
					}
				}
			}
			return _metrica;
		}
	}

	private void SetupMetrica()
	{
		YandexAppMetricaConfig yandexAppMetricaConfig = new YandexAppMetricaConfig(ApiKey);
		yandexAppMetricaConfig.SessionTimeout = (int)SessionTimeoutSec;
		yandexAppMetricaConfig.Logs = Logs;
		yandexAppMetricaConfig.HandleFirstActivationAsUpdate = HandleFirstActivationAsUpdate;
		yandexAppMetricaConfig.StatisticsSending = StatisticsSending;
		YandexAppMetricaConfig config = yandexAppMetricaConfig;
		config.LocationTracking = LocationTracking;
		if (LocationTracking)
		{
			Input.location.Start();
		}
		Instance.ActivateWithConfiguration(config);
		ProcessCrashReports();
	}

	private void Awake()
	{
		if (!_isInitialized)
		{
			_isInitialized = true;
			Object.DontDestroyOnLoad(base.gameObject);
			SetupMetrica();
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void Start()
	{
		Instance.ResumeSession();
	}

	private void OnEnable()
	{
		if (ExceptionsReporting)
		{
			Application.logMessageReceived += HandleLog;
		}
	}

	private void OnDisable()
	{
		if (ExceptionsReporting)
		{
			Application.logMessageReceived -= HandleLog;
		}
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		if (_actualPauseStatus != pauseStatus)
		{
			_actualPauseStatus = pauseStatus;
			if (pauseStatus)
			{
				Instance.PauseSession();
			}
			else
			{
				Instance.ResumeSession();
			}
		}
	}

	public void ProcessCrashReports()
	{
		if (ExceptionsReporting)
		{
			CrashReport[] reports = CrashReport.reports;
			CrashReport[] array = reports;
			foreach (CrashReport crashReport in array)
			{
				string stackTrace = $"Time: {crashReport.time}\nText: {crashReport.text}";
				Instance.ReportError("Crash", stackTrace);
				crashReport.Remove();
			}
		}
	}

	private void HandleLog(string condition, string stackTrace, LogType type)
	{
		if (type == LogType.Exception)
		{
			Instance.ReportError(condition, stackTrace);
		}
	}
}
