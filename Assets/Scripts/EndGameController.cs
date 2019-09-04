using UnityEngine;
using UnityEngine.UI;
using v2Gameplay;

public class EndGameController : MonoBehaviour
{
	public static EndGameController instance;

	public GameObject next;

	public GameObject collect;

	public Text voyeurText;

	private void Awake()
	{
		instance = this;
	}

	private void OnEnable()
	{
	}

	public void playAnim()
	{
		GetComponent<Animation>().Play();
		setFalseVoyeur();
		EndType endType = GameController.endType;
		switch (endType)
		{
		case EndType.Level:
			next.SetActive(value: true);
			collect.SetActive(value: false);
			return;
		case EndType.Hidden:
			next.SetActive(value: false);
			collect.SetActive(value: true);
			GameMenuController.instance.setGoldAmount(100);
			PlayerPrefsManager.SetLastPlayedMode(GameMode.NORMAL);
			return;
		}
		next.SetActive(value: false);
		collect.SetActive(value: true);
		if (endType == EndType.Set)
		{
			GameMenuController.instance.setGoldAmount(25);
		}
		else
		{
			GameMenuController.instance.setGoldAmount(125);
		}
	}

	public void enableCollectButton()
	{
		UnityEngine.Object.Destroy(GetComponent<Animation>());
		Movements.instance.scale(collect, Vector3.zero, Vector3.one, 0.2f);
	}

	public void hideObjects()
	{
	}

	private void checkCollectButton()
	{
		if (FugoAdManager.instance.InterstitialReady())
		{
			FugoAdManager.instance.interstitialClosed = enableCollectButton;
			FugoAdManager.instance.ShowInterstitial();
		}
		else
		{
			enableCollectButton();
		}
	}

	public void walkOnMap()
	{
		int num = MapController.instance.nextLevel();
		switch (GameController.endType)
		{
		case EndType.Level:
			break;
		case EndType.Hidden:
			Movements.instance.executeWithDelay((Movements.Execute)checkCollectButton, 1f);
			break;
		default:
			Movements.instance.executeWithDelay((Movements.Execute)checkCollectButton, 1f);
			break;
		}
	}

	public void setFalseVoyeur()
	{
		if (GameController.levelToOpen == -1)
		{
			string str = (PlayerPrefsManager.GetBrilliance() - 1).ToString();
			voyeurText.text = str + "\n<color=#88d5ff><size=25>" + LanguageScript.ExpeditionText + "</size></color>";
			if (UISwapper.flipGame)
			{
				ArabicText arabicText = voyeurText.GetComponent<ArabicText>();
				if (arabicText == null)
				{
					arabicText = voyeurText.gameObject.AddComponent<ArabicText>();
				}
				arabicText.Text = str + "\n\n" + LanguageScript.ExpeditionText;
			}
		}
		else
		{
			setTrueVoyeur();
		}
	}

	public void setTrueVoyeur()
	{
		string str = PlayerPrefsManager.GetBrilliance().ToString();
		voyeurText.text = str + "\n<color=#88d5ff><size=25>" + LanguageScript.ExpeditionText + "</size></color>";
		if (UISwapper.flipGame)
		{
			ArabicText arabicText = voyeurText.GetComponent<ArabicText>();
			if (arabicText == null)
			{
				arabicText = voyeurText.gameObject.AddComponent<ArabicText>();
			}
			arabicText.Text = str + "\n\n" + LanguageScript.ExpeditionText;
		}
	}

	public void blinkVoyeur()
	{
		if (GameController.levelToOpen == -1)
		{
			Movements.instance.scale(voyeurText.gameObject, 0f, 0.1f);
			if (UISwapper.flipGame)
			{
				Movements.instance.scale(voyeurText.gameObject, Vector3.zero, new Vector3(-1f, 1f, 1f), 0.1f, 0.1f);
			}
			else
			{
				Movements.instance.scale(voyeurText.gameObject, Vector3.zero, Vector3.one, 0.1f, 0.1f);
			}
			Movements.instance.executeWithDelay((Movements.Execute)setTrueVoyeur, 0.1f);
		}
	}

	public void spinSphere()
	{
		if (GameController.game.mode == GameMode.NORMAL)
		{
			Sphere.instance.spin();
		}
		blinkVoyeur();
	}
}
