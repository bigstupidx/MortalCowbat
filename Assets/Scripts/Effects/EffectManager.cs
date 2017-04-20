using UnityEngine;
using System.Collections.Generic;

public class EffectManager : MonoBehaviour
{

	List<Effect> runningEffects;

	void Awake()
	{
		runningEffects = new List<Effect>();
	}

	public Effect CreateEffect(GameObject prefab)
	{
		var effectGo = Instantiate(prefab);
		effectGo.transform.localPosition = Vector3.zero;
		//effectGo.transform.localScale = Vector3.one;
		var effect = effectGo.GetComponent<Effect>();

		runningEffects.Add(effect);
		effect.FinishAction += OnEffectFinished;

		return effect;
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

		//effectGo.transform.localScale = new Vector3(1, 1,1);
	
		int offset = 0;
		var sprRen = onWhat.GetComponentInChildren<SpriteRenderer>();
		if (sprRen != null) {
			offset = sprRen.sortingOrder + 1;
		}
		Utils.OffsetSortingOrder(effectGo, offset);

		var effect = effectGo.GetComponent<Effect>();

		runningEffects.Add(effect);
		effect.FinishAction += OnEffectFinished;

		return effect;
	}


	public void StopEffect(Effect effect)
	{
		effect.OnEvent("finished");
	}

	public void StopEffect(string id)
	{
		var effect = runningEffects.Find(x=>x.Id.Equals(id));
		if (effect != null) {
			effect.OnEvent("finished");
		}
	}

	void OnEffectFinished(Effect effect)
	{
		runningEffects.Remove(effect);
	}

}

