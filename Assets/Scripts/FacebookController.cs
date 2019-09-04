using Fabric.Answers;
using Facebook.Unity;
using System.Collections.Generic;
using UnityEngine;

public class FacebookController : MonoBehaviour
{
	public GameObject fbMenuButton;

	private void Awake()
	{
		if (!FB.IsInitialized)
		{
			FB.Init(InitCallback, OnHideUnity);
		}
		else
		{
			FB.ActivateApp();
		}
	}

	private void InitCallback()
	{
		if (FB.IsInitialized)
		{
			FB.ActivateApp();
		}
		else
		{
			UnityEngine.Debug.Log("Failed to Initialize the Facebook SDK");
		}
	}

	private void Update()
	{
		if (UnityEngine.Input.GetKeyDown(KeyCode.P))
		{
			LoginWithFacebook();
		}
	}

	private void OnHideUnity(bool isGameShown)
	{
		if (!isGameShown)
		{
			Time.timeScale = 0f;
		}
		else
		{
			Time.timeScale = 1f;
		}
	}

	public void LoginWithFacebook()
	{
		if (PlayerPrefsManager.GetFBID() == string.Empty)
		{
			List<string> list = new List<string>();
			list.Add("public_profile");
			list.Add("email");
			List<string> permissions = list;
			FB.LogInWithReadPermissions(permissions, AuthCallback);
			fbMenuButton.transform.parent.gameObject.SetActive(value: false);
		}
		else
		{
			PlayerPrefsManager.SetFBID(string.Empty);
			TextController.instance.SetTexts("facebook");
			if (ThemeManager.theme == 1)
			{
				fbMenuButton.transform.parent.gameObject.SetActive(value: true);
			}
		}
	}

	private void AuthCallback(ILoginResult result)
	{
		if (FB.IsLoggedIn)
		{
			AccessToken currentAccessToken = AccessToken.CurrentAccessToken;
			PlayerPrefsManager.SetFBID(currentAccessToken.UserId);
			TextController.instance.SetTexts("facebook");
			RequestManager.instance.LogRequestAfterLogin();
			RequestManager.instance.GetLogRequest();
			Answers.LogLogin("Facebook", true);
			MenuController.instance.SetVersionText();
		}
		else
		{
			if (ThemeManager.theme == 1)
			{
				fbMenuButton.transform.parent.gameObject.SetActive(value: true);
			}
			UnityEngine.Debug.Log("User cancelled login");
		}
	}
}
