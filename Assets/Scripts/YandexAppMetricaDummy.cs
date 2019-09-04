using System;
using System.Collections.Generic;

public class YandexAppMetricaDummy : BaseYandexAppMetrica
{
	public override string LibraryVersion => null;

	public override int LibraryApiLevel => 0;

	public override void ActivateWithConfiguration(YandexAppMetricaConfig config)
	{
	}

	public override void ResumeSession()
	{
	}

	public override void PauseSession()
	{
	}

	public override void ReportEvent(string message)
	{
	}

	public override void ReportEvent(string message, Dictionary<string, object> parameters)
	{
	}

	public override void ReportError(string condition, string stackTrace)
	{
	}

	public override void SetLocationTracking(bool enabled)
	{
	}

	public override void SetLocation(YandexAppMetricaConfig.Coordinates? coordinates)
	{
	}

	public override void SetUserProfileID(string userProfileID)
	{
	}

	public override void ReportUserProfile(YandexAppMetricaUserProfile userProfile)
	{
	}

	public override void ReportRevenue(YandexAppMetricaRevenue revenue)
	{
	}

	public override void SetStatisticsSending(bool enabled)
	{
	}

	public override void SendEventsBuffer()
	{
	}

	public override void RequestAppMetricaDeviceID(Action<string, YandexAppMetricaRequestDeviceIDError?> action)
	{
	}
}
