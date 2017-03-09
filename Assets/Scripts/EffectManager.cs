using System;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
	public Effect CreateEffect(GameObject prefab, Transform container)
	{
		var effectGo = Instantiate(prefab);
		effectGo.transform.SetParent(container);
		effectGo.transform.localPosition = Vector3.zero;
		effectGo.transform.localScale = Vector3.one;
		return effectGo.GetComponent<Effect>();
	}
}

