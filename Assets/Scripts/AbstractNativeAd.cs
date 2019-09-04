using MoPubInternal.ThirdParty.MiniJSON;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class AbstractNativeAd : MonoBehaviour
{
	public struct Data
	{
		public Uri MainImageUrl;

		public Uri IconImageUrl;

		public Uri ClickDestinationUrl;

		public string CallToAction;

		public string Title;

		public string Text;

		public double StarRating;

		public Uri PrivacyInformationIconClickThroughUrl;

		public Uri PrivacyInformationIconImageUrl;

		public static Uri ToUri(object value)
		{
			Uri uri = value as Uri;
			if (uri != null)
			{
				return uri;
			}
			string text = value as string;
			if (string.IsNullOrEmpty(text))
			{
				return null;
			}
			if (Uri.IsWellFormedUriString(text, UriKind.Absolute))
			{
				return new Uri(text, UriKind.Absolute);
			}
			UnityEngine.Debug.LogError("Invalid URL: " + text);
			return null;
		}

		public static Data FromJson(string json)
		{
			Dictionary<string, object> dictionary = (Json.Deserialize(json) as Dictionary<string, object>) ?? new Dictionary<string, object>();
			Data result = default(Data);
			result.MainImageUrl = ((!dictionary.TryGetValue("mainImageUrl", out object value)) ? null : ToUri(value));
			result.IconImageUrl = ((!dictionary.TryGetValue("iconImageUrl", out value)) ? null : ToUri(value));
			result.ClickDestinationUrl = ((!dictionary.TryGetValue("clickDestinationUrl", out value)) ? null : ToUri(value));
			result.CallToAction = ((!dictionary.TryGetValue("callToAction", out value)) ? string.Empty : (value as string));
			result.Title = ((!dictionary.TryGetValue("title", out value)) ? string.Empty : (value as string));
			result.Text = ((!dictionary.TryGetValue("text", out value)) ? string.Empty : (value as string));
			result.StarRating = ((!dictionary.TryGetValue("starRating", out value)) ? 0.0 : ((double)value));
			result.PrivacyInformationIconClickThroughUrl = ((!dictionary.TryGetValue("privacyInformationIconClickThroughUrl", out value)) ? null : ToUri(value));
			result.PrivacyInformationIconImageUrl = ((!dictionary.TryGetValue("privacyInformationIconImageUrl", out value)) ? null : ToUri(value));
			return result;
		}

		public string ToString()
		{
			return $"mainImageUrl: {MainImageUrl}\niconImageUrl: {IconImageUrl}\nclickDestinationUrl: {ClickDestinationUrl}\ncallToAction: {CallToAction}\ntitle: {Title}\ntext: {Text}\nstarRating: {StarRating}\nprivacyInformationIconClickThroughUrl: {PrivacyInformationIconClickThroughUrl}\nprivacyInformationIconImageUrl: {PrivacyInformationIconImageUrl}";
		}
	}

	public string AdUnitId;

	[Header("Text")]
	public Text Title;

	public Text Text;

	public Text CallToAction;

	[Header("Images")]
	public Renderer MainImage;

	public Renderer IconImage;

	public Renderer PrivacyInformationIconImage;
}
