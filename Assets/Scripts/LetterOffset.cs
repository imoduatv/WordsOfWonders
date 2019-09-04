using UnityEngine;

public class LetterOffset
{
	public string letter;

	public Vector2 cellOffset;

	public Vector2 letterOffset;

	public Vector2 letterMin;

	public Vector2 letterMax;

	public Vector2 cellMin;

	public Vector2 cellMax;

	public void calculateOffsets()
	{
		letterMin = new Vector2(letterOffset.x, -1f * letterOffset.y);
		letterMax = new Vector2(-1f * letterOffset.x, letterOffset.y);
		cellMin = new Vector2(cellOffset.x, -1f * cellOffset.y);
		cellMax = new Vector2(-1f * cellOffset.x, cellOffset.y);
	}
}
