public class NativeRateUS
{
	public string title;

	public string message;

	public string yes;

	public string later;

	public string no;

	public string appLink;

	public NativeRateUS(string title, string message)
	{
		this.title = title;
		this.message = message;
		yes = LanguageScript.YesText;
		later = LanguageScript.LaterText;
		no = LanguageScript.NoText;
	}

	public void SetAppLink(string _appLink)
	{
		appLink = _appLink;
	}

	public void InitRateUS()
	{
		AndroidRateUsPopUp androidRateUsPopUp = AndroidRateUsPopUp.Create(title, message, yes, later, no);
		androidRateUsPopUp.appLink = appLink;
	}
}
