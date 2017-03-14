using System;
using UnityEngine;
using System.Collections;

public class HitBlink : Effect
{
	[SerializeField]
	float duration;

	[SerializeField]
	Color color;

	public override void Run(GameObject go)
	{
		StartCoroutine(PlayCoroutine(go));
	}

	IEnumerator PlayCoroutine(GameObject go)
	{
		Utils.SetColor(go, color);
		yield return new WaitForSeconds(duration);
		Utils.SetColor(go, Color.white);

		OnEvent("finished");
	}
}

