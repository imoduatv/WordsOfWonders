using UnityEngine;
using UnityEngine.UI;

public class PopupController : MonoBehaviour
{
	public static PopupController instance;

	public Transform holder;

	public Image fade;

	private float animTime = 0.3f;

	private void Awake()
	{
		instance = this;
	}

	public void proClicked()
	{
	}

	private void handlePurchase()
	{
	}

	public void setPopup(GameObject popup)
	{
		popup.transform.SetParent(holder);
		popup.transform.localScale = Vector3.one;
		popup.transform.localPosition = Vector3.one;
	}

	public void popIn()
	{
		fade.raycastTarget = true;
		Movements.instance.scale(holder.gameObject, Vector3.zero, Vector3.one, animTime);
		Movements.instance.startFadeIn(fade.gameObject, animTime, 1f);
	}

	public void popOut()
	{
		fade.raycastTarget = false;
		Movements.instance.scale(holder.gameObject, Vector3.one, Vector3.zero, animTime);
		Movements.instance.startFadeIn(fade.gameObject, animTime, 0f);
		Movements.instance.executeWithDelay((Movements.Execute)discardPopup, animTime);
	}

	private void discardPopup()
	{
		holder.GetChild(0).SetParent(null);
	}
}
