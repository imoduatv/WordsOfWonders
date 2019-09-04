using System;
using UnityEngine;
using UnityEngine.UI;

public class OfferController : MonoBehaviour
{
	public static OfferController instance;

	public GameObject[] particles;

	public GameObject trumpets;

	private bool active;

	public GameObject popup;

	private void Awake()
	{
		instance = this;
	}

	public void tryShowOffer()
	{
	}

	private void enableTrumpets()
	{
		trumpets.SetActive(value: true);
		spawnParticle();
		for (int i = 0; i < trumpets.transform.childCount; i++)
		{
			GameObject gameObject = trumpets.transform.GetChild(i).gameObject;
			gameObject.SetActive(value: true);
			if (i < 2)
			{
				Movements.instance.scale(gameObject, Vector3.zero, Vector3.one * 1.9f, 0.4f);
				Movements.instance.scale(gameObject, Vector3.one * 1.9f, Vector3.one * 1.7f, 0.1f, 0.4f);
			}
			else
			{
				Movements.instance.scale(gameObject, Vector3.zero, (Vector3.one + Vector3.left * 2f) * 1.9f, 0.4f);
				Movements.instance.scale(gameObject, (Vector3.one + Vector3.left * 2f) * 1.9f, (Vector3.one + Vector3.left * 2f) * 1.7f, 0.1f, 0.4f);
			}
		}
	}

	private void spawnParticle()
	{
		int num = UnityEngine.Random.Range(0, particles.Length);
		float num2 = 10f;
		GameObject gameObject = UnityEngine.Object.Instantiate(particles[num]);
		gameObject.transform.SetParent(popup.transform);
		gameObject.transform.localScale = Vector3.one;
		gameObject.SetActive(value: true);
		gameObject.transform.GetComponent<RectTransform>().anchoredPosition = Vector2.up * 100f + Vector2.right * UnityEngine.Random.Range(-300f, 300f);
		Movements.instance.move(gameObject, gameObject.transform.position, gameObject.transform.position + Vector3.down * 4f, num2);
		Movements.instance.rotate(gameObject, 0f, UnityEngine.Random.Range(-720f, 720f), num2);
		Movements.instance.startFadeOut(gameObject.GetComponent<Image>(), 1f, 0f, num2 - 1f);
		UnityEngine.Object.Destroy(gameObject, num2 + 0.1f);
		Movements.instance.executeWithDelay((Movements.Execute)spawnParticle, UnityEngine.Random.Range(0.2f, 1.2f));
	}

	private void showOffer()
	{
	}

	private void setLastDate()
	{
		PlayerPrefs.SetString("last_offer_time", DateTime.Now.ToString());
	}

	private bool isCooledDown()
	{
		string @string = PlayerPrefs.GetString("last_offer_time", string.Empty);
		if (@string == string.Empty)
		{
			return true;
		}
		DateTime d = DateTime.Parse(@string);
		int num = Math.Abs((d - DateTime.Now).Days);
		return num >= 7;
	}

	public void onBuyClick()
	{
		ShopScript.instance.BuyTrialProButton();
	}

	public void onCloseClick()
	{
		PopupController.instance.popOut();
		UnityEngine.Object.Destroy(popup, 2f);
		UnityEngine.Object.Destroy(this, 2f);
		UnityEngine.Object.Destroy(base.gameObject, 2f);
	}
}
