public class YandexAppMetricaGenderAttribute
{
	public enum Gender
	{
		MALE,
		FEMALE,
		OTHER
	}

	private const string AttributeName = "gender";

	public YandexAppMetricaUserProfileUpdate WithValue(Gender value)
	{
		return new YandexAppMetricaUserProfileUpdate("gender", "withValue", null, value.ToString());
	}

	public YandexAppMetricaUserProfileUpdate WithValueReset()
	{
		return new YandexAppMetricaUserProfileUpdate("gender", "withValueReset", null);
	}
}
