using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
	private GameObject frame;

	private GameObject image;

	private Animator animator;

	private Coroutine fade;

	private void Start()
	{
		frame = base.transform.Find("Frame").gameObject;
		image = base.transform.Find("Image").gameObject;
		frame.SetActive(value: true);
		frame.GetComponent<RectTransform>().sizeDelta = image.GetComponent<RectTransform>().sizeDelta;
		frame.GetComponent<Image>().sprite = image.GetComponent<Image>().sprite;
		FugoUtils.ChangeAlpha(frame.GetComponent<Image>(), 0f);
		frame.SetActive(value: false);
		frame.transform.SetAsLastSibling();
		HideFrame();
		AddTriggers();
		animator = GetComponent<Animator>();
		if (animator != null)
		{
			animator.enabled = false;
		}
	}

	public void PlayPressedAnim()
	{
		if (animator != null)
		{
			animator.enabled = true;
			animator.Play("Pressed");
			StartCoroutine(DisableAnimator());
		}
	}

	public void disableFrame()
	{
		frame.SetActive(value: false);
	}

	public void enableFrame()
	{
		frame.SetActive(value: true);
	}

	public void ShowFrame()
	{
		frame.SetActive(value: true);
		if (fade != null)
		{
			StopCoroutine(fade);
		}
		fade = Movements.instance.startFadeIn(frame.gameObject, 0.2f, 0.6f);
		Movements.instance.executeWithDelay((Movements.Execute)enableFrame, 0.21f);
	}

	public void HideFrame()
	{
		if (fade != null)
		{
			StopCoroutine(fade);
		}
		fade = Movements.instance.startFadeOut(frame.gameObject, 0.2f, 0f);
		Movements.instance.executeWithDelay((Movements.Execute)disableFrame, 0.21f);
	}

	public void AddTriggers()
	{
		EventTrigger eventTrigger = base.gameObject.AddComponent<EventTrigger>();
		EventTrigger.Entry entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerEnter;
		entry.callback.AddListener(delegate
		{
			ShowFrame();
		});
		eventTrigger.triggers.Add(entry);
		entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerClick;
		entry.callback.AddListener(delegate
		{
			HideFrame();
			PlayPressedAnim();
		});
		eventTrigger.triggers.Add(entry);
		entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerExit;
		entry.callback.AddListener(delegate
		{
			HideFrame();
		});
		eventTrigger.triggers.Add(entry);
		entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerUp;
		entry.callback.AddListener(delegate
		{
			HideFrame();
		});
		eventTrigger.triggers.Add(entry);
	}

	private IEnumerator DisableAnimator()
	{
		yield return new WaitForSeconds(1f);
		animator.enabled = false;
	}
}
