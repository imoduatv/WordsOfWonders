using UnityEngine;

public class ExtraWord : MonoBehaviour
{
	public Vector3 start;

	public Vector3 end;

	private Vector3 direction;

	public void startMove()
	{
		Movements.instance.move(moveFunc, 0.3f);
	}

	public void moveFunc(float par, MoveState state)
	{
		if (state == MoveState.START)
		{
			direction = end - start;
			base.gameObject.SetActive(value: true);
			base.transform.position = start;
		}
		if (state == MoveState.MOVE)
		{
			float d = Mathf.Lerp(0f, direction.x, Mathf.Pow(par, 0.5f));
			float d2 = Mathf.Lerp(0f, direction.y, Mathf.Pow(par, 2f));
			base.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 0.2f, par);
			Vector3 b = Vector3.right * d + Vector3.up * d2;
			base.transform.position = start + b;
		}
		if (state == MoveState.END)
		{
			base.transform.position = end;
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
