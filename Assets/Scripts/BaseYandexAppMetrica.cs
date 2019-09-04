using System;
using System.Collections.Generic;

public abstract class BaseYandexAppMetrica : IYandexAppMetrica
{
	private YandexAppMetricaConfig? _metricaConfig;

	public YandexAppMetricaConfig? ActivationConfig => _metricaConfig;

	public abstract string LibraryVersion
	{
		get;
	}

	public abstract int LibraryApiLevel
	{
		get;
	}

	public event ConfigUpdateHandler OnActivation;

	public virtual void ActivateWithConfiguration(YandexAppMetricaConfig config)
	{
		UpdateConfiguration(config);
	}

	private void UpdateConfiguration(YandexAppMetricaConfig config)
	{
		_metricaConfig = config;
		this.OnActivation?.Invoke(config);
	}

	public abstract void ResumeSession();

	public abstract void PauseSession();

	public abstract void ReportEvent(string message);

	public abstract void ReportEvent(string message, Dictionary<string, object> parameters);

	public abstract void ReportError(string condition, string stackTrace);

	public abstract void SetLocationTracking(bool enabled);

	public abstract void SetLocation(YandexAppMetricaConfig.Coordinates? coordinates);

	public abstract void SetUserProfileID(string userProfileID);

	public abstract void ReportUserProfile(YandexAppMetricaUserProfile userProfile);

	public abstract void ReportRevenue(YandexAppMetricaRevenue revenue);

	public abstract void SetStatisticsSending(bool enabled);

	public abstract void SendEventsBuffer();

	public abstract void RequestAppMetricaDeviceID(Action<string, YandexAppMetricaRequestDeviceIDError?> action);
}
