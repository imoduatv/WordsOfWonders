using UnityEngine;

public class HandAnimation : MonoBehaviour
{
	private Vector3 defaultPosition;

	private Vector3 touchPosition;

	private float speed = 7f;

	private float par;

	private void Start()
	{
		defaultPosition = new Vector3(1.4f, -1.7f, 0f);
		defaultPosition = ObjectHolder.instance.wheel.transform.position + Vector3.right * 1.8f + Vector3.down * 0.57f;
		base.transform.position = defaultPosition;
	}

	private void Update()
	{
		if (Input.GetMouseButton(0))
		{
			touchPosition = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
			par += speed * Time.deltaTime;
			par = Mathf.Clamp01(par);
		}
		else
		{
			par -= speed * Time.deltaTime;
			par = Mathf.Clamp01(par);
		}
		calculate();
	}

	private void calculate()
	{
		touchPosition.z = -5f;
		base.transform.position = Vector3.Lerp(defaultPosition, touchPosition, par);
	}
}
