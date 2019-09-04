public class YandexAppMetricaNotificationsEnabledAttribute
{
	private const string AttributeName = "notificationsEnabled";

	public YandexAppMetricaUserProfileUpdate WithValue(bool value)
	{
		return new YandexAppMetricaUserProfileUpdate("notificationsEnabled", "withValue", null, value);
	}

	public YandexAppMetricaUserProfileUpdate WithValueReset()
	{
		return new YandexAppMetricaUserProfileUpdate("notificationsEnabled", "withValueReset", null);
	}
}
