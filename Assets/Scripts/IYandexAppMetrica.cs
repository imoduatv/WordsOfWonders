using System;
using System.Collections.Generic;

public interface IYandexAppMetrica
{
	YandexAppMetricaConfig? ActivationConfig
	{
		get;
	}

	string LibraryVersion
	{
		get;
	}

	int LibraryApiLevel
	{
		get;
	}

	event ConfigUpdateHandler OnActivation;

	void ActivateWithConfiguration(YandexAppMetricaConfig config);

	void ResumeSession();

	void PauseSession();

	void ReportEvent(string message);

	void ReportEvent(string message, Dictionary<string, object> parameters);

	void ReportError(string condition, string stackTrace);

	void SetLocationTracking(bool enabled);

	void SetLocation(YandexAppMetricaConfig.Coordinates? coordinates);

	void SetUserProfileID(string userProfileID);

	void ReportUserProfile(YandexAppMetricaUserProfile userProfile);

	void ReportRevenue(YandexAppMetricaRevenue revenue);

	void SetStatisticsSending(bool enabled);

	void SendEventsBuffer();

	void RequestAppMetricaDeviceID(Action<string, YandexAppMetricaRequestDeviceIDError?> action);
}
