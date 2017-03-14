using System;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
	public Effect CreateEffect(GameObject prefab)
	{
		var effectGo = Instantiate(prefab);
		effectGo.transform.localPosition = Vector3.zero;
		effectGo.transform.localScale = Vector3.one;

		return effectGo.GetComponent<Effect>();
	}

	public Effect CreateEffect(GameObject prefab, Transform container)
	{
		var effectGo = Instantiate(prefab);
		effectGo.transform.SetParent(container);
		effectGo.transform.localPosition = Vector3.zero;
		effectGo.transform.localScale = Vector3.one;
	
		int offset = 0;
		var sprRen = Utils.FindParentComponent<SpriteRenderer>(container.gameObject);
		if (sprRen != null) {
			offset = sprRen.sortingOrder + 1;
		}
		Utils.OffsetSortingOrder(effectGo, offset);

		return effectGo.GetComponent<Effect>();
	}
}

