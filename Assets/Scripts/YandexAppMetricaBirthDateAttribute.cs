using System;

public class YandexAppMetricaBirthDateAttribute
{
	private const string AttributeName = "birthDate";

	public YandexAppMetricaUserProfileUpdate WithAge(int age)
	{
		return new YandexAppMetricaUserProfileUpdate("birthDate", "withAge", null, age);
	}

	public YandexAppMetricaUserProfileUpdate WithBirthDate(DateTime date)
	{
		return WithBirthDate(date.Year, date.Month, date.Day);
	}

	public YandexAppMetricaUserProfileUpdate WithBirthDate(int year)
	{
		return new YandexAppMetricaUserProfileUpdate("birthDate", "withBirthDate", null, year);
	}

	public YandexAppMetricaUserProfileUpdate WithBirthDate(int year, int month)
	{
		return new YandexAppMetricaUserProfileUpdate("birthDate", "withBirthDate", null, year, month);
	}

	public YandexAppMetricaUserProfileUpdate WithBirthDate(int year, int month, int day)
	{
		return new YandexAppMetricaUserProfileUpdate("birthDate", "withBirthDate", null, year, month, day);
	}

	public YandexAppMetricaUserProfileUpdate WithValueReset()
	{
		return new YandexAppMetricaUserProfileUpdate("birthDate", "withValueReset", null);
	}
}
