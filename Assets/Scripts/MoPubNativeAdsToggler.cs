using System;
using System.Collections;
using UnityEngine;

[ExecuteInEditMode]
public class MoPubNativeAdsToggler : MonoBehaviour
{
	private void Awake()
	{
		if (Application.isPlaying)
		{
			base.enabled = false;
		}
	}

	private void Update()
	{
		base.gameObject.hideFlags = (HideFlags.HideInHierarchy | HideFlags.HideInInspector);
		IEnumerator enumerator = base.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform transform = (Transform)enumerator.Current;
				transform.gameObject.SetActive(value: false);
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
	}
}
