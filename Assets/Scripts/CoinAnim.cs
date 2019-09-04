using UnityEngine;

public class CoinAnim : MonoBehaviour
{
	public Vector3 start;

	public Vector3 end;

	public int amount;

	private Vector3 distance;

	private void Awake()
	{
		amount = 1;
	}

	private void Update()
	{
	}

	public void startMove()
	{
		float num = 5f;
		float d = num;
		Movements.instance.scale(base.gameObject, Vector3.zero, Vector3.one, 0.2f);
		Movements.instance.move(base.gameObject, base.transform.position, base.transform.position + Vector3.left * 1.2f * d + Vector3.up * d, 0.2f);
		Movements.instance.executeWithDelay((Movements.Execute)go, 0.2f);
	}

	private void go()
	{
		Movements.instance.move(func, 0.7f);
		base.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
	}

	private void func(float par, MoveState state)
	{
		if (state == MoveState.START)
		{
			distance = -base.transform.position + GameAnimController.instance.store.transform.Find("ShopButton").transform.position;
			start = base.transform.position;
		}
		if (state == MoveState.MOVE)
		{
			float x = Mathf.Pow(par, 2f) * distance.x;
			float y = Mathf.Pow(par, 0.8f) * distance.y;
			base.transform.position = start + new Vector3(x, y, 0f);
		}
		if (state == MoveState.END)
		{
			base.transform.position = GameAnimController.instance.store.transform.position;
			UnityEngine.Object.Destroy(base.gameObject);
			PlayerPrefsManager.IncreaseCoin(amount);
			GameMenuController.instance.updateCoin(animating: true);
		}
	}

	public void startParabolicMove()
	{
		Movements.instance.move(parabolic, 0.4f);
		base.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
	}

	public void startParabolicMove(float delay)
	{
		Movements.instance.executeWithDelay((Movements.Execute)startParabolicMove, delay);
	}

	private void parabolic(float par, MoveState state)
	{
		if (state == MoveState.START)
		{
			base.gameObject.SetActive(value: true);
		}
		if (state == MoveState.MOVE)
		{
			Vector3 position = Vector3.Lerp(start, end, par);
			position.y -= 5f * par * (par - 1f);
			base.transform.position = position;
		}
		if (state == MoveState.END)
		{
			base.transform.position = end;
			UnityEngine.Object.Destroy(base.gameObject);
			PlayerPrefsManager.IncreaseCoin(amount);
			GameMenuController.instance.updateCoin(animating: true);
			SoundManager.instance.CoinIncrease();
		}
	}
}
