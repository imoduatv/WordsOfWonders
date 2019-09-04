using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BarController : MonoBehaviour
{
	public static BarController instance;

	public static float animTime;

	public int steps;

	private Vector3 start;

	private Vector3 end;

	private Transform bar;

	private Transform parent;

	private Transform grandparent;

	private Transform parentStart;

	private Transform parentEnd;

	private Coroutine moving;

	private void Awake()
	{
		instance = this;
		start = base.transform.Find("Start").localPosition;
		end = base.transform.Find("End").localPosition;
		bar = base.transform.Find("Bar");
		bar.GetComponent<Image>().color = FugoUtils.HexToColor(BoardController.set.InGameSelectedLetterBGColor);
		parent = base.transform.parent;
		grandparent = parent.parent;
		parentStart = grandparent.Find("End");
		parentEnd = grandparent.Find("Start");
		animTime = 0.5f;
		steps = FugoUtils.extraPerCoin;
	}

	private void Update()
	{
	}

	public void goToStep(int step)
	{
		start = base.transform.Find("Start").localPosition;
		end = base.transform.Find("End").localPosition;
		bar = base.transform.Find("Bar");
		parent = base.transform.parent;
		grandparent = parent.parent;
		parentStart = grandparent.Find("End");
		parentEnd = grandparent.Find("Start");
		Vector3 localPosition = Vector3.Lerp(start, end, (float)step / (float)steps);
		bar.transform.localPosition = localPosition;
	}

	public Coroutine goToStepAnimating(int step)
	{
		start = base.transform.Find("Start").localPosition;
		end = base.transform.Find("End").localPosition;
		bar = base.transform.Find("Bar");
		parent = base.transform.parent;
		grandparent = parent.parent;
		parentStart = grandparent.Find("End");
		parentEnd = grandparent.Find("Start");
		if (moving != null)
		{
			StopCoroutine(moving);
		}
		Vector3 target = Vector3.Lerp(start, end, (float)step / (float)steps);
		if (step != 0)
		{
		}
		moving = Movements.instance.move(parent.gameObject, parent.position, parentEnd.position, 0.2f);
		return StartCoroutine(moveTo(target, 0.4f));
	}

	private IEnumerator moveTo(Vector3 target, float delay)
	{
		yield return new WaitForSeconds(delay);
		float t = 0f;
		Vector3 s = bar.transform.localPosition;
		while (t < animTime)
		{
			bar.localPosition = Vector3.Lerp(s, target, t / animTime);
			t += Time.deltaTime;
			yield return null;
		}
		bar.localPosition = target;
		yield return new WaitForSeconds(0.3f);
		moving = Movements.instance.move(parent.gameObject, parent.position, parentStart.position, 0.2f);
	}
}
