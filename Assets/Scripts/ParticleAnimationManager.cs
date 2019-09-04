using UnityEngine;

public class ParticleAnimationManager : MonoBehaviour
{
	public static ParticleAnimationManager instance;

	public GameObject particleSystem;

	private GameObject[] particles;

	private void Start()
	{
		instance = this;
		Transform transform = UnityEngine.Object.Instantiate(particleSystem, GameObject.Find("Canvas").transform).transform;
		transform.SetAsLastSibling();
		transform.localScale = Vector3.one;
		transform.localPosition = Vector3.zero;
		particles = new GameObject[transform.childCount];
		for (int i = 0; i < transform.childCount; i++)
		{
			particles[i] = transform.GetChild(i).gameObject;
		}
	}

	public void PlayAnimation(LevelSet set)
	{
	}

	private void SetProperties(GameObject particle, LevelSet set)
	{
	}

	private void SetYPosition(GameObject go, int coor)
	{
		Vector3 localPosition = go.transform.localPosition;
		localPosition.y = coor;
		ParticleSystem.MainModule main = go.GetComponent<ParticleSystem>().main;
		go.transform.localPosition = localPosition;
	}

	private void SetXPosition(GameObject go, int coor)
	{
		Vector3 localPosition = go.transform.localPosition;
		localPosition.x = coor;
		ParticleSystem.MainModule main = go.GetComponent<ParticleSystem>().main;
		go.transform.localPosition = localPosition;
	}

	private void RotateAnimation(GameObject go, string side)
	{
		Vector3 localPosition = go.transform.localPosition;
		if (side.ToLower() == "left")
		{
			if (localPosition.x > 0f)
			{
				localPosition.x = -1f * localPosition.x;
			}
			go.transform.localRotation = Quaternion.Euler(180f, 0f, 0f);
			if (go.name.ToLower().Contains("gull") || go.name.ToLower().Contains("bird"))
			{
                //go.GetComponent<ParticleSystem>().textureSheetAnimation.flipV = 1f;
                ParticleSystem.TextureSheetAnimationModule module = go.GetComponent<ParticleSystem>().textureSheetAnimation;
                module.flipV = 1;
            }
			if (go.name.ToLower().Contains("fog") || go.name.ToLower().Contains("cloud") || go.name.ToLower().Contains("sand"))
			{
				go.transform.localRotation = Quaternion.Euler(0f, 105f, 0f);
			}
			go.transform.localPosition = localPosition;
		}
		else if (side == "right")
		{
			if (localPosition.x < 0f)
			{
				localPosition.x = -1f * localPosition.x;
			}
			go.transform.localRotation = Quaternion.Euler(180f, 0f, 180f);
			if (go.name.ToLower().Contains("gull") || go.name.ToLower().Contains("bird"))
			{
                //go.GetComponent<ParticleSystem>().textureSheetAnimation.flipV = 0f;
                ParticleSystem.TextureSheetAnimationModule module = go.GetComponent<ParticleSystem>().textureSheetAnimation;
                module.flipV = 0;
            }
			if (go.name.ToLower().Contains("fog") || go.name.ToLower().Contains("cloud") || go.name.ToLower().Contains("sand"))
			{
				go.transform.localRotation = Quaternion.Euler(180f, 80f, 0f);
			}
			go.transform.localPosition = localPosition;
		}
	}

	public void StopAnimations()
	{
		GameObject[] array = particles;
		foreach (GameObject gameObject in array)
		{
			gameObject.SetActive(value: false);
		}
	}
}
