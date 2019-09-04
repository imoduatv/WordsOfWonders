public class YandexAppMetricaStringAttribute
{
	private const string AttributeName = "customString";

	private readonly string Key;

	public YandexAppMetricaStringAttribute(string key)
	{
		Key = key;
	}

	public YandexAppMetricaUserProfileUpdate WithValue(string value)
	{
		return new YandexAppMetricaUserProfileUpdate("customString", "withValue", Key, value);
	}

	public YandexAppMetricaUserProfileUpdate WithValueIfUndefined(string value)
	{
		return new YandexAppMetricaUserProfileUpdate("customString", "withValueIfUndefined", Key, value);
	}

	public YandexAppMetricaUserProfileUpdate WithValueReset()
	{
		return new YandexAppMetricaUserProfileUpdate("customString", "withValueReset", Key);
	}
}
