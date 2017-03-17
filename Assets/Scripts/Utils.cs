using System;
using UnityEngine;
using System.Linq;


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
}

