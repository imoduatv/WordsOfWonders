using System.Collections.Generic;
using UnityEngine;

public static class YandexAppMetricaExtensionsAndroid
{
	public static AndroidJavaObject ToAndroidAppMetricaConfig(this YandexAppMetricaConfig self)
	{
		AndroidJavaObject androidJavaObject = null;
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.yandex.metrica.YandexMetricaConfig"))
		{
			AndroidJavaObject androidJavaObject2 = androidJavaClass.CallStatic<AndroidJavaObject>("newConfigBuilder", new object[1]
			{
				self.ApiKey
			});
			if (self.Location.HasValue)
			{
				YandexAppMetricaConfig.Coordinates value = self.Location.Value;
				androidJavaObject2.Call<AndroidJavaObject>("withLocation", new object[1]
				{
					value.ToAndroidLocation()
				});
			}
			if (self.AppVersion != null)
			{
				androidJavaObject2.Call<AndroidJavaObject>("withAppVersion", new object[1]
				{
					self.AppVersion
				});
			}
			if (self.LocationTracking.HasValue)
			{
				androidJavaObject2.Call<AndroidJavaObject>("withLocationTracking", new object[1]
				{
					self.LocationTracking.Value
				});
			}
			if (self.SessionTimeout.HasValue)
			{
				androidJavaObject2.Call<AndroidJavaObject>("withSessionTimeout", new object[1]
				{
					self.SessionTimeout.Value
				});
			}
			if (self.CrashReporting.HasValue)
			{
				androidJavaObject2.Call<AndroidJavaObject>("withCrashReporting", new object[1]
				{
					self.CrashReporting.Value
				});
			}
			bool? logs = self.Logs;
			if (logs.HasValue && logs.Value)
			{
				androidJavaObject2.Call<AndroidJavaObject>("withLogs", new object[0]);
			}
			if (self.InstalledAppCollecting.HasValue)
			{
				androidJavaObject2.Call<AndroidJavaObject>("withInstalledAppCollecting", new object[1]
				{
					self.InstalledAppCollecting.Value
				});
			}
			if (self.HandleFirstActivationAsUpdate.HasValue)
			{
				androidJavaObject2.Call<AndroidJavaObject>("handleFirstActivationAsUpdate", new object[1]
				{
					self.HandleFirstActivationAsUpdate.Value
				});
			}
			if (self.PreloadInfo.HasValue)
			{
				YandexAppMetricaPreloadInfo value2 = self.PreloadInfo.Value;
				AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("com.yandex.metrica.PreloadInfo");
				AndroidJavaObject androidJavaObject3 = androidJavaClass2.CallStatic<AndroidJavaObject>("newBuilder", new object[1]
				{
					value2.TrackingId
				});
				foreach (KeyValuePair<string, string> item in value2.AdditionalInfo)
				{
					androidJavaObject3.Call<AndroidJavaObject>("setAdditionalParams", new object[2]
					{
						item.Key,
						item.Value
					});
				}
				androidJavaObject2.Call<AndroidJavaObject>("withPreloadInfo", new object[1]
				{
					androidJavaObject3.Call<AndroidJavaObject>("build", new object[0])
				});
			}
			if (self.StatisticsSending.HasValue)
			{
				androidJavaObject2.Call<AndroidJavaObject>("withStatisticsSending", new object[1]
				{
					self.StatisticsSending.Value
				});
			}
			androidJavaObject2.Call<AndroidJavaObject>("withNativeCrashReporting", new object[1]
			{
				false
			});
			return androidJavaObject2.Call<AndroidJavaObject>("build", new object[0]);
		}
	}

	public static AndroidJavaObject ToAndroidLocation(this YandexAppMetricaConfig.Coordinates self)
	{
		AndroidJavaObject androidJavaObject = new AndroidJavaObject("android.location.Location", string.Empty);
		androidJavaObject.Call("setLatitude", self.Latitude);
		androidJavaObject.Call("setLongitude", self.Longitude);
		return androidJavaObject;
	}

