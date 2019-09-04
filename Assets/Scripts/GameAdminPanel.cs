using UnityEngine;

public class GameAdminPanel : MonoBehaviour
{
	public GameObject finishGame;

	private void Start()
	{
		finishGame.SetActive(PlayerPrefsManager.GetHiddenMenu());
	}
}
