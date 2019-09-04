using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fabric.Answers.Internal
{
	internal class AnswersEventInstanceJavaObject
	{
		public AndroidJavaObject javaObject;

		public AnswersEventInstanceJavaObject(string eventType, Dictionary<string, object> customAttributes, params string[] args)
		{
			javaObject = new AndroidJavaObject($"com.crashlytics.android.answers.{eventType}", args);
			foreach (KeyValuePair<string, object> customAttribute in customAttributes)
			{
				string key = customAttribute.Key;
				object value = customAttribute.Value;
				if (value == null)
				{
					UnityEngine.Debug.Log($"[Answers] Expected custom attribute value to be non-null. Received: {value}");
				}
				else if (IsNumericType(value))
				{
					javaObject.Call<AndroidJavaObject>("putCustomAttribute", new object[2]
					{
						key,
						AsDouble(value)
					});
				}
				else if (value is string)
				{
					javaObject.Call<AndroidJavaObject>("putCustomAttribute", new object[2]
					{
						key,
						value
					});
				}
				else
				{
					UnityEngine.Debug.Log($"[Answers] Expected custom attribute value to be a string or numeric. Received: {value}");
				}
			}
		}

		public void PutMethod(string method)
		{
			InvokeSafelyAsString("putMethod", method);
		}

		public void PutSuccess(bool? success)
		{
			InvokeSafelyAsBoolean("putSuccess", success);
		}

		public void PutContentName(string contentName)
		{
			InvokeSafelyAsString("putContentName", contentName);
		}

		public void PutContentType(string contentType)
		{
			InvokeSafelyAsString("putContentType", contentType);
		}

		public void PutContentId(string contentId)
		{
			InvokeSafelyAsString("putContentId", contentId);
		}

		public void PutCurrency(string currency)
		{
			InvokeSafelyAsCurrency("putCurrency", currency);
		}

		public void InvokeSafelyAsCurrency(string methodName, string currency)
		{
			if (currency != null)
			{
				AndroidJavaClass androidJavaClass = new AndroidJavaClass("java.util.Currency");
				AndroidJavaObject androidJavaObject = androidJavaClass.CallStatic<AndroidJavaObject>("getInstance", new object[1]
				{
					currency
				});
				javaObject.Call<AndroidJavaObject>("putCurrency", new object[1]
				{
					androidJavaObject
				});
			}
		}

		public void InvokeSafelyAsBoolean(string methodName, bool? arg)
		{
			if (arg.HasValue)
			{
				javaObject.Call<AndroidJavaObject>(methodName, new object[1]
				{
					arg
				});
			}
		}

		public void InvokeSafelyAsInt(string methodName, int? arg)
		{
			if (arg.HasValue)
			{
				javaObject.Call<AndroidJavaObject>(methodName, new object[1]
				{
					arg
				});
			}
		}

		public void InvokeSafelyAsString(string methodName, string arg)
		{
			if (arg != null)
			{
				javaObject.Call<AndroidJavaObject>(methodName, new object[1]
				{
					arg
				});
			}
		}

		public void InvokeSafelyAsDecimal(string methodName, object arg)
		{
			if (arg != null)
			{
				javaObject.Call<AndroidJavaObject>(methodName, new object[1]
				{
					new AndroidJavaObject("java.math.BigDecimal", arg.ToString())
				});
			}
		}

		public void InvokeSafelyAsDouble(string methodName, object arg)
		{
			if (arg != null)
			{
				javaObject.Call<AndroidJavaObject>(methodName, new object[1]
				{
					AsDouble(arg)
				});
			}
		}

		private static bool IsNumericType(object o)
		{
			switch (Type.GetTypeCode(o.GetType()))
			{
			case TypeCode.SByte:
			case TypeCode.Byte:
			case TypeCode.Int16:
			case TypeCode.UInt16:
			case TypeCode.Int32:
			case TypeCode.UInt32:
			case TypeCode.Int64:
			case TypeCode.UInt64:
			case TypeCode.Single:
			case TypeCode.Double:
			case TypeCode.Decimal:
				return true;
			default:
				return false;
			}
		}

		private static AndroidJavaObject AsDouble(object param)
		{
			return new AndroidJavaObject("java.lang.Double", param.ToString());
		}
	}
}
