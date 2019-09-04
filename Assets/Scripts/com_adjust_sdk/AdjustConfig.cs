using System;

namespace com.adjust.sdk
{
	public class AdjustConfig
	{
		internal string appToken;

		internal string sceneName;

		internal string userAgent;

		internal string defaultTracker;

		internal long? info1;

		internal long? info2;

		internal long? info3;

		internal long? info4;

		internal long? secretId;

		internal double? delayStart;

		internal bool? isDeviceKnown;

		internal bool? sendInBackground;

		internal bool? eventBufferingEnabled;

		internal bool? allowSuppressLogLevel;

		internal bool launchDeferredDeeplink;

		internal AdjustLogLevel? logLevel;

		internal AdjustEnvironment environment;

		internal Action<string> deferredDeeplinkDelegate;

		internal Action<AdjustEventSuccess> eventSuccessDelegate;

		internal Action<AdjustEventFailure> eventFailureDelegate;

		internal Action<AdjustSessionSuccess> sessionSuccessDelegate;

		internal Action<AdjustSessionFailure> sessionFailureDelegate;

		internal Action<AdjustAttribution> attributionChangedDelegate;

		internal bool? readImei;

		internal string processName;

		internal Action<string> logDelegate;

		public AdjustConfig(string appToken, AdjustEnvironment environment)
		{
			sceneName = string.Empty;
			processName = string.Empty;
			this.appToken = appToken;
			this.environment = environment;
		}

		public AdjustConfig(string appToken, AdjustEnvironment environment, bool allowSuppressLogLevel)
		{
			sceneName = string.Empty;
			processName = string.Empty;
			this.appToken = appToken;
			this.environment = environment;
			this.allowSuppressLogLevel = allowSuppressLogLevel;
		}

		public void setLogLevel(AdjustLogLevel logLevel)
		{
			this.logLevel = logLevel;
		}

		public void setDefaultTracker(string defaultTracker)
		{
			this.defaultTracker = defaultTracker;
		}

		public void setLaunchDeferredDeeplink(bool launchDeferredDeeplink)
		{
			this.launchDeferredDeeplink = launchDeferredDeeplink;
		}

		public void setSendInBackground(bool sendInBackground)
		{
			this.sendInBackground = sendInBackground;
		}

		public void setEventBufferingEnabled(bool eventBufferingEnabled)
		{
			this.eventBufferingEnabled = eventBufferingEnabled;
		}

		public void setDelayStart(double delayStart)
		{
			this.delayStart = delayStart;
		}

		public void setUserAgent(string userAgent)
		{
			this.userAgent = userAgent;
		}

		public void setIsDeviceKnown(bool isDeviceKnown)
		{
			this.isDeviceKnown = isDeviceKnown;
		}

		public void setDeferredDeeplinkDelegate(Action<string> deferredDeeplinkDelegate, string sceneName = "Adjust")
		{
			this.deferredDeeplinkDelegate = deferredDeeplinkDelegate;
			this.sceneName = sceneName;
		}

		public Action<string> getDeferredDeeplinkDelegate()
		{
			return deferredDeeplinkDelegate;
		}

		public void setAttributionChangedDelegate(Action<AdjustAttribution> attributionChangedDelegate, string sceneName = "Adjust")
		{
			this.attributionChangedDelegate = attributionChangedDelegate;
			this.sceneName = sceneName;
		}

		public Action<AdjustAttribution> getAttributionChangedDelegate()
		{
			return attributionChangedDelegate;
		}

		public void setEventSuccessDelegate(Action<AdjustEventSuccess> eventSuccessDelegate, string sceneName = "Adjust")
		{
			this.eventSuccessDelegate = eventSuccessDelegate;
			this.sceneName = sceneName;
		}

		public Action<AdjustEventSuccess> getEventSuccessDelegate()
		{
			return eventSuccessDelegate;
		}

		public void setEventFailureDelegate(Action<AdjustEventFailure> eventFailureDelegate, string sceneName = "Adjust")
		{
			this.eventFailureDelegate = eventFailureDelegate;
			this.sceneName = sceneName;
		}

		public Action<AdjustEventFailure> getEventFailureDelegate()
		{
			return eventFailureDelegate;
		}

		public void setSessionSuccessDelegate(Action<AdjustSessionSuccess> sessionSuccessDelegate, string sceneName = "Adjust")
		{
			this.sessionSuccessDelegate = sessionSuccessDelegate;
			this.sceneName = sceneName;
		}

		public Action<AdjustSessionSuccess> getSessionSuccessDelegate()
		{
			return sessionSuccessDelegate;
		}

		public void setSessionFailureDelegate(Action<AdjustSessionFailure> sessionFailureDelegate, string sceneName = "Adjust")
		{
			this.sessionFailureDelegate = sessionFailureDelegate;
			this.sceneName = sceneName;
		}

		public Action<AdjustSessionFailure> getSessionFailureDelegate()
		{
			return sessionFailureDelegate;
		}

		public void setAppSecret(long secretId, long info1, long info2, long info3, long info4)
		{
			this.secretId = secretId;
			this.info1 = info1;
			this.info2 = info2;
			this.info3 = info3;
			this.info4 = info4;
		}

		public void setProcessName(string processName)
		{
			this.processName = processName;
		}

		[Obsolete("This is an obsolete method.")]
		public void setReadMobileEquipmentIdentity(bool readMobileEquipmentIdentity)
		{
		}

		public void setLogDelegate(Action<string> logDelegate)
		{
			this.logDelegate = logDelegate;
		}
	}
}
