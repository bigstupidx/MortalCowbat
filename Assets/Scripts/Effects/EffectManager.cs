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

	public Effect CreateEffect(EffectDescriptor descr, Transform container, GameObject onWhat)
	{
		var effectGo = Instantiate(descr.Effect.gameObject);

		if (descr.InWorldSpace) {
			effectGo.transform.position = container.position;
		} else {
 			effectGo.transform.SetParent(container);
			effectGo.transform.localPosition = Vector3.zero;
		}

		effectGo.transform.localScale = new Vector3(1, 1,1);
	
		int offset = 0;
		var sprRen = onWhat.GetComponentInChildren<SpriteRenderer>();
		if (sprRen != null) {
			offset = sprRen.sortingOrder + 1;
		}
		Utils.OffsetSortingOrder(effectGo, offset);

		return effectGo.GetComponent<Effect>();
	}
}

