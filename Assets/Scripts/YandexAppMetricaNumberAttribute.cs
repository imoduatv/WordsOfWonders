public class YandexAppMetricaNumberAttribute
{
	private const string AttributeName = "customNumber";

	private readonly string Key;

	public YandexAppMetricaNumberAttribute(string key)
	{
		Key = key;
	}

	public YandexAppMetricaUserProfileUpdate WithValue(double value)
	{
		return new YandexAppMetricaUserProfileUpdate("customNumber", "withValue", Key, value);
	}

	public YandexAppMetricaUserProfileUpdate WithValueIfUndefined(double value)
	{
		return new YandexAppMetricaUserProfileUpdate("customNumber", "withValueIfUndefined", Key, value);
	}

	public YandexAppMetricaUserProfileUpdate WithValueReset()
	{
		return new YandexAppMetricaUserProfileUpdate("customNumber", "withValueReset", Key);
	}
}
