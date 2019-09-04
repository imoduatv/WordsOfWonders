using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BlurControl : MonoBehaviour
{
	public static BlurControl instance;

	public static int maxBlur = 20;

	public GameObject normalBackground;

	public GameObject blurBackground;

	public Material blurMaterial;

	private float animTime;

	private void Awake()
	{
		instance = this;
		animTime = 1.5f;
		if (!(instance == null))
		{
		}
	}

	private void Update()
	{
	}

	private IEnumerator deblur()
	{
		animTime = 0.5f;
		float t = 0f;
		int maxSize = blurMaterial.GetInt("_Size");
		while (t < animTime)
		{
			float par2 = t / animTime;
			par2 = 1f - par2;
			int size = (int)(par2 * (float)maxSize);
			setSize(size);
			t += Time.deltaTime;
			yield return null;
		}
		setSize(0);
	}

	private IEnumerator makeBlur()
	{
		animTime = 0.5f;
		float t = 0f;
		int maxSize = maxBlur;
		while (t < animTime)
		{
			float par = t / animTime;
			int size = (int)(par * (float)maxSize);
			setSize(size);
			t += Time.deltaTime;
			yield return null;
		}
		setSize(maxSize);
	}

	private void setSize(int size)
	{
		blurMaterial.SetInt("_Size", size);
	}

	public void fullBlur()
	{
		try
		{
			FugoUtils.ChangeAlpha(base.transform.GetComponent<Image>(), 1f);
		}
		catch (Exception)
		{
		}
		if (!PlayerPrefsManager.IsBlurOn())
		{
			FugoUtils.ChangeAlpha(base.transform.GetComponent<Image>(), 0f);
		}
	}

	public void noBlur()
	{
		try
		{
			FugoUtils.ChangeAlpha(base.transform.GetComponent<Image>(), 0f);
		}
		catch (Exception)
		{
		}
		if (!PlayerPrefsManager.IsBlurOn())
		{
			FugoUtils.ChangeAlpha(base.transform.GetComponent<Image>(), 0f);
		}
	}

	public void disableBlur()
	{
		SoundManager.instance.blurReveal();
		if (!PlayerPrefsManager.IsBlurOn())
		{
			FugoUtils.ChangeAlpha(base.transform.GetComponent<Image>(), 0f);
		}
		Movements.instance.startFadeOut(base.gameObject, 1f, 0f);
	}

	public void enableBlur()
	{
		if (!PlayerPrefsManager.IsBlurOn())
		{
			FugoUtils.ChangeAlpha(base.transform.GetComponent<Image>(), 0f);
		}
		else
		{
			Movements.instance.startFadeIn(base.gameObject, 1f, 1f);
		}
	}
}
