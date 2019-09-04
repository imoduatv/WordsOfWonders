using UnityEngine;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{
	public static FadeController instance;

	private Image fade;

	private void Awake()
	{
		instance = this;
		fade = GetComponent<Image>();
	}

	public void fadeIn()
	{
		fade.raycastTarget = true;
		Movements.instance.startFadeIn(fade.gameObject, 0.2f, 0.3f);
		base.transform.SetAsLastSibling();
	}

	public void fadeOut()
	{
		fade.raycastTarget = false;
		Movements.instance.startFadeOut(fade.gameObject, 0.2f, 0f);
	}
}
