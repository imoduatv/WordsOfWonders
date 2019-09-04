using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Movements : MonoBehaviour
{
	public delegate void MoveFunc(float par, MoveState state);

	public delegate void Execute();

	public delegate float ExecuteFloat();

	public static Movements instance;

	private void Awake()
	{
		instance = this;
	}

	public Coroutine move(RectTransform rect, Vector2 startPos, Vector2 endPos, float animTime, float delay)
	{
		return StartCoroutine(moveAnch(rect, startPos, endPos, animTime, delay));
	}

	private IEnumerator moveAnch(RectTransform rect, Vector2 startPos, Vector2 endPos, float animTime, float delay)
	{
		float t = 0f;
		yield return new WaitForSeconds(delay);
		while (t <= animTime)
		{
			float par = t / animTime;
			if (rect != null)
			{
				rect.anchoredPosition = Vector2.Lerp(startPos, endPos, par);
			}
			t += Time.deltaTime;
			yield return null;
		}
		if (rect != null)
		{
			rect.anchoredPosition = endPos;
		}
	}

	public void fadeImage(GameObject img, Sprite newImg, float time)
	{
		StartCoroutine(FadeImage(img, newImg, time));
	}

	private IEnumerator FadeImage(GameObject img, Sprite newImg, float time)
	{
		GameObject copy = UnityEngine.Object.Instantiate(img);
		copy.transform.SetParent(img.transform.parent);
		copy.transform.position = img.transform.position;
		copy.transform.localScale = img.transform.localScale;
		float t = 0f;
		img.GetComponent<Image>();
		Image replace = copy.GetComponent<Image>();
		copy.GetComponent<RectTransform>().sizeDelta = img.GetComponent<RectTransform>().sizeDelta;
		replace.sprite = newImg;
		while (t <= time)
		{
			float par = t / time;
			FugoUtils.ChangeAlpha(replace, par);
			t += Time.deltaTime;
			yield return null;
		}
		UnityEngine.Object.Destroy(img);
	}

	public void fadeAll(GameObject obj, float target, float time)
	{
		fadeAll(obj, target, time, 0f);
	}

	public void fadeAll(GameObject obj, float target, float time, float delay)
	{
		Text[] componentsInChildren = obj.GetComponentsInChildren<Text>();
		Text[] array = componentsInChildren;
		foreach (Text text in array)
		{
			try
			{
				Text txt = text;
				Color color = text.color;
				fadeOut(txt, time, color.a, target, delay);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.Log(ex.ToString());
			}
		}
		Image[] componentsInChildren2 = obj.GetComponentsInChildren<Image>();
		Text[] array2 = componentsInChildren;
		foreach (Text text2 in array2)
		{
			try
			{
				Text txt2 = text2;
				Color color2 = text2.color;
				fadeOut(txt2, time, color2.a, target, delay);
			}
			catch (Exception ex2)
			{
				UnityEngine.Debug.Log(ex2.ToString());
			}
		}
	}

	public Coroutine executeWithDelay(Execute func)
	{
		return StartCoroutine(executeAfterFrame(func));
	}

	public Coroutine executeWithDelay(Execute func, float time)
	{
		return StartCoroutine(execute(func, time));
	}

	public Coroutine executeWithDelay(ExecuteFloat func, float time)
	{
		return StartCoroutine(execute(func, time));
	}

	private IEnumerator executeAfterFrame(Execute func)
	{
		yield return null;
		func();
	}

	private IEnumerator execute(Execute func, float time)
	{
		yield return new WaitForSeconds(time);
		func();
	}

	private IEnumerator execute(ExecuteFloat func, float time)
	{
		yield return new WaitForSeconds(time);
		func();
	}

	public Coroutine move(GameObject obj, Vector3 start, Vector3 end, float animTime)
	{
		return StartCoroutine(moveThread(obj, start, end, animTime, 0f, local: false));
	}

	public void move(GameObject obj, Vector3 start, Vector3 end, float animTime, bool local)
	{
		StartCoroutine(moveThread(obj, start, end, animTime, 0f, local));
	}

	public void move(GameObject obj, Vector3 start, Vector3 end, float animTime, float delay, bool local)
	{
		StartCoroutine(moveThread(obj, start, end, animTime, delay, local));
	}

	private IEnumerator moveThread(GameObject obj, Vector3 start, Vector3 end, float animTime, float delay, bool local)
	{
		float t = 0f;
		yield return new WaitForSeconds(delay);
		while (t <= animTime)
		{
			if (animTime == 0f)
			{
				animTime = 0.01f;
			}
			float par2 = t / animTime;
			par2 = Mathf.Clamp01(par2);
			if (obj != null)
			{
				if (local)
				{
					obj.transform.localPosition = Vector3.Lerp(start, end, par2);
				}
				else
				{
					obj.transform.position = Vector3.Lerp(start, end, par2);
				}
			}
			t += Time.deltaTime;
			yield return null;
		}
		if (obj != null)
		{
			if (local)
			{
				obj.transform.localPosition = end;
			}
			else
			{
				obj.transform.position = end;
			}
		}
	}

	public Coroutine scale(GameObject obj, float end, float animTime)
	{
		Vector3 localScale = obj.transform.localScale;
		return StartCoroutine(scaleThread(obj, localScale, Vector3.one * end, animTime));
	}

	public Coroutine scale(GameObject obj, float end, float animTime, bool active)
	{
		Vector3 localScale = obj.transform.localScale;
		return StartCoroutine(scaleThread(obj, localScale, Vector3.one * end, animTime, active));
	}

	public Coroutine scale(GameObject obj, float start, float end, float animTime)
	{
		return StartCoroutine(scaleThread(obj, Vector3.one * start, Vector3.one * end, animTime));
	}

	public Coroutine scale(GameObject obj, Vector3 start, Vector3 end, float animTime)
	{
		return StartCoroutine(scaleThread(obj, start, end, animTime));
	}

	private IEnumerator scaleThread(GameObject obj, Vector3 start, Vector3 end, float animTime, bool active)
	{
		float t = 0f;
		while (t <= animTime)
		{
			obj.transform.localScale = Vector3.Lerp(start, end, t / animTime);
			t += Time.deltaTime;
			yield return null;
		}
		obj.transform.localScale = end;
		obj.SetActive(active);
	}

	private IEnumerator scaleThread(GameObject obj, Vector3 start, Vector3 end, float animTime)
	{
		float t = 0f;
		while (t <= animTime)
		{
			if (obj != null)
			{
				obj.transform.localScale = Vector3.Lerp(start, end, t / animTime);
			}
			t += Time.deltaTime;
			yield return null;
		}
		if (obj != null)
		{
			obj.transform.localScale = end;
		}
	}

	public Coroutine move(GameObject obj, Vector3 start, Vector3 end, float animTime, float delay)
	{
		return StartCoroutine(moveThread(obj, start, end, animTime, delay));
	}

	private IEnumerator moveThread(GameObject obj, Vector3 start, Vector3 end, float animTime, float delay)
	{
		float t = 0f;
		yield return new WaitForSeconds(delay);
		if (obj != null)
		{
			obj.transform.position = Vector3.Lerp(start, end, 0f);
			obj.SetActive(value: true);
			while (t <= animTime)
			{
				obj.transform.position = Vector3.Lerp(start, end, t / animTime);
				t += Time.deltaTime;
				yield return null;
			}
			obj.transform.position = end;
		}
	}

	public void scale(GameObject obj, Vector3 start, Vector3 end, float animTime, float delay)
	{
		StartCoroutine(scaleThread(obj, start, end, animTime, delay));
	}

	private IEnumerator scaleThread(GameObject obj, Vector3 start, Vector3 end, float animTime, float delay)
	{
		float t = 0f;
		yield return new WaitForSeconds(delay);
		if (!(obj != null))
		{
			yield break;
		}
		while (t <= animTime)
		{
			if (obj != null)
			{
				obj.transform.localScale = Vector3.Lerp(start, end, t / animTime);
			}
			t += Time.deltaTime;
			yield return null;
		}
		if (obj != null)
		{
			obj.transform.localScale = end;
		}
	}

	public Coroutine move(MoveFunc func, float animTime)
	{
		return StartCoroutine(moving(func, animTime));
	}

	private IEnumerator moving(MoveFunc func, float animTime)
	{
		func(0f, MoveState.START);
		float t = 0f;
		while (t <= animTime)
		{
			func(t / animTime, MoveState.MOVE);
			t += Time.deltaTime;
			yield return null;
		}
		func(1f, MoveState.END);
	}

	public Coroutine scale(MoveFunc func, float animTime)
	{
		return StartCoroutine(moving(func, animTime));
	}

	private IEnumerator scaling(MoveFunc func, float animTime)
	{
		func(0f, MoveState.START);
		float t = 0f;
		while (t <= animTime)
		{
			func(t / animTime, MoveState.MOVE);
			t += Time.deltaTime;
			yield return null;
		}
		func(1f, MoveState.END);
	}

	private IEnumerator fadeIn(SpriteRenderer img, float animTime, float maxAlpha, float minAlpha)
	{
		float t = 0f;
		while (t <= animTime)
		{
			float alpha2 = t / animTime;
			alpha2 = (maxAlpha - minAlpha) * alpha2 + minAlpha;
			FugoUtils.ChangeAlpha(img, alpha2);
			t += Time.deltaTime;
			yield return null;
		}
		FugoUtils.ChangeAlpha(img, maxAlpha);
	}

	private IEnumerator fadeOut(SpriteRenderer img, float animTime, float maxAlpha, float minAlpha, float delay)
	{
		yield return new WaitForSeconds(delay);
		float t = 0f;
		while (t <= animTime)
		{
			float alpha2 = t / animTime;
			alpha2 = (maxAlpha - minAlpha) * (1f - alpha2) + minAlpha;
			FugoUtils.ChangeAlpha(img, alpha2);
			t += Time.deltaTime;
			yield return null;
		}
		FugoUtils.ChangeAlpha(img, minAlpha);
	}

	private IEnumerator fadeIn(Image img, float animTime, float maxAlpha, float minAlpha, float delay)
	{
		yield return new WaitForSeconds(delay);
		float t = 0f;
		while (t <= animTime)
		{
			float alpha2 = t / animTime;
			alpha2 = (maxAlpha - minAlpha) * alpha2 + minAlpha;
			FugoUtils.ChangeAlpha(img, alpha2);
			t += Time.deltaTime;
			yield return null;
		}
		FugoUtils.ChangeAlpha(img, maxAlpha);
	}

	private IEnumerator fadeOut(Image img, float animTime, float maxAlpha, float minAlpha, float delay)
	{
		yield return new WaitForSeconds(delay);
		float t = 0f;
		while (t <= animTime)
		{
			float alpha2 = t / animTime;
			alpha2 = (maxAlpha - minAlpha) * (1f - alpha2) + minAlpha;
			FugoUtils.ChangeAlpha(img, alpha2);
			t += Time.deltaTime;
			yield return null;
		}
		FugoUtils.ChangeAlpha(img, minAlpha);
	}

	private IEnumerator fadeIn(Text txt, float animTime, float maxAlpha, float minAlpha, float delay)
	{
		yield return new WaitForSeconds(delay);
		float t = 0f;
		while (t <= animTime)
		{
			float alpha2 = t / animTime;
			alpha2 = (maxAlpha - minAlpha) * alpha2 + minAlpha;
			FugoUtils.ChangeAlpha(txt, alpha2);
			t += Time.deltaTime;
			yield return null;
		}
		FugoUtils.ChangeAlpha(txt, maxAlpha);
	}

	private IEnumerator fadeOut(Text txt, float animTime, float maxAlpha, float minAlpha, float delay)
	{
		yield return new WaitForSeconds(delay);
		float t = 0f;
		while (t <= animTime)
		{
			float alpha2 = t / animTime;
			alpha2 = (maxAlpha - minAlpha) * (1f - alpha2) + minAlpha;
			if (txt != null)
			{
				FugoUtils.ChangeAlpha(txt, alpha2);
			}
			t += Time.deltaTime;
			yield return null;
		}
		if (txt != null)
		{
			FugoUtils.ChangeAlpha(txt, minAlpha);
		}
	}

	public Coroutine startFadeIn(GameObject obj, float time, float max)
	{
		try
		{
			SpriteRenderer component = obj.GetComponent<SpriteRenderer>();
			return startFadeIn(component, time, max);
		}
		catch
		{
			try
			{
				Image component2 = obj.GetComponent<Image>();
				return startFadeIn(component2, time, max);
			}
			catch
			{
				Text component3 = obj.GetComponent<Text>();
				return startFadeIn(component3, time, max);
			}
		}
	}

	public Coroutine startFadeOut(GameObject obj, float time, float min)
	{
		try
		{
			SpriteRenderer component = obj.GetComponent<SpriteRenderer>();
			return startFadeOut(component, time, min);
		}
		catch
		{
			try
			{
				Image component2 = obj.GetComponent<Image>();
				return startFadeOut(component2, time, min);
			}
			catch
			{
				try
				{
					Text component3 = obj.GetComponent<Text>();
					return startFadeOut(component3, time, min);
				}
				catch (Exception)
				{
				}
			}
		}
		return null;
	}

	private Coroutine startFadeIn(SpriteRenderer img, float time, float max)
	{
		Color color = img.color;
		float a = color.a;
		return StartCoroutine(fadeIn(img, time, max, a));
	}

	private Coroutine startFadeIn(Image img, float time, float max)
	{
		Color color = img.color;
		float a = color.a;
		return StartCoroutine(fadeIn(img, time, max, a, 0f));
	}

	private Coroutine startFadeIn(Image img, float time, float max, float delay)
	{
		Color color = img.color;
		float a = color.a;
		return StartCoroutine(fadeIn(img, time, max, a, delay));
	}

	private Coroutine startFadeIn(Text txt, float time, float max)
	{
		Color color = txt.color;
		float a = color.a;
		return StartCoroutine(fadeIn(txt, time, max, a, 0f));
	}

	private Coroutine startFadeOut(SpriteRenderer img, float time, float min)
	{
		Color color = img.color;
		float a = color.a;
		return StartCoroutine(fadeOut(img, time, a, min, 0f));
	}

	public Coroutine startFadeOut(Image img, float time, float min, float delay)
	{
		Color color = img.color;
		float a = color.a;
		return StartCoroutine(fadeOut(img, time, a, min, delay));
	}

	private Coroutine startFadeOut(Image img, float time, float min)
	{
		Color color = img.color;
		float a = color.a;
		return StartCoroutine(fadeOut(img, time, a, min, 0f));
	}

	public Coroutine startFadeOut(Text txt, float time, float min, float delay)
	{
		Color color = txt.color;
		float a = color.a;
		return StartCoroutine(fadeOut(txt, time, a, min, delay));
	}

	private Coroutine startFadeOut(Text txt, float time, float min)
	{
		Color color = txt.color;
		float a = color.a;
		return StartCoroutine(fadeOut(txt, time, a, min, 0f));
	}

	public Coroutine lerpColorTo(GameObject obj, Color target, float time)
	{
		return lerpColorTo(obj, target, time, 0f);
	}

	public Coroutine lerpColorTo(GameObject obj, Color target, float time, float delay)
	{
		try
		{
			Text component = obj.GetComponent<Text>();
			if (component != null)
			{
				return StartCoroutine(lerpColor(component, target, time, delay));
			}
		}
		catch (Exception)
		{
		}
		return StartCoroutine(lerpColor(obj, target, time, delay));
	}

	public Coroutine startFlash(Image img, Color start, Color end, float time)
	{
		return StartCoroutine(flash(img, start, end, time));
	}

	private IEnumerator flash(Image img, Color start, Color end, float time)
	{
		float t = 0f;
		while (t < time)
		{
			img.color = Color.Lerp(start, end, t / time);
			t += Time.deltaTime;
			yield return null;
		}
		while (t > 0f)
		{
			img.color = Color.Lerp(start, end, t / time);
			t -= Time.deltaTime;
			yield return null;
		}
		img.color = start;
	}

	private IEnumerator lerpColor(GameObject obj, Color target, float time, float delay)
	{
		yield return new WaitForSeconds(delay);
		Image img = obj.GetComponent<Image>();
		Color start = img.color;
		float t = 0f;
		while (t < time)
		{
			img.color = Color.Lerp(start, target, t / time);
			t += Time.deltaTime;
			yield return null;
		}
		img.color = target;
	}

	private IEnumerator lerpColor(Text txt, Color target, float time, float delay)
	{
		yield return new WaitForSeconds(delay);
		Color start = txt.color;
		float t = 0f;
		while (t < time)
		{
			if (txt != null)
			{
				txt.color = Color.Lerp(start, target, t / time);
			}
			t += Time.deltaTime;
			yield return null;
		}
		if (txt != null)
		{
			txt.color = target;
		}
	}

	public Coroutine tilt(GameObject obj, int cycle, float time, float size)
	{
		return StartCoroutine(tilting(obj, cycle, time, size));
	}

	public Coroutine reverseTilt(GameObject obj, int cycle, float time, float size)
	{
		return StartCoroutine(reverseTilting(obj, cycle, time, size));
	}

	public Coroutine tilt(GameObject obj, int cycle, float time)
	{
		return StartCoroutine(tilting(obj, cycle, time, 10f));
	}

	private IEnumerator tilting(GameObject obj, int cycle, float time, float size)
	{
		float t = 0f;
		Vector3 end = obj.transform.localPosition;
		while (t < time)
		{
			Vector3 pos = Vector3.zero;
			pos.x = Mathf.Sin((float)Math.PI * (t / time) * (float)cycle * 2f) * size;
			pos += end;
			if (obj != null)
			{
				obj.transform.localPosition = pos;
			}
			t += Time.deltaTime;
			yield return null;
		}
		if (obj != null)
		{
			obj.transform.localPosition = end;
		}
	}

	private IEnumerator reverseTilting(GameObject obj, int cycle, float time, float size)
	{
		float t = 0f;
		Vector3 end = obj.transform.localPosition;
		while (t < time)
		{
			Vector3 pos = Vector3.zero;
			pos.x = (0f - Mathf.Sin((float)Math.PI * (t / time) * (float)cycle * 2f)) * size;
			pos += end;
			obj.transform.localPosition = pos;
			t += Time.deltaTime;
			yield return null;
		}
		obj.transform.localPosition = end;
	}

	public Coroutine tiltVertical(GameObject obj, int cycle, float time, float size)
	{
		return StartCoroutine(VerticalTilting(obj, cycle, time, size));
	}

	public Coroutine reverseVerticalTilt(GameObject obj, int cycle, float time, float size)
	{
		return StartCoroutine(reverseVerticalTilting(obj, cycle, time, size));
	}

	public Coroutine tiltVertical(GameObject obj, int cycle, float time)
	{
		return StartCoroutine(VerticalTilting(obj, cycle, time, 10f));
	}

	private IEnumerator VerticalTilting(GameObject obj, int cycle, float time, float size)
	{
		float t = 0f;
		Vector3 end = obj.transform.localPosition;
		while (t < time)
		{
			Vector3 pos = Vector3.zero;
			pos.y = Mathf.Sin((float)Math.PI * (t / time) * (float)cycle * 2f) * size;
			pos += end;
			obj.transform.localPosition = pos;
			t += Time.deltaTime;
			yield return null;
		}
		obj.transform.localPosition = end;
	}

	private IEnumerator reverseVerticalTilting(GameObject obj, int cycle, float time, float size)
	{
		float t = 0f;
		Vector3 end = obj.transform.localPosition;
		while (t < time)
		{
			Vector3 pos = Vector3.zero;
			pos.y = (0f - Mathf.Sin((float)Math.PI * (t / time) * (float)cycle * 2f)) * size;
			pos += end;
			obj.transform.localPosition = pos;
			t += Time.deltaTime;
			yield return null;
		}
		obj.transform.localPosition = end;
	}

	public Coroutine startRotate(GameObject obj, float speed)
	{
		return StartCoroutine(rotate(obj, speed));
	}

	private IEnumerator rotate(GameObject obj, float speed)
	{
		while (true)
		{
			obj.transform.Rotate(Vector3.back, speed * Time.deltaTime);
			yield return null;
		}
	}

	public Coroutine startIncrease(Text txt, float s, float e, float anim, float delay)
	{
		return StartCoroutine(increase(txt, s, e, anim, delay));
	}

	private IEnumerator increase(Text txt, float s, float e, float anim, float delay)
	{
		float t = 0f;
		txt.text = s.ToString("F0");
		yield return new WaitForSeconds(delay);
		while (t < anim)
		{
			txt.text = Mathf.Lerp(s, e, t / anim).ToString("F0") + "\n<color=#71b1e0><size=25>" + LanguageScript.ExpeditionText + "</size></color>";
			t += Time.deltaTime;
			yield return null;
		}
		txt.text = e.ToString("F0") + "\n<color=#71b1e0><size=25>" + LanguageScript.ExpeditionText + "</size></color>";
	}

	public Coroutine rotate(GameObject obj, float start, float end, float anim, float delay)
	{
		return StartCoroutine(rotating(obj, start, end, anim, delay));
	}

	public Coroutine rotate(GameObject obj, float start, float end, float anim)
	{
		return StartCoroutine(rotating(obj, start, end, anim, 0f));
	}

	private IEnumerator rotating(GameObject obj, float start, float end, float anim, float delay)
	{
		float t = 0f;
		yield return new WaitForSeconds(delay);
		Vector3 v3 = new Vector3(0f, 0f, start);
		Vector3 v2 = new Vector3(0f, 0f, end);
		while (t < anim)
		{
			if (obj != null)
			{
				obj.transform.localRotation = Quaternion.Euler(Vector3.Lerp(v3, v2, t / anim));
			}
			t += Time.deltaTime;
			yield return null;
		}
		if (obj != null)
		{
			obj.transform.localRotation = Quaternion.Euler(Vector3.back * end);
		}
	}
}
