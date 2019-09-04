using UnityEngine;

public class ObjectHolder : MonoBehaviour
{
	public static ObjectHolder instance;

	public new GameObject name;

	public GameObject starButton;

	public GameObject hintButton;

	public GameObject backButton;

	public GameObject sectionsButton;

	public GameObject shopButton;

	public GameObject shuffleButton;

	public GameObject logo;

	public GameObject sun;

	public GameObject map;

	public GameObject buttonHolder;

	public GameObject starHolder;

	public GameObject endGame;

	public GameObject wheel;

	public GameObject board;

	public GameObject dailyStarPrefab;

	public GameObject botHolder;

	private void Awake()
	{
		instance = this;
	}
}
