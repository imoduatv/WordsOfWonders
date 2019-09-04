using UnityEngine;

public class LineDrawer : MonoBehaviour
{
	public GameObject pointPrefab;

	public float gap;

	private Vector3 lastPos;

	private void Start()
	{
	}

	private void Update()
	{
		if (UnityEngine.Input.GetKeyDown(KeyCode.A))
		{
			clear();
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
		{
			spawnPoint();
		}
		if (UnityEngine.Input.GetKey(KeyCode.Space))
		{
			Vector3 a = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
			a.z = 0f;
			a -= lastPos;
			if (a.magnitude >= gap * 3f)
			{
				spawnPoint();
			}
		}
	}

	private void spawnPoint()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(pointPrefab);
		gameObject.transform.SetParent(base.transform);
		gameObject.transform.localScale = Vector3.one;
		Vector3 position = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
		position.z = 0f;
		gameObject.transform.position = position;
		lastPos = gameObject.transform.position;
	}

	private void clear()
	{
		for (int i = 0; i < base.transform.childCount; i++)
		{
			UnityEngine.Object.Destroy(base.transform.GetChild(i).gameObject);
		}
	}
}
