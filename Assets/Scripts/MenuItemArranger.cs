using UnityEngine;
using UnityEngine.UI;

public class MenuItemArranger : MonoBehaviour
{
	public Transform galleryBottomArea;

	public Transform galleryLevelButton;

	public Transform shopButton;

	public Transform menuItemContainer;

	public Transform sectionPanel;

	public Transform dailyPuzzleBackButton;

	public Transform dailyPuzzleStarHolder;

	public Transform setPanelBackButton;

	public Transform setBackButton;

	public Transform sectionBackButton;

	public Transform settingsButton;

	public Transform settingsHolder;

	public Transform fakeShopButton;

	public Transform dailyGaleryButton;

	public Transform questButton;

	public static MenuItemArranger instance;

	private float bannerHeight = 100f;

	private float topBannerHeight = 70f;

	private void Start()
	{
		instance = this;
		Arrange();
		if (PlayerPrefsManager.GetLang() == "Arabic" || PlayerPrefsManager.GetLang() == "Hebrew")
		{
			SetArabic();
		}
		fakeShopButton.transform.position = shopButton.transform.position;
	}

	private void Update()
	{
	}

	public void Arrange()
	{
		if (FugoAdManager.isBannerAtTop)
		{
			if (PlayerPrefsManager.GetNoAd() == 0)
			{
				SetPosition(menuItemContainer, 0f - topBannerHeight);
				Transform t = menuItemContainer.Find("Settings&HiddenMenuButtonHolder");
				Vector2 anchoredPosition = menuItemContainer.Find("Logo").GetComponent<RectTransform>().anchoredPosition;
				SetPosition(t, anchoredPosition.y);
				SetPosition(dailyPuzzleBackButton, -211f);
				SetPosition(dailyPuzzleStarHolder, 508f);
				SetPosition(setPanelBackButton, -211f);
				Vector2 offsetMax = sectionPanel.GetComponent<RectTransform>().offsetMax;
				offsetMax.y = -160f;
				sectionPanel.GetComponent<RectTransform>().offsetMax = offsetMax;
			}
			else
			{
				SetPosition(menuItemContainer, 0f);
				SetPosition(menuItemContainer.Find("Settings&HiddenMenuButtonHolder"), 572f);
				SetPosition(dailyPuzzleBackButton, -46f);
				SetPosition(dailyPuzzleStarHolder, 597f);
				SetPosition(setPanelBackButton, -46f);
				Vector2 offsetMax2 = sectionPanel.GetComponent<RectTransform>().offsetMax;
				offsetMax2.y = 1f;
				sectionPanel.GetComponent<RectTransform>().offsetMax = offsetMax2;
			}
			SetPosition(galleryBottomArea, 92.5f);
			SetPosition(galleryLevelButton, 94f);
			SetPosition(shopButton.transform, 122f);
		}
		else if (PlayerPrefsManager.GetNoAd() == 0)
		{
			SetPosition(galleryBottomArea, 230f);
			SetPosition(galleryLevelButton, 235f);
			SetPosition(shopButton.transform, 208f);
		}
		else
		{
			SetPosition(galleryBottomArea, 92.5f);
			SetPosition(galleryLevelButton, 94f);
			SetPosition(shopButton.transform, 122f);
		}
		fakeShopButton.transform.position = shopButton.transform.position;
		Vector3 position = questButton.position;
		Vector3 position2 = shopButton.position;
		position.y = position2.y;
		questButton.position = position;
	}

	private void SetPosition(Transform t, float y)
	{
		Vector2 anchoredPosition = t.GetComponent<RectTransform>().anchoredPosition;
		anchoredPosition.y = y;
		t.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;
	}

	private void SetArabic()
	{
		shopButton.GetComponent<RectTransform>().localScale = new Vector3(-1f, 1f, 1f);
		shopButton.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 0f);
		shopButton.GetComponent<RectTransform>().anchorMax = new Vector2(0f, 0f);
		shopButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(70f, 208f);
		shopButton.Find("CoinHolderBG/CoinHolder/CoinText").localScale = new Vector3(-1f, 1f, 1f);
		shopButton.Find("CoinHolderBG/CoinHolder/CoinText").GetComponent<Text>().alignment = TextAnchor.MiddleLeft;
		sectionBackButton.GetComponent<RectTransform>().localScale = new Vector3(-1f, 1f, 1f);
		sectionBackButton.GetComponent<RectTransform>().anchorMin = new Vector2(1f, 1f);
		sectionBackButton.GetComponent<RectTransform>().anchorMax = new Vector2(1f, 1f);
		sectionBackButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-54f, -46f);
		setBackButton.GetComponent<RectTransform>().localScale = new Vector3(-1f, 1f, 1f);
		setBackButton.GetComponent<RectTransform>().anchorMin = new Vector2(1f, 1f);
		setBackButton.GetComponent<RectTransform>().anchorMax = new Vector2(1f, 1f);
		setBackButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-54f, -46f);
		dailyGaleryButton.GetComponent<RectTransform>().anchorMin = new Vector2(1f, 0f);
		dailyGaleryButton.GetComponent<RectTransform>().anchorMax = new Vector2(1f, 0f);
		dailyGaleryButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-54f, 208f);
		settingsHolder.GetComponent<RectTransform>().localScale = new Vector3(-1f, 1f, 1f);
		Text[] allComponents = settingsHolder.GetAllComponents<Text>();
		Text[] array = allComponents;
		foreach (Text text in array)
		{
			text.GetComponent<RectTransform>().localScale = new Vector3(-1f, 1f, 1f);
		}
		settingsHolder.Find("FBLoginButtonHolder/FBLoginButton/Icon").transform.localScale = new Vector3(-1f, 1f, 1f);
	}
}
