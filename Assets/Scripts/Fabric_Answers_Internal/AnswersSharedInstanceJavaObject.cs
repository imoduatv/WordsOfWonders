using UnityEngine;

namespace Fabric.Answers.Internal
{
	internal class AnswersSharedInstanceJavaObject
	{
		private AndroidJavaObject javaObject;

		public AnswersSharedInstanceJavaObject()
		{
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.crashlytics.android.answers.Answers");
			javaObject = androidJavaClass.CallStatic<AndroidJavaObject>("getInstance", new object[0]);
		}

		public void Log(string methodName, AnswersEventInstanceJavaObject eventInstance)
		{
			javaObject.Call(methodName, eventInstance.javaObject);
		}
	}
}
