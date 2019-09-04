using UnityEngine;

public class LoadingAnimation : MonoBehaviour
{
	private void OnEnable()
	{
		GetComponent<Animator>().Play("Loading");
	}

	private void OnDisable()
	{
	}
}
