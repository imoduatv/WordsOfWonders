using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AvoidMulticlick : MonoBehaviour
{
	public float delay = 1.5f;

	private void Start()
	{
		GetComponent<Button>().onClick.AddListener(delegate
		{
			ButtonClicked();
		});
	}

	public void ButtonClicked()
	{
		GetComponent<Button>().enabled = false;
		StartCoroutine(ActivateButton());
	}

	private IEnumerator ActivateButton()
	{
		yield return new WaitForSeconds(delay);
		GetComponent<Button>().enabled = true;
	}

	private void OnEnable()
	{
		GetComponent<Button>().enabled = true;
	}
}
