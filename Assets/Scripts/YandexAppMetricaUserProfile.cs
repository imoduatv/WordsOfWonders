using System.Collections.Generic;

public class YandexAppMetricaUserProfile
{
	private readonly List<YandexAppMetricaUserProfileUpdate> Updates = new List<YandexAppMetricaUserProfileUpdate>();

	public List<YandexAppMetricaUserProfileUpdate> GetUserProfileUpdates()
	{
		return new List<YandexAppMetricaUserProfileUpdate>(Updates);
	}

	public YandexAppMetricaUserProfile Apply(YandexAppMetricaUserProfileUpdate update)
	{
		Updates.Add(update);
		return this;
	}

	public YandexAppMetricaUserProfile ApplyFromArray(List<YandexAppMetricaUserProfileUpdate> updates)
	{
		Updates.AddRange(updates);
		return this;
	}
}
