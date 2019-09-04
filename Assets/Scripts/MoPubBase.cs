using MoPubInternal.ThirdParty.MiniJSON;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoPubBase
{
	public enum AdPosition
	{
		TopLeft,
		TopCenter,
		TopRight,
		Centered,
		BottomLeft,
		BottomCenter,
		BottomRight
	}

	public static class Consent
	{
		public enum Status
		{
			Unknown,
			Denied,
			DoNotTrack,
			PotentialWhitelist,
			Consented
		}

		private static class Strings
		{
			public const string ExplicitYes = "explicit_yes";

			public const string ExplicitNo = "explicit_no";

			public const string Unknown = "unknown";

			public const string PotentialWhitelist = "potential_whitelist";

			public const string Dnt = "dnt";
		}

		public static Status FromString(string status)
		{
			if (status != null)
			{
				if (status == "explicit_yes")
				{
					return Status.Consented;
				}
				if (status == "explicit_no")
				{
					return Status.Denied;
				}
				if (status == "dnt")
				{
					return Status.DoNotTrack;
				}
				if (status == "potential_whitelist")
				{
					return Status.PotentialWhitelist;
				}
				if (status == "unknown")
				{
					return Status.Unknown;
				}
			}
			try
			{
				return (Status)Enum.Parse(typeof(Status), status);
			}
			catch
			{
				UnityEngine.Debug.LogError("Unknown consent status string: " + status);
				return Status.Unknown;
			}
		}
	}

	public enum BannerType
	{
		Size320x50,
		Size300x250,
		Size728x90,
		Size160x600
	}

	public enum LogLevel
	{
		MPLogLevelAll = 0,
		MPLogLevelTrace = 10,
		MPLogLevelDebug = 20,
		MPLogLevelInfo = 30,
		MPLogLevelWarn = 40,
		MPLogLevelError = 50,
		MPLogLevelFatal = 60,
		MPLogLevelOff = 70
	}

	public struct SdkConfiguration
	{
		public string AdUnitId;

		public AdvancedBidder[] AdvancedBidders;

		public MediationSetting[] MediationSettings;

		public RewardedNetwork[] NetworksToInit;

		public string AdvancedBiddersString => (AdvancedBidders == null) ? string.Empty : string.Join(",", (from b in AdvancedBidders
			select b.ToString()).ToArray());

		public string MediationSettingsJson => (MediationSettings == null) ? string.Empty : Json.Serialize(MediationSettings);

		public string NetworksToInitString => (NetworksToInit == null) ? string.Empty : string.Join(",", (from b in NetworksToInit
			select b.ToString()).ToArray());
	}

	public class MediationSetting : Dictionary<string, object>
	{
		public class AdColony : MediationSetting
		{
			public AdColony()
				: base("AdColony")
			{
			}
		}

		public class AdMob : MediationSetting
		{
			public AdMob()
				: base("GooglePlayServices", "MPGoogle")
			{
			}
		}

		public class Chartboost : MediationSetting
		{
			public Chartboost()
				: base("Chartboost")
			{
			}
		}

		public class Vungle : MediationSetting
		{
			public Vungle()
				: base("Vungle")
			{
			}
		}

		public MediationSetting(string adVendor)
		{
			Add("adVendor", adVendor);
		}

		public MediationSetting(string android, string ios)
			: this(android)
		{
		}
	}

	public struct Reward
	{
		public string Label;

		public int Amount;

		public override string ToString()
		{
			return $"\"{Amount} {Label}\"";
		}

		public bool IsValid()
		{
			return !string.IsNullOrEmpty(Label) && Amount > 0;
		}
	}

	public abstract class ThirdPartyNetwork
	{
		private readonly string _name;

		protected ThirdPartyNetwork(string name)
		{
			_name = "com.mopub.mobileads." + name;
		}

		public override string ToString()
		{
			return _name;
		}
	}

	public class AdvancedBidder : ThirdPartyNetwork
	{
		public static readonly AdvancedBidder AdColony = GetBidder("AdColony");

		public static readonly AdvancedBidder Facebook = GetBidder("Facebook");

		private AdvancedBidder(string name)
			: base(name)
		{
		}

		private static AdvancedBidder GetBidder(string network)
		{
			network += "AdvancedBidder";
			return new AdvancedBidder(network);
		}
	}

	public class RewardedNetwork : ThirdPartyNetwork
	{
		private readonly string _name;

		public static readonly RewardedNetwork AdColony = GetNetwork("AdColony");

		public static readonly RewardedNetwork AdMob = GetNetwork("GooglePlayServices", "MPGoogleAdMob");

		public static readonly RewardedNetwork AppLovin = GetNetwork("AppLovin");

		public static readonly RewardedNetwork Chartboost = GetNetwork("Chartboost");

		public static readonly RewardedNetwork Facebook = GetNetwork("Facebook");

		public static readonly RewardedNetwork IronSource = GetNetwork("IronSource");

		public static readonly RewardedNetwork OnebyAOL = GetNetwork("Millenial", "MPMillennial");

		public static readonly RewardedNetwork Tapjoy = GetNetwork("Tapjoy");

		public static readonly RewardedNetwork Unity = GetNetwork("Unity", "UnityAds");

		public static readonly RewardedNetwork Vungle = GetNetwork("Vungle");

		private RewardedNetwork(string name)
			: base(name)
		{
		}

		private static RewardedNetwork GetNetwork(string network)
		{
			network += "RewardedVideo";
			return new RewardedNetwork(network);
		}

		private static RewardedNetwork GetNetwork(string android, string ios)
		{
			return GetNetwork(android);
		}
	}

	public const double LatLongSentinel = 99999.0;

	private static string _pluginName;

	public static string ConsentLanguageCode
	{
		get;
		set;
	}

	public static string PluginName => _pluginName ?? (_pluginName = "MoPub Unity Plugin v" + Application.version);

	protected MoPubBase()
	{
	}

	protected static void ValidateAdUnitForSdkInit(string adUnitId)
	{
		if (string.IsNullOrEmpty(adUnitId))
		{
			UnityEngine.Debug.LogError("A valid ad unit ID is needed to initialize the MoPub SDK.");
		}
	}

	protected static void ReportAdUnitNotFound(string adUnitId)
	{
		UnityEngine.Debug.LogWarning($"AdUnit {adUnitId} not found: no plugin was initialized");
	}

	protected static Uri UrlFromString(string url)
	{
		if (string.IsNullOrEmpty(url))
		{
			return null;
		}
		try
		{
			return new Uri(url);
		}
		catch
		{
			UnityEngine.Debug.LogError("Invalid URL: " + url);
			return null;
		}
	}

	protected static void InitManager()
	{
		Type typeFromHandle = typeof(MoPubManager);
		MoPubManager component = new GameObject("MoPubManager", typeFromHandle).GetComponent<MoPubManager>();
		if (MoPubManager.Instance != component)
		{
			UnityEngine.Debug.LogWarning("It looks like you have the " + typeFromHandle.Name + " on a GameObject in your scene. Please remove the script from your scene.");
		}
	}
}
