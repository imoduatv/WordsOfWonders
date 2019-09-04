public class YandexAppMetricaNameAttribute
{
	private const string AttributeName = "name";

	public YandexAppMetricaUserProfileUpdate WithValue(string value)
	{
		return new YandexAppMetricaUserProfileUpdate("name", "withValue", null, value);
	}

	public YandexAppMetricaUserProfileUpdate WithValueReset()
	{
		return new YandexAppMetricaUserProfileUpdate("name", "withValueReset", null);
	}
}
