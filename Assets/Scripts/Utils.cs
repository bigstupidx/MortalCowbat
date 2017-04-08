﻿using System;
using UnityEngine;
using System.Linq;
using System.Collections;


public static class Utils
{
	public static T FindParentComponent<T>(GameObject go) where T: UnityEngine.Object
	{
		Transform parent = go.transform.parent;
		while (parent != null) {
			var comp = parent.gameObject.GetComponent<T>();
			if (comp != null) {
				return comp;
			}
			parent = parent.parent;
		}
		return null;
	}

	public static void OffsetSortingOrder(GameObject go, int offset)
	{
		Array.ForEach(go.GetComponentsInChildren<SpriteRenderer>(true), x=>x.sortingOrder += offset);
	}

	public static int GetMaxSortingOrder(GameObject go)
	{
		return go.GetComponentsInChildren<SpriteRenderer>(true).Max(x=> x.sortingOrder);
	}

	public static void SetColor(GameObject go, Color color)
	{
		var renderers = go.GetComponentsInChildren<SpriteRenderer>(true);
		Array.ForEach(renderers, x=>x.color = color);
	}

	public static bool GetRandomBool()
	{
		return UnityEngine.Random.Range(0,2) == 0;
	}

	public static float Ease (float c)
	{
		float sqx = c * c;
		return sqx / (2.0f * (sqx - c) + 1.0f);
	}

	public static IEnumerator LerpWithEase(float startValue, float endValue, float duration, Action<float> onUpdate, Func<float, float> easing)
	{
		float startTime = Time.time;
		while (true) {
			float c = Math.Min( (Time.time - startTime) / duration, 1.0f);

			if (easing != null) {
				c = easing (c);
			}
			var lerp = Mathf.Lerp(startValue, endValue, c);
			onUpdate(lerp);

			if (Mathf.Abs(c - 1) < float.Epsilon)
				break;
			yield return 0;
		}
	}
}

