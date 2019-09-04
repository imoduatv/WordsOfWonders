using UnityEngine;

public class NarrowScreenScript : MonoBehaviour
{
	public bool onlyWidth;

	private void Start()
	{
		if (SectionController.ratio < 1f && !onlyWidth)
		{
			base.transform.localScale = Vector3.one * SectionController.ratio;
		}
		else if (onlyWidth)
		{
			Vector3 one = Vector3.one;
			one.x = SectionController.ratio;
			base.transform.localScale = one;
		}
	}
}
