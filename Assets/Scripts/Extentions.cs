using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Extentions
{
	public static string Reverse(this string s)
	{
		char[] array = s.ToCharArray();
		Array.Reverse(array);
		return new string(array);
	}

	public static string AddSpaces(this string text)
	{
		string text2 = string.Empty;
		bool flag = false;
		foreach (char c in text)
		{
			switch (c)
			{
			case '<':
				flag = true;
				break;
			case '>':
				flag = false;
				break;
			}
			text2 += c;
			if (!flag && c != '>')
			{
				text2 += "\u00a0";
			}
		}
		return text2;
	}

	public static void ResetTransform(this Transform transform)
	{
		transform.localPosition = Vector3.zero;
		transform.localScale = Vector3.one;
		transform.localRotation = Quaternion.identity;
	}

	public static T[] GetAllComponents<T>(this Transform transform)
	{
		List<Transform> allChilds = transform.GetAllChilds();
		List<T> list = new List<T>();
		foreach (Transform item in allChilds)
		{
			T component = item.GetComponent<T>();
			if (component != null)
			{
				list.Add(item.GetComponent<T>());
			}
		}
		T[] array = new T[list.Count];
		for (int i = 0; i < list.Count; i++)
		{
			array[i] = list[i];
		}
		return array;
	}

	public static List<Transform> GetAllChilds(this Transform transform)
	{
		List<Transform> list = new List<Transform>();
		for (int i = 0; i < transform.childCount; i++)
		{
			list.Add(transform.GetChild(i));
		}
		List<Transform> list2 = new List<Transform>();
		foreach (Transform item in list)
		{
			list2 = list2.Union(item.GetAllChilds()).ToList();
		}
		return list.Union(list2).ToList();
	}
}
