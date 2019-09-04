using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FugoUtils
{
	public static float defaultTimescale = 0.02f;

	public static int extraPerCoin = 10;

	public static int boardHeight = 555;

	public static int adBoardHeight = 617;

	private static Dictionary<string, Color> colorCache;

	public static Color Color(int r, int g, int b)
	{
		float num = r;
		float num2 = g;
		float num3 = b;
		return new Color(num / 255f, num2 / 255f, num3 / 255f);
	}

	public static void ChangeAlpha(Text txt, float alpha)
	{
		Color color = txt.color;
		color.a = alpha;
		txt.color = color;
	}

	public static void ChangeAlpha(Image image, float alpha)
	{
		Color color = image.color;
		color.a = alpha;
		image.color = color;
	}

	public static void ChangeAlpha(SpriteRenderer image, float alpha)
	{
		Color color = image.color;
		color.a = alpha;
		image.color = color;
	}

	public static void openScene(string name)
	{
		SceneManager.LoadScene(name);
	}

	public static IEnumerator FillImage(float aValue, float aTime, Image go)
	{
		float oldfill = go.fillAmount;
		for (float t = 0f; t <= 1f; t += Time.deltaTime / aTime)
		{
			float newfill = go.fillAmount = Mathf.Lerp(oldfill, aValue, t);
			yield return null;
		}
		go.fillAmount = aValue;
	}

	public static IEnumerator FadeImage(float aValue, float aTime, Image go)
	{
		Color newColor = default(Color);
		Color color = go.color;
		float alpha = color.a;
		for (float t = 0f; t <= 1f; t += Time.deltaTime / aTime)
		{
			newColor = go.color;
			newColor.a = Mathf.Lerp(alpha, aValue, t);
			go.color = newColor;
			yield return null;
		}
		newColor.a = aValue;
		go.color = newColor;
	}

	public static IEnumerator Blend(Color color, float aTime, Image go)
	{
		Color oldColor = go.color;
		Color color2 = go.color;
		float a = color2.a;
		for (float t = 0f; t <= 1f; t += Time.deltaTime / aTime)
		{
			Color newColor = go.color = Color32.Lerp(oldColor, color, t);
			yield return null;
		}
		go.color = color;
	}

	public static IEnumerator Blend(Color color, float aTime, Text go)
	{
		Color oldColor = go.color;
		Color color2 = go.color;
		float a = color2.a;
		for (float t = 0f; t <= 1f; t += Time.deltaTime / aTime)
		{
			Color newColor = go.color = Color32.Lerp(oldColor, color, t);
			yield return null;
		}
		go.color = color;
	}

	public static IEnumerator FadeText(float aValue, float aTime, Text go)
	{
		Color newColor = default(Color);
		Color color = go.color;
		float alpha = color.a;
		for (float t = 0f; t <= 1f; t += Time.deltaTime / aTime)
		{
			newColor = go.color;
			newColor.a = Mathf.Lerp(alpha, aValue, t);
			go.color = newColor;
			yield return null;
		}
		newColor.a = aValue;
		go.color = newColor;
	}

	public static IEnumerator Scaler(Vector3 scale, float aTime, Transform go)
	{
		Vector3 oldScale = go.transform.localScale;
		for (float t = 0f; t <= 1f; t += Time.deltaTime / aTime)
		{
			Vector3 newScale = Vector3.Lerp(oldScale, scale, t);
			go.transform.localScale = newScale;
			yield return null;
		}
		go.transform.localScale = scale;
	}

	public static IEnumerator Sizer(float aTime, Transform go)
	{
		Vector2 newSize = go.GetComponent<RectTransform>().sizeDelta;
		Vector2 oldSize = new Vector2(0f, newSize.y);
		for (float t = 0f; t <= 1f; t += Time.deltaTime / aTime)
		{
			go.GetComponent<RectTransform>().sizeDelta = Vector2.Lerp(oldSize, newSize, t);
			yield return null;
		}
		go.GetComponent<RectTransform>().sizeDelta = newSize;
	}

	public static Color[] GetGradient(Color c1, Color c2, int count)
	{
		Color[] array = new Color[count];
		for (int i = 0; i < count; i++)
		{
			array[i] = Color32.Lerp(c1, c2, (float)i / (float)count);
		}
		return array;
	}

	public static Color HexToColor(string hex)
	{
		if (colorCache == null)
		{
			colorCache = new Dictionary<string, Color>();
		}
		if (colorCache.ContainsKey(hex))
		{
			return colorCache[hex];
		}
		int num = int.Parse(hex.Substring(0, 2), NumberStyles.AllowHexSpecifier);
		int num2 = int.Parse(hex.Substring(2, 2), NumberStyles.AllowHexSpecifier);
		int num3 = int.Parse(hex.Substring(4, 2), NumberStyles.AllowHexSpecifier);
		int num4 = 255;
		if (hex.Length > 6)
		{
			num4 = int.Parse(hex.Substring(6, 2), NumberStyles.AllowHexSpecifier);
		}
		Color color = new Color((float)num / 255f, (float)num2 / 255f, (float)num3 / 255f, (float)num4 / 255f);
		colorCache.Add(hex, color);
		return color;
	}

	public static string MyEscapeURL(string url)
	{
		return WWW.EscapeURL(url).Replace("+", "%20");
	}

	public static void ShuffleArray<T>(T[] arr)
	{
		for (int num = arr.Length - 1; num > 0; num--)
		{
			int num2 = UnityEngine.Random.Range(0, num + 1);
			T val = arr[num];
			arr[num] = arr[num2];
			arr[num2] = val;
		}
	}

	public static IEnumerator Mover(Vector3 pos, float aTime, Transform go)
	{
		Vector3 oldPos = go.transform.localPosition;
		for (float t = 0f; t <= 1f; t += Time.deltaTime / aTime)
		{
			Vector3 newPos = Vector3.Lerp(oldPos, pos, t);
			go.transform.localPosition = newPos;
			yield return null;
		}
		go.transform.localPosition = pos;
	}

	public static IEnumerator MoverWorldPos(Vector3 pos, float aTime, Transform go)
	{
		Vector3 oldPos = go.transform.position;
		for (float t = 0f; t <= 1f; t += Time.deltaTime / aTime)
		{
			float tt = Mathf.Sin(t * (float)Math.PI * 0.5f);
			Vector3 newPos = Vector3.Lerp(oldPos, pos, tt);
			go.transform.position = newPos;
			yield return null;
		}
		go.transform.position = pos;
	}

	public static IEnumerator CurveMover(Vector3 pos, float aTime, Transform go)
	{
		if (go != null)
		{
			Vector3 oldPos = go.transform.position;
			for (float t = 0f; t <= 1f; t += Time.deltaTime / aTime)
			{
				float yCoor = Mathf.Lerp(oldPos.y, pos.y, Mathf.Sin(t * t * (float)Math.PI * 0.5f));
				float xCoor = Mathf.Lerp(oldPos.x, pos.x, 1f - Mathf.Cos(t * t * (float)Math.PI * 0.5f));
				go.position = new Vector3(xCoor, yCoor, 0f);
				Vector3 pos2 = go.localPosition;
				pos2.z = 0f;
				go.localPosition = pos2;
				yield return null;
			}
		}
		else
		{
			yield return null;
		}
	}

	public static IEnumerator CurveMoverLeft(Vector3 pos, float aTime, Transform go)
	{
		if (go != null)
		{
			Vector3 oldPos = go.transform.position;
			for (float t = 0f; t <= 1f; t += Time.deltaTime / aTime)
			{
				float yCoor = Mathf.Lerp(oldPos.y, pos.y, 1f - Mathf.Cos(t * t * (float)Math.PI * 0.5f));
				float xCoor = Mathf.Lerp(oldPos.x, pos.x, Mathf.Sin(t * t * (float)Math.PI * 0.5f));
				go.position = new Vector3(xCoor, yCoor, 0f);
				Vector3 pos2 = go.localPosition;
				pos2.z = 0f;
				go.localPosition = pos2;
				yield return null;
			}
		}
		else
		{
			yield return null;
		}
	}

	public static IEnumerator Rotator(float rotation, float aTime, Transform go)
	{
		Vector3 eulerAngles = go.localRotation.eulerAngles;
		float oldPos = eulerAngles.z;
		for (float t = 0f; t <= 1f; t += Time.deltaTime / aTime)
		{
			float newPos = Mathf.Lerp(oldPos, rotation, t);
			go.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 1f) * newPos);
			yield return null;
		}
		go.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 1f) * rotation);
	}

	public static IEnumerator RotatorReverse(float rotation, float aTime, Transform go)
	{
		Vector3 eulerAngles = go.localRotation.eulerAngles;
		float oldPos = (360f - eulerAngles.z) * -1f;
		for (float t = 0f; t <= 1f; t += Time.deltaTime / aTime)
		{
			float newPos = Mathf.Lerp(oldPos, rotation, t);
			go.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 1f) * newPos);
			yield return null;
		}
		go.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 1f) * rotation);
	}

	public static int[] getLevelInfo()
	{
		int level = PlayerPrefsManager.GetLevel();
		int num = 1;
		for (int i = 0; i < Games.sections.Count; i++)
		{
			for (int j = 0; j < Games.sections[i].sets.Count; j++)
			{
				for (int k = 0; k < Games.sections[i].sets[j].levels.Count; k++)
				{
					Level level2 = Games.sections[i].sets[j].levels[k];
					if (num == level)
					{
						return new int[3]
						{
							i,
							j,
							k
						};
					}
					num++;
				}
			}
		}
		return null;
	}

	public static int[] getLevelInfo(int levelNumber)
	{
		int num = 1;
		for (int i = 0; i < Games.sections.Count; i++)
		{
			for (int j = 0; j < Games.sections[i].sets.Count; j++)
			{
				for (int k = 0; k < Games.sections[i].sets[j].levels.Count; k++)
				{
					Level level = Games.sections[i].sets[j].levels[k];
					if (num == levelNumber)
					{
						return new int[3]
						{
							i,
							j,
							k
						};
					}
					num++;
				}
			}
		}
		return null;
	}

	public static int GetLevelCount(Section s)
	{
		int num = 0;
		foreach (LevelSet set in s.sets)
		{
			num += set.levels.Count;
		}
		return num;
	}

	public static int GetFirstLevelOfNextSet()
	{
		int num = getLevelInfo()[0];
		int num2 = getLevelInfo()[1];
		int num3 = 0;
		for (int i = 0; i < num; i++)
		{
			foreach (LevelSet set in Games.sections[i].sets)
			{
				num3 += set.levels.Count;
			}
		}
		for (int j = 0; j < Games.sections[num].sets.Count; j++)
		{
			if (j <= num2)
			{
				num3 += Games.sections[num].sets[j].levels.Count;
			}
		}
		return num3 + 1;
	}

	public static int CalculateLengthOfMessage(string message, Text text)
	{
		int num = 0;
		text.font.RequestCharactersInTexture(message);
		foreach (char ch in message)
		{
			if (text.font.GetCharacterInfo(ch, out CharacterInfo info))
			{
				num += info.maxX;
			}
		}
		return num;
	}

	public static Sprite BlurImage(Sprite original)
	{
		int num = 1;
		Texture2D texture = original.texture;
		for (int i = 0; i < texture.width; i++)
		{
			for (int j = 0; j < texture.height; j++)
			{
				float num2 = 0f;
				float num3 = 0f;
				float num4 = 0f;
				for (int k = -num; k < num; k++)
				{
					for (int l = -num; l < num; l++)
					{
						try
						{
							Color pixel = texture.GetPixel(i + k, j + l);
							num2 += pixel.r;
							num3 += pixel.g;
							num4 += pixel.b;
						}
						catch (Exception)
						{
						}
					}
				}
				Color color = new Color(1f, 1f, 1f);
				float num5 = (float)(num * num) * 4f;
				color.r = num2 / num5;
				color.g = num3 / num5;
				color.b = num4 / num5;
				texture.SetPixel(i, j, color);
			}
		}
		UnityEngine.Debug.Log("blur done");
		return Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100f);
	}

	public static float getBoardRatio()
	{
		float num = (PlayerPrefsManager.GetNoAd() != 0) ? ((float)adBoardHeight) : ((float)boardHeight);
		float num2 = 1336f / (float)Screen.height;
		float num3 = (float)Screen.width * num2;
		return num3 / num;
	}

	public static string getGameIndex()
	{
		return string.Empty;
	}

	public static string DateFormatterLong(string s, TimeSpan ts)
	{
		string text = s.Replace("%@", ((int)ts.TotalDays).ToString()).Replace("@%", ts.Hours.ToString()).Replace("#@", ts.Minutes.ToString());
		string[] array = text.Split('-');
		if (ts.TotalDays > 1.0)
		{
			return array[0] + " " + array[1];
		}
		return array[1] + " " + array[2];
	}

	public static string DateFormatter(string s, TimeSpan ts)
	{
		string text = s.Replace("%@", ((int)ts.TotalDays).ToString()).Replace("@%", ts.Hours.ToString()).Replace("#@", ts.Minutes.ToString());
		string[] array = text.Split(' ');
		if (ts.TotalDays > 1.0)
		{
			return array[0] + " " + array[1];
		}
		return array[1] + " " + array[2];
	}

	public static string DateFormatterNewTheme(string s, TimeSpan ts)
	{
		return ((int)ts.TotalHours).ToString("D2") + ":" + ts.Minutes.ToString("D2");
	}

	public static List<int> GetHiddenLevelsInSet(LevelSet set)
	{
		int num = -1;
		List<int> list = new List<int>();
		foreach (Level level in set.levels)
		{
			if (num != int.Parse(level.gameID))
			{
				num = int.Parse(level.gameID);
				list.Add(num);
			}
		}
		List<int> list2 = new List<int>();
		for (int i = 0; i < list.Count; i++)
		{
			list2.Add(0);
			foreach (Level level2 in set.levels)
			{
				if (int.Parse(level2.gameID) == list[i])
				{
					List<int> list3;
					int index;
					(list3 = list2)[index = i] = list3[index] + 1;
				}
			}
		}
		return list2;
	}

	public static Color ChangeBrightness(Color c, float ratio)
	{
		c.r = Mathf.Clamp01(c.r * ratio);
		c.g = Mathf.Clamp01(c.g * ratio);
		c.b = Mathf.Clamp01(c.b * ratio);
		return c;
	}

	public static bool IsAdventureCompleted(string num)
	{
		int num2 = 0;
		foreach (Section section in Games.sections)
		{
			foreach (LevelSet hiddenset in section.hiddensets)
			{
				if (hiddenset.SetID == num)
				{
					num2 = hiddenset.levels.Count;
				}
			}
		}
		if (PlayerPrefsManager.GetHiddenLevel(int.Parse(num)) > num2)
		{
			return true;
		}
		return false;
	}

	public static int GetHiddenSetSectionIndex(int setid)
	{
		for (int i = 0; i < Games.sections.Count; i++)
		{
			foreach (LevelSet hiddenset in Games.sections[i].hiddensets)
			{
				if (hiddenset.SetID == setid.ToString())
				{
					return i;
				}
			}
		}
		return -1;
	}

	public static bool IsIphoneXorSomethingLikeThat()
	{
		return false;
	}

	public static IEnumerator SetSameTextSize(List<Text> list, Canvas canvas)
	{
		yield return null;
		yield return null;
		int min = 9999;
		foreach (Text item in list)
		{
			if (item.cachedTextGenerator.fontSizeUsedForBestFit < min)
			{
				min = item.cachedTextGenerator.fontSizeUsedForBestFit;
			}
			item.resizeTextMaxSize = 1000;
		}
		yield return null;
		float multiplier = 1f / canvas.scaleFactor;
		foreach (Text item2 in list)
		{
			item2.resizeTextMaxSize = (int)Mathf.Floor((float)min * multiplier) + 1;
		}
	}
}
