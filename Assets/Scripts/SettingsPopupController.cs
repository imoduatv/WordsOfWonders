using UnityEngine;

public class SettingsPopupController : MonoBehaviour
{
	public Animator menuSwapAnimator;

	public void SupportButtonShowMenuClick()
	{
		menuSwapAnimator.enabled = true;
		menuSwapAnimator.Play("MenuShowAnimation2");
	}
}
