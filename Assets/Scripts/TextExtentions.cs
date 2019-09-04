using UnityEngine.UI;

public static class TextExtentions
{
	public static void SetText(this Text text, string value)
	{
		if (UISwapper.flipGame)
		{
			ArabicText arabicText = text.GetComponent<ArabicText>();
			if (arabicText == null)
			{
				arabicText = text.gameObject.AddComponent<ArabicText>();
			}
			arabicText.Text = value;
		}
		else
		{
			text.text = value;
		}
	}
}
