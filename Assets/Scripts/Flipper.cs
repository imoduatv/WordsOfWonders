using UnityEngine;

public static class Flipper
{
	public static void Flip(this Transform transform)
	{
		Vector3 localScale = transform.localScale;
		localScale.x *= -1f;
		transform.localScale = localScale;
	}
}