	public static AndroidJavaObject ToAndroidGender(this string self)
	{
		AndroidJavaObject result = null;
		if (self != null)
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.yandex.metrica.profile.GenderAttribute$Gender"))
			{
				return androidJavaClass.GetStatic<AndroidJavaObject>(self);
			}
		}
		return result;
	}

	public static AndroidJavaObject ToAndroidUserProfileUpdate(this YandexAppMetricaUserProfileUpdate self)
	{
		AndroidJavaObject androidJavaObject = null;
		AndroidJavaObject androidJavaObject2 = null;
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.yandex.metrica.profile.Attribute"))
		{
			androidJavaObject2 = ((self.Key == null) ? androidJavaClass.CallStatic<AndroidJavaObject>(self.AttributeName, new object[0]) : androidJavaClass.CallStatic<AndroidJavaObject>(self.AttributeName, new object[1]
			{
				self.Key
			}));
		}
		if (self.AttributeName == "gender" && self.Values.Length > 0)
		{
			self.Values[0] = (self.Values[0] as string).ToAndroidGender();
		}
		return androidJavaObject2.Call<AndroidJavaObject>(self.MethodName, self.Values);
	}

	public static AndroidJavaObject ToAndroidUserProfile(this YandexAppMetricaUserProfile self)
	{
		AndroidJavaObject result = null;
		if (self != null)
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.yandex.metrica.profile.UserProfile"))
			{
				AndroidJavaObject androidJavaObject = androidJavaClass.CallStatic<AndroidJavaObject>("newBuilder", new object[0]);
				List<YandexAppMetricaUserProfileUpdate> userProfileUpdates = self.GetUserProfileUpdates();
				foreach (YandexAppMetricaUserProfileUpdate item in userProfileUpdates)
				{
					androidJavaObject.Call<AndroidJavaObject>("apply", new object[1]
					{
						item.ToAndroidUserProfileUpdate()
					});
				}
				return androidJavaObject.Call<AndroidJavaObject>("build", new object[0]);
			}
		}
		return result;
	}

	public static AndroidJavaObject ToAndroidReceipt(this YandexAppMetricaReceipt? self)
	{
		AndroidJavaObject result = null;
		if (self.HasValue)
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.yandex.metrica.Revenue$Receipt"))
			{
				AndroidJavaObject androidJavaObject = androidJavaClass.CallStatic<AndroidJavaObject>("newBuilder", new object[0]);
				androidJavaObject.Call<AndroidJavaObject>("withData", new object[1]
				{
					self.Value.Data
				});
				androidJavaObject.Call<AndroidJavaObject>("withSignature", new object[1]
				{
					self.Value.Signature
				});
				return androidJavaObject.Call<AndroidJavaObject>("build", new object[0]);
			}
		}
		return result;
	}

	public static AndroidJavaObject ToAndroidInteger(this int? self)
	{
		AndroidJavaObject result = null;
		if (self.HasValue)
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("java.lang.Integer"))
			{
				return androidJavaClass.CallStatic<AndroidJavaObject>("valueOf", new object[1]
				{
					self
				});
			}
		}
		return result;
	}

	public static AndroidJavaObject ToAndroidCurrency(this string self)
	{
		AndroidJavaObject result = null;
		if (self != null)
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("java.util.Currency"))
			{
				return androidJavaClass.CallStatic<AndroidJavaObject>("getInstance", new object[1]
				{
					self
				});
			}
		}
		return result;
	}

	public static AndroidJavaObject ToAndroidRevenue(this YandexAppMetricaRevenue self)
	{
		AndroidJavaObject androidJavaObject = null;
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.yandex.metrica.Revenue"))
		{
			AndroidJavaObject androidJavaObject2 = androidJavaClass.CallStatic<AndroidJavaObject>("newBuilder", new object[2]
			{
				self.Price,
				self.Currency.ToAndroidCurrency()
			});
			androidJavaObject2.Call<AndroidJavaObject>("withQuantity", new object[1]
			{
				self.Quantity.ToAndroidInteger()
			});
			androidJavaObject2.Call<AndroidJavaObject>("withProductID", new object[1]
			{
				self.ProductID
			});
			androidJavaObject2.Call<AndroidJavaObject>("withPayload", new object[1]
			{
				self.Payload
			});
			androidJavaObject2.Call<AndroidJavaObject>("withReceipt", new object[1]
			{
				self.Receipt.ToAndroidReceipt()
			});
			return androidJavaObject2.Call<AndroidJavaObject>("build", new object[0]);
		}
	}
}
