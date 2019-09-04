public class YandexAppMetricaBooleanAttribute
{
	private const string AttributeName = "customBoolean";

	private readonly string Key;

	public YandexAppMetricaBooleanAttribute(string key)
	{
		Key = key;
	}

	public YandexAppMetricaUserProfileUpdate WithValue(bool value)
	{
		return new YandexAppMetricaUserProfileUpdate("customBoolean", "withValue", Key, value);
	}

	public YandexAppMetricaUserProfileUpdate WithValueIfUndefined(bool value)
	{
		return new YandexAppMetricaUserProfileUpdate("customBoolean", "withValueIfUndefined", Key, value);
	}

	public YandexAppMetricaUserProfileUpdate WithValueReset()
	{
		return new YandexAppMetricaUserProfileUpdate("customBoolean", "withValueReset", Key);
	}
}
