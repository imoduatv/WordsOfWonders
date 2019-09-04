public class MoPub : MoPubAndroid
{
	private static string _sdkName;

	public static string SdkName => _sdkName ?? (_sdkName = MoPubAndroid.GetSdkName().Replace("+unity", string.Empty));
}
