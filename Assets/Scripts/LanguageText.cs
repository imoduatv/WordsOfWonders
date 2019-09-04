using System;
using UnityEngine;
using UnityEngine.UI;

public class LanguageText : Text
{
	public string key;

	protected override void Start()
	{
		AutoLanguage instance = AutoLanguage.instance;
		instance.onTextChanged = (AutoLanguage.OnTextChanged)Delegate.Combine(instance.onTextChanged, new AutoLanguage.OnTextChanged(setText));
		setText();
	}

	public void setText()
	{
		try
		{
			if (UISwapper.flipGame && Application.isPlaying)
			{
				ArabicText arabicText = base.gameObject.GetComponent<ArabicText>();
				if (arabicText == null)
				{
					arabicText = base.gameObject.AddComponent<ArabicText>();
				}
				arabicText.Text = AutoLanguage.dict[key];
				arabicText.FixArabicText();
			}
			else
			{
				base.text = AutoLanguage.dict[key];
			}
		}
		catch (Exception ex)
		{
			base.text = ex.ToString().Split(':')[0];
		}
	}
}
