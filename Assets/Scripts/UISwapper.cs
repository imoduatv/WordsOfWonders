using UnityEngine;
using UnityEngine.UI;

public class UISwapper : MonoBehaviour
{
	public static bool flipGame = true;

	public Transform[] flips;

	private void Awake()
	{
		flipGame = (PlayerPrefsManager.GetLang() == SystemLanguage.Arabic.ToString() || PlayerPrefsManager.GetLang() == SystemLanguage.Hebrew.ToString());
		if (flipGame)
		{
			flipTexts();
			flipObjects();
			Matrix4x4 projectionMatrix = Camera.main.projectionMatrix;
			projectionMatrix *= Matrix4x4.Scale(new Vector3(-1f, 1f, 1f));
			Camera.main.projectionMatrix = projectionMatrix;
		}
	}

	private void flipObjects()
	{
		Transform[] array = flips;
		foreach (Transform transform in array)
		{
			transform.transform.Flip();
		}
	}

	private void flipTexts()
	{
		Text[] allComponents = GameObject.Find("Canvas").transform.GetAllComponents<Text>();
		Text[] array = allComponents;
		foreach (Text text in array)
		{
			text.transform.Flip();
		}
	}

	private void flipImages()
	{
		Image[] allComponents = GameObject.Find("Canvas").transform.GetAllComponents<Image>();
		Image[] array = allComponents;
		foreach (Image image in array)
		{
			image.transform.Flip();
		}
	}

	private void swapGameScene()
	{
		swapItems(ObjectHolder.instance.hintButton.transform.GetChild(0).gameObject, ObjectHolder.instance.shuffleButton.transform.GetChild(0).gameObject);
	}

	private void swapItems(GameObject A, GameObject B)
	{
		Transform parent = A.transform.parent;
		Transform parent2 = B.transform.parent;
		A.transform.SetParent(parent2);
		B.transform.SetParent(parent);
		A.transform.localPosition = Vector3.zero;
		B.transform.localPosition = Vector3.zero;
	}
}
