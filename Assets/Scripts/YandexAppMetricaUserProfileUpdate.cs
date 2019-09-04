using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct YandexAppMetricaUserProfileUpdate
{
	public string AttributeName
	{
		get;
		private set;
	}

	public string MethodName
	{
		get;
		private set;
	}

	public string Key
	{
		get;
		private set;
	}

	public object[] Values
	{
		get;
		private set;
	}

	public YandexAppMetricaUserProfileUpdate(string attributeName, string methodName, string key, params object[] values)
	{
		AttributeName = attributeName;
		MethodName = methodName;
		Key = key;
		Values = values;
	}
}
