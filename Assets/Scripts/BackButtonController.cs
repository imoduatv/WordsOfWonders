using UnityEngine;

public class BackButtonController : MonoBehaviour
{
	public delegate void OnBackClicked();

	public static BackButtonController instance;

	public OnBackClicked onBackClicked;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		setDefaultAction();
	}

	private void Update()
	{
		if (UnityEngine.Input.GetKeyUp(KeyCode.Escape))
		{
			backClicked();
		}
	}

	private void backClicked()
	{
		if (onBackClicked != null)
		{
			onBackClicked();
		}
		setDefaultAction();
	}

	public void setDefaultAction()
	{
		onBackClicked = GameMenuController.instance.onBackClicked;
	}
}
