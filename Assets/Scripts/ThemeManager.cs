using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ThemeManager : MonoBehaviour
{
	public static ThemeManager instance;

	public static int theme;

	public GameObject oldBG;

	public GameObject starSystem;

	public GameObject newBG;

	public GameObject oldLogo;

	public GameObject newLogo;

	public GameObject oldWorld;

	public GameObject newWorld;

	public GameObject newQuestButton;

	public GameObject lightSystem;

	public GameObject fbButton;

	public Transform startButton;

	public Transform levelsButton;

	public Transform dailyButton;

	public Transform proBadge;

	public Transform shopButton;

	public Sprite[] coinBGs;

	public Transform fog1;

	public Transform fog2;

	public Transform fog3;

	public Transform fog4;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void ChangeTheme(int val)
	{
		theme = val;
		if (PlayerPrefsManager.GetLang() == "Arabic" || PlayerPrefsManager.GetLang() == "Hebrew")
		{
			theme = 0;
		}
		ArrangeMenuButtonPositions();
		DisableAll();
		shopButton.Find("CoinHolderBG").GetComponent<Image>().sprite = coinBGs[theme];
		Animation();
		if (theme == 0)
		{
			MenuController.instance.logo = oldLogo.transform;
			MenuController.instance.langText = oldLogo.transform.Find("LangRibbon/LangText").GetComponent<Text>();
			MenuController.instance.sun = oldWorld.transform;
			shopButton.Find("CoinHolderBG/CoinHolder/CoinText").GetComponent<Text>().color = Color.white;
			oldBG.SetActive(value: true);
			oldLogo.SetActive(value: true);
			oldWorld.SetActive(value: true);
			BGRotate();
			starSystem.SetActive(value: true);
		}
		else if (theme == 1)
		{
			MenuController.instance.proBadge = proBadge.gameObject;
			QuestController.instance.questButton = newQuestButton.transform;
			MenuController.instance.logo = newLogo.transform;
			MenuController.instance.langText = newLogo.transform.Find("LangRibbon/LangText").GetComponent<Text>();
			MenuController.instance.sun = newWorld.transform;
			MenuController.instance.questButton = newQuestButton.transform;
			shopButton.Find("CoinHolderBG/CoinHolder/CoinText").GetComponent<Text>().color = Color.black;
			fbButton.SetActive(value: true);
			newLogo.SetActive(value: true);
			newWorld.SetActive(value: true);
			newBG.SetActive(value: true);
			starSystem.SetActive(value: true);
			StartCoroutine(LightSystemActivator());
		}
	}

	public void Animation()
	{
		if (theme == 0)
		{
			UnityEngine.Object.Destroy(fog1.gameObject);
			UnityEngine.Object.Destroy(fog2.gameObject);
		}
		else if (theme == 1)
		{
			StartCoroutine(FogAnimationThread());
		}
	}

	private IEnumerator FogAnimationThread()
	{
		yield return null;
		yield return null;
		FogScript component = fog1.GetComponent<FogScript>();
		Vector2 sizeDelta = fog1.GetComponent<RectTransform>().sizeDelta;
		Vector3 initalPos = new Vector3(sizeDelta.x / 4f, -55f, 0f);
		Vector2 sizeDelta2 = fog1.GetComponent<RectTransform>().sizeDelta;
		component.Move(initalPos, -50f, (sizeDelta2.x + SectionController.canvasWidth) / -2f);
		FogScript component2 = fog2.GetComponent<FogScript>();
		Vector2 sizeDelta3 = fog2.GetComponent<RectTransform>().sizeDelta;
		Vector3 initalPos2 = new Vector3(0f - sizeDelta3.x / 4f, -719f, 0f);
		Vector2 sizeDelta4 = fog2.GetComponent<RectTransform>().sizeDelta;
		component2.Move(initalPos2, 30f, (sizeDelta4.x + SectionController.canvasWidth) / 2f);
		FogScript component3 = fog3.GetComponent<FogScript>();
		Vector2 sizeDelta5 = fog3.GetComponent<RectTransform>().sizeDelta;
		Vector3 initalPos3 = new Vector3(3f * (sizeDelta5.x / 4f), -55f, 0f);
		Vector2 sizeDelta6 = fog3.GetComponent<RectTransform>().sizeDelta;
		component3.Move(initalPos3, -50f, (sizeDelta6.x + SectionController.canvasWidth) / -2f);
		FogScript component4 = fog4.GetComponent<FogScript>();
		Vector2 sizeDelta7 = fog4.GetComponent<RectTransform>().sizeDelta;
		Vector3 initalPos4 = new Vector3(3f * (0f - sizeDelta7.x / 4f), -719f, 0f);
		Vector2 sizeDelta8 = fog4.GetComponent<RectTransform>().sizeDelta;
		component4.Move(initalPos4, 30f, (sizeDelta8.x + SectionController.canvasWidth) / 2f);
	}

	private IEnumerator LightSystemActivator()
	{
		yield return new WaitForSeconds(0.5f);
		lightSystem.SetActive(value: true);
	}

	public void BGRotate()
	{
		oldBG.transform.localRotation = Quaternion.Euler(0f, 0f, -2.5f);
		StartCoroutine(FugoUtils.RotatorReverse(3.75f, 45f, oldBG.transform));
	}

	private void DisableAll()
	{
		oldBG.SetActive(value: false);
		starSystem.SetActive(value: false);
		fbButton.SetActive(value: false);
		newBG.SetActive(value: false);
		oldLogo.SetActive(value: false);
		newLogo.SetActive(value: false);
		oldWorld.SetActive(value: false);
		newWorld.SetActive(value: false);
		lightSystem.SetActive(value: false);
	}

	private void ArrangeMenuButtonPositions()
	{
		if (theme == 0)
		{
			startButton.transform.localPosition = new Vector3(0f, -30f, 0f);
			levelsButton.transform.localPosition = new Vector3(0f, -180f, 0f);
			dailyButton.transform.localPosition = new Vector3(0f, -330f, 0f);
			if (PlayerPrefsManager.GetLevel() < 13)
			{
				startButton.transform.localPosition = new Vector3(0f, -120f, 0f);
				levelsButton.transform.localPosition = new Vector3(0f, -270f, 0f);
			}
		}
		else if (theme == 1)
		{
			startButton.transform.localPosition = new Vector3(0f, -75f, 0f);
			levelsButton.transform.localPosition = new Vector3(0f, -195f, 0f);
			dailyButton.transform.localPosition = new Vector3(0f, -316f, 0f);
			if (PlayerPrefsManager.GetLevel() < 13)
			{
				startButton.transform.localPosition = new Vector3(0f, -165f, 0f);
				levelsButton.transform.localPosition = new Vector3(0f, -315f, 0f);
			}
		}
	}
}
